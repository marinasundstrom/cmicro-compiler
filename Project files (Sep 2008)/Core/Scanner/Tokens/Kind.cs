using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public enum Kind
    {
        Identifier,
        Keyword,
        StringLiteral,
        CharLiteral,
        IntLiteral,
        RealLiteral,
        Operator,
        Delimiter,
        Comment,
        Other,
        EOL,
        EOF
    }

    public enum OperatorKind
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Equals,
        NotEquals,
        Not,
        Less,
        LessThanOrEqual,
        Greater,
        GreaterThanOrEqual,
        Modulo,
        Assign
    }
}
