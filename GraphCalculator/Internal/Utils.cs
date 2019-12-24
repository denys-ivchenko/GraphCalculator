using System;
using System.Collections.Generic;

namespace Telesyk.GraphCalculator.Internal
{
	public static class Utils
	{
		#region Read Integers

		public static int ReadInteger(Writer writer, string message)
		{
			return ReadIntegers(writer, message, 1, true)[0];
		}

		public static int[] ReadIntegers(Writer writer, string message)
		{
			return ReadIntegers(writer, message, -1, true);
		}

		public static int[] ReadIntegers(Writer writer, string message, int length)
		{
			return ReadIntegers(writer, message, length, false);
		}

		public static int[] ReadIntegers(Writer writer, string message, int length, bool writeNewLineForException)
		{
			bool result = false;
			int[] values = new int[0];

			while (!result)
			{
				writer.Write($"{message}: ");

				string input = Console.ReadLine();
				writer.WriteFile(input);

				if (string.IsNullOrWhiteSpace(input))
				{
					writer.WriteLine(Settings.StringWrongData);

					if (writeNewLineForException)
						writer.WriteLine();

					continue;
				}

				string[] inputs = input.Split(' ', ',');

				List<int> items = new List<int>();

				try
				{
					for (int i = 0; i < inputs.Length; i++)
						try
						{
							string value = inputs[i].Trim();

							if (value != String.Empty)
								items.Add(int.Parse(value));
						}
						catch
						{
							writer.WriteLine(Settings.StringWrongData);

							if (writeNewLineForException)
								writer.WriteLine();

							throw;
						}
				}
				catch
				{ continue; }

				if (length > -1 && items.Count != length)
				{
					writer.WriteLine(Settings.StringWrongData);

					if (writeNewLineForException)
						writer.WriteLine();

					continue;
				}

				values = items.ToArray();
				result = true;
			}

			writer.WriteLine();
			return values;
		}

		#endregion

		#region Read Boolean

		public static bool ReadBoolean(Writer writer, string message)
		{
			bool result = false;
			bool value = false;

			while (!result)
			{
				writer.Write($"{message}: ");
				string input = Console.ReadLine().Trim();
				writer.WriteFile(input);

				try
				{
					if (!(input == "0" || input == "1"))
						value = bool.Parse(input);
					else
						value = Convert.ToBoolean(int.Parse(input));

					result = true;
				}
				catch 
				{
					writer.WriteLine(Settings.StringWrongData);
					writer.WriteLine();

					continue;
				}
			}

			writer.WriteLine();
			return value;
		}

		#endregion

		#region Read Function

		public static List<T> ReadFunctions<T>(Writer writer, int count, int valueCount, bool isFunctionsDenominators) where T : Function, new()
			=> _readFunctions<T>(writer, count, valueCount, null, isFunctionsDenominators);

		public static List<T> ReadFunctions<T>(Writer writer, int count, int valueCount, int[] values) where T : Function, new()
			=> _readFunctions<T>(writer, count, valueCount, values, false);

		private static List<T> _readFunctions<T>(Writer writer, int count, int valueCount, int[] values, bool isFunctionsDenominators) where T : Function, new()
		{
			List<T> functions = new List<T>();

			string functionString = Settings.StringFunction;

			if (typeof(T) == typeof(LimitationFunction))
				functionString = Settings.StringLimitationFunction;

			for (int i = 0; i < count; i++)
			{
				T function = new T();
				List<int> numbers = new List<int>();
				List<char> operators = new List<char>();
				
				_parseFunctionBySymbols(writer, string.Format(functionString, i + 1), i, valueCount, numbers, operators);

				if (Settings.IsMultiInputForValues)
					values = Utils.ReadIntegers(writer, Settings.StringFunctionInputValue.Replace("{0}", (i + 1).ToString()), valueCount, true);

				function.Multipliers = numbers.ToArray();
				function.Operators = operators.ToArray();

				if (function is LimitationFunction)
				{
					_parseLimitationByFunction(writer, i, function as LimitationFunction);
					(function as LimitationFunction).Values = values;
				}

				if (function is MaximalFunction && isFunctionsDenominators)
					(function as MaximalFunction).Denominators = Utils.ReadIntegers(writer, Settings.StringFunctionsDenominators.Replace("{0}", (i + 1).ToString()), valueCount, true); ;

				functions.Add(function);
			}

			return functions;
		}

		#endregion

		#region Get limited matches

