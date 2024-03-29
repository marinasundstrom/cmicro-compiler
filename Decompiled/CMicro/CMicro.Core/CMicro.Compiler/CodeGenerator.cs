using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using CMicro.Compiler.Ast;
using CMicro.Compiler.Tokens;

namespace CMicro.Compiler
{
	public class CodeGenerator
	{
		public class FunctionInfo
		{
			public MethodBuilder FunctionBuilder { get; protected set; }

			public List<System.Type> ParameterTypes { get; protected set; }

			public System.Type ReturnType { get; protected set; }

			public FunctionInfo(MethodBuilder functionBuilder, List<System.Type> parameterTypes, System.Type returnType)
			{
				FunctionBuilder = functionBuilder;
				ParameterTypes = parameterTypes;
				ReturnType = returnType;
			}
		}

		private readonly SymbolTable symbolTable;

		private Dictionary<string, LocalBuilder> locals;

		private Dictionary<string, FieldBuilder> globals;

		private Dictionary<string, FunctionInfo> functions;

		private List<System.Type> parames;

		private readonly Program astnode;

		private AssemblyBuilder assemblybuilder;

		private ModuleBuilder modulebuilder;

		private MethodBuilder methodbuilder;

		private TypeBuilder globaltype_builder;

		private AssemblyName assemblyname;

		// INFO: Originally PEFileKinds
		private PortableExecutableKinds outputkind;

		private Assembly CMicroLibrary;

		private bool generateExceptionHandlers = false;

		private System.Type consoleClass;

		private MethodInfo readLineMethod;

		private MethodInfo readMethod;

		private MethodInfo writeLineMethod;

		private System.Type doubleClass;

		private MethodInfo parseDoubleMethod;

		private System.Type int32Class;

		private MethodInfo parseInt32Method;

		private System.Type charClass;

		private MethodInfo parseCharMethod;

		private System.Type objectClass;

		private MethodInfo toStringObjectMethod;

		private System.Type stringClass;

		private Label EndLabel;

		private LocalBuilder loc;

		private LocalBuilder array;

		public CodeGenerator(Program ast)
		{
			astnode = ast;
			locals = new Dictionary<string, LocalBuilder>();
			globals = new Dictionary<string, FieldBuilder>();
			functions = new Dictionary<string, FunctionInfo>();
			symbolTable = new SymbolTable();
			GetAssemblyReferences();
			GetTypeReferences();
		}

		private void GetAssemblyReferences()
		{
			CMicroLibrary = Assembly.GetExecutingAssembly();
		}

		private void GetTypeReferences()
		{
			// INFO: Updated GetType calls due to changed behavior

			consoleClass = System.Type.GetType("System.Console, System.Console", true);
			readLineMethod = consoleClass.GetMethod("ReadLine");
			readMethod = consoleClass.GetMethod("Read");
			writeLineMethod = consoleClass.GetMethod("WriteLine", new System.Type[1] { typeof(string) });
			doubleClass = System.Type.GetType("System.Double");
			parseDoubleMethod = doubleClass.GetMethod("Parse", new System.Type[1] { typeof(string) });
			int32Class = System.Type.GetType("System.Int32");
			parseInt32Method = int32Class.GetMethod("Parse", new System.Type[1] { typeof(string) });
			charClass = System.Type.GetType("System.Char");
			parseCharMethod = charClass.GetMethod("Parse", new System.Type[1] { typeof(string) });
			stringClass = System.Type.GetType("System.String");
			objectClass = System.Type.GetType("System.Object");
			toStringObjectMethod = objectClass.GetMethod("ToString");
		}

		public void GenerateAssembly(string path, PortableExecutableKinds kind)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			outputkind = kind;
			InputPrecheck();
			InitializeCompiler(path, fileNameWithoutExtension);
			GenerateAssemblyMembers();
			assemblybuilder.Save(path);

			// INFO: Pre-migration
			//assemblybuilder.Save(path, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
		}

		private void InputPrecheck()
		{
			/*
			INFO: Pre-migration code
			if (astnode.Members.Count > 0)
			{
				if (outputkind == PEFileKinds.ConsoleApplication)
				{
					if (astnode.EntryPoint == null)
					{
						throw new Exception("A console program must have a main-function.");
					}
				}
				else if (outputkind == PEFileKinds.Dll)
				{
					if (astnode.EntryPoint != null)
					{
						throw new Exception("Main-function is not supported with the DLL option.");
					}
				}
				else if (outputkind == PEFileKinds.WindowApplication)
				{
					throw new Exception("Windows Applications are not supported by this compiler.");
				}
				return;
			}
			throw new Exception("Nothing to compile.");
			*/
		}

