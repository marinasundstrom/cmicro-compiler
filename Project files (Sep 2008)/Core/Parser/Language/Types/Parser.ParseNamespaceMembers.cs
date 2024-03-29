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
        public void ParseNamespaceMembers(NamespaceDeclaration nmnspace)
        {
            this.NextWhile(Kind.EOL);

            #region Private and Public attributes

            List<IAttribute> attr = this.ParsePublicPrivateAttributes();

            #endregion

            INamespaceMember member = new NullMember();

            #region Enumeration declaration
            if (NextIs("enum"))
            {
                EnumDeclaration enumdecl = this.ParseEnumDeclaration();
                enumdecl.Attributes = attr;

                member = enumdecl;

            }
            #endregion
            #region Delegate declaration
            else if (this.NextIs("delegate"))
            {
                DelegateDeclaration deld = this.ParseDelegateDeclaration();
                deld.Attributes = attr;

                member = deld;
            }
            #endregion
            #region Interface declaration
            else if (NextIs("interface"))
            {
                InterfaceDeclaration interfaced = this.ParseInterfaceDeclaration();
                interfaced.Attributes = attr;

                member = interfaced;
            }
            #endregion
            #region Class declaration
            else if (NextIs("class") || (this.CheckIndex(1, "abstract") && this.CheckIndex(2, "class")))
            {
                this.ParseAttribute("abstract", attr, false);

                ClassDeclaration classd = this.ParseClassDeclaration();
                classd.Attributes = attr;

                member = classd;
            }
            #endregion
            #region Method declaration
            else if (NextIs(Kind.Identifier, Kind.Keyword))
            {
                this.ParseAttribute("static", attr, false);

                MethodDeclaration methd = this.ParseMethodDeclaration();
                methd.Attributes = attr;

                member = methd;
            }
            #endregion

            this.NextWhile(Kind.EOL);

            #region Check

            //Check if this parse is valid, if not report an error.
            if (member is NullMember && attr.Count > 0)
                this.ReportSyntaxError("Unexpected attribute", this.CurrentToken, true);

            #endregion

            nmnspace.Members.Add(member);
        }
    }
}
