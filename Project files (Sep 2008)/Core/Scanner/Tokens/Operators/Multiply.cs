using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Multiply : Operator
    {
        public Multiply()
        {
            this.OperatorKind = OperatorKind.Multiply;
            this.Value = "*";

            this.Precedence = 4;
        }
    }

    public class MultiplyAssign : Operator
    {
        public MultiplyAssign()
        {
            this.OperatorKind = OperatorKind.Multiply;
            this.Value = "*=";

            this.Precedence = 0;
        }
    }
}