		private void InitializeCompiler(string path, string name_wo_ext)
		{
			// INFO: Updated to use AssemblyBuilder.DefinePersistedAssembly from .NET 9

			assemblyname = new AssemblyName(name_wo_ext);
			assemblybuilder =
			AssemblyBuilder.DefinePersistedAssembly(
				assemblyname,
				typeof(object).Assembly);

			modulebuilder = assemblybuilder.DefineDynamicModule(name_wo_ext);
			globaltype_builder = modulebuilder.DefineType("global", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit);

			/*
			// INFO: Pre-migration code

			assemblyname = new AssemblyName(name_wo_ext);
			assemblybuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyname, (AssemblyBuilderAccess)3);
			modulebuilder = assemblybuilder.DefineDynamicModule(name_wo_ext, path, true);
			globaltype_builder = modulebuilder.DefineType("global", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit);
			*/
		}

		private void GenerateAssemblyMembers()
		{
			foreach (IProgramDeclaration member in astnode.Members)
			{
				if (member is StructDeclaration)
				{
					StructDeclaration structDeclaration = (StructDeclaration)member;
					symbolTable.NewSymbol(structDeclaration.Name, SymbolType.Struct);
					GenerateStruct(structDeclaration, modulebuilder.DefineType(structDeclaration.Name, TypeAttributes.Public, typeof(ValueType)));
				}
				else if (member is FunctionDeclaration)
				{
					GenerateFunction(member);
				}
				else if (member is EnumDeclaration)
				{
					EnumDeclaration enumDeclaration = (EnumDeclaration)member;
					symbolTable.NewSymbol(enumDeclaration.Name, SymbolType.Enum);
					GenerateEnum(enumDeclaration, modulebuilder.DefineEnum(enumDeclaration.Name, TypeAttributes.Public, typeof(int)));
				}
			}
			globaltype_builder.CreateType();
		}

		private void GenerateFunction(IProgramDeclaration member)
		{
			FunctionDeclaration functionDeclaration = (FunctionDeclaration)member;
			symbolTable.BeginScope();
			parames = new List<System.Type>();
			foreach (KeyValuePair<string, CMicro.Compiler.Ast.Type> parameter in functionDeclaration.Parameters)
			{
				System.Type type = System.Type.GetType(parameter.Value.Name, throwOnError: true);
				symbolTable.NewSymbol(parameter.Key, SymbolType.Local, isParameter: true);
				parames.Add(type);
			}
			System.Type type2 = System.Type.GetType(functionDeclaration.ReturnType.Name, throwOnError: true);
			if (functionDeclaration.Name == "main")
			{
				if ((object)type2 != typeof(int) && !(type2.FullName == "System.Void"))
				{
					throw new Exception("Illegal signature for function 'main'. Must return either void or int.");
				}
				methodbuilder = globaltype_builder.DefineMethod(functionDeclaration.Name, MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig, CallingConventions.Standard, type2, parames.ToArray());
				if (functionDeclaration.Name == "main")
				{
					// TODO: Fix when added to .NET 9
					//assemblybuilder.SetEntryPoint((MethodInfo)methodbuilder);
				}
			}
			else
			{
				methodbuilder = globaltype_builder.DefineMethod(functionDeclaration.Name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, type2, parames.ToArray());
			}
			int num = 1;
			foreach (KeyValuePair<string, CMicro.Compiler.Ast.Type> parameter2 in functionDeclaration.Parameters)
			{
				ParameterBuilder parameterBuilder = methodbuilder.DefineParameter(num, ParameterAttributes.None, parameter2.Key);
				num++;
			}
			functions.Add(functionDeclaration.Name, new FunctionInfo(methodbuilder, parames, type2));
			GenerateMethod(functionDeclaration, methodbuilder);
			symbolTable.EndScope();
		}

		private void GenerateEnum(EnumDeclaration enumd, EnumBuilder enumBuilder)
		{
			symbolTable.BeginScope();
			foreach (KeyValuePair<string, int> member in enumd.Members)
			{
				FieldBuilder fieldBuilder = enumBuilder.DefineLiteral(member.Key, member.Value);
			}
			symbolTable.EndScope();
			enumBuilder.CreateType();
		}

		private void GenerateStruct(StructDeclaration classd, TypeBuilder tb)
		{
			symbolTable.BeginScope();
			foreach (IClassMember member in classd.Members)
			{
				FieldVariableDeclaration fieldVariableDeclaration = (FieldVariableDeclaration)member;
				tb.DefineField(fieldVariableDeclaration.Name, System.Type.GetType(fieldVariableDeclaration.Type.ToString()), FieldAttributes.Public);
			}
			symbolTable.EndScope();
			tb.CreateType();
		}

