using System;
using System.Collections.Generic;

namespace Telesyk.GraphCalculator.Internal
{
	public class Watcher
	{
		public Watcher()
		{
			
		}

		public TimeSpan Interval { get; private set; } = TimeSpan.Zero;

		public int Operations { get; private set; } = 0;

		public TimeSpan BagInterval { get; private set; } = TimeSpan.Zero;

		public int BagOperations { get; private set; } = 0;

		public void Run(Action action)
		{
			DateTime start = DateTime.Now;

			action();

			Operations++;
			Interval += (DateTime.Now - start);
		}

		public void RunBag(Action action)
		{
			DateTime start = DateTime.Now;

			action();

			BagOperations++;
			BagInterval += (DateTime.Now - start);
		}

		public void Clear()
		{
			Operations = 0;
			Interval = TimeSpan.Zero;
		}

		public void ClearBag()
		{
			BagOperations = 0;
			BagInterval = TimeSpan.Zero;
		}

		public void MergeBug()
		{
			Operations += BagOperations;
			Interval += BagInterval;

			ClearBag();
		}
	}
}
