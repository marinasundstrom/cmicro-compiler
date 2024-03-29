namespace CMicro.Compiler.Tokens
{
	public class Period : Token<object>
	{
		public Period()
		{
			base.Value = new object();
			base.Kind = Kind.Other;
			base.Value = ".";
		}
	}
}