		private void GenerateMethod(FunctionDeclaration funcdecl, MethodBuilder methodInfo)
		{
			ILGenerator iLGenerator = methodInfo.GetILGenerator();
			EndLabel = iLGenerator.DefineLabel();
			if (generateExceptionHandlers)
			{
				iLGenerator.BeginExceptionBlock();
			}
			foreach (Statement item in funcdecl.Code)
			{
				GenerateStatement(item, funcdecl, iLGenerator);
			}
			if (generateExceptionHandlers)
			{
				iLGenerator.BeginCatchBlock(typeof(Exception));
				LocalBuilder local = iLGenerator.DeclareLocal(typeof(Exception));
				iLGenerator.Emit(OpCodes.Stloc, local);
				iLGenerator.Emit(OpCodes.Nop);
				iLGenerator.Emit(OpCodes.Ldloc, local);
				iLGenerator.EmitCall(OpCodes.Callvirt, toStringObjectMethod, null);
				iLGenerator.EmitCall(OpCodes.Call, writeLineMethod, new System.Type[1] { typeof(string) });
				iLGenerator.EndExceptionBlock();
			}
			iLGenerator.MarkLabel(EndLabel);
			iLGenerator.Emit(OpCodes.Ret);
			locals.Clear();
		}

		public void GenerateStatement(Statement statement, FunctionDeclaration funcdecl, ILGenerator ilgen)
		{
			if (statement is IfStatement)
			{
				GenerateIfStatement(statement, funcdecl, ilgen);
			}
			else if (statement is WhileStatement)
			{
				GenerateWhileStatement(statement, funcdecl, ilgen);
			}
			else if (statement is AssignStatement)
			{
				GenerateAssignStatement(statement, ilgen);
			}
			else if (statement is DeclareAssignStatement)
			{
				GenerateDeclareAssignStatement(statement, ilgen);
			}
			else if (statement is PrintStatement)
			{
				GeneratePrintStatement(statement, ilgen);
			}
			else if (statement is MethodCall)
			{
				MethodCall methcall = statement as MethodCall;
				if (GenerateMethodCall(methcall, ilgen, out var returnType) && returnType.FullName != "System.Void")
				{
					ilgen.Emit(OpCodes.Pop);
				}
			}
			else if (statement is ArrayAccessor)
			{
				ArrayAccessor accessor = statement as ArrayAccessor;
				GenerateAssignToArrayIndexStatement(accessor, ilgen);
			}
			else if (statement is ReturnStatement)
			{
				GenerateReturnStatement(statement, funcdecl, ilgen);
			}
		}

		private void GenerateAssignToArrayIndexStatement(ArrayAccessor accessor, ILGenerator ilgen)
		{
		}

		private void GenerateReturnStatement(Statement statement, FunctionDeclaration funcdecl, ILGenerator ilgen)
		{
			if (funcdecl.ReturnType.Name == "System.Void")
			{
				throw new Exception($"Illegal return statement in function '{funcdecl.Name}'. Function returns void.");
			}
			ReturnStatement returnStatement = statement as ReturnStatement;
			string name = funcdecl.ReturnType.Name;
			string fullName = GenerateExpression(returnStatement.Expression, ilgen).FullName;
			if (fullName == name)
			{
				ilgen.Emit(OpCodes.Br, EndLabel);
				return;
			}
			throw new Exception($"Function {funcdecl.Name} is supposed to return {name}, but the expression in return statement is of type {fullName}.");
		}

		private void GeneratePrintStatement(Statement statement, ILGenerator ilgen)
		{
			PrintStatement printStatement = statement as PrintStatement;
			System.Type type = System.Type.GetType("System.Console");
			System.Type type2 = GenerateExpression(printStatement.Expression, ilgen);
			if ((object)type2 == typeof(string))
			{
				ilgen.Emit(OpCodes.Call, type.GetMethod("WriteLine", new System.Type[1] { typeof(string) }));
			}
			else
			{
				ilgen.Emit(OpCodes.Box, type2);
				ilgen.Emit(OpCodes.Call, type.GetMethod("WriteLine", new System.Type[1] { typeof(object) }));
			}
			ilgen.Emit(OpCodes.Nop);
		}

