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
        public string ParseName()
        {
            this.Next(); //Moves pointer to the next token

            StringBuilder sb = new StringBuilder();
            bool t = true;

            while (t)
            {
                if (TokenIs(this.CurrentToken, Kind.Identifier, Kind.Keyword) && this.LookaheadToken.GetValue() == ".")
                {
                    sb.Append(this.CurrentToken.GetValue());
                    sb.Append(this.Next().GetValue());
                    this.Next();
                }
                //else if (this.NextIs(Kind.Identifier, Kind.Keyword) && this.CheckIndex(2, ","))
                //{
                //    this.Next();
                //    sb.Append(this.CurrentToken.GetValue());

                //    t = false;
                //}
                else if (TokenIs(this.CurrentToken, Kind.Identifier, Kind.Keyword) && TokenIs(this.LookaheadToken, Kind.Identifier, Kind.Keyword))
                {
                    sb.Append(this.CurrentToken.GetValue());

                    t = false;
                }
                #region Old code
                //else if (TokenIs(this.CurrentToken, Kind.Identifier, Kind.Keyword) &&
                //            (this.NextIs("[") || this.NextIs("*")))
                //{
                //    if (this.NextIs("["))
                //    {
                //        sb.Append(this.CurrentToken.GetValue());
                //        sb.Append(this.NextIf("["));
                //        sb.Append(this.NextIf("]"));
                            
                //    }
                //    else if (this.NextIs("*"))
                //    {
                //        sb.Append(this.Next().GetValue());
                //        sb.Append(this.CurrentToken.GetValue());
                        
                //    }

                //    t = false;
                //}

                #endregion
                else if (TokenIs(this.CurrentToken, Kind.Identifier, Kind.Keyword) &&
                            (!this.NextIs(".") || !this.NextIs(",") || this.NextIs(Kind.Identifier, Kind.Keyword)))
                {
                    sb.Append(this.CurrentToken.GetValue());

                    t = false;
                }
                #region Old Code
                else if (TokenIs(this.CurrentToken, Kind.Identifier, Kind.Keyword) &&
                            (this.LookaheadToken.GetValue() == ";" || this.LookaheadToken.GetValue() == "[" || this.LookaheadToken.GetValue() == "," || this.LookaheadToken.GetValue() == "(" || this.LookaheadToken.Kind == Kind.EOL || this.LookaheadToken.Kind == Kind.Operator))
                {
                    sb.Append(this.CurrentToken.GetValue());

                    t = false;
                }
                #endregion
                else if (TokenIs(this.CurrentToken,"."))
                {
                    this.ReportSyntaxError("Invalid identifier", this.CurrentToken, true);
                    this.Next();
                }
                else
                    t = false;
            }

            return sb.ToString();
        }
    }
}
