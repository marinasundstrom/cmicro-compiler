using CMicro.Compiler.Tokens;

namespace CMicro.Compiler
{
	public class ParseError2 : IParseError
	{
		private readonly ParseErrorCodes errorCode;

		private readonly bool includedata;

		private readonly string message;

		private readonly IToken token;

		public IToken Token => token;

		public ParseErrorCodes ErrorCode => errorCode;

		public ParseError2(string message, IToken token, bool includedata)
		{
			this.message = message;
			this.includedata = includedata;
			this.token = token;
			errorCode = ParseErrorCodes.Defined;
		}

		public override string ToString()
		{
			if (includedata)
			{
				return $"{message} at {token.Char}:{token.Line}";
			}
			return $"{message}";
		}
	}
}
