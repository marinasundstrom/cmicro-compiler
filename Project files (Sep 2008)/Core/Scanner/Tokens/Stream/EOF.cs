using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class EOF : Token<object>
    {
        public EOF()
        {
            this.Value = new object();
            this.Kind = Kind.EOF;

            this.Value = "EOF";
        }
    }
}
