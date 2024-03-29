using System.Collections.Generic;

namespace CMicro.Compiler.Ast
{
	public class WhileStatement : Statement
	{
		public IExpression Condition;

		public List<Statement> Statements = new List<Statement>();
	}
}
