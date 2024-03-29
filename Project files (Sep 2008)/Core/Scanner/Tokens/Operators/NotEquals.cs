using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class NotEqual : Operator
    {
        public NotEqual()
        {
            this.OperatorKind = OperatorKind.NotEquals;
            this.Value = "!=";

            this.Precedence = 1;
        }
    }
}
