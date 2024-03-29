using CMicro.Compiler.Tokens;

namespace CMicro.Compiler
{
	public interface IParseError
	{
		IToken Token { get; }

		ParseErrorCodes ErrorCode { get; }
	}
}
