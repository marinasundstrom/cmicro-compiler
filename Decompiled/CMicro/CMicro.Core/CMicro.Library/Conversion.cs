using System;

namespace CMicro.Library
{
	[CMicroModule(CMicroModuleTypes.Library)]
	public static class Conversion
	{
		public static string object_to_str(object value)
		{
			return value.ToString();
		}

		public static int double_to_int(double value)
		{
			return Convert.ToInt32(value);
		}

		public static string double_to_str(double value)
		{
			return Convert.ToString(value);
		}

		public static double str_to_double(string value)
		{
			return double.Parse(value);
		}

		public static double str_to_int(string value)
		{
			return int.Parse(value);
		}

		public static bool str_to_bool(string value)
		{
			return bool.Parse(value);
		}

		public static char str_to_char(string value)
		{
			return char.Parse(value);
		}

		public static double int_to_double(int value)
		{
			return Convert.ToDouble(value);
		}

		public static bool int_to_bool(int value)
		{
			return Convert.ToBoolean(value);
		}

		public static char int_to_char(int value)
		{
			return Convert.ToChar(value);
		}

		public static string int_to_str(int value)
		{
			return Convert.ToString(value);
		}

		public static int bool_to_int(bool value)
		{
			return Convert.ToInt32(value);
		}

		public static string bool_to_str(bool value)
		{
			return Convert.ToString(value);
		}

		public static int char_to_int(char value)
		{
			return Convert.ToInt32(value);
		}

		public static string char_to_str(char value)
		{
			return Convert.ToString(value);
		}

		public static object box_int(int value)
		{
			return value;
		}

		public static object box_(char value)
		{
			return value;
		}

		public static object box_double(double value)
		{
			return value;
		}

		public static object box_bool(bool value)
		{
			return value;
		}
	}
}
