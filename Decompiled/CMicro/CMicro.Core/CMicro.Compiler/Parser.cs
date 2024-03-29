using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CMicro.Compiler.Ast;
using CMicro.Compiler.Tokens;

namespace CMicro.Compiler
{
	public class Parser : IParser
	{
		private readonly List<IParseError> errors;

		private readonly bool ignoreCase;

		private readonly Thread parsingThread;

		private bool abortOnError;

		private string currentNamespace;

		private int index;

		private List<IToken> input;

		private int instance_Index;

		private AstRoot root;

		private readonly Stack<IExpression> parse_operandstack = new Stack<IExpression>();

		private IExpression parse_current;

		private Queue<IExpression> parse_input = new Queue<IExpression>();

		private readonly Stack<KeyValuePair<int, List<IToken>>> states = new Stack<KeyValuePair<int, List<IToken>>>();

		public IToken LookaheadToken
		{
			get
			{
				if (index + 1 >= input.Count)
				{
					return new EOF();
				}
				return input[index + 1];
			}
		}

		public IToken CurrentToken
		{
			get
			{
				if (index >= input.Count)
				{
					return new EOF();
				}
				return input[index];
			}
		}

		public AstRoot Result => root;

		public bool IgnoreCase => ignoreCase;

		public bool AbortOnError
		{
			get
			{
				return abortOnError;
			}
			set
			{
				abortOnError = value;
			}
		}

		public List<IParseError> Errors => errors;

		public void ParseCases()
		{
			switch (LookaheadToken.Kind)
			{
				case Kind.EOF:
					Abort();
					break;
				case Kind.EOL:
					NextToken();
					break;
				default:
					ReportSyntaxError("Expected class, struct, interface or enum definition", CurrentToken, includeData: true);
					break;
			}
		}

		public void ParseStructMembers(StructDeclaration classd)
		{
			EatIfWhile(Kind.EOL);
			IClassMember classMember = null;
			if (NextTokenIs(Kind.Keyword, Kind.Identifier))
			{
				FieldVariableDeclaration fieldVariableDeclaration = ParseFieldVariableDeclaration();
				fieldVariableDeclaration.Parent = classd;
				classMember = fieldVariableDeclaration;
			}
			if (classMember != null)
			{
				classd.Members.Add(classMember);
			}
		}

		public FieldVariableDeclaration ParseFieldVariableDeclaration()
		{
			FieldVariableDeclaration fieldVariableDeclaration = new FieldVariableDeclaration();
			fieldVariableDeclaration.Type = ParseTypename().Name;
			fieldVariableDeclaration.Name = ParseName();
			Eat(";");
			EatIfWhile(Kind.EOL);
			return fieldVariableDeclaration;
		}

		public string ParseImportStatement()
		{
			Eat("import");
			string result = ParseName();
			Eat(";");
			return result;
		}

		public Parser(List<IToken> tokens, bool ignoreCase)
		{
			input = new List<IToken>();
			input.Add(CMicro.Compiler.Tokens.Tokens.BOF);
			input.AddRange(tokens);
			errors = new List<IParseError>();
			this.ignoreCase = ignoreCase;
			abortOnError = true;
			currentNamespace = "global";

			// INFO: This is BAD and doesn't work anymore
			/*
			parsingThread = new Thread(Parse);
			parsingThread.Start();
			parsingThread.Join();
			*/
		}

		public IToken NextToken()
		{
			index++;
			instance_Index++;
			return CurrentToken;
		}

		public void GoBack()
		{
			index--;
		}

		public void GoBack(int steps)
		{
			index -= steps;
		}

		public void Abort()
		{
			// INFO: BAD
			//parsingThread.Abort();

			// INFO: Added
			//Environment.Exit(0);
			throw new Exception();
		}

		public IToken Eat(string str)
		{
			if (string.Compare(LookaheadToken.GetValue(), str, ignoreCase) == 0)
			{
				NextToken();
				return CurrentToken;
			}
			ReportSyntaxError(LookaheadToken, ParseErrorCodes.UnexpectedToken);
			return null;
		}

		public IToken Eat(Kind tokenType)
		{
			if (LookaheadToken.Kind == tokenType)
			{
				NextToken();
				return CurrentToken;
			}
			ReportSyntaxError(LookaheadToken, ParseErrorCodes.UnexpectedToken);
			return null;
		}

		public IToken Eat(Kind kind1, Kind kind2)
		{
			if (LookaheadToken.Kind == kind1 || LookaheadToken.Kind == kind2)
			{
				NextToken();
				return CurrentToken;
			}
			ReportSyntaxError(LookaheadToken, ParseErrorCodes.UnexpectedToken);
			return null;
		}

