using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class RightBracket : Token<object>
    {
        public RightBracket()
        {
            this.Value = new object();
            this.Kind = Kind.Delimiter;

            this.Value = "}";
        }
    }
}
