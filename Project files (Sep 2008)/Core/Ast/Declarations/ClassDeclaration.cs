using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes a class declaration.
    /// </summary>
    public class ClassDeclaration : Declaration, IProgramDeclaration, INamespaceMember, IClassMember
    {
        public string Name;
        public IType Inherits;
        public List<IType> Implements = new List<IType>();

        public List<IClassMember> Members = new List<IClassMember>();

        public List<IAttribute> Attributes = new List<IAttribute>();

        public bool IsGeneric;
        public List<IType> GenricParameters = new List<IType>();
    }
}
