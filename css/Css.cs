using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
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
            "FLEX_WRAP",            "LIBCSS_FLOAT",         "FONT",                 "FONT_FAMILY",          // 68
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
            "Z_INDEX"                                                                                       // 141
        };

        // FIXME: Maybe unnecessary, or could be converted to an array
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

            RuleCount = 0;
            RuleList = new LinkedList<CssRule>();

            QuirksAllowed = false;
            QuirksUsed = false;
            Disabled = false;

            Selectors = new CssSelectorHash();

            // Fill in property parse handlers table (CodeGenerator.GenerateCodeStubs2())
            ParseHandlers = new ParseProperty[CssStrings.Props.Length - 1];
            ParseHandlers[0] = CssPropertyParser.Parse_align_content;
            ParseHandlers[1] = CssPropertyParser.Parse_align_items;
            ParseHandlers[2] = CssPropertyParser.Parse_align_self;
            //ParseHandlers[3] = CssPropertyParser.Parse_azimuth;
            //ParseHandlers[4] = CssPropertyParser.Parse_background;
            ParseHandlers[5] = CssPropertyParser.Parse_background_attachment;
            ParseHandlers[6] = CssPropertyParser.Parse_background_color;
            ParseHandlers[7] = CssPropertyParser.Parse_background_image;
            //ParseHandlers[8] = CssPropertyParser.Parse_background_position;
            ParseHandlers[9] = CssPropertyParser.Parse_background_repeat;
            //ParseHandlers[10] = CssPropertyParser.Parse_border;
            ParseHandlers[11] = CssPropertyParser.Parse_border_bottom;
            ParseHandlers[12] = CssPropertyParser.Parse_border_bottom_color;
            ParseHandlers[13] = CssPropertyParser.Parse_border_bottom_style;
            ParseHandlers[14] = CssPropertyParser.Parse_border_bottom_width;
            ParseHandlers[15] = CssPropertyParser.Parse_border_collapse;
            //ParseHandlers[16] = CssPropertyParser.Parse_border_color;
            ParseHandlers[17] = CssPropertyParser.Parse_border_left;
            ParseHandlers[18] = CssPropertyParser.Parse_border_left_color;
            ParseHandlers[19] = CssPropertyParser.Parse_border_left_style;
            ParseHandlers[20] = CssPropertyParser.Parse_border_left_width;
            ParseHandlers[21] = CssPropertyParser.Parse_border_right;
            ParseHandlers[22] = CssPropertyParser.Parse_border_right_color;
            ParseHandlers[23] = CssPropertyParser.Parse_border_right_style;
            ParseHandlers[24] = CssPropertyParser.Parse_border_right_width;
            //ParseHandlers[25] = CssPropertyParser.Parse_border_spacing;
            //ParseHandlers[26] = CssPropertyParser.Parse_border_style;
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
            //ParseHandlers[51] = CssPropertyParser.Parse_content;
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
            //ParseHandlers[69] = CssPropertyParser.Parse_libcss_float;
            //ParseHandlers[70] = CssPropertyParser.Parse_font;
            //ParseHandlers[71] = CssPropertyParser.Parse_font_family;
            ParseHandlers[72] = CssPropertyParser.Parse_font_size;
            ParseHandlers[73] = CssPropertyParser.Parse_font_style;
            ParseHandlers[74] = CssPropertyParser.Parse_font_variant;
            //ParseHandlers[75] = CssPropertyParser.Parse_font_weight;
            ParseHandlers[76] = CssPropertyParser.Parse_height;
            ParseHandlers[77] = CssPropertyParser.Parse_justify_content;
            ParseHandlers[78] = CssPropertyParser.Parse_left;
            ParseHandlers[79] = CssPropertyParser.Parse_letter_spacing;
            ParseHandlers[80] = CssPropertyParser.Parse_line_height;
            //ParseHandlers[81] = CssPropertyParser.Parse_list_style;
            ParseHandlers[82] = CssPropertyParser.Parse_list_style_image;
            ParseHandlers[83] = CssPropertyParser.Parse_list_style_position;
            //ParseHandlers[84] = CssPropertyParser.Parse_list_style_type;
            //ParseHandlers[85] = CssPropertyParser.Parse_margin;
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
            //ParseHandlers[101] = CssPropertyParser.Parse_overflow;
            ParseHandlers[102] = CssPropertyParser.Parse_overflow_x;
            ParseHandlers[103] = CssPropertyParser.Parse_overflow_y;
            //ParseHandlers[104] = CssPropertyParser.Parse_padding;
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
        }

        // stylesheet.c:311
        public void AppendData(string data)
        {
            Parser.ParseChunk(data);
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

        // language.c:949
        void ParseClass(List<CssToken> tokens, ref int index, out CssSelectorDetail specific)
        {
            CssQname qname = new CssQname();
            CssSelectorDetailValue detail_value = new CssSelectorDetailValue();

            /* class     -> '.' IDENT */
            //token = parserutils_vector_iterate(vector, ctx);
            if (index >= tokens.Count - 1)
            {
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
            if (index >= tokens.Count - 1)
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
                Log.Unimplemented();
                specific = new CssSelectorDetail();
                //error = parseAttrib(c, vector, ctx, specific);
            }
            else if (token.IsChar(':'))
            {
                Log.Unimplemented();
                specific = new CssSelectorDetail();
                //error = parsePseudo(c, vector, ctx, in_not, specific);
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
                Log.Unimplemented();
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
            if (data[k+0] == '-')
            {
                sign = -1;
                len--;
                k++;
            }
            else if (data[k+0] == '+')
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
                if (data[k+0] == '.')
                {
                    if (len == 1 || data[k+1] < '0' || '9' < data[k+1])
                    {
                        consumed = 0;
                        return new Fixed(0);
                    }
                }
                else if (data[k+0] < '0' || '9' < data[k+0])
                {
                    consumed = 0;
                    return new Fixed(0);
                }
            }

            // Now extract intpart, assuming base 10
            while (len > 0)
            {
                // Stop on first non-digit
                if (data[k+0] < '0' || '9' < data[k+0])
                    break;

                // Prevent overflow of 'intpart'; proper clamping below
                if (intpart < (1 << 22))
                {
                    intpart *= 10;
                    intpart += data[k+0] - '0';
                }
                k++;
                len--;
            }

            // And fracpart, again, assuming base 10
            if (intOnly == false && len > 1 && data[k+0] == '.' &&
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
        public static CssStatus Parse_border_side(List<CssToken> tokens, ref int index, CssStyle style, BorderSide side)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        // parse/properties/utils.c:368
        public static void ParseProperty_ColourSpecifier(List<CssToken> tokens, ref int index, out ushort value, out uint result)
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

                if (!quirksAllowed ||
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

                error = ParseProperty_NamedColour(token.iData, ref result);
                if (error != CssStatus.CSS_OK && quirksAllowed)
                {
                    error = error = ParseHashColour(token.iData, out result);
                    if (error == CssStatus.CSS_OK)
                        quirksUsed = true;
                }

                if (error != CssStatus.CSS_OK)
                {
                    Console.WriteLine("Invalid CSS 764");
                    return;
                }
            }
            else if (token.Type == CssTokenType.CSS_TOKEN_HASH)
            {
                error = ParseHashColour(token.iData, out result);
                if (error != CssStatus.CSS_OK)
                {
                    index = origIndex;
                    return;
                    //goto invalid;
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
                    return;
                    //goto invalid;
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
                    return;
                    //goto invalid;
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

            if (index >= tokens.Count - 1) return CssStatus.CSS_INVALID;
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

                    ConsumeWhitespace(tokens,  ref tempIndex);

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
                if (String.Equals(ptr, "grad", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.GRAD;
                else if (String.Equals(ptr, "turn", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.TURN;
                else if (String.Equals(ptr, "dppx", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.DPPX;
                else if (String.Equals(ptr, "dpcm", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.DPCM;
                else if (String.Equals(ptr, "vmin", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VMIN;
                else if (String.Equals(ptr, "vmax", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VMAX;
                else
                    return CssStatus.CSS_INVALID;
            }
            else if (len == 3)
            {
                if (String.Equals(ptr, "kHz", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.KHZ;
                else if (String.Equals(ptr, "deg", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.DEG;
                else if (String.Equals(ptr, "rad", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.RAD;
                else if (String.Equals(ptr, "rem", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.REM;
                else if (String.Equals(ptr, "dpi", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.DPI;
                else
                    return CssStatus.CSS_INVALID;
            }
            else if (len == 2)
            {
                if (String.Equals(ptr, "Hz", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.HZ;
                else if (String.Equals(ptr, "ms", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.MS;
                else if (String.Equals(ptr, "px", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.PX;
                else if (String.Equals(ptr, "ex", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.EX;
                else if (String.Equals(ptr, "em", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.EM;
                else if (String.Equals(ptr, "in", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.IN;
                else if (String.Equals(ptr, "cm", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.CM;
                else if (String.Equals(ptr, "mm", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.MM;
                else if (String.Equals(ptr, "pt", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.PT;
                else if (String.Equals(ptr, "pc", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.PC;
                else if (String.Equals(ptr, "ch", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.CH;
                else if (String.Equals(ptr, "lh", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.LH;
                else if (String.Equals(ptr, "vh", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VH;
                else if (String.Equals(ptr, "vw", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VW;
                else if (String.Equals(ptr, "vi", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VI;
                else if (String.Equals(ptr, "vb", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.VB;
                else
                    return CssStatus.CSS_INVALID;
            }
            else if (len == 1)
            {
                if (String.Equals(ptr, "s", StringComparison.OrdinalIgnoreCase))
                    unit = OpcodeUnit.S;
                else if (String.Equals(ptr, "q", StringComparison.OrdinalIgnoreCase))
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
