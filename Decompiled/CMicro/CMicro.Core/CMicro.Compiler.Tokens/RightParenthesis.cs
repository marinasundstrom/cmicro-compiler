namespace CMicro.Compiler.Tokens
{
	public class RightParenthesis : Operator
	{
		public RightParenthesis()
		{
			base.Kind = Kind.RightSquareBracket;
			base.Value = ")";
			base.Precedence = -1;
		}
	}
}
