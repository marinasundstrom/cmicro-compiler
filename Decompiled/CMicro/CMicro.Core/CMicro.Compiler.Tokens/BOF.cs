namespace CMicro.Compiler.Tokens
{
	public class BOF : Token<object>
	{
		public BOF()
		{
			base.Value = new object();
			base.Kind = Kind.Other;
			base.Value = "BOF";
		}
	}
}
