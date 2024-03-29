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
        public List<IAttribute> ParseAttributes()
        {
            return this.ParseAttributes(true, true, true);
        }

        public List<IAttribute> ParseAttributes(bool Static, bool Constant, bool Abstract)
        {
            List<IAttribute> list = new List<IAttribute>();

            IAttribute attr = new PublicAttribute();

            while (this.NextIs("private") || this.NextIs("public") || this.NextIs("const") || this.NextIs("static") || this.NextIs("abstract"))
            {
                if (this.NextIs("private"))
                {
                    this.Next();
                    attr = new PrivateAttribute();
                }
                else if (this.NextIs("public"))
                {
                    this.Next();
                    attr = new PublicAttribute();
                }
                else if (this.NextIs("const") && Constant == true)
                {
                    this.Next();
                    attr = new ConstantAttribute();
                }
                else if (this.NextIs("static") && Static == true)
                {
                    this.Next();
                    attr = new StaticAttribute();
                }
                else if (this.NextIs("abstract") && Abstract == true)
                {
                    this.Next();
                    attr = new AbstractAttribute();
                }
                else
                    this.ReportSyntaxError("Syntax error: Unexpected attribute", this.CurrentToken, true);

                if (!list.Any(n => n.GetType() == attr.GetType()))
                    list.Add(attr);
                else
                    this.ReportSyntaxError("Syntax error: Declaration already has that attribute,", this.CurrentToken, true);
            }

            return list;
        }
    }
}
