using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    public enum CssStatus : int
    {
        CSS_OK = 0,
        CSS_INVALID = 3,
        CSS_NEEDDATA = 5,
        CSS_EOF = 7
    }

    // Parser event types
    // parse.h:24
    public enum CssParserEvent
    {
        CSS_PARSER_START_STYLESHEET,
        CSS_PARSER_END_STYLESHEET,
        CSS_PARSER_START_RULESET,
        CSS_PARSER_END_RULESET,
        CSS_PARSER_START_ATRULE,
        CSS_PARSER_END_ATRULE,
        CSS_PARSER_START_BLOCK,
        CSS_PARSER_END_BLOCK,
        CSS_PARSER_BLOCK_CONTENT,
        CSS_PARSER_END_BLOCK_CONTENT,
        CSS_PARSER_DECLARATION
    }

    public enum ParserStateValue : int
    {
        sStart = 0,
        sStylesheet = 1,
        sStatement = 2,
        sRuleset = 3,
        sRulesetEnd = 4,
        sAtRule = 5,
        sAtRuleEnd = 6,
        sBlock = 7,
        sBlockContent = 8,
        sSelector = 9,
        sDeclaration = 10,
        sDeclList = 11,
        sDeclListEnd = 12,
        sProperty = 13,
        sValue0 = 14,
        sValue1 = 15,
        sValue = 16,
        sAny0 = 17,
        sAny1 = 18,
        sAny = 19,
        sMalformedDecl = 20,
        sMalformedSelector = 21,
        sMalformedAtRule = 22,
        sInlineStyle = 23,
        sISBody0 = 24,
        sISBody = 25,
        sMediaQuery = 26,
    };

    public struct ParserState
    {
        public ParserStateValue State;
        public int Substate;
        public ParserState(ParserStateValue state, int substate)
        {
            State = state;
            Substate = substate;
        }
        public override string ToString()
        {
            return $"[{(int)State} {Substate}]";
            //return $"[{State} {Substate}]";
        }
    }

    internal delegate CssStatus Parser();

    internal class CssParser
    {
        CssLexer Lex;
        Stack<ParserState> States; // Stack of states
        Parser[] ParseFunc;

        //MemoryStream Stream;
        TextSource Source;

        public List<CssToken> Tokens; // Vector of pending tokens

        CssToken? Pushback;	// Push back buffer

        bool ParseError; // A parse error occured
        Stack<string> OpenItems; // Stack of open brackets (FIXME: maybe should be char instead of a string)

        bool LastWasWs; // Last token was whitespace


        // Debug flags
        bool DebugStack = true;

        // Event callback
        Action<CssParserEvent, bool> Event;

        // Parser routines
        // parse.c:721
        public CssStatus ParseStart()
        {
            var state = States.Peek();
            CssStatus error = CssStatus.CSS_OK;

            // Initial = 0, AfterWS = 1, AfterStylesheet = 2
            // start -> ws stylesheet EOF
            switch (state.Substate)
            {
                case 0: // Initial
                        //printf("Begin stylesheet\n");
                    if (Event != null)
                        Event(CssParserEvent.CSS_PARSER_START_STYLESHEET, false);

                    error = EatWS();
                    if (error != CssStatus.CSS_OK)
                        return error;
                    state.Substate = 1;
                    goto case 1;
                // Fall through
                case 1:
                    ParserState to = new ParserState(ParserStateValue.sStylesheet, 0);
                    ParserState subsequent = new ParserState(ParserStateValue.sStart, 2);

                    Transition(to, subsequent);
                    return CssStatus.CSS_OK;
                case 2:
                    /*error = expect(parser, CSS_TOKEN_EOF);
                    if (error != 0)
                        return error;*/

                    // Flag completion, just in case
                    break;
                default:
                    // Should not happen
                    break;
            }

            /*
            #if !defined(NDEBUG) && defined(DEBUG_EVENTS)
                parserutils_vector_dump(parser->tokens, __func__, tprinter);
            printf("End stylesheet\n");
            #endif
            if (parser->event != NULL) {
                parser->event(CSS_PARSER_END_STYLESHEET, NULL,
                    parser->event_pw);
            }*/
            if (Event != null)
                Event(CssParserEvent.CSS_PARSER_END_STYLESHEET, false);

            DiscardTokens();
            Done();

            return CssStatus.CSS_OK; // remove
        }
        // parse.c:773
        public CssStatus ParseStylesheet()
        {
            //enum { Initial = 0, WS = 1 };
            //parser_state *state = parserutils_stack_get_current(parser->states);
            var state = States.Peek();
            CssToken token;
            CssStatus status;

            /* stylesheet -> CDO ws stylesheet
	         *            -> CDC ws stylesheet
	         *            -> statement ws stylesheet
	         *            ->
	         */

            while (true)
            {
                switch (state.Substate)
                {
                    case 0: // Initial
                        token = GetToken(out status);

                        switch (token.Type)
                        {
                            case CssTokenType.CSS_TOKEN_EOF:
                                PushBack(token);

                                //discard_tokens(parser);
                                Console.WriteLine("parser:159 - DiscardTokens missing");

                                Done();
                                return CssStatus.CSS_OK;
                            case CssTokenType.CSS_TOKEN_CDO:
                            case CssTokenType.CSS_TOKEN_CDC:
                                break;
                            default:
                                {
                                    ParserState to = new ParserState(ParserStateValue.sStatement, 0);
                                    ParserState subsequent = new ParserState(ParserStateValue.sStylesheet, 1);

                                    PushBack(token);

                                    Transition(to, subsequent);
                                    return CssStatus.CSS_OK;
                                }
                        }

                        state = States.Pop();
                        state.Substate = 1; // WS
                        States.Push(state);

                        /* Fall through */
                        goto case 1;
                    case 1: // WS
                        var error = EatWS();
                        if (error != CssStatus.CSS_OK)
                            return error;

                        state = States.Pop();
                        state.Substate = 0; // Initial
                        States.Push(state);
                        break;

                    default:
                        return CssStatus.CSS_OK;
                }
            }

            return CssStatus.CSS_OK;
        }

        // parse.c:830
        public CssStatus ParseStatement()
        {
            //enum { Initial = 0 };
            var to = new ParserState(ParserStateValue.sRuleset, 0);
            CssStatus status;

            /* statement -> ruleset
             *              at-rule
             */

            var token = GetToken(out status);

            if (token.Type == CssTokenType.CSS_TOKEN_ATKEYWORD)
                to.State = ParserStateValue.sAtRule;

            PushBack(token);

            TransitionNoRet(to);

            return CssStatus.CSS_OK;
        }

        // parse.c:855
        public CssStatus ParseRuleset()
        {
            //enum { Initial = 0, Brace = 1, WS = 2 };
            var state = States.Peek();
            CssStatus status;
            ParserState to = new ParserState(ParserStateValue.sRulesetEnd, 0);

            /* ruleset -> selector '{' ws ruleset-end
	         *         -> '{' ws ruleset-end
	         */

            switch (state.Substate)
            {
                case 0: /*Initial*/
                    // discard_tokens(parser);

                    var token = GetToken(out status);

                    /* The grammar's ambiguous here -- selectors may start with a
		             * brace. We're going to assume that that won't happen,
		             * however. */
                    if (token.Type == CssTokenType.CSS_TOKEN_CHAR &&
                        token.iData.Length == 1 && token.iData[0] == '{')
                    {
                        Log.Unimplemented();

                        /*
			            if (parser->event != NULL) {
				            if (parser->event(CSS_PARSER_START_RULESET,
						            NULL, parser->event_pw) ==
						            CSS_INVALID) {
					            parser_state to =
						            { sMalformedSelector, Initial };

					            return transitionNoRet(parser, to);
				            }
			            }*/
                        if (Event != null)
                            Event(CssParserEvent.CSS_PARSER_START_RULESET, false);


                        // Replace top of stack state's substate
                        {
                            var state_ = States.Pop();
                            state_.Substate = 2; // WS
                            States.Push(state_);
                        }

                        goto ws;
                    }
                    else
                    {
                        to = new ParserState(ParserStateValue.sSelector, 0);
                        ParserState subsequent = new ParserState(ParserStateValue.sRuleset, 1);

                        PushBack(token);

                        Transition(to, subsequent);
                        return CssStatus.CSS_OK;
                    }
                //break;

                case 1: // Brace
                    if (ParseError == true)
                    {
                        to = new ParserState(ParserStateValue.sMalformedSelector, 0);

                        TransitionNoRet(to);
                        return CssStatus.CSS_OK;
                    }

                    token = GetToken(out status);

                    if (token.Type != CssTokenType.CSS_TOKEN_CHAR ||
                            token.iData.Length != 1 ||
                            token.iData[0] != '{')
                    {
                        Log.Unimplemented();

                        // FOLLOW(selector) contains only '{', but we may
                        // also have seen EOF, which is a parse error.
                        PushBack(token);

                        ParseError = true;
                        Done();
                        return CssStatus.CSS_OK;
                    }

                    /* We don't want to emit the brace, so push it back */
                    PushBack(token);

                    Console.WriteLine("Begin ruleset");
                    //parserutils_vector_dump(parser->tokens, __func__, tprinter);
                    /*
		            if (parser->parseError == false && parser->event != NULL) {
			            if (parser->event(CSS_PARSER_START_RULESET,
					            parser->tokens, parser->event_pw) ==
					            CSS_INVALID)
				            parser->parseError = true;
		            }*/
                    if (ParseError == false && Event != null)
                        Event(CssParserEvent.CSS_PARSER_START_RULESET, true);


                    /* Re-read the brace */
                    token = GetToken(out status);

                    // Replace top of stack state's substate
                    {
                        var state_ = States.Pop();
                        state_.Substate = 2; // WS
                        States.Push(state_);
                    }
                    goto ws;

                /* Fall through */
                case 2:
                    ws:
                    EatWS();
                    break;
            }

            TransitionNoRet(to);
            return CssStatus.CSS_OK;
        }

        // parse.c:967
        public CssStatus ParseRulesetEnd()
        {
            // enum { Initial = 0, DeclList = 1, Brace = 2, WS = 3 };
            var state = States.Peek();
            CssToken token;
            CssStatus status;
            ParserState to, subsequent;

            /* ruleset-end -> declaration decl-list '}' ws
	        *             -> decl-list '}' ws
	        */

            switch (state.Substate)
            {
                case 0: // Initial
                    token = GetToken(out status);
                    PushBack(token);

                    if (token.Type == CssTokenType.CSS_TOKEN_EOF)
                    {
                        Done();
                        return CssStatus.CSS_OK;
                    }

                    /* If this can't possibly be the start of a decl-list, then
		             * attempt to parse a declaration. This will catch any invalid
		             * input at this point and read to the start of the next
		             * declaration. FIRST(decl-list) = (';', '}') */
                    if (token.Type != CssTokenType.CSS_TOKEN_CHAR ||
                            token.iData.Length != 1 ||
                            (token.iData[0] != '}' &&
                            token.iData[0] != ';'))
                    {
                        to = new ParserState(ParserStateValue.sDeclaration, 0);
                        subsequent = new ParserState(ParserStateValue.sRulesetEnd, 1);

                        Transition(to, subsequent);
                        return CssStatus.CSS_OK;
                    }

                    state.Substate = 1; //DeclList
                    // Fall through
                    goto case 1;

                case 1: //DeclList
                    to = new ParserState(ParserStateValue.sDeclList, 0);
                    subsequent = new ParserState(ParserStateValue.sRulesetEnd, 2);

                    Transition(to, subsequent);
                    return CssStatus.CSS_OK;

                case 2: //Brace
                    token = GetToken(out status);
                    if (token.Type == CssTokenType.CSS_TOKEN_EOF)
                    {
                        PushBack(token);
                        Done();
                        return CssStatus.CSS_OK;
                    }

                    if (token.Type != CssTokenType.CSS_TOKEN_CHAR ||
                        token.iData.Length != 1 ||
                        token.iData[0] != '}')
                    {
                        /* This should never happen, as FOLLOW(decl-list)
			             * contains only '}' */
                        //assert(0 && "Expected }");
                        Console.WriteLine("Parser:412 Something happened which should never happen");
                    }

                    state.Substate = 3;
                    // Fall through
                    goto case 3;

                case 3: // WS
                    EatWS();
                    break;
            }

            //#if !defined(NDEBUG) && defined(DEBUG_EVENTS)
            Console.WriteLine("End ruleset");
            //#endif
            //if (parser->event != NULL) {
            //parser->event(CSS_PARSER_END_RULESET, NULL, parser->event_pw);
            //}
            if (Event != null)
                Event(CssParserEvent.CSS_PARSER_END_RULESET, false);


            Done();
            return CssStatus.CSS_OK;
        }

        // parse.c:1055
        public CssStatus ParseAtRule()
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        // parse.c:1126
        public CssStatus ParseAtRuleEnd()
        {
            Log.Unimplemented();
            //if (Event != null)
            //Event(CssParserEvent.CSS_PARSER_START_ATRULE);

            //if (Event != null)
            //Event(CssParserEvent.CSS_PARSER_END_ATRULE);

            return CssStatus.CSS_OK;
        }

        // parse.c:1207
        public CssStatus ParseBlock()
        {
            Log.Unimplemented();

            //if (Event != null)
            //Event(CssParserEvent.CSS_PARSER_START_BLOCK);

            //if (Event != null)
            //Event(CssParserEvent.CSS_PARSER_END_BLOCK);


            return CssStatus.CSS_OK;
        }

        // parse.c:1298
        public CssStatus ParseBlockContent()
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        // parse.c:1445
        public CssStatus ParseSelector()
        {
            //enum { Initial = 0, AfterAny1 = 1 };
            var state = States.Peek();

            /* selector -> any1 */
            switch (state.Substate)
            {
                case 0: //Initial:
                    {
                        ParserState to = new ParserState(ParserStateValue.sAny1, 0);
                        ParserState subsequent = new ParserState(ParserStateValue.sSelector, 1);

                        DiscardTokens();

                        Transition(to, subsequent);
                        return CssStatus.CSS_OK;
                    }
                case 1://AfterAny1:
                    break;
                default:
                    break;
            }

            Done();
            return CssStatus.CSS_OK;
        }

        // parse.c:1469
        public CssStatus ParseDeclaration()
        {
            // enum { Initial = 0, Colon = 1, WS = 2, AfterValue1 = 3 };
            var state = States.Peek();
            CssToken token;
            CssStatus status;
            ParserState to, subsequent;

            // declaration -> property ':' ws value1

            switch (state.Substate)
            {
                case 0: //Initial
                    to = new ParserState(ParserStateValue.sProperty, 0);
                    subsequent = new ParserState(ParserStateValue.sDeclaration, 1);

                    DiscardTokens();

                    Transition(to, subsequent);
                    return CssStatus.CSS_OK;

                case 1: //Colon
                    if (ParseError)
                    {
                        to = new ParserState(ParserStateValue.sMalformedDecl, 0);
                        ParseError = false;

                        TransitionNoRet(to);
                        return CssStatus.CSS_OK;
                    }

                    token = GetToken(out status);

                    if (token.Type == CssTokenType.CSS_TOKEN_EOF)
                    {
                        PushBack(token);

                        Done();
                        return CssStatus.CSS_OK;
                    }

                    if (token.Type != CssTokenType.CSS_TOKEN_CHAR ||
                            token.iData.Length != 1 ||
                            token.iData[0] != ':')
                    {
                        // parse error -- expected :
                        to = new ParserState(ParserStateValue.sMalformedDecl, 0);

                        PushBack(token);

                        TransitionNoRet(to);
                        return CssStatus.CSS_OK;
                    }

                    state.Substate = 2; //WS
                    // Fall through
                    goto case 2;

                case 2: // WS
                    to = new ParserState(ParserStateValue.sValue1, 0);
                    subsequent = new ParserState(ParserStateValue.sDeclaration, 3);

                    EatWS();

                    Transition(to, subsequent);
                    return CssStatus.CSS_OK;

                case 3: // AfterValue1
                    if (ParseError)
                    {
                        to = new ParserState(ParserStateValue.sMalformedDecl, 0);

                        ParseError = false;

                        TransitionNoRet(to);
                        return CssStatus.CSS_OK;
                    }

                    //#if !defined(NDEBUG) && defined(DEBUG_EVENTS)
                    //parserutils_vector_dump(parser->tokens, __func__, tprinter);
                    //#endif

                    //if (parser->event != NULL) {
                    //    parser->event(CSS_PARSER_DECLARATION, parser->tokens,
                    //            parser->event_pw);
                    //}
                    if (Event != null)
                        Event(CssParserEvent.CSS_PARSER_DECLARATION, true);

                    break;
            }

            Done();

            return CssStatus.CSS_OK;
        }

        // parse.c:1557
        public CssStatus ParseDeclList()
        {
            // enum { Initial = 0, WS = 1 };
            var state = States.Peek();
            ParserState to = new ParserState(ParserStateValue.sDeclListEnd, 0);
            ParserState subsequent;
            CssToken token;
            CssStatus status;

            /* decl-list -> ';' ws decl-list-end
             *           ->
             */

            switch (state.Substate)
            {
                case 0: // Initial
                    token = GetToken(out status);

                    if (token.Type == CssTokenType.CSS_TOKEN_EOF)
                    {
                        PushBack(token);

                        Done();
                        return CssStatus.CSS_OK;
                    }

                    if (token.Type != CssTokenType.CSS_TOKEN_CHAR ||
                            token.iData.Length != 1 ||
                            (token.iData[0] != '}' &&
                            token.iData[0] != ';'))
                    {
                        // Should never happen
                        //assert(0 && "Expected ; or  }");
                        Console.WriteLine("parser:620 should never happen");
                    }

                    if (token.iData[0] == '}')
                    {
                        PushBack(token);

                        Done();
                        return CssStatus.CSS_OK;
                    }
                    else
                    {
                        /* ; */
                        state.Substate = 1; // WS
                    }
                    // Fall through
                    goto case 1;

                case 1: // WS
                    EatWS();
                    break;
            }

            TransitionNoRet(to);

            return CssStatus.CSS_OK;
        }
        // parse.c:1614
        public CssStatus ParseDeclListEnd()
        {
            // enum { Initial = 0, AfterDeclaration = 1 };

            var state = States.Peek();
            ParserState to = new ParserState(ParserStateValue.sDeclList, 0);
            ParserState subsequent;
            CssToken token;
            CssStatus status;

            /* decl-list-end -> declaration decl-list
             *               -> decl-list
             */

            switch (state.Substate)
            {
                case 0: // Initial
                    token = GetToken(out status);

                    if (token.Type != CssTokenType.CSS_TOKEN_CHAR ||
                            token.iData.Length != 1 ||
                            (token.iData[0] != ';' &&
                            token.iData[0] != '}'))
                    {
                        to = new ParserState(ParserStateValue.sDeclaration, 0);
                        subsequent = new ParserState(ParserStateValue.sDeclListEnd, 1);

                        PushBack(token);
                        Transition(to, subsequent);
                        return CssStatus.CSS_OK;
                    }
                    else
                    {
                        PushBack(token);
                    }

                    state.Substate = 1; // AfterDeclaration
                    // Fall through
                    goto case 1;

                case 1: //AfterDeclaration
                    break;
            }

            TransitionNoRet(to);
            return CssStatus.CSS_OK;
        }
        // parse.c:1660
        public CssStatus ParseProperty()
        {
            // enum { Initial = 0, WS = 1 };
            var state = States.Peek();
            CssToken token;
            CssStatus status;

            // property -> IDENT ws
            switch (state.Substate)
            {
                case 0: // Initial
                    token = GetToken(out status);

                    if (token.Type == CssTokenType.CSS_TOKEN_EOF)
                    {
                        PushBack(token);

                        Done();
                        return CssStatus.CSS_OK;
                    }

                    if (token.Type != CssTokenType.CSS_TOKEN_IDENT)
                    {
                        // parse error
                        ParseError = true;

                        Done();
                        return CssStatus.CSS_OK;
                    }

                    state.Substate = 1; //WS
                    // Fall through
                    goto case 1;

                case 1: //WS
                    EatWS();
                    break;
            }

            Done();

            return CssStatus.CSS_OK;
        }
        // parse.c:1749
        public CssStatus ParseValue0()
        {
            // enum { Initial = 0, AfterValue = 1 };

            var state = States.Peek();
            ParserState to, subsequent;
            CssToken token;
            CssStatus status;

            /* value0 -> value value0
             *        ->
             */

            while (true)
            {
                switch (state.Substate)
                {
                    case 0: // Initial
                        {
                            to = new ParserState(ParserStateValue.sValue, 0);
                            subsequent = new ParserState(ParserStateValue.sValue0, 1);

                            token = GetToken(out status);

                            PushBack(token);

                            if (token.Type == CssTokenType.CSS_TOKEN_EOF)
                            {
                                Done();
                                return CssStatus.CSS_OK;
                            }

                            // Grammar ambiguity -- assume ';' or '}' mark end
                            if (token.Type == CssTokenType.CSS_TOKEN_CHAR &&
                                    token.iData.Length == 1 &&
                                    (token.iData[0] == ';' ||
                                     token.iData[0] == '}'))
                            {
                                Done();
                                return CssStatus.CSS_OK;
                            }

                            Transition(to, subsequent);
                            return CssStatus.CSS_OK;
                        }
                    case 1: // AfterValue
                        if (ParseError)
                        {
                            Done();
                            return CssStatus.CSS_OK;
                        }

                        state.Substate = 0; // Initial
                        break;
                }
            }

            Done();

            return CssStatus.CSS_OK;
        }
        // parse.c:1703
        public CssStatus ParseValue1()
        {
            // enum { Initial = 0, AfterValue = 1 };
            var state = States.Peek();
            var to = new ParserState(ParserStateValue.sValue0, 0);
            CssToken token;
            CssStatus status;

            /* value1 -> value value0 */
            switch (state.Substate)
            {
                case 0: //Initial
                    {
                        to = new ParserState(ParserStateValue.sValue, 0);
                        var subsequent = new ParserState(ParserStateValue.sValue1, 1);

                        token = GetToken(out status);
                        PushBack(token);

                        // Grammar ambiguity -- assume ';' or '}' mark end
                        if (token.Type == CssTokenType.CSS_TOKEN_CHAR &&
                                token.iData.Length == 1 &&
                                (token.iData[0] == ';' ||
                                token.iData[0] == '}'))
                        {
                            // Parse error
                            ParseError = true;

                            Done();
                            return CssStatus.CSS_OK;
                        }

                        Transition(to, subsequent);
                        return CssStatus.CSS_OK;
                    }

                case 1: //AfterValue
                    if (ParseError)
                    {
                        Done();
                        return CssStatus.CSS_OK;
                    }
                    break;
            }

            TransitionNoRet(to);

            return CssStatus.CSS_OK;
        }
        // parse.c:1804
        public CssStatus ParseValue()
        {
            // enum { Initial = 0, WS = 1 };
            var state = States.Peek();
            ParserState to, subsequent;
            CssToken token;
            CssStatus status;

            /* value  -> any
             *        -> block
             *        -> ATKEYWORD ws
             */
            switch (state.Substate)
            {
                case 0: // Initial
                    token = GetToken(out status);

                    if (token.Type == CssTokenType.CSS_TOKEN_ATKEYWORD)
                    {
                        state.Substate = 1; //WS
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_CHAR &&
                          token.iData.Length == 1 &&
                          token.iData[0] == '{')
                    {
                        // Grammar ambiguity. Assume block.
                        to = new ParserState(ParserStateValue.sBlock, 0);

                        PushBack(token);

                        TransitionNoRet(to);
                        return CssStatus.CSS_OK;
                    }
                    else
                    {
                        to = new ParserState(ParserStateValue.sAny, 0);

                        PushBack(token);

                        TransitionNoRet(to);
                        return CssStatus.CSS_OK;
                    }

                    // Fall through
                    goto case 1;

                case 1: //WS
                    EatWS();
                    break;
            }

            Done();

            return CssStatus.CSS_OK;
        }
        // parse.c:1857
        public CssStatus ParseAny0()
        {
            //enum { Initial = 0, AfterAny = 1 };
            var state = States.Peek();
            CssStatus status;

            /* any0 -> any any0
             *      ->
             */

            while (true)
            {
                switch (state.Substate)
                {
                    case 0: // Initial
                        var to = new ParserState(ParserStateValue.sAny, 0);
                        var subsequent = new ParserState(ParserStateValue.sAny0, 1);
                        var token = GetToken(out status);
                        if (status != CssStatus.CSS_OK)
                        {
                            Console.WriteLine($"GetToken() error: {status.ToString()}");
                            return status;
                        }
                        PushBack(token);

                        if (token.Type == CssTokenType.CSS_TOKEN_EOF)
                        {
                            Done();
                            return CssStatus.CSS_OK;
                        }

                        /* Grammar ambiguity:
                                     * assume '{', ';', ')', ']' mark end */
                        if (token.Type == CssTokenType.CSS_TOKEN_CHAR &&
                                    token.iData.Length == 1 &&
                                    (token.iData[0] == '{' ||
                                    token.iData[0] == ';' ||
                                    token.iData[0] == ')' ||
                                    token.iData[0] == ']'))
                        {
                            Done();
                            return CssStatus.CSS_OK;
                        }

                        Transition(to, subsequent);
                        return CssStatus.CSS_OK;

                    case 1: // AfterAny
                        if (ParseError)
                        {
                            Done();
                            return CssStatus.CSS_OK;
                        }

                        state.Substate = 0;
                        break;
                }
            }

            Done();
            return CssStatus.CSS_OK;
        }
        // parse.c:1917
        public CssStatus ParseAny1()
        {
            //enum { Initial = 0, AfterAny = 1, AfterAny0 = 2 };
            var state = States.Peek();
            ParserState to, subsequent;
            CssStatus status;

            /* selector -> any1 */
            switch (state.Substate)
            {
                case 0: //Initial:
                    to = new ParserState(ParserStateValue.sAny, 0);
                    subsequent = new ParserState(ParserStateValue.sAny1, 1);

                    Transition(to, subsequent);
                    return CssStatus.CSS_OK;
                case 1://AfterAny:
                    to = new ParserState(ParserStateValue.sAny0, 0);
                    subsequent = new ParserState(ParserStateValue.sAny1, 2);

                    if (ParseError)
                    {
                        Done();
                        return CssStatus.CSS_OK;
                    }

                    Transition(to, subsequent);
                    return CssStatus.CSS_OK;

                case 2://AfterAny0:
                    if (ParseError)
                    {
                        Done();
                        return CssStatus.CSS_OK;
                    }

                    var token = GetToken(out status);
                    PushBack(token);

                    if (token.Type == CssTokenType.CSS_TOKEN_EOF)
                    {
                        Done();
                        return CssStatus.CSS_OK;
                    }

                    /* Grammar ambiguity: any0 can be followed by
                     * '{', ';', ')', ']'. any1 can only be followed by '{'. */
                    if (token.Type == CssTokenType.CSS_TOKEN_CHAR &&
                        token.iData.Length == 1)
                    {
                        if (token.iData[0] == ';' ||
                            token.iData[0] == ')' ||
                            token.iData[0] == ']')
                        {
                            to = new ParserState(ParserStateValue.sAny, 0);
                            subsequent = new ParserState(ParserStateValue.sAny1, 1);

                            Transition(to, subsequent);
                            return CssStatus.CSS_OK;
                        }
                        else if (token.iData[0] != '{')
                        {
                            // parse error
                            ParseError = true;
                        }
                    }
                    else
                    {
                        // parse error
                        ParseError = true;
                    }
                    break;

                default:
                    break;
            }

            Done();
            return CssStatus.CSS_OK;
        }

        // parse.c:1985
        public CssStatus ParseAny()
        {
            // enum { Initial = 0, WS = 1, AfterAny0 = 2, WS2 = 3 };

            var state = States.Peek();
            ParserState to, subsequent;
            CssToken token;
            CssStatus status;

            /* any -> IDENT ws
             *     -> NUMBER ws
             *     -> PERCENTAGE ws
             *     -> DIMENSION ws
             *     -> STRING ws
             *     -> CHAR ws
             *     -> URI ws
             *     -> HASH ws
             *     -> UNICODE-RANGE ws
             *     -> INCLUDES ws
             *     -> DASHMATCH ws
             *     -> PREFIXMATCH ws
             *     -> SUFFIXMATCH ws
             *     -> SUBSTRINGMATCH ws
             *     -> FUNCTION ws any0 ')' ws
             *     -> '(' ws any0 ')' ws
             *     -> '[' ws any0 ']' ws
             */

            switch (state.Substate)
            {
                case 0: //Initial:
                    token = GetToken(out status);

                    if (token.Type != CssTokenType.CSS_TOKEN_IDENT &&
                        token.Type != CssTokenType.CSS_TOKEN_NUMBER &&
                        token.Type != CssTokenType.CSS_TOKEN_PERCENTAGE &&
                        token.Type != CssTokenType.CSS_TOKEN_DIMENSION &&
                        token.Type != CssTokenType.CSS_TOKEN_STRING &&
                        token.Type != CssTokenType.CSS_TOKEN_CHAR &&
                        token.Type != CssTokenType.CSS_TOKEN_URI &&
                        token.Type != CssTokenType.CSS_TOKEN_HASH &&
                        token.Type != CssTokenType.CSS_TOKEN_UNICODE_RANGE &&
                        token.Type != CssTokenType.CSS_TOKEN_INCLUDES &&
                        token.Type != CssTokenType.CSS_TOKEN_DASHMATCH &&
                        token.Type != CssTokenType.CSS_TOKEN_PREFIXMATCH &&
                        token.Type != CssTokenType.CSS_TOKEN_SUFFIXMATCH &&
                        token.Type != CssTokenType.CSS_TOKEN_SUBSTRINGMATCH &&
                        token.Type != CssTokenType.CSS_TOKEN_FUNCTION)
                    {
                        // parse error
                        ParseError = true;
                        Done();
                        return CssStatus.CSS_OK;
                    }

                    if (token.Type == CssTokenType.CSS_TOKEN_FUNCTION)
                    {
                        //parserutils_stack_push(parser->open_items, &")"[0]);
                        state.Substate = 1; // WS
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_CHAR &&
                                token.iData.Length == 1 &&
                                (token.iData[0] == '(' || token.iData[0] == '['))
                    {
                        var c = (token.iData[0] == '(') ? ")" : "]";
                        OpenItems.Push(c);
                        state.Substate = 1; // WS;

                    }
                    else
                    {
                        state.Substate = 3; // WS2
                    }
                    // Fall through
                    goto ws2;

                case 1: // WS
                case 3: // WS2
                    ws2:
                    to = new ParserState(ParserStateValue.sAny0, 0);
                    subsequent = new ParserState(ParserStateValue.sAny, 2);

                    EatWS();
                    if (state.Substate == 3)
                        break;

                    Transition(to, subsequent);
                    return CssStatus.CSS_OK;

                case 2: // AfterAny0
                    to = new ParserState(ParserStateValue.sAny0, 0);
                    subsequent = new ParserState(ParserStateValue.sAny, 2);

                    token = GetToken(out status);

                    if (token.Type == CssTokenType.CSS_TOKEN_EOF)
                    {
                        PushBack(token);
                        ParseError = true;
                        Done();
                        return CssStatus.CSS_OK;
                    }

                    // Match correct close bracket (grammar ambiguity)
                    if (token.Type == CssTokenType.CSS_TOKEN_CHAR &&
                            token.iData.Length == 1 &&
                            token.iData[0] == OpenItems.Peek()[0])
                    {
                        OpenItems.Pop();
                        state.Substate = 3; //WS2;
                        goto ws2;
                    }

                    Transition(to, subsequent);
                    return CssStatus.CSS_OK;
            }

            Done();
            return CssStatus.CSS_OK;
        }
        // parse.c:2107
        public CssStatus ParseMalformedDecl()
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }
        // parse.c:2207
        public CssStatus ParseMalformedSelector()
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }
        // parse.c:2302
        public CssStatus ParseMalformedAtRule()
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }
        // parse.c:2410
        public CssStatus ParseInlineStyle()
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }
        // parse.c:2469
        public CssStatus ParseISBody0()
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }
        // parse.c:2515
        public CssStatus ParseISBody()
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }
        // parse.c:2596
        public CssStatus ParseMediaQuery()
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        // parse.c:506
        // Transition to a new state, ensuring return to the one specified
        // param parser      The parser instance
        // param to          Destination state
        // subsequent        The state to return to
        // return CSS_OK on success, appropriate error otherwise
        void Transition(ParserState To, ParserState Subsequent)
        {
            // Replace current state on the stack with the subsequent one
            var state = States.Pop();
            States.Push(Subsequent);

            // Push next state on the stack
            States.Push(To);

            if (DebugStack) DumpStates(Program.GetCurrentMethod());

            // Clear the error flag
            ParseError = false;
        }

        // parse.c:530
        /**
         * Transition to a new state, returning to previous state on stack
         *
         * \param parser  The parser instance
         * \param to      Destination state
         * \return CSS_OK on success, appropriate error otherwise
         */
        void TransitionNoRet(ParserState To)
        {
            // Replace current state on the stack with destination
            var state = States.Pop();
            States.Push(To);

            if (DebugStack) DumpStates(Program.GetCurrentMethod());

            // Clear the error flag
            ParseError = false;
        }

        void Done()
        {
            // Pop current state from stack
            var item = States.Pop();

            if (DebugStack) DumpStates(Program.GetCurrentMethod());
        }

        // parse.c:609
        // Retrieve the next token in the input
        // parser  The parser instance
        // token   Pointer to location to receive token
        // CSS_OK on success, appropriate error otherwise
        CssToken GetToken(out CssStatus status)
        {
            //parserutils_error perror;
            //lwc_error lerror;
            CssStatus error;
            CssToken token;

            // Use pushback, if it exists
            if (Pushback.HasValue)
            {
                token = Pushback.Value;
                Pushback = null;
            }
            else
            {
                // Otherwise, ask the lexer
                error = Lex.GetToken(out token);
                if (error != CssStatus.CSS_OK)
                {
                    status = error;
                    return new CssToken(1);
                }

                // If the last token read was whitespace, keep reading
                // tokens until we encounter one that isn't whitespace
                while (LastWasWs && token.Type == CssTokenType.CSS_TOKEN_S)
                {
                    error = Lex.GetToken(out token);

                    if (error != CssStatus.CSS_OK)
                    {
                        status = error;
                        return new CssToken(1);
                    }
                }

                /* We need only intern for the following token types:
                 *
                 * CSS_TOKEN_IDENT, CSS_TOKEN_ATKEYWORD, CSS_TOKEN_STRING,
                 * CSS_TOKEN_INVALID_STRING, CSS_TOKEN_HASH, CSS_TOKEN_URI,
                 * CSS_TOKEN_UNICODE_RANGE?, CSS_TOKEN_FUNCTION, CSS_TOKEN_CHAR,
                 * CSS_TOKEN_NUMBER, CSS_TOKEN_PERCENTAGE, CSS_TOKEN_DIMENSION
                 *
                 * These token types all appear before CSS_TOKEN_LAST_INTERN.
                 * All other token types appear after this magic value.
                 */
                if (token.Type < CssTokenType.CSS_TOKEN_LAST_INTERN && token.DataLen > 0)
                {
                    Console.WriteLine("Token data needs interning");
                    // Insert token text into the dictionary
                    token.iData = Source.GetContents(token.DataIndex, token.DataLen);
                    /*
                    lerror = lwc_intern_string((char*)t->data.data,
                                                t->data.len, &t->idata);
                    if (lerror != lwc_error_ok)
                        return css_error_from_lwc_error(lerror);*/
                }
                else
                {
                    token.iData = "";
                }

            }

            // Append token to vector
            Tokens.Add(token);
            LastWasWs = (token.Type == CssTokenType.CSS_TOKEN_S);

            status = CssStatus.CSS_OK;
            return token;
        }

        // parse.c:677
        void PushBack(CssToken token)
        {
            // The pushback buffer depth is 1 token. Assert this.
            //assert(parser->pushback == NULL);

            //Tokens.RemoveAt(^1); // remove last
            Tokens.RemoveAt(Tokens.Count - 1); // It must not be null at this point

            Pushback = token;
        }

        // parse.c:699
        // Eat whitespace tokens
        // \param parser  The parser instance
        // \return CSS_OK on success, appropriate error otherwise
        CssStatus EatWS()
        {
            CssStatus status;
            var token = GetToken(out status);
            if (status != CssStatus.CSS_OK) return status;

            if (token.Type != CssTokenType.CSS_TOKEN_S)
                PushBack(token);

            return CssStatus.CSS_OK;
        }

        // parse.c:2625
        void DiscardTokens()
        {
            Tokens.Clear();

            // TODO: free interned strings in token.idata
        }

        // libcss/src/parse/parse.c:409
        public CssParser(string charset, ParserState initialState)
        {
            //Stream = new MemoryStream(1024); // FIXME: Arbitrary input capacity used
            Lex = new CssLexer();
            States = new Stack<ParserState>();
            OpenItems = new Stack<string>();
            Tokens = new List<CssToken>();

            // Push initial state to the stack
            States.Push(initialState);

            // Fill parser functions
            ParseFunc = new Parser[27];
            ParseFunc[0] = ParseStart;
            ParseFunc[1] = ParseStylesheet;
            ParseFunc[2] = ParseStatement;
            ParseFunc[3] = ParseRuleset;
            ParseFunc[4] = ParseRulesetEnd;
            ParseFunc[5] = ParseAtRule;
            ParseFunc[6] = ParseAtRuleEnd;
            ParseFunc[7] = ParseBlock;
            ParseFunc[8] = ParseBlockContent;
            ParseFunc[9] = ParseSelector;
            ParseFunc[10] = ParseDeclaration;

            ParseFunc[11] = ParseDeclList;
            ParseFunc[12] = ParseDeclListEnd;
            ParseFunc[13] = ParseProperty;
            ParseFunc[14] = ParseValue0;
            ParseFunc[15] = ParseValue1;
            ParseFunc[16] = ParseValue;
            ParseFunc[17] = ParseAny0;
            ParseFunc[18] = ParseAny1;
            ParseFunc[19] = ParseAny;
            ParseFunc[20] = ParseMalformedDecl;
            ParseFunc[21] = ParseMalformedSelector;
            ParseFunc[22] = ParseMalformedAtRule;
            ParseFunc[23] = ParseInlineStyle;
            ParseFunc[24] = ParseISBody0;
            ParseFunc[25] = ParseISBody;
            ParseFunc[26] = ParseMediaQuery;
        }

        public void RegisterCallbacks(Action<CssParserEvent, bool> handler)
        {
            Event = handler;
        }

        // parse.c:303
        public void ParseChunk(string data)
        {
            // Append data to the stream
            /*
            UnicodeEncoding uniEncoding = new UnicodeEncoding();
            Stream.Write(uniEncoding.GetBytes(data), 0, data.Length);
            Stream.Flush();
            Stream.Seek(-data.Length, SeekOrigin.Current);*/

            Source = new TextSource(data);
            Lex.SetSource(Source);

            // Flush through any remaining data
            do
            {
                // Get one item from the states stack but don't pop it
                if (States.Count == 0) break;
                var state = States.Peek();

                // Call parse func
                var status = ParseFunc[(int)state.State]();
                if (status != CssStatus.CSS_OK) break;
            } while (true);
        }

        void DumpStates(string prefix)
        {
            Console.WriteLine($"{prefix}: {string.Join(", ", States)}");
            //Console.WriteLine("{" + string.Join(", ", States) + "}");
        }
    }
}
