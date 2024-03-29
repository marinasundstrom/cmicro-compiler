using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Compiler
{
    /// <summary>
    /// An implementation of a scanner which implements the IScanner interface.
    /// </summary>
    public class Scanner
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader">Input file</param>
        public Scanner(System.IO.TextReader reader)
        {
            this.reader = reader;
            this.reservedWords = new string[] { };
            this.ignoreCase = false;

            this.ln = 1;
            this.ch = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader">Input file</param>
        /// <param name="reservedWords">Words that will be treated as reserved keywords.</param>
        /// <param name="ignoreCase">Decides if the scanner is case-sensitive.</param>
        public Scanner(System.IO.TextReader reader, string[] reservedWords, bool ignoreCase)
        {
            this.reader = reader;
            this.reservedWords = reservedWords;
            this.ignoreCase = ignoreCase;

            this.ln = 1;
            this.ch = 0;
        }

        #endregion

        #region Fields

        private System.IO.TextReader reader;

        private int ln;
        private int ch;

        private int tokenCh = 0;
        private bool ignoreCase;

        private string[] reservedWords;

        #endregion

        #region Methods

        public Tokens.IToken Next()
        {
            Tokens.IToken token = new Tokens.Token<object>();

            //Ignore whitespaces
            while (this.LookaheadChar == ' ')
            {
                this.reader.Read();
                this.ch++;
            }

            #region EOF

            if (reader.Peek() < 0)
            {
                this.ch++;
                this.tokenCh = this.ch;

                token = new Tokens.EOF();
                token.Char = this.tokenCh;
                token.Line = this.ln;

                return token;
            }

            #endregion

            //Read characters
            char ch;
            while (this.LookaheadChar != ' ' && this.reader.Peek() > -1)
            {
                ch = this.LookaheadChar;

                #region Identifier and keyword

                if (char.IsLetter(ch) || ch == '_' || ch == '@')
                {
                    StringBuilder value = new StringBuilder(((char)this.reader.Read()).ToString());

                    this.ch++;
                    this.tokenCh = this.ch;

                    ch = this.LookaheadChar;
                    while (char.IsLetterOrDigit(ch) || ch == '_' 
                        && ch != ' ' && this.reader.Peek() > -1)
                    {
                        value.Append(((char)this.reader.Read()));
                        this.ch++;

                        ch = this.LookaheadChar;
                    }

                    //Check if the value is a reserved word or not.
                    //Create the appropriate token.
                    //if (reservedWords.Contains(value.ToString()))
                    //    token = new Tokens.Keyword(value.ToString());
                    //else
                    //    token = new Tokens.Identifier(value.ToString());


                    int i = 0;
                    bool isReserved = false;
                    while (i < reservedWords.Length && !isReserved)
                    {
                        if (string.Compare(reservedWords[i], value.ToString(), ignoreCase) == 0)
                            isReserved = true;

                        i++;
                    }

                    if(isReserved)
                        token = new Tokens.Keyword(value.ToString());
                    else
                        token = new Tokens.Identifier(value.ToString());


                    token.Char = this.tokenCh;
                    token.Line = this.Line;

                    return token;
                }

                #endregion

                #region Numbers

                else if (char.IsDigit(ch))
                {
                    string value = ((char)reader.Read()).ToString();

                    this.ch++;
                    this.tokenCh = this.ch;

                    bool isReal = false;

                    ch = this.LookaheadChar;
                    while ( 
                        ch != ';' 
                        && ch != ' ' 
                        && !IsOperator(ch) 
                        && !IsDelimiter(ch) 
                        && ch != ':' 
                        && ch != ',' 
                        && this.reader.Peek() > -1 
                        && char.IsDigit(ch))
                    {
                        if (char.IsDigit(ch))
                        {
                            value += (char)reader.Read();
                            this.ch++;

                            ch = this.LookaheadChar;
                        }
                        else if (ch == '.')
                        {
                            value += (char)reader.Read();
                            this.ch++;

                            isReal = true;

                            ch = this.LookaheadChar;
                            while (ch != ';' && ch != ' ' && !IsOperator(ch) && !IsDelimiter(ch) && ch != ':' && ch != ',' && char.IsDigit(ch) 
                                && this.reader.Peek() > -1)
                            {
                                value += (char)reader.Read();
                                this.ch++;

                                ch = this.LookaheadChar;
                            }

                            if(this.LookaheadChar == '.')
                                throw new Exception(string.Format("Syntax error at ({0}, {1}).", this.ch, this.ln));
                        }
                        else
                        {
                            throw new Exception(string.Format("Syntax error at ({0}, {1}).", this.ch, this.ln));
                        }
                    }

                    //Check if it is an integer or a real number.
                    if (isReal)
                    {
                        value = value.Replace('.', ',');
                        token = new Tokens.RealLiteral(double.Parse(value));
                    }
                    else
                        token = new Tokens.IntLiteral(int.Parse(value));

                    token.Char = this.tokenCh;
                    token.Line = this.Line;

                    return token;
                }

                #endregion

                #region String Literal

                else if (ch == '"')
                {
                    reader.Read(); //Pop character from reader stream
                    
                    this.ch++;
                    tokenCh = this.ch;

                    //Initialize StringBuilder
                    StringBuilder sb = new StringBuilder();

                    ch = (char)reader.Peek();
                    while (ch != '"')
                    {
                        sb.Append(ch);
                        reader.Read();
                        this.ch++;

                        if (reader.Peek() == -1)
                            throw new Exception(string.Format("Syntax error: Unterminated string literal at ({0}, {1}).", this.ch, this.ln));

                        ch = (char)reader.Peek();
                    }

                    //Add one for end of literal
                    this.ch++;

                    //Read the end '"'
                    reader.Read();

                    token = new Tokens.StringLiteral(sb.ToString());
                    token.Char = this.tokenCh;
                    token.Line = this.ln;

                    return token;
                }

                #endregion

                #region Char Literal

                else if (ch == '\'')
                {
                    reader.Read(); //Pop character from reader stream
                    this.ch++;
                    tokenCh = this.ch;

                    //Initialize StringBuilder
                    StringBuilder sb = new StringBuilder();

                    //How many characters read for this token
                    int charcount = 0;  //max 1 allowed for char
                    bool hasSlash = false;

                    ch = this.LookaheadChar;
                    while (ch != '\'')
                    {
                        sb.Append(ch);

                        //Add 1 to char count if this is not an escape slash.
                        if (ch == '\\')
                        {
                            if (hasSlash)
                                //Increase counter if this is not the first '\'
                                charcount++;
                            else
                                //Set as true if this is the first '\'
                                hasSlash = true;
                        }
                        else
                            //Increase counter when other characters is added
                            charcount++;

                        //Check if this token contains a valid character
                        if (charcount > 1)
                            throw new Exception(string.Format("Syntax error: Invalid character literal at ({0}, {1}).", this.ch, this.ln));

                        reader.Read();
                        this.ch++;

                        ch = (char)reader.Peek();
                    }

                    //Add one for end of iteral
                    this.ch++;
                    
                    //Read the end '
                    reader.Read();

                    token = new Tokens.CharLiteral(sb[0]);
                    token.Char = this.tokenCh;
                    token.Line = this.Line;

                    return token;
                }

                #endregion

                #region Other

                else switch (ch)
                    {
                        /* Newline character */
                        case '\n':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.EOL();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            this.ln++;
                            this.ch = 0;
                            
                            return token;

                        case '\r':
                            reader.Read();
                            break;

                        case '\t':
                            reader.Read();
                            this.ch++;
                            break;

                        /* Comma */
                        case ',':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.Comma();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            this.ln++;

                            return token;

                        /* Semicolon */
                        case ';':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.Semicolon();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Colon */
                        case ':':
                            reader.Read();
                            this.ch++;

                            ////Check if it is only ":" or operator ":="
                            //if ('=' == this.LookaheadChar)
                            //{
                            //    reader.Read();
                            //
                            //    token = new Tokens.Assign();
                            //    this.ch++;
                            //}
                            //else
                            
                            token = new Tokens.Colon();

                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Period */
                        case '.':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.Period();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* = (Assign) */
                        case '=':
                            reader.Read();
                            
                            this.ch++;
                            this.tokenCh = this.ch;

                            //Check if it is only operator "=" or "=="
                            if ('=' == this.LookaheadChar)
                            {
                                reader.Read();

                                token = new Tokens.Equal();
                                this.ch++;
                            }
                            else
                                token = new Tokens.Assign();

                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* ! (Negate/Not) */
                        case '!':
                            reader.Read();
                            
                            this.ch++;
                            this.tokenCh = this.ch;

                            //Check if it is only operator "!" or "!="
                            if ('=' == this.LookaheadChar)
                            {
                                reader.Read();

                                token = new Tokens.NotEqual();
                                this.ch++;
                            }
                            else
                                token = new Tokens.Not();

                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Add */
                        case '+':
                            reader.Read();
                            
                            this.ch++;
                            this.tokenCh = this.ch;

                            //Check if it is only operator "+" or "+="
                            if ('=' == this.LookaheadChar)
                            {
                                reader.Read();

                                token = new Tokens.AddAssign();
                                this.ch++;
                            }
                            else
                                token = new Tokens.Add();

                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Subtract */
                        case '-':
                            reader.Read();
                            
                            this.ch++;
                            this.tokenCh = this.ch;

                            //Check if it is only operator "-" or "-="
                            if ('=' == this.LookaheadChar)
                            {
                                reader.Read();

                                token = new Tokens.SubtractAssign();
                                this.ch++;
                            }
                            else
                                token = new Tokens.Subtract();

                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Divide */
                        case '/':
                            reader.Read();
                            
                            this.ch++;
                            this.tokenCh = this.ch;

                            //Check if it is only operator "/" or "/="
                            if ('=' == this.LookaheadChar)
                            {
                                reader.Read();

                                token = new Tokens.DivideAssign();
                                this.ch++;
                            }
                            else if ('/' == this.LookaheadChar)
                            {
                                //Single-line comment

                                reader.Read();

                                StringBuilder sb = new StringBuilder();

                                while (this.LookaheadChar != '\n')
                                {
                                    sb.Append(this.LookaheadChar);
                                    reader.Read();
                                    this.ch++;
                                }

                                token = new Tokens.Comment(sb.ToString());
                            }
                            else if ('*' == this.LookaheadChar)
                            {
                                //Multi-line comments

                                reader.Read();

                                StringBuilder sb = new StringBuilder();
                                bool go = true;

                                while (go == true && this.reader.Peek() > -1)
                                {
                                    if (this.LookaheadChar == '\n' )
                                    {
                                        this.ln++;
                                    }
                                    else
                                        sb.Append(this.LookaheadChar);
                                    
                                    this.ch++;
                                    reader.Read();

                                    if(this.LookaheadChar == '*')
                                    {
                                        reader.Read();
                                        this.ch++;

                                        if (this.LookaheadChar == '/')
                                        {
                                            go = false;
                                            reader.Read();
                                            this.ch++;
                                        }
                                    }
                                    else if (((int)this.LookaheadChar) == -1)
                                    {
                                        go = false;
                                    }
                                }

                                token = new Tokens.Comment(sb.ToString());
                            }
                            else
                                token = new Tokens.Divide();

                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Multiply */
                        case '*':
                            reader.Read();
                            
                            this.ch++;
                            this.tokenCh = this.ch;

                            //Check if it is only operator "*" or "*="
                            if ('=' == this.LookaheadChar)
                            {
                                reader.Read();

                                token = new Tokens.MultiplyAssign();
                                this.ch++;
                            }
                            else
                                token = new Tokens.Multiply();

                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Brackets */
                        case '{':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.LeftBracket();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        case '}':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.RightBracket();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Paranthesis */
                        case '(':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.LeftParenthesis();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        case ')':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.RightParenthesis();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Square Brackets */
                        case '[':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.LeftSquareBracket();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        case ']':
                            this.ch++;
                            this.tokenCh = this.ch;

                            reader.Read();
                            token = new Tokens.RightSquareBracket();
                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        /* Angle Brackets */
                        case '<':
                            reader.Read();
                            
                            this.ch++;
                            this.tokenCh = this.ch;

                            //Check if it is only operator "<" or "<="
                            if ('=' == this.LookaheadChar)
                            {
                                reader.Read();

                                token = new Tokens.LessThanOrEqual();
                                this.ch++;
                            }
                            else
                                token = new Tokens.LeftAngleBracket();

                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        case '>':
                           reader.Read();
                            
                            this.ch++;
                            this.tokenCh = this.ch;

                            //Check if it is only operator ">" or ">="
                            if ('=' == this.LookaheadChar)
                            {
                                reader.Read();

                                token = new Tokens.GreaterThanOrEqual();
                                this.ch++;
                            }
                            else
                                token = new Tokens.RightAngleBracket();

                            token.Char = this.tokenCh;
                            token.Line = this.ln;

                            return token;

                        default:
                            throw new Exception(string.Format("Syntax error at ({0}, {1}).", this.ch, this.ln));


                    }

                #endregion

            }

            //Return the token
            return token;
        }

        public List<Tokens.IToken> Scan()
        {
            return this.Scan(false);
        }

        public List<Tokens.IToken> Scan(bool removeComments)
        {
            List<Tokens.IToken> tokens = new List<Tokens.IToken>();

            Tokens.IToken token;

            while(true)
            {
                token = this.Next();
                tokens.Add(token);

                if (token.Kind == Tokens.Kind.EOF)
                {
                    if(removeComments)
                        tokens.RemoveAll(x => x.Kind == CSharp.Compiler.Tokens.Kind.Comment);

                    return tokens;
                }
            }
        }

        #region Extensions

        List<char> operators;
        public bool IsOperator(char c)
        {
            if (this.operators == null)
            {
                this.operators = new List<char>()
                {
                    '+', '-', '/', '*', '%', '=', '!'
                };
            }

            return operators.Contains(c);
        }

        List<char> delimiters;
        public bool IsDelimiter(char c)
        {
            if (this.delimiters == null)
            {
                this.delimiters = new List<char>()
                {
                    '[', ']', '(', ')', '{', '}', '<', '>'
                };
            }

            return delimiters.Contains(c);
        }
            #endregion

        #endregion

        #region Properties

        public int Line
        {
            get { return this.ln; }
        }

        public int Char
        {
            get { return this.ch; }
        }

        private char LookaheadChar
        {
            get { return (char)reader.Peek(); }
        }

        #endregion

        /// <summary>
        /// Default keywords defined for the specific language.
        /// </summary>
        public static string[] Keywords = { "int", "string", "double", "bool", "char", "void", "return", "import", "namespace", "class", "extends", "implements", "delegate", "interface", "public", "private", "abstract", "static", "const", "get", "set" };
    }
}