		private void GenerateDeclareAssignStatement(Statement statement, ILGenerator ilgen)
		{
			DeclareAssignStatement declareAssignStatement = statement as DeclareAssignStatement;
			if (NameExists(declareAssignStatement.Name, out var _))
			{
				throw new Exception(string.Format("Cannot declare variable {0} because the name already exists in the current context."));
			}
			System.Type type2 = System.Type.GetType(declareAssignStatement.Type.Name);
			if (declareAssignStatement.Global)
			{
				if (!DeclareGlobal(declareAssignStatement.Name, type2, out var fb, globaltype_builder, declareAssignStatement.Constant))
				{
					throw new Exception($"Failed to declare global variable '{declareAssignStatement.Name}'.");
				}
				if (declareAssignStatement.Constant || declareAssignStatement.Expression != null)
				{
					if (declareAssignStatement.Constant && declareAssignStatement.Expression == null)
					{
						throw new Exception($"Read-only constant '{declareAssignStatement.Name}' must be initialized.");
					}
					System.Type type3 = null;
					type3 = GenerateExpression(declareAssignStatement.Expression, ilgen);
					if (!CanCast(type3, type2))
					{
						throw new Exception($"Cannot assign a value of type '{type3.Name}' to a variable of type '{type2.Name}'.");
					}
					ilgen.Emit(OpCodes.Stsfld, fb);
				}
				return;
			}
			if (!DeclareLocal(declareAssignStatement.Name, type2, out var lb, ilgen))
			{
				throw new Exception($"Failed to declare local variable '{declareAssignStatement.Name}'.");
			}
			if (declareAssignStatement.Constant || declareAssignStatement.Expression != null)
			{
				if (declareAssignStatement.Constant && declareAssignStatement.Expression == null)
				{
					throw new Exception($"Read-only constant '{declareAssignStatement.Name}' must be initialized when it is declared.");
				}
				System.Type type3 = null;
				type3 = GenerateExpression(declareAssignStatement.Expression, ilgen);
				if (!CanCast(type3, type2))
				{
					throw new Exception($"Cannot assign a value of type '{type3.Name}' to a variable of type '{type2.Name}'.");
				}
				ilgen.Emit(OpCodes.Stloc, lb);
			}
		}

		private void GenerateAssignStatement(Statement statement, ILGenerator ilgen)
		{
			AssignStatement assignStatement = statement as AssignStatement;
			if (NameExists(assignStatement.Variable, out var type))
			{
				System.Type type2;
				switch (type)
				{
					case SymbolType.Local:
						if (assignStatement.Expression != null)
						{
							GetLocal(assignStatement.Variable, out var lb, out type2);
							System.Type type3 = null;
							type3 = GenerateExpression(assignStatement.Expression, ilgen);
							if (!CanCast(type3, type2))
							{
								throw new Exception($"Cannot assign a value of type '{type3.Name}' to a variable of type '{type2.Name}'.");
							}
							ilgen.Emit(OpCodes.Stloc, lb);
						}
						break;
					case SymbolType.Global:
						if (assignStatement.Expression != null)
						{
							GetGlobal(assignStatement.Variable, out var fb, out type2);
							System.Type type3 = null;
							type3 = GenerateExpression(assignStatement.Expression, ilgen);
							if (!CanCast(type3, type2))
							{
								throw new Exception($"Cannot assign a value of type '{type3.Name}' to a variable of type '{type2.Name}'.");
							}
							ilgen.Emit(OpCodes.Stsfld, fb);
						}
						break;
					default:
						throw new Exception($"The identifier '{assignStatement.Variable}' is not a variable.");
				}
				return;
			}
			throw new Exception($"The name '{assignStatement.Variable}' does not exist in the current context.");
		}

		private void GenerateForStatement(Statement statement, FunctionDeclaration funcdecl, ILGenerator ilgen)
		{
			ForStatement forStatement = statement as ForStatement;
		}

		private void GenerateWhileStatement(Statement statement, FunctionDeclaration funcdecl, ILGenerator ilgen)
		{
			WhileStatement whileStatement = statement as WhileStatement;
			Label label = ilgen.DefineLabel();
			Label label2 = ilgen.DefineLabel();
			ilgen.Emit(OpCodes.Br_S, label);
			ilgen.BeginScope();
			symbolTable.BeginScope();
			ilgen.MarkLabel(label2);
			foreach (Statement statement2 in whileStatement.Statements)
			{
				GenerateStatement(statement2, funcdecl, ilgen);
			}
			ilgen.Emit(OpCodes.Br, label);
			ilgen.EndScope();
			symbolTable.EndScope();
			ilgen.MarkLabel(label);
			if ((object)GenerateExpression(whileStatement.Condition, ilgen) != typeof(bool))
			{
				throw new Exception("Invalid expression in conditional statement. Expression cannot be converted to bool.");
			}
			ilgen.Emit(OpCodes.Brtrue_S, label2);
		}

