using System.Collections.Generic;

namespace CMicro.Compiler.Ast
{
	public class IfStatement : Statement
	{
		public IExpression Condition;

		public List<Statement> ElseStatements = new List<Statement>();

		public List<Statement> Statements = new List<Statement>();
	}
}
