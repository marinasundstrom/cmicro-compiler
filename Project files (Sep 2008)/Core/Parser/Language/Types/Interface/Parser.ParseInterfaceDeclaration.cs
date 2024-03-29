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
        public InterfaceDeclaration ParseInterfaceDeclaration()
        {
            InterfaceDeclaration interfaced = new InterfaceDeclaration();

            this.NextIf("interface");
            interfaced.Name = this.NextIf(Kind.Identifier).GetValue();


            if (this.NextIs("implements"))
            {
                this.Next();

                while (!this.NextIs(Kind.EOL) && !this.NextIs("{"))
                {
                    IType name = this.ParseType();
                    interfaced.Implements.Add(name);

                    if (!this.NextIs(Kind.EOL) && !this.NextIs("{"))
                        this.NextIf(",");
                }
            }


            this.NextIf(Kind.EOL);
            
            this.NextIf("{");
            while (!this.NextIs("}"))
            {
                this.NextWhile(Kind.EOL);
                this.ParseInterfaceMembers(interfaced);
            }
            this.NextIf("}");

            return interfaced;
        }
    }
}