using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Ast;

namespace CSharp.Compiler.Tokens
{
    public class Identifier : Token<string>, INode
    {
        public Identifier()
        {

        }

        public Identifier(string Value)
        {
            this.Value = Value;
        }
    }
}
