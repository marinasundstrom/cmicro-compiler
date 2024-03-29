using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler
{
    public enum ParseErrorCodes
    {
        UnexpectedToken = 0,
        ExpectedToken = 1,
        Defined = 3,
        Warning = 4,
        Other = 5
    }
}
