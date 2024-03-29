using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class RightAngleBracket : Token<object>
    {
        public RightAngleBracket()
        {
            this.Value = new object();
            this.Kind = Kind.Delimiter;

            this.Value = ">";
        }
    }
}
