using System;
using System.Collections.Generic;

namespace Telesyk.GraphCalculator.Internal
{
	public static class Proccesor
	{
		public static void Procces()
		{
			using (Writer writer = new Writer())
				_procces(writer);
		}
  
		private static void _procces(Writer writer)
		{
			while (true)
			{
				Watcher watcher = new Watcher();
	
				bool isFractionalFunctions = false;
				bool isFunctionsDenominators = false;

				int valueCount = Utils.ReadInteger(writer, Settings.StringValueCount);

				int[] values = null;

				if (!Settings.IsMultiInputForValues)
					values = Utils.ReadIntegers(writer, Settings.StringInputValue, valueCount, true);

				int limitedFunctionCount = Utils.ReadInteger(writer, Settings.StringLimitationFunctionCount);
				List<LimitationFunction> limitationFunctions = Utils.ReadFunctions<LimitationFunction>(writer, limitedFunctionCount, valueCount, values);

				int functionCount = Utils.ReadInteger(writer, Settings.StringFunctionCount);

				if (functionCount > 0)
				{ 
					isFractionalFunctions = Utils.ReadBoolean(writer, Settings.StringAsFractionalFunctions);

					if (isFractionalFunctions)
						isFunctionsDenominators = Utils.ReadBoolean(writer, Settings.StringInputFunctionsDenominators);
				}

				List<MaximalFunction> maximalFunctions = Utils.ReadFunctions<MaximalFunction>(writer, functionCount, valueCount, isFunctionsDenominators);

				DateTime start = DateTime.Now;

				foreach (LimitationFunction function in limitationFunctions)
					function.Results = Utils.CalculateLimitRange(writer, watcher, function);

				#region Advanced mode: limitation functions results
				if (Settings.IsAdvancedMode)
				{
					for (int i = 0; i < limitationFunctions.Count; i++)
					{
						writer.WriteLine($"{string.Format(Settings.StringLimitationFunctionResults, i + 1)}:");

						foreach (Result result in limitationFunctions[i].Results.Values)
							writer.WriteLine($"{result.Combination}\t{result.Value}");

						writer.WriteLine($"{Settings.StringCount} : {limitationFunctions[i].Results.Count}");
						writer.WriteLine();
					}
				}
				#endregion

				List<Result> matches = Utils.GetLimitedMatches(limitationFunctions);

				#region Advanced mode: matches
				if (Settings.IsAdvancedMode)
				{
					writer.WriteLine($"{Settings.StringMatches}:");

					foreach (Result result in matches)
						writer.WriteLine($"{result.Combination}\t{result.Value}");

					writer.WriteLine($"{Settings.StringCount} : {matches.Count}");
					writer.WriteLine();
				}
				#endregion

				List<Result> maximums = Utils.GetMaximums(watcher, matches, maximalFunctions, isFractionalFunctions, isFunctionsDenominators);

				foreach (Result result in maximums)
					writer.WriteLine($"{Settings.StringResult}: {result.Combination}\t{result.Value}\t{result.MaximalFunctionResult}");

				writer.WriteLine();

				//writer.WriteLineAsync($"{Settings.CountString}: {maximums.Count}");

				TimeSpan executionTime = DateTime.Now - start;

				if (Settings.IsCalculateTime)
				{
					writer.WriteLine(string.Format(Settings.StringExecutionInfo, watcher.Operations, watcher.Interval));
					writer.WriteLine();
				}

				writer.WriteLine(string.Format(Settings.StringExecutionTime, executionTime));
				writer.WriteLine();

				writer.WriteLine(Settings.StringGoNewly);
				writer.WriteLine();
			}
		}
	}
}