		public IToken Eat(OperatorKind Operator)
		{
			if (LookaheadToken is Operator)
			{
				Operator @operator = (Operator)LookaheadToken;
				if (@operator.OperatorKind == Operator)
				{
					NextToken();
					return CurrentToken;
				}
			}
			ReportSyntaxError(LookaheadToken, ParseErrorCodes.UnexpectedToken);
			return null;
		}

		public bool NextTokenIs(string str)
		{
			if (string.Compare(LookaheadToken.GetValue(), str, ignoreCase) == 0)
			{
				return true;
			}
			return false;
		}

		public bool NextTokenIs(Kind tokenType)
		{
			if (LookaheadToken.Kind == tokenType)
			{
				return true;
			}
			return false;
		}

		public bool NextTokenIs(Kind Kind1, Kind Kind2)
		{
			return LookaheadToken.Kind == Kind1 || LookaheadToken.Kind == Kind2;
		}

		public bool NextTokenIs(OperatorKind Operator)
		{
			if (LookaheadToken is Operator)
			{
				Operator @operator = (Operator)LookaheadToken;
				if (@operator.OperatorKind == Operator)
				{
					return true;
				}
			}
			ReportSyntaxError(LookaheadToken, ParseErrorCodes.UnexpectedToken);
			return false;
		}

		public bool NextNameIs(string str)
		{
			int num = index;
			string text = ParseName();
			index = num;
			if (text == str)
			{
				return true;
			}
			return false;
		}

		public bool CheckIndex(int Ift, string str)
		{
			return CompareTokenString(input[index + Ift], str);
		}

		public void EatIfWhile(string str)
		{
			while (string.Compare(LookaheadToken.GetValue(), str, ignoreCase) == 0)
			{
				NextToken();
			}
		}

		public void EatIfWhile(Kind tokenType)
		{
			while (LookaheadToken.Kind == tokenType)
			{
				NextToken();
			}
		}

		public bool SameKind(IToken token1, IToken token2)
		{
			return (object)token1.GetType() == token2.GetType();
		}

		public bool TokenIsKind(IToken Token, Kind Kind)
		{
			return Token.Kind == Kind;
		}

		public bool TokenIsOneOfKinds(IToken Token, Kind Kind1, Kind Kind2)
		{
			return Token.Kind == Kind1 || Token.Kind == Kind2;
		}

		public bool IsTokenOperatorKind(IToken Token, OperatorKind OperatorKind)
		{
			if (Token is Operator)
			{
				return ((Operator)Token).OperatorKind == OperatorKind;
			}
			return false;
		}

		public bool CompareTokenString(IToken Token, string Value)
		{
			return Token.GetValue() == Value;
		}

		public void ReportSyntaxError(IToken token, ParseErrorCodes errorCode)
		{
			errors.Add(new ParseError(token, errorCode));
			if (abortOnError)
			{
				//parsingThread.Abort();

				Abort();
			}
		}

		// INFO: Made public
		public void Parse()
		{
			root = new Program();
			Program program = (Program)root;
			while (input.Count > 0)
			{
				if (NextTokenIs("import"))
				{
					program.References.Add(ParseImportStatement());
					continue;
				}
				if (NextTokenIs(Kind.EOF))
				{
					// INFO: Removed
					//Abort();

					// INFO: Added
					return;
				}
				if (NextTokenIs(Kind.EOL))
				{
					NextToken();
					continue;
				}
				IProgramDeclaration programDeclaration = null;
				if (NextTokenIs("struct"))
				{
					StructDeclaration item = ParseStructDeclaration();
					program.Members.Add(item);
				}
				else if (NextTokenIs("enum"))
				{
					EnumDeclaration enumDeclaration = ParseEnumDeclaration();
					programDeclaration = enumDeclaration;
				}
				else if (NextTokenIs(Kind.Identifier, Kind.Keyword))
				{
					Typename returnType = ParseTypename();
					string value = Eat(Kind.Identifier).GetValue();
					if (NextTokenIs(typeof(LeftParenthesis)))
					{
						FunctionDeclaration functionDeclaration = new FunctionDeclaration();
						functionDeclaration.ReturnType = returnType;
						functionDeclaration.Name = value;
						functionDeclaration.Parameters = ParseFunctionParameters();
						EatIfWhile(Kind.EOL);
						functionDeclaration.Code = ParseCodeBlock();
						IsMainMethod(functionDeclaration);
						programDeclaration = functionDeclaration;
					}
					else
					{
						ReportSyntaxError("Expected function, struct or enum definition", CurrentToken, includeData: true);
					}
				}
				else
				{
					ParseCases();
				}
				if (programDeclaration != null)
				{
					program.Members.Add(programDeclaration);
				}
			}
		}

