using System;
using System.Configuration;

namespace Telesyk.GraphCalculator.Internal
{
	public sealed class Settings
	{
		#region Singleton members

		//private static Lazy<Settings> _instance = new Lazy<Settings>(() => new Settings());

		//public static Settings Instance
		//{
		//	get { return _instance.Value; }
		//}

		#endregion

		#region Initialization

		static Settings()
		{
			bool isAdvancedMode = false;
			bool.TryParse(ConfigurationManager.AppSettings["advanced-mode"], out isAdvancedMode);
			IsAdvancedMode = isAdvancedMode;

			bool isCalculateTime = false;
			bool.TryParse(ConfigurationManager.AppSettings["calculate-time"], out isCalculateTime);
			IsCalculateTime = isCalculateTime;

			bool isMultiInputForValues = false;
			bool.TryParse(ConfigurationManager.AppSettings["multi-input-for-values"], out isMultiInputForValues);
			IsMultiInputForValues = isMultiInputForValues;

			StringTitle = ConfigurationManager.AppSettings["title"];
			StringEncoding = ConfigurationManager.AppSettings["encoding"];
			StringWrongData = ConfigurationManager.AppSettings["wrong-data"];
			StringValueCount = ConfigurationManager.AppSettings["value-count"];
			StringInputValue = ConfigurationManager.AppSettings["input-values"];
			StringFunctionInputValue = ConfigurationManager.AppSettings["function-input-values"];
			StringLimitationFunctionCount = ConfigurationManager.AppSettings["limitation-function-count"];
			StringLimitationFunction = ConfigurationManager.AppSettings["limitation-function"];
			StringLimitation = ConfigurationManager.AppSettings["limitation"];
			StringFunctionMaximum = ConfigurationManager.AppSettings["function-maximum"];
			StringFunctionCount = ConfigurationManager.AppSettings["function-count"];
			StringFunction = ConfigurationManager.AppSettings["function"];
			StringAsFractionalFunctions = ConfigurationManager.AppSettings["as-fractional-functions"];
			StringInputFunctionsDenominators = ConfigurationManager.AppSettings["input-functions-denominators"];
			StringFunctionsDenominators = ConfigurationManager.AppSettings["functions-denominators"];
			StringResult = ConfigurationManager.AppSettings["result"];
			StringCount = ConfigurationManager.AppSettings["count"];
			StringLimitationFunctionResults = ConfigurationManager.AppSettings["limitation-function-results"];
			StringMatches = ConfigurationManager.AppSettings["matches"];
			StringExecutionInfo = ConfigurationManager.AppSettings["execution-info"];
			StringExecutionTime = ConfigurationManager.AppSettings["execution-time"];
			StringGoNewly = ConfigurationManager.AppSettings["go-newly"];
		}

		#endregion

		#region String Data

		public static bool IsAdvancedMode { get; }

		public static bool IsCalculateTime { get; }

		public static bool IsMultiInputForValues { get; }

		public static string StringTitle { get; }

		public static string StringEncoding { get; }

		public static string StringWrongData { get; }

		public static string StringValueCount { get; }

		public static string StringInputValue { get; }

		public static string StringFunctionInputValue { get; }

		public static string StringLimitationFunctionCount { get; }

		public static string StringLimitationFunction { get; }

		public static string StringLimitation { get; }

		public static string StringFunctionMaximum { get; }

		public static string StringFunctionCount { get; }

		public static string StringFunction { get; }

		public static string StringAsFractionalFunctions { get; }

		public static string StringInputFunctionsDenominators { get; }

		public static string StringFunctionsDenominators { get; }

		public static string StringResult { get; }

		public static string StringCount { get; }

		public static string StringLimitationFunctionResults { get; }

		public static string StringMatches { get; }

		public static string StringExecutionInfo { get; }

		public static string StringExecutionTime { get; }

		public static string StringGoNewly { get; }

		#endregion
	}
}
