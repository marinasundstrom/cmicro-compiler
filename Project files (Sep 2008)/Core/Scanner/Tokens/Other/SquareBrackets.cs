using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class LeftSquareBracket : Token<object>
    {
        public LeftSquareBracket()
        {
            this.Value = new object();
            this.Kind = Kind.Delimiter;

            this.Value = "[";
        }
    }

    public class RightSquareBracket : Token<object>
    {
        public RightSquareBracket()
        {
            this.Value = new object();
            this.Kind = Kind.Delimiter;

            this.Value = "]";
        }
    }
}
