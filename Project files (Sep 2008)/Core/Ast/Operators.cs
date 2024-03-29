using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    public enum Operator
    {
        OR,
        AND,
        Mod,
        Mul,
        Div,
        Add,
        Sub
    }

    public class Operators
    {
        public static object[] OR = {0, "||"};
        public static object[] AND = {1, "&&"};
        public static object[] Mod = { 2, "%" };
        public static object[] Mul = { 3, "*" };
        public static object[] Div = { 4, "/" };
        public static object[] Add = { 5, "+" };
        public static object[] Sub = { 6, "-" };
    }
}
