using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Less : Operator
    {
        public Less()
        {
            this.OperatorKind = OperatorKind.Less;
            this.Value = "<";

            this.Precedence = 1;
        }
    }
}
