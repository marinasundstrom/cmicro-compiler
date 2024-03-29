using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Ast;

namespace CSharp.Compiler.Tokens
{
    public class StringLiteral : Token<string>, ILiteral
    {
        public StringLiteral()
        {
            this.Kind = Kind.StringLiteral;
        }

        public StringLiteral(string Value)
        {
            this.Value = Value;
            this.Kind = Kind.StringLiteral;
        }

        #region ILiteral Members

        public Operand ToOperand()
        {
            return new Operand() { Token = this };
        }

        #endregion
    }
}
