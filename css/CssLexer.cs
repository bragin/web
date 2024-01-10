using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    public enum CssTokenType
    {
        CSS_TOKEN_IDENT, CSS_TOKEN_ATKEYWORD, CSS_TOKEN_HASH,
        CSS_TOKEN_FUNCTION, CSS_TOKEN_STRING, CSS_TOKEN_INVALID_STRING,
        CSS_TOKEN_URI, CSS_TOKEN_UNICODE_RANGE, CSS_TOKEN_CHAR,
        CSS_TOKEN_NUMBER, CSS_TOKEN_PERCENTAGE, CSS_TOKEN_DIMENSION,

        // Those tokens that want strings interned appear above
        CSS_TOKEN_LAST_INTERN,

        CSS_TOKEN_CDO, CSS_TOKEN_CDC, CSS_TOKEN_S, CSS_TOKEN_COMMENT,
        CSS_TOKEN_INCLUDES, CSS_TOKEN_DASHMATCH, CSS_TOKEN_PREFIXMATCH,
        CSS_TOKEN_SUFFIXMATCH, CSS_TOKEN_SUBSTRINGMATCH, CSS_TOKEN_EOF
    }

    public struct CssToken
    {
        public CssTokenType Type;

        public int DataIndex; // struct data
        public int DataLen;  // struct data

        public string iData; // interned data

        public int Col;
        public int Line;

        public int Error;

        public CssToken(CssTokenType type)
        {
            Type = type;
            DataIndex = 0;
            DataLen = 0;
            iData = "";
            Col = 0;
            Line = 0;

            Error = 0;
        }

        public CssToken(int error)
        {
            Type = CssTokenType.CSS_TOKEN_EOF;
            DataIndex = 0;
            DataLen = 0;
            iData = "";
            Col = 0;
            Line = 0;

            Error = error;
        }

        public CssToken(CssToken t)
        {
            Type = t.Type;
            DataIndex = t.DataIndex;
            DataLen = t.DataLen;
            iData = t.iData;
            Col = t.Col;
            Line = t.Line;
            Error = t.Error;
        }

        public bool IsChar(char c)
        {
            bool result = false;

            if (Type == CssTokenType.CSS_TOKEN_CHAR && iData.Length == 1)
            {
                char d = iData[0];

                // Ensure lowercase comparison
                d = Char.ToLower(d);

                result = (d == c);
            }

            return result;
        }
    }

    /** \todo Optimisation -- we're currently revisiting a bunch of input
     *	  characters (Currently, we're calling parserutils_inputstream_peek
     *	  about 1.5x the number of characters in the input stream). Ideally,
     *	  we'll visit each character in the input exactly once. In reality,
     *	  the upper bound is twice, due to the need, in some cases, to read
     *	  one character beyond the end of a token's input to detect the end
     *	  of the token. Resumability adds a little overhead here, unless
     *	  we're somewhat more clever when it comes to having support for
     *	  restarting mid-escape sequence. Currently, we rewind back to the
     *	  start of the sequence and process the whole thing again.
     */

    public enum CssLexerState : int
    {
        sSTART = 0,
        sATKEYWORD = 1,
        sSTRING = 2,
        sHASH = 3,
        sNUMBER = 4,
        sCDO = 5,
        sCDC = 6,
        sS = 7,
        sCOMMENT = 8,
        sMATCH = 9,
        sURI = 10,
        sIDENT = 11,
        sESCAPEDIDENT = 12,
        sURL = 13,
        sUCR = 14
    };

    internal class CssLexer
    {
        //MemoryStream Stream;
        TextSource Source;
        int BytesReadForToken;
        CssToken Token;

        CssLexerState State;
        int Substate;

        // Context
        char First;         // First character read for token
        int OrigBytes;   // < Storage of current number of bytes read, for rewinding
        bool LastWasStar;   // Whether the previous character was an asterisk
        bool LastWasCR;     // Whether the previous character was CR
        int CurrentCol;     // Current column in source
        int CurrentLine;    // Current line in source
        bool EscapeSeen;

        bool EmitComments;

        // libcss/src/lex.c:174
        public CssLexer()
        {
            State = CssLexerState.sSTART;
            Substate = 0;

            Token = new CssToken();
            BytesReadForToken = 0;

            First = (char)0;
            LastWasStar = false;
            LastWasCR = false;
            CurrentCol = 1;
            CurrentLine = 1;
            EscapeSeen = false;

            EmitComments = false;
        }

        public void SetSource(TextSource source)
        {
            Source = source;
        }

        // lex.c:262
        /**
         * Retrieve a token from a lexer
         *
         * \param lexer  The lexer instance to read from
         * \param token  Pointer to location to receive pointer to token
         * \return CSS_OK on success, appropriate error otherwise
         *
         * The returned token object is owned by the lexer. However, the client is
         * permitted to modify the data members of the token. The token must not be
         * freed by the client (it may not have been allocated in the first place),
         * nor may any of the pointers contained within it. The client may, if they
         * wish, overwrite any data member of the returned token object -- the lexer
         * does not depend on these remaining constant. This allows the client code
         * to efficiently implement a push-back buffer with interned string data.
         */
        public CssStatus GetToken(out CssToken token)
        {
            switch (State)
            {
                case CssLexerState.sSTART:
                    start:
                    return Start(out token);
                case CssLexerState.sATKEYWORD:
                    return AtKeyword(out token);
                case CssLexerState.sSTRING:
                    return String(out token);
                case CssLexerState.sHASH:
                    return Hash(out token);
                case CssLexerState.sNUMBER:
                    return NumberOrPercentageOrDimension(out token);
                case CssLexerState.sCDO:
                    return CDO(out token);
                case CssLexerState.sCDC:
                    return CDCOrIdentOrFunctionOrNPD(out token);
                case CssLexerState.sS:
                    return S(out token);
                case CssLexerState.sCOMMENT:
                    Comment(out token);
                    if (!EmitComments && token.Error == 0 &&
                            token.Type == CssTokenType.CSS_TOKEN_COMMENT)
                    {
                        goto start;
                    }
                    return CssStatus.CSS_OK;
                case CssLexerState.sMATCH:
                    return Match(out token);
                case CssLexerState.sURI:
                    return URI(out token);
                case CssLexerState.sIDENT:
                    return IdentOrFunction(out token);
                case CssLexerState.sESCAPEDIDENT:
                    return EscapedIdentOrFunction(out token);
                case CssLexerState.sURL:
                    return URI(out token);
                case CssLexerState.sUCR:
                    return UnicodeRange(out token);
                default:
                    throw new Exception();
            }
        }


        // lex.c:319
        /**
         * Append some data to the current token
         *
         * \param lexer  The lexer instance
         * \param data   Pointer to data to append
         * \param len    Length, in bytes, of data
         * \return CSS_OK on success, appropriate error otherwise
         *
         * This should not be called directly without good reason. Use the APPEND()
         * macro instead.
         */
        CssStatus AppendToTokenData(int data, int len)
        {
            if (EscapeSeen)
            {
                /*
                    css_error error =
                        css_error_from_parserutils_error(
                            parserutils_buffer_append(lexer->unescapedTokenData, data, len)
                            );
                    if (error != CSS_OK)
                        return error;
                */
                Log.Unimplemented("EscapeSeen");
            }

            Token.DataLen += len;

            return CssStatus.CSS_OK;
        }

        // lex.c:354
        /**
         * Prepare a token for consumption and emit it to the client
         *
         * \param lexer  The lexer instance
         * \param type   The type of token to emit
         * \param token  Pointer to location to receive pointer to token
         * \return CSS_OK on success, appropriate error otherwise
         */
        CssToken EmitToken(CssTokenType type)
        {
            var t = new CssToken(Token);
            t.Type = type;

            if (EscapeSeen)
            {
                Log.Unimplemented("EscapeSeen");
            }
            else
            {
                if (type == CssTokenType.CSS_TOKEN_EOF)
                {
                    t.DataIndex = -1;
                }
                else
                {
                    // Current position in the stream is the index
                    t.DataIndex = Source.Index;
                }
            }

            switch (type)
            {
                case CssTokenType.CSS_TOKEN_ATKEYWORD:
                    // Strip the '@' from the front
                    t.DataIndex += 1;
                    t.DataLen -= 1;
                    break;
                case CssTokenType.CSS_TOKEN_STRING:
                    // Strip the leading quote
                    t.DataIndex += 1;
                    t.DataLen -= 1;

                    // Strip the trailing quote, iff it exists (may have hit EOF)
                    Log.Unimplemented();
                    /*
                    if (t->data.len > 0 && (t->data.data[t->data.len - 1] == '"' ||
                            t->data.data[t->data.len - 1] == '\''))
                    {
                        t.DataLen -= 1;
                    }*/
                    break;
                case CssTokenType.CSS_TOKEN_INVALID_STRING:
                    // Strip the leading quote
                    t.DataIndex += 1;
                    t.DataLen -= 1;
                    break;
                case CssTokenType.CSS_TOKEN_HASH:
                    // Strip the '#' from the front
                    t.DataIndex += 1;
                    t.DataLen -= 1;
                    break;
                case CssTokenType.CSS_TOKEN_PERCENTAGE:
                    // Strip the '%' from the end
                    t.DataLen -= 1;
                    break;
                case CssTokenType.CSS_TOKEN_DIMENSION:
                    break;
                case CssTokenType.CSS_TOKEN_URI:
                    // Strip the "url(" from the start
                    Log.Unimplemented();
                    /*
                    t.DataIndex += SLEN("url(");
                    t.DataLen -= SLEN("url(");

                    // Strip any leading whitespace
                    while (isSpace(t->data.data[0]))
                    {
                        t.DataIndex++;
                        t.DataLen--;
                    }

                    // Strip any leading quote
                    if (t->data.data[0] == '"' || t->data.data[0] == '\'')
                    {
                        t.DataIndex += 1;
                        t.DataLen -= 1;
                    }

                    // Strip the trailing ')'
                    t.DataLen -= 1;

                    // Strip any trailing whitespace
                    while (t->data.len > 0 &&
                            isSpace(t->data.data[t->data.len - 1]))
                    {
                        t.DataLen--;
                    }

                    // Strip any trailing quote
                    if (t->data.len > 0 && (t->data.data[t->data.len - 1] == '"' ||
                            t->data.data[t->data.len - 1] == '\''))
                    {
                        t.DataLen -= 1;
                    }*/
                    break;
                case CssTokenType.CSS_TOKEN_UNICODE_RANGE:
                    // Remove "U+" from the start
                    Log.Unimplemented();
                    /*
                    t.DataIndex += SLEN("U+");
                    t.DataLen -= SLEN("U+");*/
                    break;
                case CssTokenType.CSS_TOKEN_COMMENT:
                    Log.Unimplemented();
                    // Strip the leading '/' and '*'
                    //t.DataIndex += SLEN("/*");
                    //t.DataLen -= SLEN("/*");

                    // Strip the trailing '*' and '/'
                    //t.DataLen -= SLEN("*/");
                    break;
                case CssTokenType.CSS_TOKEN_FUNCTION:
                    // Strip the trailing '('
                    t.DataLen -= 1;
                    break;
                default:
                    break;
            }

            // Reset the lexer's state
            State = CssLexerState.sSTART;
            Substate = 0;

            return t;
        }

        // lex.c:1156
        CssStatus Start(out CssToken token)
        {
            bool emitEOF = false;
start:
            // Advance past the input read for the previous token
            if (BytesReadForToken > 0)
            {
                //parserutils_inputstream_advance(
                //      lexer->input, lexer->bytesReadForToken);
                //Console.WriteLine("lexer:384 Advance not implemented!");
                Source.Index += BytesReadForToken;
                BytesReadForToken = 0;
            }

            // Reset in preparation for the next token
            Token.Type = CssTokenType.CSS_TOKEN_EOF;
            //Token.data.data = NULL;
            Token.iData = "";
            Token.DataIndex = -1;
            Token.DataLen = 0;
            Token.Col = CurrentCol;
            Token.Line = CurrentLine;
            EscapeSeen = false;
            //if (lexer->unescapedTokenData != NULL)
            //  lexer->unescapedTokenData->length = 0;

            // Check for EOF in the stream
            if (Source.Index >= Source.Length)
            {
                if (emitEOF)
                {
                    Console.WriteLine("lexer:404 Emitting EOF token loosin Col and Line data");
                    token = EmitToken(CssTokenType.CSS_TOKEN_EOF);
                    return CssStatus.CSS_EOF;
                }
                else
                {
                    token = new CssToken(1); // FIXME: Get rid of this
                    return CssStatus.CSS_NEEDDATA;
                }
            }

            // Peek one character from the stream
            var c = Source[Source.Index];
            AppendToTokenData(c, 1);
            BytesReadForToken++;
            CurrentCol++;

            /*if (clen > 1 || c >= 0x80)
            {
                lexer->state = sIDENT;
                lexer->substate = 0;
                return IdentOrFunction(lexer, token);
            }*/

            switch (c)
            {
                case '@':
                    State = CssLexerState.sATKEYWORD;
                    Substate = 0;
                    AtKeyword(out token);
                    return CssStatus.CSS_OK;
                case '"':
                case '\'':
                    State = CssLexerState.sSTRING;
                    Substate = 0;
                    First = c;
                    String(out token);
                    return CssStatus.CSS_OK;
                case '#':
                    State = CssLexerState.sHASH;
                    Substate = 0;
                    OrigBytes = BytesReadForToken;
                    Hash(out token);
                    return CssStatus.CSS_OK;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '.':
                case '+':
                    State = CssLexerState.sNUMBER;
                    Substate = 0;
                    First = c;
                    NumberOrPercentageOrDimension(out token);
                    return CssStatus.CSS_OK;
                case '<':
                    State = CssLexerState.sCDO;
                    Substate = 0;
                    CDO(out token);
                    return CssStatus.CSS_OK;
                case '-':
                    State = CssLexerState.sCDC;
                    Substate = 0;
                    CDCOrIdentOrFunctionOrNPD(out token);
                    return CssStatus.CSS_OK;
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                case '\f':
                    State = CssLexerState.sS;
                    Substate = 0;
                    if (c == '\n' || c == '\f')
                    {
                        CurrentCol = 1;
                        CurrentLine++;
                    }
                    LastWasCR = (c == '\r');
                    S(out token);
                    return CssStatus.CSS_OK;
                case '/':
                    State = CssLexerState.sCOMMENT;
                    Substate = 0;
                    LastWasStar = false;
                    LastWasCR = false;
                    Comment(out token);
                    if (!EmitComments && token.Type == CssTokenType.CSS_TOKEN_COMMENT)
                        goto start;
                    return CssStatus.CSS_OK;
                case '~':
                case '|':
                case '^':
                case '$':
                case '*':
                    State = CssLexerState.sMATCH;
                    Substate = 0;
                    First = c;
                    Match(out token);
                    return CssStatus.CSS_OK;
                case 'u':
                case 'U':
                    State = CssLexerState.sURI;
                    Substate = 0;
                    URIOrUnicodeRangeOrIdentOrFunction(out token);
                    return CssStatus.CSS_OK;
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't': /*  'u'*/
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T': /*  'U'*/
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                case '_':
                    State = CssLexerState.sIDENT;
                    Substate = 0;
                    IdentOrFunction(out token);
                    return CssStatus.CSS_OK;
                case '\\':
                    State = CssLexerState.sESCAPEDIDENT;
                    Substate = 0;
                    EscapedIdentOrFunction(out token);
                    return CssStatus.CSS_OK;
                case '>':
                    // Check for >=
                    Log.Unimplemented();
                    /*
                    perror = parserutils_inputstream_peek(lexer->input,
                            lexer->bytesReadForToken, &cptr, &clen);
                    if (perror != PARSERUTILS_OK && perror != PARSERUTILS_EOF)
                    {
                        return css_error_from_parserutils_error(perror);
                    }

                    if (perror == PARSERUTILS_EOF)
                    {
                        return emitToken(lexer, CSS_TOKEN_CHAR, token);
                    }

                    c = *cptr;

                    if (c == '=')
                    {
                        APPEND(lexer, cptr, clen);
                    }*/
                    token = EmitToken(CssTokenType.CSS_TOKEN_CHAR);
                    return CssStatus.CSS_OK;
                default:
                    token = EmitToken(CssTokenType.CSS_TOKEN_CHAR);
                    return CssStatus.CSS_OK;
            }

            return CssStatus.CSS_OK;
        }

        #region State machine components
        // lex.c:481
        CssStatus AtKeyword(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }

        // lex.c:1301
        CssStatus String(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }
        // lex.c:1321
        CssStatus URIOrUnicodeRangeOrIdentOrFunction(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }

        // lex.c:870
        CssStatus Hash(out CssToken token)
        {
            /* HASH = '#' name  = '#' nmchar+
             *
             * The '#' has been consumed.
             */
            ConsumeNMChars();

            // Require at least one NMChar otherwise, we're just a raw '#'
            if (BytesReadForToken - OrigBytes > 0)
            {
                token = EmitToken(CssTokenType.CSS_TOKEN_HASH);
                return CssStatus.CSS_OK;
            }

            token = EmitToken(CssTokenType.CSS_TOKEN_CHAR);
            return CssStatus.CSS_OK;
        }

        // lex.c:996
        CssStatus NumberOrPercentageOrDimension(out CssToken token)
        {
            char c;
            const int Initial = 0, Dot = 1, MoreDigits = 2, Suffix = 3, NMChars = 4, Escape = 5;

            /* NUMBER = num = [-+]? ([0-9]+ | [0-9]* '.' [0-9]+)
	         * PERCENTAGE = num '%'
	         * DIMENSION = num ident
	         *
	         * The sign, or sign and first digit or dot,
	         * or first digit, or '.' has been consumed.
	         */
            switch (Substate)
            {
                case Initial:
                    ConsumeDigits();

                    // Fall through
                    goto case Dot;
                case Dot:
                    Substate = Dot;

                    // Check for EOF
                    if (Source.Length <= Source.Index + BytesReadForToken)
                    {
                        if (Token.DataLen == 1 && (First == '.' || First == '+'))
                            token = EmitToken(CssTokenType.CSS_TOKEN_CHAR);
                        else
                            token = EmitToken(CssTokenType.CSS_TOKEN_NUMBER);

                        return CssStatus.CSS_OK;
                    }

                    c = Source[Source.Index + BytesReadForToken];

                    // Bail if we've not got a '.' or we've seen one already
                    if (c != '.' || First == '.')
                        goto suffix;

                    // Save the token length up to the end of the digits
                    OrigBytes = BytesReadForToken;
                    
                    // Append the '.' to the token
                    //APPEND(lexer, cptr, clen);
                    AppendToTokenData(c, 1);
                    BytesReadForToken++;
                    CurrentCol++;

                    // Fall through
                    goto case MoreDigits;

                case MoreDigits:
                    Substate = MoreDigits;
                    ConsumeDigits();

                    if (BytesReadForToken - OrigBytes == 1)
                    {
                        // No digits after dot => dot isn't part of number
                        BytesReadForToken -= 1;
                        Token.DataLen -= 1;
                    }

                    // Fall through
                    goto case Suffix;

                case Suffix:
                    suffix:
                    Substate = Suffix;

                    // Check for EOF
                    if (Source.Length <= Source.Index + BytesReadForToken)
                    {
                        if (Token.DataLen == 1 && (First == '.' || First == '+'))
                            token = EmitToken(CssTokenType.CSS_TOKEN_CHAR);
                        else
                            token = EmitToken(CssTokenType.CSS_TOKEN_NUMBER);

                        return CssStatus.CSS_OK;
                    }

                    c = Source[Source.Index + BytesReadForToken];

                    // A solitary '.' or '+' is a CHAR, not numeric
                    if (Token.DataLen == 1 && (First == '.' || First == '+'))
                    {
                        token = EmitToken(CssTokenType.CSS_TOKEN_CHAR);
                        return CssStatus.CSS_OK;
                    }

                    if (c == '%')
                    {
                        //APPEND(lexer, cptr, clen);
                        AppendToTokenData(c, 1);
                        BytesReadForToken++;
                        CurrentCol++;

                        token = EmitToken(CssTokenType.CSS_TOKEN_PERCENTAGE);
                        return CssStatus.CSS_OK;
                    }
                    else if (!StartNMStart(c))
                    {
                        token = EmitToken(CssTokenType.CSS_TOKEN_NUMBER);
                        return CssStatus.CSS_OK;
                    }

                    if (c != '\\')
                    {
                        //APPEND(lexer, cptr, clen);
                        AppendToTokenData(c, 1);
                        BytesReadForToken++;
                        CurrentCol++;
                    }
                    else
                    {
                        BytesReadForToken++;
                        goto escape;
                    }

                    // Fall through
                    goto case NMChars;

                case NMChars:
                    nmchars:
                    Substate = NMChars;

                    ConsumeNMChars();
                    break;

                case Escape:
                    escape:
                    Substate = Escape;

                    Log.Unimplemented();
                    /*
                    ConsumeEscape(lexer, false);
                    if (error != CSS_OK)
                    {
                        if (error == CSS_EOF || error == CSS_INVALID)
                        {
                            // Rewind the '\\'
                            lexer->bytesReadForToken -= 1;

                            // This can only be a number
                            return emitToken(lexer,
                                    CSS_TOKEN_NUMBER, token);
                        }

                        return error;
                    }
                    */
                    goto case NMChars;
            }

            token = EmitToken(CssTokenType.CSS_TOKEN_DIMENSION);
            return CssStatus.CSS_OK;
        }

        // lex.c:1140
        CssStatus S(out CssToken token)
        {
            /* S = wc*
             *
             * The first whitespace character has been consumed.
             */
            ConsumeWChars();

            token = EmitToken(CssTokenType.CSS_TOKEN_S);
            return CssStatus.CSS_OK;
        }
        
        // lex.c:772
        CssStatus Comment(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }

        // lex.c:941
        CssStatus Match(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }

        // lex.c:1370
        CssStatus URI(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }
        // lex.c:890
        CssStatus IdentOrFunction(out CssToken token)
        {
            // enum { Initial = 0, Bracket = 1 };

            /* IDENT = ident = [-]? nmstart nmchar*
	         * FUNCTION = ident '(' = [-]? nmstart nmchar* '('
	         *
	         * The optional dash and nmstart are already consumed
	         */

            switch (Substate)
            {
                case 0: //Initial:
                    // Consume all subsequent nmchars (if any exist)
                    ConsumeNMChars();

                    // Fall through
                    goto case 1;
                case 1: //Bracket:
                    Substate = 1;

                    // Check for EOF
                    if (Source.Length <= Source.Index + BytesReadForToken)
                    {
                        // IDENT, rather than CHAR
                        token = EmitToken(CssTokenType.CSS_TOKEN_IDENT);
                        return CssStatus.CSS_OK;
                    }

                    var c = Source[Source.Index + BytesReadForToken];

                    if (c == '(')
                    {
                        //APPEND(lexer, cptr, clen);
                        AppendToTokenData(c, 1);
                        BytesReadForToken++;
                        CurrentCol++;

                        token = EmitToken(CssTokenType.CSS_TOKEN_FUNCTION);
                        return CssStatus.CSS_OK;
                    }
                    else
                    {
                        token = EmitToken(CssTokenType.CSS_TOKEN_IDENT);
                        return CssStatus.CSS_OK;
                    }
                default:
                    token = new CssToken(1);
                    return CssStatus.CSS_OK;
            }
        }

        // lex.c:843
        CssStatus EscapedIdentOrFunction(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }

        // lex.c:1545
        CssStatus UnicodeRange(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }

        // lex.c:670
        CssStatus CDO(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }

        // lex.c:547
        CssStatus CDCOrIdentOrFunctionOrNPD(out CssToken token)
        {
            Log.Unimplemented();
            token = new CssToken(1);
            return CssStatus.CSS_OK;
        }


        // lex.c:1814
        void ConsumeNMChars()
        {
            /* nmchar = [a-zA-Z] | '-' | '_' | nonascii | escape */
            char c;
            do
            {
                // Check for EOF
                if (Source.Length <= Source.Index + BytesReadForToken)
                    return;

                c = Source[Source.Index + BytesReadForToken];

                if (StartNMChar(c) && c != '\\')
                {
                    //APPEND(lexer, cptr, clen);
                    AppendToTokenData(c, 1);
                    BytesReadForToken++;
                    CurrentCol++;
                }

                if (c == '\\')
                {
                    BytesReadForToken++;

                    Log.Unimplemented();
                    //error = consumeEscape(lexer, false);
                    //if (error != CSS_OK)
                    //{
                    // Rewind '\\', so we do the right thing next time
                    //BytesReadForToken--;

                    /* Convert either EOF or INVALID into OK.
                     * This will cause the caller to believe that
                     * all NMChars in the sequence have been
                     * processed (and thus proceed to the next
                     * state). Eventually, the '\\' will be output
                     * as a CHAR. */
                    //if (error == CSS_EOF || error == CSS_INVALID)
                    //  return CSS_OK;

                    //return error;
                    //}
                }
            } while (StartNMChar(c));
        }
        #endregion

        #region Input consumers
        // lex.c:1674
        void ConsumeDigits()
        {
            char c;
            int clen;

            /* digit = [0-9] */

            // Consume all digits
            do
            {
                // Check for EOF
                if (Source.Length <= Source.Index + BytesReadForToken)
                    return;
                c = Source[Source.Index + BytesReadForToken];

                if (IsDigit(c))
                {
                    //APPEND(lexer, cptr, clen);
                    AppendToTokenData(c, 1);
                    BytesReadForToken++;
                    CurrentCol++;
                }
            } while (IsDigit(c));
        }

        // lex.c:2133
        void ConsumeWChars()
        {
            //const uint8_t* cptr;
            char c;
            int clen;

            do
            {
                /*
                perror = parserutils_inputstream_peek(lexer->input,
                        lexer->bytesReadForToken, &cptr, &clen);
                if (perror != PARSERUTILS_OK && perror != PARSERUTILS_EOF)
                    return css_error_from_parserutils_error(perror);

                if (perror == PARSERUTILS_EOF)
                    return CSS_OK;

                c = *cptr;*/

                // Check for EOF
                if (Source.Length <= Source.Index + BytesReadForToken)
                    return;

                c = Source[Source.Index + BytesReadForToken];


                if (IsSpace(c))
                {
                    //APPEND(lexer, cptr, clen);
                    AppendToTokenData(c, 1);
                    BytesReadForToken++;
                    CurrentCol++;
                }

                if (c == '\n' || c == '\f')
                {
                    CurrentCol = 1;
                    CurrentLine++;
                }

                if (LastWasCR && c != '\n')
                {
                    CurrentCol = 1;
                    CurrentLine++;
                }
                LastWasCR = (c == '\r');
            } while (IsSpace(c));

            if (LastWasCR)
            {
                CurrentCol = 1;
                CurrentLine++;
            }
        }
        #endregion

        /******************************************************************************
         * More utility routines                                                      *
         ******************************************************************************/
        static bool StartNMChar(char c)
        {
            return c == '_' || ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') ||
                ('0' <= c && c <= '9') || c == '-' || c >= 0x80 || c == '\\';
        }
        static bool StartNMStart(char c)
        {
            return c == '_' || ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') ||
                c >= 0x80 || c == '\\';
        }

        static bool IsSpace(char c)
        {
            return c == ' ' || c == '\r' || c == '\n' || c == '\f' || c == '\t';
        }
        static bool IsDigit(char c)
        {
            return ('0' <= c) && (c <= '9');
        }

    }
}