		private void GenerateIfStatement(Statement statement, FunctionDeclaration funcdecl, ILGenerator ilgen)
		{
			IfStatement ifStatement = statement as IfStatement;
			Label label = ilgen.DefineLabel();
			Label label2 = ilgen.DefineLabel();
			Label label3 = ilgen.DefineLabel();
			ilgen.BeginScope();
			symbolTable.BeginScope();
			if ((object)GenerateExpression(ifStatement.Condition, ilgen) != typeof(bool))
			{
				throw new Exception("Invalid expression in conditional statement. Expression cannot be converted to bool.");
			}
			ilgen.Emit(OpCodes.Ldc_I4_0);
			ilgen.Emit(OpCodes.Ceq);
			if (ifStatement.ElseStatements != null)
			{
				ilgen.Emit(OpCodes.Brtrue_S, label2);
			}
			else
			{
				ilgen.Emit(OpCodes.Brtrue_S, label3);
			}
			ilgen.BeginScope();
			symbolTable.BeginScope();
			ilgen.MarkLabel(label);
			foreach (Statement statement2 in ifStatement.Statements)
			{
				GenerateStatement(statement2, funcdecl, ilgen);
			}
			ilgen.EndScope();
			symbolTable.EndScope();
			ilgen.Emit(OpCodes.Br, label3);
			if (ifStatement.ElseStatements != null)
			{
				ilgen.BeginScope();
				symbolTable.BeginScope();
				ilgen.MarkLabel(label2);
				foreach (Statement elseStatement in ifStatement.ElseStatements)
				{
					GenerateStatement(elseStatement, funcdecl, ilgen);
				}
				ilgen.EndScope();
				symbolTable.EndScope();
				ilgen.Emit(OpCodes.Br, label3);
			}
			ilgen.EndScope();
			symbolTable.EndScope();
			ilgen.MarkLabel(label3);
		}

