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
        private void IsMainMethod(MethodDeclaration methd)
        {
            if (methd.Name == "main" && ((Program)this.root).EntryPoint == null)
            {
                ((Program)this.root).EntryPoint = methd;
            }
            else if (methd.Name == "main" && ((Program)this.root).EntryPoint != null)
            {
                this.ReportSyntaxError("Program can only have one main method.", this.CurrentToken, false);
            }
        }
    }
}