		public IToken Eat(System.Type type)
		{
			if ((object)LookaheadToken.GetType() == type)
			{
				NextToken();
				return CurrentToken;
			}
			ReportSyntaxError(LookaheadToken, ParseErrorCodes.UnexpectedToken);
			return null;
		}

		public bool NextTokenIs(System.Type type)
		{
			if ((object)LookaheadToken.GetType() == type)
			{
				return true;
			}
			return false;
		}

		public void ReportSyntaxError(string message, IToken token, bool includeData)
		{
			errors.Add(new ParseError2(message, token, includeData));
			if (abortOnError)
			{
				//parsingThread.Abort();

				Abort();
			}
		}

		public FunctionDeclaration ParseMethodDeclaration()
		{
			FunctionDeclaration functionDeclaration = new FunctionDeclaration();
			functionDeclaration.ReturnType = ParseTypename();
			functionDeclaration.Name = Eat(Kind.Identifier).GetValue();
			IsMainMethod(functionDeclaration);
			functionDeclaration.Parameters = ParseFunctionParameters();
			EatIfWhile(Kind.EOL);
			functionDeclaration.Code = ParseCodeBlock();
			EatIfWhile(Kind.EOL);
			return functionDeclaration;
		}

		public Typename ParseTypename()
		{
			Typename typename = new Typename();
			if (NextTokenIs(typeof(Keyword)))
			{
				string text = NextToken().GetValue();
				switch (text)
				{
					default:
						if (!(text == "var"))
						{
							throw new Exception("Not a valid type.");
						}
						break;
					case "int":
					case "double":
					case "char":
					case "string":
					case "bool":
					case "void":
						break;
				}
				switch (text)
				{
					case "int":
						text = "System.Int32";
						break;
					case "double":
						text = "System.Double";
						break;
					case "char":
						text = "System.Char";
						break;
					case "string":
						text = "System.String";
						break;
					case "bool":
						text = "System.Boolean";
						break;
					case "var":
						text = "System.Object";
						break;
					case "void":
						text = "System.Void";
						break;
				}
				if (NextTokenIs("["))
				{
					StringBuilder stringBuilder = new StringBuilder();
					NextToken();
					stringBuilder.Append("[");
					while (!NextTokenIs(typeof(RightSquareBracket)))
					{
						Eat(typeof(Comma));
						stringBuilder.Append(",");
					}
					Eat(typeof(RightSquareBracket));
					stringBuilder.Append("]");
					typename.IsArray = true;
					text += stringBuilder.ToString();
				}
				typename.Name = text;
			}
			else if (NextTokenIs(typeof(CMicro.Compiler.Tokens.Identifier)))
			{
				typename.Name = NextToken().GetValue();
			}
			return typename;
		}

		public object ParseCodeElement()
		{
			object obj = null;
			if ((NextTokenIs(typeof(Keyword)) && input[index + 2] is CMicro.Compiler.Tokens.Identifier) || (NextTokenIs(typeof(Keyword)) && input[index + 4] is CMicro.Compiler.Tokens.Identifier))
			{
				ObjectDeclaration objectDeclaration = new ObjectDeclaration();
				objectDeclaration.Type = ParseTypename();
				objectDeclaration.Name = NextToken().GetValue();
				obj = objectDeclaration;
			}
			else if (NextTokenIs(typeof(CMicro.Compiler.Tokens.Identifier)) && input[index + 2] is CMicro.Compiler.Tokens.Identifier)
			{
				ObjectDeclaration objectDeclaration = new ObjectDeclaration();
				objectDeclaration.Type = ParseTypename();
				objectDeclaration.Name = NextToken().GetValue();
				obj = objectDeclaration;
			}
			else if (NextTokenIs(typeof(CMicro.Compiler.Tokens.Identifier)) && input[index + 2] is Period)
			{
				StructObject structObject = new StructObject();
				structObject.Name = NextToken().GetValue();
				NextToken();
				StructObject structObject2 = (StructObject)(structObject.Member = new StructObject());
				while (NextTokenIs(typeof(Period)))
				{
					StructObject structObject3 = new StructObject();
					structObject3.Name = NextToken().GetValue();
					NextToken();
					structObject2.Member = structObject3;
					structObject2 = structObject3;
				}
				obj = structObject;
			}
			else if (NextTokenIs(typeof(CMicro.Compiler.Tokens.Identifier)) && input[index + 2] is LeftParenthesis)
			{
				MethodCall methodCall = new MethodCall();
				methodCall.Name = NextToken().GetValue();
				Eat(typeof(LeftParenthesis));
				while (!NextTokenIs(typeof(RightParenthesis)))
				{
					IExpression item = ParseExpression2();
					methodCall.Arguments.Add(item);
					if (!NextTokenIs(typeof(RightParenthesis)))
					{
						Eat(",");
					}
				}
				Eat(typeof(RightParenthesis));
				obj = methodCall;
			}
			else if (NextTokenIs(typeof(CMicro.Compiler.Tokens.Identifier)))
			{
				PrimitiveObject primitiveObject = new PrimitiveObject();
				primitiveObject.Name = NextToken().GetValue();
				obj = primitiveObject;
			}
			if (obj == null)
			{
				throw new Exception();
			}
			return obj;
		}

