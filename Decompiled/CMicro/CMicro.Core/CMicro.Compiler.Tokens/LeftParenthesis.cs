namespace CMicro.Compiler.Tokens
{
	public class LeftParenthesis : Operator
	{
		public LeftParenthesis()
		{
			base.Kind = Kind.RightSquareBracket;
			base.Value = "(";
			base.Precedence = -1;
		}
	}
}
