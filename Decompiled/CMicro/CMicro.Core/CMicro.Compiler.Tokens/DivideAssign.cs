namespace CMicro.Compiler.Tokens
{
	public class DivideAssign : Operator
	{
		public DivideAssign()
		{
			base.OperatorKind = OperatorKind.Divide;
			base.Value = "/=";
			base.Precedence = 16;
		}
	}
}
