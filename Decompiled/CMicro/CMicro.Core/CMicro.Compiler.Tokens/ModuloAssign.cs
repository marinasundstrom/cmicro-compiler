namespace CMicro.Compiler.Tokens
{
	public class ModuloAssign : Operator
	{
		public ModuloAssign()
		{
			base.OperatorKind = OperatorKind.Modulo;
			base.Value = "%=";
			base.Precedence = 16;
		}
	}
}
