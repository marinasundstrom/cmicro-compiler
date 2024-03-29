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
        public List<IAttribute> ParseMethodAttributes()
        {
            List<IAttribute> list = new List<IAttribute>();

            if (this.NextIs("private"))
            {
                this.Next();
                list.Add(new PrivateAttribute());
            }
            else if (this.NextIs("public"))
            {
                this.Next();
                list.Add(new PublicAttribute());
            }

            if (this.NextIs("static"))
            {
                this.Next();
                list.Add(new StaticAttribute());
            }

            return list;
        }
    }
}
