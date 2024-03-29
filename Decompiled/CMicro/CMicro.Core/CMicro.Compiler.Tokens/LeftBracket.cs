namespace CMicro.Compiler.Tokens
{
	public class LeftBracket : Token<object>
	{
		public LeftBracket()
		{
			base.Value = new object();
			base.Kind = Kind.RightSquareBracket;
			base.Value = "{";
		}
	}
}
