namespace CMicro.Compiler.Ast
{
	public class VariableDeclaration : Declaration, IMethodElement
	{
		public IExpression IExpression;

		public bool IsPointer;

		public string Name;

		public string TypeName;
	}
}
