using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes a namespace declaration.
    /// </summary>
    public class NamespaceDeclaration : IProgramDeclaration, INamespaceMember
    {
        public string Name;
        public List<INamespaceMember> Members = new List<INamespaceMember>();

        public bool Internal;
    }
}
