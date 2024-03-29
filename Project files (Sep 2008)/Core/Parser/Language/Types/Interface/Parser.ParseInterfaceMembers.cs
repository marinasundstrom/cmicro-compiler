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
        public void ParseInterfaceMembers(InterfaceDeclaration interfaced)
        {

            this.NextWhile(Kind.EOL);

            IInterfaceMember member = new NullMember();

            if (this.NextIs(Kind.Identifier, Kind.Keyword))
            {
                InterfaceMethod methd = new InterfaceMethod();
                methd.ReturnType = this.ParseType();
                methd.Name = this.NextIf(Kind.Identifier).GetValue();

                if (this.NextIs(Kind.EOL) || this.NextIs("{"))
                {
                    #region Property

                    InterfaceProperty prop = new InterfaceProperty();
                    prop.Name = methd.Name;
                    prop.ReturnType = methd.ReturnType;

                    this.NextWhile(Kind.EOL);
                    this.NextIf("{");

                    while (!this.NextIs("}"))
                    {
                        if (this.NextIs("get"))
                        {
                            this.Next();
                            this.NextIf(";");

                            prop.HasGetter = true;
                        }
                        else if (this.NextIs("set"))
                        {
                            this.Next();
                            this.NextIf(";");

                            prop.HasSetter = true;
                        }

                        this.NextWhile(Kind.EOL);
                    }

                    this.NextIf("}");

                    member = prop; ;

                    #endregion
                }
                else
                {
                    #region Method

                    if (this.NextIs("<"))
                    {
                        this.Next();

                        while (!this.NextIs(">"))
                        {
                            methd.GenericParameters.Add(this.ParseType());

                            if (!this.NextIs(">"))
                                this.NextIf(",");
                        }

                        this.Next();

                        methd.IsGeneric = true;
                    }

                    methd.Arguments = this.ParseMethodArguments();

                    this.NextIf(";");

                    member = methd;

                    #endregion
                }
            }

            this.NextWhile(Kind.EOL);

            if(!(member is NullMember))
                interfaced.Members.Add(member);
        }

    }
}