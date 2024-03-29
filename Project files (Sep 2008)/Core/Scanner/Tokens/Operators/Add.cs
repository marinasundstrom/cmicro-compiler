using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Add : Operator
    {
        public Add()
        {
            this.OperatorKind = OperatorKind.Add;
            this.Value = "+";

            this.Precedence = 5;
        }
    }

    public class AddAssign : Operator
    {
        public AddAssign()
        {
            this.OperatorKind = OperatorKind.Add;
            this.Value = "+=";

            this.Precedence = 0;
        }
    }
}
