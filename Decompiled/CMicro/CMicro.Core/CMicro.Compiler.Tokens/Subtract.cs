namespace CMicro.Compiler.Tokens
{
	public class Subtract : Operator, IArithmeticOperator
	{
		public Subtract()
		{
			base.OperatorKind = OperatorKind.Subtract;
			base.Value = "sub";
			base.Precedence = 3;
		}
	}
}
