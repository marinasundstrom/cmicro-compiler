using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Not : Operator
    {
        public Not()
        {
            this.Value = new object();
            this.Value = "!";
            
            this.Precedence = 1;
        }
    }
}
