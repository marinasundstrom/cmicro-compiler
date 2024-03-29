using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class EOL : Token<object>
    {
        public EOL()
        {
            this.Value = new object();
            this.Kind = Kind.EOL;

            this.Value = "EOL";
        }
    }
}
