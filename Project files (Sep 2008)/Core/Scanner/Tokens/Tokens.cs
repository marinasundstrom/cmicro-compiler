using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Tokens
    {
        public static readonly Period Period = new Period();
        public static readonly Star Star = new Star();

        public static readonly Colon Colon = new Colon();
        public static readonly Comma Comma = new Comma();

        public static readonly LeftAngleBracket LeftAngleBracket = new LeftAngleBracket();
        public static readonly RightAngleBracket RightAngleBracket = new RightAngleBracket();

        public static readonly LeftSquareBracket LeftSquareBracket = new LeftSquareBracket();
        public static readonly RightSquareBracket RightSquareBracket = new RightSquareBracket();

        public static readonly LeftParenthesis LeftParenthesis = new LeftParenthesis();
        public static readonly RightParenthesis RightParenthesis = new RightParenthesis();


        public static readonly Add Add = new Add();
        public static readonly AddAssign AddAssign = new AddAssign();

        public static readonly Subtract Subtract = new Subtract();
        public static readonly SubtractAssign SubractAssign = new SubtractAssign();

        public static readonly Multiply Multiply = new Multiply();
        public static readonly MultiplyAssign MultiplyAssign = new MultiplyAssign();

        public static readonly Divide Divide = new Divide();
        public static readonly DivideAssign DivideAssign = new DivideAssign();

        public static readonly Equal Equal = new Equal();
        public static readonly NotEqual NotEqual = new NotEqual();

        public static readonly Greater Greater = new Greater();
        public static readonly Less Less = new Less();

        public static readonly GreaterThanOrEqual GreaterThanOrEqual = new GreaterThanOrEqual();
        public static readonly LessThanOrEqual LessThanOrEqual = new LessThanOrEqual();

        public static readonly Not Not = new Not();

        public static readonly Modulo Modulo = new Modulo();
        public static readonly ModuloAssign ModuloAssign = new ModuloAssign();

        public static readonly BOF BOF = new BOF();
        public static readonly EOF EOF = new EOF();
        public static readonly EOL EOL = new EOL();
    }
}
