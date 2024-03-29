using System.Collections.Generic;

namespace CMicro.Compiler.Ast
{
	public class ForStatement : Statement
	{
		public Statement Initializer;

		public IExpression Condition;

		public Statement Action;

		public List<Statement> Statements = new List<Statement>();
	}
}
