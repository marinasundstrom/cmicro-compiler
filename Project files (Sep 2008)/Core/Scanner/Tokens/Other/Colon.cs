using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Colon : Token<object>
    {
        public Colon()
        {
            this.Value = new object();
            this.Kind = Kind.Other;

            this.Value = ":";
        }
    }
}
