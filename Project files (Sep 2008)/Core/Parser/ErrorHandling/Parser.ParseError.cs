using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharp.Compiler.Tokens;

namespace CSharp.Compiler
{
    public class ParseError : IParseError
    {
        public ParseError(IToken token, ParseErrorCodes errorCode)
        {
            this.token = token;
            this.errorCode = errorCode;
        }

        private IToken token;
        private ParseErrorCodes errorCode;

        #region ISyntaxError Members

        public IToken Token
        {
            get { return this.token; }
        }

        public ParseErrorCodes ErrorCode
        {
            get { return this.errorCode; }
        }

        #endregion

        public override string ToString()
        {
            if (this.errorCode == ParseErrorCodes.ExpectedToken)
            {
                return string.Format("Expected {0} at {1}:{2}.", this.token.Kind.ToString().ToLower(), this.token.Char, this.token.Line);
            }
            else if (this.errorCode == ParseErrorCodes.UnexpectedToken)
            {
                return string.Format("Unexpected {0} at {1}:{2}.", this.token.Kind.ToString().ToLower(), this.token.Char, this.token.Line);
            }
            else
                return string.Format("Undefined error at {1}:{2}.", this.token.Kind.ToString().ToLower(), this.token.Char, this.token.Line);
        }
    }

    public class ParseError2 : IParseError
    {
        public ParseError2(string message, IToken token, bool includedata)
        {
            this.message = message;
            this.includedata = includedata;
            this.token = token;
            this.errorCode = ParseErrorCodes.Defined;
        }

        private string message;
        private bool includedata;
        private IToken token;
        private ParseErrorCodes errorCode;

        #region ISyntaxError Members

        public IToken Token
        {
            get { return this.token; }
        }

        public ParseErrorCodes ErrorCode
        {
            get { return this.errorCode; }
        }

        #endregion

        public override string ToString()
        {
            if (includedata)
                return string.Format("{0} at {1}:{2}", message, token.Char, token.Line);
            else
                return string.Format("{0}", message);
        }
    }
}