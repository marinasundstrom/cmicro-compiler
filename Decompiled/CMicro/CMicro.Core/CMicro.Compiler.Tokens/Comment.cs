namespace CMicro.Compiler.Tokens
{
	public class Comment : Token<string>
	{
		public Comment()
		{
			base.Kind = Kind.Comment;
		}

		public Comment(string Value)
		{
			base.Kind = Kind.Comment;
			base.Value = Value;
		}
	}
}
