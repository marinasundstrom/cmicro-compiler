using System.Linq;

namespace CMicro.Library
{
	[CMicroModule(CMicroModuleTypes.Library)]
	public static class Strings
	{
		public static string concat(string string1, string string2)
		{
			return string1 + string2;
		}

		public static string insert(string input, int index, string value)
		{
			return input.Insert(index, value);
		}

		public static int strlen(string input)
		{
			return input.Length;
		}

		public static string substr(string input, int start, int length)
		{
			return input.Substring(start, length);
		}

		public static bool str_startswith(string input, string term, bool ignoreCase)
		{
			return input.StartsWith(term, ignoreCase, null);
		}

		public static bool str_endswith(string input, string value, bool ignoreCase)
		{
			return input.EndsWith(value, ignoreCase, null);
		}

		public static int str_indexof(string input, string value)
		{
			return input.IndexOf(value);
		}

		public static string strtolower(string input)
		{
			return input.ToLower();
		}

		public static string strtoupper(string input)
		{
			return input.ToUpper();
		}

		public static bool str_contains(string input, string value)
		{
			return input.Contains(value);
		}

		public static string str_replace(string input, string oldValue, string newValue)
		{
			return input.Replace(oldValue, newValue);
		}

		public static int strcmp(string str1, string str2, bool ignoreCase)
		{
			return string.Compare(str1, str2, ignoreCase);
		}

		public static char[] str_to_chararray(string input)
		{
			return input.ToCharArray();
		}

		public static string chararray_to_str(char[] input)
		{
			return new string(input);
		}

		public static string str_reverse(string input)
		{
			return new string(input.ToCharArray().Reverse().ToArray());
		}
	}
}
