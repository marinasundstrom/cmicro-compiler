using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CMicro.Compiler.Tokens;

namespace CMicro.Compiler
{
	public class Scanner
	{
		private readonly bool ignoreCase;

		private readonly TextReader reader;

		private readonly string[] reservedWords;

		private int ch;

		private int ln;

		private int tokenCh;

		private List<char> delimiters;

		private List<char> operators;

		public static string[] Keywords = new string[21]
		{
			"int", "string", "double", "bool", "char", "enum", "var", "void", "null", "true",
			"false", "return", "import", "struct", "global", "delete", "const", "if", "else", "while",
			"for"
		};

		public int Line => ln;

		public int Char => ch;

		private char LookaheadChar => (char)reader.Peek();

		public Scanner(TextReader reader)
		{
			this.reader = reader;
			reservedWords = new string[0];
			ignoreCase = false;
			ln = 1;
			ch = 0;
		}

		public Scanner(TextReader reader, string[] reservedWords, bool ignoreCase)
		{
			this.reader = reader;
			this.reservedWords = reservedWords;
			this.ignoreCase = ignoreCase;
			ln = 1;
			ch = 0;
		}

		public IToken Next()
		{
			IToken result = new Token<object>();
			while (LookaheadChar == ' ')
			{
				reader.Read();
				ch++;
			}
			if (reader.Peek() < 0)
			{
				ch++;
				tokenCh = ch;
				result = new EOF();
				result.Char = tokenCh;
				result.Line = ln;
				return result;
			}
			while (LookaheadChar != ' ' && reader.Peek() > -1)
			{
				char lookaheadChar = LookaheadChar;
				if (char.IsLetter(lookaheadChar) || lookaheadChar == '_' || lookaheadChar == '@')
				{
					StringBuilder stringBuilder = new StringBuilder(((char)reader.Read()).ToString());
					ch++;
					tokenCh = ch;
					lookaheadChar = LookaheadChar;
					while (char.IsLetterOrDigit(lookaheadChar) || (lookaheadChar == '_' && lookaheadChar != ' ' && reader.Peek() > -1))
					{
						stringBuilder.Append((char)reader.Read());
						ch++;
						lookaheadChar = LookaheadChar;
					}
					int i = 0;
					bool flag = false;
					for (; i < reservedWords.Length; i++)
					{
						if (flag)
						{
							break;
						}
						if (string.Compare(reservedWords[i], stringBuilder.ToString(), ignoreCase) == 0)
						{
							flag = true;
						}
					}
					result = ((!flag) ? ((IToken)new Identifier(stringBuilder.ToString())) : ((IToken)new Keyword(stringBuilder.ToString())));
					result.Char = tokenCh;
					result.Line = Line;
					return result;
				}
				if (char.IsDigit(lookaheadChar))
				{
					string text = ((char)reader.Read()).ToString();
					ch++;
					tokenCh = ch;
					bool flag2 = false;
					lookaheadChar = LookaheadChar;
					while (char.IsDigit(lookaheadChar) || lookaheadChar == '.')
					{
						if (char.IsDigit(lookaheadChar))
						{
							text += (char)reader.Read();
							ch++;
							lookaheadChar = LookaheadChar;
							continue;
						}
						if (lookaheadChar == '.')
						{
							text += (char)reader.Read();
							ch++;
							flag2 = true;
							lookaheadChar = LookaheadChar;
							while (lookaheadChar != ';' && lookaheadChar != ' ' && !IsOperator(lookaheadChar) && !IsDelimiter(lookaheadChar) && lookaheadChar != ':' && lookaheadChar != ',' && char.IsDigit(lookaheadChar) && reader.Peek() > -1)
							{
								text += (char)reader.Read();
								ch++;
								lookaheadChar = LookaheadChar;
							}
							if (LookaheadChar != '.')
							{
								continue;
							}
							throw new Exception($"Syntax error at ({ch}, {ln}).");
						}
						throw new Exception($"Syntax error at ({ch}, {ln}).");
					}
					if (flag2)
					{
						text = text.Replace('.', ',');
						result = new RealLiteral(double.Parse(text));
					}
					else
					{
						result = new IntLiteral(int.Parse(text));
					}
					result.Char = tokenCh;
					result.Line = Line;
					return result;
				}
				switch (lookaheadChar)
				{
				case '"':
				{
					reader.Read();
					ch++;
					tokenCh = ch;
					StringBuilder stringBuilder2 = new StringBuilder();
					for (lookaheadChar = (char)reader.Peek(); lookaheadChar != '"'; lookaheadChar = (char)reader.Peek())
					{
						stringBuilder2.Append(lookaheadChar);
						reader.Read();
						ch++;
						if (reader.Peek() == -1)
						{
							throw new Exception($"Syntax error: Unterminated string literal at ({ch}, {ln}).");
						}
					}
					ch++;
					reader.Read();
					result = new StringLiteral(stringBuilder2.ToString());
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				}
				case '\'':
				{
					reader.Read();
					ch++;
					tokenCh = ch;
					StringBuilder stringBuilder2 = new StringBuilder();
					int num = 0;
					bool flag3 = false;
					for (lookaheadChar = LookaheadChar; lookaheadChar != '\''; lookaheadChar = (char)reader.Peek())
					{
						stringBuilder2.Append(lookaheadChar);
						if (lookaheadChar == '\\')
						{
							if (flag3)
							{
								num++;
							}
							else
							{
								flag3 = true;
							}
						}
						else
						{
							num++;
						}
						if (num > 1)
						{
							throw new Exception($"Syntax error: Invalid character literal at ({ch}, {ln}).");
						}
						reader.Read();
						ch++;
					}
					ch++;
					reader.Read();
					result = new CharLiteral(stringBuilder2[0]);
					result.Char = tokenCh;
					result.Line = Line;
					return result;
				}
				}
				switch (lookaheadChar)
				{
				case '\n':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new EOL();
					result.Char = tokenCh;
					result.Line = ln;
					ln++;
					ch = 0;
					return result;
				case '\r':
					reader.Read();
					break;
				case '\t':
					reader.Read();
					ch += 4;
					break;
				case ',':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new Comma();
					result.Char = tokenCh;
					result.Line = ln;
					ln++;
					return result;
				case ';':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new Semicolon();
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case ':':
					reader.Read();
					ch++;
					result = new Colon();
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '.':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new Period();
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '=':
					reader.Read();
					ch++;
					tokenCh = ch;
					if ('=' == LookaheadChar)
					{
						reader.Read();
						result = new Equal();
						ch++;
					}
					else if ('>' == LookaheadChar)
					{
						reader.Read();
						result = new LambdaOperator();
						ch++;
					}
					else
					{
						result = new Assign();
					}
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '!':
					reader.Read();
					ch++;
					tokenCh = ch;
					if ('=' == LookaheadChar)
					{
						reader.Read();
						result = new NotEqual();
						ch++;
					}
					else
					{
						result = new Not();
					}
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '+':
					reader.Read();
					ch++;
					tokenCh = ch;
					if ('=' == LookaheadChar)
					{
						reader.Read();
						result = new AddAssign();
						ch++;
					}
					else
					{
						result = new Add();
					}
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '-':
					reader.Read();
					ch++;
					tokenCh = ch;
					if ('=' == LookaheadChar)
					{
						reader.Read();
						result = new SubtractAssign();
						ch++;
					}
					else
					{
						result = new Subtract();
					}
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '/':
					reader.Read();
					ch++;
					tokenCh = ch;
					if ('=' == LookaheadChar)
					{
						reader.Read();
						result = new DivideAssign();
						ch++;
					}
					else if ('/' == LookaheadChar)
					{
						reader.Read();
						StringBuilder stringBuilder2 = new StringBuilder();
						while (LookaheadChar != '\n')
						{
							stringBuilder2.Append(LookaheadChar);
							reader.Read();
							ch++;
						}
						result = new Comment(stringBuilder2.ToString());
					}
					else if ('*' == LookaheadChar)
					{
						reader.Read();
						StringBuilder stringBuilder2 = new StringBuilder();
						bool flag4 = true;
						while (flag4 && reader.Peek() > -1)
						{
							if (LookaheadChar == '\n')
							{
								ln++;
							}
							else
							{
								stringBuilder2.Append(LookaheadChar);
							}
							ch++;
							reader.Read();
							if (LookaheadChar == '*')
							{
								reader.Read();
								ch++;
								if (LookaheadChar == '/')
								{
									flag4 = false;
									reader.Read();
									ch++;
								}
							}
							else if (LookaheadChar < '\0')
							{
								flag4 = false;
							}
						}
						result = new Comment(stringBuilder2.ToString());
					}
					else
					{
						result = new Divide();
					}
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '*':
					reader.Read();
					ch++;
					tokenCh = ch;
					if ('=' == LookaheadChar)
					{
						reader.Read();
						result = new MultiplyAssign();
						ch++;
					}
					else
					{
						result = new Multiply();
					}
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '&':
					reader.Read();
					ch++;
					tokenCh = ch;
					reader.Read();
					if (LookaheadChar == '&')
					{
						Next();
						result = new AND();
					}
					else
					{
						result = new And();
					}
					ch++;
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '|':
					reader.Read();
					ch++;
					tokenCh = ch;
					reader.Read();
					if (LookaheadChar == '|')
					{
						Next();
						result = new OR();
						ch++;
						result.Char = tokenCh;
						result.Line = ln;
						return result;
					}
					throw new Exception($"Syntax error at ({ch}, {ln}).");
				case '{':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new LeftBracket();
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '}':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new RightBracket();
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '(':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new LeftParenthesis();
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case ')':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new RightParenthesis();
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '[':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new LeftSquareBracket();
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case ']':
					ch++;
					tokenCh = ch;
					reader.Read();
					result = new RightSquareBracket();
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '<':
					reader.Read();
					ch++;
					tokenCh = ch;
					if ('=' == LookaheadChar)
					{
						reader.Read();
						result = new LessThanOrEqual();
						ch++;
					}
					else
					{
						result = new LeftAngleBracket();
					}
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				case '>':
					reader.Read();
					ch++;
					tokenCh = ch;
					if ('=' == LookaheadChar)
					{
						reader.Read();
						result = new GreaterThanOrEqual();
						ch++;
					}
					else
					{
						result = new RightAngleBracket();
					}
					result.Char = tokenCh;
					result.Line = ln;
					return result;
				default:
					throw new Exception($"Syntax error at ({ch}, {ln}).");
				}
			}
			return result;
		}

		public List<IToken> Scan()
		{
			return Scan(removeComments: false);
		}

		public List<IToken> Scan(bool removeComments)
		{
			List<IToken> list = new List<IToken>();
			IToken token;
			do
			{
				bool flag = true;
				token = Next();
				list.Add(token);
			}
			while (token.Kind != Kind.EOF);
			if (removeComments)
			{
				list.RemoveAll((IToken x) => x.Kind == Kind.Comment);
			}
			return list;
		}

		public bool IsOperator(char c)
		{
			if (operators == null)
			{
				operators = new List<char> { '+', '-', '/', '*', '%', '=', '!' };
			}
			return operators.Contains(c);
		}

		public bool IsDelimiter(char c)
		{
			if (delimiters == null)
			{
				delimiters = new List<char> { '[', ']', '(', ')', '{', '}', '<', '>' };
			}
			return delimiters.Contains(c);
		}
	}
}
