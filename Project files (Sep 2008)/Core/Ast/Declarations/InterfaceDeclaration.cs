using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    public class InterfaceDeclaration : INamespaceMember, IProgramDeclaration, IClassMember
    {
        public string Name;

        public List<IInterfaceMember> Members = new List<IInterfaceMember>();
        public List<IAttribute> Attributes;

        public List<IType> Implements = new List<IType>();
    }
}
