using System;
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
        public List<IMethodElement> ParseCodeBlock()
        {
            List<IMethodElement> elements = new List<IMethodElement>();

            this.NextIf("{");
            while (!this.NextIs("}"))
            {
                this.Next();
            }
            this.NextIf("}");

            return elements;
        }
    }
}