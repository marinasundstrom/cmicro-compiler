using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    public class VariableDeclaration : Declaration, IMethodElement
    {
        public string Name;

        //Name of type
        public string TypeName;

        //Is it a pointer type?
        public bool IsPointer;

        public Expression Expression;
    }
}
