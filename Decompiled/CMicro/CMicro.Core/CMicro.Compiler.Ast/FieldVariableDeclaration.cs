namespace CMicro.Compiler.Ast
{
	public class FieldVariableDeclaration : Declaration, IClassMember
	{
		public IExpression IExpression;

		public string Name;

		public string Type;
	}
}
