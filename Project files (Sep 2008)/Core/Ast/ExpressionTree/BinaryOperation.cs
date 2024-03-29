using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Tokens;

namespace CSharp.Compiler.Ast
{
    public class BinaryOperation : INode
    {
        Operator Operation;

        public INode Node1;
        public INode Node2;
    }
}