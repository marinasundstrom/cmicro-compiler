namespace CMicro.Compiler.Tokens
{
	public class EOL : Token<object>
	{
		public EOL()
		{
			base.Value = new object();
			base.Kind = Kind.EOL;
			base.Value = "EOL";
		}
	}
}
