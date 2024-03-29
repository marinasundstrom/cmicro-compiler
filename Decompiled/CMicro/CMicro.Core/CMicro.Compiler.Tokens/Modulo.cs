namespace CMicro.Compiler.Tokens
{
	public class Modulo : Operator, IArithmeticOperator
	{
		public Modulo()
		{
			base.OperatorKind = OperatorKind.Modulo;
			base.Value = "%";
			base.Precedence = 5;
		}
	}
}
