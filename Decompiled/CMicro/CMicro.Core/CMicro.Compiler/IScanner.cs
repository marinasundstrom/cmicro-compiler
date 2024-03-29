using System.Collections.Generic;
using System.IO;
using CMicro.Compiler.Tokens;

namespace CMicro.Compiler
{
	public interface IScanner
	{
		TextReader Reader { get; }

		int Line { get; }

		int Char { get; }

		bool IsCaseSensitive { get; }

		IToken Next();

		List<IToken> Scan();
	}
}
