namespace CMicro.Compiler.Tokens
{
	public class NotEqual : Operator, IComparisonOperator
	{
		public NotEqual()
		{
			base.OperatorKind = OperatorKind.NotEquals;
			base.Value = "neq";
			base.Precedence = 9;
		}
	}
}
