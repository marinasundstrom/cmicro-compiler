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
        public Dictionary<string, CSharp.Compiler.Ast.IType> ParseMethodArguments()
        {
            Dictionary<string, CSharp.Compiler.Ast.IType> args = new Dictionary<string, CSharp.Compiler.Ast.IType>();

            /*
             *  System.String text, Char ch
             * 
             * */

            NextIf("(");
            while (!NextIs(")"))
            {
                IType type = this.ParseType();
                string name  = this.NextIf(Kind.Identifier).GetValue();

                args.Add(name, type);

                if (!NextIs(")"))
                    NextIf(",");
            }
            this.NextIf(")");

            return args;
        }
    }
}