namespace CMicro.Compiler.Tokens
{
	public class Not : Operator, IComparisonOperator
	{
		public Not()
		{
			base.Value = new object();
			base.Value = "!";
			base.Precedence = 3;
		}
	}
}
