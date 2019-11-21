using System;

namespace Telesyk.GraphCalculator.Internal
{
	public sealed class MaximalFunction : Function
	{
		public MaximalFunction()
		{

		}

		public int[] Denominators { get; internal set; }
	}
}