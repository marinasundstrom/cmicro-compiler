namespace CMicro.Compiler.Tokens
{
	public class RightAngleBracket : Operator, IComparisonOperator
	{
		public RightAngleBracket()
		{
			base.Value = new object();
			base.Kind = Kind.RightSquareBracket;
			base.Value = "grt";
			base.Precedence = 8;
		}
	}
}
