using System;

namespace CMicro.Library
{
	[CMicroModule(CMicroModuleTypes.Library)]
	public static class Console
	{
		public static void println(string text)
		{
			System.Console.WriteLine(text);
		}

		public static void print(string text)
		{
			System.Console.Write(text);
		}

		public static string readln()
		{
			return System.Console.ReadLine();
		}

		public static int read()
		{
			return System.Console.Read();
		}

		public static int readint()
		{
			return int.Parse(System.Console.ReadLine());
		}

		public static double readdouble()
		{
			return double.Parse(System.Console.ReadLine());
		}

		public static char readchar()
		{
			return Convert.ToChar(System.Console.Read());
		}

		public static void setConsoleTitle(string text)
		{
			System.Console.Title = text;
		}

		public static void setConsoleHeight(int value)
		{
			System.Console.WindowHeight = value;
		}

		public static void setConsoleWidth(int value)
		{
			System.Console.WindowWidth = value;
		}

		public static void setConsoleSize(int width, int height)
		{
			System.Console.SetWindowSize(width, height);
		}

		public static void setConsolePosition(int left, int top)
		{
			System.Console.SetWindowPosition(left, top);
		}

		public static void setConsoleForeground(int value)
		{
			System.Console.ForegroundColor = (ConsoleColor)value;
		}

		public static void setConsoleBackground(int value)
		{
			System.Console.ForegroundColor = (ConsoleColor)value;
		}
	}
}
