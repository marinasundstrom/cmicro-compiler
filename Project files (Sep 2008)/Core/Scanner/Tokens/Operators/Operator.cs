using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Ast;

namespace CSharp.Compiler.Tokens
{
    abstract public class Operator : Token<object>
    {
        private OperatorKind operatorkind;
        private int precedence;

        public Operator()
        {
            this.Value = new object();
            this.Kind = Kind.Operator;
        }

        public OperatorKind OperatorKind
        {
            get { return this.OperatorKind; }
            set { this.operatorkind = value; }
        }

        public int Precedence
        {
            get { return this.precedence; }
            set { this.precedence = value; }
        }
    }
}
