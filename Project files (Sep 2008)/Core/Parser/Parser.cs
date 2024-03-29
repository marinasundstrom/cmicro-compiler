using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using CSharp.Compiler.Tokens;
using CSharp.Compiler.Ast;

namespace CSharp.Compiler
{
    /// <summary>
    /// An implementation of a parser which implements the IParser interface.
    /// </summary>
    public partial class Parser : IParser
    {
        #region Fields

        private bool ignoreCase;
        private bool abortOnError;
        private List<IParseError> errors;

        private List<IToken> input;
        private AstRoot root;

        private int index;

        private Thread parsingThread;

        #endregion

        public Parser(List<IToken> tokens, bool ignoreCase)
        {
            #region Parser parameters

            //Parser parameters
            this.input = new List<IToken>();
            this.input.Add(Tokens.Tokens.BOF);
            this.input.AddRange(tokens);

            //Initialize the error list.
            this.errors = new List<IParseError>();

            this.ignoreCase = ignoreCase;
            this.abortOnError = true;

            #endregion

            #region Start parsing

            // Uncommented this code
            /*
            //Create a separate thread for the parse method and run it.
            this.parsingThread = new Thread(new ThreadStart(this.Parse));
            this.parsingThread.Start();

            //Wait for parse method to finish and then continue.
            parsingThread.Join();
            */

            #endregion
        }

        // INFO: Made public
        public void Parse()
        {
            this.root = new Ast.Program();
            Program program = (Program)this.root;

            #region Variables

            List<IAttribute> attrStore = new List<IAttribute>();

            #endregion

            while (this.input.Count > 0)
            {
                #region Elements to parse

                if (this.NextIs("import"))
                {
                    program.References.Add(this.ParseImportStatement());
                }
                else if ((this.NextIsAttribute() || !this.NextIsAttribute())
                                && !this.NextIs(Kind.EOF))
                {
                    attrStore = this.ParseAttributes(false, false, false);

                    IProgramDeclaration member = new NullMember();

                    if (this.NextIs("namespace"))
                    {
                        if (attrStore.Count > 0)
                            this.ReportSyntaxError("Syntax error, invalid modifiers for namespace declaration", this.CurrentToken, true);

                        NamespaceDeclaration namespaced = this.ParseNamespaceDeclaration();

                        attrStore.Clear();

                        member = namespaced;
                    }
                    else if (this.NextIs("delegate"))
                    {
                        DelegateDeclaration deld = this.ParseDelegateDeclaration();
                        deld.Attributes = attrStore;

                        member = deld;
                    }
                    else if (NextIs("class") ||
                        (this.NextIs("abstract") && this.CheckIndex(2, "class")))
                    {
                        this.ParseAttribute("abstract", attrStore, false);

                        ClassDeclaration classd = this.ParseClassDeclaration();
                        classd.Attributes.AddRange(attrStore);
                        attrStore.Clear();

                        program.Members.Add(classd);
                    }
                    else if (this.NextIs("enum"))
                    {
                        EnumDeclaration enumdecl = this.ParseEnumDeclaration();
                        enumdecl.Attributes = attrStore;

                        member = enumdecl;
                    }
                    else if (NextIs("interface"))
                    {
                        InterfaceDeclaration interfaced = this.ParseInterfaceDeclaration();
                        interfaced.Attributes = attrStore;

                        member = interfaced;
                    }
                    else if (this.NextIs(Kind.Identifier, Kind.Keyword))
                    {
                        IType type = this.ParseType();
                        string name = this.NextIf(Kind.Identifier).GetValue();

                        if (NextIs("("))
                        {
                            //Method declaration

                            MethodDeclaration decl = new MethodDeclaration();
                            decl.Attributes = attrStore;
                            decl.ReturnType = type;
                            decl.Name = name;
                            decl.Arguments = this.ParseMethodArguments();

                            this.NextWhile(Kind.EOL);
                            decl.Code = this.ParseCodeBlock();
                            this.IsMainMethod(decl);

                            member = decl;
                        }
                        else if (NextIs("<"))
                        {
                            //Generic method declaration

                            MethodDeclaration decl = new MethodDeclaration();
                            decl.Attributes = attrStore;
                            decl.ReturnType = type;
                            decl.Name = name;

                            if (this.NextIs("<"))
                            {
                                this.Next();

                                while (!this.NextIs(">"))
                                {
                                    decl.GenricParameters.Add(this.ParseType());

                                    if (!this.NextIs(">"))
                                        this.NextIf(",");
                                }

                                decl.IsGeneric = true;

                                this.Next();
                            }

                            decl.Arguments = this.ParseMethodArguments();

                            this.NextWhile(Kind.EOL);
                            decl.Code = this.ParseCodeBlock();
                            this.IsMainMethod(decl);

                            member = decl;
                        }
                        else
                            this.ReportSyntaxError("Expected class, struct, interface or enum definition", this.CurrentToken, true);
                    }
                    else
                        this.ParseCases();


                    /* Following lines will be executed */

                    #region Parsing check

                    //Check if this parse is valid, if not report an error.
                    if (member is NullMember && attrStore.Count > 0)
                        this.ReportSyntaxError("Unexpected attribute", this.CurrentToken, true);

                    #endregion

                    if (member.GetType() != typeof(NullMember))
                        program.Members.Add(member);
                }
                else
                    this.ParseCases();

                #endregion
            }
        }