		private bool GenerateMethodCall(MethodCall methcall, ILGenerator ilgen, out System.Type returnType)
		{
			List<System.Type> list = new List<System.Type>();
			for (int i = 0; i < methcall.Arguments.Count; i++)
			{
				System.Type item = GenerateExpression(methcall.Arguments[i], ilgen);
				list.Add(item);
			}
			MethodInfo methodInfo = LoadFunctionFromAssembly(CMicroLibrary, "CMicro.Library.Strings", methcall.Name, list.ToArray());
			if ((object)methodInfo == null)
			{
				methodInfo = LoadFunctionFromAssembly(Assembly.GetAssembly(typeof(Math)), "System.Math", methcall.Name);
				if ((object)methodInfo == null)
				{
					methodInfo = LoadFunctionFromAssembly(CMicroLibrary, "CMicro.Library.IO", methcall.Name);
					if ((object)methodInfo == null)
					{
						methodInfo = LoadFunctionFromAssembly(CMicroLibrary, "CMicro.Library.Types", methcall.Name);
						if ((object)methodInfo == null)
						{
							methodInfo = LoadFunctionFromAssembly(CMicroLibrary, "CMicro.Library.Conversion", methcall.Name);
							if ((object)methodInfo == null)
							{
								methodInfo = LoadFunctionFromAssembly(CMicroLibrary, "CMicro.Library.Console", methcall.Name);
							}
						}
					}
				}
			}
			if ((object)methodInfo != null)
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters.Length == methcall.Arguments.Count)
				{
					ilgen.EmitCall(OpCodes.Call, methodInfo, null);

					// INFO: Old
					//ilgen.EmitCall(OpCodes.Call, methodInfo, list.ToArray());
					returnType = methodInfo.ReturnType;
					return true;
				}
				throw new Exception(string.Format("Invalid number of arguments. Function '{0}' takes {2} arguments.", methcall.Name, methcall.Arguments.Count));
			}
			FunctionInfo value = null;
			if (functions.TryGetValue(methcall.Name, out value))
			{
				List<System.Type> parameterTypes = value.ParameterTypes;
				if (methcall.Arguments.Count == parameterTypes.Count)
				{
					List<System.Type> list2 = new List<System.Type>();
					for (int i = 0; i < parameterTypes.Count; i++)
					{
						System.Type type = parameterTypes[i];
						MakeCast(ilgen, type, list[i]);
						if (CanCast(list[i], type))
						{
							list2.Add(type);
							continue;
						}
						throw new Exception($"Invalid type of argument in call to function '{methcall.Name}'.");
					}
					ilgen.EmitCall(OpCodes.Call, value.FunctionBuilder, list2.ToArray());
					returnType = value.ReturnType;
					return true;
				}
				throw new Exception(string.Format("Invalid number of arguments. Function '{0}' takes {2} arguments.", methcall.Name, methcall.Arguments.Count));
			}
			throw new Exception($"Function '{methcall.Name}' is not defined in assembly.");
		}

		private MethodInfo LoadFunctionFromAssembly(Assembly assembly, string module, string function)
		{
			return assembly.GetType(module, throwOnError: true, ignoreCase: false)!.GetMethod(function);
		}

		private MethodInfo LoadFunctionFromAssembly(Assembly assembly, string module, string function, System.Type[] parameters)
		{
			return assembly.GetType(module, throwOnError: true, ignoreCase: false)!.GetMethod(function, parameters);
		}

		private static void MakeCast(ILGenerator ilgen, System.Type paramtype, System.Type argexprtype)
		{
			if (argexprtype.IsValueType && paramtype.IsClass)
			{
				ilgen.Emit(OpCodes.Box, argexprtype);
			}
			else if (argexprtype.IsValueType && paramtype.FullName == "System.String")
			{
				MethodInfo method = System.Type.GetType("System.Object")!.GetMethod("ToString");
				ilgen.EmitCall(OpCodes.Callvirt, method, new System.Type[0]);
			}
		}

		private bool CanCast(System.Type argexprtype, System.Type paramtype)
		{
			if ((object)argexprtype == paramtype)
			{
				return true;
			}
			if (((object)argexprtype == typeof(int) || (object)argexprtype == typeof(double)) && (object)paramtype == typeof(string))
			{
				return true;
			}
			if ((object)argexprtype == typeof(int) && (object)paramtype == typeof(double))
			{
				return true;
			}
			if ((object)argexprtype == typeof(char) && (object)paramtype == typeof(string))
			{
				return true;
			}
			if ((object)paramtype == typeof(object))
			{
				return true;
			}
			return false;
		}

		private System.Type GenerateExpression(IExpression expression, ILGenerator ilgen)
		{
			if (expression is BinaryExpression)
			{
				return GenerateBinaryExpression(expression, ilgen);
			}
			if (expression is CMicro.Compiler.Tokens.Identifier)
			{
				CMicro.Compiler.Tokens.Identifier identifier = expression as CMicro.Compiler.Tokens.Identifier;
				return GenerateIdentifierLoad(identifier.Value, ilgen);
			}
			return GenerateLiteralLoad(expression, ilgen);
		}

		private System.Type GenerateLiteralLoad(IExpression expression, ILGenerator ilgen)
		{
			if (expression is IntLiteral)
			{
				IntLiteral intLiteral = expression as IntLiteral;
				ilgen.Emit(OpCodes.Ldc_I4, intLiteral.Value);
				return typeof(int);
			}
			if (expression is RealLiteral)
			{
				RealLiteral realLiteral = expression as RealLiteral;
				ilgen.Emit(OpCodes.Ldc_R8, realLiteral.Value);
				return typeof(double);
			}
			if (expression is StringLiteral)
			{
				StringLiteral stringLiteral = expression as StringLiteral;
				ilgen.Emit(OpCodes.Ldstr, stringLiteral.Value);
				return typeof(string);
			}
			if (expression is CharLiteral)
			{
				CharLiteral charLiteral = expression as CharLiteral;
				LocalBuilder local = ilgen.DeclareLocal(typeof(char));
				ilgen.Emit(OpCodes.Ldc_I4, charLiteral.Value);
				ilgen.Emit(OpCodes.Stloc, local);
				ilgen.Emit(OpCodes.Ldloc, local);
				ilgen.Emit(OpCodes.Box, typeof(char));
				return typeof(char);
			}
			if (expression is Keyword)
			{
				Keyword keyword = expression as Keyword;
				switch (keyword.Value)
				{
					case "true":
						ilgen.Emit(OpCodes.Ldc_I4_1);
						return typeof(bool);
					case "false":
						ilgen.Emit(OpCodes.Ldc_I4_0);
						return typeof(bool);
					default:
						return null;
				}
			}
			if (expression is MethodCall)
			{
				MethodCall methodCall = expression as MethodCall;
				System.Type returnType = null;
				if (GenerateMethodCall(methodCall, ilgen, out returnType))
				{
					return returnType;
				}
				throw new Exception($"Function {methodCall.Name} does not return any value.");
			}
			if (expression is ArrayAccessor)
			{
				ArrayAccessor accessor = expression as ArrayAccessor;
				System.Type returnType = null;
				GenerateGetArrayElement(accessor, ilgen, out returnType);
				return returnType;
			}
			if (expression is ArrayInitialization)
			{
				ArrayInitialization arrayinit = expression as ArrayInitialization;
				System.Type returnType = null;
				GenerateArrayInitialization(arrayinit, ilgen, out returnType);
				return returnType;
			}
			throw new Exception("Unknown compiler exception.");
		}

		private void GenerateArrayInitialization(ArrayInitialization arrayinit, ILGenerator ilgen, out System.Type returnType)
		{
			int count = arrayinit.Arguments.Count;
			System.Type type = null;
			int num = 0;
			foreach (IExpression argument in arrayinit.Arguments)
			{
				if (num != 0)
				{
					ilgen.Emit(OpCodes.Ldloc, array);
					ilgen.Emit(OpCodes.Ldc_I4, num);
				}
				System.Type type2 = GenerateExpression(argument, ilgen);
				loc = ilgen.DeclareLocal(type2);
				if (num == 0)
				{
					ilgen.Emit(OpCodes.Stloc, loc);
				}
				if (num == 0)
				{
					ilgen.Emit(OpCodes.Ldc_I4, count);
					ilgen.Emit(OpCodes.Newarr, type2);
					array = ilgen.DeclareLocal(type2.MakeArrayType());
					ilgen.Emit(OpCodes.Stloc_S, array);
					ilgen.Emit(OpCodes.Ldloc, array);
				}
				if ((object)type == type2 || (object)type == null)
				{
					if (num == 0)
					{
						ilgen.Emit(OpCodes.Ldc_I4, num);
						ilgen.Emit(OpCodes.Ldloc, loc);
					}
					ilgen.Emit(OpCodes.Stelem, type2);
					num++;
					type = type2;
					continue;
				}
				throw new Exception($"Element at index {num} in array initializer is of the wrong type. Expected {type.FullName}.");
			}
			ilgen.Emit(OpCodes.Ldloc, array);
			returnType = type.MakeArrayType();
		}

		private void GenerateGetArrayElement(ArrayAccessor accessor, ILGenerator ilgen, out System.Type returnType)
		{
			LocalBuilder localBuilder = ilgen.DeclareLocal(typeof(int));
			System.Type type = GenerateIdentifierLoad(accessor.Name, ilgen);
			if (type.IsArray)
			{
				LocalBuilder localBuilder2 = ilgen.DeclareLocal(type);
				// INFO: Not existing anymore
				//localBuilder2.SetLocalSymInfo("array");
				ilgen.Emit(OpCodes.Stloc, localBuilder2);
				if (accessor.Arguments.Count > 0)
				{
					foreach (IExpression argument in accessor.Arguments)
					{
						ilgen.Emit(OpCodes.Ldloc, localBuilder2);
						if ((object)GenerateExpression(argument, ilgen) == typeof(int))
						{
							ilgen.Emit(OpCodes.Ldelem, type.GetElementType());
							continue;
						}
						throw new Exception("An index of array must be of type 'int'.");
					}
					returnType = type.GetElementType();
					return;
				}
				throw new Exception($"No arguments in array accessor for array {accessor.Name}.");
			}
			throw new Exception($"Object '{accessor.Name}' is not an array.");
		}

		private System.Type GenerateIdentifierLoad(string name, ILGenerator ilgen)
		{
			if (NameExists(name, out var type))
			{
				switch (type)
				{
					case SymbolType.Global:
						{
							FieldBuilder fieldBuilder = globals[name];
							ilgen.Emit(OpCodes.Ldsfld, fieldBuilder);
							return fieldBuilder.FieldType;
						}
					case SymbolType.Local:
						{
							List<Symbol> list = symbolTable.Symbols.FindAll((Symbol n) => n.IsParameter);
							if (list.Count > 0 && list.Exists((Symbol n) => n.Name == name))
							{
								int num = list.FindIndex((Symbol x) => x.Name == name);
								ilgen.Emit(OpCodes.Ldarg, num);
								return parames[num];
							}
							LocalBuilder localBuilder = locals[name];
							ilgen.Emit(OpCodes.Ldloc, localBuilder);
							return localBuilder.LocalType;
						}
					default:
						return null;
				}
			}
			throw new Exception($"The name '{name}' does not exist in the current context.");
		}

		private System.Type GenerateBinaryExpression(IExpression expression, ILGenerator ilgen)
		{
			if (expression is ArithmeticExpression)
			{
				ArithmeticExpression arithmeticExpression = expression as ArithmeticExpression;
				System.Type typeLhs = GenerateExpression(arithmeticExpression.LeftSide, ilgen);
				System.Type typeRhs = GenerateExpression(arithmeticExpression.RightSide, ilgen);
				System.Type type = CheckAndGetCastType(typeRhs, typeLhs);
				if ((object)type == typeof(string) || (object)type == typeof(char))
				{
					GenerateStringOperation(arithmeticExpression.Operation, ilgen);
				}
				else
				{
					GenerateNumericOperation(arithmeticExpression.Operation, ilgen);
				}
				return type;
			}
			if (expression is ComparisonExpression)
			{
				ComparisonExpression comparisonExpression = expression as ComparisonExpression;
				GenerateExpression(comparisonExpression.RightSide, ilgen);
				GenerateExpression(comparisonExpression.LeftSide, ilgen);
				switch (comparisonExpression.Operation)
				{
					case ComparisonOperations.Eq:
						ilgen.Emit(OpCodes.Ceq);
						break;
					case ComparisonOperations.Grt:
						ilgen.Emit(OpCodes.Cgt);
						break;
					case ComparisonOperations.Les:
						ilgen.Emit(OpCodes.Clt);
						break;
					case ComparisonOperations.NEq:
						ilgen.Emit(OpCodes.Ceq);
						ilgen.Emit(OpCodes.Neg);
						break;
				}
				return typeof(bool);
			}
			return null;
		}

		private void GenerateNumericOperation(ArithmeticOperations arithmeticOperations, ILGenerator ilgen)
		{
			switch (arithmeticOperations)
			{
				case ArithmeticOperations.Add:
					ilgen.Emit(OpCodes.Add);
					break;
				case ArithmeticOperations.Sub:
					ilgen.Emit(OpCodes.Sub);
					break;
				case ArithmeticOperations.Mul:
					ilgen.Emit(OpCodes.Mul);
					break;
				case ArithmeticOperations.Div:
					ilgen.Emit(OpCodes.Div);
					break;
			}
		}

		private void GenerateStringOperation(ArithmeticOperations arithmeticOperations, ILGenerator ilgen)
		{
			System.Type type = System.Type.GetType("System.String");
			if (arithmeticOperations == ArithmeticOperations.Add)
			{
				MethodInfo method = type.GetMethod("Concat", new System.Type[2]
				{
					typeof(object),
					typeof(object)
				});
				ilgen.Emit(OpCodes.Call, method);
				return;
			}
			throw new Exception("Invalid operation on strings.");
		}

		private System.Type CheckAndGetCastType(System.Type typeRhs, System.Type typeLhs)
		{
			List<System.Type> list = new List<System.Type>();
			list.Add(typeLhs);
			list.Add(typeRhs);
			List<System.Type> list2 = list;
			if (list2.Exists((System.Type x) => (object)x == typeof(int)) && list2.Exists((System.Type x) => (object)x == typeof(double)))
			{
				return typeof(double);
			}
			if (list2.Exists((System.Type x) => (object)x == typeof(string)) && list2.Exists((System.Type x) => (object)x == typeof(double)))
			{
				return typeof(string);
			}
			if (list2.Exists((System.Type x) => (object)x == typeof(string)) && list2.Exists((System.Type x) => (object)x == typeof(int)))
			{
				return typeof(string);
			}
			if (list2.Exists((System.Type x) => (object)x == typeof(string)) && list2.Exists((System.Type x) => (object)x == typeof(char)))
			{
				return typeof(string);
			}
			if (list2.Exists((System.Type x) => (object)x == typeof(char)) && list2.Exists((System.Type x) => (object)x == typeof(char)))
			{
				return typeof(string);
			}
			if ((object)list2[0] == list2[1])
			{
				return list2[0];
			}
			throw new Exception($"Cannot implicitly cast {list2[0].Name} to {list2[1].Name}.");
		}

		public bool NameExists(string name, out SymbolType type)
		{
			Symbol symbol = symbolTable.Symbols.Find((Symbol x) => x.Name == name);
			if (symbol != null)
			{
				type = symbol.Type;
				return true;
			}
			type = SymbolType.None;
			return false;
		}

		public bool GetLocal(string name, out LocalBuilder lb, out System.Type type)
		{
			if (symbolTable.Symbols.Exists((Symbol x) => x.Name == name))
			{
				lb = locals[name];
				type = lb.LocalType;
				return true;
			}
			lb = null;
			type = null;
			return false;
		}

		public bool DeclareLocal(string name, System.Type type, out LocalBuilder lb, ILGenerator ilgen)
		{
			try
			{
				lb = ilgen.DeclareLocal(type);
				locals.Add(name, lb);
				symbolTable.NewSymbol(name, SymbolType.Local);
			}
			catch (Exception)
			{
				lb = null;
				return false;
			}
			return true;
		}

		public bool GetGlobal(string name, out FieldBuilder fb, out System.Type type)
		{
			if (symbolTable.Symbols.Exists((Symbol x) => x.Name == name))
			{
				fb = globals[name];
				type = fb.FieldType;
				return true;
			}
			fb = null;
			type = null;
			return false;
		}

		public bool DeclareGlobal(string name, System.Type type, out FieldBuilder fb, TypeBuilder tb, bool isConstant)
		{
			try
			{
				FieldAttributes attributes = FieldAttributes.Assembly | FieldAttributes.Static;
				if (isConstant)
				{
					attributes = FieldAttributes.Assembly | FieldAttributes.InitOnly;
				}
				fb = tb.DefineField(name, type, attributes);
				globals.Add(name, fb);
				symbolTable.NewGlobalSymbol(name, SymbolType.Global);
			}
			catch (Exception)
			{
				fb = null;
				return false;
			}
			return true;
		}

		public System.Type GetVariableType(string name)
		{
			Symbol symbol = symbolTable.Symbols.Find((Symbol x) => x.Name == name);
			if (symbol != null)
			{
				if (symbol.Type == SymbolType.Global)
				{
					FieldBuilder fieldBuilder = globals[name];
					return fieldBuilder.FieldType;
				}
				if (symbol.Type == SymbolType.Local)
				{
					LocalBuilder localBuilder = locals[name];
					return localBuilder.LocalType;
				}
				return null;
			}
			return null;
		}
	}
}
