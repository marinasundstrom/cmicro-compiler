using System.Collections.Generic;

namespace CMicro.Compiler.Ast
{
	public class StructDeclaration : Declaration, IProgramDeclaration, INamespaceMember, IClassMember
	{
		public string FullName;

		public List<Type> GenricParameters = new List<Type>();

		public bool IsGeneric;

		public List<IClassMember> Members = new List<IClassMember>();

		public string Name;
	}
}
