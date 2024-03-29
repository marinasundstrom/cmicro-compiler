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
        public void ParseCases()
        {
            switch (this.LookaheadToken.Kind)
            {
                case Kind.EOF:
                    //this.Abort();
                    break;

                case Kind.EOL:
                    Next();
                    break;

                default:
                    this.ReportSyntaxError("Expected class, struct, interface or enum definition", this.CurrentToken, true);
                    break;
            }
        }
    }
}
