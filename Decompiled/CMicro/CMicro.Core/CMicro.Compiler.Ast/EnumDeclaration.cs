using System.Collections.Generic;

namespace CMicro.Compiler.Ast
{
	public class EnumDeclaration : Declaration, IClassMember, INamespaceMember, IMethodElement, IProgramDeclaration
	{
		public string FullName;

		public Dictionary<string, int> Members = new Dictionary<string, int>();

		public string Name;
	}
}
