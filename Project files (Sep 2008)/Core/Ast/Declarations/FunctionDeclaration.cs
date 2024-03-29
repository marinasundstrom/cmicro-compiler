using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes a function declaration.
    /// </summary>
    public class FunctionDeclaration
    {
        public CSharp.Compiler.Tokens.IToken ReturnType;
        public string Name;
        public Dictionary<string, CSharp.Compiler.Ast.Type> Parameters = new Dictionary<string, CSharp.Compiler.Ast.Type>();

        //public Expression Statement;
    }
}
