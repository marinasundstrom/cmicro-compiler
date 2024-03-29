using CMicro.Compiler.Tokens;

namespace CMicro.Compiler
{
	public class ParseError : IParseError
	{
		private readonly ParseErrorCodes errorCode;

		private readonly IToken token;

		public IToken Token => token;

		public ParseErrorCodes ErrorCode => errorCode;

		public ParseError(IToken token, ParseErrorCodes errorCode)
		{
			this.token = token;
			this.errorCode = errorCode;
		}

		public override string ToString()
		{
			if (errorCode == ParseErrorCodes.ExpectedToken)
			{
				return $"Expected {token.Kind.ToString().ToLower()} at {token.Char}:{token.Line}.";
			}
			if (errorCode == ParseErrorCodes.UnexpectedToken)
			{
				return $"Unexpected {token.Kind.ToString().ToLower()} at {token.Char}:{token.Line}.";
			}
			return string.Format("Undefined error at {1}:{2}.", token.Kind.ToString().ToLower(), token.Char, token.Line);
		}
	}
}
