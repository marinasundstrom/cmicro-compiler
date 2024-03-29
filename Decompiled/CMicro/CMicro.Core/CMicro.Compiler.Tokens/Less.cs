namespace CMicro.Compiler.Tokens
{
	public class Less : Operator, IComparisonOperator
	{
		public Less()
		{
			base.OperatorKind = OperatorKind.Less;
			base.Value = "les";
			base.Precedence = 8;
		}
	}
}
