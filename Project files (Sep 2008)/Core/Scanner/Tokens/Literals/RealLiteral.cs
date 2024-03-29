using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Ast;

namespace CSharp.Compiler.Tokens
{
    public class RealLiteral : Token<double>, ILiteral
    {
        public RealLiteral()
        {
            this.Kind = Kind.RealLiteral;
        }

        public RealLiteral(double Value)
        {
            this.Value = Value;
            this.Kind = Kind.RealLiteral;
        }

        #region ILiteral Members

        public Operand ToOperand()
        {
            return new Operand() { Token = this };
        }

        #endregion
    }
}
