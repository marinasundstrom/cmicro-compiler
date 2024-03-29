using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes a constructor method (or initializer).
    /// </summary>
    /// 
    public class ConstructorDeclaration : Declaration, IClassMember, IProgramDeclaration, INamespaceMember
    {
        public string Name;
        public Dictionary<string, CSharp.Compiler.Ast.IType> Arguments = new Dictionary<string, CSharp.Compiler.Ast.IType>();

        public List<IMethodElement> Code = new List<IMethodElement>();

        public List<IAttribute> Attributes = new List<IAttribute>();
    }
}
