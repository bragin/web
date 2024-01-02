namespace SkiaSharpOpenGLBenchmark.css
{
    public enum OpColor : ushort
    {
        COLOR_TRANSPARENT = 0x0000,
        COLOR_CURRENT_COLOR = 0x0001,
        COLOR_SET = 0x0080
    }

    public enum OpCodeFlag : byte
    {
        FLAG_IMPORTANT = (1 << 0),
        FLAG_INHERIT = (1 << 1)
    }

    // same as opcode_t (bytecode.h:19)
    public enum CssPropertiesEnum
    {
        CSS_PROP_AZIMUTH = 0x000,
        CSS_PROP_BACKGROUND_ATTACHMENT = 0x001,
        CSS_PROP_BACKGROUND_COLOR = 0x002,
        CSS_PROP_BACKGROUND_IMAGE = 0x003,
        CSS_PROP_BACKGROUND_POSITION = 0x004,
        CSS_PROP_BACKGROUND_REPEAT = 0x005,
        CSS_PROP_BORDER_COLLAPSE = 0x006,
        CSS_PROP_BORDER_SPACING = 0x007,
        CSS_PROP_BORDER_TOP_COLOR = 0x008,
        CSS_PROP_BORDER_RIGHT_COLOR = 0x009,
        CSS_PROP_BORDER_BOTTOM_COLOR = 0x00a,
        CSS_PROP_BORDER_LEFT_COLOR = 0x00b,
        CSS_PROP_BORDER_TOP_STYLE = 0x00c,
        CSS_PROP_BORDER_RIGHT_STYLE = 0x00d,
        CSS_PROP_BORDER_BOTTOM_STYLE = 0x00e,
        CSS_PROP_BORDER_LEFT_STYLE = 0x00f,
        CSS_PROP_BORDER_TOP_WIDTH = 0x010,
        CSS_PROP_BORDER_RIGHT_WIDTH = 0x011,
        CSS_PROP_BORDER_BOTTOM_WIDTH = 0x012,
        CSS_PROP_BORDER_LEFT_WIDTH = 0x013,
        CSS_PROP_BOTTOM = 0x014,
        CSS_PROP_CAPTION_SIDE = 0x015,
        CSS_PROP_CLEAR = 0x016,
        CSS_PROP_CLIP = 0x017,
        CSS_PROP_COLOR = 0x018,
        CSS_PROP_CONTENT = 0x019,
        CSS_PROP_COUNTER_INCREMENT = 0x01a,
        CSS_PROP_COUNTER_RESET = 0x01b,
        CSS_PROP_CUE_AFTER = 0x01c,
        CSS_PROP_CUE_BEFORE = 0x01d,
        CSS_PROP_CURSOR = 0x01e,
        CSS_PROP_DIRECTION = 0x01f,
        CSS_PROP_DISPLAY = 0x020,
        CSS_PROP_ELEVATION = 0x021,
        CSS_PROP_EMPTY_CELLS = 0x022,
        CSS_PROP_FLOAT = 0x023,
        CSS_PROP_FONT_FAMILY = 0x024,
        CSS_PROP_FONT_SIZE = 0x025,
        CSS_PROP_FONT_STYLE = 0x026,
        CSS_PROP_FONT_VARIANT = 0x027,
        CSS_PROP_FONT_WEIGHT = 0x028,
        CSS_PROP_HEIGHT = 0x029,
        CSS_PROP_LEFT = 0x02a,
        CSS_PROP_LETTER_SPACING = 0x02b,
        CSS_PROP_LINE_HEIGHT = 0x02c,
        CSS_PROP_LIST_STYLE_IMAGE = 0x02d,
        CSS_PROP_LIST_STYLE_POSITION = 0x02e,
        CSS_PROP_LIST_STYLE_TYPE = 0x02f,
        CSS_PROP_MARGIN_TOP = 0x030,
        CSS_PROP_MARGIN_RIGHT = 0x031,
        CSS_PROP_MARGIN_BOTTOM = 0x032,
        CSS_PROP_MARGIN_LEFT = 0x033,
        CSS_PROP_MAX_HEIGHT = 0x034,
        CSS_PROP_MAX_WIDTH = 0x035,
        CSS_PROP_MIN_HEIGHT = 0x036,
        CSS_PROP_MIN_WIDTH = 0x037,
        CSS_PROP_ORPHANS = 0x038,
        CSS_PROP_OUTLINE_COLOR = 0x039,
        CSS_PROP_OUTLINE_STYLE = 0x03a,
        CSS_PROP_OUTLINE_WIDTH = 0x03b,
        CSS_PROP_OVERFLOW_X = 0x03c,
        CSS_PROP_PADDING_TOP = 0x03d,
        CSS_PROP_PADDING_RIGHT = 0x03e,
        CSS_PROP_PADDING_BOTTOM = 0x03f,
        CSS_PROP_PADDING_LEFT = 0x040,
        CSS_PROP_PAGE_BREAK_AFTER = 0x041,
        CSS_PROP_PAGE_BREAK_BEFORE = 0x042,
        CSS_PROP_PAGE_BREAK_INSIDE = 0x043,
        CSS_PROP_PAUSE_AFTER = 0x044,
        CSS_PROP_PAUSE_BEFORE = 0x045,
        CSS_PROP_PITCH_RANGE = 0x046,
        CSS_PROP_PITCH = 0x047,
        CSS_PROP_PLAY_DURING = 0x048,
        CSS_PROP_POSITION = 0x049,
        CSS_PROP_QUOTES = 0x04a,
        CSS_PROP_RICHNESS = 0x04b,
        CSS_PROP_RIGHT = 0x04c,
        CSS_PROP_SPEAK_HEADER = 0x04d,
        CSS_PROP_SPEAK_NUMERAL = 0x04e,
        CSS_PROP_SPEAK_PUNCTUATION = 0x04f,
        CSS_PROP_SPEAK = 0x050,
        CSS_PROP_SPEECH_RATE = 0x051,
        CSS_PROP_STRESS = 0x052,
        CSS_PROP_TABLE_LAYOUT = 0x053,
        CSS_PROP_TEXT_ALIGN = 0x054,
        CSS_PROP_TEXT_DECORATION = 0x055,
        CSS_PROP_TEXT_INDENT = 0x056,
        CSS_PROP_TEXT_TRANSFORM = 0x057,
        CSS_PROP_TOP = 0x058,
        CSS_PROP_UNICODE_BIDI = 0x059,
        CSS_PROP_VERTICAL_ALIGN = 0x05a,
        CSS_PROP_VISIBILITY = 0x05b,
        CSS_PROP_VOICE_FAMILY = 0x05c,
        CSS_PROP_VOLUME = 0x05d,
        CSS_PROP_WHITE_SPACE = 0x05e,
        CSS_PROP_WIDOWS = 0x05f,
        CSS_PROP_WIDTH = 0x060,
        CSS_PROP_WORD_SPACING = 0x061,
        CSS_PROP_Z_INDEX = 0x062,
        CSS_PROP_OPACITY = 0x063,
        CSS_PROP_BREAK_AFTER = 0x064,
        CSS_PROP_BREAK_BEFORE = 0x065,
        CSS_PROP_BREAK_INSIDE = 0x066,
        CSS_PROP_COLUMN_COUNT = 0x067,
        CSS_PROP_COLUMN_FILL = 0x068,
        CSS_PROP_COLUMN_GAP = 0x069,
        CSS_PROP_COLUMN_RULE_COLOR = 0x06a,
        CSS_PROP_COLUMN_RULE_STYLE = 0x06b,
        CSS_PROP_COLUMN_RULE_WIDTH = 0x06c,
        CSS_PROP_COLUMN_SPAN = 0x06d,
        CSS_PROP_COLUMN_WIDTH = 0x06e,
        CSS_PROP_WRITING_MODE = 0x06f,
        CSS_PROP_OVERFLOW_Y = 0x070,
        CSS_PROP_BOX_SIZING = 0x071,
        CSS_PROP_ALIGN_CONTENT = 0x072,
        CSS_PROP_ALIGN_ITEMS = 0x073,
        CSS_PROP_ALIGN_SELF = 0x074,
        CSS_PROP_FLEX_BASIS = 0x075,
        CSS_PROP_FLEX_DIRECTION = 0x076,
        CSS_PROP_FLEX_GROW = 0x077,
        CSS_PROP_FLEX_SHRINK = 0x078,
        CSS_PROP_FLEX_WRAP = 0x079,
        CSS_PROP_JUSTIFY_CONTENT = 0x07a,
        CSS_PROP_ORDER = 0x07b,

        CSS_N_PROPERTIES
    }

    public struct OpCode
    {
        uint OPV;

        public OpCode(ushort opcode, byte flags, ushort value)
        {
            OPV = (uint)((opcode & 0x3ff) | (flags << 10) | ((value & 0x3fff) << 18));
        }

        public OpCode(uint raw)
        {
            OPV = raw;
        }

        public uint GetOPV()
        {
            return OPV;
        }

        public CssPropertiesEnum GetOpcode()
        {
            return (CssPropertiesEnum)(OPV & 0x3ff);
        }

        public byte GetFlags()
        {
            return (byte)((OPV >> 10) & 0xff);
        }

        public ushort GetValue()
        {
            return (ushort)(OPV >> 18);
        }

        public bool IsImportant()
        {
            return (GetFlags() & 0x1) != 0 ? true : false;
        }

        public bool IsInherit()
        {
            return (GetFlags() & 0x2) != 0 ? true : false;
        }
    }
}