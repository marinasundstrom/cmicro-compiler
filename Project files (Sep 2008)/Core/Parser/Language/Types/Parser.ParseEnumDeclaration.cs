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
        public EnumDeclaration ParseEnumDeclaration()
        {
            EnumDeclaration enumd = new EnumDeclaration();

            this.NextIf("enum");
            enumd.Name = this.NextIf(Kind.Identifier).GetValue();

            this.NextWhile(Kind.EOL);
            this.NextIf("{");
            this.NextWhile(Kind.EOL);

            int i = 0;

            #region Members

            while (!NextIs("}"))
            {                
                this.NextWhile(Kind.EOL);

                enumd.Members.Add(this.NextIf(Kind.Identifier).GetValue(), i);
                i++;

                this.NextWhile(Kind.EOL);

                if (!NextIs("}"))
                    NextIf(",");
            }

            #endregion

            this.NextWhile(Kind.EOL);
            this.NextIf("}");

            return enumd;
        }
    }
}
