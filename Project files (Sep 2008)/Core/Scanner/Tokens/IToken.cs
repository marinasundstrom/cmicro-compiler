using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    /// <summary>
    /// Absolute base interface of all tokens. Provides important structures for the implementing class.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// The line of the token.
        /// </summary>
        int Line {get; set;}
        /// <summary>
        /// The position of the token.
        /// </summary>
        int Char {get; set;}

        /// <summary>
        /// The kind of the token.
        /// </summary>
        Kind Kind { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The value of the token as a string.</returns>
        string GetValue();
    }

    /// <summary>
    /// Interface containg extensions for the IToken class.
    /// </summary>
    /// <typeparam name="T">Type of value the implementing token class contains.</typeparam>
    public interface IToken<T> : IToken
    {
        /// <summary>
        /// The value of the token.
        /// </summary>
        T Value { get; set; }
    }
}
