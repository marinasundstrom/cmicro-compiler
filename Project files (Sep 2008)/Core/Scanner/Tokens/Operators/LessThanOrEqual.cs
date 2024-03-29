using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class LessThanOrEqual : Operator
    {
        public LessThanOrEqual()
        {
            this.OperatorKind = OperatorKind.LessThanOrEqual;
            this.Value = ">=";

            this.Precedence = 1;
        }
    }
}
