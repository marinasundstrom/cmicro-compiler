using CMicro.Compiler.Ast;

namespace CMicro.Compiler.Tokens
{
	public class Keyword : Token<string>, IExpression, IOperand
	{
		public Keyword()
		{
			base.Kind = Kind.Keyword;
		}

		public Keyword(string Value)
		{
			base.Kind = Kind.Keyword;
			base.Value = Value;
		}
	}
}
