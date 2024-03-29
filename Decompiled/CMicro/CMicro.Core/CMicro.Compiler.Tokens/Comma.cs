namespace CMicro.Compiler.Tokens
{
	public class Comma : Token<object>
	{
		public Comma()
		{
			base.Value = new object();
			base.Kind = Kind.Other;
			base.Value = ",";
		}
	}
}
