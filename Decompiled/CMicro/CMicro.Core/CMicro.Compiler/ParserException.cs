using System;

namespace CMicro.Compiler
{
	public class ParserException : Exception
	{
		private readonly int _ch;

		private readonly int _ln;

		private string message;

		public int Ln => _ln;

		public int Ch => _ch;

		public ParserException(string message, int ln, int ch)
			: base(message)
		{
			this.message = message;
			_ln = ln;
			_ch = ch;
		}

		public ParserException()
		{
		}
	}
}
