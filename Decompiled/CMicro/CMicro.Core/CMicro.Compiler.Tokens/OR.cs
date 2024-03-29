namespace CMicro.Compiler.Tokens
{
	public class OR : Operator, ILogicOperator
	{
		public OR()
		{
			base.OperatorKind = OperatorKind.OR;
			base.Value = "OR";
			base.Precedence = 14;
		}
	}
}
