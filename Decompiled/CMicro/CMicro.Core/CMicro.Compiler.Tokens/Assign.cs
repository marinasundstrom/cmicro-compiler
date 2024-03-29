namespace CMicro.Compiler.Tokens
{
	public class Assign : Operator
	{
		public Assign()
		{
			base.OperatorKind = OperatorKind.Assign;
			base.Value = "=";
			base.Precedence = 0;
		}
	}
}
