using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using CSharp.Compiler.Tokens;
using CSharp.Compiler.Ast;

namespace CSharp.Compiler
{
    public partial class Parser : IParser
    {
        public FieldVariableDeclaration ParseFieldVariableDeclaration()
        {
            FieldVariableDeclaration field = new FieldVariableDeclaration();

            field.Type = this.ParseType();
            field.Name = this.ParseName();

            this.NextIf(";");

            return field;
        }
    }
}
