namespace CMicro.Compiler.Tokens
{
	public interface IToken
	{
		int Line { get; set; }

		int Char { get; set; }

		Kind Kind { get; set; }

		string GetValue();
	}
	public interface IToken<T> : IToken
	{
		T Value { get; set; }
	}
}
