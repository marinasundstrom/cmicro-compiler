namespace CMicro.Compiler.Tokens
{
	public class GreaterThanOrEqual : Operator, IComparisonOperator
	{
		public GreaterThanOrEqual()
		{
			base.OperatorKind = OperatorKind.GreaterThanOrEqual;
			base.Value = "geq";
			base.Precedence = 8;
		}
	}
}
