using System.Collections.Generic;

namespace CMicro.Compiler.Ast
{
	public class ArrayInitialization : IExpression, IOperand
	{
		public List<IExpression> Arguments = new List<IExpression>();
	}
}
