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
        public MethodDeclaration ParseMethodDeclaration()
        {
            MethodDeclaration methd = new MethodDeclaration();

            //methd.Attributes = this.ParseMethodAttributes();

            /*
             *  <ReturnType> <Name> ( <Arguments> )
             *  {
             *      <Code>
             *  }
             *  
             * */

            methd.ReturnType = this.ParseType();

            methd.Name = this.NextIf(Kind.Identifier).GetValue();
            this.IsMainMethod(methd);


            //if (this.NextIs("<"))
            //{
            //    this.Next();

            //    while (!this.NextIs(">"))
            //    {
            //        methd.GenricParameters.Add(this.ParseType());

            //        if (!this.NextIs(">"))
            //            this.NextIf(",");
            //    }

            //    this.Next();

            //    methd.IsGeneric = true;
            //}

            methd.Arguments = this.ParseMethodArguments();

            this.NextWhile(Kind.EOL);
            methd.Code = this.ParseCodeBlock();
            this.NextWhile(Kind.EOL);

            return methd;
        }
    }
}