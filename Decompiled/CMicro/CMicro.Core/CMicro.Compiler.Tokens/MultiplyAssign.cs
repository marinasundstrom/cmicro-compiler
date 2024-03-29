namespace CMicro.Compiler.Tokens
{
	public class MultiplyAssign : Operator
	{
		public MultiplyAssign()
		{
			base.OperatorKind = OperatorKind.Multiply;
			base.Value = "*=";
			base.Precedence = 16;
		}
	}
}
