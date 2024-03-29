using System.Collections.Generic;

namespace CMicro.Compiler.Ast
{
	public class Program : AstRoot
	{
		public FunctionDeclaration EntryPoint;

		public List<IProgramDeclaration> Members = new List<IProgramDeclaration>();

		public List<string> References = new List<string>();
	}
}
