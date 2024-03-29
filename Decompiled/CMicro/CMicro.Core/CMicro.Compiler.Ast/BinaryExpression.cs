namespace CMicro.Compiler.Ast
{
	public class BinaryExpression : IExpression
	{
		public IExpression LeftSide;

		public IExpression RightSide;
	}
}
