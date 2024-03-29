namespace CMicro.Compiler.Ast
{
	public class TypeOf : Identifier, IExpression, IOperand
	{
		public Type Operand;
	}
}
