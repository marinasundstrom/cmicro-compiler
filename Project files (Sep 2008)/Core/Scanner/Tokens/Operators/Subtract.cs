using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Subtract : Operator
    {
        public Subtract()
        {
            this.OperatorKind = OperatorKind.Subtract;
            this.Value = "-";

            this.Precedence = 5;
        }
    }

    public class SubtractAssign : Operator
    {
        public SubtractAssign()
        {
            this.OperatorKind = OperatorKind.Subtract;
            this.Value = "-=";

            this.Precedence = 0;
        }
    }
}
