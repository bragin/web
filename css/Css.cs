using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    // propstrings.c:30
    public static class CssStrings
    {
        public const string Universal = "*";
        public static readonly string[] Props =  {
            "ALIGN_CONTENT", "ALIGN_ITEMS", "ALIGN_SELF", "AZIMUTH",
            "BACKGROUND", "BACKGROUND_ATTACHMENT", "BACKGROUND_COLOR", "BACKGROUND_IMAGE",
            "BACKGROUND_POSITION", "BACKGROUND_REPEAT", "BORDER", "BORDER_BOTTOM",
            "BORDER_BOTTOM_COLOR", "BORDER_BOTTOM_STYLE", "BORDER_BOTTOM_WIDTH",
            "BORDER_COLLAPSE", "BORDER_COLOR", "BORDER_LEFT", "BORDER_LEFT_COLOR",
            "BORDER_LEFT_STYLE", "BORDER_LEFT_WIDTH", "BORDER_RIGHT", "BORDER_RIGHT_COLOR",
            "BORDER_RIGHT_STYLE", "BORDER_RIGHT_WIDTH", "BORDER_SPACING",
            "BORDER_STYLE", "BORDER_TOP", "BORDER_TOP_COLOR", "BORDER_TOP_STYLE",
            "BORDER_TOP_WIDTH", "BORDER_WIDTH", "BOTTOM", "BOX_SIZING", "BREAK_AFTER",
            "BREAK_BEFORE", "BREAK_INSIDE", "CAPTION_SIDE", "CLEAR", "CLIP", "COLOR", "COLUMNS",
            "COLUMN_COUNT", "COLUMN_FILL", "COLUMN_GAP", "COLUMN_RULE", "COLUMN_RULE_COLOR",
            "COLUMN_RULE_STYLE", "COLUMN_RULE_WIDTH", "COLUMN_SPAN", "COLUMN_WIDTH",
            "CONTENT", "COUNTER_INCREMENT", "COUNTER_RESET", "CUE", "CUE_AFTER", "CUE_BEFORE",
            "CURSOR", "DIRECTION", "DISPLAY", "ELEVATION", "EMPTY_CELLS", "FLEX", "FLEX_BASIS",
            "FLEX_DIRECTION", "FLEX_FLOW", "FLEX_GROW", "FLEX_SHRINK", "FLEX_WRAP",
            "LIBCSS_FLOAT", "FONT", "FONT_FAMILY", "FONT_SIZE", "FONT_STYLE", "FONT_VARIANT",
            "FONT_WEIGHT", "HEIGHT", "JUSTIFY_CONTENT", "LEFT", "LETTER_SPACING", "LINE_HEIGHT",
            "LIST_STYLE", "LIST_STYLE_IMAGE", "LIST_STYLE_POSITION", "LIST_STYLE_TYPE",
            "MARGIN", "MARGIN_BOTTOM", "MARGIN_LEFT", "MARGIN_RIGHT", "MARGIN_TOP",
            "MAX_HEIGHT", "MAX_WIDTH", "MIN_HEIGHT", "MIN_WIDTH", "OPACITY", "ORDER", "ORPHANS",
            "OUTLINE", "OUTLINE_COLOR", "OUTLINE_STYLE", "OUTLINE_WIDTH", "OVERFLOW",
            "OVERFLOW_X", "OVERFLOW_Y", "PADDING", "PADDING_BOTTOM", "PADDING_LEFT",
            "PADDING_RIGHT", "PADDING_TOP", "PAGE_BREAK_AFTER", "PAGE_BREAK_BEFORE",
            "PAGE_BREAK_INSIDE", "PAUSE", "PAUSE_AFTER", "PAUSE_BEFORE", "PITCH_RANGE", "PITCH",
            "PLAY_DURING", "POSITION", "QUOTES", "RICHNESS", "RIGHT", "SPEAK_HEADER",
            "SPEAK_NUMERAL", "SPEAK_PUNCTUATION", "SPEAK", "SPEECH_RATE", "STRESS",
            "TABLE_LAYOUT", "TEXT_ALIGN", "TEXT_DECORATION", "TEXT_INDENT", "TEXT_TRANSFORM",
            "TOP", "UNICODE_BIDI", "VERTICAL_ALIGN", "VISIBILITY", "VOICE_FAMILY", "VOLUME",
            "WHITE_SPACE", "WIDOWS", "WIDTH", "WORD_SPACING", "WRITING_MODE", "Z_INDEX"
        };
        public const string Inherit = "inherit";
        public const string Important = "important";
        public const string Transparent = "transparent";
        public const string CurrentColor = "currentColor";

        public static readonly string[] Colors =  {
            "ALICEBLUE", "ANTIQUEWHITE", "AQUA", "AQUAMARINE", "AZURE",
            "BEIGE", "BISQUE", "BLACK", "BLANCHEDALMOND", "BLUE", "BLUEVIOLET", "BROWN",
            "BURLYWOOD", "CADETBLUE", "CHARTREUSE", "CHOCOLATE", "CORAL", "CORNFLOWERBLUE",
            "CORNSILK", "CRIMSON", "CYAN", "DARKBLUE", "DARKCYAN", "DARKGOLDENROD", "DARKGRAY",
            "DARKGREEN", "DARKGREY", "DARKKHAKI", "DARKMAGENTA", "DARKOLIVEGREEN", "DARKORANGE",
            "DARKORCHID", "DARKRED", "DARKSALMON", "DARKSEAGREEN", "DARKSLATEBLUE",
            "DARKSLATEGRAY", "DARKSLATEGREY", "DARKTURQUOISE", "DARKVIOLET", "DEEPPINK",
            "DEEPSKYBLUE", "DIMGRAY", "DIMGREY", "DODGERBLUE", "FELDSPAR", "FIREBRICK",
            "FLORALWHITE", "FORESTGREEN", "FUCHSIA", "GAINSBORO", "GHOSTWHITE", "GOLD",
            "GOLDENROD", "GRAY", "GREEN", "GREENYELLOW", "GREY", "HONEYDEW", "HOTPINK",
            "INDIANRED", "INDIGO", "IVORY", "KHAKI", "LAVENDER", "LAVENDERBLUSH", "LAWNGREEN",
            "LEMONCHIFFON", "LIGHTBLUE", "LIGHTCORAL", "LIGHTCYAN", "LIGHTGOLDENRODYELLOW",
            "LIGHTGRAY", "LIGHTGREEN", "LIGHTGREY", "LIGHTPINK", "LIGHTSALMON", "LIGHTSEAGREEN",
            "LIGHTSKYBLUE", "LIGHTSLATEBLUE", "LIGHTSLATEGRAY", "LIGHTSLATEGREY",
            "LIGHTSTEELBLUE", "LIGHTYELLOW", "LIME", "LIMEGREEN", "LINEN", "MAGENTA", "MAROON",
            "MEDIUMAQUAMARINE", "MEDIUMBLUE", "MEDIUMORCHID", "MEDIUMPURPLE",
            "MEDIUMSEAGREEN", "MEDIUMSLATEBLUE", "MEDIUMSPRINGGREEN", "MEDIUMTURQUOISE",
            "MEDIUMVIOLETRED", "MIDNIGHTBLUE", "MINTCREAM", "MISTYROSE", "MOCCASIN",
            "NAVAJOWHITE", "NAVY", "OLDLACE", "OLIVE", "OLIVEDRAB", "ORANGE", "ORANGERED",
            "ORCHID", "PALEGOLDENROD", "PALEGREEN", "PALETURQUOISE", "PALEVIOLETRED",
            "PAPAYAWHIP", "PEACHPUFF", "PERU", "PINK", "PLUM", "POWDERBLUE", "PURPLE", "RED",
            "ROSYBROWN", "ROYALBLUE", "SADDLEBROWN", "SALMON", "SANDYBROWN", "SEAGREEN",
            "SEASHELL", "SIENNA", "SILVER", "SKYBLUE", "SLATEBLUE", "SLATEGRAY", "SLATEGREY",
            "SNOW", "SPRINGGREEN", "STEELBLUE", "TAN", "TEAL", "THISTLE", "TOMATO", "TURQUOISE",
            "VIOLET", "VIOLETRED", "WHEAT", "WHITE", "WHITESMOKE", "YELLOW", "YELLOWGREEN"
        };
    }

    public enum CssLanguageLevel : int
    {
        CSS_LEVEL_1 = 0,
        CSS_LEVEL_2 = 1,
        CSS_LEVEL_21 = 2,
        CSS_LEVEL_3 = 3,
        CSS_LEVEL_DEFAULT = CSS_LEVEL_21
    }
    public enum CssOrigin : int
    {
        CSS_ORIGIN_UA = 0,    // < User agent stylesheet
        CSS_ORIGIN_USER = 1,  // < User stylesheet
        CSS_ORIGIN_AUTHOR = 2 // < Author stylesheet
    }

    // language.h:38
    public enum CssLanguageState
    {
        CHARSET_PERMITTED,
        IMPORT_PERMITTED,
        NAMESPACE_PERMITTED,
        HAD_RULE
    }

    public struct CssStylesheetContextEntry
    {
        public CssParserEvent Type;
        public CssRule Rule;
        public CssStylesheetContextEntry(CssParserEvent type)
        {
            Type = type;
            Rule = null;
        }
        public CssStylesheetContextEntry(CssParserEvent type, CssRule rule)
        {
            Type = type;
            Rule = rule;
        }
    }

    // stylesheet.h:170, struct css_stylesheet
    public class CssStylesheet
    {
        bool InlineStyle;
        CssParser Parser;
        CssLanguageLevel Level;
        uint RuleCount;          // Number of rules in sheet
        public LinkedList<CssRule> RuleList; // List of rules in sheet

        string Title;
        string Url;

        // css_language, language.h:32
        Stack<CssStylesheetContextEntry> Context;
        CssLanguageState LanguageState; // State flag, for at-rule handling
        string DefaultNamespace; // Default namespace URI

        bool QuirksAllowed;

        // libcss/src/stylesheet.c:125
        public CssStylesheet(string charset, string url, string title, bool inlineStyle, CssLanguageLevel level = CssLanguageLevel.CSS_LEVEL_DEFAULT)
        {
            InlineStyle = inlineStyle;

            var ps = new ParserState(0, 0);

            if (InlineStyle)
                ps.State = ParserStateValue.sInlineStyle;

            Parser = new CssParser(charset, ps);
            Parser.RegisterCallbacks(EventHandler);
            Context = new Stack<CssStylesheetContextEntry>();

            Level = level;
            Url = url;
            Title = title;

            DefaultNamespace = null;

            RuleCount = 0;
            RuleList = new LinkedList<CssRule>();

            QuirksAllowed = false;
        }

        // stylesheet.c:311
        public void AppendData(string data)
        {
            Parser.ParseChunk(data);
        }

        // libcss/src/stylesheet.c:1414
        void AddRule(CssRule rule, CssRule parent)
        {
            /* Need to fill in rule's index field before adding selectors
             * because selector chains consider the rule index for sort order
             */
            rule.Index = RuleCount;

            // Add any selectors to the hash
            //error = _add_selectors(sheet, rule);

            // Add to the sheet's size
            //sheet->size += _rule_size(rule);

            if (parent != null)
            {
                Console.WriteLine("UNIMPLEMENTED parent rule handling 203");
                /*
                css_rule_media* media = (css_rule_media*)parent;

                // Parent must be an @media rule, or NULL
                assert(parent->type == CSS_RULE_MEDIA);

                // Add rule to parent
                rule->ptype = CSS_RULE_PARENT_RULE;
                rule->parent = parent;
                sheet->rule_count++;

                if (media->last_child == NULL)
                {
                    rule->prev = rule->next = NULL;
                    media->first_child = media->last_child = rule;
                }
                else
                {
                    media->last_child->next = rule;
                    rule->prev = media->last_child;
                    rule->next = NULL;
                    media->last_child = rule;
                }*/
            }
            else
            {
                // Add rule to sheet
                rule.ParentType = CssRuleParentType.CSS_RULE_PARENT_STYLESHEET;
                rule.ParentSheet = this;
                RuleCount++;

                RuleList.AddLast(rule);
            }

            // TODO: needs to trigger some event announcing styles have changed
        }

        // libcss/src/parse/language.c:200
        public void EventHandler(CssParserEvent type, bool vector)
        {
            Console.WriteLine($"Event handler type {type.ToString()}!");

            switch (type)
            {
                case CssParserEvent.CSS_PARSER_START_STYLESHEET:
                    HandleStartStylesheet();
                    break;
                case CssParserEvent.CSS_PARSER_END_STYLESHEET:
                    HandleEndStylesheet();
                    break;
                case CssParserEvent.CSS_PARSER_START_RULESET:
                    HandleStartRuleset(vector);
                    break;
                case CssParserEvent.CSS_PARSER_END_RULESET:
                    HandleEndRuleset();
                    break;
                /*case CssParserEvent.CSS_PARSER_START_ATRULE:
                    HandleStartAtRule();
                    break;
                case CssParserEvent.CSS_PARSER_END_ATRULE:
                    HandleEndAtRule();
                    break;
                case CssParserEvent.CSS_PARSER_START_BLOCK:
                    HandleStartBlock();
                    break;
                case CssParserEvent.CSS_PARSER_END_BLOCK:
                    HandleEndBlock();
                    break;
                case CssParserEvent.CSS_PARSER_BLOCK_CONTENT:
                    HandleBlockContent();
                    break;
                case CssParserEvent.CSS_PARSER_END_BLOCK_CONTENT:
                    HandleEndBlockContent();
                    break;*/
                case CssParserEvent.CSS_PARSER_DECLARATION:
                    HandleDeclaration();
                    break;
                default:
                    Console.WriteLine($"UNIMPLEMENTED event handle type {type.ToString()} requested!");
                    break;
            }
        }

        void HandleStartStylesheet()
        {
            Context.Push(new CssStylesheetContextEntry(CssParserEvent.CSS_PARSER_START_STYLESHEET));
        }

        void HandleEndStylesheet()
        {
            var e = Context.Pop();

            if (e.Type != CssParserEvent.CSS_PARSER_START_STYLESHEET)
            {
                Console.WriteLine("Stylesheet Stack inconsistency");
            }
        }

        // language.c:276
        void HandleStartRuleset(bool vector)
        {
            CssRule parentRule = null;

            // Retrieve parent rule from stack, if any
            if (Context.Count > 0)
            {
                var cur = Context.Peek();

                if (cur.Type != CssParserEvent.CSS_PARSER_START_STYLESHEET)
                    parentRule = cur.Rule;
            }

            var rule = new CssRule(CssRuleType.CSS_RULE_SELECTOR);

            if (vector)
            {
                // Parse selectors, if there are any
                ParseSelectorList(Parser.Tokens, rule);
            }

            var entry = new CssStylesheetContextEntry(CssParserEvent.CSS_PARSER_START_RULESET, rule);
            Context.Push(entry);

            AddRule(rule, parentRule);

            // Flag that we've had a valid rule, so @import/@namespace/@charset have no effect.
            LanguageState = CssLanguageState.HAD_RULE;
        }

        // language.c:329
        void HandleEndRuleset()
        {
            if (Context.Count == 0)
            {
                Console.WriteLine("ERROR: Invalid css start ruleset / end ruleset sequence");
                return;
            }

            var entry = Context.Peek();
            if (entry.Type != CssParserEvent.CSS_PARSER_START_RULESET)
            {
                Console.WriteLine("ERROR: Invalid css start ruleset / end ruleset sequence");
                return;
            }

            Context.Pop();
        }

        // language.c:789
        void HandleDeclaration()
        {
            int index = 0;
            var tokens = Parser.Tokens;

            if (Context.Count == 0)
            {
                Console.WriteLine("ERROR: Invalid declaration sequence 1");
                return;
            }

            var entry = Context.Peek();
            var rule = entry.Rule;
            if (rule.Type != CssRuleType.CSS_RULE_SELECTOR &&
                rule.Type != CssRuleType.CSS_RULE_PAGE &&
                rule.Type != CssRuleType.CSS_RULE_FONT_FACE)
            {
                Console.WriteLine("ERROR: Invalid declaration sequence 2");
                return;
            }

            // Strip any leading whitespace (can happen if in nested block)
            ConsumeWhitespace(tokens, ref index);

            /* IDENT ws ':' ws value
             *
             * In CSS 2.1, value is any1, so '{' or ATKEYWORD => parse error
             */
            if (index >= tokens.Count)
            {
                Console.WriteLine("ERROR: Invalid declaration sequence 3");
                return;
            }

            var ident = tokens[index++];
            if (ident.Type != CssTokenType.CSS_TOKEN_IDENT)
            {
                Console.WriteLine("ERROR: Invalid declaration sequence 4");
                return;
            }

            ConsumeWhitespace(tokens, ref index);

            if (index >= tokens.Count)
            {
                Console.WriteLine("ERROR: Invalid declaration sequence 5");
                return;
            }

            var token = tokens[index++];

            if (!token.IsChar(':'))
            {
                Console.WriteLine("ERROR: Invalid declaration sequence 5");
                return;
            }

            ConsumeWhitespace(tokens, ref index);

            if (rule.Type == CssRuleType.CSS_RULE_FONT_FACE)
            {
                /*
                css_rule_font_face* ff_rule = (css_rule_font_face*)rule;
                error = css__parse_font_descriptor(c, ident, vector, &ctx, ff_rule);
                */
                Console.WriteLine("UNIMPLEMENTED font face rule 436");
            }
            else
            {
                //error = parseProperty(c, ident, vector, &ctx, rule);
                ParseProperty(ident, tokens, ref index, rule);
            }
        }

        // language.c:1461
        void ParseSpecific(List<CssToken> tokens, ref int index, bool inNot, CssSelector parent)
        {
            Console.WriteLine("UNIMPLEMENTED 365");
        }

        // language.c:1516
        void ParseAppendSpecific(List<CssToken> tokens, ref int index, CssSelector parent)
        {
            ParseSpecific(tokens, ref index, false, parent);

            Console.WriteLine("UNIMPLEMENTED 373");

            //css__stylesheet_selector_append_specific(c->sheet, parent, &specific);
        }

        // language.c:1531
        void ParseSelectorSpecifics(List<CssToken> tokens, ref int index, CssSelector parent)
        {
            // specifics -> specific*
            while (true)
            {
                if (index >= tokens.Count)
                    break;

                var token = tokens[index];

                if (token.Type == CssTokenType.CSS_TOKEN_S ||
                    token.IsChar('+') ||
                    token.IsChar('>') ||
                    token.IsChar('~') ||
                    token.IsChar(','))
                {
                    break;
                }

                ParseAppendSpecific(tokens, ref index, parent);
            }

        }

        // language.c:1553
        void ParseTypeSelector(List<CssToken> tokens, ref int index, ref CssQname qname)
        {
            CssToken token;
            string prefix = null;

            /* type_selector    -> namespace_prefix? element_name
             * namespace_prefix -> [ IDENT | '*' ]? '|'
             * element_name	    -> IDENT | '*'
             */

            token = tokens[index];

            if (!token.IsChar('|'))
            {
                prefix = token.iData;
                index++;
                token = tokens[index];
            }

            if (index < tokens.Count && token.IsChar('|'))
            {
                // Have namespace prefix
                index += 2;

                if (index >= tokens.Count)
                {
                    Console.WriteLine("Should throw CSS_INVALID error");
                    return;
                }

                // Expect element_name
                token = tokens[index];

                if (token.Type != CssTokenType.CSS_TOKEN_IDENT && !token.IsChar('*'))
                {
                    // Same as above
                    Console.WriteLine("Should throw CSS_INVALID error");
                    return;
                }
                /*
                error = lookupNamespace(c, prefix, &qname->ns);
                if (error != CSS_OK)
                    return error;
                 */
                Console.WriteLine("UNIMLEMENTED 345");

                qname.Name = token.iData;
            }
            else
            {
                // No namespace prefix
                if (DefaultNamespace == null)
                    qname.Namespace = CssStrings.Universal;
                else
                    qname.Namespace = DefaultNamespace;

                qname.Name = prefix;
            }
        }

        // language.c:1614
        CssSelector ParseSimpleSelector(List<CssToken> tokens, ref int index)
        {
            CssSelector selector;
            //int origIndex = index;
            CssQname qname = new CssQname();

            /* simple_selector  -> type_selector specifics
             *        -> specific specifics
             */
            var token = tokens[index];
            if (token.Type == CssTokenType.CSS_TOKEN_IDENT ||
                token.IsChar('*') || token.IsChar('|'))
            {
                // Have type selector
                ParseTypeSelector(tokens, ref index, ref qname);
                selector = new CssSelector(ref qname, InlineStyle);
            }
            else
            {
                // Universal selector
                if (DefaultNamespace == null)
                    qname.Namespace = CssStrings.Universal;
                else
                    qname.Namespace = DefaultNamespace;

                qname.Name = CssStrings.Universal;
                selector = new CssSelector(ref qname, InlineStyle);
                /*

                // Ensure we have at least one specific selector
                error = parseAppendSpecific(c, vector, ctx, &selector);
                if (error != CSS_OK)
                {
                    css__stylesheet_selector_destroy(c->sheet, selector);
                    return error;
                }*/
                Console.WriteLine("UNIMPLEMENTED 357");
            }

            ParseSelectorSpecifics(tokens, ref index, selector);

            return selector;
        }

        CssCombinator ParseCombinator(List<CssToken> tokens, ref int index)
        {
            CssCombinator comb = CssCombinator.CSS_COMBINATOR_NONE;

            /* combinator	   -> ws '+' ws | ws '>' ws | ws '~' ws | ws1 */

            //while ((token = parserutils_vector_peek(vector, *ctx)) != NULL)
            while (index < tokens.Count)
            {
                var token = tokens[index];

                if (token.IsChar('+'))
                    comb = CssCombinator.CSS_COMBINATOR_SIBLING;
                else if (token.IsChar('>'))
                    comb = CssCombinator.CSS_COMBINATOR_PARENT;
                else if (token.IsChar('~'))
                    comb = CssCombinator.CSS_COMBINATOR_GENERIC_SIBLING;
                else if (token.Type == CssTokenType.CSS_TOKEN_S)
                    comb = CssCombinator.CSS_COMBINATOR_ANCESTOR;
                else
                    break;

                index++;

                // If we've seen a '+', '>', or '~', we're done.
                if (comb != CssCombinator.CSS_COMBINATOR_ANCESTOR)
                    break;
            }

            // No valid combinator found
            if (comb == CssCombinator.CSS_COMBINATOR_NONE)
            {
                Console.WriteLine("ERROR: No valid combinator found, should return error");
                throw new InvalidOperationException();
            }

            // Consume any trailing whitespace
            ConsumeWhitespace(tokens, ref index);

            return comb;
        }

        // language.c:1721
        CssSelector ParseSelector(List<CssToken> tokens, ref int index)
        {
            CssSelector result;

            /* selector -> simple_selector [ combinator simple_selector ]* ws
                 *
                 * Note, however, that, as combinator can be wholly whitespace,
                 * there's an ambiguity as to whether "ws" has been reached. We
                 * resolve this by attempting to extract a combinator, then
                 * recovering when we detect that we've reached the end of the
                 * selector.
                 */

            var selector = ParseSimpleSelector(tokens, ref index);
            result = selector;

            while (index < tokens.Count)
            {
                var token = tokens[index];
                if (token.IsChar(',')) break;

                var comb = ParseCombinator(tokens, ref index); //FIXME: Any error of ParsCombinator is fatal

                /* In the case of "html , body { ... }", the whitespace after
                 * "html" and "body" will be considered an ancestor combinator.
                 * This clearly is not the case, however. Therefore, as a
                 * special case, if we've got an ancestor combinator and there
                 * are no further tokens, or if the next token is a comma,
                 * we ignore the supposed combinator and continue. */

                // TODO: Test this place!
                if (comb == CssCombinator.CSS_COMBINATOR_ANCESTOR &&
                    (index >= tokens.Count || tokens[index + 1].IsChar(',')))
                {
                    continue;
                }

                var other = ParseSimpleSelector(tokens, ref index);
                result = other;
                /*
                error = css__stylesheet_selector_combine(c->sheet,
                        comb, selector, other);
                if (error != CSS_OK)
                {
                    css__stylesheet_selector_destroy(c->sheet, selector);
                    return error;
                }*/
                Console.WriteLine("UNIMPLEMENTED 496");

                selector = other;
            }

            return result;
        }

        // language.c:1782
        void ParseSelectorList(List<CssToken> tokens, CssRule rule)
        {
            int index = 0;

            // Strip any leading whitespace (can happen if in nested block)
            ConsumeWhitespace(tokens, ref index);

            // selector_list   -> selector [ ',' ws selector ]*
            var selector = ParseSelector(tokens, ref index);

            rule.AddSelector(selector);

            if (index >= tokens.Count) return;

            CssToken token = tokens[index];
            for (; index < tokens.Count; index++, token = tokens[index])
            {
                if (!token.IsChar(','))
                {
                    Console.WriteLine($"ERROR parsing CSS: following char is not ,");
                    return;
                }

                ConsumeWhitespace(tokens, ref index);

                selector = ParseSelector(tokens, ref index);
                rule.AddSelector(selector);
            }
        }

        // utils.c:682
        CssStatus ParseProperty_NamedColour(string data, ref uint result)
        {
            result = 0;

            int i;
            for (i = 0; i < CssStrings.Colors.Length; i++)
            {
                if (data.Equals(CssStrings.Colors[i], StringComparison.OrdinalIgnoreCase))
                    break;
            }

            if (i < CssStrings.Colors.Length)
            {
                // Known named color
                result = Colormap.Values[i];
                return CssStatus.CSS_OK;
            }

            Console.WriteLine("UNIMPLEMENTED 711");
            // We don't know this colour name; ask the client
            //if (c->sheet->color != NULL)
            //  return c->sheet->color(c->sheet->color_pw, data, result);

            // Invalid color name
            return CssStatus.CSS_INVALID;
        }

        // parse/properties/utils.c:368
        void ParseProperty_ColourSpecifier(List<CssToken> tokens, ref int index, out ushort value, out uint result)
        {
            result = 0;
            value = 0;

            ConsumeWhitespace(tokens, ref index);

            /* IDENT(<colour name>) |
             * HASH(rgb | rrggbb) |
             * FUNCTION(rgb) [ [ NUMBER | PERCENTAGE ] ',' ] {3} ')'
             * FUNCTION(rgba) [ [ NUMBER | PERCENTAGE ] ',' ] {4} ')'
             * FUNCTION(hsl) ANGLE ',' PERCENTAGE ',' PERCENTAGE  ')'
             * FUNCTION(hsla) ANGLE ',' PERCENTAGE ',' PERCENTAGE ',' NUMBER ')'
             *
             * For quirks, NUMBER | DIMENSION | IDENT, too
             * I.E. "123456" -> NUMBER, "1234f0" -> DIMENSION, "f00000" -> IDENT
             */
            if (index >= tokens.Count)
            {
                Console.WriteLine("Invalid CSS 677");
                return;
            }

            var token = tokens[index++];
            if ((token.Type != CssTokenType.CSS_TOKEN_IDENT &&
                 token.Type != CssTokenType.CSS_TOKEN_HASH &&
                 token.Type != CssTokenType.CSS_TOKEN_FUNCTION))
            {

                if (!QuirksAllowed ||
                    (token.Type != CssTokenType.CSS_TOKEN_NUMBER &&
                     token.Type != CssTokenType.CSS_TOKEN_DIMENSION))
                {
                    Console.WriteLine("Invalid CSS 691");
                    return;
                }
            }

            if (token.Type == CssTokenType.CSS_TOKEN_IDENT)
            {
                if (token.iData.Equals(CssStrings.Transparent, StringComparison.OrdinalIgnoreCase))
                {
                    value = (ushort)OpColor.COLOR_TRANSPARENT;
                    result = 0; // black transparent
                    return;
                }
                else if (token.iData.Equals(CssStrings.CurrentColor, StringComparison.OrdinalIgnoreCase))
                {
                    value = (ushort)OpColor.COLOR_CURRENT_COLOR;
                    result = 0;
                    return;
                }

                var error = ParseProperty_NamedColour(token.iData, ref result);
                if (error != CssStatus.CSS_OK && QuirksAllowed)
                {
                    Console.WriteLine("UNIMPLEMENTED 757");
                    /*
                    error = css__parse_hash_colour(token->idata, result);
                    if (error == CSS_OK)
                        c->sheet->quirks_used = true;
                    */
                }

                if (error != CssStatus.CSS_OK)
                {
                    Console.WriteLine("Invalid CSS 764");
                    return;
                }
            }
            else if (token.Type == CssTokenType.CSS_TOKEN_HASH)
            {
                Console.WriteLine("UNIMPLEMENTED 733");
                /*
                error = css__parse_hash_colour(token->idata, result);
                if (error != CSS_OK)
                    goto invalid;
                */
            }
            else if (QuirksAllowed &&
                  token.Type == CssTokenType.CSS_TOKEN_NUMBER)
            {
                /*
                error = css__parse_hash_colour(token->idata, result);
                if (error == CSS_OK)
                    c->sheet->quirks_used = true;
                else
                    goto invalid;*/
                Console.WriteLine("UNIMPLEMENTED 749");
            }
            else if (QuirksAllowed &&
                  token.Type == CssTokenType.CSS_TOKEN_DIMENSION)
            {
                /*
                error = css__parse_hash_colour(token->idata, result);
                if (error == CSS_OK)
                    c->sheet->quirks_used = true;
                else
                    goto invalid;*/
                Console.WriteLine("UNIMPLEMENTED 760");
            }
            else if (token.Type == CssTokenType.CSS_TOKEN_FUNCTION)
            {
                Console.WriteLine("UNIMPLEMENTED 764");
                /*
                uint8_t r = 0, g = 0, b = 0, a = 0xff;
                int colour_channels = 0;

                if ((lwc_string_caseless_isequal(
                        token->idata, c->strings[RGB],
                        &match) == lwc_error_ok && match))
                {
                    colour_channels = 3;
                }
                else if ((lwc_string_caseless_isequal(
                      token->idata, c->strings[RGBA],
                      &match) == lwc_error_ok && match))
                {
                    colour_channels = 4;
                }
                if ((lwc_string_caseless_isequal(
                      token->idata, c->strings[HSL],
                      &match) == lwc_error_ok && match))
                {
                    colour_channels = 5;
                }
                else if ((lwc_string_caseless_isequal(
                      token->idata, c->strings[HSLA],
                      &match) == lwc_error_ok && match))
                {
                    colour_channels = 6;
                }

                if (colour_channels == 3 || colour_channels == 4)
                {
                    int i;
                    css_token_type valid = CSS_TOKEN_NUMBER;
                    uint8_t* components[4] = { &r, &g, &b, &a };

                    for (i = 0; i < colour_channels; i++)
                    {
                        uint8_t* component;
                        css_fixed num;
                        size_t consumed = 0;
                        int32_t intval;
                        bool int_only;

                        component = components[i];

                        consumeWhitespace(vector, ctx);

                        token = parserutils_vector_peek(vector, *ctx);
                        if (token == NULL || (token->type !=
                                CSS_TOKEN_NUMBER &&
                                token->type !=
                                CSS_TOKEN_PERCENTAGE))
                            goto invalid;

                        if (i == 0)
                            valid = token->type;
                        else if (i < 3 && token->type != valid)
                            goto invalid;

                        // The alpha channel may be a float
                        if (i < 3)
                            int_only = (valid == CSS_TOKEN_NUMBER);
                        else
                            int_only = false;

                        num = css__number_from_lwc_string(token->idata,
                                int_only, &consumed);
                        if (consumed != lwc_string_length(token->idata))
                            goto invalid;

                        if (valid == CSS_TOKEN_NUMBER)
                        {
                            if (i == 3)
                            {
                                // alpha channel
                                intval = FIXTOINT(
                                    FMUL(num, F_255));
                            }
                            else
                            {
                                // colour channels
                                intval = FIXTOINT(num);
                            }
                        }
                        else
                        {
                            intval = FIXTOINT(
                                FDIV(FMUL(num, F_255), F_100));
                        }

                        if (intval > 255)
                            *component = 255;
                        else if (intval < 0)
                            *component = 0;
                        else
                            *component = intval;

                        parserutils_vector_iterate(vector, ctx);

                        consumeWhitespace(vector, ctx);

                        token = parserutils_vector_peek(vector, *ctx);
                        if (token == NULL)
                            goto invalid;

                        if (i != (colour_channels - 1) &&
                                tokenIsChar(token, ','))
                        {
                            parserutils_vector_iterate(vector, ctx);
                        }
                        else if (i == (colour_channels - 1) &&
                              tokenIsChar(token, ')'))
                        {
                            parserutils_vector_iterate(vector, ctx);
                        }
                        else
                        {
                            goto invalid;
                        }
                    }
                }
                else if (colour_channels == 5 || colour_channels == 6)
                {
                    // hue - saturation - lightness
                    size_t consumed = 0;
                    css_fixed hue, sat, lit;
                    int32_t alpha = 255;

                    // hue is a number without a unit representing an
                    // angle (0-360) degrees
                    consumeWhitespace(vector, ctx);

                    token = parserutils_vector_iterate(vector, ctx);
                    if ((token == NULL) || (token->type != CSS_TOKEN_NUMBER))
                        goto invalid;

                    hue = css__number_from_lwc_string(token->idata, false, &consumed);
                    if (consumed != lwc_string_length(token->idata))
                        goto invalid; // failed to consume the whole string as a number

                    // Normalise hue to the range [0, 360)
                    while (hue < 0)
                        hue += F_360;
                    while (hue >= F_360)
                        hue -= F_360;

                    consumeWhitespace(vector, ctx);

                    token = parserutils_vector_iterate(vector, ctx);
                    if (!tokenIsChar(token, ','))
                        goto invalid;


                    // saturation
                    consumeWhitespace(vector, ctx);

                    token = parserutils_vector_iterate(vector, ctx);
                    if ((token == NULL) || (token->type != CSS_TOKEN_PERCENTAGE))
                        goto invalid;

                    sat = css__number_from_lwc_string(token->idata, false, &consumed);
                    if (consumed != lwc_string_length(token->idata))
                        goto invalid; // failed to consume the whole string as a number

                    // Normalise saturation to the range [0, 100]
                    if (sat < INTTOFIX(0))
                        sat = INTTOFIX(0);
                    else if (sat > INTTOFIX(100))
                        sat = INTTOFIX(100);

                    consumeWhitespace(vector, ctx);

                    token = parserutils_vector_iterate(vector, ctx);
                    if (!tokenIsChar(token, ','))
                        goto invalid;


                    // lightness
                    consumeWhitespace(vector, ctx);

                    token = parserutils_vector_iterate(vector, ctx);
                    if ((token == NULL) || (token->type != CSS_TOKEN_PERCENTAGE))
                        goto invalid;

                    lit = css__number_from_lwc_string(token->idata, false, &consumed);
                    if (consumed != lwc_string_length(token->idata))
                        goto invalid; // failed to consume the whole string as a number

                    // Normalise lightness to the range [0, 100]
                    if (lit < INTTOFIX(0))
                        lit = INTTOFIX(0);
                    else if (lit > INTTOFIX(100))
                        lit = INTTOFIX(100);

                    consumeWhitespace(vector, ctx);

                    token = parserutils_vector_iterate(vector, ctx);

                    if (colour_channels == 6)
                    {
                        // alpha

                        if (!tokenIsChar(token, ','))
                            goto invalid;

                        consumeWhitespace(vector, ctx);

                        token = parserutils_vector_iterate(vector, ctx);
                        if ((token == NULL) || (token->type != CSS_TOKEN_NUMBER))
                            goto invalid;

                        alpha = css__number_from_lwc_string(token->idata, false, &consumed);
                        if (consumed != lwc_string_length(token->idata))
                            goto invalid; // failed to consume the whole string as a number

                        alpha = FIXTOINT(FMUL(alpha, F_255));

                        consumeWhitespace(vector, ctx);

                        token = parserutils_vector_iterate(vector, ctx);

                    }

                    if (!tokenIsChar(token, ')'))
                        goto invalid;

                    // have a valid HSV entry, convert to RGB
                    HSL_to_RGB(hue, sat, lit, &r, &g, &b);

                    //* apply alpha
                    if (alpha > 255)
                        a = 255;
                    else if (alpha < 0)
                        a = 0;
                    else
                        a = alpha;

                }
                else
                {
                    goto invalid;
                }

                *result = ((unsigned)a << 24) | (r << 16) | (g << 8) | b;
                */
            }

            value = (ushort)OpColor.COLOR_SET;
        }

        // autogenerated_color.c:35
        void ParseProperty_Color(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;

            if (index >= tokens.Count)
            {
                Console.WriteLine("ERROR: Invalid CSS 659");
                return;
            }

            var token = tokens[index++];

            if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                token.iData == CssStrings.Inherit)
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_COLOR,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
            }
            else
            {
                ushort value = 0;
                uint color = 0;
                index = origIndex;

                ParseProperty_ColourSpecifier(tokens, ref index, out value, out color);

                //ParseProperty_AppendOPV((ushort)CssPropertiesEnum.CSS_PROP_COLOR);
                style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_COLOR, 0, value));

                if (value == (ushort)OpColor.COLOR_SET)
                {
                    style.AppendStyle(new OpCode(color));
                }
            }
        }

        void ParseImportant(List<CssToken> tokens, ref int index, ref OpCodeFlag flags)
        {
            int origIndex = index;
            ConsumeWhitespace(tokens, ref index);

            if (index < tokens.Count - 1)
            {
                var token = tokens[index++];

                if (token.IsChar('!'))
                {
                    ConsumeWhitespace(tokens, ref index);
                    token = tokens[index++];

                    if (index >= tokens.Count || token.Type != CssTokenType.CSS_TOKEN_IDENT)
                    {
                        Console.WriteLine("CSS Invalid 721");
                        index = origIndex;
                        return;
                    }

                    if (token.iData.Equals(CssStrings.Important, StringComparison.OrdinalIgnoreCase))
                    {
                        flags |= OpCodeFlag.FLAG_IMPORTANT;
                    }
                    else
                    {
                        Console.WriteLine("CSS Invalid 731");
                        index = origIndex;
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("CSS Invalid 738");
                    index = origIndex;
                    return;
                }
            }
        }


        // language.c:1845
        void ParseProperty(CssToken property, List<CssToken> tokens, ref int index, CssRule rule)
        {
            //css_prop_handler handler = NULL;
            int i = 0;

            // Find property index
            // TODO: improve on this linear search
            for (i = 0; i < CssStrings.Props.Length; i++)
            {
                if (property.iData.Equals(CssStrings.Props[i], StringComparison.OrdinalIgnoreCase))
                    break;
            }

            if (i == CssStrings.Props.Length)
            {
                throw new Exception("CSS Invalid");
                return;
            }

            // Allocate style
            var style = new CssStyle(this);

            // TODO: Get handler

            // FIXME: Testing. Call the handler
            ParseProperty_Color(tokens, ref index, style);

            // Determine if this declaration is important or not
            OpCodeFlag flags = 0;
            ParseImportant(tokens, ref index, ref flags);

            // Ensure that we've exhausted all the input
            ConsumeWhitespace(tokens, ref index);
            if (index < tokens.Count - 1)
            {
                // Trailing junk, so discard declaration
                Console.WriteLine("Invalid CSS, trailig junk");
                return;
            }

            // If it's important, then mark the style appropriately
            if (flags != 0)
            {
                //css__make_style_important(style);
                Console.WriteLine("UNIMPLEMENTED 792");
            }

            // Append style to rule
            rule.AppendStyle(style);
        }

        static void ConsumeWhitespace(List<CssToken> tokens, ref int index)
        {
            /*while ((token = parserutils_vector_peek(vector, * ctx)) != NULL &&
			        token->type == CSS_TOKEN_S)
		        parserutils_vector_iterate(vector, ctx);*/

            while (true)
            {
                if (index >= tokens.Count - 1) break; // FIXME: Check this place!

                var token = tokens[index];

                if (token.Type == CssTokenType.CSS_TOKEN_S)
                    index++;
                else
                    break;
            }
        }
    }

    // select.c:39
    // Container for stylesheet selection info
    public struct CssSelectSheet
    {
        public CssStylesheet Sheet;
        public CssOrigin Origin;
        //CssMqQuery Media;
    }

    public class CssSelectionContext
    {
        public List<CssSelectSheet> Sheets = new List<CssSelectSheet>();
        // Maybe something else, but for now empty

        public CssSelectionContext()
        {

        }
    }
}
