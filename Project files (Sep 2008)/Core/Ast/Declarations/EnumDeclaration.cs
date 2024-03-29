using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes an enumeration declaration.
    /// </summary>
    public class EnumDeclaration : Declaration, IClassMember, INamespaceMember, IMethodElement, IProgramDeclaration
    {
        public string Name;
        public Dictionary<string, int> Members = new Dictionary<string, int>();

        public List<IAttribute> Attributes;
    }
}
