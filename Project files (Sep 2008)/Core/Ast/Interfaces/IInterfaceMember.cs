using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    public interface IInterfaceMember
    {

    }

    public class InterfaceMethod : IInterfaceMember
    {
        public string Name;
        public IType ReturnType;
        public Dictionary<string, IType> Arguments = new Dictionary<string, IType>();

        public bool IsGeneric;
        public List<IType> GenericParameters = new List<IType>();
    }

    public class InterfaceProperty : IInterfaceMember
    {
        public string Name;
        public IType ReturnType;

        public bool HasSetter;
        public bool HasGetter;
    }
}
