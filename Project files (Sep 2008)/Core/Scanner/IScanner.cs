using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler
{
    /// <summary>
    /// Represents methods in a lexical scanner.
    /// </summary>
    public interface IScanner
    {
        /// <summary>
        /// Returns the next token in the stream.
        /// </summary>
        /// <returns>The next token in the stream.</returns>
        Tokens.IToken Next();
        /// <summary>
        /// Start scanning for tokens.
        /// </summary>
        /// <returns>Collection of tokens.</returns>
        List<Tokens.IToken> Scan();

        /// <summary>
        /// Gets the reader object used in this process.
        /// </summary>
        System.IO.TextReader Reader { get; }

        /// <summary>
        /// The current line.
        /// </summary>
        int Line { get; }
        /// <summary>
        /// The current character.
        /// </summary>
        int Char { get; }

        /// <summary>
        /// Gets a value indicating if the scanner is case-sensitive.
        /// </summary>
        bool IsCaseSensitive { get; }
    }
}
