using CMicro.Compiler.Ast;

namespace CMicro.Compiler.Tokens
{
	public class StringLiteral : Token<string>, ILiteral, IExpression, IOperand
	{
		public StringLiteral()
		{
			base.Kind = Kind.StringLiteral;
		}

		public StringLiteral(string Value)
		{
			base.Value = Value;
			base.Kind = Kind.StringLiteral;
		}
	}
}
