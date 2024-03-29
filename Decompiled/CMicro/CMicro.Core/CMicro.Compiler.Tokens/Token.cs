using System;

namespace CMicro.Compiler.Tokens
{
	public class Token<T> : IToken<T>, IToken
	{
		private T value;

		public T Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}

		public int Line { get; set; }

		public int Char { get; set; }

		public Kind Kind { get; set; }

		public bool IsIdentifier => value is Identifier;

		public bool IsKeyword => value is Keyword;

		public Token()
		{
			Line = 0;
			Char = 0;
		}

		public Token(T Value)
		{
			value = Value;
		}

		public Token(T Value, int Line, int Column)
		{
			value = Value;
			this.Line = Line;
			Char = Column;
		}

		public string GetValue()
		{
			return value.ToString();
		}

		public bool Is(IToken type)
		{
			return (object)value.GetType() == type.GetType();
		}

		public static Type Identifier()
		{
			return typeof(Identifier);
		}

		public static Type Keyword()
		{
			return typeof(Keyword);
		}

		public static Type StringLiteral()
		{
			return typeof(StringLiteral);
		}

		public static Type CharLiteral()
		{
			return typeof(CharLiteral);
		}

		public static Type IntLiteral()
		{
			return typeof(IntLiteral);
		}

		public static Type RealLiteral()
		{
			return typeof(RealLiteral);
		}

		public static Type Operator()
		{
			return typeof(Operator);
		}

		public static Type Comment()
		{
			return typeof(Comment);
		}

		public static Type EOL()
		{
			return typeof(EOL);
		}

		public static Type EOF()
		{
			return typeof(EOF);
		}
	}
}
