namespace CMicro.Compiler.Tokens
{
	public class Greater : Operator, IComparisonOperator
	{
		public Greater()
		{
			base.OperatorKind = OperatorKind.Greater;
			base.Value = "grt";
			base.Precedence = 8;
		}
	}
}
