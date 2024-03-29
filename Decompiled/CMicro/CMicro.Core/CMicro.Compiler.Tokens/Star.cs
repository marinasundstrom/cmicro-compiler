namespace CMicro.Compiler.Tokens
{
	public class Star : Token<object>
	{
		public Star()
		{
			base.Value = new object();
			base.Kind = Kind.Other;
			base.Value = "*";
		}
	}
}
