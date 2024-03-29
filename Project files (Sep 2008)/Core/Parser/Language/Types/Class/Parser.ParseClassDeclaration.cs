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
        public ClassDeclaration ParseClassDeclaration()
        {
            ClassDeclaration classd = new ClassDeclaration();

            //classd.Attributes = this.ParseClassAttributes();

            /*
             * [public/private] [abstract] class <Name>
             * {
             *    <Members>
             * }
             * 
             * */

            this.NextIf("class");
            classd.Name = this.NextIf(Kind.Identifier).GetValue();

            if(this.NextIs("<"))
            {
                this.Next();

                while (!this.NextIs(">"))
                {
                    classd.GenricParameters.Add(this.ParseType());

                    if (!this.NextIs(">"))
                        this.NextIf(",");
                }

                this.Next();

                classd.IsGeneric = true;
            }

            if (this.NextIs("extends"))
            {
                this.Next();
                classd.Inherits = this.ParseType();
            }

            if (this.NextIs("implements"))
            {
                this.Next();

                while (!this.NextIs(Kind.EOL) && !this.NextIs("{"))
                {
                    IType name = this.ParseType();
                    classd.Implements.Add(name);

                    if (!this.NextIs(Kind.EOL) && !this.NextIs("{"))
                        this.NextIf(",");
                }
            }

            this.NextWhile(Kind.EOL);

            this.NextIf("{");
            while (!this.NextIs("}"))
            {
                this.NextWhile(Kind.EOL);
                this.ParseClassMembers(classd);
            }
            this.NextIf("}");

            return classd;
        }
    }
}

