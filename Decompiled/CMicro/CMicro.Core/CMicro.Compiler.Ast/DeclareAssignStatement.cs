namespace CMicro.Compiler.Ast
{
	public class DeclareAssignStatement : Statement
	{
		public IExpression Expression;

		public string Name;

		public Typename Type;

		public bool Global;

		public bool Constant;
	}
}
