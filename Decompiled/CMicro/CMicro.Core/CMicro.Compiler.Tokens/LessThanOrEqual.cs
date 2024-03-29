namespace CMicro.Compiler.Tokens
{
	public class LessThanOrEqual : Operator, IComparisonOperator
	{
		public LessThanOrEqual()
		{
			base.OperatorKind = OperatorKind.LessThanOrEqual;
			base.Value = "leq";
			base.Precedence = 8;
		}
	}
}
