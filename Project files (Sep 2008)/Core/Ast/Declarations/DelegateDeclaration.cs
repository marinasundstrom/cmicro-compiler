using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes a delegate declaration.
    /// </summary>
    public class DelegateDeclaration : Declaration, IClassMember, IProgramDeclaration, INamespaceMember, IMethodElement
    {
        public string Name;
        public IType ReturnType;
        public Dictionary<string, CSharp.Compiler.Ast.IType> Arguments = new Dictionary<string, CSharp.Compiler.Ast.IType>();

        public Expression Expression;

        public List<IAttribute> Attributes = new List<IAttribute>();

        public bool IsGeneric;
        public List<IType> GenricParameters = new List<IType>();
    }
}
