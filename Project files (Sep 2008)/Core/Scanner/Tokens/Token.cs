using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    /// <summary>
    /// Base class of all tokens.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the token.</typeparam>
    public class Token<T> : IToken<T>
    {
        #region IToken<T>

        private T value;

        private int ln;
        private int ch;

        private Kind kind;

        public Token()
        {
            this.ln = 0;
            this.ch = 0;
        }

        public Token(T Value)
        {
            this.value = Value;
        }

        public Token(T Value, int Line, int Column)
        {
            this.value = Value;
            this.ln = Line;
            this.ch = Column;
        }

        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public int Line
        {
            get { return this.ln; }
            set { this.ln = value; }
        }

        public int Char
        {
            get { return this.ch; }
            set { this.ch = value; }
        }

        public Kind Kind
        {
            get { return this.kind; }
            set { this.kind = value; }
        }

        public string GetValue()
        {
            return this.value.ToString();
        }

        #endregion

        public bool Is(IToken type)
        {
            return value.GetType() == type.GetType();
        }

        public bool IsIdentifier
        {
            get { return value is Identifier; }
        }

        public bool IsKeyword
        {
            get { return value is Keyword; }
        }

        #region Static Members

        public static Type Identifier()
        {
            return typeof(Identifier);
        }

        public static Type Keyword()
        {
            return typeof(Keyword);
        }

        public static Type StringLiteral()
        {
            return typeof(StringLiteral);
        }

        public static Type CharLiteral()
        {
            return typeof(CharLiteral);
        }

        public static Type IntLiteral()
        {
            return typeof(IntLiteral);
        }

        public static Type RealLiteral()
        {
            return typeof(RealLiteral);
        }

        public static Type Operator()
        {
            return typeof(Operator);
        }

        public static Type Comment()
        {
            return typeof(Comment);
        }

        public static Type EOL()
        {
            return typeof(EOL);
        }

        public static Type EOF()
        {
            return typeof(EOF);
        }

        #endregion
    }
}
