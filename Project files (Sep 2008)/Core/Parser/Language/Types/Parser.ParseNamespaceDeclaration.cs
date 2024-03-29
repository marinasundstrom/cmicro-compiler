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
        public NamespaceDeclaration ParseNamespaceDeclaration()
        {
            NamespaceDeclaration namespaced = new NamespaceDeclaration();

            /*
             * namespace <Name>
             * {
             *    <Members>
             * }
             * 
             * */

            this.NextIf("namespace");
            namespaced.Name = this.NextIf(Kind.Identifier).GetValue();

            this.NextWhile(Kind.EOL);

            this.NextIf("{");
            while (!this.NextIs("}"))
            {
                this.NextWhile(Kind.EOL);
                this.ParseNamespaceMembers(namespaced);
            }
            this.NextIf("}");

            return namespaced;
        }
    }
}

