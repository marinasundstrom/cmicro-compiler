using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using CMicro.Compiler;
using CMicro.Compiler.Ast;
using CMicro.Compiler.Tokens;

namespace cmc
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Cµ Compiler\n");
			Console.Write(new StreamReader("example.cm").ReadToEnd());
			Console.Write("\n\n");
			string[] keywords = Scanner.Keywords;
			Scanner scanner = new Scanner(new StreamReader("example.cm"), keywords, ignoreCase: false);
			List<IToken> list = scanner.Scan(removeComments: true);
			foreach (IToken item in list)
			{
				if (item is CMicro.Compiler.Tokens.Identifier)
				{
					Console.ForegroundColor = ConsoleColor.Gray;
					CMicro.Compiler.Tokens.Identifier identifier = (CMicro.Compiler.Tokens.Identifier)item;
					Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", identifier.Value, identifier.GetType().Name, identifier.Char, identifier.Line);
				}
				else if (item is Keyword)
				{
					Console.ForegroundColor = ConsoleColor.Blue;
					Keyword keyword = (Keyword)item;
					Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", keyword.Value, keyword.GetType().Name, keyword.Char, keyword.Line);
				}
				else if (item is StringLiteral)
				{
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					StringLiteral stringLiteral = (StringLiteral)item;
					Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", stringLiteral.Value, stringLiteral.GetType().Name, stringLiteral.Char, stringLiteral.Line);
				}
				else if (item is IntLiteral)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					IntLiteral intLiteral = (IntLiteral)item;
					Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", intLiteral.Value, intLiteral.GetType().Name, intLiteral.Char, intLiteral.Line);
				}
				else if (item is CharLiteral)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					CharLiteral charLiteral = (CharLiteral)item;
					Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", charLiteral.Value, charLiteral.GetType().Name, charLiteral.Char, charLiteral.Line);
				}
				else if (item is RealLiteral)
				{
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					RealLiteral realLiteral = (RealLiteral)item;
					Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", realLiteral.Value, realLiteral.GetType().Name, realLiteral.Char, realLiteral.Line);
				}
				else if (item is Operator)
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
					Operator @operator = (Operator)item;
					Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", @operator.Value, @operator.GetType().Name, @operator.Char, @operator.Line);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.White;
					Token<object> token = (Token<object>)item;
					Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", token.Value, token.GetType().Name, token.Char, token.Line);
				}
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();
			Parser parser = new Parser(list, ignoreCase: false);

			// INFO: Added this
			parser.Parse();

			foreach (IParseError error in parser.Errors)
			{
				Console.WriteLine(error.ToString());
			}
			CodeGenerator codeGenerator = new CodeGenerator((CMicro.Compiler.Ast.Program)parser.Result);
			codeGenerator.GenerateAssembly("output.exe", PortableExecutableKinds.ILOnly); //INFO: Was PortableExecutableKinds.ConsoleApplication
			Console.ReadLine();
		}
	}
}
