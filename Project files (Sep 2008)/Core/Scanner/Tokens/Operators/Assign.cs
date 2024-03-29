using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Assign : Operator
    {
        public Assign()
        {
            this.OperatorKind = OperatorKind.Assign;
            this.Value = "=";

            this.Precedence = 0;
        }
    }
}
