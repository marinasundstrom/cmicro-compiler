using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Comma : Token<object>
    {
        public Comma()
        {
            this.Value = new object();
            this.Kind = Kind.Other;

            this.Value = ",";
        }
    }
}