		public string ParseName()
		{
			NextToken();
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			while (flag)
			{
				if (TokenIsOneOfKinds(CurrentToken, Kind.Identifier, Kind.Keyword) && LookaheadToken.GetValue() == ".")
				{
					stringBuilder.Append(CurrentToken.GetValue());
					stringBuilder.Append(NextToken().GetValue());
					NextToken();
				}
				else if (TokenIsOneOfKinds(CurrentToken, Kind.Identifier, Kind.Keyword) && TokenIsOneOfKinds(LookaheadToken, Kind.Identifier, Kind.Keyword))
				{
					stringBuilder.Append(CurrentToken.GetValue());
					flag = false;
				}
				else if (TokenIsOneOfKinds(CurrentToken, Kind.Identifier, Kind.Keyword) && (!NextTokenIs(".") || !NextTokenIs(",") || NextTokenIs(Kind.Identifier, Kind.Keyword)))
				{
					stringBuilder.Append(CurrentToken.GetValue());
					flag = false;
				}
				else if (TokenIsOneOfKinds(CurrentToken, Kind.Identifier, Kind.Keyword) && (LookaheadToken.GetValue() == ";" || LookaheadToken.GetValue() == "[" || LookaheadToken.GetValue() == "," || LookaheadToken.GetValue() == "(" || LookaheadToken.Kind == Kind.EOL || LookaheadToken.Kind == Kind.Operator))
				{
					stringBuilder.Append(CurrentToken.GetValue());
					flag = false;
				}
				else if (CompareTokenString(CurrentToken, "."))
				{
					ReportSyntaxError("Invalid identifier", CurrentToken, includeData: true);
					NextToken();
				}
				else
				{
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}

		public IExpression ParseExpression()
		{
			Queue<IExpression> queue = new Queue<IExpression>();
			while ((!NextTokenIs(typeof(RightParenthesis)) || !(input[index + 2] is Operator) || NextTokenIs(typeof(RightParenthesis))) && (!NextTokenIs(typeof(RightParenthesis)) || !(input[index + 2] is LeftBracket)) && (!NextTokenIs(typeof(RightParenthesis)) || !(input[index + 2] is Keyword)) && (!NextTokenIs(typeof(RightParenthesis)) || !(input[index + 2] is CMicro.Compiler.Tokens.Identifier)) && !NextTokenIs(typeof(LambdaOperator)) && !NextTokenIs(typeof(Semicolon)) && !NextTokenIs(typeof(EOL)))
			{
				if ((!NextTokenIs(typeof(LambdaOperator)) && NextTokenIs(typeof(IntLiteral))) || NextTokenIs(typeof(RealLiteral)) || NextTokenIs(typeof(StringLiteral)) || NextTokenIs(typeof(CharLiteral)) || LookaheadToken is Operator || NextTokenIs(typeof(LeftParenthesis)) || NextTokenIs(typeof(RightParenthesis)))
				{
					queue.Enqueue(ToIExpression(NextToken()));
				}
				else if (NextTokenIs(typeof(CMicro.Compiler.Tokens.Identifier)) || NextTokenIs(typeof(Keyword)))
				{
					if (CheckIndex(2, "("))
					{
						queue.Enqueue(ParseMethodCall());
					}
					else if (CheckIndex(2, "["))
					{
						queue.Enqueue(ParseArrayAccessor());
					}
					else
					{
						queue.Enqueue(ToOperand(NextToken()));
					}
				}
				else
				{
					if (!NextTokenIs(typeof(LeftBracket)))
					{
						break;
					}
					queue.Enqueue(ParseArrayInitialization());
				}
			}
			List<IExpression> list = queue.ToList();
			Queue<IExpression> queue2 = (parse_input = ToPostfix(queue.ToList()));
			IExpression expression = null;
			if (parse_input.Count > 1)
			{
				ParseTree();
				expression = parse_current;
			}
			else
			{
				expression = parse_input.Dequeue();
			}
			parse_current = null;
			return expression;
		}

		private ArrayAccessor ParseArrayAccessor()
		{
			ArrayAccessor arrayAccessor = new ArrayAccessor();
			arrayAccessor.Name = NextToken().GetValue();
			Eat(typeof(LeftSquareBracket));
			while (!NextTokenIs(typeof(RightSquareBracket)))
			{
				IExpression item = ParseExpression2();
				arrayAccessor.Arguments.Add(item);
				if (!NextTokenIs(typeof(RightSquareBracket)))
				{
					Eat(",");
				}
			}
			Eat(typeof(RightSquareBracket));
			return arrayAccessor;
		}

		private ArrayInitialization ParseArrayInitialization()
		{
			ArrayInitialization arrayInitialization = new ArrayInitialization();
			Eat("{");
			while (!NextTokenIs("}"))
			{
				arrayInitialization.Arguments.Add(ParseExpression2());
				if (!NextTokenIs("}"))
				{
					Eat(",");
				}
			}
			Eat("}");
			return arrayInitialization;
		}

		public IExpression ParseExpression2()
		{
			Queue<IExpression> queue = new Queue<IExpression>();
			int num = 0;
			while ((!NextTokenIs(typeof(RightParenthesis)) || !(input[index + 2] is Operator)) && (!NextTokenIs(typeof(RightParenthesis)) || !(input[index + 2] is LeftBracket)) && (!NextTokenIs(typeof(RightParenthesis)) || !(input[index + 2] is Keyword)) && (!NextTokenIs(typeof(RightParenthesis)) || !(input[index + 2] is CMicro.Compiler.Tokens.Identifier)) && !NextTokenIs(typeof(LambdaOperator)) && !NextTokenIs(typeof(Semicolon)) && !NextTokenIs(typeof(Comma)) && !NextTokenIs(typeof(EOL)))
			{
				if (NextTokenIs(typeof(IntLiteral)) || NextTokenIs(typeof(RealLiteral)) || NextTokenIs(typeof(StringLiteral)) || NextTokenIs(typeof(CharLiteral)))
				{
					queue.Enqueue(ToIExpression(NextToken()));
				}
				else if (NextTokenIs(typeof(Operator)))
				{
					queue.Enqueue(ToIExpression(NextToken()));
				}
				else if (NextTokenIs(typeof(LeftParenthesis)))
				{
					num++;
					queue.Enqueue(ToIExpression(NextToken()));
				}
				else if (NextTokenIs(typeof(RightParenthesis)))
				{
					if (num <= 0)
					{
						break;
					}
					queue.Enqueue(ToIExpression(NextToken()));
					num--;
				}
				else if (NextTokenIs(typeof(CMicro.Compiler.Tokens.Identifier)) || NextTokenIs(typeof(Keyword)))
				{
					if (CheckIndex(2, "("))
					{
						queue.Enqueue(ParseMethodCall());
					}
					else if (CheckIndex(2, "["))
					{
						queue.Enqueue(ParseArrayAccessor());
					}
					else
					{
						queue.Enqueue(ToOperand(NextToken()));
					}
				}
				else
				{
					if (!NextTokenIs(typeof(LeftBracket)))
					{
						break;
					}
					queue.Enqueue(ParseArrayInitialization());
				}
			}
			Queue<IExpression> queue2 = (parse_input = ToPostfix(queue.ToList()));
			IExpression expression = null;
			if (parse_input.Count > 1)
			{
				ParseTree();
				expression = parse_current;
			}
			else
			{
				expression = parse_input.Dequeue();
			}
			parse_current = null;
			return expression;
		}

		private MethodCall ParseMethodCall()
		{
			MethodCall methodCall = new MethodCall();
			methodCall.Name = NextToken().GetValue();
			Eat(typeof(LeftParenthesis));
			while (!NextTokenIs(typeof(RightParenthesis)))
			{
				IExpression item = ParseExpression2();
				methodCall.Arguments.Add(item);
				if (!NextTokenIs(typeof(RightParenthesis)))
				{
					Eat(",");
				}
			}
			Eat(typeof(RightParenthesis));
			return methodCall;
		}

		private IExpression ToOperand(IToken iToken)
		{
			IExpression result = null;
			if (iToken is IOperand)
			{
				result = (IExpression)iToken;
			}
			else
			{
				ReportSyntaxError("Unexpected operand", iToken, includeData: true);
			}
			return result;
		}

		private IExpression ToIExpression(IToken iToken)
		{
			IExpression result = null;
			if (iToken is IExpression)
			{
				result = (IExpression)iToken;
			}
			return result;
		}

		public void ParseTree()
		{
			if (parse_input.Count <= 0)
			{
				return;
			}
			IExpression expression = parse_current;
			IExpression expression2 = null;
			if (parse_current != null)
			{
				expression2 = parse_current;
			}
			while (parse_input.Peek() is IOperand)
			{
				parse_operandstack.Push(parse_input.Dequeue());
			}
			if (parse_input.Peek() is IArithmeticOperator)
			{
				ArithmeticExpression arithmeticExpression = new ArithmeticExpression();
				Operator @operator = (Operator)parse_input.Dequeue();
				if (@operator is Add)
				{
					arithmeticExpression.Operation = ArithmeticOperations.Add;
				}
				else if (@operator is Subtract)
				{
					arithmeticExpression.Operation = ArithmeticOperations.Sub;
				}
				else if (@operator is Multiply)
				{
					arithmeticExpression.Operation = ArithmeticOperations.Mul;
				}
				else if (@operator is Divide)
				{
					arithmeticExpression.Operation = ArithmeticOperations.Div;
				}
				parse_current = arithmeticExpression;
			}
			else if (parse_input.Peek() is IComparisonOperator)
			{
				ComparisonExpression comparisonExpression = new ComparisonExpression();
				Operator @operator = (Operator)parse_input.Dequeue();
				if (@operator is RightAngleBracket)
				{
					comparisonExpression.Operation = ComparisonOperations.Grt;
				}
				else if (@operator is LeftAngleBracket)
				{
					comparisonExpression.Operation = ComparisonOperations.Les;
				}
				else if (@operator is GreaterThanOrEqual)
				{
					comparisonExpression.Operation = ComparisonOperations.GEq;
				}
				else if (@operator is LessThanOrEqual)
				{
					comparisonExpression.Operation = ComparisonOperations.LEq;
				}
				else if (@operator is NotEqual)
				{
					comparisonExpression.Operation = ComparisonOperations.NEq;
				}
				else if (@operator is Equal)
				{
					comparisonExpression.Operation = ComparisonOperations.Eq;
				}
				parse_current = comparisonExpression;
			}
			else if (parse_input.Peek() is ILogicOperator)
			{
				LogicExpression logicExpression = new LogicExpression();
				Operator @operator = (Operator)parse_input.Dequeue();
				if (@operator is AND)
				{
					logicExpression.Operation = LogicOperations.AND;
				}
				else if (@operator is OR)
				{
					logicExpression.Operation = LogicOperations.OR;
				}
				parse_current = logicExpression;
			}
			if (expression2 == null)
			{
				((BinaryExpression)parse_current).LeftSide = parse_operandstack.Pop();
			}
			else
			{
				((BinaryExpression)parse_current).LeftSide = expression2;
			}
			((BinaryExpression)parse_current).RightSide = parse_operandstack.Pop();
			if (parse_current is BinaryExpression)
			{
				BinaryExpression binaryExpression = parse_current as BinaryExpression;
				if (!(binaryExpression.LeftSide is BinaryExpression))
				{
					IExpression leftSide = binaryExpression.LeftSide;
					binaryExpression.LeftSide = binaryExpression.RightSide;
					binaryExpression.RightSide = leftSide;
				}
				if (binaryExpression is ComparisonExpression)
				{
					IExpression leftSide = binaryExpression.LeftSide;
					binaryExpression.LeftSide = binaryExpression.RightSide;
					binaryExpression.RightSide = leftSide;
				}
			}
			ParseTree();
		}

		private void IsMainMethod(FunctionDeclaration methd)
		{
			if (methd.Name == "main" && ((Program)root).EntryPoint == null)
			{
				((Program)root).EntryPoint = methd;
			}
			else if (methd.Name == "main" && ((Program)root).EntryPoint != null)
			{
				ReportSyntaxError("Program can only have one main method.", CurrentToken, includeData: false);
			}
		}

		public Statement ParseStatement()
		{
			Statement result = null;
			if (NextTokenIs(Kind.Keyword))
			{
				if (NextTokenIs("return"))
				{
					result = ParseReturnStatement();
				}
				else if (NextTokenIs("if"))
				{
					result = ParseIfStatement();
				}
				else if (NextTokenIs("while"))
				{
					result = ParseWhileStatement();
				}
				else
				{
					bool global = false;
					bool constant = false;
					if (NextTokenIs("global"))
					{
						NextToken();
						global = true;
						if (NextTokenIs("const"))
						{
							NextToken();
							constant = true;
						}
					}
					object obj = ParseCodeElement();
					if (!(obj is ObjectDeclaration))
					{
						throw new Exception();
					}
					ObjectDeclaration objectDeclaration = obj as ObjectDeclaration;
					DeclareAssignStatement declareAssignStatement = new DeclareAssignStatement();
					declareAssignStatement.Global = global;
					declareAssignStatement.Constant = constant;
					if (NextTokenIs("="))
					{
						NextToken();
						declareAssignStatement.Expression = ParseExpression();
					}
					Eat(";");
					declareAssignStatement.Type = objectDeclaration.Type;
					declareAssignStatement.Name = objectDeclaration.Name;
					result = declareAssignStatement;
				}
			}
			else if (NextTokenIs(Kind.Identifier))
			{
				object obj = ParseCodeElement();
				if (obj is PrimitiveObject)
				{
					PrimitiveObject primitiveObject = obj as PrimitiveObject;
					AssignStatement assignStatement = new AssignStatement();
					if (!NextTokenIs("="))
					{
						throw new Exception("Invalid statement.");
					}
					NextToken();
					assignStatement.Expression = ParseExpression();
					Eat(";");
					assignStatement.Variable = primitiveObject.Name;
					result = assignStatement;
				}
				else if (obj is StructObject)
				{
					StructObject structObject = obj as StructObject;
					AssignStatement assignStatement = new AssignStatement();
					if (NextTokenIs("="))
					{
						NextToken();
						assignStatement.Expression = ParseExpression();
					}
					Eat(";");
					assignStatement.Variable = structObject.Name;
					result = assignStatement;
				}
				else if (obj is MethodCall)
				{
					MethodCall result2 = obj as MethodCall;
					Eat(";");
					return result2;
				}
			}
			else
			{
				ReportSyntaxError($"Unexpected {LookaheadToken.GetType().Name.ToLower()}", LookaheadToken, includeData: true);
			}
			return result;
		}

		private TypeOf ParseTypeOf()
		{
			Eat("typeof");
			TypeOf typeOf = new TypeOf();
			Eat(typeof(LeftParenthesis));
			typeOf.Operand = ParseTypename();
			Eat(typeof(RightParenthesis));
			return typeOf;
		}

		private Statement ParseAssignStatement()
		{
			AssignStatement assignStatement = new AssignStatement();
			string value = Eat(Kind.Identifier).GetValue();
			if (NextTokenIs("="))
			{
				NextToken();
				assignStatement.Expression = ParseExpression();
				Eat(";");
			}
			EatIfWhile(Kind.EOL);
			return assignStatement;
		}

		private Statement ParseIfStatement()
		{
			Eat("if");
			Eat(typeof(LeftParenthesis));
			IfStatement ifStatement = new IfStatement();
			ifStatement.Condition = ParseExpression();
			Eat(typeof(RightParenthesis));
			EatIfWhile(Kind.EOL);
			if (NextTokenIs(typeof(LeftBracket)))
			{
				ifStatement.Statements = ParseCodeBlock();
			}
			else
			{
				ifStatement.Statements.Add(ParseStatement());
			}
			if (NextTokenIs("else"))
			{
				NextToken();
				if (NextTokenIs(typeof(LeftBracket)))
				{
					ifStatement.ElseStatements = ParseCodeBlock();
				}
				else
				{
					ifStatement.ElseStatements.Add(ParseStatement());
				}
			}
			EatIfWhile(Kind.EOL);
			return ifStatement;
		}

		private Statement ParseWhileStatement()
		{
			Eat("while");
			Eat(typeof(LeftParenthesis));
			WhileStatement whileStatement = new WhileStatement();
			whileStatement.Condition = ParseExpression();
			Eat(typeof(RightParenthesis));
			if (NextTokenIs(Kind.EOL))
			{
				NextToken();
			}
			if (NextTokenIs(typeof(LeftBracket)))
			{
				whileStatement.Statements = ParseCodeBlock();
			}
			else
			{
				whileStatement.Statements.Add(ParseStatement());
			}
			return whileStatement;
		}

		private ReturnStatement ParseReturnStatement()
		{
			Eat("return");
			ReturnStatement returnStatement = new ReturnStatement();
			returnStatement.Expression = ParseExpression();
			Eat(";");
			return returnStatement;
		}

		private DeleteStatement ParseDeleteStatement()
		{
			Eat("delete");
			DeleteStatement deleteStatement = new DeleteStatement();
			deleteStatement.Expression = ParseExpression();
			if (deleteStatement.Expression is BinaryExpression)
			{
				throw new Exception("Illegal instruction. Parameter must be a global variable.");
			}
			Eat(";");
			return deleteStatement;
		}

		public Dictionary<string, CMicro.Compiler.Ast.Type> ParseFunctionParameters()
		{
			Dictionary<string, CMicro.Compiler.Ast.Type> dictionary = new Dictionary<string, CMicro.Compiler.Ast.Type>();
			Eat(typeof(LeftParenthesis));
			while (!NextTokenIs(typeof(RightParenthesis)))
			{
				Typename value = ParseTypename();
				string value2 = Eat(Kind.Identifier).GetValue();
				dictionary.Add(value2, value);
				if (!NextTokenIs(typeof(RightParenthesis)))
				{
					Eat(",");
				}
			}
			Eat(typeof(RightParenthesis));
			return dictionary;
		}

		public EnumDeclaration ParseEnumDeclaration()
		{
			EnumDeclaration enumDeclaration = new EnumDeclaration();
			Eat("enum");
			enumDeclaration.FullName = string.Format(arg1: enumDeclaration.Name = Eat(Kind.Identifier).GetValue(), format: "{0}.{1}", arg0: currentNamespace);
			EatIfWhile(Kind.EOL);
			Eat("{");
			EatIfWhile(Kind.EOL);
			int num = 0;
			while (!NextTokenIs("}"))
			{
				EatIfWhile(Kind.EOL);
				enumDeclaration.Members.Add(Eat(Kind.Identifier).GetValue(), num);
				num++;
				EatIfWhile(Kind.EOL);
				if (!NextTokenIs("}"))
				{
					Eat(",");
				}
			}
			EatIfWhile(Kind.EOL);
			Eat("}");
			if (NextTokenIs(";"))
			{
				NextToken();
			}
			return enumDeclaration;
		}

		public StructDeclaration ParseStructDeclaration()
		{
			StructDeclaration structDeclaration = new StructDeclaration();
			Eat("struct");
			structDeclaration.FullName = (structDeclaration.Name = Eat(Kind.Identifier).GetValue());
			EatIfWhile(Kind.EOL);
			Eat("{");
			while (!NextTokenIs("}") && !NextTokenIs(Kind.EOF))
			{
				EatIfWhile(Kind.EOL);
				ParseStructMembers(structDeclaration);
			}
			Eat("}");
			if (NextTokenIs(";"))
			{
				NextToken();
			}
			return structDeclaration;
		}

		private void SaveState()
		{
			states.Push(new KeyValuePair<int, List<IToken>>(index, input));
		}

		private void ChangeState(List<IToken> tokenlist, int startIndex)
		{
			index = startIndex;
			input = tokenlist;
		}

		private void LoadSavedState()
		{
			KeyValuePair<int, List<IToken>> keyValuePair = states.Pop();
			index = keyValuePair.Key;
			input = keyValuePair.Value;
		}

		public Queue<IExpression> ToPostfix(List<IExpression> input)
		{
			Stack<IExpression> stack = new Stack<IExpression>();
			Queue<IExpression> queue = new Queue<IExpression>();
			for (int i = 0; i < input.Count; i++)
			{
				if (input[i] is IntLiteral || input[i] is RealLiteral || input[i] is CMicro.Compiler.Tokens.Identifier || input[i] is MethodCall || input[i] is ArrayInitialization || input[i] is StringLiteral || input[i] is CharLiteral || input[i] is Keyword || input[i] is ArrayAccessor)
				{
					if (input[i] is Keyword && ((IToken)input[i]).GetValue() != "true" && ((IToken)input[i]).GetValue() != "false" && ((IToken)input[i]).GetValue() != "null")
					{
						ReportSyntaxError("Unexpected keyword", (IToken)input[i], includeData: true);
					}
					queue.Enqueue(input[i]);
					continue;
				}
				if (input[i] is Operator && !(input[i] is LeftParenthesis) && !(input[i] is RightParenthesis))
				{
					while (stack.Count > 0 && ((Operator)input[i]).Precedence <= ((Operator)stack.Peek()).Precedence)
					{
						queue.Enqueue(stack.Pop());
					}
					stack.Push(input[i]);
					continue;
				}
				if (input[i] is LeftParenthesis)
				{
					stack.Push(input[i]);
					continue;
				}
				if (input[i] is RightParenthesis)
				{
					while (!(stack.Peek() is LeftParenthesis))
					{
						queue.Enqueue(stack.Pop());
					}
					stack.Pop();
					continue;
				}
				throw new Exception("An error has occurred");
			}
			while (stack.Count > 0)
			{
				queue.Enqueue(stack.Pop());
			}
			return queue;
		}

		public List<Statement> ParseCodeBlock()
		{
			List<Statement> list = new List<Statement>();
			List<IToken> list2 = new List<IToken>();
			int num = 0;
			bool flag = true;
			Eat("{");
			while (flag)
			{
				EatIfWhile(Kind.EOL);
				if (NextTokenIs(typeof(LeftBracket)))
				{
					num++;
					list2.Add(NextToken());
				}
				else if (NextTokenIs(typeof(RightBracket)) && num == 0)
				{
					num--;
					flag = false;
				}
				else if (NextTokenIs(typeof(RightBracket)))
				{
					num--;
					list2.Add(NextToken());
				}
				else
				{
					list2.Add(NextToken());
				}
			}
			Eat("}");
			SaveState();
			ChangeState(list2, -1);
			Statement statement = new Statement();
			while (LookaheadToken.Kind != Kind.EOF)
			{
				EatIfWhile(Kind.EOL);
				statement = ((!NextTokenIs("if")) ? ((!NextTokenIs("while")) ? ParseStatement() : ParseWhileStatement()) : ParseIfStatement());
				list.Add(statement);
			}
			LoadSavedState();
			return list;
		}
	}
}
