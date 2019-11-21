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

			EncodingString = ConfigurationManager.AppSettings["encoding"];
			WrongDataString = ConfigurationManager.AppSettings["wrong-data"];
			ValueCountString = ConfigurationManager.AppSettings["value-count"];
			InputValueString = ConfigurationManager.AppSettings["input-values"];
			FunctionInputValueString = ConfigurationManager.AppSettings["function-input-values"];
			LimitationFunctionCountString = ConfigurationManager.AppSettings["limitation-function-count"];
			LimitationFunctionString = ConfigurationManager.AppSettings["limitation-function"];
			LimitationString = ConfigurationManager.AppSettings["limitation"];
			FunctionMaximumString = ConfigurationManager.AppSettings["function-maximum"];
			FunctionCountString = ConfigurationManager.AppSettings["function-count"];
			FunctionString = ConfigurationManager.AppSettings["function"];
			AsFractionalFunctionsString = ConfigurationManager.AppSettings["as-fractional-functions"];
			InputFunctionsDenominatorsString = ConfigurationManager.AppSettings["input-functions-denominators"];
			FunctionsDenominatorsString = ConfigurationManager.AppSettings["functions-denominators"];
			ResultString = ConfigurationManager.AppSettings["result"];
			CountString = ConfigurationManager.AppSettings["count"];
			LimitationFunctionResultsString = ConfigurationManager.AppSettings["limitation-function-results"];
			MatchesString = ConfigurationManager.AppSettings["matches"];
			ExecutionInfoString = ConfigurationManager.AppSettings["execution-info"];
			ExecutionTimeString = ConfigurationManager.AppSettings["execution-time"];
			GoNewlyString = ConfigurationManager.AppSettings["go-newly"];
		}

		#endregion

		#region String Data

		public static bool IsAdvancedMode { get; }

		public static bool IsCalculateTime { get; }

		public static bool IsMultiInputForValues { get; }

		public static string EncodingString { get; }

		public static string WrongDataString { get; }

		public static string ValueCountString { get; }

		public static string InputValueString { get; }

		public static string FunctionInputValueString { get; }

		public static string LimitationFunctionCountString { get; }

		public static string LimitationFunctionString { get; }

		public static string LimitationString { get; }

		public static string FunctionMaximumString { get; }

		public static string FunctionCountString { get; }

		public static string FunctionString { get; }

		public static string AsFractionalFunctionsString { get; }

		public static string InputFunctionsDenominatorsString { get; }

		public static string FunctionsDenominatorsString { get; }

		public static string ResultString { get; }

		public static string CountString { get; }

		public static string LimitationFunctionResultsString { get; }

		public static string MatchesString { get; }

		public static string ExecutionInfoString { get; }

		public static string ExecutionTimeString { get; }

		public static string GoNewlyString { get; }

		#endregion
	}
}
