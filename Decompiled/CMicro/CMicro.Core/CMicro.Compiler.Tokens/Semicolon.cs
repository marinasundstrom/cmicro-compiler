namespace CMicro.Compiler.Tokens
{
	public class Semicolon : Token<object>
	{
		public Semicolon()
		{
			base.Value = new object();
			base.Kind = Kind.Other;
			base.Value = ";";
		}
	}
}
