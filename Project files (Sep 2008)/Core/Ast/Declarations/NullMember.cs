using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Dummy class that symbolizes a null value of a declaration.
    /// </summary>
    public class NullMember : IClassMember, INamespaceMember, IProgramDeclaration, IInterfaceMember
    {
    }
}
