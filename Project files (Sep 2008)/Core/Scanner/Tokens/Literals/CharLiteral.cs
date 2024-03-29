using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Ast;

namespace CSharp.Compiler.Tokens
{
    public class CharLiteral : Token<char>, ILiteral
    {
        public CharLiteral()
        {
            this.Kind = Kind.CharLiteral;
        }

        public CharLiteral(char Value)
        {
            this.Value = Value;
            this.Kind = Kind.CharLiteral;
        }

        #region ILiteral Members

        public Operand ToOperand()
        {
            return new Operand(){ Token = this };
        }

        #endregion
    }
}
