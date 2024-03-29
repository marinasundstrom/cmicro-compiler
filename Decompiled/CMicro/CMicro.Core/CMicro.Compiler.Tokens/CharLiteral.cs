using CMicro.Compiler.Ast;

namespace CMicro.Compiler.Tokens
{
	public class CharLiteral : Token<char>, ILiteral, IExpression, IOperand
	{
		public CharLiteral()
		{
			base.Kind = Kind.CharLiteral;
		}

		public CharLiteral(char Value)
		{
			base.Value = Value;
			base.Kind = Kind.CharLiteral;
		}
	}
}
