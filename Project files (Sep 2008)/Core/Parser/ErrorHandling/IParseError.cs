using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Tokens;
using CSharp.Compiler.Ast;

namespace CSharp.Compiler
{
    public interface IParseError
    {
        IToken Token { get; }
        ParseErrorCodes ErrorCode { get; }
    }
}
