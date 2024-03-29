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
        public DelegateDeclaration ParseDelegateDeclaration()
        {
            DelegateDeclaration deld = new DelegateDeclaration();

            this.NextIf("delegate");

            deld.ReturnType = this.ParseType();

            deld.Name = this.NextIf(Kind.Identifier).GetValue();

            #region Generic parameters

            if (this.NextIs("<"))
            {
                this.Next();

                while (!this.NextIs(">"))
                {
                    deld.GenricParameters.Add(this.ParseType());

                    if (!this.NextIs(">"))
                        this.NextIf(",");
                }

                this.Next();

                deld.IsGeneric = true;
            }

            #endregion

            deld.Arguments = this.ParseMethodArguments();

            this.NextIf(";");

            this.NextWhile(Kind.EOL);

            return deld;
        }
    }
}