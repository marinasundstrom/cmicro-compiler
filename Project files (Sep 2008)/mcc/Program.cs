using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler;
using CSharp.Compiler.Tokens;

namespace ScannerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Print text to parse
            Console.Write(new System.IO.StreamReader("example.c").ReadToEnd());
            Console.Write("\n\n");

            //Parse
            string[] keywords = Scanner.Keywords;
            Scanner scanner = new Scanner(new System.IO.StreamReader("example.c"), keywords, false);
            var tokens = scanner.Scan(true);

            //List tokens
            foreach (var token in tokens)
            {
                if (token is Identifier)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Identifier t = (Identifier)token;
                    Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", t.Value, t.GetType().Name, t.Char, t.Line);
                }
                else if (token is Keyword)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;

                    Keyword t = (Keyword)token;
                    Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", t.Value, t.GetType().Name, t.Char, t.Line);
                }
                else if (token is StringLiteral)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                    StringLiteral t = (StringLiteral)token;
                    Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", t.Value, t.GetType().Name, t.Char, t.Line);
                }
                else if (token is IntLiteral)
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    IntLiteral t = (IntLiteral)token;
                    Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", t.Value, t.GetType().Name, t.Char, t.Line);
                }
                else if (token is Operator)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    Operator t = (Operator)token;
                    Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", t.Value, t.GetType().Name, t.Char, t.Line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    Token<object> t = (Token<object>)token;
                    Console.WriteLine("{0, -15} {1,-20} Ch: {2,-6} Ln: {3}", t.Value, t.GetType().Name, t.Char, t.Line);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            //Parse
            CSharp.Compiler.Parser parser = new Parser(tokens, false);

            parser.Parse();

            foreach (var e in parser.Errors)
                Console.WriteLine(e.ToString());

            if (parser.Errors.Count < 1)
            {
                //Generate XML
                CodeXMLGenerator xmlgen = new CodeXMLGenerator((CSharp.Compiler.Ast.Program)parser.Result);
                xmlgen.GenerateAndSave("code.xml");
            }

            Console.ReadLine();
        }
    }
}
