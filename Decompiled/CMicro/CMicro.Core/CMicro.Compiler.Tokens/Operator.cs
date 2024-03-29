using CMicro.Compiler.Ast;

namespace CMicro.Compiler.Tokens
{
	public abstract class Operator : Token<object>, IExpression
	{
		private OperatorKind operatorkind;

		public OperatorKind OperatorKind
		{
			get
			{
				return OperatorKind;
			}
			set
			{
				operatorkind = value;
			}
		}

		public int Precedence { get; set; }

		public Operator()
		{
			base.Value = new object();
			base.Kind = Kind.Operator;
		}
	}
}
