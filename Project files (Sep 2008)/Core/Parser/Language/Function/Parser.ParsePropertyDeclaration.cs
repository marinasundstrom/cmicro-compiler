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
        public PropertyDeclaration ParsePropertyDeclaration()
        {
            PropertyDeclaration prop = new PropertyDeclaration();

            prop.ReturnType = this.ParseType();

            prop.Name = this.NextIf(Kind.Identifier).GetValue();

           
            while(!this.NextIs("}"))
            {
                this.NextWhile(Kind.EOL);

                if(this.NextIs("set") && prop.Set == null)
                {
                    prop.Get = this.ParseCodeBlock();
                }
                else if (this.NextIs("set") && prop.Set != null)
                    this.ReportSyntaxError("Syntax error: Setter already declared", this.CurrentToken, true);
                else if (this.NextIs("get") && prop.Get == null)
                {
                    prop.Get = this.ParseCodeBlock();
                }
                else if (this.NextIs("get") && prop.Get != null)
                    this.ReportSyntaxError("Syntax error: Getter already declared", this.CurrentToken, true);
                
                
                this.NextWhile(Kind.EOL);
            }
            this.NextIf("}");

            return prop;
        }
    }
}