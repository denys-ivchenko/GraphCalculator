using System;

/// Internal ^
using Telesyk.GraphCalculator.Internal;

namespace Telesyk.GraphCalculator
{
	public class Program
	{
		static void Main(string[] args)
		{
			Console.Title = Settings.StringTitle;

			Proccesor.Procces();
		} 
	}
}
