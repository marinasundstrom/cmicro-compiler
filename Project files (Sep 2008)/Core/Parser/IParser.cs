using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Tokens;
using CSharp.Compiler.Ast;

namespace CSharp.Compiler
{
    /// <summary>
    /// Represents methods in a semantic parser.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Returns the next token in the stream.
        /// </summary>
        IToken LookaheadToken { get; }
        /// <summary>
        /// Returns the current token in the stream.
        /// </summary>
        IToken CurrentToken { get; }
        /// <summary>
        /// Accesses the result of the parsing process.
        /// </summary>
        AstRoot Result { get; }

        /// <summary>
        /// Gets the value indicating if the parser ignores the case a character or not.
        /// </summary>
        bool IgnoreCase { get; }
        /// <summary>
        /// Gets the value indicating if the parser will abort when an error has been detected.
        /// </summary>
        bool AbortOnError { get; }
        /// <summary>
        /// Returns a collection of errors.
        /// </summary>
        List<IParseError> Errors { get; }

        /// <summary>
        /// Abort the process.
        /// </summary>
        void Abort();
        /// <summary>
        /// Move to the next token and return it.
        /// </summary>
        /// <returns>The next token in the stream.</returns>
        IToken Next();

        void GoBack();
        void GoBack(int steps);

        IToken NextIf(string str);
        IToken NextIf(Kind tokenType);
        IToken NextIf(Kind kind1, Kind kind2);
        IToken NextIf(OperatorKind Operator);

        bool NextIs(string str);
        bool NextIs(Kind tokenType);
        bool NextIs(Kind kind1, Kind kind2);
        bool NextIs(OperatorKind Operator);

        bool NextNameIs(string str);
        /// <summary>
        /// Checks a token at a specific index and compares its value to a specific string.
        /// </summary>
        /// <param name="Ift">Indexes from the current position.</param>
        /// <param name="str">The string to compare.</param>
        /// <returns>Value indicating the equallity of the token and the string.</returns>
        bool CheckIndex(int Ift, string str);

        void NextWhile(string str);
        void NextWhile(Kind tokenType);

        bool SameKind(IToken Token1, IToken Token2);
        bool TokenIs(IToken Token, Kind tokenKind);
        bool TokenIs(IToken Token, Kind kind1, Kind kind2);
        bool TokenIs(IToken Token, OperatorKind operatorKind);
        bool TokenIs(IToken Token, string Value);

        /// <summary>
        /// Report an error that occurred during the parse.
        /// </summary>
        /// <param name="token">The token at the position when the error was reported.</param>
        /// <param name="errorCode">The error code indicating what type of error.</param>
        void ReportSyntaxError(IToken token, ParseErrorCodes errorCode);
    }
}
