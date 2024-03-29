using CMicro.Compiler.Ast;

namespace CMicro.Compiler.Tokens
{
	public class IntLiteral : Token<int>, ILiteral, IExpression, IOperand
	{
		public IntLiteral()
		{
			base.Kind = Kind.IntLiteral;
		}

		public IntLiteral(int Value)
		{
			base.Value = Value;
			base.Kind = Kind.IntLiteral;
		}
	}
}
