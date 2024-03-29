using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes a property declaration.
    /// </summary>
    public class PropertyDeclaration : Declaration, IClassMember
    {
        public string Name;

        public IType ReturnType;

        public List<IMethodElement> Set;
        public List<IMethodElement> Get;

        public List<IAttribute> Attributes = new List<IAttribute>();
    }
}
