using CMicro.Compiler.Ast;

namespace CMicro.Compiler.Tokens
{
	public class Identifier : Token<string>, IExpression, IOperand
	{
		public Identifier()
		{
		}

		public Identifier(string Value)
		{
			base.Value = Value;
		}
	}
}
