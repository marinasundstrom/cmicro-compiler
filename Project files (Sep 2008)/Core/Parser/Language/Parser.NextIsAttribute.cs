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
        public bool NextIsAttribute()
        {
            if (this.NextIs("public") || this.NextIs("private") || this.NextIs("static"))
            {
                return true;
            }
            else
                return false;
        }
    }
}