        #region IParser Members

        public IToken LookaheadToken
        {
            get { return this.input[index + 1]; }
        }

        public IToken CurrentToken
        {
            get { return this.input[index]; }
        }

        public AstRoot Result
        {
            get { return this.root; }
        }

        public bool IgnoreCase
        {
            get { return this.ignoreCase; }
        }

        public bool AbortOnError
        {
            get { return this.abortOnError; }
        }

        public IToken Next()
        {
            index++;
            return this.CurrentToken;
        }

        public void GoBack()
        {
            index--;
        }

        public void GoBack(int steps)
        {
            index -= steps;
        }

        public void Abort()
        {
            this.parsingThread.Abort();
        }

        public IToken NextIf(string str)
        {
            if (string.Compare(this.LookaheadToken.GetValue(), str, this.ignoreCase) == 0)
            {
                Next();
                return this.CurrentToken;
            }
            else
            {
                this.ReportSyntaxError(this.LookaheadToken, ParseErrorCodes.UnexpectedToken);
                return null;
            }
        }

        public IToken NextIf(Kind tokenType)
        {
            if (this.LookaheadToken.Kind == tokenType)
            {
                Next();
                return this.CurrentToken;
            }
            else
            {
                this.ReportSyntaxError(this.LookaheadToken, ParseErrorCodes.UnexpectedToken);
                return null;
            }
        }

        public IToken NextIf(Kind kind1, Kind kind2)
        {
            if (this.LookaheadToken.Kind == kind1 || this.LookaheadToken.Kind == kind2)
            {
                Next();
                return this.CurrentToken;
            }
            else
            {
                this.ReportSyntaxError(this.LookaheadToken, ParseErrorCodes.UnexpectedToken);
                return null;
            }
        }

        public IToken NextIf(OperatorKind Operator)
        {
            if (this.LookaheadToken is Tokens.Operator)
            {
                Tokens.Operator op = (Tokens.Operator)this.LookaheadToken;

                if (op.OperatorKind == Operator)
                {
                    Next();
                    return this.CurrentToken;
                }
            }

            this.ReportSyntaxError(this.LookaheadToken, ParseErrorCodes.UnexpectedToken);
            return null;
        }

        public bool NextIs(string str)
        {
            if (string.Compare(this.LookaheadToken.GetValue(), str, this.ignoreCase) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NextIs(Kind tokenType)
        {
            if (this.LookaheadToken.Kind == tokenType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NextIs(Tokens.Kind Kind1, Tokens.Kind Kind2)
        {
            return this.LookaheadToken.Kind == Kind1 || this.LookaheadToken.Kind == Kind2;
        }

        public bool NextIs(OperatorKind Operator)
        {
            if (this.LookaheadToken is Tokens.Operator)
            {
                Tokens.Operator op = (Tokens.Operator)this.LookaheadToken;

                if (op.OperatorKind == Operator)
                {
                    return true;
                }
            }

            this.ReportSyntaxError(this.LookaheadToken, ParseErrorCodes.UnexpectedToken);
            return false;
        }

        public bool NextNameIs(string str)
        {
            int index = this.index;
            string n = this.ParseName();
            this.index = index;

            if (n == str)
                return true;
            else
                return false;
        }

        public bool CheckIndex(int Ift, string str)
        {
            return this.TokenIs(this.input[index + Ift], str);
        }

        public void NextWhile(string str)
        {
            while (string.Compare(this.LookaheadToken.GetValue(), str, this.ignoreCase) == 0)
            {
                Next();
            }
        }

        public void NextWhile(Kind tokenType)
        {
            while (this.LookaheadToken.Kind == tokenType)
            {
                Next();
            }
        }

        public bool SameKind(IToken token1, IToken token2)
        {
            return token1.GetType() == token2.GetType();
        }

        public bool TokenIs(IToken Token, Tokens.Kind Kind)
        {
            return Token.Kind == Kind;
        }

        public bool TokenIs(IToken Token, Tokens.Kind Kind1, Tokens.Kind Kind2)
        {
            return Token.Kind == Kind1 || Token.Kind == Kind2;
        }

        public bool TokenIs(IToken Token, Tokens.OperatorKind OperatorKind)
        {
            if (Token is Tokens.Operator)
            {
                return ((Tokens.Operator)Token).OperatorKind == OperatorKind;
            }

            return false;
        }

        public bool TokenIs(IToken Token, string Value)
        {
            return Token.GetValue() == Value;
        }

        public List<IParseError> Errors
        {
            get { return this.errors; }
        }


        public void ReportSyntaxError(IToken token, ParseErrorCodes errorCode)
        {
            this.errors.Add(new ParseError(token, errorCode));

            if (this.abortOnError)
                this.parsingThread.Abort();
        }

        public void ReportSyntaxError(string message, IToken token, bool includeData)
        {
            this.errors.Add(new ParseError2(message, token, includeData));

            if (this.abortOnError)
                this.parsingThread.Abort();
        }

        #endregion
    }
}