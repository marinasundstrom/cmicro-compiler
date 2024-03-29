namespace CMicro.Compiler.Ast
{
	public class Typename : Type
	{
		public bool IsArray { get; set; }

		public bool IsPointer { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
