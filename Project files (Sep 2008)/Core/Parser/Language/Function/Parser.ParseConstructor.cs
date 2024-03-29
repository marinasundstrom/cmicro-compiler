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
        public ConstructorDeclaration ParseConstructorDeclaration()
        {
            ConstructorDeclaration methd = new ConstructorDeclaration();

            //methd.Attributes = this.ParseMethodAttributes();

            /*
             *  <ReturnType> <Name> ( <Arguments> )
             *  {
             *      <Code>
             *  }
             *  
             * */

            methd.Name = this.NextIf(Kind.Identifier).GetValue();

            methd.Arguments = this.ParseMethodArguments();

            this.NextWhile(Kind.EOL);
            methd.Code = this.ParseCodeBlock();
            this.NextWhile(Kind.EOL);

            return methd;
        }
    }
}