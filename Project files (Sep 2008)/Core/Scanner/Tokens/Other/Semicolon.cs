using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Semicolon : Token<object>
    {
        public Semicolon()
        {
            this.Value = new object();
            this.Kind = Kind.Other;

            this.Value = ";";
        }
    }
}
