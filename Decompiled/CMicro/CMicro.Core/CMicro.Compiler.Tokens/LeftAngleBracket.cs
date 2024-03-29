namespace CMicro.Compiler.Tokens
{
	public class LeftAngleBracket : Operator, IComparisonOperator
	{
		public LeftAngleBracket()
		{
			base.Value = new object();
			base.Kind = Kind.RightSquareBracket;
			base.Value = "les";
			base.Precedence = 8;
		}
	}
}
