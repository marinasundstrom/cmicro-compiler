using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Interface of all attributes ('public', 'private' and so on).
    /// </summary>
    public interface IAttribute
    {
        int Kind {get;}
    }
}