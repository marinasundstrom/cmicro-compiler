using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Modulo : Operator
    {
        public Modulo()
        {
            this.OperatorKind = OperatorKind.Modulo;
            this.Value = "%";

            this.Precedence = 3;
        }
    }

    public class ModuloAssign : Operator
    {
        public ModuloAssign()
        {
            this.OperatorKind = OperatorKind.Modulo;
            this.Value = "%=";

            this.Precedence = 0;
        }
    }
}
