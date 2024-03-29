namespace CMicro.Compiler.Tokens
{
	public class Colon : Token<object>
	{
		public Colon()
		{
			base.Value = new object();
			base.Kind = Kind.Other;
			base.Value = ":";
		}
	}
}
