namespace CMicro.Compiler.Tokens
{
	public class AND : Operator, ILogicOperator
	{
		public AND()
		{
			base.OperatorKind = OperatorKind.AND;
			base.Value = "AND";
			base.Precedence = 13;
		}
	}
	public class And : Token<object>
	{
		public And()
		{
			base.Value = new object();
			base.Kind = Kind.Other;
			base.Value = "&";
		}
	}
}
