using System;
using System.IO;
using System.Text;

namespace Telesyk.GraphCalculator.Internal
{
	public class Writer : IDisposable
	{
		private StreamWriter _file;
		private bool _lastIsWriteLine;

		public Writer()
		{
			Encoding encoding;

			try { encoding = Encoding.GetEncoding(Settings.StringEncoding); }
			catch { encoding = Encoding.UTF8; }
   
			_file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "out.txt", false, encoding);
			_file.AutoFlush = true;

			Console.OutputEncoding = encoding;
		}

		public void Write(string message)
		{
			Console.Write(message);
			_file.Write(message);

			_lastIsWriteLine = false;
		}

		public void WriteFile(string message)
		{
			_file.Write(message);

			_lastIsWriteLine = false;
		}

		public void WriteLine(string message)
		{
			Console.WriteLine(message);
			_file.WriteLine(message);

			_lastIsWriteLine = true;
		}

		public void WriteLine()
		{
			Console.WriteLine();
			_file.WriteLine("");

			if (!_lastIsWriteLine)
				_file.WriteLine("");

			_lastIsWriteLine = true;
		}

		public void Dispose()
		{
			_file.Dispose();
		}
	}
}
