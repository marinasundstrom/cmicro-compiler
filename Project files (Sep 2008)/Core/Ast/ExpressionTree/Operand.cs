using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Tokens;

namespace CSharp.Compiler.Ast
{
    public class Operand : INode
    {
        public IToken Token;

        public string Namn
        {
            get 
            {
                if (Token is Identifier)
                    return Token.GetValue();
                else
                    return null;
            }
        }

        public bool Literal
        {
            get { return this.Token is ILiteral; }
        }

        public bool Variable
        {
            get { return this.Token.GetType() != typeof(ILiteral); }
        }

        public string Value
        {
            get
            {
                if (!(Token is Identifier))
                    return Token.GetValue();
                else
                    return null;
            }
        }

        public System.Type Type
        {
            get { return this.Token.GetType(); }
        }
    }
}
