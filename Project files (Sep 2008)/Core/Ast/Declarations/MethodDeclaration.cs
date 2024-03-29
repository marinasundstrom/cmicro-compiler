using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes a method declaration.
    /// </summary>
    public class MethodDeclaration : Declaration, IClassMember, IProgramDeclaration, INamespaceMember
    {
        public string Name;
        public IType ReturnType;
        public Dictionary<string, CSharp.Compiler.Ast.IType> Arguments = new Dictionary<string, CSharp.Compiler.Ast.IType>();

        public List<IMethodElement> Code = new List<IMethodElement>();

        public List<IAttribute> Attributes = new List<IAttribute>();

        public bool IsGeneric;
        public List<IType> GenricParameters = new List<IType>();
    }
}
