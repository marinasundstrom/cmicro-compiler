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
        public void ParseClassMembers(ClassDeclaration classd)
        {
            this.NextWhile(Kind.EOL);

            #region Attributes
            
            List<IAttribute> attr = this.ParsePublicPrivateAttributes();
            this.ParseAttribute("static", attr, false);

            #endregion

            IClassMember member = new NullMember();

            #region Enumeration declaration
            if (NextIs("enum"))
            {
                if (attr.Any(x => x is ConstantAttribute) || attr.Any(x => x is StaticAttribute))
                    this.ReportSyntaxError("Syntax error: Enumeration cannot be declared as static or constant", this.CurrentToken, true);

                EnumDeclaration enumdecl = this.ParseEnumDeclaration();
                enumdecl.Attributes = attr;

                member = enumdecl;

            }
            #endregion
            else if (NextIs(Kind.Identifier, Kind.Keyword))
            {
                this.ParseAttribute("const", attr, false);

                #region Delegate declaration
                if (this.NextIs("delegate"))
                {
                    if (attr.Any(n => n is ConstantAttribute))
                        this.ReportSyntaxError("Syntax error: A delegate cannot be declared as constant,", this.CurrentToken, true);

                    DelegateDeclaration deld = this.ParseDelegateDeclaration();
                    deld.Attributes = attr;

                    member = deld;
                }
                #endregion
                #region Nested Class declaration
                else if (this.NextIs("class"))
                {
                    if (attr.Any(n => n is ConstantAttribute))
                        this.ReportSyntaxError("Syntax error: A delegate cannot be declared as constant,", this.CurrentToken, true);

                    ClassDeclaration dclass = this.ParseClassDeclaration();

                    dclass.Attributes = attr;

                    member = dclass;
                }
                #endregion
                #region Nested Interface declaration
                else if (this.NextIs("interface"))
                {
                    if (attr.Any(n => n is ConstantAttribute))
                        this.ReportSyntaxError("Syntax error: A delegate cannot be declared as constant,", this.CurrentToken, true);
                    
                    InterfaceDeclaration interfaced = this.ParseInterfaceDeclaration();

                    interfaced.Attributes = attr;

                    member = interfaced;
                }
                #endregion
                #region Field declaration
                else if (!attr.Any(n => n is StaticAttribute) && attr.Any(n => n is ConstantAttribute))
                {                  
                    //Expect a field declaration

                    FieldVariableDeclaration fieldd = this.ParseFieldVariableDeclaration();
                    fieldd.Attributes = attr;
                    member = fieldd;
                    
                }
                #endregion
                else
                {
                    if (this.CheckIndex(2, "("))
                    {
                        #region Constructor declaration
                        //Constructor declaration

                        ConstructorDeclaration constrd = this.ParseConstructorDeclaration();
                        constrd.Attributes = attr;

                        member = constrd;
                        #endregion
                    }
                    else
                    {
                        #region Method declaration
                        IType type = this.ParseType();
                        IToken name = this.NextIf(Kind.Identifier, Kind.Keyword);

                        if (NextIs("("))
                        {
                            //Method declaration

                            #region Report if name is a keyword

                            if (name is Keyword)
                                this.ReportSyntaxError("Syntax error: Unexpected keyword", name, true);

                            #endregion

                            MethodDeclaration decl = new MethodDeclaration();
                            decl.Attributes = attr;
                            decl.ReturnType = type;
                            decl.Name = name.GetValue();
                            decl.Arguments = this.ParseMethodArguments();

                            this.NextWhile(Kind.EOL);
                            decl.Code = this.ParseCodeBlock();
                            this.IsMainMethod(decl);

                            member = decl;
                        }
                        #endregion
                        #region Generic method declaration
                        else if (NextIs("<"))
                        {
                            //Generic method declaration

                            #region Report if name is a keyword

                            if (name is Keyword)
                                this.ReportSyntaxError("Syntax error: Unexpected keyword", name, true);

                            #endregion

                            MethodDeclaration decl = new MethodDeclaration();
                            decl.Attributes = attr;
                            decl.ReturnType = type;
                            decl.Name = name.GetValue();

                            if (this.NextIs("<"))
                            {
                                this.Next();

                                while (!this.NextIs(">"))
                                {
                                    decl.GenricParameters.Add(this.ParseType());

                                    if (!this.NextIs(">"))
                                        this.NextIf(",");
                                }

                                decl.IsGeneric = true;

                                this.Next();
                            }

                            decl.Arguments = this.ParseMethodArguments();

                            this.NextWhile(Kind.EOL);
                            decl.Code = this.ParseCodeBlock();
                            this.IsMainMethod(decl);

                            member = decl;
                        }
#endregion
                        #region Property declaration
                        else if (this.NextIs(Kind.EOL) || this.NextIs("{"))
                        {
                            //Property Declaration

                            #region Report if name is a keyword

                            if (name is Keyword)
                                this.ReportSyntaxError("Syntax error: Unexpected keyword", name, true);

                            #endregion

                            PropertyDeclaration prop = new PropertyDeclaration();

                            prop.ReturnType = type;
                            prop.Name = name.GetValue();

                            this.NextWhile(Kind.EOL);


                            this.NextIf("{");

                            while (!this.NextIs("}"))
                            {
                                this.NextWhile(Kind.EOL);

                                if (this.NextIs("set") && prop.Set == null)
                                {
                                    this.Next();
                                    
                                    this.NextWhile(Kind.EOL);
                                    prop.Set = this.ParseCodeBlock();
                                }
                                else if (this.NextIs("set") && prop.Set != null)
                                    this.ReportSyntaxError("Syntax error: Setter already declared", this.CurrentToken, true);
                                else if (this.NextIs("get") && prop.Get == null)
                                {
                                    this.Next();
                                    
                                    this.NextWhile(Kind.EOL);
                                    prop.Get = this.ParseCodeBlock();
                                }
                                else if (this.NextIs("get") && prop.Get != null)
                                    this.ReportSyntaxError("Syntax error: Getter already declared", this.CurrentToken, true);


                                this.NextWhile(Kind.EOL);
                            }
                            this.NextIf("}");

                            this.NextWhile(Kind.EOL);

                            prop.Attributes = attr;

                            member = prop;
                        }
                        #endregion
                        #region Indexer declaration
                        else if (name.GetValue() == "this" && this.CheckIndex(1, "["))
                        {
                            //Indexer Declaration

                            IndexerDeclaration indexd = new IndexerDeclaration();

                            indexd.ReturnType = type;

                            this.Next();

                            while (!this.NextIs("]"))
                            {
                                IType itype = this.ParseType();
                                string iname = this.NextIf(Kind.Identifier).GetValue();
                               
                                indexd.IndexerArgs.Add(iname, itype);

                                if (!this.NextIs("]"))
                                    this.NextIf(",");
                            }
                            this.Next();


                            this.NextWhile(Kind.EOL);


                            this.NextIf("{");

                            while (!this.NextIs("}"))
                            {
                                this.NextWhile(Kind.EOL);

                                if (this.NextIs("set") && indexd.Set == null)
                                {
                                    this.Next();

                                    this.NextWhile(Kind.EOL);
                                    indexd.Set = this.ParseCodeBlock();
                                }
                                else if (this.NextIs("set") && indexd.Set != null)
                                    this.ReportSyntaxError("Syntax error: Setter already declared", this.CurrentToken, true);
                                else if (this.NextIs("get") && indexd.Get == null)
                                {
                                    this.Next();

                                    this.NextWhile(Kind.EOL);
                                    indexd.Get = this.ParseCodeBlock();
                                }
                                else if (this.NextIs("get") && indexd.Get != null)
                                    this.ReportSyntaxError("Syntax error: Getter already declared", this.CurrentToken, true);


                                this.NextWhile(Kind.EOL);
                            }
                            this.NextIf("}");

                            this.NextWhile(Kind.EOL);

                            indexd.Attributes = attr;

                            member = indexd;
                        }
                        #endregion
                        #region Field delcaration
                        else if (NextIs(";") || NextIs("="))
                        {
                            //Field declaration

                            #region Report if name is a keyword

                            if (name is Keyword)
                                this.ReportSyntaxError("Syntax error: Unexpected keyword", name, true);

                            #endregion

                            FieldVariableDeclaration decl = new FieldVariableDeclaration();
                            decl.Attributes = attr;
                            decl.Type = type;
                            decl.Name = name.GetValue();

                            this.NextIf(";");

                            member = decl;
                        }
                        #endregion
                    }
                }
            }

            #region Check

            //Check if this parse is valid, if not report an error.
            if (member is NullMember && attr.Count > 0)
                this.ReportSyntaxError("Unexpected attribute", this.CurrentToken, true);

            #endregion

            if (!(member is NullMember))
                classd.Members.Add(member);
        }
    }
}
