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
        public void ParseAttribute(string Attribute, List<IAttribute> To, bool ThrowExcOnFail)
        {
            if (this.NextIs(Attribute))
            {
                if (this.NextIs("private"))
                {
                    this.Next();
                    To.Add(new PrivateAttribute());
                }
                else if (this.NextIs("public"))
                {
                    this.Next();
                    To.Add(new PublicAttribute());
                }
                else if (this.NextIs("const"))
                {
                    this.Next();
                    To.Add(new ConstantAttribute());
                }
                else if (this.NextIs("static"))
                {
                    this.Next();
                    To.Add(new StaticAttribute());
                }
                else if (this.NextIs("abstract"))
                {
                    this.Next();
                    To.Add(new AbstractAttribute());
                }
                else if (this.NextIs("override"))
                {
                    this.Next();
                    To.Add(new OverrideAttribute());
                }
                else if (this.NextIs("virtual"))
                {
                    this.Next();
                    To.Add(new VirtualAttribute());
                }
            }
            else if (ThrowExcOnFail)
                this.ReportSyntaxError(string.Format("Syntax error, attribute {0}", Attribute), this.CurrentToken, true);
        }
    }
}
