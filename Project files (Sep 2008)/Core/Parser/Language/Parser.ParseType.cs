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
        public CSharp.Compiler.Ast.IType ParseType()
        {
            string name = this.ParseName();

            if (this.NextIs("["))
            {
                ArrayType arrtype = new ArrayType();
                arrtype.Type = new CSharp.Compiler.Ast.Type(name);

                this.Next();

                while (!this.NextIs("]"))
                {
                    this.NextIf("*");

                    arrtype.Dimensions++;

                    if (!this.NextIs("]"))
                        this.NextIf(",");
                }

                this.Next();

                if (this.NextIs("*"))
                {
                    this.Next();
                    arrtype.IsPointer = true;
                }

                return arrtype;
            }
            else if (this.NextIs("<"))
            {
                GenericType gentype = new GenericType();
                gentype.Type = new Ast.Type(name);

                this.Next();

                while (!this.NextIs(">"))
                {
                    gentype.GenericParameters.Add(this.ParseType());

                    if (!this.NextIs(">"))
                        this.NextIf(",");
                }

                this.Next();

                return gentype;
            }
            else
            {
                var typename = new CSharp.Compiler.Ast.Type(name);

                if (this.NextIs("*"))
                {
                    this.Next();
                    typename.IsPointer = true;
                }
                return typename;
            }
        }
    }
}
