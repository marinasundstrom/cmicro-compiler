using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Divide : Operator
    {
        public Divide()
        {
            this.OperatorKind = OperatorKind.Divide;
            this.Value = "/";

            this.Precedence = 4;
        }
    }

    public class DivideAssign : Operator
    {
        public DivideAssign()
        {
            this.OperatorKind = OperatorKind.Divide;
            this.Value = "/=";

            this.Precedence = 0;
        }
    }
}

