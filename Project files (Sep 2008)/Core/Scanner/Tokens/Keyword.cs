using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Keyword : Token<string>
    {
        public Keyword()
        {
            this.Kind = Kind.Keyword;
        }

        public Keyword(string Value)
        {
            this.Kind = Kind.Keyword;
            this.Value = Value;
        }
    }
}
