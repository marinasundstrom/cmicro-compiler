using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// The defined root of the program and output of the parser. (Inherits AstRoot)
    /// </summary>
    public class Program : AstRoot
    {
        public List<string> References = new List<string>();
        public List<IProgramDeclaration> Members = new List<IProgramDeclaration>();

        public MethodDeclaration EntryPoint;
    }
}
