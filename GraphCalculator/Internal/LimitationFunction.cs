using System;
using System.Collections.Generic;

namespace Telesyk.GraphCalculator.Internal
{
	public sealed class LimitationFunction : Function
	{
		public LimitationFunction()
		{

		}

		public int Limitation { get; internal set; }

		public LimitationExpression LimitExpression { get; internal set; }

		public int[] Values { get; internal set; }

		internal Dictionary<string, Result> Results { get; set; }
	}
}
