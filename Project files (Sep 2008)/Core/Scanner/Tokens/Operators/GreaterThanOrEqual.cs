using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class GreaterThanOrEqual : Operator
    {
        public GreaterThanOrEqual()
        {
            this.OperatorKind = OperatorKind.GreaterThanOrEqual;
            this.Value = ">=";

            this.Precedence = 1;
        }
    }
}
