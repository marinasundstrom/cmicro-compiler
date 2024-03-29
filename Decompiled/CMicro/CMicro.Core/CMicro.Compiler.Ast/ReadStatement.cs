using System;

namespace CMicro.Compiler.Ast
{
	public class ReadStatement : Statement, IExpression, IOperand
	{
		public System.Type TypeToRead;
	}
}
