﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using CSharp.Compiler.Tokens;
using CSharp.Compiler.Ast;

namespace CSharp.Compiler
{
    public partial class Parser : IParser
    {
        public INode ParseExpressionTree(INode n, List<IToken> l)
        {
            return new Operand();
        }
    }
}