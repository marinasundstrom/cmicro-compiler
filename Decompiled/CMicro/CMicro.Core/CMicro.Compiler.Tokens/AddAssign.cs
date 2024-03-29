namespace CMicro.Compiler.Tokens
{
	public class AddAssign : Operator
	{
		public AddAssign()
		{
			base.OperatorKind = OperatorKind.Add;
			base.Value = "+=";
			base.Precedence = 16;
		}
	}
}
