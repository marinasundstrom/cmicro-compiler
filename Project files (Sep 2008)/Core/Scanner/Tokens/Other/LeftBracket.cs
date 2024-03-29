using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class LeftBracket : Token<object>
    {
        public LeftBracket()
        {
            this.Value = new object();
            this.Kind = Kind.Delimiter;


            this.Value = "{";
        }
    }
}
