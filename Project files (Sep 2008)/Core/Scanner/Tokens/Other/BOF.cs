using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class BOF : Token<object>
    {
        public BOF()
        {
            this.Value = new object();
            this.Kind = Kind.Other;

            this.Value = "BOF";
        }
    }
}
