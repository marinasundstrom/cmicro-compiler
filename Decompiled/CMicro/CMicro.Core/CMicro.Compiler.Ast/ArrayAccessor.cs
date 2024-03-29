using System.Collections.Generic;

namespace CMicro.Compiler.Ast
{
	public class ArrayAccessor : Statement, IExpression, IOperand
	{
		public string Name;

		public List<IExpression> Arguments = new List<IExpression>();
	}
}
