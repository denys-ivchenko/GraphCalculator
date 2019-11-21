using System;

namespace Telesyk.GraphCalculator.Internal
{
	public sealed class Result
	{
		internal Result(string combination, int value, int[] combinationValues)
		{
			Combination = combination;
			Value = value;
			CombinationValues = combinationValues;
		}

		public string Combination { get; }

		public int[] CombinationValues { get; }

		public decimal Value { get; }

		public decimal MaximalFunctionResult { get; internal set; }
	}
}
