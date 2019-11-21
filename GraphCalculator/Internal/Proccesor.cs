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

				int valueCount = Utils.ReadInteger(writer, Settings.ValueCountString);

				int[] values = null;

				if (!Settings.IsMultiInputForValues)
					values = Utils.ReadIntegers(writer, Settings.InputValueString, valueCount, true);

				int limitedFunctionCount = Utils.ReadInteger(writer, Settings.LimitationFunctionCountString);
				List<LimitationFunction> limitationFunctions = Utils.ReadFunctions<LimitationFunction>(writer, limitedFunctionCount, valueCount, values);

				int functionCount = Utils.ReadInteger(writer, Settings.FunctionCountString);

				if (functionCount > 0)
				{ 
					isFractionalFunctions = Utils.ReadBoolean(writer, Settings.AsFractionalFunctionsString);

					if (isFractionalFunctions)
						isFunctionsDenominators = Utils.ReadBoolean(writer, Settings.InputFunctionsDenominatorsString);
				}

				List<MaximalFunction> maximalFunctions = Utils.ReadFunctions<MaximalFunction>(writer, functionCount, valueCount, isFunctionsDenominators);

				DateTime start = DateTime.Now;

				foreach (LimitationFunction function in limitationFunctions)
					function.Results = Utils.CalculateLimitRange(watcher, function);

				#region Advanced mode: limitation functions results
				if (Settings.IsAdvancedMode)
				{
					for (int i = 0; i < limitationFunctions.Count; i++)
					{
						writer.WriteLine($"{string.Format(Settings.LimitationFunctionResultsString, i + 1)}:");

						foreach (Result result in limitationFunctions[i].Results.Values)
							writer.WriteLine($"{result.Combination}\t{result.Value}");

						writer.WriteLine($"{Settings.CountString} : {limitationFunctions[i].Results.Count}");
						writer.WriteLine();
					}
				}
				#endregion

				List<Result> matches = Utils.GetLimitedMatches(limitationFunctions);

				#region Advanced mode: matches
				if (Settings.IsAdvancedMode)
				{
					writer.WriteLine($"{Settings.MatchesString}:");

					foreach (Result result in matches)
						writer.WriteLine($"{result.Combination}\t{result.Value}");

					writer.WriteLine($"{Settings.CountString} : {matches.Count}");
					writer.WriteLine();
				}
				#endregion

				List<Result> maximums = Utils.GetMaximums(watcher, matches, maximalFunctions, isFractionalFunctions, isFunctionsDenominators);

				foreach (Result result in maximums)
					writer.WriteLine($"{Settings.ResultString}: {result.Combination}\t{result.Value}\t{result.MaximalFunctionResult}");

				writer.WriteLine();

				//writer.WriteLineAsync($"{Settings.CountString}: {maximums.Count}");

				TimeSpan executionTime = DateTime.Now - start;

				if (Settings.IsCalculateTime)
				{
					writer.WriteLine(string.Format(Settings.ExecutionInfoString, watcher.Operations, watcher.Interval));
					writer.WriteLine();
				}

				writer.WriteLine(string.Format(Settings.ExecutionTimeString, executionTime));
				writer.WriteLine();

				writer.WriteLine(Settings.GoNewlyString);
				writer.WriteLine();
			}
		}
	}
}
