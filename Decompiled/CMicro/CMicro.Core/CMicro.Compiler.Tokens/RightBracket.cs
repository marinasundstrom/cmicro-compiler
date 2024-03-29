namespace CMicro.Compiler.Tokens
{
	public class RightBracket : Token<object>
	{
		public RightBracket()
		{
			base.Value = new object();
			base.Kind = Kind.RightSquareBracket;
			base.Value = "}";
		}
	}
}
