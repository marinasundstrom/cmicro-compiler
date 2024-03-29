using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Star : Token<object>
    {
        public Star()
        {
            this.Value = new object();
            this.Kind = Kind.Other;

            this.Value = "*";
        }
    }
}
