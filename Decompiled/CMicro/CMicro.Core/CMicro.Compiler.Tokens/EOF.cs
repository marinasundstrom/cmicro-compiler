namespace CMicro.Compiler.Tokens
{
	public class EOF : Token<object>
	{
		public EOF()
		{
			base.Value = new object();
			base.Kind = Kind.EOF;
			base.Value = "EOF";
		}
	}
}
