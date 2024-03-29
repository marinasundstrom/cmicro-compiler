using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Ast
{
    /// <summary>
    /// Symbolizes a field declaration.
    /// </summary>
    /// 
    public class FieldVariableDeclaration : Declaration, IClassMember
    {
        public string Name;

        public CSharp.Compiler.Ast.IType Type;

        //public string TypeName;
        //public bool IsPointer;
        //public bool IsArray;

        //public bool IsGeneric;
        //public string GenericParameter;

        public Expression Expression;

        public List<IAttribute> Attributes = new List<IAttribute>();
    }
}