		public static List<Result> GetLimitedMatches(List<LimitationFunction> limitationFunctions)
		{
			List<Result> matches = new List<Result>();
			LimitationFunction first = limitationFunctions[0];

			foreach (Result result in first.Results.Values)
			{
				bool match = true;

				for (int i = 1; i < limitationFunctions.Count; i++)
				{
					if (!limitationFunctions[i].Results.ContainsKey(result.Combination))
					{
						match = false;
						break;
					}
				}

				if (match)
					matches.Add(result);
			}

			return matches;
		}

		#endregion

		#region Get maximums

		public static List<Result> GetMaximums(Watcher watcher, List<Result> matches, List<MaximalFunction> functions, bool isFractionalFunctions, bool isFunctionsDenominators)
		{
			for (int i = 0; i < matches.Count; i++)
			{
				int combinationSum = 0;

				Result result = matches[i];

				foreach (MaximalFunction function in functions)
				{
					for (int j = 0; j < result.CombinationValues.Length; j++)
					{
						int multiplied = 0;

						if (Settings.IsCalculateTime)
						{
							watcher.Run(() => multiplied = result.CombinationValues[j] * function.Multipliers[j]);
							watcher.RunBag(() => combinationSum += result.CombinationValues[j]);

							if (isFunctionsDenominators)
								watcher.RunBag(() => combinationSum += result.CombinationValues[j] * function.Denominators[j]);
							else
								watcher.RunBag(() => combinationSum += result.CombinationValues[j]);
						}
						else
						{
							multiplied = result.CombinationValues[j] * function.Multipliers[j];
							combinationSum += result.CombinationValues[j];
						}

						if (j != 0)
							result.MaximalFunctionResult = _doOperator(watcher, result.MaximalFunctionResult, multiplied, function.Operators[j - 1]);
						else
							result.MaximalFunctionResult += multiplied;
					}

					result.MaximalFunctionResult = result.MaximalFunctionResult;
				}

				if (isFractionalFunctions)
					if (Settings.IsCalculateTime)
					{
						watcher.Run(() => result.MaximalFunctionResult /= combinationSum);
						watcher.MergeBug();
					}
					else
						result.MaximalFunctionResult /= combinationSum;
				else
					if (Settings.IsCalculateTime)
					{
						watcher.Run(() => result.MaximalFunctionResult /= functions.Count);
						watcher.ClearBag();
					}
					else
						result.MaximalFunctionResult /= functions.Count;
			}

			List<Result> maximums = new List<Result>();

			if (matches.Count > 0)
			{
				maximums.Add(matches[0]);

				for (int i = 1; i < matches.Count; i++)
				{
					if (matches[i].MaximalFunctionResult >= maximums[0].MaximalFunctionResult)
					{
						if (matches[i].MaximalFunctionResult > maximums[0].MaximalFunctionResult)
							maximums.Clear();

						maximums.Add(matches[i]);
					}
					else continue;
				}
			}

			return maximums;
		}

		#endregion

		#region Calculating limit range

		public static Dictionary<string, Result> CalculateLimitRange(Writer writer, Watcher watcher, LimitationFunction function)
		{
			Dictionary<string, Result> results = new Dictionary<string, Result>();
			List<int> list = new List<int>(function.Values);

			_getCombinations(writer, watcher, list, function, results, String.Empty);
			return results;
		}

		private static void _getCombinations(Writer writer, Watcher watcher, List<int> list, LimitationFunction function, Dictionary<string, Result> results, string current)
		{
			if (list.Count == 0 && !results.ContainsKey(current))
			{
				string[] stringValues = current.Split('.');
				int[] values = new int[stringValues.Length];

				for (int i = 0; i < stringValues.Length; i++)
					values[i] = int.Parse(stringValues[i]);

				int value = 0;

				for (int i = 0; i < values.Length; i++)
				{
					int multiplied = 0;

					if (Settings.IsCalculateTime)
						watcher.Run(() => multiplied = values[i] * function.Multipliers[i]);
					else
						multiplied = values[i] * function.Multipliers[i];

					if (i != 0)
						value = Convert.ToInt32(_doOperator(watcher, value, multiplied, function.Operators[i - 1]));
					else
						value += multiplied;
				}

				bool inLimit = false;

				switch (function.LimitExpression)
				{
					case LimitationExpression.Less:
						inLimit = (value < function.Limitation);
						break;
					case LimitationExpression.LessOrEqual:
						inLimit = (value <= function.Limitation);
						break;
					case LimitationExpression.GreaterOrEqual:
						inLimit = (value >= function.Limitation);
						break;
					case LimitationExpression.Greater:
						inLimit = (value > function.Limitation);
						break;
				}

				if (inLimit && !results.ContainsKey(current))
				{
					Result result = new Result(current, value, values);
					results.Add(current, result);
				}
			}

			for (int i = 0; i < list.Count; i++)
			{
				List<int> next = new List<int>(list);
				next.RemoveAt(i);
				_getCombinations(writer, watcher, next, function, results, current + (current != string.Empty ? "." : string.Empty) + list[i].ToString());
			}
		}

