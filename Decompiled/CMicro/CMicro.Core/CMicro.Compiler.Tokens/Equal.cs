namespace CMicro.Compiler.Tokens
{
	public class Equal : Operator, IComparisonOperator
	{
		public Equal()
		{
			base.OperatorKind = OperatorKind.Equals;
			base.Value = "eql";
			base.Precedence = 9;
		}
	}
}
