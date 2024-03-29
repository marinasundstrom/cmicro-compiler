namespace CMicro.Compiler.Tokens
{
	public class SubtractAssign : Operator
	{
		public SubtractAssign()
		{
			base.OperatorKind = OperatorKind.Subtract;
			base.Value = "-=";
			base.Precedence = 6;
		}
	}
}
