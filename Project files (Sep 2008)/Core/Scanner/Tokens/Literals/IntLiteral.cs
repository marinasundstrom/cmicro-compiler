using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Ast;

namespace CSharp.Compiler.Tokens
{
    public class IntLiteral : Token<int>, ILiteral
    {
        public IntLiteral()
        {
            this.Kind = Kind.IntLiteral;
        }

        public IntLiteral(int Value)
        {
            this.Value = Value;
            this.Kind = Kind.IntLiteral;
        }

        #region ILiteral Members

        public Operand ToOperand()
        {
            return new Operand() { Token = this };
        }

        #endregion
    }
}
