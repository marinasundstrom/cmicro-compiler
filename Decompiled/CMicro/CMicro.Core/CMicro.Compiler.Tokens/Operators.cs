namespace CMicro.Compiler.Tokens
{
	public class Operators
	{
		public static readonly Add Add = new Add();

		public static readonly AddAssign AddAssign = new AddAssign();

		public static readonly Divide Divide = new Divide();

		public static readonly DivideAssign DivideAssign = new DivideAssign();

		public static readonly Equal Equal = new Equal();

		public static readonly Greater Greater = new Greater();

		public static readonly GreaterThanOrEqual GreaterThanOrEqual = new GreaterThanOrEqual();

		public static readonly Less Less = new Less();

		public static readonly LessThanOrEqual LessThanOrEqual = new LessThanOrEqual();

		public static readonly Modulo Modulo = new Modulo();

		public static readonly ModuloAssign ModuloAssign = new ModuloAssign();

		public static readonly Multiply Multiply = new Multiply();

		public static readonly MultiplyAssign MultiplyAssign = new MultiplyAssign();

		public static readonly Not Not = new Not();

		public static readonly NotEqual NotEqual = new NotEqual();

		public static readonly SubtractAssign SubractAssign = new SubtractAssign();

		public static readonly Subtract Subtract = new Subtract();
	}
}
