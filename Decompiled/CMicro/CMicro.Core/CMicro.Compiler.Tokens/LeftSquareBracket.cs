namespace CMicro.Compiler.Tokens
{
	public class LeftSquareBracket : Token<object>
	{
		public LeftSquareBracket()
		{
			base.Value = new object();
			base.Kind = Kind.RightSquareBracket;
			base.Value = "[";
		}
	}
}
