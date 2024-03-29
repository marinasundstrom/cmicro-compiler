using System.Collections.Generic;
using CMicro.Compiler.Tokens;

namespace CMicro.Compiler.Ast
{
	public class FunctionDeclaration : Declaration, IClassMember, IProgramDeclaration, INamespaceMember
	{
		public List<IToken> _statementTokenStore = new List<IToken>();

		public Dictionary<string, Type> Parameters = new Dictionary<string, Type>();

		public List<Statement> Code = new List<Statement>();

		public string Name;

		public Type ReturnType;
	}
}
