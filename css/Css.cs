using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    public enum CssStylesheetEnum : byte
    {
        BASE = 0,       // base style sheet
        QUIRKS = 1,     // quirks mode stylesheet
        ADBLOCK = 2,    // adblocking stylesheet
        USER = 3,       // user stylesheet
        START = 4       // start of document stylesheets
    }

    // propstrings.c:30
    public static class CssStrings
    {
        public const string Universal = "*";
        public static readonly string[] Props =  {
            "ALIGN_CONTENT",        "ALIGN_ITEMS",          "ALIGN_SELF",           "AZIMUTH",              // 0
            "BACKGROUND",           "BACKGROUND_ATTACHMENT","BACKGROUND_COLOR",     "BACKGROUND_IMAGE",     // 4
            "BACKGROUND_POSITION",  "BACKGROUND_REPEAT",    "BORDER",               "BORDER_BOTTOM",        // 8
            "BORDER_BOTTOM_COLOR",  "BORDER_BOTTOM_STYLE",  "BORDER_BOTTOM_WIDTH",  "BORDER_COLLAPSE",      // 12
            "BORDER_COLOR",         "BORDER_LEFT",          "BORDER_LEFT_COLOR",    "BORDER_LEFT_STYLE",    // 16
            "BORDER_LEFT_WIDTH",    "BORDER_RIGHT",         "BORDER_RIGHT_COLOR",   "BORDER_RIGHT_STYLE",   // 20
            "BORDER_RIGHT_WIDTH",   "BORDER_SPACING",       "BORDER_STYLE",         "BORDER_TOP",           // 24
            "BORDER_TOP_COLOR",     "BORDER_TOP_STYLE",     "BORDER_TOP_WIDTH",     "BORDER_WIDTH",         // 28
            "BOTTOM",               "BOX_SIZING",           "BREAK_AFTER",          "BREAK_BEFORE",         // 32
            "BREAK_INSIDE",         "CAPTION_SIDE",         "CLEAR",                "CLIP",                 // 36
            "COLOR",                "COLUMNS",              "COLUMN_COUNT",         "COLUMN_FILL",          // 40
            "COLUMN_GAP",           "COLUMN_RULE",          "COLUMN_RULE_COLOR",    "COLUMN_RULE_STYLE",    // 44
            "COLUMN_RULE_WIDTH",    "COLUMN_SPAN",          "COLUMN_WIDTH",         "CONTENT",              // 48
            "COUNTER_INCREMENT",    "COUNTER_RESET",        "CUE",                  "CUE_AFTER",            // 52
            "CUE_BEFORE",           "CURSOR",               "DIRECTION",            "DISPLAY",              // 56
            "ELEVATION",            "EMPTY_CELLS",          "FLEX",                 "FLEX_BASIS",           // 60
            "FLEX_DIRECTION",       "FLEX_FLOW",            "FLEX_GROW",            "FLEX_SHRINK",          // 64
            "FLEX_WRAP",            "FLOAT",                "FONT",                 "FONT_FAMILY",          // 68
            "FONT_SIZE",            "FONT_STYLE",           "FONT_VARIANT",         "FONT_WEIGHT",          // 72
            "HEIGHT",               "JUSTIFY_CONTENT",      "LEFT",                 "LETTER_SPACING",       // 76
            "LINE_HEIGHT",          "LIST_STYLE",           "LIST_STYLE_IMAGE",     "LIST_STYLE_POSITION",  // 80
            "LIST_STYLE_TYPE",      "MARGIN",               "MARGIN_BOTTOM",        "MARGIN_LEFT",          // 84
            "MARGIN_RIGHT",         "MARGIN_TOP",           "MAX_HEIGHT",           "MAX_WIDTH",            // 88
            "MIN_HEIGHT",           "MIN_WIDTH",            "OPACITY",              "ORDER",                // 92
            "ORPHANS",              "OUTLINE",              "OUTLINE_COLOR",        "OUTLINE_STYLE",        // 96
            "OUTLINE_WIDTH",        "OVERFLOW",             "OVERFLOW_X",           "OVERFLOW_Y",           // 100
            "PADDING",              "PADDING_BOTTOM",       "PADDING_LEFT",         "PADDING_RIGHT",        // 104
            "PADDING_TOP",          "PAGE_BREAK_AFTER",     "PAGE_BREAK_BEFORE",    "PAGE_BREAK_INSIDE",    // 108
            "PAUSE",                "PAUSE_AFTER",          "PAUSE_BEFORE",         "PITCH_RANGE",          // 112
            "PITCH",                "PLAY_DURING",          "POSITION",             "QUOTES",               // 116
            "RICHNESS",             "RIGHT",                "SPEAK_HEADER",         "SPEAK_NUMERAL",        // 120
            "SPEAK_PUNCTUATION",    "SPEAK",                "SPEECH_RATE",          "STRESS",               // 124
            "TABLE_LAYOUT",         "TEXT_ALIGN",           "TEXT_DECORATION",      "TEXT_INDENT",          // 128
            "TEXT_TRANSFORM",       "TOP",                  "UNICODE_BIDI",         "VERTICAL_ALIGN",       // 132
            "VISIBILITY",           "VOICE_FAMILY",         "VOLUME",               "WHITE_SPACE",          // 136
            "WIDOWS",               "WIDTH",                "WORD_SPACING",         "WRITING_MODE",         // 140
            "Z_INDEX"                                                                                       // 144
        };

        // FIXME: Maybe unnecessary, or could be converted to an array
        public const string Auto = "auto";
        public const string CurrentColor = "currentColor";
        public const string Normal = "normal";

        public const string FirstChild = "first-child"; // :59
        public const string Link = "link";
        public const string Visited = "visited";
        public const string Hover = "hover";
        public const string Active = "active";
        public const string Focus = "focus";
        public const string Lang = "lang";
        public const string First = "first";
        public const string Root = "root";
        public const string NthChild = "nth-child";
        public const string NthLastChild = "nth-last-child";
        public const string NthOfType = "nth-of-type";
        public const string NthLastOfType = "nth-last-of-type";
        public const string LastChild = "last-child";
        public const string FirstOfType = "first-of-type";
        public const string LastOfType = "last-of-type";
        public const string OnlyChild = "only-child";
        public const string OnlyOfType = "only-of-type";
        public const string Empty = "empty";
        public const string Target = "target";
        public const string Enabled = "enabled";
        public const string Disabled = "disabled";
        public const string Checked = "checked";
        public const string Not = "not";

        public const string FirstLine = "first-line";
        public const string FirstLetter = "first-letter";
        public const string Before = "before";
        public const string After = "after"; //:87

        public const string Left = "left"; //:167

        public const string Right = "right"; //:210

        public const string Inherit = "inherit"; //:235
        public const string Important = "important";
        public const string None = "none";
        public const string Both = "both";
        public const string Fixed = "fixed";
        public const string Scroll = "scroll";
        public const string Transparent = "transparent";
        public const string NoRepeat = "no-repeat";
        public const string RepeatX = "repeat-x";
        public const string RepeatY = "repeat-y";
        public const string Repeat = "repeat";
        public const string Hidden = "hidden";
        public const string Dotted = "dotted";
        public const string Dashed = "dashed";
        public const string Solid = "solid";
        public const string LibcssDouble = "double";
        public const string Groove = "groove";
        public const string Ridge = "ridge";
        public const string Inset = "inset";
        public const string Outset = "outset"; //:254

        public const string Bold = "bold"; // :297
        public const string Bolder = "bolder";
        public const string Lighter = "lighter";
        public const string Inside = "inside";
        public const string Outside = "outside";
        public const string Disc = "disc";
        public const string Circle = "circle";
        public const string Square = "square";
        public const string Decimal = "decimal";
        public const string DecimalLeadingZero = "decimal-leading-zero";
        public const string LowerRoman = "lower-roman";
        public const string UpperRoman = "upper-roman";
        public const string LowerGreek = "lower-greek";
        public const string LowerLatin = "lower-latin";
        public const string UpperLatin = "upper-latin";
        public const string Armenian = "armenian";
        public const string Georgian = "georgian";
        public const string LowerAlpha = "lower-alpha";
        public const string UpperAlpha = "upper-alpha";
        public const string Binary = "binary";
        public const string Octal = "octal";
        public const string LowerHexadecimal = "lower-hexadecimal";
        public const string UpperHexadecimal = "upper-hexadecimal";
        public const string ArabicIndic = "arabic-indic";
        public const string LowerArmenian = "lower-armenian";
        public const string UpperArmenian = "upper-armenian";
        public const string Bengali = "bengali";
        public const string Cambodian = "cambodian";
        public const string Khmer = "khmer";
        public const string CjkDecimal = "cjk-decimal";
        public const string Devanagari = "devanagari";
        public const string Gujarati = "gujarati";
        public const string Gurmukhi = "gurmukhi";
        public const string Hebrew = "hebrew";
        public const string Kannada = "kannada";
        public const string Lao = "lao";
        public const string Malayalam = "malayalam";
        public const string Mongolian = "mongolian";
        public const string Myanmar = "myanmar";
        public const string Oriya = "oriya";
        public const string Persian = "persian";
        public const string Tamil = "tamil";
        public const string Telugu = "telugu";
        public const string Thai = "thai";
        public const string Tibetan = "tibetan";
        public const string CjkEarthlyBranch = "cjk-earthly-branch";
        public const string CjkHeavenlyStem = "cjk-heavenly-stem";
        public const string Hiragana = "hiragana";
        public const string HiraganaIroha = "hiragana-iroha";
        public const string Katakana = "katakana";
        public const string KatakanaIroha = "katakana-iroha";
        public const string JapaneseInformal = "japanese-informal";
        public const string JapaneseFormal = "japanese-formal";
        public const string KoreanHangulFormal = "korean-hangul-formal";
        public const string KoreanHanjaInformal = "korean-hanja-informal";
        public const string KoreanHanjaFormal = "korean-hanja-formal";
        public const string Invert = "invert";
        public const string Visible = "visible"; // :354

        public const string OpenQuote = "open-quote"; // :407
        public const string CloseQuote = "close-quote";
        public const string NoOpenQuote = "no-open-quote";
        public const string NoCloseQuote = "no-close-quote";
        public const string Attr = "attr";
        public const string Counter = "counter";
        public const string Counters = "counters"; // :413

        public const string Underline = "underline"; // :439
        public const string Overline = "overline";
        public const string LineThrough = "line-through";
        public const string Serif = "serif"; // 430
        public const string SansSerif = "sans-serif";
        public const string Cursive = "cursive";
        public const string Fantasy = "fantasy";
        public const string Monospace = "monospace"; // 434
        public const string Blink = "blink"; // :442

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

    // properties.h:464
    public enum UnitMask : uint
    {
        UNIT_MASK_AZIMUTH               = OpcodeUnit.ANGLE,
        UNIT_MASK_BACKGROUND_ATTACHMENT = 0,
        UNIT_MASK_BACKGROUND_COLOR      = 0,
        UNIT_MASK_BACKGROUND_IMAGE      = 0,
        UNIT_MASK_BACKGROUND_POSITION   = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_BACKGROUND_REPEAT     = 0,
        UNIT_MASK_BORDER_COLLAPSE       = 0,
        UNIT_MASK_BORDER_SPACING        = OpcodeUnit.LENGTH,
        UNIT_MASK_BORDER_SIDE_COLOR     = 0,
        UNIT_MASK_BORDER_SIDE_STYLE     = 0,
        UNIT_MASK_BORDER_SIDE_WIDTH     = OpcodeUnit.LENGTH,
        UNIT_MASK_BOTTOM                = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_CAPTION_SIDE          = 0,
        UNIT_MASK_CLEAR                 = 0,
        UNIT_MASK_CLIP                  = OpcodeUnit.LENGTH,
        UNIT_MASK_COLOR                 = 0,
        UNIT_MASK_CONTENT               = 0,
        UNIT_MASK_COUNTER_INCREMENT     = 0,
        UNIT_MASK_COUNTER_RESET         = 0,
        UNIT_MASK_CUE_AFTER             = 0,
        UNIT_MASK_CUE_BEFORE            = 0,
        UNIT_MASK_CURSOR                = 0,
        UNIT_MASK_DIRECTION             = 0,
        UNIT_MASK_DISPLAY               = 0,
        UNIT_MASK_ELEVATION             = OpcodeUnit.ANGLE,
        UNIT_MASK_EMPTY_CELLS           = 0,
        UNIT_MASK_FLOAT                 = 0,
        UNIT_MASK_FONT_FAMILY           = 0,
        UNIT_MASK_FONT_SIZE             = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_FONT_STYLE            = 0,
        UNIT_MASK_FONT_VARIANT          = 0,
        UNIT_MASK_FONT_WEIGHT           = 0,
        UNIT_MASK_HEIGHT                = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_LEFT                  = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_LETTER_SPACING        = OpcodeUnit.LENGTH,
        UNIT_MASK_LINE_HEIGHT           = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_LIST_STYLE_IMAGE      = 0,
        UNIT_MASK_LIST_STYLE_POSITION   = 0,
        UNIT_MASK_LIST_STYLE_TYPE       = 0,
        UNIT_MASK_MARGIN_SIDE           = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_MAX_HEIGHT            = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_MAX_WIDTH             = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_MIN_HEIGHT            = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_MIN_WIDTH             = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_ORPHANS               = 0,
        UNIT_MASK_OUTLINE_COLOR         = 0,
        UNIT_MASK_OUTLINE_STYLE         = 0,
        UNIT_MASK_OUTLINE_WIDTH         = OpcodeUnit.LENGTH,
        UNIT_MASK_OVERFLOW_X            = 0,
        UNIT_MASK_PADDING_SIDE          = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_PAGE_BREAK_AFTER      = 0,
        UNIT_MASK_PAGE_BREAK_BEFORE     = 0,
        UNIT_MASK_PAGE_BREAK_INSIDE     = 0,
        UNIT_MASK_PAUSE_AFTER           = OpcodeUnit.TIME | OpcodeUnit.PCT,
        UNIT_MASK_PAUSE_BEFORE          = OpcodeUnit.TIME | OpcodeUnit.PCT,
        UNIT_MASK_PITCH_RANGE           = 0,
        UNIT_MASK_PITCH                 = OpcodeUnit.FREQ,
        UNIT_MASK_PLAY_DURING           = 0,
        UNIT_MASK_POSITION              = 0,
        UNIT_MASK_QUOTES                = 0,
        UNIT_MASK_RICHNESS              = 0,
        UNIT_MASK_RIGHT                 = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_SPEAK_HEADER          = 0,
        UNIT_MASK_SPEAK_NUMERAL         = 0,
        UNIT_MASK_SPEAK_PUNCTUATION     = 0,
        UNIT_MASK_SPEAK                 = 0,
        UNIT_MASK_SPEECH_RATE           = 0,
        UNIT_MASK_STRESS                = 0,
        UNIT_MASK_TABLE_LAYOUT          = 0,
        UNIT_MASK_TEXT_ALIGN            = 0,
        UNIT_MASK_TEXT_DECORATION       = 0,
        UNIT_MASK_TEXT_INDENT           = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_TEXT_TRANSFORM        = 0,
        UNIT_MASK_TOP                   = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_UNICODE_BIDI          = 0,
        UNIT_MASK_VERTICAL_ALIGN        = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_VISIBILITY            = 0,
        UNIT_MASK_VOICE_FAMILY          = 0,
        UNIT_MASK_VOLUME                = OpcodeUnit.PCT,
        UNIT_MASK_WHITE_SPACE           = 0,
        UNIT_MASK_WIDOWS                = 0,
        UNIT_MASK_WIDTH                 = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_WORD_SPACING          = OpcodeUnit.LENGTH,
        UNIT_MASK_Z_INDEX               = 0,
        UNIT_MASK_OPACITY               = 0,
        UNIT_MASK_BREAK_AFTER           = 0,
        UNIT_MASK_BREAK_BEFORE          = 0,
        UNIT_MASK_BREAK_INSIDE          = 0,
        UNIT_MASK_COLUMN_COUNT          = 0,
        UNIT_MASK_COLUMN_FILL           = 0,
        UNIT_MASK_COLUMN_GAP            = OpcodeUnit.LENGTH,
        UNIT_MASK_COLUMN_RULE_COLOR     = 0,
        UNIT_MASK_COLUMN_RULE_STYLE     = 0,
        UNIT_MASK_COLUMN_RULE_WIDTH     = OpcodeUnit.LENGTH,
        UNIT_MASK_COLUMN_SPAN           = 0,
        UNIT_MASK_COLUMN_WIDTH          = OpcodeUnit.LENGTH,
        UNIT_MASK_WRITING_MODE          = 0,
        UNIT_MASK_OVERFLOW_Y            = 0,
        UNIT_MASK_BOX_SIZING            = 0,
        UNIT_MASK_ALIGN_CONTENT         = 0,
        UNIT_MASK_ALIGN_ITEMS           = 0,
        UNIT_MASK_ALIGN_SELF            = 0,
        UNIT_MASK_FLEX_BASIS            = OpcodeUnit.LENGTH | OpcodeUnit.PCT,
        UNIT_MASK_FLEX_DIRECTION        = 0,
        UNIT_MASK_FLEX_GROW             = 0,
        UNIT_MASK_FLEX_SHRINK           = 0,
        UNIT_MASK_FLEX_WRAP             = 0,
        UNIT_MASK_JUSTIFY_CONTENT       = 0,
        UNIT_MASK_ORDER                 = 0
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

    // utils.h:22
    public enum BorderSide : int
    {
        BORDER_SIDE_TOP = 0,
        BORDER_SIDE_RIGHT = 1,
        BORDER_SIDE_BOTTOM = 2,
        BORDER_SIDE_LEFT = 3
    };

    delegate CssStatus ParseProperty(List<CssToken> tokens, ref int index, CssStyle style);

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

    public struct CssNamespace
    {
        public string Prefix; // Namespace prefix
        public string Uri; // Namespace URI
    }

    // stylesheet.h:170, struct css_stylesheet
    public class CssStylesheet
    {
        public CssSelectorHash Selectors; // Hashtable of selectors
        uint RuleCount;          // Number of rules in sheet
        public LinkedList<CssRule> RuleList; // List of rules in sheet

        bool Disabled; // Whether this sheet is disabled

        string Title;
        string Url;

        // css_language, language.h:32
        Stack<CssStylesheetContextEntry> Context;
        CssLanguageState LanguageState; // State flag, for at-rule handling
        string DefaultNamespace; // Default namespace URI
        List<CssNamespace> Namespaces; // Array of namespace mappings

        CssParser Parser; // Core parser for sheet
        CssLanguageLevel Level; // Language level of sheet

        bool QuirksAllowed; // Quirks permitted 
        bool QuirksUsed;    // Quirks actually used

        public bool InlineStyle; // Is an inline style

        ParseProperty[] ParseHandlers;

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
            Namespaces = new List<CssNamespace>();

            RuleCount = 0;
            RuleList = new LinkedList<CssRule>();

            QuirksAllowed = false;
            QuirksUsed = false;
            Disabled = false;

            Selectors = new CssSelectorHash();

            // Fill in property parse handlers table (CodeGenerator.GenerateCodeStubs2())
            ParseHandlers = new ParseProperty[CssStrings.Props.Length];
            ParseHandlers[0] = CssPropertyParser.Parse_align_content;
            ParseHandlers[1] = CssPropertyParser.Parse_align_items;
            ParseHandlers[2] = CssPropertyParser.Parse_align_self;
            //ParseHandlers[3] = CssPropertyParser.Parse_azimuth;
            ParseHandlers[4] = Parse_background;
            ParseHandlers[5] = CssPropertyParser.Parse_background_attachment;
            ParseHandlers[6] = CssPropertyParser.Parse_background_color;
            ParseHandlers[7] = CssPropertyParser.Parse_background_image;
            //ParseHandlers[8] = CssPropertyParser.Parse_background_position;
            ParseHandlers[9] = CssPropertyParser.Parse_background_repeat;
            ParseHandlers[10] = Parse_border;
            ParseHandlers[11] = CssPropertyParser.Parse_border_bottom;
            ParseHandlers[12] = CssPropertyParser.Parse_border_bottom_color;
            ParseHandlers[13] = CssPropertyParser.Parse_border_bottom_style;
            ParseHandlers[14] = CssPropertyParser.Parse_border_bottom_width;
            ParseHandlers[15] = CssPropertyParser.Parse_border_collapse;
            ParseHandlers[16] = Parse_border_color;
            ParseHandlers[17] = CssPropertyParser.Parse_border_left;
            ParseHandlers[18] = CssPropertyParser.Parse_border_left_color;
            ParseHandlers[19] = CssPropertyParser.Parse_border_left_style;
            ParseHandlers[20] = CssPropertyParser.Parse_border_left_width;
            ParseHandlers[21] = CssPropertyParser.Parse_border_right;
            ParseHandlers[22] = CssPropertyParser.Parse_border_right_color;
            ParseHandlers[23] = CssPropertyParser.Parse_border_right_style;
            ParseHandlers[24] = CssPropertyParser.Parse_border_right_width;
            ParseHandlers[25] = Parse_border_spacing;
            ParseHandlers[26] = Parse_border_style;
            ParseHandlers[27] = CssPropertyParser.Parse_border_top;
            ParseHandlers[28] = CssPropertyParser.Parse_border_top_color;
            ParseHandlers[29] = CssPropertyParser.Parse_border_top_style;
            ParseHandlers[30] = CssPropertyParser.Parse_border_top_width;
            //ParseHandlers[31] = CssPropertyParser.Parse_border_width;
            ParseHandlers[32] = CssPropertyParser.Parse_bottom;
            ParseHandlers[33] = CssPropertyParser.Parse_box_sizing;
            ParseHandlers[34] = CssPropertyParser.Parse_break_after;
            ParseHandlers[35] = CssPropertyParser.Parse_break_before;
            ParseHandlers[36] = CssPropertyParser.Parse_break_inside;
            ParseHandlers[37] = CssPropertyParser.Parse_caption_side;
            ParseHandlers[38] = CssPropertyParser.Parse_clear;
            //ParseHandlers[39] = CssPropertyParser.Parse_clip;
            ParseHandlers[40] = CssPropertyParser.Parse_color;
            //ParseHandlers[41] = CssPropertyParser.Parse_columns;
            ParseHandlers[42] = CssPropertyParser.Parse_column_count;
            ParseHandlers[43] = CssPropertyParser.Parse_column_fill;
            ParseHandlers[44] = CssPropertyParser.Parse_column_gap;
            //ParseHandlers[45] = CssPropertyParser.Parse_column_rule;
            ParseHandlers[46] = CssPropertyParser.Parse_column_rule_color;
            ParseHandlers[47] = CssPropertyParser.Parse_column_rule_style;
            ParseHandlers[48] = CssPropertyParser.Parse_column_rule_width;
            ParseHandlers[49] = CssPropertyParser.Parse_column_span;
            ParseHandlers[50] = CssPropertyParser.Parse_column_width;
            ParseHandlers[51] = Parse_content;
            ParseHandlers[52] = CssPropertyParser.Parse_counter_increment;
            ParseHandlers[53] = CssPropertyParser.Parse_counter_reset;
            //ParseHandlers[54] = CssPropertyParser.Parse_cue;
            ParseHandlers[55] = CssPropertyParser.Parse_cue_after;
            ParseHandlers[56] = CssPropertyParser.Parse_cue_before;
            //ParseHandlers[57] = CssPropertyParser.Parse_cursor;
            ParseHandlers[58] = CssPropertyParser.Parse_direction;
            ParseHandlers[59] = CssPropertyParser.Parse_display;
            //ParseHandlers[60] = CssPropertyParser.Parse_elevation;
            ParseHandlers[61] = CssPropertyParser.Parse_empty_cells;
            //ParseHandlers[62] = CssPropertyParser.Parse_flex;
            ParseHandlers[63] = CssPropertyParser.Parse_flex_basis;
            ParseHandlers[64] = CssPropertyParser.Parse_flex_direction;
            //ParseHandlers[65] = CssPropertyParser.Parse_flex_flow;
            ParseHandlers[66] = CssPropertyParser.Parse_flex_grow;
            ParseHandlers[67] = CssPropertyParser.Parse_flex_shrink;
            ParseHandlers[68] = CssPropertyParser.Parse_flex_wrap;
            ParseHandlers[69] = CssPropertyParser.Parse_float;
            //ParseHandlers[70] = CssPropertyParser.Parse_font;
            ParseHandlers[71] = Parse_font_family;
            ParseHandlers[72] = CssPropertyParser.Parse_font_size;
            ParseHandlers[73] = CssPropertyParser.Parse_font_style;
            ParseHandlers[74] = CssPropertyParser.Parse_font_variant;
            ParseHandlers[75] = Parse_font_weight;
            ParseHandlers[76] = CssPropertyParser.Parse_height;
            ParseHandlers[77] = CssPropertyParser.Parse_justify_content;
            ParseHandlers[78] = CssPropertyParser.Parse_left;
            ParseHandlers[79] = CssPropertyParser.Parse_letter_spacing;
            ParseHandlers[80] = CssPropertyParser.Parse_line_height;
            //ParseHandlers[81] = CssPropertyParser.Parse_list_style;
            ParseHandlers[82] = CssPropertyParser.Parse_list_style_image;
            ParseHandlers[83] = CssPropertyParser.Parse_list_style_position;
            ParseHandlers[84] = Parse_list_style_type;
            ParseHandlers[85] = Parse_margin;
            ParseHandlers[86] = CssPropertyParser.Parse_margin_bottom;
            ParseHandlers[87] = CssPropertyParser.Parse_margin_left;
            ParseHandlers[88] = CssPropertyParser.Parse_margin_right;
            ParseHandlers[89] = CssPropertyParser.Parse_margin_top;
            ParseHandlers[90] = CssPropertyParser.Parse_max_height;
            ParseHandlers[91] = CssPropertyParser.Parse_max_width;
            ParseHandlers[92] = CssPropertyParser.Parse_min_height;
            ParseHandlers[93] = CssPropertyParser.Parse_min_width;
            //ParseHandlers[94] = CssPropertyParser.Parse_opacity;
            ParseHandlers[95] = CssPropertyParser.Parse_order;
            ParseHandlers[96] = CssPropertyParser.Parse_orphans;
            //ParseHandlers[97] = CssPropertyParser.Parse_outline;
            ParseHandlers[98] = CssPropertyParser.Parse_outline_color;
            ParseHandlers[99] = CssPropertyParser.Parse_outline_style;
            ParseHandlers[100] = CssPropertyParser.Parse_outline_width;
            ParseHandlers[101] = Parse_overflow;
            ParseHandlers[102] = CssPropertyParser.Parse_overflow_x;
            ParseHandlers[103] = CssPropertyParser.Parse_overflow_y;
            ParseHandlers[104] = Parse_padding;
            ParseHandlers[105] = CssPropertyParser.Parse_padding_bottom;
            ParseHandlers[106] = CssPropertyParser.Parse_padding_left;
            ParseHandlers[107] = CssPropertyParser.Parse_padding_right;
            ParseHandlers[108] = CssPropertyParser.Parse_padding_top;
            ParseHandlers[109] = CssPropertyParser.Parse_page_break_after;
            ParseHandlers[110] = CssPropertyParser.Parse_page_break_before;
            ParseHandlers[111] = CssPropertyParser.Parse_page_break_inside;
            //ParseHandlers[112] = CssPropertyParser.Parse_pause;
            ParseHandlers[113] = CssPropertyParser.Parse_pause_after;
            ParseHandlers[114] = CssPropertyParser.Parse_pause_before;
            ParseHandlers[115] = CssPropertyParser.Parse_pitch_range;
            ParseHandlers[116] = CssPropertyParser.Parse_pitch;
            //ParseHandlers[117] = CssPropertyParser.Parse_play_during;
            ParseHandlers[118] = CssPropertyParser.Parse_position;
            //ParseHandlers[119] = CssPropertyParser.Parse_quotes;
            ParseHandlers[120] = CssPropertyParser.Parse_richness;
            ParseHandlers[121] = CssPropertyParser.Parse_right;
            ParseHandlers[122] = CssPropertyParser.Parse_speak_header;
            ParseHandlers[123] = CssPropertyParser.Parse_speak_numeral;
            //ParseHandlers[124] = css__parse_speak_punctuation;
            //ParseHandlers[125] = css__parse_speak;
            //ParseHandlers[126] = css__parse_speech_rate;
            ParseHandlers[127] = CssPropertyParser.Parse_stress;
            ParseHandlers[128] = CssPropertyParser.Parse_table_layout;
            ParseHandlers[129] = CssPropertyParser.Parse_text_align;
            ParseHandlers[130] = Parse_text_decoration;
            ParseHandlers[131] = CssPropertyParser.Parse_text_indent;
            ParseHandlers[132] = CssPropertyParser.Parse_text_transform;
            ParseHandlers[133] = CssPropertyParser.Parse_top;
            ParseHandlers[134] = CssPropertyParser.Parse_unicode_bidi;
            ParseHandlers[135] = CssPropertyParser.Parse_vertical_align;
            ParseHandlers[136] = CssPropertyParser.Parse_visibility;
            //ParseHandlers[137] = css__parse_voice_family;
            ParseHandlers[138] = CssPropertyParser.Parse_volume;
            ParseHandlers[139] = CssPropertyParser.Parse_white_space;
            ParseHandlers[140] = CssPropertyParser.Parse_widows;
            ParseHandlers[141] = CssPropertyParser.Parse_width;
            ParseHandlers[142] = CssPropertyParser.Parse_word_spacing;
            ParseHandlers[143] = CssPropertyParser.Parse_writing_mode;
            ParseHandlers[144] = CssPropertyParser.Parse_z_index;

        }

        // stylesheet.c:311
        public void AppendData(string data)
        {
            Parser.ParseChunk(data);
        }

        // css__stylesheet_string_add()
        // stylesheet.c:39
        public void AddString(string str, out uint stringNumber)
        {
            stringNumber = 0;
            Log.Unimplemented();
        }

        // libcss/src/stylesheet.c:1534
        void AddSelectors(CssRule rule)
        {
            // Rule must not be in sheet
            Debug.Assert(rule.ParentSheet == null);

            switch (rule.Type)
            {
                case CssRuleType.CSS_RULE_SELECTOR:
                    {
                        foreach (var sel in rule.Selectors)
                        {
                            Selectors.Insert(sel);
                        }
                    }
                    break;
                case CssRuleType.CSS_RULE_MEDIA:
                    {
                        Log.Unimplemented();
                        /*
                        css_rule_media* m = (css_rule_media*)rule;
                        css_rule* r;

                        for (r = m->first_child; r != NULL; r = r->next)
                        {
                            AddSelectors(r); // error = _add_selectors(sheet, r);
                        }*/
                    }
                    break;
            }
        }

        // libcss/src/stylesheet.c:1414
        void AddRule(CssRule rule, CssRule parent)
        {
            /* Need to fill in rule's index field before adding selectors
             * because selector chains consider the rule index for sort order
             */
            rule.Index = RuleCount;

            // Add any selectors to the hash
            AddSelectors(rule);

            // Add to the sheet's size
            //sheet->size += _rule_size(rule);

            if (parent != null)
            {
                Log.Unimplemented("parent rule handling");
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
                    Log.Unimplemented($"event handle type {type.ToString()} requested!");
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
                Log.Unimplemented("rule.Type == CSS_RULE_FONT_FACE");
            }
            else
            {
                //error = parseProperty(c, ident, vector, &ctx, rule);
                ParseProperty(ident, tokens, ref index, rule);
            }
        }

        // Look up a namespace prefix
        // TODO: This function belongs to some other place
        // language.c:920
        void LookupNamespace(string prefix, out string uri)
        {
            foreach (var entry in Namespaces)
            {
                if (entry.Prefix == prefix)
                {
                    uri = entry.Uri;
                    break;
                }
            }

            uri = "";
        }

        // language.c:949
        void ParseClass(List<CssToken> tokens, ref int index, out CssSelectorDetail specific)
        {
            CssQname qname = new CssQname();
            CssSelectorDetailValue detail_value = new CssSelectorDetailValue();

            /* class     -> '.' IDENT */
            //token = parserutils_vector_iterate(vector, ctx);
            if (index >= tokens.Count - 1)
            {
                Log.Unimplemented(); // To check that condition, it's maybe off by one
                specific = new CssSelectorDetail();
                return; // CSS_INVALID
            }

            var token = tokens[index++];

            if (token.IsChar('.') == false)
            {
                specific = new CssSelectorDetail();
                return;// CSS_IVALID;
            }

            //token = parserutils_vector_iterate(vector, ctx);
            if (index >= tokens.Count)
            {
                specific = new CssSelectorDetail();
                return; // CSS_INVALID
            }

            token = tokens[index++];

            if (token.Type != CssTokenType.CSS_TOKEN_IDENT)
            {
                specific = new CssSelectorDetail();
                return;// CSS_INVALID;
            }

            detail_value.Str = null;

            qname.Namespace = null;
            qname.Name = token.iData;

            specific = new CssSelectorDetail(
                CssSelectorType.CSS_SELECTOR_CLASS,
                ref qname, ref detail_value, CssSelectorDetailValueType.CSS_SELECTOR_DETAIL_VALUE_STRING, false);
        }

        // language.c:980
        CssStatus ParseAttrib(List<CssToken> tokens, ref int index, out CssSelectorDetail specific)
        {
            CssQname qname = new CssQname();
            CssSelectorDetailValue detail_value = new CssSelectorDetailValue();

            //const css_token* token, *value = NULL;
            CssSelectorType type = CssSelectorType.CSS_SELECTOR_ATTRIBUTE;
            //css_error error;
            specific = new CssSelectorDetail(); // just a placeholder
            string prefix = null;

            /* attrib    -> '[' ws namespace_prefix? IDENT ws [
             *		       [ '=' |
             *		         INCLUDES |
             *		         DASHMATCH |
             *		         PREFIXMATCH |
             *		         SUFFIXMATCH |
             *		         SUBSTRINGMATCH
             *		       ] ws
             *		       [ IDENT | STRING ] ws ]? ']'
             * namespace_prefix -> [ IDENT | '*' ]? '|'
             */
            if (index >= tokens.Count)
                return CssStatus.CSS_INVALID;

            var token = tokens[index++];

            if (token.IsChar('[') == false)
                return CssStatus.CSS_INVALID;


            ConsumeWhitespace(tokens, ref index);

            // again iterate tokens vector
            if (index >= tokens.Count)
                return CssStatus.CSS_INVALID;

            token = tokens[index++];

            if ((token.Type != CssTokenType.CSS_TOKEN_IDENT &&
                    token.IsChar('*') == false &&
                    token.IsChar('|') == false))
            {
                return CssStatus.CSS_INVALID;
            }

            if (token.IsChar('|'))
            {
                if (index >= tokens.Count)
                    return CssStatus.CSS_INVALID;

                token = tokens[index++];
            }
            else
            {
                var temp = tokens[index];
                if (/*temp != NULL &&*/ temp.IsChar('|'))
                {
                    prefix = token.iData;

                    index++;

                    if (index >= tokens.Count)
                    {
                        specific = new CssSelectorDetail();
                        return CssStatus.CSS_INVALID;
                    }
                    token = tokens[index++];
                }
            }

            if (token.Type != CssTokenType.CSS_TOKEN_IDENT)
            {
                specific = new CssSelectorDetail();
                return CssStatus.CSS_INVALID;
            }

            LookupNamespace(prefix, out qname.Namespace);

            qname.Name = token.iData;

            ConsumeWhitespace(tokens, ref index);

            if (index >= tokens.Count)
                return CssStatus.CSS_INVALID;
            token = tokens[index++];

            if (token.IsChar(']') == false)
            {
                if (token.IsChar('='))
                    type = CssSelectorType.CSS_SELECTOR_ATTRIBUTE_EQUAL;
                else if (token.Type == CssTokenType.CSS_TOKEN_INCLUDES)
                    type = CssSelectorType.CSS_SELECTOR_ATTRIBUTE_INCLUDES;
                else if (token.Type == CssTokenType.CSS_TOKEN_DASHMATCH)
                    type = CssSelectorType.CSS_SELECTOR_ATTRIBUTE_DASHMATCH;
                else if (token.Type == CssTokenType.CSS_TOKEN_PREFIXMATCH)
                    type = CssSelectorType.CSS_SELECTOR_ATTRIBUTE_PREFIX;
                else if (token.Type == CssTokenType.CSS_TOKEN_SUFFIXMATCH)
                    type = CssSelectorType.CSS_SELECTOR_ATTRIBUTE_SUFFIX;
                else if (token.Type == CssTokenType.CSS_TOKEN_SUBSTRINGMATCH)
                    type = CssSelectorType.CSS_SELECTOR_ATTRIBUTE_SUBSTRING;
                else
                    return CssStatus.CSS_INVALID;

                ConsumeWhitespace(tokens, ref index);

                if (index >= tokens.Count)
                    return CssStatus.CSS_INVALID;
                token = tokens[index++];

                if (token.Type != CssTokenType.CSS_TOKEN_IDENT &&
                    token.Type != CssTokenType.CSS_TOKEN_STRING)
                {
                    return CssStatus.CSS_INVALID;
                }

                var value = token;

                ConsumeWhitespace(tokens, ref index);

                if (index >= tokens.Count)
                {
                    specific = new CssSelectorDetail();
                    return CssStatus.CSS_INVALID;
                }
                token = tokens[index++];
                if (token.IsChar(']') == false)
                    return CssStatus.CSS_INVALID;

                detail_value.Str = value.iData;
            }

            //detail_value.Str = value != NULL ? value->idata : NULL; // moved it upwards

            specific = new CssSelectorDetail(type, ref qname, ref detail_value,
                CssSelectorDetailValueType.CSS_SELECTOR_DETAIL_VALUE_STRING, false);

            return CssStatus.CSS_OK;
        }

        // language.c: 1288
        static Dictionary<string, CssSelectorType> PseudoLut = new Dictionary<string, CssSelectorType>()
        {
            { CssStrings.FirstChild, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Link, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Visited, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Hover, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Active, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Focus, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Lang, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Left, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Right, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.First, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Root, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.NthChild, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.NthLastChild, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.NthOfType, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.NthLastOfType, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.LastChild, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.FirstOfType, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.LastOfType, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.OnlyChild, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.OnlyOfType, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Empty, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Target, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Enabled, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Disabled, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Checked, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },
            { CssStrings.Not, CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS },

            { CssStrings.FirstLine, CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT },
            { CssStrings.FirstLetter, CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT },
            { CssStrings.Before, CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT },
            { CssStrings.After, CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT }
        };

        // language.c:1283
        CssStatus ParsePseudo(List<CssToken> tokens, ref int index, bool inNot, out CssSelectorDetail specific)
        {
            CssQname qname = new CssQname();
            CssSelectorDetailValue detail_value = new CssSelectorDetailValue();
            CssSelectorDetailValueType value_type = CssSelectorDetailValueType.CSS_SELECTOR_DETAIL_VALUE_STRING;
            CssSelectorType type = CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS;
            bool require_element = false, negate = false;
            specific = new CssSelectorDetail(); // just a placeholder

            /* pseudo    -> ':' ':'? [ IDENT | FUNCTION ws any1 ws ')' ] */
            if (index >= tokens.Count)
                return CssStatus.CSS_INVALID;

            var token = tokens[index++];

            if (token.IsChar(':') == false)
                return CssStatus.CSS_INVALID;


            // Optional second colon before pseudo element names
            if (index >= tokens.Count)
                return CssStatus.CSS_INVALID;
            token = tokens[index++];

            if (token.IsChar(':'))
            {
                // If present, we require a pseudo element
                require_element = true;

                // Consume subsequent token
                if (index >= tokens.Count)
                    return CssStatus.CSS_INVALID;
                token = tokens[index++];
            }

            // Expect IDENT or FUNCTION
            if ((token.Type != CssTokenType.CSS_TOKEN_IDENT &&
                token.Type != CssTokenType.CSS_TOKEN_FUNCTION))
            {
                return CssStatus.CSS_INVALID;
            }

            qname.Namespace = null;
            qname.Name = token.iData;

            // Search lut for selector type
            if (!PseudoLut.ContainsKey(qname.Name))
                return CssStatus.CSS_INVALID; // Not found: invalid

            type = PseudoLut[qname.Name];

            // Required a pseudo element, but didn't find one: invalid
            if (require_element && type != CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT)
                return CssStatus.CSS_INVALID;
            // :not() and pseudo elements are not permitted in :not()
            if (inNot && (type == CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT ||
                    /*pseudo_lut[lut_idx].index == NOT*/qname.Name == CssStrings.Not))
                return CssStatus.CSS_INVALID;

            if (token.Type == CssTokenType.CSS_TOKEN_FUNCTION)
            {
                Log.Unimplemented("ParsePseudoList 1180");
                /*
                int fun_type = pseudo_lut[lut_idx].index;

                consumeWhitespace(vector, ctx);

                if (fun_type == LANG)
                {
                    // IDENT
                    token = parserutils_vector_iterate(vector, ctx);
                    if (token == NULL || token->type != CSS_TOKEN_IDENT)
                        return CSS_INVALID;

                    detail_value.string = token->idata;
                    value_type = CSS_SELECTOR_DETAIL_VALUE_STRING;

                    consumeWhitespace(vector, ctx);
                }
                else if (fun_type == NTH_CHILD ||
                        fun_type == NTH_LAST_CHILD ||
                        fun_type == NTH_OF_TYPE ||
                        fun_type == NTH_LAST_OF_TYPE)
                {
                    // an + b
                    error = parseNth(c, vector, ctx, &detail_value);
                    if (error != CSS_OK)
                        return error;

                    value_type = CSS_SELECTOR_DETAIL_VALUE_NTH;
                }
                else if (fun_type == NOT)
                {
                    // type_selector | specific
                    token = parserutils_vector_peek(vector, *ctx);
                    if (token == NULL)
                        return CSS_INVALID;

                    if (token->type == CSS_TOKEN_IDENT ||
                            tokenIsChar(token, '*') ||
                            tokenIsChar(token, '|'))
                    {
                        // Have type selector
                        error = parseTypeSelector(c, vector, ctx,
                                &qname);
                        if (error != CSS_OK)
                            return error;

                        type = CSS_SELECTOR_ELEMENT;

                        // Ensure lwc insensitive string is available
                        // for element names
                        if (qname.name->insensitive == NULL &&
                                lwc__intern_caseless_string(
                                qname.name) != lwc_error_ok)
                            return CSS_NOMEM;

                        detail_value.string = NULL;
                        value_type = CSS_SELECTOR_DETAIL_VALUE_STRING;
                    }
                    else
                    {
                        // specific
                        css_selector_detail det;

                        error = parseSpecific(c, vector, ctx, true,
                                &det);
                        if (error != CSS_OK)
                            return error;

                        qname = det.qname;
                        type = det.type;
                        detail_value = det.value;
                        value_type = det.value_type;
                    }

                    negate = true;

                    consumeWhitespace(vector, ctx);
                }

                token = parserutils_vector_iterate(vector, ctx);
                if (token == NULL || tokenIsChar(token, ')') == false)
                    return CSS_INVALID;
                */
            }

            specific = new CssSelectorDetail(type, ref qname, ref detail_value,
                value_type, negate);

            return CssStatus.CSS_OK;
        }

        // language.c:1461
        void ParseSpecific(List<CssToken> tokens, ref int index, bool inNot, out CssSelectorDetail specific)
        {
            // specific  -> [ HASH | class | attrib | pseudo ]
            var token = tokens[index];

            if (token.Type == CssTokenType.CSS_TOKEN_HASH)
            {
                CssQname qname = new CssQname();
                CssSelectorDetailValue detailValue = new CssSelectorDetailValue();

                detailValue.Str = null;

                qname.Namespace = null;
                qname.Name = token.iData;

                specific = new CssSelectorDetail(
                    CssSelectorType.CSS_SELECTOR_ID,
                    ref qname,
                    ref detailValue,
                    CssSelectorDetailValueType.CSS_SELECTOR_DETAIL_VALUE_STRING,
                    false);

                //parserutils_vector_iterate(vector, ctx);
                if (index < tokens.Count)
                    index++;
            }
            else if (token.IsChar('.'))
            {
                ParseClass(tokens, ref index, out specific);
            }
            else if (token.IsChar('['))
            {
                ParseAttrib(tokens, ref index, out specific);
            }
            else if (token.IsChar(':'))
            {
                ParsePseudo(tokens, ref index, inNot, out specific);
            }
            else
            {
                Console.WriteLine("CSS_INVALID");
                specific = new CssSelectorDetail();
                return;
            }
        }

        // language.c:1516
        void ParseAppendSpecific(List<CssToken> tokens, ref int index, CssSelector parent)
        {
            CssSelectorDetail specific;
            ParseSpecific(tokens, ref index, false, out specific);

            parent.AppendSpecific(specific);
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

                // Ensure we have at least one specific selector
                ParseAppendSpecific(tokens, ref index, selector);
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
                    (index >= tokens.Count || tokens[index].IsChar(',')))
                {
                    continue;
                }

                var other = ParseSimpleSelector(tokens, ref index);
                result = other;

                var error = other.Combine(comb, selector);
                if (error != CssStatus.CSS_OK)
                {
                    Log.Unimplemented("ERROR 1316");
                    return null;
                }

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
            while (index < tokens.Count)
            {
                token = tokens[index++];
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

        // utils/utils.c:26
        public static Fixed NumberFromString(string data, bool intOnly, out int consumed)
        {
            //const uint8_t* ptr = data;
            int k = 0;
            int sign = 1;
            int intpart = 0;
            int fracpart = 0;
            int pwr = 1;
            int len = data.Length;

            // number = [+-]? ([0-9]+ | [0-9]* '.' [0-9]+)

            // Extract sign, if any
            if (data[k + 0] == '-')
            {
                sign = -1;
                len--;
                k++;
            }
            else if (data[k + 0] == '+')
            {
                len--;
                k++;
            }

            // Ensure we have either a digit or a '.' followed by a digit
            if (len == 0)
            {
                consumed = 0;
                return new Fixed(0);
            }
            else
            {
                if (data[k + 0] == '.')
                {
                    if (len == 1 || data[k + 1] < '0' || '9' < data[k + 1])
                    {
                        consumed = 0;
                        return new Fixed(0);
                    }
                }
                else if (data[k + 0] < '0' || '9' < data[k + 0])
                {
                    consumed = 0;
                    return new Fixed(0);
                }
            }

            // Now extract intpart, assuming base 10
            while (len > 0)
            {
                // Stop on first non-digit
                if (data[k + 0] < '0' || '9' < data[k + 0])
                    break;

                // Prevent overflow of 'intpart'; proper clamping below
                if (intpart < (1 << 22))
                {
                    intpart *= 10;
                    intpart += data[k + 0] - '0';
                }
                k++;
                len--;
            }

            // And fracpart, again, assuming base 10
            if (intOnly == false && len > 1 && data[k + 0] == '.' &&
                    ('0' <= data[k + 1] && data[k + 1] <= '9'))
            {
                k++;
                len--;

                while (len > 0)
                {
                    if (data[k + 0] < '0' || '9' < data[k + 0])
                        break;

                    if (pwr < 1000000)
                    {
                        pwr *= 10;
                        fracpart *= 10;
                        fracpart += data[k + 0] - '0';
                    }
                    k++;
                    len--;
                }
                fracpart = ((1 << 10) * fracpart + pwr / 2) / pwr;
                if (fracpart >= (1 << 10))
                {
                    intpart++;
                    fracpart &= (1 << 10) - 1;
                }
            }

            consumed = k;

            if (sign > 0)
            {
                // If the result is larger than we can represent,
                // then clamp to the maximum value we can store.
                if (intpart >= (1 << 21))
                {
                    intpart = (1 << 21) - 1;
                    fracpart = (1 << 10) - 1;
                }
            }
            else
            {
                // If the negated result is smaller than we can represent
                // then clamp to the minimum value we can store.
                if (intpart >= (1 << 21))
                {
                    intpart = -(1 << 21);
                    fracpart = 0;
                }
                else
                {
                    intpart = -intpart;
                    if (fracpart != 0)
                    {
                        fracpart = (1 << 10) - fracpart;
                        intpart--;
                    }
                }
            }

            return new Fixed((int)(((uint)intpart << 10) | fracpart), true);
        }

        // utils.c:682
        static CssStatus ParseProperty_NamedColour(string data, ref uint result)
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

            Log.Unimplemented();
            // We don't know this colour name; ask the client
            //if (c->sheet->color != NULL)
            //  return c->sheet->color(c->sheet->color_pw, data, result);

            // Invalid color name
            return CssStatus.CSS_INVALID;
        }

        // parse/properties/utils.c:131
        public static CssStatus Parse_border_side(List<CssToken> tokens, ref int index, CssStyle result, BorderSide side)
        {
            int origIndex = index;
            int prevIndex;
            CssStatus error = CssStatus.CSS_OK;
            bool color = true;
            bool style = true;
            bool width = true;
            CssStyle color_style;
            CssStyle style_style;
            CssStyle width_style;

            // Firstly, handle inherit
            if (index >= tokens.Count)
                return CssStatus.CSS_INVALID;

            var token = tokens[index];

            if (token.IsCssInherit())
            {
                result.AppendStyle(
                    new OpCode(
                        (ushort)((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_COLOR + (byte)side),
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                result.AppendStyle(
                    new OpCode(
                        (ushort)((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_STYLE + (byte)side),
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                result.AppendStyle(
                    new OpCode(
                        (ushort)((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_WIDTH + (byte)side),
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );


                if (index < tokens.Count) index++;

                return CssStatus.CSS_OK;
            }

            // allocate styles
            color_style = new CssStyle();
            style_style = new CssStyle();
            width_style = new CssStyle();

            // Attempt to parse the various longhand properties
            do
            {
                prevIndex = index;
                error = CssStatus.CSS_OK;

                // Ensure that we're not about to parse another inherit
                if (index >= tokens.Count)
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }
                token = tokens[index];
                if (token.IsCssInherit())
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }

                // Try each property parser in turn, but only if we
                // haven't already got a value for this property.
                if ((color) &&
                    (error = CssPropertyParser.Parse_border_side_color(tokens, ref index, color_style,
                        (CssPropertiesEnum)(((int)CssPropertiesEnum.CSS_PROP_BORDER_TOP_COLOR) + side))) == CssStatus.CSS_OK)
                {
                    color = false;
                }
                else if ((style) &&
                       (error = CssPropertyParser.Parse_border_side_style(tokens, ref index, style_style,
                        (CssPropertiesEnum)(((int)CssPropertiesEnum.CSS_PROP_BORDER_TOP_STYLE) + side))) == CssStatus.CSS_OK)
                {
                    style = false;
                }
                else if ((width) &&
                       (error = CssPropertyParser.Parse_border_side_width(tokens, ref index, width_style,
                        (CssPropertiesEnum)(((int)CssPropertiesEnum.CSS_PROP_BORDER_TOP_WIDTH) + side))) == CssStatus.CSS_OK)
                {
                    width = false;
                }

                if (error == CssStatus.CSS_OK)
                {
                    ConsumeWhitespace(tokens, ref index);

                    //token = parserutils_vector_peek(vector, *ctx);
                    if (index >= tokens.Count) break;
                    token = tokens[index];
                }
                else
                {
                    // Forcibly cause loop to exit
                    //token = NULL;
                    break;
                }
            } while (index != prevIndex);

            // No failure beyond this point possible

            if (style)
            {
                style_style.AppendStyle(
                    new OpCode(
                        (ushort)((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_STYLE + (byte)side),
                        0,
                        (ushort)OpCodeValues.BORDER_STYLE_NONE)
                );
            }

            if (width)
            {
                width_style.AppendStyle(
                    new OpCode(
                        (ushort)((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_WIDTH + (byte)side),
                        0,
                        (ushort)OpCodeValues.BORDER_WIDTH_MEDIUM)
                );
            }

            if (color)
            {
                width_style.AppendStyle(
                    new OpCode(
                        (ushort)((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_COLOR + (byte)side),
                        0,
                        (ushort)OpCodeValues.BORDER_COLOR_CURRENT_COLOR)
                );
            }

            result.MergeStyle(color_style);
            result.MergeStyle(style_style);
            result.MergeStyle(width_style);

            return CssStatus.CSS_OK;
        }

        // parse/properties/utils.c:368
        public static CssStatus ParseProperty_ColourSpecifier(List<CssToken> tokens, ref int index, out ushort value, out uint result)
        {
            bool quirksAllowed = false, quirksUsed = false; // FIXME: quirks are hardcoded to be disallowed

            result = 0;
            value = 0;
            var origIndex = index;
            CssStatus error;

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
            if (index >= tokens.Count) return CssStatus.CSS_INVALID;

            var token = tokens[index++];
            if ((token.Type != CssTokenType.CSS_TOKEN_IDENT &&
                 token.Type != CssTokenType.CSS_TOKEN_HASH &&
                 token.Type != CssTokenType.CSS_TOKEN_FUNCTION))
            {

                if (!quirksAllowed ||
                    (token.Type != CssTokenType.CSS_TOKEN_NUMBER &&
                     token.Type != CssTokenType.CSS_TOKEN_DIMENSION))
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }
            }

            if (token.Type == CssTokenType.CSS_TOKEN_IDENT)
            {
                if (token.iData.Equals(CssStrings.Transparent, StringComparison.OrdinalIgnoreCase))
                {
                    value = (ushort)OpColor.COLOR_TRANSPARENT;
                    result = 0; // black transparent
                    return CssStatus.CSS_OK;
                }
                else if (token.iData.Equals(CssStrings.CurrentColor, StringComparison.OrdinalIgnoreCase))
                {
                    value = (ushort)OpColor.COLOR_CURRENT_COLOR;
                    result = 0;
                    return CssStatus.CSS_OK;
                }

                error = ParseProperty_NamedColour(token.iData, ref result);
                if (error != CssStatus.CSS_OK && quirksAllowed)
                {
                    error = error = ParseHashColour(token.iData, out result);
                    if (error == CssStatus.CSS_OK)
                        quirksUsed = true;
                }

                if (error != CssStatus.CSS_OK)
                {
                    index = origIndex;
                    return error;
                }
            }
            else if (token.Type == CssTokenType.CSS_TOKEN_HASH)
            {
                error = ParseHashColour(token.iData, out result);
                if (error != CssStatus.CSS_OK)
                {
                    index = origIndex;
                    return error;
                }
            }
            else if (quirksAllowed &&
                  token.Type == CssTokenType.CSS_TOKEN_NUMBER)
            {
                error = ParseHashColour(token.iData, out result);
                if (error == CssStatus.CSS_OK)
                {
                    quirksUsed = true;
                }
                else
                {
                    index = origIndex;
                    return error;
                }
            }
            else if (quirksAllowed &&
                  token.Type == CssTokenType.CSS_TOKEN_DIMENSION)
            {
                error = ParseHashColour(token.iData, out result);
                if (error == CssStatus.CSS_OK)
                {
                    quirksUsed = true;
                }
                else
                {
                    index = origIndex;
                    return error;
                }
            }
            else if (token.Type == CssTokenType.CSS_TOKEN_FUNCTION)
            {
                Log.Unimplemented();
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
            return CssStatus.CSS_OK;
        }

        /**
         * Parse a hash colour (#rgb or #rrggbb)
         *
         * \param data    Pointer to colour string
         * \param result  Pointer to location to receive result (AARRGGBB)
         */
        // utils.c:869
        static CssStatus ParseHashColour(string data, out uint result)
        {
            byte r = 0, g = 0, b = 0, a = 0xff;
            int len = data.Length;

            if ((len == 3) && data[0].IsHex() && data[1].IsHex() &&
                    data[2].IsHex())
            {
                r = data[0].ToByte();
                g = data[1].ToByte();
                b = data[2].ToByte();

                r |= (byte)(r << 4);
                g |= (byte)(g << 4);
                b |= (byte)(b << 4);
            }
            else if (len == 6 && data[0].IsHex() && data[1].IsHex() &&
                    data[2].IsHex() && data[3].IsHex() &&
                    data[4].IsHex() && data[5].IsHex())
            {
                r = (byte)(data[0].ToByte() << 4);
                r |= data[1].ToByte();
                g = (byte)(data[2].ToByte() << 4);
                g |= data[3].ToByte();
                b = (byte)(data[4].ToByte() << 4);
                b |= data[5].ToByte();
            }
            else
            {
                result = 0;
                return CssStatus.CSS_INVALID;
            }

            result = ((uint)a << 24) | ((uint)r << 16) | ((uint)g << 8) | b;

            return CssStatus.CSS_OK;
        }

        // parse/properties/utils.c:919
        public static CssStatus ParseProperty_UnitSpecifier(List<CssToken> tokens, ref int index, OpcodeUnit defaultUnit, out Fixed length, out uint unit)
        {
            // FIXME: Hack for now
            bool quirksAllowed = false;
            bool quirksUsed = false;

            int origIndex = index;

            length = new Fixed(0);
            unit = 0;

            ConsumeWhitespace(tokens, ref index);

            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var token = tokens[index++];

            if (token.Type != CssTokenType.CSS_TOKEN_DIMENSION &&
                token.Type != CssTokenType.CSS_TOKEN_NUMBER &&
                token.Type != CssTokenType.CSS_TOKEN_PERCENTAGE)
            {
                index = origIndex;
                return CssStatus.CSS_INVALID;
            }

            int consumed = 0;
            var num = NumberFromString(token.iData, false, out consumed);

            if (token.Type == CssTokenType.CSS_TOKEN_DIMENSION)
            {
                var len = token.iData.Length;
                //const char* data = lwc_string_data(token->idata);
                OpcodeUnit temp_unit = OpcodeUnit.PX;

                var error = ParseUnitKeyword(token.iData.Substring(consumed, len - consumed), out temp_unit);
                if (error != CssStatus.CSS_OK)
                {
                    index = origIndex;
                    return error;
                }

                unit = (uint)temp_unit;
            }
            else if (token.Type == CssTokenType.CSS_TOKEN_NUMBER)
            {
                // Non-zero values are permitted in quirks mode
                if (num.RawValue != 0)
                {
                    if (quirksAllowed)
                    {
                        quirksUsed = true;
                    }
                    else
                    {
                        index = origIndex;
                        return CssStatus.CSS_INVALID;
                    }
                }

                unit = (uint)defaultUnit;

                if (quirksAllowed)
                {
                    // Also, in quirks mode, we need to cater for
                    // dimensions separated from their units by whitespace
                    // (e.g. "0 px")
                    int tempIndex = index;
                    CssUnit tempUnit;

                    ConsumeWhitespace(tokens, ref tempIndex);

                    Log.Unimplemented();

                    /*
                    // Try to parse the unit keyword, ignoring errors
                    token = parserutils_vector_iterate(vector, &temp_ctx);
                    if (token != NULL && token->type == CSS_TOKEN_IDENT)
                    {
                        error = css__parse_unit_keyword(
                                lwc_string_data(token->idata),
                                lwc_string_length(token->idata),
                                &temp_unit);
                        if (error == CSS_OK)
                        {
                            c->sheet->quirks_used = true;
                            *ctx = temp_ctx;
                            *unit = (uint32_t)temp_unit;
                        }
                    }*/
                }
            }
            else
            {
                // Percentage -- number must be entire token data
                if (consumed != token.iData.Length)
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }
                unit = (uint)OpcodeUnit.PCT;
            }

            length = num;
            return CssStatus.CSS_OK;
        }

        // utils.c:1012
        public static CssStatus ParseUnitKeyword(string ptr, out OpcodeUnit unit)
        {
            var len = ptr.Length;
            unit = (OpcodeUnit)0;

            if (len == 4)
            {
                if (string.Equals(ptr, "grad", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.GRAD;
                else if (string.Equals(ptr, "turn", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.TURN;
                else if (string.Equals(ptr, "dppx", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.DPPX;
                else if (string.Equals(ptr, "dpcm", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.DPCM;
                else if (string.Equals(ptr, "vmin", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VMIN;
                else if (string.Equals(ptr, "vmax", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VMAX;
                else
                    return CssStatus.CSS_INVALID;
            }
            else if (len == 3)
            {
                if (string.Equals(ptr, "kHz", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.KHZ;
                else if (string.Equals(ptr, "deg", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.DEG;
                else if (string.Equals(ptr, "rad", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.RAD;
                else if (string.Equals(ptr, "rem", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.REM;
                else if (string.Equals(ptr, "dpi", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.DPI;
                else
                    return CssStatus.CSS_INVALID;
            }
            else if (len == 2)
            {
                if (string.Equals(ptr, "Hz", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.HZ;
                else if (string.Equals(ptr, "ms", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.MS;
                else if (string.Equals(ptr, "px", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.PX;
                else if (string.Equals(ptr, "ex", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.EX;
                else if (string.Equals(ptr, "em", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.EM;
                else if (string.Equals(ptr, "in", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.IN;
                else if (string.Equals(ptr, "cm", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.CM;
                else if (string.Equals(ptr, "mm", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.MM;
                else if (string.Equals(ptr, "pt", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.PT;
                else if (string.Equals(ptr, "pc", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.PC;
                else if (string.Equals(ptr, "ch", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.CH;
                else if (string.Equals(ptr, "lh", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.LH;
                else if (string.Equals(ptr, "vh", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VH;
                else if (string.Equals(ptr, "vw", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VW;
                else if (string.Equals(ptr, "vi", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VI;
                else if (string.Equals(ptr, "vb", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VB;
                else
                    return CssStatus.CSS_INVALID;
            }
            else if (len == 1)
            {
                if (string.Equals(ptr, "s", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.S;
                else if (string.Equals(ptr, "q", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.Q;
                else
                    return CssStatus.CSS_INVALID;
            }
            else
                return CssStatus.CSS_INVALID;

            return CssStatus.CSS_OK;
        }

        void ParseImportant(List<CssToken> tokens, ref int index, ref OpCodeFlag flags)
        {
            int origIndex = index;
            ConsumeWhitespace(tokens, ref index);

            if (index >= tokens.Count) return;
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

        // language.c:1845
        void ParseProperty(CssToken property, List<CssToken> tokens, ref int index, CssRule rule)
        {
            //css_prop_handler handler = NULL;
            int i = 0;

            // Find property index
            // TODO: improve on this linear search
            for (i = 0; i < CssStrings.Props.Length; i++)
            {
                var propname = CssStrings.Props[i].Replace('_', '-'); // So that we don't have to have two tables
                if (property.iData.Equals(propname, StringComparison.OrdinalIgnoreCase))
                    break;
            }

            if (i == CssStrings.Props.Length)
            {
                throw new Exception("CSS Invalid");
                return;
            }

            // Allocate style
            var style = new CssStyle(this);

            //if (i == 0xa) Debug.Assert(false);

            // Get handler and call it
            ParseHandlers[i](tokens, ref index, style);

            // Determine if this declaration is important or not
            OpCodeFlag flags = 0;
            ParseImportant(tokens, ref index, ref flags);

            // Ensure that we've exhausted all the input
            ConsumeWhitespace(tokens, ref index);
            if (index < tokens.Count - 1)
            {
                // Trailing junk, so discard declaration
                Console.WriteLine("Invalid CSS, trailing junk");
                return;
            }

            // If it's important, then mark the style appropriately
            if (flags != 0)
            {
                //css__make_style_important(style);
                Log.Unimplemented();
            }

            // Append style to rule
            rule.AppendStyle(style);
        }

        public static void ConsumeWhitespace(List<CssToken> tokens, ref int index)
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

        // css/parse/properties/css_property_parser_gen.c
        #region Property Parse Handlers
        // autogenerated_color.c:35
        public CssStatus ParseProperty_Color(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;

            if (index >= tokens.Count)
            {
                Console.WriteLine("ERROR: Invalid CSS 659");
                return CssStatus.CSS_INVALID;
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

            return CssStatus.CSS_OK;
        }

        // background.c:33
        public CssStatus Parse_background(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;
            int prevIndex;
            CssStatus error = CssStatus.CSS_OK;
            bool attachment = true;
            bool color = true;
            bool image = true;
            bool position = true;
            bool repeat = true;
            CssStyle attachment_style;
            CssStyle color_style;
            CssStyle image_style;
            CssStyle position_style;
            CssStyle repeat_style;

            // Firstly, handle inherit
            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var token = tokens[index];

            if (token.IsCssInherit())
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_ATTACHMENT,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_COLOR,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_IMAGE,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_POSITION,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_REPEAT,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );


                // parserutils_vector_iterate(vector, ctx);
                if (index < tokens.Count) index++;

                return CssStatus.CSS_OK;
            }

            // allocate styles
            attachment_style = new CssStyle();
            color_style = new CssStyle();
            image_style = new CssStyle();
            position_style = new CssStyle();
            repeat_style = new CssStyle();

            // Attempt to parse the various longhand properties
            do
            {
                prevIndex = index;
                error = CssStatus.CSS_OK;

                if (token.IsCssInherit())
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }

                /* Try each property parser in turn, but only if we
                 * haven't already got a value for this property.
                 */
                if ((attachment) &&
                    (error = CssPropertyParser.Parse_background_attachment(tokens, ref index,
                                attachment_style)) == CssStatus.CSS_OK)
                {
                    attachment = false;
                }
                else if ((color) &&
                       (error = CssPropertyParser.Parse_background_color(tokens, ref index,
                                color_style)) == CssStatus.CSS_OK)
                {
                    color = false;
                }
                else if ((image) &&
                       (error = CssPropertyParser.Parse_background_image(tokens, ref index,
                                image_style)) == CssStatus.CSS_OK)
                {
                    image = false;
                }
                else if ((position) &&
                       (error = Parse_background_position(tokens, ref index,
                            position_style)) == CssStatus.CSS_OK)
                {
                    position = false;
                }
                else if ((repeat) &&
                       (error = CssPropertyParser.Parse_background_repeat(tokens, ref index,
                            repeat_style)) == CssStatus.CSS_OK)
                {
                    repeat = false;
                }

                if (error == CssStatus.CSS_OK)
                {
                    ConsumeWhitespace(tokens, ref index);

                    //token = parserutils_vector_peek(vector, *ctx);
                    if (index >= tokens.Count) break;
                    token = tokens[index];
                }
                else
                {
                    // Forcibly cause loop to exit
                    break;
                }
            } while (index != prevIndex);

            if (attachment)
            {
                attachment_style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_ATTACHMENT,
                        0,
                        (ushort)OpCodeValues.BACKGROUND_ATTACHMENT_SCROLL)
                );
            }

            if (color)
            {
                color_style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_COLOR,
                        0,
                        (ushort)OpCodeValues.BACKGROUND_COLOR_TRANSPARENT)
                );
            }

            if (image)
            {
                image_style.AppendStyle(
                                    new OpCode(
                                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_IMAGE,
                                        0,
                                        (ushort)OpCodeValues.BACKGROUND_IMAGE_NONE)
                                );
            }

            if (position)
            {
                position_style.AppendStyle(
                                    new OpCode(
                                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_POSITION,
                                        0,
                                        (ushort)OpCodeValues.BACKGROUND_POSITION_HORZ_LEFT |
                                        (ushort)OpCodeValues.BACKGROUND_POSITION_VERT_TOP)
                                );
            }

            if (repeat)
            {
                repeat_style.AppendStyle(
                                    new OpCode(
                                        (ushort)CssPropertiesEnum.CSS_PROP_BACKGROUND_REPEAT,
                                        0,
                                        (ushort)OpCodeValues.BACKGROUND_REPEAT_REPEAT)
                                );
            }

            style.MergeStyle(attachment_style);
            style.MergeStyle(color_style);
            style.MergeStyle(image_style);
            style.MergeStyle(position_style);
            style.MergeStyle(repeat_style);

            if (error != CssStatus.CSS_OK)
                index = origIndex;

            return error;
        }

        // background_position.c:33
        public CssStatus Parse_background_position(List<CssToken> tokens, ref int index, CssStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_INVALID;
        }

        // Parse border shorthand
        // border.c:33
        public CssStatus Parse_border(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;
            CssStatus error = CssStatus.CSS_OK;

            error = Parse_border_side(tokens, ref index, style, BorderSide.BORDER_SIDE_TOP);
            if (error != CssStatus.CSS_OK)
            {
                index = origIndex;
                return error;
            }

            index = origIndex;
            error = Parse_border_side(tokens, ref index, style, BorderSide.BORDER_SIDE_RIGHT);
            if (error != CssStatus.CSS_OK)
            {
                index = origIndex;
                return error;
            }

            index = origIndex;
            error = Parse_border_side(tokens, ref index, style, BorderSide.BORDER_SIDE_BOTTOM);
            if (error != CssStatus.CSS_OK)
            {
                index = origIndex;
                return error;
            }

            index = origIndex;
            error = Parse_border_side(tokens, ref index, style, BorderSide.BORDER_SIDE_LEFT);
            if (error != CssStatus.CSS_OK)
                index = origIndex;

            return error;
        }

        // border_style.c:33
        public CssStatus Parse_border_style(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;
            int prevIndex;
            ushort[] side_val = new ushort[4];
            uint side_count = 0;
            CssStatus error = CssStatus.CSS_OK;

            // Firstly, handle inherit
            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var token = tokens[index];

            if (token.IsCssInherit())
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_STYLE,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_STYLE,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_STYLE,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_STYLE,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );

                //parserutils_vector_iterate(vector, ctx);
                if (index < tokens.Count) index++;

                return CssStatus.CSS_OK;
            }

            // Attempt to parse up to 4 styles
            do
            {
                prevIndex = index;

                if (/*(token != null) &&*/ token.IsCssInherit())
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }

                if (token.Type != CssTokenType.CSS_TOKEN_IDENT) break;

                if (string.Equals(token.iData, CssStrings.None, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_NONE;
                }
                else if (string.Equals(token.iData, CssStrings.Hidden, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_HIDDEN;
                }
                else if (string.Equals(token.iData, CssStrings.Dotted, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_DOTTED;
                }
                else if (string.Equals(token.iData, CssStrings.Dashed, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_DASHED;
                }
                else if (string.Equals(token.iData, CssStrings.Solid, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_SOLID;
                }
                else if (string.Equals(token.iData, CssStrings.LibcssDouble, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_DOUBLE;
                }
                else if (string.Equals(token.iData, CssStrings.Groove, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_GROOVE;
                }
                else if (string.Equals(token.iData, CssStrings.Ridge, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_RIDGE;
                }
                else if (string.Equals(token.iData, CssStrings.Inset, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_INSET;
                }
                else if (string.Equals(token.iData, CssStrings.Outset, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (ushort)OpCodeValues.BORDER_STYLE_OUTSET;
                }
                else
                {
                    break;
                }

                side_count++;

                index++;

                ConsumeWhitespace(tokens, ref index);

                //token = parserutils_vector_peek(vector, *ctx);
                if (index >= tokens.Count) break;
                token = tokens[index];
            } while ((index != prevIndex) && (side_count < 4));

            switch (side_count)
            {
                case 1:
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_STYLE, 0, side_val[0]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_STYLE, 0, side_val[0]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_STYLE, 0, side_val[0]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_STYLE, 0, side_val[0]));
                    break;
                case 2:
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_STYLE, 0, side_val[0]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_STYLE, 0, side_val[1]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_STYLE, 0, side_val[0]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_STYLE, 0, side_val[1]));
                    break;
                case 3:
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_STYLE, 0, side_val[0]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_STYLE, 0, side_val[1]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_STYLE, 0, side_val[2]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_STYLE, 0, side_val[1]));
                    break;
                case 4:
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_STYLE, 0, side_val[0]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_STYLE, 0, side_val[1]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_STYLE, 0, side_val[2]));
                    style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_STYLE, 0, side_val[3]));
                    break;
                default:
                    error = CssStatus.CSS_INVALID;
                    break;
            }

            if (error != CssStatus.CSS_OK)
                index = origIndex;

            return error;
        }

        // border_color.c:33
        public CssStatus Parse_border_color(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;
            int prevIndex;
            ushort[] side_val = new ushort[4];
            uint[] side_color = new uint[4];
            uint side_count = 0;
            CssStatus error;

            // Firstly, handle inherit
            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var token = tokens[index];

            if (token.IsCssInherit())
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_COLOR,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_COLOR,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_COLOR,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_COLOR,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );

                //parserutils_vector_iterate(vector, ctx);
                if (index < tokens.Count) index++;

                return CssStatus.CSS_OK;
            }

            // Attempt to parse up to 4 colors
            do
            {
                prevIndex = index;

                if (/*(token != null) &&*/ token.IsCssInherit())
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }

                ParseProperty_ColourSpecifier(tokens, ref index, out side_val[side_count], out side_color[side_count]);
                error = CssStatus.CSS_OK;// because function above doesn't return status

                if (error == CssStatus.CSS_OK)
                {
                    side_count++;

                    ConsumeWhitespace(tokens, ref index);

                    //token = parserutils_vector_peek(vector, *ctx);
                    if (index >= tokens.Count) break;
                    token = tokens[index];
                }
                else
                {
                    // Forcibly cause loop to exit
                    //token = NULL;
                    break;
                }
            } while ((index != prevIndex) && (side_count < 4));

            switch (side_count)
            {
                case 1:
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_COLOR, 0, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_COLOR, 0, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_COLOR, 0, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_COLOR, 0, side_val, side_color);
                    break;
                case 2:
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_COLOR, 0, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_COLOR, 1, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_COLOR, 0, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_COLOR, 1, side_val, side_color);
                    break;
                case 3:
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_COLOR, 0, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_COLOR, 1, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_COLOR, 2, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_COLOR, 1, side_val, side_color);
                    break;
                case 4:
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_TOP_COLOR, 0, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_RIGHT_COLOR, 1, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_BOTTOM_COLOR, 2, side_val, side_color);
                    SIDE_APPEND_border_color(style, (ushort)CssPropertiesEnum.CSS_PROP_BORDER_LEFT_COLOR, 3, side_val, side_color);
                    break;
                default:
                    error = CssStatus.CSS_INVALID;
                    break;
            }

            if (error != CssStatus.CSS_OK)
                index = origIndex;

            return error;
        }

        // Parse border-spacing
        // border_spacing.c:33
        public CssStatus Parse_border_spacing(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;
            CssStatus error = CssStatus.CSS_OK;
            Fixed[] length = new Fixed[2];
            uint[] unit = new uint[2];

            /* length length? | IDENT(inherit) */

            if (index >= tokens.Count) return CssStatus.CSS_INVALID;

            var token = tokens[index];

            if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                string.Equals(token.iData, CssStrings.Inherit, StringComparison.OrdinalIgnoreCase))
            {
                index++;

                // inherit
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_SPACING,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0));
            }
            else
            {
                int num_lengths = 0;

                error = ParseProperty_UnitSpecifier(
                            tokens, ref index,
                            OpcodeUnit.PX,
                            out length[0],
                            out unit[0]);
                if (error != CssStatus.CSS_OK)
                {
                    index = origIndex;
                    return error;
                }

                if (((unit[0] & (uint)OpcodeUnit.ANGLE) != 0) ||
                    ((unit[0] & (uint)OpcodeUnit.TIME) != 0) ||
                    ((unit[0] & (uint)OpcodeUnit.FREQ) != 0) ||
                    ((unit[0] & (uint)OpcodeUnit.PCT) != 0))
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }

                num_lengths = 1;

                ConsumeWhitespace(tokens, ref index);

                if (index < tokens.Count)
                {
                    token = tokens[index];
                    /* Attempt second length, ignoring errors.
                     * The core !important parser will ensure
                     * any remaining junk is thrown out.
                     * Ctx will be preserved on error, as usual
                     */
                    error = ParseProperty_UnitSpecifier(
                                tokens, ref index,
                                OpcodeUnit.PX,
                                out length[1],
                                out unit[1]);

                    if (error == CssStatus.CSS_OK)
                    {
                        if (((unit[1] & (uint)OpcodeUnit.ANGLE) != 0) ||
                            ((unit[1] & (uint)OpcodeUnit.TIME) != 0) ||
                            ((unit[1] & (uint)OpcodeUnit.FREQ) != 0) ||
                            ((unit[1] & (uint)OpcodeUnit.PCT) != 0))
                        {
                            index = origIndex;
                            return CssStatus.CSS_INVALID;
                        }

                        num_lengths = 2;
                    }
                }

                if (num_lengths == 1)
                {
                    // Only one length specified. Use for both axes.
                    length[1] = length[0];
                    unit[1] = unit[0];
                }

                /* Lengths must not be negative */
                if (length[0] < 0 || length[1] < 0)
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }

                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_BORDER_SPACING,
                        0,
                        (ushort)OpCodeValues.BORDER_SPACING_SET));

                style.AppendStyle(new OpCode((uint)length[0].RawValue));
                style.AppendStyle(new OpCode(unit[0]));
                style.AppendStyle(new OpCode((uint)length[1].RawValue));
                style.AppendStyle(new OpCode(unit[1]));
            }

            return CssStatus.CSS_OK;
        }

        // content.c:33
        public CssStatus Parse_content(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;
            CssStatus error = CssStatus.CSS_OK;

            // IDENT(normal, none, inherit) | [ ... ]+
            if (index >= tokens.Count) return CssStatus.CSS_INVALID;

            var token = tokens[index++];

            if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                string.Equals(token.iData, CssStrings.Inherit, StringComparison.OrdinalIgnoreCase))
            {
                style.Inherit((ushort)CssPropertiesEnum.CSS_PROP_CONTENT);
            }
            else if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                string.Equals(token.iData, CssStrings.Normal, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_CONTENT,
                        0,
                        (ushort)OpCodeValues.CONTENT_NORMAL)
                );
            }
            else if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                string.Equals(token.iData, CssStrings.None, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_CONTENT,
                        0,
                        (ushort)OpCodeValues.CONTENT_NONE)
                );
            }
            else
            {
                bool first = true;
                int prevIndex = origIndex;

                /* [
                 *   IDENT(open-quote, close-quote, no-open-quote,
                 *         no-close-quote) |
                 *   STRING |
                 *   URI |
                 *   FUNCTION(attr) IDENT ')' |
                 *   FUNCTION(counter) IDENT IDENT? ')' |
                 *   FUNCTION(counters) IDENT STRING IDENT? ')'
                 * ]+
                 */

                while (true)
                {
                    if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                        string.Equals(token.iData, CssStrings.OpenQuote, StringComparison.OrdinalIgnoreCase))
                    {
                        // #define CSS_APPEND(CSSVAL) css__stylesheet_style_append(result, first?buildOPV(CSS_PROP_CONTENT, 0, CSSVAL):CSSVAL)
                        //error = CSS_APPEND(CONTENT_OPEN_QUOTE);
                        Log.Unimplemented();
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                            string.Equals(token.iData, CssStrings.CloseQuote, StringComparison.OrdinalIgnoreCase))
                    {
                        //error = CSS_APPEND(CONTENT_CLOSE_QUOTE);
                        Log.Unimplemented();
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                             string.Equals(token.iData, CssStrings.NoOpenQuote, StringComparison.OrdinalIgnoreCase))
                    {
                        //error = CSS_APPEND(CONTENT_NO_OPEN_QUOTE);
                        Log.Unimplemented();
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                            string.Equals(token.iData, CssStrings.NoCloseQuote, StringComparison.OrdinalIgnoreCase))
                    {
                        //error = CSS_APPEND(CONTENT_NO_CLOSE_QUOTE);
                        Log.Unimplemented();
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_STRING)
                    {
                        uint snumber;
                        AddString(token.iData, out snumber);

                        style.AppendStyle(first ?
                            new OpCode((ushort)CssPropertiesEnum.CSS_PROP_CONTENT, 0, (ushort)OpCodeValues.CONTENT_STRING) :
                            new OpCode((uint)OpCodeValues.CONTENT_STRING));

                        style.AppendStyle(new OpCode(snumber));

                        error = CssStatus.CSS_OK;
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_URI)
                    {
                        Log.Unimplemented();
                        /*
                        lwc_string* uri;
                        uint32_t uri_snumber;

                        error = c->sheet->resolve(c->sheet->resolve_pw,
                                      c->sheet->url,
                                      token->idata,
                                      &uri);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = css__stylesheet_string_add(c->sheet,
                                          uri,
                                          &uri_snumber);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = CSS_APPEND(CONTENT_URI);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = css__stylesheet_style_append(result, uri_snumber);*/
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_FUNCTION &&
                        string.Equals(token.iData, CssStrings.Attr, StringComparison.OrdinalIgnoreCase))
                    {
                        Log.Unimplemented();
                        /*
                        uint32_t snumber;

                        consumeWhitespace(vector, ctx);

                        // Expect IDENT
                        token = parserutils_vector_iterate(vector, ctx);
                        if (token == NULL || token->type != CSS_TOKEN_IDENT)
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }

                        error = css__stylesheet_string_add(c->sheet, lwc_string_ref(token->idata), &snumber);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = CSS_APPEND(CONTENT_ATTR);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = css__stylesheet_style_append(result, snumber);

                        consumeWhitespace(vector, ctx);

                        // Expect ')'
                        token = parserutils_vector_iterate(vector, ctx);
                        if (token == NULL || tokenIsChar(token, ')') == false)
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }*/
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_FUNCTION &&
                        string.Equals(token.iData, CssStrings.Counter, StringComparison.OrdinalIgnoreCase))
                    {
                        Log.Unimplemented();
                        /*
                        lwc_string* name;
                        uint32_t snumber;
                        uint32_t opv;

                        opv = CONTENT_COUNTER;

                        consumeWhitespace(vector, ctx);

                        // Expect IDENT
                        token = parserutils_vector_iterate(vector, ctx);
                        if (token == NULL || token->type != CSS_TOKEN_IDENT)
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }

                        name = token->idata;

                        consumeWhitespace(vector, ctx);

                        // Possible ','
                        token = parserutils_vector_peek(vector, *ctx);
                        if (token == NULL ||
                            (tokenIsChar(token, ',') == false &&
                             tokenIsChar(token, ')') == false))
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }

                        if (tokenIsChar(token, ','))
                        {
                            uint16_t v;

                            parserutils_vector_iterate(vector, ctx);

                            consumeWhitespace(vector, ctx);

                            // Expect IDENT
                            token = parserutils_vector_peek(vector, *ctx);
                            if (token == NULL || token->type !=
                                CSS_TOKEN_IDENT)
                            {
                                *ctx = orig_ctx;
                                return CSS_INVALID;
                            }

                            error = css__parse_list_style_type_value(c, token, &v);
                            if (error != CSS_OK)
                            {
                                *ctx = orig_ctx;
                                return error;
                            }

                            opv |= v << CONTENT_COUNTER_STYLE_SHIFT;

                            parserutils_vector_iterate(vector, ctx);

                            consumeWhitespace(vector, ctx);
                        }
                        else
                        {
                            opv |= LIST_STYLE_TYPE_DECIMAL <<
                                CONTENT_COUNTER_STYLE_SHIFT;
                        }

                        // Expect ')'
                        token = parserutils_vector_iterate(vector, ctx);
                        if (token == NULL || tokenIsChar(token, ')') == false)
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }

                        error = css__stylesheet_string_add(c->sheet, lwc_string_ref(name), &snumber);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = CSS_APPEND(opv);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = css__stylesheet_style_append(result, snumber);*/
                    }
                    else if (token.Type == CssTokenType.CSS_TOKEN_FUNCTION &&
                        string.Equals(token.iData, CssStrings.Counters, StringComparison.OrdinalIgnoreCase))
                    {
                        Log.Unimplemented();
                        /*
                        lwc_string* name;
                        lwc_string* sep;
                        uint32_t name_snumber;
                        uint32_t sep_snumber;
                        uint32_t opv;

                        opv = CONTENT_COUNTERS;

                        consumeWhitespace(vector, ctx);

                        // Expect IDENT
                        token = parserutils_vector_iterate(vector, ctx);
                        if (token == NULL || token->type != CSS_TOKEN_IDENT)
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }

                        name = token->idata;

                        consumeWhitespace(vector, ctx);

                        // Expect ','
                        token = parserutils_vector_iterate(vector, ctx);
                        if (token == NULL || tokenIsChar(token, ',') == false)
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }

                        consumeWhitespace(vector, ctx);

                        // Expect STRING
                        token = parserutils_vector_iterate(vector, ctx);
                        if (token == NULL || token->type != CSS_TOKEN_STRING)
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }

                        sep = token->idata;

                        consumeWhitespace(vector, ctx);

                        // Possible ','
                        token = parserutils_vector_peek(vector, *ctx);
                        if (token == NULL ||
                            (tokenIsChar(token, ',') == false &&
                             tokenIsChar(token, ')') == false))
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }

                        if (tokenIsChar(token, ','))
                        {
                            uint16_t v;

                            parserutils_vector_iterate(vector, ctx);

                            consumeWhitespace(vector, ctx);

                            // Expect IDENT
                            token = parserutils_vector_peek(vector, *ctx);
                            if (token == NULL || token->type !=
                                CSS_TOKEN_IDENT)
                            {
                                *ctx = orig_ctx;
                                return CSS_INVALID;
                            }

                            error = css__parse_list_style_type_value(c,
                                                token, &v);
                            if (error != CSS_OK)
                            {
                                *ctx = orig_ctx;
                                return error;
                            }

                            opv |= v << CONTENT_COUNTERS_STYLE_SHIFT;

                            parserutils_vector_iterate(vector, ctx);

                            consumeWhitespace(vector, ctx);
                        }
                        else
                        {
                            opv |= LIST_STYLE_TYPE_DECIMAL <<
                                CONTENT_COUNTERS_STYLE_SHIFT;
                        }

                        // Expect ')'
                        token = parserutils_vector_iterate(vector, ctx);
                        if (token == NULL || tokenIsChar(token, ')') == false)
                        {
                            *ctx = orig_ctx;
                            return CSS_INVALID;
                        }


                        error = css__stylesheet_string_add(c->sheet, lwc_string_ref(name), &name_snumber);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = css__stylesheet_string_add(c->sheet, lwc_string_ref(sep), &sep_snumber);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = CSS_APPEND(opv);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = css__stylesheet_style_append(result, name_snumber);
                        if (error != CSS_OK)
                        {
                            *ctx = orig_ctx;
                            return error;
                        }

                        error = css__stylesheet_style_append(result, sep_snumber);*/
                    }
                    else if (first)
                    {
                        // Invalid if this is the first iteration
                        error = CssStatus.CSS_INVALID;
                    }
                    else
                    {
                        // Give up, ensuring current token is reprocessed
                        index = prevIndex;
                        error = CssStatus.CSS_OK;
                        break;
                    }

                    // if there was an error bail
                    if (error != CssStatus.CSS_OK)
                    {
                        index = origIndex;
                        return error;
                    }

                    first = false;

                    ConsumeWhitespace(tokens, ref index);

                    prevIndex = index;
                    //token = parserutils_vector_iterate(vector, ctx);
                    if (index >= tokens.Count) break;
                    token = tokens[index++];
                } // while

                // Write list terminator
                style.AppendStyle(new OpCode((uint)OpCodeValues.CONTENT_NORMAL));
            }

            if (error != CssStatus.CSS_OK)
                index = origIndex;

            return error;
        }

        static Dictionary<string, OpCodeValues> ListTypeMapping = new Dictionary<string, OpCodeValues>()
        {
            { CssStrings.Disc, OpCodeValues.LIST_STYLE_TYPE_DISC},
            { CssStrings.Circle, OpCodeValues.LIST_STYLE_TYPE_CIRCLE},
            { CssStrings.Square, OpCodeValues.LIST_STYLE_TYPE_SQUARE},
            { CssStrings.Decimal, OpCodeValues.LIST_STYLE_TYPE_DECIMAL},
            { CssStrings.DecimalLeadingZero, OpCodeValues.LIST_STYLE_TYPE_DECIMAL_LEADING_ZERO},
            { CssStrings.LowerRoman, OpCodeValues.LIST_STYLE_TYPE_LOWER_ROMAN},
            { CssStrings.UpperRoman, OpCodeValues.LIST_STYLE_TYPE_UPPER_ROMAN},
            { CssStrings.LowerGreek, OpCodeValues.LIST_STYLE_TYPE_LOWER_GREEK},
            { CssStrings.LowerLatin, OpCodeValues.LIST_STYLE_TYPE_LOWER_LATIN},
            { CssStrings.UpperLatin, OpCodeValues.LIST_STYLE_TYPE_UPPER_LATIN},
            { CssStrings.Armenian, OpCodeValues.LIST_STYLE_TYPE_ARMENIAN},
            { CssStrings.Georgian, OpCodeValues.LIST_STYLE_TYPE_GEORGIAN},
            { CssStrings.LowerAlpha, OpCodeValues.LIST_STYLE_TYPE_LOWER_ALPHA},
            { CssStrings.UpperAlpha, OpCodeValues.LIST_STYLE_TYPE_UPPER_ALPHA},
            { CssStrings.None, OpCodeValues.LIST_STYLE_TYPE_NONE},
            { CssStrings.Binary, OpCodeValues.LIST_STYLE_TYPE_BINARY},
            { CssStrings.Octal, OpCodeValues.LIST_STYLE_TYPE_OCTAL},
            { CssStrings.LowerHexadecimal, OpCodeValues.LIST_STYLE_TYPE_LOWER_HEXADECIMAL},
            { CssStrings.UpperHexadecimal, OpCodeValues.LIST_STYLE_TYPE_UPPER_HEXADECIMAL},
            { CssStrings.ArabicIndic, OpCodeValues.LIST_STYLE_TYPE_ARABIC_INDIC},
            { CssStrings.LowerArmenian, OpCodeValues.LIST_STYLE_TYPE_LOWER_ARMENIAN},
            { CssStrings.UpperArmenian, OpCodeValues.LIST_STYLE_TYPE_UPPER_ARMENIAN},
            { CssStrings.Bengali, OpCodeValues.LIST_STYLE_TYPE_BENGALI},
            { CssStrings.Cambodian, OpCodeValues.LIST_STYLE_TYPE_CAMBODIAN},
            { CssStrings.Khmer, OpCodeValues.LIST_STYLE_TYPE_KHMER},
            { CssStrings.CjkDecimal, OpCodeValues.LIST_STYLE_TYPE_CJK_DECIMAL},
            { CssStrings.Devanagari, OpCodeValues.LIST_STYLE_TYPE_DEVANAGARI},
            { CssStrings.Gujarati, OpCodeValues.LIST_STYLE_TYPE_GUJARATI},
            { CssStrings.Gurmukhi, OpCodeValues.LIST_STYLE_TYPE_GURMUKHI},
            { CssStrings.Hebrew, OpCodeValues.LIST_STYLE_TYPE_HEBREW},
            { CssStrings.Kannada, OpCodeValues.LIST_STYLE_TYPE_KANNADA},
            { CssStrings.Lao, OpCodeValues.LIST_STYLE_TYPE_LAO},
            { CssStrings.Malayalam, OpCodeValues.LIST_STYLE_TYPE_MALAYALAM},
            { CssStrings.Mongolian, OpCodeValues.LIST_STYLE_TYPE_MONGOLIAN},
            { CssStrings.Myanmar, OpCodeValues.LIST_STYLE_TYPE_MYANMAR},
            { CssStrings.Oriya, OpCodeValues.LIST_STYLE_TYPE_ORIYA},
            { CssStrings.Persian, OpCodeValues.LIST_STYLE_TYPE_PERSIAN},
            { CssStrings.Tamil, OpCodeValues.LIST_STYLE_TYPE_TAMIL},
            { CssStrings.Telugu, OpCodeValues.LIST_STYLE_TYPE_TELUGU},
            { CssStrings.Thai, OpCodeValues.LIST_STYLE_TYPE_THAI},
            { CssStrings.Tibetan, OpCodeValues.LIST_STYLE_TYPE_TIBETAN},
            { CssStrings.CjkEarthlyBranch, OpCodeValues.LIST_STYLE_TYPE_CJK_EARTHLY_BRANCH},
            { CssStrings.CjkHeavenlyStem, OpCodeValues.LIST_STYLE_TYPE_CJK_HEAVENLY_STEM},
            { CssStrings.Hiragana, OpCodeValues.LIST_STYLE_TYPE_HIAGANA},
            { CssStrings.HiraganaIroha, OpCodeValues.LIST_STYLE_TYPE_HIAGANA_IROHA},
            { CssStrings.Katakana, OpCodeValues.LIST_STYLE_TYPE_KATAKANA},
            { CssStrings.KatakanaIroha, OpCodeValues.LIST_STYLE_TYPE_KATAKANA_IROHA},
            { CssStrings.JapaneseInformal, OpCodeValues.LIST_STYLE_TYPE_JAPANESE_INFORMAL},
            { CssStrings.JapaneseFormal, OpCodeValues.LIST_STYLE_TYPE_JAPANESE_FORMAL},
            { CssStrings.KoreanHangulFormal, OpCodeValues.LIST_STYLE_TYPE_KOREAN_HANGUL_FORMAL},
            { CssStrings.KoreanHanjaInformal, OpCodeValues.LIST_STYLE_TYPE_KOREAN_HANJA_INFORMAL},
            { CssStrings.KoreanHanjaFormal, OpCodeValues.LIST_STYLE_TYPE_KOREAN_HANJA_FORMAL}
        };

        // utils.c:31
        // Parse list-style-type value
        CssStatus Parse_list_style_type_value(CssToken ident, out ushort value)
        {
            /* IDENT (disc, circle, square, decimal, decimal-leading-zero,
	         *	  lower-roman, upper-roman, lower-greek, lower-latin,
	         *	  upper-latin, armenian, georgian, lower-alpha, upper-alpha,
	         *	  none)
	         */


            if (!ListTypeMapping.ContainsKey(ident.iData))
            {
                value = 0;
                return CssStatus.CSS_INVALID;
            }

            value = (ushort)ListTypeMapping[ident.iData]; // FIXME: Should be caseless comparison
            return CssStatus.CSS_OK;
        }

        // list_style_type.c:33
        public CssStatus Parse_list_style_type(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int orig_ctx = index;
            CssStatus error = CssStatus.CSS_OK;
            //const css_token* ident;
            byte flags = 0;
            ushort value = 0;
            //bool match;

            /* IDENT (disc, circle, square, decimal, decimal-leading-zero,
             *	  lower-roman, upper-roman, lower-greek, lower-latin,
             *	  upper-latin, armenian, georgian, lower-alpha, upper-alpha,
             *	  none, inherit)
             */
            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var ident = tokens[index++];
            if (ident.Type != CssTokenType.CSS_TOKEN_IDENT)
            {
                index = orig_ctx;
                return CssStatus.CSS_INVALID;
            }

            if (string.Equals(ident.iData, CssStrings.Inherit, StringComparison.OrdinalIgnoreCase))
            {
                flags |= (byte)OpCodeFlag.FLAG_INHERIT;
            }
            else
            {
                error = Parse_list_style_type_value(ident, out value);
                if (error != CssStatus.CSS_OK)
                {
                    index = orig_ctx;
                    return error;
                }
            }

            style.AppendStyle(
                new OpCode(
                    (ushort)CssPropertiesEnum.CSS_PROP_LIST_STYLE_TYPE,
                    flags,
                    value)
            );

            return error;
        }

        static void SIDE_APPEND_border_color(CssStyle style, ushort op, int num, ushort[] side_val, uint[] side_color)
        {
            style.AppendStyle(new OpCode(op, 0, side_val[(num)]));

            if (side_val[num] == (int)OpCodeValues.BORDER_COLOR_SET)
            {
                style.AppendStyle(new OpCode(side_color[num]));
            }
        }

        static void SIDE_APPEND(CssStyle style, ushort op, int num, uint[] side_val, Fixed[] side_length, uint[] side_unit)
        {
            style.AppendStyle(new OpCode(op, 0, (ushort)side_val[(num)]));

            if (side_val[num] == (int)OpCodeValues.MARGIN_SET)
            {
                style.AppendStyle(new OpCode((uint)side_length[num].RawValue));
                style.AppendStyle(new OpCode(side_unit[num]));
            }
        }

        static void SIDE_APPEND_padding(CssStyle style, ushort op, int num, Fixed[] side_length, uint[] side_unit)
        {
            style.AppendStyle(new OpCode(op, 0, (ushort)OpCodeValues.PADDING_SET));
            style.AppendStyle(new OpCode((uint)side_length[num].RawValue));
            style.AppendStyle(new OpCode(side_unit[num]));
        }

        // margin.c:33
        public CssStatus Parse_margin(List<CssToken> tokens, ref int index, CssStyle style)
        {
            CssStatus error = CssStatus.CSS_OK;
            uint [] side_val = new uint[4];
            Fixed [] side_length = new Fixed[4];
            uint [] side_unit = new uint[4];
            uint side_count = 0;
            int origIndex = index;
            int prevIndex;

            // Firstly, handle inherit
            var token = tokens[index];

            if (token.IsCssInherit())
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_TOP,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_RIGHT,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_BOTTOM,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_LEFT,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );

                //parserutils_vector_iterate(vector, ctx);
                if (index < tokens.Count) index++;

                return error;
            }

            // Attempt to parse up to 4 widths
            do
            {
                //prev_ctx = *ctx;
                prevIndex = index;

                if (/*(token != null) &&*/ token.IsCssInherit())
                {
                    //*ctx = orig_ctx;
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }

                if ((token.Type == CssTokenType.CSS_TOKEN_IDENT) &&
                    string.Equals(token.iData, CssStrings.Auto, StringComparison.OrdinalIgnoreCase))
                {
                    side_val[side_count] = (uint)OpCodeValues.MARGIN_AUTO;
                    //parserutils_vector_iterate(vector, ctx);
                    index++;
                    error = CssStatus.CSS_OK;
                }
                else
                {
                    side_val[side_count] = (uint)OpCodeValues.MARGIN_SET;

                    error = ParseProperty_UnitSpecifier(
                                tokens, ref index,
                                OpcodeUnit.PX,
                                out side_length[side_count],
                                out side_unit[side_count]);
                    if (error == CssStatus.CSS_OK)
                    {
                        if (((side_unit[side_count] & (uint)OpcodeUnit.ANGLE) != 0) ||
                            ((side_unit[side_count] & (uint)OpcodeUnit.TIME) != 0) ||
                            ((side_unit[side_count] & (uint)OpcodeUnit.FREQ) != 0))
                        {
                            //*ctx = orig_ctx;
                            index = origIndex;
                            return CssStatus.CSS_INVALID;
                        }
                    }
                }

                if (error == CssStatus.CSS_OK)
                {
                    side_count++;

                    ConsumeWhitespace(tokens, ref index);

                    //token = parserutils_vector_peek(vector, *ctx);
                    if (index >= tokens.Count) break;
                    token = tokens[index];
                }
                else
                {
                    /* Forcibly cause loop to exit */
                    //token = NULL;
                    break;
                }
            } while ((index != prevIndex) && (side_count < 4));

            switch (side_count)
            {
                case 1:
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_TOP, 0, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_RIGHT, 0, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_BOTTOM, 0, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_LEFT, 0, side_val, side_length, side_unit);
                    break;
                case 2:
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_TOP, 0, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_RIGHT, 1, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_BOTTOM, 0, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_LEFT, 1, side_val, side_length, side_unit);
                    break;
                case 3:
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_TOP, 0, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_RIGHT, 1, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_BOTTOM, 2, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_LEFT, 1, side_val, side_length, side_unit);
                    break;
                case 4:
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_TOP, 0, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_RIGHT, 1, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_BOTTOM, 2, side_val, side_length, side_unit);
                    SIDE_APPEND(style, (ushort)CssPropertiesEnum.CSS_PROP_MARGIN_LEFT, 3, side_val, side_length, side_unit);
                    break;
                default:
                    error = CssStatus.CSS_INVALID;
                    break;
            }

            if (error != CssStatus.CSS_OK)
                index = origIndex;

            return error;
        }

        // overflow.c:33
        public CssStatus Parse_overflow(List<CssToken> tokens, ref int index, CssStyle style)
        {
            CssStatus error1 = CssStatus.CSS_OK, error2 = CssStatus.CSS_OK;
            int origIndex = index;

            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var token = tokens[index++];

            if (token.Type != CssTokenType.CSS_TOKEN_IDENT)
            {
                index = origIndex;
                return CssStatus.CSS_INVALID;
            }


            if (string.Equals(token.iData, CssStrings.Inherit, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_X,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_Y,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
            }
            else if (string.Equals(token.iData, CssStrings.Visible, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_X,
                        0,
                        (ushort)OpCodeValues.OVERFLOW_VISIBLE)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_Y,
                        0,
                        (ushort)OpCodeValues.OVERFLOW_VISIBLE)
                );

            }
            else if (string.Equals(token.iData, CssStrings.Hidden, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_X,
                        0,
                        (ushort)OpCodeValues.OVERFLOW_HIDDEN)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_Y,
                        0,
                        (ushort)OpCodeValues.OVERFLOW_HIDDEN)
                );
            }
            else if (string.Equals(token.iData, CssStrings.Scroll, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_X,
                        0,
                        (ushort)OpCodeValues.OVERFLOW_SCROLL)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_Y,
                        0,
                        (ushort)OpCodeValues.OVERFLOW_SCROLL)
                );
            }
            else if (string.Equals(token.iData, CssStrings.Auto, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_X,
                        0,
                        (ushort)OpCodeValues.OVERFLOW_AUTO)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_OVERFLOW_Y,
                        0,
                        (ushort)OpCodeValues.OVERFLOW_AUTO)
                );
            }
            else
            {
                error1 = CssStatus.CSS_INVALID;
            }

            if (error2 != CssStatus.CSS_OK)
                error1 = error2;

            if (error1 != CssStatus.CSS_OK)
                index = origIndex;

            return error1;
        }

        // padding.c:33
        public CssStatus Parse_padding(List<CssToken> tokens, ref int index, CssStyle style)
        {
            CssStatus error = CssStatus.CSS_OK;
            Fixed[] side_length = new Fixed[4];
            uint[] side_unit = new uint[4];
            uint side_count = 0;
            int origIndex = index;
            int prevIndex;

            // Firstly, handle inherit
            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var token = tokens[index];

            if (token.IsCssInherit())
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_PADDING_TOP,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_PADDING_RIGHT,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_PADDING_BOTTOM,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_PADDING_LEFT,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );

                if (index < tokens.Count) index++;

                return error;
            }

            // Attempt to parse up to 4 widths
            do
            {
                prevIndex = index;

                if (/*(token != null) &&*/ token.IsCssInherit())
                {
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }

                error = ParseProperty_UnitSpecifier(
                    tokens, ref index,
                    OpcodeUnit.PX,
                    out side_length[side_count],
                    out side_unit[side_count]);
                if (error == CssStatus.CSS_OK)
                {
                    if (((side_unit[side_count] & (uint)OpcodeUnit.ANGLE) != 0) ||
                        ((side_unit[side_count] & (uint)OpcodeUnit.TIME) != 0) ||
                        ((side_unit[side_count] & (uint)OpcodeUnit.FREQ) != 0))
                    {
                        index = origIndex;
                        return CssStatus.CSS_INVALID;
                    }

                    if (side_length[side_count] < 0)
                    {
                        index = origIndex;
                        return CssStatus.CSS_INVALID;
                    }

                    side_count++;

                    ConsumeWhitespace(tokens, ref index);

                    if (index >= tokens.Count) break;
                    token = tokens[index];
                }
                else
                {
                    /* Forcibly cause loop to exit */
                    //token = NULL;
                    break;
                }
            } while ((index != prevIndex) && (side_count < 4));

            switch (side_count)
            {
                case 1:
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_TOP, 0, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_RIGHT, 0, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_BOTTOM, 0, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_LEFT, 0, side_length, side_unit);
                    break;
                case 2:
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_TOP, 0, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_RIGHT, 1, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_BOTTOM, 0, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_LEFT, 1, side_length, side_unit);
                    break;
                case 3:
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_TOP, 0, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_RIGHT, 1, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_BOTTOM, 2, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_LEFT, 1, side_length, side_unit);
                    break;
                case 4:
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_TOP, 0, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_RIGHT, 1, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_BOTTOM, 2, side_length, side_unit);
                    SIDE_APPEND_padding(style, (ushort)CssPropertiesEnum.CSS_PROP_PADDING_LEFT, 3, side_length, side_unit);
                    break;
                default:
                    error = CssStatus.CSS_INVALID;
                    break;
            }

            if (error != CssStatus.CSS_OK)
                index = origIndex;

            return error;
        }

        // font_family.c:33
        public CssStatus Parse_font_family(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;
            CssStatus error = CssStatus.CSS_OK;

            /* [ IDENT+ | STRING ] [ ',' [ IDENT+ | STRING ] ]* | IDENT(inherit)
             *
             * In the case of IDENT+, any whitespace between tokens is collapsed to
             * a single space
             *
             * \todo Mozilla makes the comma optional.
             * Perhaps this is a quirk we should inherit?
             */

            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var token = tokens[index++];

            if (token.Type != CssTokenType.CSS_TOKEN_IDENT &&
                token.Type != CssTokenType.CSS_TOKEN_STRING)
            {
                index = origIndex;
                return CssStatus.CSS_INVALID;
            }

            if (token.Type == CssTokenType.CSS_TOKEN_IDENT &&
                string.Equals(token.iData, CssStrings.Inherit, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_FONT_FAMILY,
                        0,
                        0));
            }
            else
            {
                index = origIndex;

                error = CommaListToStyle(tokens, ref index, true, style);
                if (error != CssStatus.CSS_OK)
                {
                    index = origIndex;
                    return error;
                }

                style.AppendStyle(new OpCode((uint)OpCodeValues.FONT_FAMILY_END));
            }

            if (error != CssStatus.CSS_OK)
            {
                index = origIndex;
                return error;
            }

            return CssStatus.CSS_OK;
        }

        // font_family.c:24
        // Determine if a given font-family ident is reserved
        static bool IsFontFamilyReserved(CssToken ident)
        {
            return
                string.Equals(ident.iData, CssStrings.Serif, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ident.iData, CssStrings.SansSerif, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ident.iData, CssStrings.Cursive, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ident.iData, CssStrings.Fantasy, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ident.iData, CssStrings.Monospace, StringComparison.OrdinalIgnoreCase);
        }

        static OpCode GetFontFamilyValue(CssToken token, bool first)
        {
            OpCodeValues value;

            if (token.Type == CssTokenType.CSS_TOKEN_IDENT)
            {
                if (string.Equals(token.iData, CssStrings.Serif, StringComparison.OrdinalIgnoreCase))
                    value = OpCodeValues.FONT_FAMILY_SERIF;
                else if (string.Equals(token.iData, CssStrings.SansSerif, StringComparison.OrdinalIgnoreCase))
                    value = OpCodeValues.FONT_FAMILY_SANS_SERIF;
                else if (string.Equals(token.iData, CssStrings.Cursive, StringComparison.OrdinalIgnoreCase))
                    value = OpCodeValues.FONT_FAMILY_CURSIVE;
                else if (string.Equals(token.iData, CssStrings.Fantasy, StringComparison.OrdinalIgnoreCase))
                    value = OpCodeValues.FONT_FAMILY_FANTASY;
                else if (string.Equals(token.iData, CssStrings.Monospace, StringComparison.OrdinalIgnoreCase))
                    value = OpCodeValues.FONT_FAMILY_MONOSPACE;
                else
                    value = OpCodeValues.FONT_FAMILY_IDENT_LIST;
            }
            else
            {
                value = OpCodeValues.FONT_FAMILY_STRING;
            }

            OpCode opcode;
            if (first)
                opcode = new OpCode((ushort)CssPropertiesEnum.CSS_PROP_FONT_FAMILY, 0, (ushort)value);
            else
                opcode = new OpCode((uint)value);

            return opcode;
        }

        // utils.c:1236
        /**
         * Parse a comma separated list, converting to bytecode
         * Post condition: index is updated with the next token to process
         *                 If the input is invalid, then index remains unchanged.
         */
        CssStatus CommaListToStyle(List<CssToken> tokens, ref int index,
                                   //CssLanguage c,
                                   bool fontfamily, // either font family or voicefamily
                                   CssStyle result) 
                                   //bool (* reserved) (css_language* c, const css_token* ident),
                                   //css_code_t (*get_value)(css_language* c, const css_token* token, bool first))
        {
            int origIndex = index;
            int prevIndex = origIndex;
            //const css_token* token;
            bool first = true;
            CssStatus error = CssStatus.CSS_OK;

            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var token = tokens[index++];

            if (fontfamily == false)
                Console.WriteLine("UNIMPLEMENTED voice-family support");

            while (true)
            {
                if (token.Type == CssTokenType.CSS_TOKEN_IDENT)
                {
                    var value = GetFontFamilyValue(token, first);

                    if (!IsFontFamilyReserved(token))
                    {
                        string str = null;
                        uint snumber = 0;


                        index = prevIndex;
                        /*
                        error = css__ident_list_to_string(c, vector, ctx, reserved, &str);
                        if (error != CssStatus.CSS_OK)
                            goto cleanup;

                        error = css__stylesheet_string_add(c->sheet, str, &snumber);
                        if (error != CssStatus.CSS_OK)
                            goto cleanup;*/
                        Log.Unimplemented();

                        result.AppendStyle(value);
                        result.AppendStyle(new OpCode(snumber));
                    }
                    else
                    {
                        result.AppendStyle(value);
                    }
                }
                else if (token.Type == CssTokenType.CSS_TOKEN_STRING)
                {
                    var value = GetFontFamilyValue(token, first);
                    uint snumber = 0;

                    /*error = css__stylesheet_string_add(c->sheet, lwc_string_ref(token->idata), &snumber);
                    if (error != CssStatus.CSS_OK)
                        goto cleanup;*/
                    Log.Unimplemented();

                    result.AppendStyle(value);
                    result.AppendStyle(new OpCode(snumber));
                }
                else
                {
                    error = CssStatus.CSS_INVALID;
                    goto cleanup;
                }

                ConsumeWhitespace(tokens, ref index);

                if (index >= tokens.Count) break;
                token = tokens[index];
                if (token.IsChar(','))
                {
                    index++;

                    ConsumeWhitespace(tokens, ref index);

                    if (index >= tokens.Count)
                    {
                        error = CssStatus.CSS_INVALID;
                        goto cleanup;
                    }
                    token = tokens[index];

                    if ((token.Type != CssTokenType.CSS_TOKEN_IDENT &&
                         token.Type != CssTokenType.CSS_TOKEN_STRING))
                    {
                        error = CssStatus.CSS_INVALID;
                        goto cleanup;
                    }
                }

                first = false;
                prevIndex = index;

                if (index >= tokens.Count) break;
                token = tokens[index++];
            }

            cleanup:
            if (error != CssStatus.CSS_OK)
                index = origIndex;

            return error;
        }


        // font_weight.c:33
        public CssStatus Parse_font_weight(List<CssToken> tokens, ref int index, CssStyle style)
        {
            CssStatus error = CssStatus.CSS_OK;
            byte flags = 0;
            OpCodeValues value = 0;

            //int orig_ctx = *ctx;
            int origIndex = index;

            /* NUMBER (100, 200, 300, 400, 500, 600, 700, 800, 900) |
             * IDENT (normal, bold, bolder, lighter, inherit) */
            //token = parserutils_vector_iterate(vector, ctx);
            var token = tokens[index++];

            if (index > tokens.Count || (token.Type != CssTokenType.CSS_TOKEN_IDENT && token.Type != CssTokenType.CSS_TOKEN_NUMBER))
            {
                //*ctx = orig_ctx;
                index = origIndex;
                return CssStatus.CSS_INVALID;
            }

            if (string.Equals(token.iData, CssStrings.Inherit, StringComparison.OrdinalIgnoreCase))
            {
                flags |= (byte)OpCodeFlag.FLAG_INHERIT;
            }
            else if (token.Type == CssTokenType.CSS_TOKEN_NUMBER)
            {
                int consumed = 0;
                var num = CssStylesheet.NumberFromString(token.iData, true, out consumed);
                // Invalid if there are trailing characters
                if (consumed != token.iData.Length)
                {
                    //*ctx = orig_ctx;
                    index = origIndex;
                    return CssStatus.CSS_INVALID;
                }

                switch (num.ToInt())
                {
                    case 100: value = OpCodeValues.FONT_WEIGHT_100; break;
                    case 200: value = OpCodeValues.FONT_WEIGHT_200; break;
                    case 300: value = OpCodeValues.FONT_WEIGHT_300; break;
                    case 400: value = OpCodeValues.FONT_WEIGHT_400; break;
                    case 500: value = OpCodeValues.FONT_WEIGHT_500; break;
                    case 600: value = OpCodeValues.FONT_WEIGHT_600; break;
                    case 700: value = OpCodeValues.FONT_WEIGHT_700; break;
                    case 800: value = OpCodeValues.FONT_WEIGHT_800; break;
                    case 900: value = OpCodeValues.FONT_WEIGHT_900; break;
                    default:
                        index = origIndex;
                        return CssStatus.CSS_INVALID;
                }
            }
            else if (string.Equals(token.iData, CssStrings.Normal, StringComparison.OrdinalIgnoreCase))
            {
                value = OpCodeValues.FONT_WEIGHT_NORMAL;
            }
            else if (string.Equals(token.iData, CssStrings.Bold, StringComparison.OrdinalIgnoreCase))
            {
                value = OpCodeValues.FONT_WEIGHT_BOLD;
            }
            else if (string.Equals(token.iData, CssStrings.Bolder, StringComparison.OrdinalIgnoreCase))
            {
                value = OpCodeValues.FONT_WEIGHT_BOLDER;
            }
            else if (string.Equals(token.iData, CssStrings.Lighter, StringComparison.OrdinalIgnoreCase))
            {
                value = OpCodeValues.FONT_WEIGHT_LIGHTER;
            }
            else
            {
                //*ctx = orig_ctx;
                index = origIndex;
                return CssStatus.CSS_INVALID;
            }

            style.AppendStyle(
                new OpCode(
                    (ushort)CssPropertiesEnum.CSS_PROP_FONT_WEIGHT,
                    flags,
                    (ushort)value)
            );

            return error;
        }

        // text_decoration.c:33
        public CssStatus Parse_text_decoration(List<CssToken> tokens, ref int index, CssStyle style)
        {
            int origIndex = index;
            //CssStatus error = CssStatus.CSS_INVALID;

            /* IDENT([ underline || overline || line-through || blink ])
             * | IDENT (none, inherit) */
            if (index >= tokens.Count) return CssStatus.CSS_INVALID;
            var token = tokens[index++];

            if (token.Type != CssTokenType.CSS_TOKEN_IDENT)
            {
                index = origIndex;
                return CssStatus.CSS_INVALID;
            }

            if (string.Equals(token.iData, CssStrings.Inherit, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_TEXT_DECORATION,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        0)
                );
            }
            else if (string.Equals(token.iData, CssStrings.None, StringComparison.OrdinalIgnoreCase))
            {
                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_TEXT_DECORATION,
                        (byte)OpCodeFlag.FLAG_INHERIT,
                        (ushort)OpCodeValues.TEXT_DECORATION_NONE)
                );
            }
            else
            {
                ushort value = 0;
                while (true)
                {
                    if (string.Equals(token.iData, CssStrings.Underline, StringComparison.OrdinalIgnoreCase))
                    {
                        if ((value & (ushort)OpCodeValues.TEXT_DECORATION_UNDERLINE) == 0)
                            value |= (ushort)OpCodeValues.TEXT_DECORATION_UNDERLINE;
                        else
                        {
                            index = origIndex;
                            return CssStatus.CSS_INVALID;
                        }
                    }
                    else if (string.Equals(token.iData, CssStrings.Overline, StringComparison.OrdinalIgnoreCase))
                    {
                        if ((value & (ushort)OpCodeValues.TEXT_DECORATION_OVERLINE) == 0)
                            value |= (ushort)OpCodeValues.TEXT_DECORATION_OVERLINE;
                        else
                        {
                            index = origIndex;
                            return CssStatus.CSS_INVALID;
                        }
                    }
                    else if (string.Equals(token.iData, CssStrings.LineThrough, StringComparison.OrdinalIgnoreCase))
                    {
                        if ((value & (ushort)OpCodeValues.TEXT_DECORATION_LINE_THROUGH) == 0)
                            value |= (ushort)OpCodeValues.TEXT_DECORATION_LINE_THROUGH;
                        else
                        {
                            index = origIndex;
                            return CssStatus.CSS_INVALID;
                        }
                    }
                    else if (string.Equals(token.iData, CssStrings.Blink, StringComparison.OrdinalIgnoreCase))
                    {
                        if ((value & (ushort)OpCodeValues.TEXT_DECORATION_BLINK) == 0)
                            value |= (ushort)OpCodeValues.TEXT_DECORATION_BLINK;
                        else
                        {
                            index = origIndex;
                            return CssStatus.CSS_INVALID;
                        }
                    }
                    else
                    {
                        index = origIndex;
                        return CssStatus.CSS_INVALID;
                    }

                    ConsumeWhitespace(tokens, ref index);

                    if (index >= tokens.Count) break;
                    token = tokens[index];

                    if (token.Type != CssTokenType.CSS_TOKEN_IDENT)
                        break;

                    //parserutils_vector_iterate(vector, ctx);
                    token = tokens[index++];
                }

                style.AppendStyle(
                    new OpCode(
                        (ushort)CssPropertiesEnum.CSS_PROP_TEXT_DECORATION,
                        0,
                        value)
                );

                //error = CssStatus.CSS_OK;
            }

            //if (error != CssStatus.CSS_OK)
                //index = origIndex;

            return CssStatus.CSS_OK;
        }
        #endregion
    }

    // select.c:39
    // Container for stylesheet selection info
    public struct CssSelectSheet
    {
        public CssStylesheet Sheet;
        public CssOrigin Origin;
        //CssMqQuery Media;
    }

    // select.c:48
    // CSS selection context
    public class CssSelectionContext
    {
        public List<CssSelectSheet> Sheets = new List<CssSelectSheet>();
        // Maybe something else, but for now empty

        // select.c:255
        public CssSelectionContext()
        {

        }

        // select.c:315
        public void AppendSheet(CssStylesheet sheet, CssOrigin origin, string media)
        {
            InsertSheet(sheet, -1, origin, media);
        }

        // select.c:336
        public void InsertSheet(CssStylesheet sheet, int index, CssOrigin origin, string media)
        {
            if (index != -1)
            {
                Log.Unimplemented("Inserting sheets at arbitrary position is not supported yet");
                return;
            }

            if (sheet.InlineStyle)
            {
                Console.WriteLine("Trying to insert inline style into selection context");
                return;
            }

            var item = new CssSelectSheet();
            item.Sheet = sheet;
            item.Origin = origin;

            Log.Unimplemented("Ignoring media query");

            Sheets.Add(item);
        }
    }
}
