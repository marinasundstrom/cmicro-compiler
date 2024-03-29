using System.Collections.Generic;
using CMicro.Compiler.Ast;
using CMicro.Compiler.Tokens;

namespace CMicro.Compiler
{
	public interface IParser
	{
		IToken LookaheadToken { get; }

		IToken CurrentToken { get; }

		AstRoot Result { get; }

		bool IgnoreCase { get; }

		bool AbortOnError { get; }

		List<IParseError> Errors { get; }

		void Abort();

		IToken NextToken();

		void GoBack();

		void GoBack(int steps);

		IToken Eat(string str);

		IToken Eat(Kind tokenType);

		IToken Eat(Kind kind1, Kind kind2);

		IToken Eat(OperatorKind Operator);

		bool NextTokenIs(string str);

		bool NextTokenIs(Kind tokenType);

		bool NextTokenIs(Kind kind1, Kind kind2);

		bool NextTokenIs(OperatorKind Operator);

		bool NextNameIs(string str);

		bool CheckIndex(int Ift, string str);

		void EatIfWhile(string str);

		void EatIfWhile(Kind tokenType);

		bool SameKind(IToken Token1, IToken Token2);

		bool TokenIsKind(IToken Token, Kind tokenKind);

		bool TokenIsOneOfKinds(IToken Token, Kind kind1, Kind kind2);

		bool IsTokenOperatorKind(IToken Token, OperatorKind operatorKind);

		bool CompareTokenString(IToken Token, string Value);

		void ReportSyntaxError(IToken token, ParseErrorCodes errorCode);
	}
}