		#endregion

		#region Parsing function by symbols

		private static void _parseFunctionBySymbols(Writer writer, string functionString, int index, int multipliersCount, List<int> numbers, List<char> operators)
		{
			bool result = false;
	 
			while (!result)
			{
				string message = functionString.Replace("{0}", (index + 1).ToString());
				writer.Write($"{message}: ");
				string input = Console.ReadLine();
				writer.WriteFile(input);

				try
				{
					char last = ' ';

					for (int j = 0; j < input.Length; j++)
					{
						char current = char.Parse(input.Substring(j, 1));

						if (current == ' ' || current == '\t')
							continue;

						bool lastIsNumber = _isNumber(last);
						bool lastIsOperator = _isOperator(last);

						bool currentIsNumber = _isNumber(current);
						bool currentIsOperator = _isOperator(current);

						if (!currentIsNumber && !currentIsOperator)
							throw new Exception(Settings.StringWrongData);
						else
						{
							if (j == input.Length - 1 && !currentIsNumber)
								throw new Exception(Settings.StringWrongData);

							if (currentIsNumber)
							{
								int currentNumber = int.Parse(current.ToString());

								if (!lastIsNumber)
									numbers.Add(currentNumber);
								else
									numbers[numbers.Count - 1] = numbers[numbers.Count - 1] * 10 + currentNumber;
							}
							else
							{
								if (lastIsOperator)
									throw new Exception(Settings.StringWrongData);
								else
									operators.Add(current);
							}

							last = current;
						}
					}

					if (numbers.Count != multipliersCount)
						throw new Exception(Settings.StringWrongData);
				}
				catch
				{
					numbers.Clear();
					operators.Clear();

					writer.WriteLine(Settings.StringWrongData);
					writer.WriteLine();

					continue;
				}

				writer.WriteLine();

				result = true;
			}
		}
		
		#endregion

		#region Parse limitation by function

		private static void _parseLimitationByFunction(Writer writer, int index, LimitationFunction function)
		{
			bool result = false;
			int limitation = 0;
			LimitationExpression expression = LimitationExpression.Less;
   
			while (!result)
			{
				string message = Settings.StringLimitation.Replace("{0}", (index + 1).ToString());
				writer.Write($"{message}: ");
				string input = Console.ReadLine();
				writer.WriteFile(input);

				try
				{
					input = input.Replace(" ", null);

					int expressionLength = 2;
					string expressionString = input.Substring(0, expressionLength);

					if (!(expressionString == "<=" || expressionString == "=<" || expressionString == ">=" || expressionString == "=>"))
					{
						expressionLength = 1;
						expressionString = input.Substring(0, expressionLength);

						if (expressionString == ">")
							expression = LimitationExpression.Greater;
					}
					else 
						if (expressionString == "<=" || expressionString == "=<")
							expression = LimitationExpression.LessOrEqual;
						else
							expression = LimitationExpression.GreaterOrEqual;

					limitation = int.Parse(input.Substring(expressionLength));
				}
				catch
				{
					writer.WriteLine(Settings.StringWrongData);
					writer.WriteLine();

					continue;
				}

				writer.WriteLine();
				result = true;
			}

			function.Limitation = limitation;
			function.LimitExpression = expression;
		}

		#endregion

		#region Util methods

		#region Do operator

		private static decimal _doOperator(Watcher watcher, decimal left, int right, char oper)
		{
			decimal value = 0;

			if (Settings.IsCalculateTime)
			{
				switch (oper)
				{
					case '+':
						watcher.Run(() => value = left + right);
						break;
					case '-':
						watcher.Run(() => value = left - right);
						break;
					case '*':
						watcher.Run(() => value = left * right);
						break;
					case '/':
						watcher.Run(() => value = left / right);
						break;
				}
			}
			else
			{
				switch (oper)
				{
					case '+':
						value = left + right;
						break;
					case '-':
						value = left - right;
						break;
					case '*':
						value = left * right;
						break;
					case '/':
						value = left / right;
						break;
				}
			}

			return value;
		}

		#endregion

		//private static bool _isNumber(char symbol) => (!(symbol < '0' && symbol > '9'));

		private static bool _isNumber(char symbol) => (symbol == '0' || symbol == '1' || symbol == '2' || symbol == '3' || symbol == '4' || symbol == '5' || symbol == '6' || symbol == '7' || symbol == '8' || symbol == '9');

		private static bool _isOperator(char symbol) => (symbol == '+' || symbol == '-' || symbol == '*' || symbol == '/');

		#endregion
	}
}
