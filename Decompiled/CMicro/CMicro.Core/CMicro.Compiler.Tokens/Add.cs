namespace CMicro.Compiler.Tokens
{
	public class Add : Operator, IArithmeticOperator
	{
		public Add()
		{
			base.OperatorKind = OperatorKind.Add;
			base.Value = "add";
			base.Precedence = 3;
		}
	}
}
