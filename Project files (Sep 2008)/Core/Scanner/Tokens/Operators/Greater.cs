using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Greater : Operator
    {
        public Greater()
        {
            this.OperatorKind = OperatorKind.Greater;
            this.Value = ">";

            this.Precedence = 1;
        }
    }
}
