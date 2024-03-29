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
        public string ParseImportStatement()
        {
            string import;

            NextIf("import");
            import = this.ParseName();
            NextIf(";");

            return import;
        }
    }
}
