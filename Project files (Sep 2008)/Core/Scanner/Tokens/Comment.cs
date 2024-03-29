using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler.Tokens
{
    public class Comment : Token<string>
    {
        public Comment()
        {
            this.Kind = Kind.Comment;
        }

        public Comment(string Value)
        {
            this.Kind = Kind.Comment;
            this.Value = Value;
        }
    }
}
