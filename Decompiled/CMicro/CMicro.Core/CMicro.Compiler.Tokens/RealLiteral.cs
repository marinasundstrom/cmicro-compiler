using CMicro.Compiler.Ast;

namespace CMicro.Compiler.Tokens
{
	public class RealLiteral : Token<double>, ILiteral, IExpression, IOperand
	{
		public RealLiteral()
		{
			base.Kind = Kind.RealLiteral;
		}

		public RealLiteral(double Value)
		{
			base.Value = Value;
			base.Kind = Kind.RealLiteral;
		}
	}
}
