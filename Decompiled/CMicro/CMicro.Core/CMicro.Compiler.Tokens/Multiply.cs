namespace CMicro.Compiler.Tokens
{
	public class Multiply : Operator, IArithmeticOperator
	{
		public Multiply()
		{
			base.OperatorKind = OperatorKind.Multiply;
			base.Value = "mul";
			base.Precedence = 5;
		}
	}
}
