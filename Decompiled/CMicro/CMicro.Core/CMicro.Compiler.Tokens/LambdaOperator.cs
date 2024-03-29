namespace CMicro.Compiler.Tokens
{
	public class LambdaOperator : Operator
	{
		public LambdaOperator()
		{
			base.OperatorKind = OperatorKind.Lambda;
			base.Value = "=>";
			base.Precedence = -1;
		}
	}
}
