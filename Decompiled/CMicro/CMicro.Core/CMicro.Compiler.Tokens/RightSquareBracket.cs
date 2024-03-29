namespace CMicro.Compiler.Tokens
{
	public class RightSquareBracket : Token<object>
	{
		public RightSquareBracket()
		{
			base.Value = new object();
			base.Kind = Kind.RightSquareBracket;
			base.Value = "]";
		}
	}
}
