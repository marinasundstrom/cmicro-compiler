using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Equal : Operator
    {
        public Equal()
        {
            this.OperatorKind = OperatorKind.Equals;
            this.Value = "==";

            this.Precedence = 1;
        }
    }
}
