namespace CMicro.Compiler.Tokens
{
	public class Divide : Operator, IArithmeticOperator
	{
		public Divide()
		{
			base.OperatorKind = OperatorKind.Divide;
			base.Value = "div";
			base.Precedence = 5;
		}
	}
}
