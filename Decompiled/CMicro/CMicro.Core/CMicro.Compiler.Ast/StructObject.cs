namespace CMicro.Compiler.Ast
{
	public class StructObject : StructMember
	{
		public StructMember Member;

		public override string ToString()
		{
			return $"{Name}.{Member}";
		}
	}
}
