﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SkiaSharpOpenGLBenchmark.css
{
    // properties.h:146
    public enum CssAlignContentEnum : byte
    {
        CSS_ALIGN_CONTENT_INHERIT = 0x0,
        CSS_ALIGN_CONTENT_STRETCH = 0x1,
        CSS_ALIGN_CONTENT_FLEX_START = 0x2,
        CSS_ALIGN_CONTENT_FLEX_END = 0x3,
        CSS_ALIGN_CONTENT_CENTER = 0x4,
        CSS_ALIGN_CONTENT_SPACE_BETWEEN = 0x5,
        CSS_ALIGN_CONTENT_SPACE_AROUND = 0x6,
        CSS_ALIGN_CONTENT_SPACE_EVENLY = 0x7
    }

    public enum CssAlignItemsEnum : byte
    {
        CSS_ALIGN_ITEMS_INHERIT = 0x0,
        CSS_ALIGN_ITEMS_STRETCH = 0x1,
        CSS_ALIGN_ITEMS_FLEX_START = 0x2,
        CSS_ALIGN_ITEMS_FLEX_END = 0x3,
        CSS_ALIGN_ITEMS_CENTER = 0x4,
        CSS_ALIGN_ITEMS_BASELINE = 0x5
    }

    public enum CssAlignSelfEnum : byte
    {
        CSS_ALIGN_SELF_INHERIT = 0x0,
        CSS_ALIGN_SELF_STRETCH = 0x1,
        CSS_ALIGN_SELF_FLEX_START = 0x2,
        CSS_ALIGN_SELF_FLEX_END = 0x3,
        CSS_ALIGN_SELF_CENTER = 0x4,
        CSS_ALIGN_SELF_BASELINE = 0x5,
        CSS_ALIGN_SELF_AUTO = 0x6
    }

    // properties.h:175
    public enum CssBackgroundAttachment : byte
    {
        CSS_BACKGROUND_ATTACHMENT_INHERIT = 0x0,
        CSS_BACKGROUND_ATTACHMENT_FIXED = 0x1,
        CSS_BACKGROUND_ATTACHMENT_SCROLL = 0x2
    }

    // properties.h:181
    public enum CssBackgroundColorEnum : byte
    {
        CSS_BACKGROUND_COLOR_INHERIT = 0x0,
        CSS_BACKGROUND_COLOR_COLOR = 0x1,
        CSS_BACKGROUND_COLOR_CURRENT_COLOR = 0x2
    }

    // properties.h:187
    public enum CssBackgroundImageEnum : byte
    {
        CSS_BACKGROUND_IMAGE_INHERIT = 0x0,
        /* Consult pointer in struct to determine which */
        CSS_BACKGROUND_IMAGE_NONE = 0x1,
        CSS_BACKGROUND_IMAGE_IMAGE = 0x1
    }

    // properties.h:194
    public enum CssBackgroundPositionEnum : byte
    {
        CSS_BACKGROUND_POSITION_INHERIT = 0x0,
        CSS_BACKGROUND_POSITION_SET = 0x1
    }

    // properties.h:199
    public enum CssBackgroundRepeat : byte
    {
        CSS_BACKGROUND_REPEAT_INHERIT = 0x0,
        CSS_BACKGROUND_REPEAT_REPEAT_X = 0x1,
        CSS_BACKGROUND_REPEAT_REPEAT_Y = 0x2,
        CSS_BACKGROUND_REPEAT_REPEAT = 0x3,
        CSS_BACKGROUND_REPEAT_NO_REPEAT = 0x4
    }

    public enum CssBorderCollapseEnum : byte
    {
        CSS_BORDER_COLLAPSE_INHERIT = 0x0,
        CSS_BORDER_COLLAPSE_SEPARATE = 0x1,
        CSS_BORDER_COLLAPSE_COLLAPSE = 0x2
    };

    public enum CssBorderSpacingEnum : byte
    {
        CSS_BORDER_SPACING_INHERIT = 0x0,
        CSS_BORDER_SPACING_SET = 0x1
    };

    public enum CssBorderColorEnum : byte
    {
        CSS_BORDER_COLOR_INHERIT = CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_INHERIT,
        CSS_BORDER_COLOR_COLOR = CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_COLOR,
        CSS_BORDER_COLOR_CURRENT_COLOR = CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_CURRENT_COLOR
    };

    public enum CssBorderStyleEnum : byte
    {
        CSS_BORDER_STYLE_INHERIT = 0x0,
        CSS_BORDER_STYLE_NONE = 0x1,
        CSS_BORDER_STYLE_HIDDEN = 0x2,
        CSS_BORDER_STYLE_DOTTED = 0x3,
        CSS_BORDER_STYLE_DASHED = 0x4,
        CSS_BORDER_STYLE_SOLID = 0x5,
        CSS_BORDER_STYLE_DOUBLE = 0x6,
        CSS_BORDER_STYLE_GROOVE = 0x7,
        CSS_BORDER_STYLE_RIDGE = 0x8,
        CSS_BORDER_STYLE_INSET = 0x9,
        CSS_BORDER_STYLE_OUTSET = 0xa
    };

    public enum CssBorderWidthEnum : byte
    {
        CSS_BORDER_WIDTH_INHERIT = 0x0,
        CSS_BORDER_WIDTH_THIN = 0x1,
        CSS_BORDER_WIDTH_MEDIUM = 0x2,
        CSS_BORDER_WIDTH_THICK = 0x3,
        CSS_BORDER_WIDTH_WIDTH = 0x4
    };

    public enum CssBottomEnum : byte
    {
        CSS_BOTTOM_INHERIT = 0x0,
        CSS_BOTTOM_SET = 0x1,
        CSS_BOTTOM_AUTO = 0x2
    };

    public enum CssBoxSizingEnum : byte
    {
        CSS_BOX_SIZING_INHERIT = 0x0,
        CSS_BOX_SIZING_CONTENT_BOX = 0x1,
        CSS_BOX_SIZING_BORDER_BOX = 0x2
    };

    public enum CssBreakAfterEnum : byte
    {
        CSS_BREAK_AFTER_INHERIT = 0x0,
        CSS_BREAK_AFTER_AUTO = 0x1,
        CSS_BREAK_AFTER_AVOID = 0x2,
        CSS_BREAK_AFTER_ALWAYS = 0x3,
        CSS_BREAK_AFTER_LEFT = 0x4,
        CSS_BREAK_AFTER_RIGHT = 0x5,
        CSS_BREAK_AFTER_PAGE = 0x6,
        CSS_BREAK_AFTER_COLUMN = 0x7,
        CSS_BREAK_AFTER_AVOID_PAGE = 0x8,
        CSS_BREAK_AFTER_AVOID_COLUMN = 0x9
    }

    public enum CssBreakBeforeEnum : byte
    {
        CSS_BREAK_BEFORE_INHERIT = CssBreakAfterEnum.CSS_BREAK_AFTER_INHERIT,
        CSS_BREAK_BEFORE_AUTO = CssBreakAfterEnum.CSS_BREAK_AFTER_AUTO,
        CSS_BREAK_BEFORE_AVOID = CssBreakAfterEnum.CSS_BREAK_AFTER_AVOID,
        CSS_BREAK_BEFORE_ALWAYS = CssBreakAfterEnum.CSS_BREAK_AFTER_ALWAYS,
        CSS_BREAK_BEFORE_LEFT = CssBreakAfterEnum.CSS_BREAK_AFTER_LEFT,
        CSS_BREAK_BEFORE_RIGHT = CssBreakAfterEnum.CSS_BREAK_AFTER_RIGHT,
        CSS_BREAK_BEFORE_PAGE = CssBreakAfterEnum.CSS_BREAK_AFTER_PAGE,
        CSS_BREAK_BEFORE_COLUMN = CssBreakAfterEnum.CSS_BREAK_AFTER_COLUMN,
        CSS_BREAK_BEFORE_AVOID_PAGE = CssBreakAfterEnum.CSS_BREAK_AFTER_AVOID_PAGE,
        CSS_BREAK_BEFORE_AVOID_COLUMN = CssBreakAfterEnum.CSS_BREAK_AFTER_AVOID_COLUMN
    }

    public enum CssBreakInsideEnum : byte
    {
        CSS_BREAK_INSIDE_INHERIT = CssBreakAfterEnum.CSS_BREAK_AFTER_INHERIT,
        CSS_BREAK_INSIDE_AUTO = CssBreakAfterEnum.CSS_BREAK_AFTER_AUTO,
        CSS_BREAK_INSIDE_AVOID = CssBreakAfterEnum.CSS_BREAK_AFTER_AVOID,
        CSS_BREAK_INSIDE_AVOID_PAGE = CssBreakAfterEnum.CSS_BREAK_AFTER_AVOID_PAGE,
        CSS_BREAK_INSIDE_AVOID_COLUMN = CssBreakAfterEnum.CSS_BREAK_AFTER_AVOID_COLUMN
    }

    public enum CssCaptionSideEnum : byte
    {
        CSS_CAPTION_SIDE_INHERIT = 0x0,
        CSS_CAPTION_SIDE_TOP = 0x1,
        CSS_CAPTION_SIDE_BOTTOM = 0x2
    }

    public enum CssClearEnum : byte
    {
        CSS_CLEAR_INHERIT = 0x0,
        CSS_CLEAR_NONE = 0x1,
        CSS_CLEAR_LEFT = 0x2,
        CSS_CLEAR_RIGHT = 0x3,
        CSS_CLEAR_BOTH = 0x4
    }

    public enum CssClipEnum : byte
    {
        CSS_CLIP_INHERIT = 0x0,
        CSS_CLIP_AUTO = 0x1,
        CSS_CLIP_RECT = 0x2
    }

    // properties.h:312
    public enum CssColorEnum : byte
    {
        CSS_COLOR_INHERIT = 0x0,
        CSS_COLOR_COLOR = 0x1
    }

    public enum CssColumnCountEnum : byte
    {
        CSS_COLUMN_COUNT_INHERIT = 0x0,
        CSS_COLUMN_COUNT_AUTO = 0x1,
        CSS_COLUMN_COUNT_SET = 0x2
    }

    public enum CssColumnFillEnum : byte
    {
        CSS_COLUMN_FILL_INHERIT = 0x0,
        CSS_COLUMN_FILL_BALANCE = 0x1,
        CSS_COLUMN_FILL_AUTO = 0x2
    }

    public enum CssColumnGapEnum : byte
    {
        CSS_COLUMN_GAP_INHERIT = 0x0,
        CSS_COLUMN_GAP_SET = 0x1,
        CSS_COLUMN_GAP_NORMAL = 0x2
    }

    public enum CssColumnRuleColorEnum : byte
    {
        CSS_COLUMN_RULE_COLOR_INHERIT = CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_INHERIT,
        CSS_COLUMN_RULE_COLOR_COLOR = CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_COLOR,
        CSS_COLUMN_RULE_COLOR_CURRENT_COLOR = CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_CURRENT_COLOR
    }

    public enum CssColumnRuleStyleEnum : byte
    {
        CSS_COLUMN_RULE_STYLE_INHERIT = CssBorderStyleEnum.CSS_BORDER_STYLE_INHERIT,
        CSS_COLUMN_RULE_STYLE_NONE = CssBorderStyleEnum.CSS_BORDER_STYLE_NONE,
        CSS_COLUMN_RULE_STYLE_HIDDEN = CssBorderStyleEnum.CSS_BORDER_STYLE_HIDDEN,
        CSS_COLUMN_RULE_STYLE_DOTTED = CssBorderStyleEnum.CSS_BORDER_STYLE_DOTTED,
        CSS_COLUMN_RULE_STYLE_DASHED = CssBorderStyleEnum.CSS_BORDER_STYLE_DASHED,
        CSS_COLUMN_RULE_STYLE_SOLID = CssBorderStyleEnum.CSS_BORDER_STYLE_SOLID,
        CSS_COLUMN_RULE_STYLE_DOUBLE = CssBorderStyleEnum.CSS_BORDER_STYLE_DOUBLE,
        CSS_COLUMN_RULE_STYLE_GROOVE = CssBorderStyleEnum.CSS_BORDER_STYLE_GROOVE,
        CSS_COLUMN_RULE_STYLE_RIDGE = CssBorderStyleEnum.CSS_BORDER_STYLE_RIDGE,
        CSS_COLUMN_RULE_STYLE_INSET = CssBorderStyleEnum.CSS_BORDER_STYLE_INSET,
        CSS_COLUMN_RULE_STYLE_OUTSET = CssBorderStyleEnum.CSS_BORDER_STYLE_OUTSET
    }

    public enum CssColumnRuleWidthEnum : byte
    {
        CSS_COLUMN_RULE_WIDTH_INHERIT = CssBorderWidthEnum.CSS_BORDER_WIDTH_INHERIT,
        CSS_COLUMN_RULE_WIDTH_THIN = CssBorderWidthEnum.CSS_BORDER_WIDTH_THIN,
        CSS_COLUMN_RULE_WIDTH_MEDIUM = CssBorderWidthEnum.CSS_BORDER_WIDTH_MEDIUM,
        CSS_COLUMN_RULE_WIDTH_THICK = CssBorderWidthEnum.CSS_BORDER_WIDTH_THICK,
        CSS_COLUMN_RULE_WIDTH_WIDTH = CssBorderWidthEnum.CSS_BORDER_WIDTH_WIDTH
    }

    public enum CssColumnSpanEnum : byte
    {
        CSS_COLUMN_SPAN_INHERIT = 0x0,
        CSS_COLUMN_SPAN_NONE = 0x1,
        CSS_COLUMN_SPAN_ALL = 0x2
    }

    public enum CssColumnWidthEnum : byte
    {
        CSS_COLUMN_WIDTH_INHERIT = 0x0,
        CSS_COLUMN_WIDTH_SET = 0x1,
        CSS_COLUMN_WIDTH_AUTO = 0x2
    }

    // properties.h:375
    public enum CssContent : byte
    {
        CSS_CONTENT_INHERIT = 0x0,
        CSS_CONTENT_NONE = 0x1,
        CSS_CONTENT_NORMAL = 0x2,
        CSS_CONTENT_SET = 0x3
    }

    public enum CssCounterIncrementEnum : byte
    {
        CSS_COUNTER_INCREMENT_INHERIT = 0x0,
        /* Consult pointer in struct to determine which */
        CSS_COUNTER_INCREMENT_NAMED = 0x1,
        CSS_COUNTER_INCREMENT_NONE = 0x1
    }

    public enum CssCounterResetEnum : byte
    {
        CSS_COUNTER_RESET_INHERIT = 0x0,
        /* Consult pointer in struct to determine which */
        CSS_COUNTER_RESET_NAMED = 0x1,
        CSS_COUNTER_RESET_NONE = 0x1
    }

    public enum CssCursorEnum : byte
    {
        CSS_CURSOR_INHERIT = 0x00,
        /* URLs exist if pointer is non-NULL */
        CSS_CURSOR_AUTO = 0x01,
        CSS_CURSOR_CROSSHAIR = 0x02,
        CSS_CURSOR_DEFAULT = 0x03,
        CSS_CURSOR_POINTER = 0x04,
        CSS_CURSOR_MOVE = 0x05,
        CSS_CURSOR_E_RESIZE = 0x06,
        CSS_CURSOR_NE_RESIZE = 0x07,
        CSS_CURSOR_NW_RESIZE = 0x08,
        CSS_CURSOR_N_RESIZE = 0x09,
        CSS_CURSOR_SE_RESIZE = 0x0a,
        CSS_CURSOR_SW_RESIZE = 0x0b,
        CSS_CURSOR_S_RESIZE = 0x0c,
        CSS_CURSOR_W_RESIZE = 0x0d,
        CSS_CURSOR_TEXT = 0x0e,
        CSS_CURSOR_WAIT = 0x0f,
        CSS_CURSOR_HELP = 0x10,
        CSS_CURSOR_PROGRESS = 0x11
    };

    public enum CssDirectionEnum : byte
    {
        CSS_DIRECTION_INHERIT = 0x0,
        CSS_DIRECTION_LTR = 0x1,
        CSS_DIRECTION_RTL = 0x2
    }

    // properties.h:424
    public enum CssDisplay : byte
    {
        CSS_DISPLAY_INHERIT = 0x00,
        CSS_DISPLAY_INLINE = 0x01,
        CSS_DISPLAY_BLOCK = 0x02,
        CSS_DISPLAY_LIST_ITEM = 0x03,
        CSS_DISPLAY_RUN_IN = 0x04,
        CSS_DISPLAY_INLINE_BLOCK = 0x05,
        CSS_DISPLAY_TABLE = 0x06,
        CSS_DISPLAY_INLINE_TABLE = 0x07,
        CSS_DISPLAY_TABLE_ROW_GROUP = 0x08,
        CSS_DISPLAY_TABLE_HEADER_GROUP = 0x09,
        CSS_DISPLAY_TABLE_FOOTER_GROUP = 0x0a,
        CSS_DISPLAY_TABLE_ROW = 0x0b,
        CSS_DISPLAY_TABLE_COLUMN_GROUP = 0x0c,
        CSS_DISPLAY_TABLE_COLUMN = 0x0d,
        CSS_DISPLAY_TABLE_CELL = 0x0e,
        CSS_DISPLAY_TABLE_CAPTION = 0x0f,
        CSS_DISPLAY_NONE = 0x10,
        CSS_DISPLAY_FLEX = 0x11,
        CSS_DISPLAY_INLINE_FLEX = 0x12
    };

    public enum CssEmptyCellsEnum : byte
    {
        CSS_EMPTY_CELLS_INHERIT = 0x0,
        CSS_EMPTY_CELLS_SHOW = 0x1,
        CSS_EMPTY_CELLS_HIDE = 0x2
    };

    public enum CssFlexBasisEnum : byte
    {
        CSS_FLEX_BASIS_INHERIT = 0x0,
        CSS_FLEX_BASIS_SET = 0x1,
        CSS_FLEX_BASIS_AUTO = 0x2,
        CSS_FLEX_BASIS_CONTENT = 0x3
    };

    public enum CssFlexDirectionEnum : byte
    {
        CSS_FLEX_DIRECTION_INHERIT = 0x0,
        CSS_FLEX_DIRECTION_ROW = 0x1,
        CSS_FLEX_DIRECTION_ROW_REVERSE = 0x2,
        CSS_FLEX_DIRECTION_COLUMN = 0x3,
        CSS_FLEX_DIRECTION_COLUMN_REVERSE = 0x4
    };

    public enum CssFlexGrowEnum : byte
    {
        CSS_FLEX_GROW_INHERIT = 0x0,
        CSS_FLEX_GROW_SET = 0x1
    };

    public enum CssFlexShrinkEnum : byte
    {
        CSS_FLEX_SHRINK_INHERIT = 0x0,
        CSS_FLEX_SHRINK_SET = 0x1
    };

    public enum CssFlexWrapEnum : byte
    {
        CSS_FLEX_WRAP_INHERIT = 0x0,
        CSS_FLEX_WRAP_NOWRAP = 0x1,
        CSS_FLEX_WRAP_WRAP = 0x2,
        CSS_FLEX_WRAP_WRAP_REVERSE = 0x3
    };

    // properties.h:484
    public enum CssFloat : byte
    {
        CSS_FLOAT_INHERIT = 0x0,
        CSS_FLOAT_LEFT = 0x1,
        CSS_FLOAT_RIGHT = 0x2,
        CSS_FLOAT_NONE = 0x3
    }

    // properties.h:491
    public enum CssFontFamilyEnum : byte
    {
        CSS_FONT_FAMILY_INHERIT = 0x0,
        /* Named fonts exist if pointer is non-NULL */
        CSS_FONT_FAMILY_SERIF = 0x1,
        CSS_FONT_FAMILY_SANS_SERIF = 0x2,
        CSS_FONT_FAMILY_CURSIVE = 0x3,
        CSS_FONT_FAMILY_FANTASY = 0x4,
        CSS_FONT_FAMILY_MONOSPACE = 0x5
    }

    public enum CssFontSizeEnum : byte
    {
        CSS_FONT_SIZE_INHERIT = 0x0,
        CSS_FONT_SIZE_XX_SMALL = 0x1,
        CSS_FONT_SIZE_X_SMALL = 0x2,
        CSS_FONT_SIZE_SMALL = 0x3,
        CSS_FONT_SIZE_MEDIUM = 0x4,
        CSS_FONT_SIZE_LARGE = 0x5,
        CSS_FONT_SIZE_X_LARGE = 0x6,
        CSS_FONT_SIZE_XX_LARGE = 0x7,
        CSS_FONT_SIZE_LARGER = 0x8,
        CSS_FONT_SIZE_SMALLER = 0x9,
        CSS_FONT_SIZE_DIMENSION = 0xa
    }

    public enum CssFontStyleEnum : byte
    {
        CSS_FONT_STYLE_INHERIT = 0x0,
        CSS_FONT_STYLE_NORMAL = 0x1,
        CSS_FONT_STYLE_ITALIC = 0x2,
        CSS_FONT_STYLE_OBLIQUE = 0x3
    };

    public enum CssFontVariantEnum : byte
    {
        CSS_FONT_VARIANT_INHERIT = 0x0,
        CSS_FONT_VARIANT_NORMAL = 0x1,
        CSS_FONT_VARIANT_SMALL_CAPS = 0x2
    };

    public enum CssFontWeightEnum : byte
    {
        CSS_FONT_WEIGHT_INHERIT = 0x0,
        CSS_FONT_WEIGHT_NORMAL = 0x1,
        CSS_FONT_WEIGHT_BOLD = 0x2,
        CSS_FONT_WEIGHT_BOLDER = 0x3,
        CSS_FONT_WEIGHT_LIGHTER = 0x4,
        CSS_FONT_WEIGHT_100 = 0x5,
        CSS_FONT_WEIGHT_200 = 0x6,
        CSS_FONT_WEIGHT_300 = 0x7,
        CSS_FONT_WEIGHT_400 = 0x8,
        CSS_FONT_WEIGHT_500 = 0x9,
        CSS_FONT_WEIGHT_600 = 0xa,
        CSS_FONT_WEIGHT_700 = 0xb,
        CSS_FONT_WEIGHT_800 = 0xc,
        CSS_FONT_WEIGHT_900 = 0xd
    };

    public enum CssHeightEnum : byte
    {
        CSS_HEIGHT_INHERIT = 0x0,
        CSS_HEIGHT_SET = 0x1,
        CSS_HEIGHT_AUTO = 0x2
    };

    public enum CssJustifyContentEnum : byte
    {
        CSS_JUSTIFY_CONTENT_INHERIT = 0x0,
        CSS_JUSTIFY_CONTENT_FLEX_START = 0x1,
        CSS_JUSTIFY_CONTENT_FLEX_END = 0x2,
        CSS_JUSTIFY_CONTENT_CENTER = 0x3,
        CSS_JUSTIFY_CONTENT_SPACE_BETWEEN = 0x4,
        CSS_JUSTIFY_CONTENT_SPACE_AROUND = 0x5,
        CSS_JUSTIFY_CONTENT_SPACE_EVENLY = 0x6
    };

    public enum CssLeftEnum : byte
    {
        CSS_LEFT_INHERIT = 0x0,
        CSS_LEFT_SET = 0x1,
        CSS_LEFT_AUTO = 0x2
    };

    public enum CssLetterSpacingEnum : byte
    {
        CSS_LETTER_SPACING_INHERIT = CssColumnGapEnum.CSS_COLUMN_GAP_INHERIT,
        CSS_LETTER_SPACING_SET = CssColumnGapEnum.CSS_COLUMN_GAP_SET,
        CSS_LETTER_SPACING_NORMAL = CssColumnGapEnum.CSS_COLUMN_GAP_NORMAL
    };

    public enum CssLineHeightEnum : byte
    {
        CSS_LINE_HEIGHT_INHERIT = 0x0,
        CSS_LINE_HEIGHT_NUMBER = 0x1,
        CSS_LINE_HEIGHT_DIMENSION = 0x2,
        CSS_LINE_HEIGHT_NORMAL = 0x3
    };

    public enum CssListStyleImageEnum : byte
    {
        CSS_LIST_STYLE_IMAGE_INHERIT = 0x0,
        /* Consult pointer in struct to determine which */
        CSS_LIST_STYLE_IMAGE_URI = 0x1,
        CSS_LIST_STYLE_IMAGE_NONE = 0x1
    };

    public enum CssListStylePositionEnum : byte
    {
        CSS_LIST_STYLE_POSITION_INHERIT = 0x0,
        CSS_LIST_STYLE_POSITION_INSIDE = 0x1,
        CSS_LIST_STYLE_POSITION_OUTSIDE = 0x2
    };

    public enum CssListStyleTypeEnum : byte
    {
        CSS_LIST_STYLE_TYPE_INHERIT = 0x0,
        CSS_LIST_STYLE_TYPE_DISC = 0x1,
        CSS_LIST_STYLE_TYPE_CIRCLE = 0x2,
        CSS_LIST_STYLE_TYPE_SQUARE = 0x3,
        CSS_LIST_STYLE_TYPE_DECIMAL = 0x4,
        CSS_LIST_STYLE_TYPE_DECIMAL_LEADING_ZERO = 0x5,
        CSS_LIST_STYLE_TYPE_LOWER_ROMAN = 0x6,
        CSS_LIST_STYLE_TYPE_UPPER_ROMAN = 0x7,
        CSS_LIST_STYLE_TYPE_LOWER_GREEK = 0x8,
        CSS_LIST_STYLE_TYPE_LOWER_LATIN = 0x9,
        CSS_LIST_STYLE_TYPE_UPPER_LATIN = 0xa,
        CSS_LIST_STYLE_TYPE_ARMENIAN = 0xb,
        CSS_LIST_STYLE_TYPE_GEORGIAN = 0xc,
        CSS_LIST_STYLE_TYPE_LOWER_ALPHA = 0xd,
        CSS_LIST_STYLE_TYPE_UPPER_ALPHA = 0xe,
        CSS_LIST_STYLE_TYPE_NONE = 0xf,
        CSS_LIST_STYLE_TYPE_BINARY = 0x10,
        CSS_LIST_STYLE_TYPE_OCTAL = 0x11,
        CSS_LIST_STYLE_TYPE_LOWER_HEXADECIMAL = 0x12,
        CSS_LIST_STYLE_TYPE_UPPER_HEXADECIMAL = 0x13,
        CSS_LIST_STYLE_TYPE_ARABIC_INDIC = 0x14,
        CSS_LIST_STYLE_TYPE_LOWER_ARMENIAN = 0x15,
        CSS_LIST_STYLE_TYPE_UPPER_ARMENIAN = 0x16,
        CSS_LIST_STYLE_TYPE_BENGALI = 0x17,
        CSS_LIST_STYLE_TYPE_CAMBODIAN = 0x18,
        CSS_LIST_STYLE_TYPE_KHMER = 0x19,
        CSS_LIST_STYLE_TYPE_CJK_DECIMAL = 0x1a,
        CSS_LIST_STYLE_TYPE_DEVANAGARI = 0x1b,
        CSS_LIST_STYLE_TYPE_GUJARATI = 0x1c,
        CSS_LIST_STYLE_TYPE_GURMUKHI = 0x1d,
        CSS_LIST_STYLE_TYPE_HEBREW = 0x1e,
        CSS_LIST_STYLE_TYPE_KANNADA = 0x1f,
        CSS_LIST_STYLE_TYPE_LAO = 0x20,
        CSS_LIST_STYLE_TYPE_MALAYALAM = 0x21,
        CSS_LIST_STYLE_TYPE_MONGOLIAN = 0x22,
        CSS_LIST_STYLE_TYPE_MYANMAR = 0x23,
        CSS_LIST_STYLE_TYPE_ORIYA = 0x24,
        CSS_LIST_STYLE_TYPE_PERSIAN = 0x25,
        CSS_LIST_STYLE_TYPE_TAMIL = 0x26,
        CSS_LIST_STYLE_TYPE_TELUGU = 0x27,
        CSS_LIST_STYLE_TYPE_THAI = 0x28,
        CSS_LIST_STYLE_TYPE_TIBETAN = 0x29,
        CSS_LIST_STYLE_TYPE_CJK_EARTHLY_BRANCH = 0x2a,
        CSS_LIST_STYLE_TYPE_CJK_HEAVENLY_STEM = 0x2b,
        CSS_LIST_STYLE_TYPE_HIAGANA = 0x2c,
        CSS_LIST_STYLE_TYPE_HIAGANA_IROHA = 0x2d,
        CSS_LIST_STYLE_TYPE_KATAKANA = 0x2e,
        CSS_LIST_STYLE_TYPE_KATAKANA_IROHA = 0x2f,
        CSS_LIST_STYLE_TYPE_JAPANESE_INFORMAL = 0x30,
        CSS_LIST_STYLE_TYPE_JAPANESE_FORMAL = 0x31,
        CSS_LIST_STYLE_TYPE_KOREAN_HANGUL_FORMAL = 0x32,
        CSS_LIST_STYLE_TYPE_KOREAN_HANJA_INFORMAL = 0x33,
        CSS_LIST_STYLE_TYPE_KOREAN_HANJA_FORMAL = 0x34
    };

    // properties.h:649
    public enum CssMarginEnum : byte
    {
        CSS_MARGIN_INHERIT = 0x0,
        CSS_MARGIN_SET = 0x1,
        CSS_MARGIN_AUTO = 0x2
    }

    public enum CssMaxHeightEnum : byte
    {
        CSS_MAX_HEIGHT_INHERIT = 0x0,
        CSS_MAX_HEIGHT_SET = 0x1,
        CSS_MAX_HEIGHT_NONE = 0x2
    }

    public enum CssMaxWidthEnum : byte
    {
        CSS_MAX_WIDTH_INHERIT = 0x0,
        CSS_MAX_WIDTH_SET = 0x1,
        CSS_MAX_WIDTH_NONE = 0x2
    }

    public enum CssMinHeightEnum : byte
    {
        CSS_MIN_HEIGHT_INHERIT = 0x0,
        CSS_MIN_HEIGHT_SET = 0x1,
        CSS_MIN_HEIGHT_AUTO = 0x2
    }

    public enum CssMinWidthEnum : byte
    {
        CSS_MIN_WIDTH_INHERIT = 0x0,
        CSS_MIN_WIDTH_SET = 0x1,
        CSS_MIN_WIDTH_AUTO = 0x2
    }

    public enum CssOpacityEnum : byte
    {
        CSS_OPACITY_INHERIT = 0x0,
        CSS_OPACITY_SET = 0x1
    }

    public enum CssOrderEnum : byte
    {
        CSS_ORDER_INHERIT = 0x0,
        CSS_ORDER_SET = 0x1
    }

    public enum CssOutlineColorEnum : byte
    {
        CSS_OUTLINE_COLOR_INHERIT = CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_INHERIT,
        CSS_OUTLINE_COLOR_COLOR = CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_COLOR,
        CSS_OUTLINE_COLOR_CURRENT_COLOR = CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_CURRENT_COLOR,
        CSS_OUTLINE_COLOR_INVERT = 0x3
    }

    public enum CssOutlineStyleEnum : byte
    {
        CSS_OUTLINE_STYLE_INHERIT = CssBorderStyleEnum.CSS_BORDER_STYLE_INHERIT,
        CSS_OUTLINE_STYLE_NONE = CssBorderStyleEnum.CSS_BORDER_STYLE_NONE,
        CSS_OUTLINE_STYLE_DOTTED = CssBorderStyleEnum.CSS_BORDER_STYLE_DOTTED,
        CSS_OUTLINE_STYLE_DASHED = CssBorderStyleEnum.CSS_BORDER_STYLE_DASHED,
        CSS_OUTLINE_STYLE_SOLID = CssBorderStyleEnum.CSS_BORDER_STYLE_SOLID,
        CSS_OUTLINE_STYLE_DOUBLE = CssBorderStyleEnum.CSS_BORDER_STYLE_DOUBLE,
        CSS_OUTLINE_STYLE_GROOVE = CssBorderStyleEnum.CSS_BORDER_STYLE_GROOVE,
        CSS_OUTLINE_STYLE_RIDGE = CssBorderStyleEnum.CSS_BORDER_STYLE_RIDGE,
        CSS_OUTLINE_STYLE_INSET = CssBorderStyleEnum.CSS_BORDER_STYLE_INSET,
        CSS_OUTLINE_STYLE_OUTSET = CssBorderStyleEnum.CSS_BORDER_STYLE_OUTSET
    }

    public enum CssOutlineWidthEnum : byte
    {
        CSS_OUTLINE_WIDTH_INHERIT = CssBorderWidthEnum.CSS_BORDER_WIDTH_INHERIT,
        CSS_OUTLINE_WIDTH_THIN = CssBorderWidthEnum.CSS_BORDER_WIDTH_THIN,
        CSS_OUTLINE_WIDTH_MEDIUM = CssBorderWidthEnum.CSS_BORDER_WIDTH_MEDIUM,
        CSS_OUTLINE_WIDTH_THICK = CssBorderWidthEnum.CSS_BORDER_WIDTH_THICK,
        CSS_OUTLINE_WIDTH_WIDTH = CssBorderWidthEnum.CSS_BORDER_WIDTH_WIDTH
    }

    // properties.h:717
    public enum CssOverflowEnum : byte
    {
        CSS_OVERFLOW_INHERIT = 0x0,
        CSS_OVERFLOW_VISIBLE = 0x1,
        CSS_OVERFLOW_HIDDEN = 0x2,
        CSS_OVERFLOW_SCROLL = 0x3,
        CSS_OVERFLOW_AUTO = 0x4
    }

    public enum CssOrphansEnum : byte
    {
        CSS_ORPHANS_INHERIT = 0x0,
        CSS_ORPHANS_SET = 0x1
    }

    public enum CssPaddingEnum : byte
    {
        CSS_PADDING_INHERIT = 0x0,
        CSS_PADDING_SET = 0x1
    }

    public enum CssPageBreakAfterEnum : byte
    {
        CSS_PAGE_BREAK_AFTER_INHERIT = CssBreakAfterEnum.CSS_BREAK_AFTER_INHERIT,
        CSS_PAGE_BREAK_AFTER_AUTO = CssBreakAfterEnum.CSS_BREAK_AFTER_AUTO,
        CSS_PAGE_BREAK_AFTER_AVOID = CssBreakAfterEnum.CSS_BREAK_AFTER_AVOID,
        CSS_PAGE_BREAK_AFTER_ALWAYS = CssBreakAfterEnum.CSS_BREAK_AFTER_ALWAYS,
        CSS_PAGE_BREAK_AFTER_LEFT = CssBreakAfterEnum.CSS_BREAK_AFTER_LEFT,
        CSS_PAGE_BREAK_AFTER_RIGHT = CssBreakAfterEnum.CSS_BREAK_AFTER_RIGHT
    }

    public enum CssPageBreakBeforeEnum : byte
    {
        CSS_PAGE_BREAK_BEFORE_INHERIT = CssBreakAfterEnum.CSS_BREAK_AFTER_INHERIT,
        CSS_PAGE_BREAK_BEFORE_AUTO = CssBreakAfterEnum.CSS_BREAK_AFTER_AUTO,
        CSS_PAGE_BREAK_BEFORE_AVOID = CssBreakAfterEnum.CSS_BREAK_AFTER_AVOID,
        CSS_PAGE_BREAK_BEFORE_ALWAYS = CssBreakAfterEnum.CSS_BREAK_AFTER_ALWAYS,
        CSS_PAGE_BREAK_BEFORE_LEFT = CssBreakAfterEnum.CSS_BREAK_AFTER_LEFT,
        CSS_PAGE_BREAK_BEFORE_RIGHT = CssBreakAfterEnum.CSS_BREAK_AFTER_RIGHT
    }

    public enum CssPageBreakInsideEnum : byte
    {
        CSS_PAGE_BREAK_INSIDE_INHERIT = CssBreakAfterEnum.CSS_BREAK_AFTER_INHERIT,
        CSS_PAGE_BREAK_INSIDE_AUTO = CssBreakAfterEnum.CSS_BREAK_AFTER_AUTO,
        CSS_PAGE_BREAK_INSIDE_AVOID = CssBreakAfterEnum.CSS_BREAK_AFTER_AVOID
    }

    // properties.h:759
    public enum CssPosition : byte
    {
        CSS_POSITION_INHERIT = 0x0,
        CSS_POSITION_STATIC = 0x1,
        CSS_POSITION_RELATIVE = 0x2,
        CSS_POSITION_ABSOLUTE = 0x3,
        CSS_POSITION_FIXED = 0x4
    }

    public enum CssQuotesEnum : byte
    {
        CSS_QUOTES_INHERIT = 0x0,
        /* Consult pointer in struct to determine which */
        CSS_QUOTES_STRING = 0x1,
        CSS_QUOTES_NONE = 0x1
    }

    public enum CssRightEnum : byte
    {
        CSS_RIGHT_INHERIT = 0x0,
        CSS_RIGHT_SET = 0x1,
        CSS_RIGHT_AUTO = 0x2
    }

    public enum CssTableLayoutEnum : byte
    {
        CSS_TABLE_LAYOUT_INHERIT = 0x0,
        CSS_TABLE_LAYOUT_AUTO = 0x1,
        CSS_TABLE_LAYOUT_FIXED = 0x2
    }

    public enum CssTextAlignEnum : byte
    {
        CSS_TEXT_ALIGN_INHERIT = 0x0,
        CSS_TEXT_ALIGN_INHERIT_IF_NON_MAGIC = 0x1,
        CSS_TEXT_ALIGN_LEFT = 0x2,
        CSS_TEXT_ALIGN_RIGHT = 0x3,
        CSS_TEXT_ALIGN_CENTER = 0x4,
        CSS_TEXT_ALIGN_JUSTIFY = 0x5,
        CSS_TEXT_ALIGN_DEFAULT = 0x6,
        CSS_TEXT_ALIGN_LIBCSS_LEFT = 0x7,
        CSS_TEXT_ALIGN_LIBCSS_CENTER = 0x8,
        CSS_TEXT_ALIGN_LIBCSS_RIGHT = 0x9
    }

    public enum CssTextDecorationEnum : byte
    {
        CSS_TEXT_DECORATION_INHERIT = 0x00,
        CSS_TEXT_DECORATION_NONE = 0x10,
        CSS_TEXT_DECORATION_BLINK = (1 << 3),
        CSS_TEXT_DECORATION_LINE_THROUGH = (1 << 2),
        CSS_TEXT_DECORATION_OVERLINE = (1 << 1),
        CSS_TEXT_DECORATION_UNDERLINE = (1 << 0)
    }

    public enum CssTextIndentEnum : byte
    {
        CSS_TEXT_INDENT_INHERIT = 0x0,
        CSS_TEXT_INDENT_SET = 0x1
    }

    public enum CssTextTransformEnum : byte
    {
        CSS_TEXT_TRANSFORM_INHERIT = 0x0,
        CSS_TEXT_TRANSFORM_CAPITALIZE = 0x1,
        CSS_TEXT_TRANSFORM_UPPERCASE = 0x2,
        CSS_TEXT_TRANSFORM_LOWERCASE = 0x3,
        CSS_TEXT_TRANSFORM_NONE = 0x4
    }

    public enum CssTopEnum : byte
    {
        CSS_TOP_INHERIT = 0x0,
        CSS_TOP_SET = 0x1,
        CSS_TOP_AUTO = 0x2
    }

    public enum CssUnicodeBidiEnum : byte
    {
        CSS_UNICODE_BIDI_INHERIT = 0x0,
        CSS_UNICODE_BIDI_NORMAL = 0x1,
        CSS_UNICODE_BIDI_EMBED = 0x2,
        CSS_UNICODE_BIDI_BIDI_OVERRIDE = 0x3
    }

    public enum CssVerticalAlignEnum : byte
    {
        CSS_VERTICAL_ALIGN_INHERIT = 0x0,
        CSS_VERTICAL_ALIGN_BASELINE = 0x1,
        CSS_VERTICAL_ALIGN_SUB = 0x2,
        CSS_VERTICAL_ALIGN_SUPER = 0x3,
        CSS_VERTICAL_ALIGN_TOP = 0x4,
        CSS_VERTICAL_ALIGN_TEXT_TOP = 0x5,
        CSS_VERTICAL_ALIGN_MIDDLE = 0x6,
        CSS_VERTICAL_ALIGN_BOTTOM = 0x7,
        CSS_VERTICAL_ALIGN_TEXT_BOTTOM = 0x8,
        CSS_VERTICAL_ALIGN_SET = 0x9
    }

    public enum CssVisibilityEnum : byte
    {
        CSS_VISIBILITY_INHERIT = 0x0,
        CSS_VISIBILITY_VISIBLE = 0x1,
        CSS_VISIBILITY_HIDDEN = 0x2,
        CSS_VISIBILITY_COLLAPSE = 0x3
    }

    public enum CssWhiteSpaceEnum : byte
    {
        CSS_WHITE_SPACE_INHERIT = 0x0,
        CSS_WHITE_SPACE_NORMAL = 0x1,
        CSS_WHITE_SPACE_PRE = 0x2,
        CSS_WHITE_SPACE_NOWRAP = 0x3,
        CSS_WHITE_SPACE_PRE_WRAP = 0x4,
        CSS_WHITE_SPACE_PRE_LINE = 0x5
    }

    public enum CssWidowsEnum : byte
    {
        CSS_WIDOWS_INHERIT = 0x0,
        CSS_WIDOWS_SET = 0x1
    }

    // properties.h:868
    public enum CssWidth : byte
    {
        CSS_WIDTH_INHERIT = 0x0,
        CSS_WIDTH_SET = 0x1,
        CSS_WIDTH_AUTO = 0x2
    }

    public enum CssWordSpacingEnum : byte
    {
        CSS_WORD_SPACING_INHERIT = CssColumnGapEnum.CSS_COLUMN_GAP_INHERIT,
        CSS_WORD_SPACING_SET = CssColumnGapEnum.CSS_COLUMN_GAP_SET,
        CSS_WORD_SPACING_NORMAL = CssColumnGapEnum.CSS_COLUMN_GAP_NORMAL
    }

    public enum CssWritingModeEnum : byte
    {
        CSS_WRITING_MODE_INHERIT = 0x0,
        CSS_WRITING_MODE_HORIZONTAL_TB = 0x1,
        CSS_WRITING_MODE_VERTICAL_RL = 0x2,
        CSS_WRITING_MODE_VERTICAL_LR = 0x3
    }

    public enum CssZindexEnum : byte
    {
        CSS_Z_INDEX_INHERIT = 0x0,
        CSS_Z_INDEX_SET = 0x1,
        CSS_Z_INDEX_AUTO = 0x2
    }

    public delegate CssStatus PropDispCascade(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state);
    public delegate CssStatus PropDispSetFromHint(CssHint hint, ComputedStyle style);
    public delegate CssStatus PropDispInitial(CssSelectState state);
    public delegate CssStatus PropDispCompose(ComputedStyle parent, ComputedStyle child, ComputedStyle result);

    // dispatch.c:20
    public struct CssPropDispatch
    {
        public bool Inherited;

        public PropDispCascade Cascade;
        public PropDispSetFromHint SetFromHint;
        public PropDispInitial Initial;
        public PropDispCompose Compose;
    }

    public struct CssHintLength
    {
        public Fixed Value;
        public CssUnit Unit;

        public CssHintLength(Fixed val, CssUnit unit)
        {
            Value = val;
            Unit = unit;
        }
    }

    public class CssHint
    {
        //css_computed_clip_rect* clip;
        public Color Color;
        //css_computed_content_item* content;
        //css_computed_counter* counter;
        public Fixed Fixed;
        public int Integer;
        public CssHintLength Length;
        /*struct {
            css_hint_length h;
            css_hint_length v;
        } position;*/
        public string[] Strings; // one string fits here

        public CssPropertiesEnum Prop; // Property index
        public byte Status;            // Property value

        public CssHint()
        {
            //strings = new string[1];
        }
    }

    public static class CssProps
    {
        // dispatch.c:20, prop_dispatch
        // Dispatch table for properties, indexed by opcode (CssPropertiesEnum)
        public static CssPropDispatch[] Dispatch = {
            new CssPropDispatch { // 0x0 azimuth
                Inherited = true,
                Cascade = PropDispCascade_azimuth,
                Compose = PropDispCompose_azimuth,
                Initial = PropDispInitial_azimuth,
                SetFromHint = PropDispSFH_azimuth
            },
            new CssPropDispatch { // 0x1 background_attachment
                Inherited = false,
                Cascade = PropDispCascade_background_attachment,
                Compose = PropDispCompose_background_attachment,
                Initial = PropDispInitial_background_attachment,
                SetFromHint = PropDispSFH_background_attachment
            },
            new CssPropDispatch { // 0x2 background_color
                Inherited = false,
                Cascade = PropDispCascade_background_color,
                Compose = PropDispCompose_background_color,
                Initial = PropDispInitial_background_color,
                SetFromHint = PropDispSFH_background_color
            },
            new CssPropDispatch { // 0x3 background_image
                Inherited = false,
                Cascade = PropDispCascade_background_image,
                Compose = PropDispCompose_background_image,
                Initial = PropDispInitial_background_image,
                SetFromHint = PropDispSFH_background_image
            },
            new CssPropDispatch { // 0x4 background_position
                Inherited = false,
                Cascade = PropDispCascade_background_position,
                Compose = PropDispCompose_background_position,
                Initial = PropDispInitial_background_position,
                SetFromHint = PropDispSFH_background_position
            },
            new CssPropDispatch { // 0x5 background_repeat
                Inherited = false,
                Cascade = PropDispCascade_background_repeat,
                Compose = PropDispCompose_background_repeat,
                Initial = PropDispInitial_background_repeat,
                SetFromHint = PropDispSFH_background_repeat
            },
            new CssPropDispatch { // 0x6 border_collapse
                Inherited = true,
                Cascade = PropDispCascade_border_collapse,
                Compose = PropDispCompose_border_collapse,
                Initial = PropDispInitial_border_collapse,
                SetFromHint = PropDispSFH_border_collapse
            },
            new CssPropDispatch { // 0x7 border_spacing
                Inherited = true,
                Cascade = PropDispCascade_border_spacing,
                Compose = PropDispCompose_border_spacing,
                Initial = PropDispInitial_border_spacing,
                SetFromHint = PropDispSFH_border_spacing
            },
            new CssPropDispatch { // 0x8 border_top_color
                Inherited = false,
                Cascade = PropDispCascade_border_top_color,
                Compose = PropDispCompose_border_top_color,
                Initial = PropDispInitial_border_top_color,
                SetFromHint = PropDispSFH_border_top_color
            },
            new CssPropDispatch { // 0x9 border_right_color
                Inherited = false,
                Cascade = PropDispCascade_border_right_color,
                Compose = PropDispCompose_border_right_color,
                Initial = PropDispInitial_border_right_color,
                SetFromHint = PropDispSFH_border_right_color
            },
            new CssPropDispatch { // 0xA border_bottom_color
                Inherited = false,
                Cascade = PropDispCascade_border_bottom_color,
                Compose = PropDispCompose_border_bottom_color,
                Initial = PropDispInitial_border_bottom_color,
                SetFromHint = PropDispSFH_border_bottom_color
            },
            new CssPropDispatch { // 0xB border_left_color
                Inherited = false,
                Cascade = PropDispCascade_border_left_color,
                Compose = PropDispCompose_border_left_color,
                Initial = PropDispInitial_border_left_color,
                SetFromHint = PropDispSFH_border_left_color
            },
            new CssPropDispatch { // 0xC border_top_style
                Inherited = false,
                Cascade = PropDispCascade_border_top_style,
                Compose = PropDispCompose_border_top_style,
                Initial = PropDispInitial_border_top_style,
                SetFromHint = PropDispSFH_border_top_style
            },
            new CssPropDispatch { // 0xD border_right_style
                Inherited = false,
                Cascade = PropDispCascade_border_right_style,
                Compose = PropDispCompose_border_right_style,
                Initial = PropDispInitial_border_right_style,
                SetFromHint = PropDispSFH_border_right_style
            },
            new CssPropDispatch { // 0xE border_bottom_style
                Inherited = false,
                Cascade = PropDispCascade_border_bottom_style,
                Compose = PropDispCompose_border_bottom_style,
                Initial = PropDispInitial_border_bottom_style,
                SetFromHint = PropDispSFH_border_bottom_style
            },
            new CssPropDispatch { // 0xF border_left_style
                Inherited = false,
                Cascade = PropDispCascade_border_left_style,
                Compose = PropDispCompose_border_left_style,
                Initial = PropDispInitial_border_left_style,
                SetFromHint = PropDispSFH_border_left_style
            },
            new CssPropDispatch { // 0x10 border_top_width
                Inherited = false,
                Cascade = PropDispCascade_border_top_width,
                Compose = PropDispCompose_border_top_width,
                Initial = PropDispInitial_border_top_width,
                SetFromHint = PropDispSFH_border_top_width
            },
            new CssPropDispatch { // 0x11 border_right_width
                Inherited = false,
                Cascade = PropDispCascade_border_right_width,
                Compose = PropDispCompose_border_right_width,
                Initial = PropDispInitial_border_right_width,
                SetFromHint = PropDispSFH_border_right_width
            },
            new CssPropDispatch { // 0x12 border_bottom_width
                Inherited = false,
                Cascade = PropDispCascade_border_bottom_width,
                Compose = PropDispCompose_border_bottom_width,
                Initial = PropDispInitial_border_bottom_width,
                SetFromHint = PropDispSFH_border_bottom_width
            },
            new CssPropDispatch { // 0x13 border_left_width
                Inherited = false,
                Cascade = PropDispCascade_border_left_width,
                Compose = PropDispCompose_border_left_width,
                Initial = PropDispInitial_border_left_width,
                SetFromHint = PropDispSFH_border_left_width
            },
            new CssPropDispatch { // 0x14 bottom
                Inherited = false,
                Cascade = PropDispCascade_bottom,
                Compose = PropDispCompose_bottom,
                Initial = PropDispInitial_bottom,
                SetFromHint = PropDispSFH_bottom
            },
            new CssPropDispatch { // 0x15 caption_side
                Inherited = true,
                Cascade = PropDispCascade_caption_side,
                Compose = PropDispCompose_caption_side,
                Initial = PropDispInitial_caption_side,
                SetFromHint = PropDispSFH_caption_side
            },
            new CssPropDispatch { // 0x16 clear
                Inherited = false,
                Cascade = PropDispCascade_clear,
                Compose = PropDispCompose_clear,
                Initial = PropDispInitial_clear,
                SetFromHint = PropDispSFH_clear
            },
            new CssPropDispatch { // 0x17 clip
                Inherited = false,
                Cascade = PropDispCascade_clip,
                Compose = PropDispCompose_clip,
                Initial = PropDispInitial_clip,
                SetFromHint = PropDispSFH_clip
            },
            new CssPropDispatch { // 0x18 color
                Inherited = true,
                Cascade = PropDispCascade_color,
                Compose = PropDispCompose_color,
                Initial = PropDispInitial_color,
                SetFromHint = PropDispSFH_color
            },
            new CssPropDispatch { // 0x19 content
                Inherited = false,
                Cascade = PropDispCascade_content,
                Compose = PropDispCompose_content,
                Initial = PropDispInitial_content,
                SetFromHint = PropDispSFH_content
            },
            new CssPropDispatch { // 0x1A counter_increment
                Inherited = false,
                Cascade = PropDispCascade_counter_increment,
                Compose = PropDispCompose_counter_increment,
                Initial = PropDispInitial_counter_increment,
                SetFromHint = PropDispSFH_counter_increment
            },
            new CssPropDispatch { // 0x1B counter_reset
                Inherited = false,
                Cascade = PropDispCascade_counter_reset,
                Compose = PropDispCompose_counter_reset,
                Initial = PropDispInitial_counter_reset,
                SetFromHint = PropDispSFH_counter_reset
            },
            new CssPropDispatch { // 0x1C cue_after
                Inherited = false,
                Cascade = PropDispCascade_cue_after,
                Compose = PropDispCompose_cue_after,
                Initial = PropDispInitial_cue_after,
                SetFromHint = PropDispSFH_cue_after
            },
            new CssPropDispatch { // 0x1D cue_before
                Inherited = false,
                Cascade = PropDispCascade_cue_before,
                Compose = PropDispCompose_cue_before,
                Initial = PropDispInitial_cue_before,
                SetFromHint = PropDispSFH_cue_before
            },
            new CssPropDispatch { // 0x1E cursor
                Inherited = true,
                Cascade = PropDispCascade_cursor,
                Compose = PropDispCompose_cursor,
                Initial = PropDispInitial_cursor,
                SetFromHint = PropDispSFH_cursor
            },
            new CssPropDispatch { // 0x1F direction
                Inherited = true,
                Cascade = PropDispCascade_direction,
                Compose = PropDispCompose_direction,
                Initial = PropDispInitial_direction,
                SetFromHint = PropDispSFH_direction
            },
            new CssPropDispatch { // 0x20 display
                Inherited = false,
                Cascade = PropDispCascade_display,
                Compose = PropDispCompose_display,
                Initial = PropDispInitial_display,
                SetFromHint = PropDispSFH_display
            },
            new CssPropDispatch { // 0x21 elevation
                Inherited = true,
                Cascade = PropDispCascade_elevation,
                Compose = PropDispCompose_elevation,
                Initial = PropDispInitial_elevation,
                SetFromHint = PropDispSFH_elevation
            },
            new CssPropDispatch { // 0x22 empty_cells
                Inherited = true,
                Cascade = PropDispCascade_empty_cells,
                Compose = PropDispCompose_empty_cells,
                Initial = PropDispInitial_empty_cells,
                SetFromHint = PropDispSFH_empty_cells
            },
            new CssPropDispatch { // 0x23 float
                Inherited = false,
                Cascade = PropDispCascade_float,
                Compose = PropDispCompose_float,
                Initial = PropDispInitial_float,
                SetFromHint = PropDispSFH_float
            },
            new CssPropDispatch { // 0x24 font_family
                Inherited = true,
                Cascade = PropDispCascade_font_family,
                Compose = PropDispCompose_font_family,
                Initial = PropDispInitial_font_family,
                SetFromHint = PropDispSFH_font_family
            },
            new CssPropDispatch { // 0x25 font_size
                Inherited = true,
                Cascade = PropDispCascade_font_size,
                Compose = PropDispCompose_font_size,
                Initial = PropDispInitial_font_size,
                SetFromHint = PropDispSFH_font_size
            },
            new CssPropDispatch { // 0x26 font_style
                Inherited = true,
                Cascade = PropDispCascade_font_style,
                Compose = PropDispCompose_font_style,
                Initial = PropDispInitial_font_style,
                SetFromHint = PropDispSFH_font_style
            },
            new CssPropDispatch { // 0x27 font_variant
                Inherited = true,
                Cascade = PropDispCascade_font_variant,
                Compose = PropDispCompose_font_variant,
                Initial = PropDispInitial_font_variant,
                SetFromHint = PropDispSFH_font_variant
            },
            new CssPropDispatch { // 0x28 font_weight
                Inherited = true,
                Cascade = PropDispCascade_font_weight,
                Compose = PropDispCompose_font_weight,
                Initial = PropDispInitial_font_weight,
                SetFromHint = PropDispSFH_font_weight
            },
            new CssPropDispatch { // 0x29 height
                Inherited = false,
                Cascade = PropDispCascade_height,
                Compose = PropDispCompose_height,
                Initial = PropDispInitial_height,
                SetFromHint = PropDispSFH_height
            },
            new CssPropDispatch { // 0x2A left
                Inherited = false,
                Cascade = PropDispCascade_left,
                Compose = PropDispCompose_left,
                Initial = PropDispInitial_left,
                SetFromHint = PropDispSFH_left
            },
            new CssPropDispatch { // 0x2B letter_spacing
                Inherited = true,
                Cascade = PropDispCascade_letter_spacing,
                Compose = PropDispCompose_letter_spacing,
                Initial = PropDispInitial_letter_spacing,
                SetFromHint = PropDispSFH_letter_spacing
            },
            new CssPropDispatch { // 0x2C line_height
                Inherited = true,
                Cascade = PropDispCascade_line_height,
                Compose = PropDispCompose_line_height,
                Initial = PropDispInitial_line_height,
                SetFromHint = PropDispSFH_line_height
            },
            new CssPropDispatch { // 0x2D list_style_image
                Inherited = true,
                Cascade = PropDispCascade_list_style_image,
                Compose = PropDispCompose_list_style_image,
                Initial = PropDispInitial_list_style_image,
                SetFromHint = PropDispSFH_list_style_image
            },
            new CssPropDispatch { // 0x2E list_style_position
                Inherited = true,
                Cascade = PropDispCascade_list_style_position,
                Compose = PropDispCompose_list_style_position,
                Initial = PropDispInitial_list_style_position,
                SetFromHint = PropDispSFH_list_style_position
            },
            new CssPropDispatch { // 0x2F list_style_type
                Inherited = true,
                Cascade = PropDispCascade_list_style_type,
                Compose = PropDispCompose_list_style_type,
                Initial = PropDispInitial_list_style_type,
                SetFromHint = PropDispSFH_list_style_type
            },
            new CssPropDispatch { // 0x30 margin_top
                Inherited = false,
                Cascade = PropDispCascade_margin_top,
                Compose = PropDispCompose_margin_top,
                Initial = PropDispInitial_margin_top,
                SetFromHint = PropDispSFH_margin_top
            },
            new CssPropDispatch { // 0x31 margin_right
                Inherited = false,
                Cascade = PropDispCascade_margin_right,
                Compose = PropDispCompose_margin_right,
                Initial = PropDispInitial_margin_right,
                SetFromHint = PropDispSFH_margin_right
            },
            new CssPropDispatch { // 0x32 margin_bottom
                Inherited = false,
                Cascade = PropDispCascade_margin_bottom,
                Compose = PropDispCompose_margin_bottom,
                Initial = PropDispInitial_margin_bottom,
                SetFromHint = PropDispSFH_margin_bottom
            },
            new CssPropDispatch { // 0x33 margin_left
                Inherited = false,
                Cascade = PropDispCascade_margin_left,
                Compose = PropDispCompose_margin_left,
                Initial = PropDispInitial_margin_left,
                SetFromHint = PropDispSFH_margin_left
            },
            new CssPropDispatch { // 0x34 max_height
                Inherited = false,
                Cascade = PropDispCascade_max_height,
                Compose = PropDispCompose_max_height,
                Initial = PropDispInitial_max_height,
                SetFromHint = PropDispSFH_max_height
            },
            new CssPropDispatch { // 0x35 max_width
                Inherited = false,
                Cascade = PropDispCascade_max_width,
                Compose = PropDispCompose_max_width,
                Initial = PropDispInitial_max_width,
                SetFromHint = PropDispSFH_max_width
            },
            new CssPropDispatch { // 0x36 min_height
                Inherited = false,
                Cascade = PropDispCascade_min_height,
                Compose = PropDispCompose_min_height,
                Initial = PropDispInitial_min_height,
                SetFromHint = PropDispSFH_min_height
            },
            new CssPropDispatch { // 0x37 min_width
                Inherited = false,
                Cascade = PropDispCascade_min_width,
                Compose = PropDispCompose_min_width,
                Initial = PropDispInitial_min_width,
                SetFromHint = PropDispSFH_min_width
            },
            new CssPropDispatch { // 0x38 orphans
                Inherited = true,
                Cascade = PropDispCascade_orphans,
                Compose = PropDispCompose_orphans,
                Initial = PropDispInitial_orphans,
                SetFromHint = PropDispSFH_orphans
            },
            new CssPropDispatch { // 0x39 outline_color
                Inherited = false,
                Cascade = PropDispCascade_outline_color,
                Compose = PropDispCompose_outline_color,
                Initial = PropDispInitial_outline_color,
                SetFromHint = PropDispSFH_outline_color
            },
            new CssPropDispatch { // 0x3A outline_style
                Inherited = false,
                Cascade = PropDispCascade_outline_style,
                Compose = PropDispCompose_outline_style,
                Initial = PropDispInitial_outline_style,
                SetFromHint = PropDispSFH_outline_style
            },
            new CssPropDispatch { // 0x3B outline_width
                Inherited = false,
                Cascade = PropDispCascade_outline_width,
                Compose = PropDispCompose_outline_width,
                Initial = PropDispInitial_outline_width,
                SetFromHint = PropDispSFH_outline_width
            },
            new CssPropDispatch { // 0x3C overflow_x
                Inherited = false,
                Cascade = PropDispCascade_overflow_x,
                Compose = PropDispCompose_overflow_x,
                Initial = PropDispInitial_overflow_x,
                SetFromHint = PropDispSFH_overflow_x
            },
            new CssPropDispatch { // 0x3D padding_top
                Inherited = false,
                Cascade = PropDispCascade_padding_top,
                Compose = PropDispCompose_padding_top,
                Initial = PropDispInitial_padding_top,
                SetFromHint = PropDispSFH_padding_top
            },
            new CssPropDispatch { // 0x3E padding_right
                Inherited = false,
                Cascade = PropDispCascade_padding_right,
                Compose = PropDispCompose_padding_right,
                Initial = PropDispInitial_padding_right,
                SetFromHint = PropDispSFH_padding_right
            },
            new CssPropDispatch { // 0x3F padding_bottom
                Inherited = false,
                Cascade = PropDispCascade_padding_bottom,
                Compose = PropDispCompose_padding_bottom,
                Initial = PropDispInitial_padding_bottom,
                SetFromHint = PropDispSFH_padding_bottom
            },
            new CssPropDispatch { // 0x40 padding_left
                Inherited = false,
                Cascade = PropDispCascade_padding_left,
                Compose = PropDispCompose_padding_left,
                Initial = PropDispInitial_padding_left,
                SetFromHint = PropDispSFH_padding_left
            },
            new CssPropDispatch { // 0x41 page_break_after
                Inherited = false,
                Cascade = PropDispCascade_page_break_after,
                Compose = PropDispCompose_page_break_after,
                Initial = PropDispInitial_page_break_after,
                SetFromHint = PropDispSFH_page_break_after
            },
            new CssPropDispatch { // 0x42 page_break_before
                Inherited = false,
                Cascade = PropDispCascade_page_break_before,
                Compose = PropDispCompose_page_break_before,
                Initial = PropDispInitial_page_break_before,
                SetFromHint = PropDispSFH_page_break_before
            },
            new CssPropDispatch { // 0x43 page_break_inside
                Inherited = true,
                Cascade = PropDispCascade_page_break_inside,
                Compose = PropDispCompose_page_break_inside,
                Initial = PropDispInitial_page_break_inside,
                SetFromHint = PropDispSFH_page_break_inside
            },
            new CssPropDispatch { // 0x44 pause_after
                Inherited = false,
                Cascade = PropDispCascade_pause_after,
                Compose = PropDispCompose_pause_after,
                Initial = PropDispInitial_pause_after,
                SetFromHint = PropDispSFH_pause_after
            },
            new CssPropDispatch { // 0x45 pause_before
                Inherited = false,
                Cascade = PropDispCascade_pause_before,
                Compose = PropDispCompose_pause_before,
                Initial = PropDispInitial_pause_before,
                SetFromHint = PropDispSFH_pause_before
            },
            new CssPropDispatch { // 0x46 pitch_range
                Inherited = true,
                Cascade = PropDispCascade_pitch_range,
                Compose = PropDispCompose_pitch_range,
                Initial = PropDispInitial_pitch_range,
                SetFromHint = PropDispSFH_pitch_range
            },
            new CssPropDispatch { // 0x47 pitch
                Inherited = true,
                Cascade = PropDispCascade_pitch,
                Compose = PropDispCompose_pitch,
                Initial = PropDispInitial_pitch,
                SetFromHint = PropDispSFH_pitch
            },
            new CssPropDispatch { // 0x48 play_during
                Inherited = false,
                Cascade = PropDispCascade_play_during,
                Compose = PropDispCompose_play_during,
                Initial = PropDispInitial_play_during,
                SetFromHint = PropDispSFH_play_during
            },
            new CssPropDispatch { // 0x49 position
                Inherited = false,
                Cascade = PropDispCascade_position,
                Compose = PropDispCompose_position,
                Initial = PropDispInitial_position,
                SetFromHint = PropDispSFH_position
            },
            new CssPropDispatch { // 0x4A quotes
                Inherited = true,
                Cascade = PropDispCascade_quotes,
                Compose = PropDispCompose_quotes,
                Initial = PropDispInitial_quotes,
                SetFromHint = PropDispSFH_quotes
            },
            new CssPropDispatch { // 0x4B richness
                Inherited = true,
                Cascade = PropDispCascade_richness,
                Compose = PropDispCompose_richness,
                Initial = PropDispInitial_richness,
                SetFromHint = PropDispSFH_richness
            },
            new CssPropDispatch { // 0x4C right
                Inherited = false,
                Cascade = PropDispCascade_right,
                Compose = PropDispCompose_right,
                Initial = PropDispInitial_right,
                SetFromHint = PropDispSFH_right
            },
            new CssPropDispatch { // 0x4D speak_header
                Inherited = true,
                Cascade = PropDispCascade_speak_header,
                Compose = PropDispCompose_speak_header,
                Initial = PropDispInitial_speak_header,
                SetFromHint = PropDispSFH_speak_header
            },
            new CssPropDispatch { // 0x4E speak_numeral
                Inherited = true,
                Cascade = PropDispCascade_speak_numeral,
                Compose = PropDispCompose_speak_numeral,
                Initial = PropDispInitial_speak_numeral,
                SetFromHint = PropDispSFH_speak_numeral
            },
            new CssPropDispatch { // 0x4F speak_punctuation
                Inherited = true,
                Cascade = PropDispCascade_speak_punctuation,
                Compose = PropDispCompose_speak_punctuation,
                Initial = PropDispInitial_speak_punctuation,
                SetFromHint = PropDispSFH_speak_punctuation
            },
            new CssPropDispatch { // 0x50 speak
                Inherited = true,
                Cascade = PropDispCascade_speak,
                Compose = PropDispCompose_speak,
                Initial = PropDispInitial_speak,
                SetFromHint = PropDispSFH_speak
            },
            new CssPropDispatch { // 0x51 speech_rate
                Inherited = true,
                Cascade = PropDispCascade_speech_rate,
                Compose = PropDispCompose_speech_rate,
                Initial = PropDispInitial_speech_rate,
                SetFromHint = PropDispSFH_speech_rate
            },
            new CssPropDispatch { // 0x52 stress
                Inherited = true,
                Cascade = PropDispCascade_stress,
                Compose = PropDispCompose_stress,
                Initial = PropDispInitial_stress,
                SetFromHint = PropDispSFH_stress
            },
            new CssPropDispatch { // 0x53 table_layout
                Inherited = false,
                Cascade = PropDispCascade_table_layout,
                Compose = PropDispCompose_table_layout,
                Initial = PropDispInitial_table_layout,
                SetFromHint = PropDispSFH_table_layout
            },
            new CssPropDispatch { // 0x54 text_align
                Inherited = true,
                Cascade = PropDispCascade_text_align,
                Compose = PropDispCompose_text_align,
                Initial = PropDispInitial_text_align,
                SetFromHint = PropDispSFH_text_align
            },
            new CssPropDispatch { // 0x55 text_decoration
                Inherited = false,
                Cascade = PropDispCascade_text_decoration,
                Compose = PropDispCompose_text_decoration,
                Initial = PropDispInitial_text_decoration,
                SetFromHint = PropDispSFH_text_decoration
            },
            new CssPropDispatch { // 0x56 text_indent
                Inherited = false,
                Cascade = PropDispCascade_text_indent,
                Compose = PropDispCompose_text_indent,
                Initial = PropDispInitial_text_indent,
                SetFromHint = PropDispSFH_text_indent
            },
            new CssPropDispatch { // 0x57 text_transform
                Inherited = true,
                Cascade = PropDispCascade_text_transform,
                Compose = PropDispCompose_text_transform,
                Initial = PropDispInitial_text_transform,
                SetFromHint = PropDispSFH_text_transform
            },
            new CssPropDispatch { // 0x58 top
                Inherited = false,
                Cascade = PropDispCascade_top,
                Compose = PropDispCompose_top,
                Initial = PropDispInitial_top,
                SetFromHint = PropDispSFH_top
            },
            new CssPropDispatch { // 0x59 unicode_bidi
                Inherited = false,
                Cascade = PropDispCascade_unicode_bidi,
                Compose = PropDispCompose_unicode_bidi,
                Initial = PropDispInitial_unicode_bidi,
                SetFromHint = PropDispSFH_unicode_bidi
            },
            new CssPropDispatch { // 0x5A vertical_align
                Inherited = false,
                Cascade = PropDispCascade_vertical_align,
                Compose = PropDispCompose_vertical_align,
                Initial = PropDispInitial_vertical_align,
                SetFromHint = PropDispSFH_vertical_align
            },
            new CssPropDispatch { // 0x5B visibility
                Inherited = true,
                Cascade = PropDispCascade_visibility,
                Compose = PropDispCompose_visibility,
                Initial = PropDispInitial_visibility,
                SetFromHint = PropDispSFH_visibility
            },
            new CssPropDispatch { // 0x5C voice_family
                Inherited = true,
                Cascade = PropDispCascade_voice_family,
                Compose = PropDispCompose_voice_family,
                Initial = PropDispInitial_voice_family,
                SetFromHint = PropDispSFH_voice_family
            },
            new CssPropDispatch { // 0x5D volume
                Inherited = true,
                Cascade = PropDispCascade_volume,
                Compose = PropDispCompose_volume,
                Initial = PropDispInitial_volume,
                SetFromHint = PropDispSFH_volume
            },
            new CssPropDispatch { // 0x5E white_space
                Inherited = true,
                Cascade = PropDispCascade_white_space,
                Compose = PropDispCompose_white_space,
                Initial = PropDispInitial_white_space,
                SetFromHint = PropDispSFH_white_space
            },
            new CssPropDispatch { // 0x5F widows
                Inherited = true,
                Cascade = PropDispCascade_widows,
                Compose = PropDispCompose_widows,
                Initial = PropDispInitial_widows,
                SetFromHint = PropDispSFH_widows
            },
            new CssPropDispatch { // 0x60 width
                Inherited = false,
                Cascade = PropDispCascade_width,
                Compose = PropDispCompose_width,
                Initial = PropDispInitial_width,
                SetFromHint = PropDispSFH_width
            },
            new CssPropDispatch { // 0x61 word_spacing
                Inherited = true,
                Cascade = PropDispCascade_word_spacing,
                Compose = PropDispCompose_word_spacing,
                Initial = PropDispInitial_word_spacing,
                SetFromHint = PropDispSFH_word_spacing
            },
            new CssPropDispatch { // 0x62 z_index
                Inherited = false,
                Cascade = PropDispCascade_z_index,
                Compose = PropDispCompose_z_index,
                Initial = PropDispInitial_z_index,
                SetFromHint = PropDispSFH_z_index
            },
            new CssPropDispatch { // 0x63 opacity
                Inherited = false,
                Cascade = PropDispCascade_opacity,
                Compose = PropDispCompose_opacity,
                Initial = PropDispInitial_opacity,
                SetFromHint = PropDispSFH_opacity
            },
            new CssPropDispatch { // 0x64 break_after
                Inherited = false,
                Cascade = PropDispCascade_break_after,
                Compose = PropDispCompose_break_after,
                Initial = PropDispInitial_break_after,
                SetFromHint = PropDispSFH_break_after
            },
            new CssPropDispatch { // 0x65 break_before
                Inherited = false,
                Cascade = PropDispCascade_break_before,
                Compose = PropDispCompose_break_before,
                Initial = PropDispInitial_break_before,
                SetFromHint = PropDispSFH_break_before
            },
            new CssPropDispatch { // 0x66 break_inside
                Inherited = false,
                Cascade = PropDispCascade_break_inside,
                Compose = PropDispCompose_break_inside,
                Initial = PropDispInitial_break_inside,
                SetFromHint = PropDispSFH_break_inside
            },
            new CssPropDispatch { // 0x67 column_count
                Inherited = false,
                Cascade = PropDispCascade_column_count,
                Compose = PropDispCompose_column_count,
                Initial = PropDispInitial_column_count,
                SetFromHint = PropDispSFH_column_count
            },
            new CssPropDispatch { // 0x68 column_fill
                Inherited = false,
                Cascade = PropDispCascade_column_fill,
                Compose = PropDispCompose_column_fill,
                Initial = PropDispInitial_column_fill,
                SetFromHint = PropDispSFH_column_fill
            },
            new CssPropDispatch { // 0x69 column_gap
                Inherited = false,
                Cascade = PropDispCascade_column_gap,
                Compose = PropDispCompose_column_gap,
                Initial = PropDispInitial_column_gap,
                SetFromHint = PropDispSFH_column_gap
            },
            new CssPropDispatch { // 0x6A column_rule_color
                Inherited = false,
                Cascade = PropDispCascade_column_rule_color,
                Compose = PropDispCompose_column_rule_color,
                Initial = PropDispInitial_column_rule_color,
                SetFromHint = PropDispSFH_column_rule_color
            },
            new CssPropDispatch { // 0x6B column_rule_style
                Inherited = false,
                Cascade = PropDispCascade_column_rule_style,
                Compose = PropDispCompose_column_rule_style,
                Initial = PropDispInitial_column_rule_style,
                SetFromHint = PropDispSFH_column_rule_style
            },
            new CssPropDispatch { // 0x6C column_rule_width
                Inherited = false,
                Cascade = PropDispCascade_column_rule_width,
                Compose = PropDispCompose_column_rule_width,
                Initial = PropDispInitial_column_rule_width,
                SetFromHint = PropDispSFH_column_rule_width
            },
            new CssPropDispatch { // 0x6D column_span
                Inherited = false,
                Cascade = PropDispCascade_column_span,
                Compose = PropDispCompose_column_span,
                Initial = PropDispInitial_column_span,
                SetFromHint = PropDispSFH_column_span
            },
            new CssPropDispatch { // 0x6E column_width
                Inherited = false,
                Cascade = PropDispCascade_column_width,
                Compose = PropDispCompose_column_width,
                Initial = PropDispInitial_column_width,
                SetFromHint = PropDispSFH_column_width
            },
            new CssPropDispatch { // 0x6F writing_mode
                Inherited = false,
                Cascade = PropDispCascade_writing_mode,
                Compose = PropDispCompose_writing_mode,
                Initial = PropDispInitial_writing_mode,
                SetFromHint = PropDispSFH_writing_mode
            },
            new CssPropDispatch { // 0x70 overflow_y
                Inherited = false,
                Cascade = PropDispCascade_overflow_y,
                Compose = PropDispCompose_overflow_y,
                Initial = PropDispInitial_overflow_y,
                SetFromHint = PropDispSFH_overflow_y
            },
            new CssPropDispatch { // 0x71 box_sizing
                Inherited = false,
                Cascade = PropDispCascade_box_sizing,
                Compose = PropDispCompose_box_sizing,
                Initial = PropDispInitial_box_sizing,
                SetFromHint = PropDispSFH_box_sizing
            },
            new CssPropDispatch { // 0x72 align_content
                Inherited = false,
                Cascade = PropDispCascade_align_content,
                Compose = PropDispCompose_align_content,
                Initial = PropDispInitial_align_content,
                SetFromHint = PropDispSFH_align_content
            },
            new CssPropDispatch { // 0x73 align_items
                Inherited = false,
                Cascade = PropDispCascade_align_items,
                Compose = PropDispCompose_align_items,
                Initial = PropDispInitial_align_items,
                SetFromHint = PropDispSFH_align_items
            },
            new CssPropDispatch { // 0x74 align_self
                Inherited = false,
                Cascade = PropDispCascade_align_self,
                Compose = PropDispCompose_align_self,
                Initial = PropDispInitial_align_self,
                SetFromHint = PropDispSFH_align_self
            },
            new CssPropDispatch { // 0x75 flex_basis
                Inherited = false,
                Cascade = PropDispCascade_flex_basis,
                Compose = PropDispCompose_flex_basis,
                Initial = PropDispInitial_flex_basis,
                SetFromHint = PropDispSFH_flex_basis
            },
            new CssPropDispatch { // 0x76 flex_direction
                Inherited = false,
                Cascade = PropDispCascade_flex_direction,
                Compose = PropDispCompose_flex_direction,
                Initial = PropDispInitial_flex_direction,
                SetFromHint = PropDispSFH_flex_direction
            },
            new CssPropDispatch { // 0x77 flex_grow
                Inherited = false,
                Cascade = PropDispCascade_flex_grow,
                Compose = PropDispCompose_flex_grow,
                Initial = PropDispInitial_flex_grow,
                SetFromHint = PropDispSFH_flex_grow
            },
            new CssPropDispatch { // 0x78 flex_shrink
                Inherited = false,
                Cascade = PropDispCascade_flex_shrink,
                Compose = PropDispCompose_flex_shrink,
                Initial = PropDispInitial_flex_shrink,
                SetFromHint = PropDispSFH_flex_shrink
            },
            new CssPropDispatch { // 0x79 flex_wrap
                Inherited = false,
                Cascade = PropDispCascade_flex_wrap,
                Compose = PropDispCompose_flex_wrap,
                Initial = PropDispInitial_flex_wrap,
                SetFromHint = PropDispSFH_flex_wrap
            },
            new CssPropDispatch { // 0x7A justify_content
                Inherited = false,
                Cascade = PropDispCascade_justify_content,
                Compose = PropDispCompose_justify_content,
                Initial = PropDispInitial_justify_content,
                SetFromHint = PropDispSFH_justify_content
            },
            new CssPropDispatch { // 0x7B order
                Inherited = false,
                Cascade = PropDispCascade_order,
                Compose = PropDispCompose_order,
                Initial = PropDispInitial_order,
                SetFromHint = PropDispSFH_order
            }
        };

        // defaults, options.h:128
        static PlotFontGenericFamily FontDefault = PlotFontGenericFamily.PLOT_FONT_FAMILY_SANS_SERIF;

        // select.c:1814
        public static void SetInitial(CssSelectState state, int i,
                               CssPseudoElement pseudo, XmlNode parent)
        {
            /* Do nothing if this property is inherited (the default state
             * of a clean computed style is for everything to be set to inherit)
             *
             * If the node is tree root and we're dealing with the base element,
             * everything should be defaulted.
             */
            if (!Dispatch[i].Inherited ||
                (pseudo == CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE && parent == null))
            {
                Dispatch[i].Initial(state);
            }
        }

        // select.c:1693
        static CssHint UserAgentDefaultForProperty(CssPropertiesEnum prop)
        {
            var hint = new CssHint();

            if (prop == CssPropertiesEnum.CSS_PROP_COLOR)
            {
                hint.Color = new Color(0xff000000);
                hint.Status = (byte)CssColorEnum.CSS_COLOR_COLOR;
            }
            else if (prop == CssPropertiesEnum.CSS_PROP_FONT_FAMILY)
            {
                hint.Strings[0] = string.Empty;
                switch (FontDefault)
                {
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_SANS_SERIF:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_SANS_SERIF;
                        break;
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_SERIF:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_SERIF;
                        break;
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_MONOSPACE:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_MONOSPACE;
                        break;
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_CURSIVE:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_CURSIVE;
                        break;
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_FANTASY:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_FANTASY;
                        break;
                }
            }
            else if (prop == CssPropertiesEnum.CSS_PROP_QUOTES)
            {
                /** \todo Not exactly useful :) */
                //hint->data.strings = NULL;
                //hint->status = CSS_QUOTES_NONE;
                Log.Unimplemented();
            }
            else if (prop == CssPropertiesEnum.CSS_PROP_VOICE_FAMILY)
            {
                /** \todo Fix this when we have voice-family done */
                //hint->data.strings = NULL;
                //hint->status = 0;
                Log.Unimplemented();
            }
            else
            {
                throw new Exception("Invalid property");
                //return hint;
            }

            return hint;
        }

		// css__cascade_length_auto()
		// helpers.c:185
		static CssStatus CascadeLengthAuto(OpCode op, CssStyle style, ref int bi, ref int used,
            CssSelectState state, string type)
        {
			// type == "margin-top"
			bool inherit = op.IsInherit();
			var value = CssBottomEnum.CSS_BOTTOM_INHERIT;
			Fixed length = Fixed.F_0;
			var unit = OpcodeUnit.PX;

			if (inherit == false)
			{
				switch ((OpBottom)op.GetValue())
				{
					case OpBottom.BOTTOM_SET:
						value = CssBottomEnum.CSS_BOTTOM_SET;
						length = new Fixed((int)style.Bytecode[bi].GetOPV(), true);//length = *((css_fixed*)style->bytecode);
						//advance_bytecode(style, sizeof(length));
						bi++; used--;

						unit = (OpcodeUnit)style.Bytecode[bi].GetOPV();//unit = *((uint32_t*)style->bytecode);
						//advance_bytecode(style, sizeof(unit));
						bi++; used--;
						break;
					case OpBottom.BOTTOM_AUTO:
						value = CssBottomEnum.CSS_BOTTOM_AUTO;
						break;
				}
			}

			var cssunit = CssUnitCtx.FromOpcodeUnit(unit);

			if (state.OutranksExisting(op, op.IsImportant(), inherit))
			{
                switch (type)
                {
                    case "margin-top":
                        state.Computed.SetMarginTop((CssMarginEnum)value, length, cssunit);
                        break;
					case "margin-bottom":
						state.Computed.SetMarginBottom((CssMarginEnum)value, length, cssunit);
						break;
					case "margin-left":
						state.Computed.SetMarginLeft((CssMarginEnum)value, length, cssunit);
						break;
					case "margin-right":
						state.Computed.SetMarginRight((CssMarginEnum)value, length, cssunit);
						break;
					default:
                        Log.Unimplemented("Not yet implemented");
                        break;
                }
			}

			return CssStatus.CSS_OK;
		}

		// autogenerated_propset.h:1327

		/////////////////////////////////////////////////////////////////////////
		// Autogenerated handlers
		// select/properties/[property_name].c
		#region azimuth
		static CssStatus PropDispCascade_azimuth(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_azimuth(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_azimuth(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_azimuth(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region background_attachment
        static CssStatus PropDispCascade_background_attachment(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_background_attachment(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_background_attachment(CssSelectState state)
        {
            state.Computed.SetBackgroundAttachment(CssBackgroundAttachment.CSS_BACKGROUND_ATTACHMENT_SCROLL);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_background_attachment(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBackgroundAttachment();

            if (type == CssBackgroundAttachment.CSS_BACKGROUND_ATTACHMENT_INHERIT)
            {
                type = parent.GetBackgroundAttachment();
            }

            result.SetBackgroundAttachment(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region background_color
        static CssStatus PropDispCascade_background_color(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_background_color(CssHint hint, ComputedStyle style)
        {
            style.SetBackgroundColor((CssBackgroundColorEnum)hint.Status, hint.Color);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_background_color(CssSelectState state)
        {
            state.Computed.SetBackgroundColor(CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_COLOR, new Color(0));
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_background_color(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Color color;
            var type = child.GetBackgroundColor(out color);

            if (type == CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_INHERIT)
                type = parent.GetBackgroundColor(out color);

            result.SetBackgroundColor(type, color);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region background_image
        static CssStatus PropDispCascade_background_image(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_background_image(CssHint hint, ComputedStyle style)
        {
            style.SetBackgroundImage((CssBackgroundImageEnum)hint.Status, hint.Strings[0]);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_background_image(CssSelectState state)
        {
            state.Computed.SetBackgroundImage(CssBackgroundImageEnum.CSS_BACKGROUND_IMAGE_NONE, null);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_background_image(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            string url;
            var type = child.GetBackgroundImage(out url);

            if (type == CssBackgroundImageEnum.CSS_BACKGROUND_IMAGE_INHERIT)
                type = parent.GetBackgroundImage(out url);

            result.SetBackgroundImage(type, url);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region background_position
        static CssStatus PropDispCascade_background_position(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_background_position(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_background_position(CssSelectState state)
        {
            state.Computed.SetBackgroundPosition(
                (byte)CssBackgroundPositionEnum.CSS_BACKGROUND_POSITION_SET,
                Fixed.F_0, CssUnit.CSS_UNIT_PCT,
                Fixed.F_0, CssUnit.CSS_UNIT_PCT);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_background_position(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed hlength = Fixed.F_0, vlength = Fixed.F_0;
            CssUnit hunit = CssUnit.CSS_UNIT_PX, vunit = CssUnit.CSS_UNIT_PX;

            var type = child.GetBackgroundPosition(out hlength, out hunit, out vlength, out vunit);

            if (type == CssBackgroundPositionEnum.CSS_BACKGROUND_POSITION_INHERIT)
            {
                type = parent.GetBackgroundPosition(out hlength, out hunit, out vlength, out vunit);
            }

            result.SetBackgroundPosition((byte)type, hlength, hunit, vlength, vunit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region background_repeat
        static CssStatus PropDispCascade_background_repeat(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_background_repeat(CssHint hint, ComputedStyle style)
        {
            style.SetBackgroundRepeat((CssBackgroundRepeat)hint.Status);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_background_repeat(CssSelectState state)
        {
            state.Computed.SetBackgroundRepeat(CssBackgroundRepeat.CSS_BACKGROUND_REPEAT_REPEAT);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_background_repeat(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBackgroundRepeat();

            if (type == CssBackgroundRepeat.CSS_BACKGROUND_REPEAT_INHERIT)
            {
                type = parent.GetBackgroundRepeat();
            }

            result.SetBackgroundRepeat(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_collapse
        static CssStatus PropDispCascade_border_collapse(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_collapse(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_collapse(CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_collapse(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBorderCollapse();

            if (type == CssBorderCollapseEnum.CSS_BORDER_COLLAPSE_INHERIT)
            {
                type = parent.GetBorderCollapse();
            }

            result.SetBorderCollapse(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_spacing
        static CssStatus PropDispCascade_border_spacing(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_spacing(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_spacing(CssSelectState state)
        {
            state.Computed.SetBorderSpacing(CssBorderSpacingEnum.CSS_BORDER_SPACING_SET,
                    Fixed.F_0, CssUnit.CSS_UNIT_PX, Fixed.F_0, CssUnit.CSS_UNIT_PX);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_spacing(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed hlength = Fixed.F_0, vlength = Fixed.F_0;
            CssUnit hunit = CssUnit.CSS_UNIT_PX, vunit = CssUnit.CSS_UNIT_PX;
            var type = child.GetBorderSpacing(ref hlength, ref hunit, ref vlength, ref vunit);

            if (type == CssBorderSpacingEnum.CSS_BORDER_SPACING_INHERIT)
            {
                type = parent.GetBorderSpacing(ref hlength, ref hunit, ref vlength, ref vunit);
            }

            result.SetBorderSpacing(type, hlength, hunit, vlength, vunit);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_top_color
        static CssStatus PropDispCascade_border_top_color(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_top_color(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_top_color(CssSelectState state)
        {
            state.Computed.SetBorderTopColor(CssBorderColorEnum.CSS_BORDER_COLOR_CURRENT_COLOR, new Color(0));
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_top_color(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Color color;
            var type = child.GetBorderTopColor(out color);

            if (type == CssBorderColorEnum.CSS_BORDER_COLOR_INHERIT)
            {
                type = parent.GetBorderTopColor(out color);
            }

            result.SetBorderTopColor(type, color);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_right_color
        static CssStatus PropDispCascade_border_right_color(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_right_color(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_right_color(CssSelectState state)
        {
            state.Computed.SetBorderRightColor(CssBorderColorEnum.CSS_BORDER_COLOR_CURRENT_COLOR, new Color(0));
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_right_color(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Color color;
            var type = child.GetBorderRightColor(out color);

            if (type == CssBorderColorEnum.CSS_BORDER_COLOR_INHERIT)
            {
                type = parent.GetBorderRightColor(out color);
            }

            result.SetBorderRightColor(type, color);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_bottom_color
        static CssStatus PropDispCascade_border_bottom_color(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_bottom_color(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_bottom_color(CssSelectState state)
        {
            state.Computed.SetBorderBottomColor(CssBorderColorEnum.CSS_BORDER_COLOR_CURRENT_COLOR, new Color(0));
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_bottom_color(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Color color;
            var type = child.GetBorderBottomColor(out color);

            if (type == CssBorderColorEnum.CSS_BORDER_COLOR_INHERIT)
            {
                type = parent.GetBorderBottomColor(out color);
            }

            result.SetBorderBottomColor(type, color);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_left_color
        static CssStatus PropDispCascade_border_left_color(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_left_color(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_left_color(CssSelectState state)
        {
            state.Computed.SetBorderLeftColor(CssBorderColorEnum.CSS_BORDER_COLOR_CURRENT_COLOR, new Color(0));
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_left_color(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Color color;
            var type = child.GetBorderLeftColor(out color);

            if (type == CssBorderColorEnum.CSS_BORDER_COLOR_INHERIT)
            {
                type = parent.GetBorderLeftColor(out color);
            }

            result.SetBorderLeftColor(type, color);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_top_style
        static CssStatus PropDispCascade_border_top_style(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_top_style(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_top_style(CssSelectState state)
        {
            state.Computed.SetBorderTopStyle(CssBorderStyleEnum.CSS_BORDER_STYLE_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_top_style(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBorderTopStyle();

            if (type == CssBorderStyleEnum.CSS_BORDER_STYLE_INHERIT)
            {
                type = parent.GetBorderTopStyle();
            }

            result.SetBorderTopStyle(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_right_style
        static CssStatus PropDispCascade_border_right_style(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_right_style(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_right_style(CssSelectState state)
        {
            state.Computed.SetBorderRightStyle(CssBorderStyleEnum.CSS_BORDER_STYLE_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_right_style(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBorderRightStyle();

            if (type == CssBorderStyleEnum.CSS_BORDER_STYLE_INHERIT)
            {
                type = parent.GetBorderRightStyle();
            }

            result.SetBorderRightStyle(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_bottom_style
        static CssStatus PropDispCascade_border_bottom_style(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_bottom_style(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_bottom_style(CssSelectState state)
        {
            state.Computed.SetBorderBottomStyle(CssBorderStyleEnum.CSS_BORDER_STYLE_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_bottom_style(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBorderBottomStyle();

            if (type == CssBorderStyleEnum.CSS_BORDER_STYLE_INHERIT)
            {
                type = parent.GetBorderBottomStyle();
            }

            result.SetBorderBottomStyle(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_left_style
        static CssStatus PropDispCascade_border_left_style(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_left_style(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_left_style(CssSelectState state)
        {
            state.Computed.SetBorderLeftStyle(CssBorderStyleEnum.CSS_BORDER_STYLE_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_left_style(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBorderLeftStyle();

            if (type == CssBorderStyleEnum.CSS_BORDER_STYLE_INHERIT)
            {
                type = parent.GetBorderLeftStyle();
            }

            result.SetBorderLeftStyle(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_top_width
        static CssStatus PropDispCascade_border_top_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_top_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_top_width(CssSelectState state)
        {
            state.Computed.SetBorderTopWidth(CssBorderWidthEnum.CSS_BORDER_WIDTH_MEDIUM, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_top_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetBorderTopWidth(ref length, ref unit);

            if (type == CssBorderWidthEnum.CSS_BORDER_WIDTH_INHERIT)
            {
                type = parent.GetBorderTopWidth(ref length, ref unit);
            }

            result.SetBorderTopWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_right_width
        static CssStatus PropDispCascade_border_right_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_right_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_right_width(CssSelectState state)
        {
            state.Computed.SetBorderRightWidth(CssBorderWidthEnum.CSS_BORDER_WIDTH_MEDIUM, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_right_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetBorderRightWidth(ref length, ref unit);

            if (type == CssBorderWidthEnum.CSS_BORDER_WIDTH_INHERIT)
            {
                type = parent.GetBorderRightWidth(ref length, ref unit);
            }

            result.SetBorderRightWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_bottom_width
        static CssStatus PropDispCascade_border_bottom_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_bottom_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_bottom_width(CssSelectState state)
        {
            state.Computed.SetBorderBottomWidth(CssBorderWidthEnum.CSS_BORDER_WIDTH_MEDIUM, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_bottom_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetBorderBottomWidth(ref length, ref unit);

            if (type == CssBorderWidthEnum.CSS_BORDER_WIDTH_INHERIT)
            {
                type = parent.GetBorderBottomWidth(ref length, ref unit);
            }

            result.SetBorderBottomWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region border_left_width
        static CssStatus PropDispCascade_border_left_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_border_left_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_border_left_width(CssSelectState state)
        {
            state.Computed.SetBorderLeftWidth(CssBorderWidthEnum.CSS_BORDER_WIDTH_MEDIUM, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_border_left_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetBorderLeftWidth(ref length, ref unit);

            if (type == CssBorderWidthEnum.CSS_BORDER_WIDTH_INHERIT)
            {
                type = parent.GetBorderLeftWidth(ref length, ref unit);
            }

            result.SetBorderLeftWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region bottom
        static CssStatus PropDispCascade_bottom(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_bottom(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_bottom(CssSelectState state)
        {
            state.Computed.SetBottom(CssBottomEnum.CSS_BOTTOM_AUTO, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_bottom(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetBottom(ref length, ref unit);

            if (type == CssBottomEnum.CSS_BOTTOM_INHERIT)
            {
                type = parent.GetBottom(ref length, ref unit);
            }

            result.SetBottom(type, length, unit);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region caption_side
        static CssStatus PropDispCascade_caption_side(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_caption_side(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_caption_side(CssSelectState state)
        {
            state.Computed.SetCaptionSide(CssCaptionSideEnum.CSS_CAPTION_SIDE_TOP);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_caption_side(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetCaptionSide();

            if (type == CssCaptionSideEnum.CSS_CAPTION_SIDE_INHERIT)
            {
                type = parent.GetCaptionSide();
            }

            result.SetCaptionSide(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region clear
        static CssStatus PropDispCascade_clear(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_clear(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_clear(CssSelectState state)
        {
            state.Computed.SetClear(CssClearEnum.CSS_CLEAR_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_clear(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetClear();

            if (type == CssClearEnum.CSS_CLEAR_INHERIT)
            {
                type = parent.GetClear();
            }

            result.SetClear(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region clip
        static CssStatus PropDispCascade_clip(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_clip(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_clip(CssSelectState state)
        {
            CssComputedClipRect rect = new CssComputedClipRect();
            rect.Top = rect.Bottom = rect.Left = rect.Right = Fixed.F_0;
            rect.Bunit = CssUnit.CSS_UNIT_PX;
            rect.Tunit = CssUnit.CSS_UNIT_PX;
            rect.Lunit = CssUnit.CSS_UNIT_PX;
            rect.Runit = CssUnit.CSS_UNIT_PX;

            state.Computed.SetClip(CssClipEnum.CSS_CLIP_AUTO, rect);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_clip(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            CssComputedClipRect rect = new CssComputedClipRect();
            rect.Top = rect.Bottom = rect.Left = rect.Right = Fixed.F_0;
            rect.Bunit = CssUnit.CSS_UNIT_PX;
            rect.Tunit = CssUnit.CSS_UNIT_PX;
            rect.Lunit = CssUnit.CSS_UNIT_PX;
            rect.Runit = CssUnit.CSS_UNIT_PX;

            var type = child.GetClip(ref rect);

            if (type == CssClipEnum.CSS_CLIP_INHERIT)
            {
                type = parent.GetClip(ref rect);
            }

            result.SetClip(type, rect);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region color
        // libcss/src/select/properties/color.c
        // css__[cascade/set/initial/compose/]_color
        static CssStatus PropDispCascade_color(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            bool inherit = op.IsInherit();
            var value = CssColorEnum.CSS_COLOR_INHERIT;
            Color color = new Color(0);

            if (inherit == false)
            {
                switch ((OpColor)op.GetValue())
                {
                    case OpColor.COLOR_TRANSPARENT:
                        value = CssColorEnum.CSS_COLOR_COLOR;
                        break;
                    case OpColor.COLOR_CURRENT_COLOR:
                        // color: currentColor always computes to inherit
                        value = CssColorEnum.CSS_COLOR_INHERIT;
                        inherit = true;
                        break;
                    case OpColor.COLOR_SET:
                        value = CssColorEnum.CSS_COLOR_COLOR;
                        color.Value = style.Bytecode[bi].GetOPV(); //color = *((css_color*)style->bytecode);
                        //advance_bytecode(style, sizeof(color));
                        bi++;
                        used--;
                        break;
                }
            }

            if (state.OutranksExisting(op, op.IsImportant(), inherit))
            {
                state.Computed.SetColor((byte)value, color);
                return CssStatus.CSS_OK;
            }

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_color(CssHint hint, ComputedStyle style)
        {
            style.SetColor(hint.Status, hint.Color);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_color(CssSelectState state)
        {
            var hint = UserAgentDefaultForProperty(CssPropertiesEnum.CSS_PROP_COLOR);
            PropDispSFH_color(hint, state.Computed);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_color(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Color color;
            var type = child.GetColor(out color);

            if (type == (byte)CssColorEnum.CSS_COLOR_INHERIT)
            {
                type = parent.GetColor(out color);
            }

            result.SetColor(type, color);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region content
        static CssStatus PropDispCascade_content(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_content(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_content(CssSelectState state)
        {
            var temp = new CssComputedContentItem[0];
            state.Computed.SetContent(CssContent.CSS_CONTENT_NORMAL, ref temp);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_content(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            CssComputedContentItem[] items = new CssComputedContentItem[0];
            CssComputedContentItem[] copy = new CssComputedContentItem[0];
            var type = child.GetContent(ref items);

            if (type == CssContent.CSS_CONTENT_INHERIT)
            {
                type = parent.GetContent(ref items);
            }

            if (type == CssContent.CSS_CONTENT_SET)
                Array.Copy(items, copy, items.Length);

            result.SetContent(type, ref copy);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region counter_increment
        static CssStatus PropDispCascade_counter_increment(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_counter_increment(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_counter_increment(CssSelectState state)
        {
            var temp = new CssComputedCounter[0];
            state.Computed.SetCounterIncrement(CssCounterIncrementEnum.CSS_COUNTER_INCREMENT_NONE, temp);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_counter_increment(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            CssComputedCounter[] items = new CssComputedCounter[0];
            CssComputedCounter[] copy = new CssComputedCounter[0];
            var type = child.GetCounterIncrement(ref items);

            if (type == CssCounterIncrementEnum.CSS_COUNTER_INCREMENT_INHERIT)
            {
                type = parent.GetCounterIncrement(ref items);
            }

            if (type == CssCounterIncrementEnum.CSS_COUNTER_INCREMENT_NAMED && items.Length > 0)
                Array.Copy(items, copy, items.Length);

            result.SetCounterIncrement(type, copy);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region counter_reset
        static CssStatus PropDispCascade_counter_reset(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_counter_reset(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_counter_reset(CssSelectState state)
        {
            var temp = new CssComputedCounter[0];
            state.Computed.SetCounterReset(CssCounterResetEnum.CSS_COUNTER_RESET_NONE, temp);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_counter_reset(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            CssComputedCounter[] items = new CssComputedCounter[0];
            CssComputedCounter[] copy = new CssComputedCounter[0];
            var type = child.GetCounterReset(ref items);

            if (type == CssCounterResetEnum.CSS_COUNTER_RESET_INHERIT)
            {
                type = parent.GetCounterReset(ref items);
            }

            if (type == CssCounterResetEnum.CSS_COUNTER_RESET_NAMED && items.Length > 0)
                Array.Copy(items, copy, items.Length);

            result.SetCounterReset(type, copy);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region cue_after
        static CssStatus PropDispCascade_cue_after(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_cue_after(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_cue_after(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_cue_after(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region cue_before
        static CssStatus PropDispCascade_cue_before(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_cue_before(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_cue_before(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_cue_before(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region cursor
        static CssStatus PropDispCascade_cursor(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_cursor(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_cursor(CssSelectState state)
        {
            state.Computed.SetCursor(CssCursorEnum.CSS_CURSOR_AUTO, new string[0]);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_cursor(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            string[] urls = new string[0];
            string[] copy; 

            var type = child.GetCursor(ref urls);

            if (type == CssCursorEnum.CSS_CURSOR_INHERIT)
            {
                type = parent.GetCursor(ref urls);
            }

            if (urls.Length > 0)
            {
                copy = new string[urls.Length];
                for (int i=0;i<urls.Length;i++)
                {
                    copy[i] = urls[i];
                }
            }
            else
                copy = new string[0];

            result.SetCursor(type, copy);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region direction
        static CssStatus PropDispCascade_direction(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_direction(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_direction(CssSelectState state)
        {
            state.Computed.SetDirection(CssDirectionEnum.CSS_DIRECTION_LTR);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_direction(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetDirection();

            if (type == CssDirectionEnum.CSS_DIRECTION_INHERIT)
            {
                type = parent.GetDirection();
            }

            result.SetDirection(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region display
        static CssStatus PropDispCascade_display(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
			bool inherit = op.IsInherit();
			CssDisplay value = CssDisplay.CSS_DISPLAY_INHERIT;

			if (inherit == false)
			{
				switch ((OpDisplay)op.GetValue())
				{
					case OpDisplay.DISPLAY_INLINE:
						value = CssDisplay.CSS_DISPLAY_INLINE;
						break;
					case OpDisplay.DISPLAY_BLOCK:
						value = CssDisplay.CSS_DISPLAY_BLOCK;
						break;
					case OpDisplay.DISPLAY_LIST_ITEM:
						value = CssDisplay.CSS_DISPLAY_LIST_ITEM;
						break;
					case OpDisplay.DISPLAY_RUN_IN:
						value = CssDisplay.CSS_DISPLAY_RUN_IN;
						break;
					case OpDisplay.DISPLAY_INLINE_BLOCK:
						value = CssDisplay.CSS_DISPLAY_INLINE_BLOCK;
						break;
					case OpDisplay.DISPLAY_TABLE:
						value = CssDisplay.CSS_DISPLAY_TABLE;
						break;
					case OpDisplay.DISPLAY_INLINE_TABLE:
						value = CssDisplay.CSS_DISPLAY_INLINE_TABLE;
						break;
					case OpDisplay.DISPLAY_TABLE_ROW_GROUP:
						value = CssDisplay.CSS_DISPLAY_TABLE_ROW_GROUP;
						break;
					case OpDisplay.DISPLAY_TABLE_HEADER_GROUP:
						value = CssDisplay.CSS_DISPLAY_TABLE_HEADER_GROUP;
						break;
					case OpDisplay.DISPLAY_TABLE_FOOTER_GROUP:
						value = CssDisplay.CSS_DISPLAY_TABLE_FOOTER_GROUP;
						break;
					case OpDisplay.DISPLAY_TABLE_ROW:
						value = CssDisplay.CSS_DISPLAY_TABLE_ROW;
						break;
					case OpDisplay.DISPLAY_TABLE_COLUMN_GROUP:
						value = CssDisplay.CSS_DISPLAY_TABLE_COLUMN_GROUP;
						break;
					case OpDisplay.DISPLAY_TABLE_COLUMN:
						value = CssDisplay.CSS_DISPLAY_TABLE_COLUMN;
						break;
					case OpDisplay.DISPLAY_TABLE_CELL:
						value = CssDisplay.CSS_DISPLAY_TABLE_CELL;
						break;
					case OpDisplay.DISPLAY_TABLE_CAPTION:
						value = CssDisplay.CSS_DISPLAY_TABLE_CAPTION;
						break;
					case OpDisplay.DISPLAY_NONE:
						value = CssDisplay.CSS_DISPLAY_NONE;
						break;
					case OpDisplay.DISPLAY_FLEX:
						value = CssDisplay.CSS_DISPLAY_FLEX;
						break;
					case OpDisplay.DISPLAY_INLINE_FLEX:
						value = CssDisplay.CSS_DISPLAY_INLINE_FLEX;
						break;
				}
			}

			if (state.OutranksExisting(op, op.IsImportant(), inherit))
			{
				state.Computed.SetDisplay(value);
				return CssStatus.CSS_OK;
			}

			return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_display(CssHint hint, ComputedStyle style)
        {
            CssDisplay display = (CssDisplay)hint.Status;
            style.SetDisplay(display);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_display(CssSelectState state)
        {
            state.Computed.SetDisplay(CssDisplay.CSS_DISPLAY_INLINE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_display(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            CssDisplay type = (CssDisplay)child.GetDisplay();

            if (type == CssDisplay.CSS_DISPLAY_INHERIT)
                type = (CssDisplay)parent.GetDisplay();

            result.SetDisplay(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region elevation
        static CssStatus PropDispCascade_elevation(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_elevation(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_elevation(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_elevation(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region empty_cells
        static CssStatus PropDispCascade_empty_cells(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_empty_cells(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_empty_cells(CssSelectState state)
        {
            state.Computed.SetEmptyCells(CssEmptyCellsEnum.CSS_EMPTY_CELLS_SHOW);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_empty_cells(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetEmptyCells();

            if (type == CssEmptyCellsEnum.CSS_EMPTY_CELLS_INHERIT)
            {
                type = parent.GetEmptyCells();
            }

            result.SetEmptyCells(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region float
        static CssStatus PropDispCascade_float(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_float(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_float(CssSelectState state)
        {
            state.Computed.SetFloat(CssFloat.CSS_FLOAT_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_float(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetFloat();

            if (type == CssFloat.CSS_FLOAT_INHERIT)
            {
                type = parent.GetFloat();
            }

            result.SetFloat(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region font_family
        static CssStatus PropDispCascade_font_family(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_font_family(CssHint hint, ComputedStyle style)
        {
            style.SetFontFamily((CssFontFamilyEnum)hint.Status, hint.Strings);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_font_family(CssSelectState state)
        {
            var hint = UserAgentDefaultForProperty(CssPropertiesEnum.CSS_PROP_FONT_FAMILY);
            PropDispSFH_font_family(hint, state.Computed);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_font_family(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            string[] names;

            var type = child.GetFontFamily(out names);

            if (type == CssFontFamilyEnum.CSS_FONT_FAMILY_INHERIT || result != child)
            {
                string[] copy;
                if (type == CssFontFamilyEnum.CSS_FONT_FAMILY_INHERIT)
                    type = parent.GetFontFamily(out names);

                if (names.Length > 0)
                {
                    copy = new string[names.Length];
                    for (int i = 0; i < names.Length; i++)
                        copy[i] = names[i];//copy[i] = string.Copy(names[i]);
                }
                else
                {
                    copy = new string[0];
                }

                result.SetFontFamily(type, copy);
            }

            return CssStatus.CSS_OK;
        }
        #endregion

        #region font_size
        static CssStatus PropDispCascade_font_size(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            CssFontSizeEnum value = CssFontSizeEnum.CSS_FONT_SIZE_INHERIT;
            Fixed size = Fixed.F_0;
            OpcodeUnit unit = OpcodeUnit.PX;

            if (!op.IsInherit())
            {
                switch (op.GetValue())
                {
                    case OpCodeValues.FONT_SIZE_DIMENSION:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION;

                        //size = *((css_fixed*)style->bytecode);
                        //advance_bytecode(style, sizeof(size));
                        size.RawValue = (int)style.Bytecode[bi].GetOPV();
                        bi++;
                        used--;

                        //unit = *((uint32_t*)style->bytecode);
                        //advance_bytecode(style, sizeof(unit));
                        unit = (OpcodeUnit)style.Bytecode[bi].GetOPV();
                        bi++;
                        used--;
                        break;
                    case OpCodeValues.FONT_SIZE_XX_SMALL:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_XX_SMALL;
                        break;
                    case OpCodeValues.FONT_SIZE_X_SMALL:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_X_SMALL;
                        break;
                    case OpCodeValues.FONT_SIZE_SMALL:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_SMALL;
                        break;
                    case OpCodeValues.FONT_SIZE_MEDIUM:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_MEDIUM;
                        break;
                    case OpCodeValues.FONT_SIZE_LARGE:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_LARGE;
                        break;
                    case OpCodeValues.FONT_SIZE_X_LARGE:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_X_LARGE;
                        break;
                    case OpCodeValues.FONT_SIZE_XX_LARGE:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_XX_LARGE;
                        break;
                    case OpCodeValues.FONT_SIZE_LARGER:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_LARGER;
                        break;
                    case OpCodeValues.FONT_SIZE_SMALLER:
                        value = CssFontSizeEnum.CSS_FONT_SIZE_SMALLER;
                        break;
                    default:
                        Log.Unimplemented();
                        break;
                }
            }

            var cssunit = CssUnitCtx.FromOpcodeUnit(unit);

            if (state.OutranksExisting(op, op.IsImportant(), op.IsInherit()))
            {
                state.Computed.SetFontSize(value, size, cssunit);
            }

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_font_size(CssHint hint, ComputedStyle style)
        {
            style.SetFontSize((CssFontSizeEnum)hint.Status,
                    hint.Length.Value, hint.Length.Unit);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_font_size(CssSelectState state)
        {
            state.Computed.SetFontSize(CssFontSizeEnum.CSS_FONT_SIZE_MEDIUM,
                        new Fixed(0),
                        CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_font_size(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed size = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            CssFontSizeEnum type = (CssFontSizeEnum)child.GetFontSize(ref size, ref unit);

            if (type == CssFontSizeEnum.CSS_FONT_SIZE_INHERIT)
            {
                type = (CssFontSizeEnum)parent.GetFontSize(ref size, ref unit);
            }

            result.SetFontSize(type, size, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region font_style
        static CssStatus PropDispCascade_font_style(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_font_style(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_font_style(CssSelectState state)
        {
            state.Computed.SetFontStyle(CssFontStyleEnum.CSS_FONT_STYLE_NORMAL);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_font_style(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetFontStyle();

            if (type == CssFontStyleEnum.CSS_FONT_STYLE_INHERIT)
            {
                type = parent.GetFontStyle();
            }

            result.SetFontStyle(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region font_variant
        static CssStatus PropDispCascade_font_variant(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_font_variant(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_font_variant(CssSelectState state)
        {
            state.Computed.SetFontVariant(CssFontVariantEnum.CSS_FONT_VARIANT_NORMAL);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_font_variant(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetFontVariant();

            if (type == CssFontVariantEnum.CSS_FONT_VARIANT_INHERIT)
            {
                type = parent.GetFontVariant();
            }

            result.SetFontVariant(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region font_weight
        static CssStatus PropDispCascade_font_weight(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_font_weight(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_font_weight(CssSelectState state)
        {
            state.Computed.SetFontWeight(CssFontWeightEnum.CSS_FONT_WEIGHT_NORMAL);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_font_weight(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetFontWeight();

            if (type == CssFontWeightEnum.CSS_FONT_WEIGHT_INHERIT)
            {
                type = parent.GetFontWeight();
            }

            result.SetFontWeight(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region height
        static CssStatus PropDispCascade_height(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_height(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_height(CssSelectState state)
        {
            state.Computed.SetHeight(CssHeightEnum.CSS_HEIGHT_AUTO, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_height(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetHeight(ref length, ref unit);

            if (type == CssHeightEnum.CSS_HEIGHT_INHERIT)
            {
                type = parent.GetHeight(ref length, ref unit);
            }

            result.SetHeight(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region left
        static CssStatus PropDispCascade_left(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_left(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_left(CssSelectState state)
        {
            state.Computed.SetLeft(CssLeftEnum.CSS_LEFT_AUTO, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_left(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetLeft(ref length, ref unit);

            if (type == CssLeftEnum.CSS_LEFT_INHERIT)
            {
                type = parent.GetLeft(ref length, ref unit);
            }

            result.SetLeft(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region letter_spacing
        static CssStatus PropDispCascade_letter_spacing(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_letter_spacing(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_letter_spacing(CssSelectState state)
        {
            state.Computed.SetLetterSpacing(CssLetterSpacingEnum.CSS_LETTER_SPACING_NORMAL, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_letter_spacing(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetLetterSpacing(ref length, ref unit);

            if (type == CssLetterSpacingEnum.CSS_LETTER_SPACING_INHERIT)
            {
                type = parent.GetLetterSpacing(ref length, ref unit);
            }

            result.SetLetterSpacing(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region line_height
        static CssStatus PropDispCascade_line_height(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
			bool inherit = op.IsInherit();
			var value = CssLineHeightEnum.CSS_LINE_HEIGHT_INHERIT;
			Fixed val = Fixed.F_0;
			var unit = OpcodeUnit.PX;

			if (inherit == false)
			{
				switch ((OpLineHeight)op.GetValue())
				{
					case OpLineHeight.LINE_HEIGHT_NUMBER:
						value = CssLineHeightEnum.CSS_LINE_HEIGHT_NUMBER;
						val = new Fixed((int)style.Bytecode[bi].GetOPV(), true);//val = *((css_fixed*)style->bytecode);
						//advance_bytecode(style, sizeof(val));
						bi++; used--;
						break;
					case OpLineHeight.LINE_HEIGHT_DIMENSION:
						value = CssLineHeightEnum.CSS_LINE_HEIGHT_DIMENSION;
						val = new Fixed((int)style.Bytecode[bi].GetOPV(), true);//val = *((css_fixed*)style->bytecode);
						//advance_bytecode(style, sizeof(val));
						bi++; used--;
						unit = (OpcodeUnit)style.Bytecode[bi].GetOPV();//unit = *((uint32_t*)style->bytecode);
						//advance_bytecode(style, sizeof(unit));
						bi++; used--;
						break;
					case OpLineHeight.LINE_HEIGHT_NORMAL:
						value = CssLineHeightEnum.CSS_LINE_HEIGHT_NORMAL;
						break;
				}
			}

			var cssunit = CssUnitCtx.FromOpcodeUnit(unit);

			if (state.OutranksExisting(op, op.IsImportant(), inherit))
			{
				state.Computed.SetLineHeight(value, val, cssunit);
			}

			return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_line_height(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_line_height(CssSelectState state)
        {
            state.Computed.SetLineHeight(CssLineHeightEnum.CSS_LINE_HEIGHT_NORMAL,
                Fixed.F_0, CssUnit.CSS_UNIT_PX);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_line_height(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetLineHeight(ref length, ref unit);

            if (type == CssLineHeightEnum.CSS_LINE_HEIGHT_INHERIT)
            {
                type = parent.GetLineHeight(ref length, ref unit);
            }

            result.SetLineHeight(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region list_style_image
        static CssStatus PropDispCascade_list_style_image(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_list_style_image(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_list_style_image(CssSelectState state)
        {
            state.Computed.SetListStyleImage(CssListStyleImageEnum.CSS_LIST_STYLE_IMAGE_NONE,
                string.Empty);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_list_style_image(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            string url = string.Empty;
            var type = child.GetListStyleImage(ref url);

            if (type == CssListStyleImageEnum.CSS_LIST_STYLE_IMAGE_INHERIT)
            {
                type = parent.GetListStyleImage(ref url);
            }

            result.SetListStyleImage(type, url);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region list_style_position
        static CssStatus PropDispCascade_list_style_position(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_list_style_position(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_list_style_position(CssSelectState state)
        {
            state.Computed.SetListStylePosition(CssListStylePositionEnum.CSS_LIST_STYLE_POSITION_OUTSIDE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_list_style_position(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetListStylePosition();

            if (type == CssListStylePositionEnum.CSS_LIST_STYLE_POSITION_INHERIT)
            {
                type = parent.GetListStylePosition();
            }

            result.SetListStylePosition(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region list_style_type
        static CssStatus PropDispCascade_list_style_type(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_list_style_type(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_list_style_type(CssSelectState state)
        {
            state.Computed.SetListStyleType(CssListStyleTypeEnum.CSS_LIST_STYLE_TYPE_DISC);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_list_style_type(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetListStyleType();

            if (type == CssListStyleTypeEnum.CSS_LIST_STYLE_TYPE_INHERIT)
            {
                type = parent.GetListStyleType();
            }

            result.SetListStyleType(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region margin_top
        static CssStatus PropDispCascade_margin_top(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            return CascadeLengthAuto(op, style, ref bi, ref used, state, "margin-top");
        }

        static CssStatus PropDispSFH_margin_top(CssHint hint, ComputedStyle style)
        {
            style.SetMarginTop((CssMarginEnum)hint.Status, hint.Length.Value, hint.Length.Unit);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_margin_top(CssSelectState state)
        {
            state.Computed.SetMarginTop(CssMarginEnum.CSS_MARGIN_SET, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_margin_top(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetMarginTop(ref length, ref unit);

            if (type == CssMarginEnum.CSS_MARGIN_INHERIT)
            {
                type = parent.GetMarginTop(ref length, ref unit);
            }

            result.SetMarginTop(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region margin_right
        static CssStatus PropDispCascade_margin_right(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
			return CascadeLengthAuto(op, style, ref bi, ref used, state, "margin-right");
        }

        static CssStatus PropDispSFH_margin_right(CssHint hint, ComputedStyle style)
        {
            style.SetMarginRight((CssMarginEnum)hint.Status, hint.Length.Value, hint.Length.Unit);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_margin_right(CssSelectState state)
        {
            state.Computed.SetMarginRight(CssMarginEnum.CSS_MARGIN_SET, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_margin_right(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetMarginRight(ref length, ref unit);

            if (type == CssMarginEnum.CSS_MARGIN_INHERIT)
            {
                type = parent.GetMarginRight(ref length, ref unit);
            }

            result.SetMarginRight(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region margin_bottom
        static CssStatus PropDispCascade_margin_bottom(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
			return CascadeLengthAuto(op, style, ref bi, ref used, state, "margin-bottom");
        }

        static CssStatus PropDispSFH_margin_bottom(CssHint hint, ComputedStyle style)
        {
            style.SetMarginBottom((CssMarginEnum)hint.Status, hint.Length.Value, hint.Length.Unit);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_margin_bottom(CssSelectState state)
        {
            state.Computed.SetMarginBottom(CssMarginEnum.CSS_MARGIN_SET, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_margin_bottom(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetMarginBottom(ref length, ref unit);

            if (type == CssMarginEnum.CSS_MARGIN_INHERIT)
            {
                type = parent.GetMarginBottom(ref length, ref unit);
            }

            result.SetMarginBottom(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region margin_left
        static CssStatus PropDispCascade_margin_left(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
			return CascadeLengthAuto(op, style, ref bi, ref used, state, "margin-left");
		}

		static CssStatus PropDispSFH_margin_left(CssHint hint, ComputedStyle style)
        {
            style.SetMarginLeft((CssMarginEnum)hint.Status, hint.Length.Value, hint.Length.Unit);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_margin_left(CssSelectState state)
        {
            state.Computed.SetMarginLeft(CssMarginEnum.CSS_MARGIN_SET, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_margin_left(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetMarginLeft(ref length, ref unit);

            if (type == CssMarginEnum.CSS_MARGIN_INHERIT)
            {
                type = parent.GetMarginLeft(ref length, ref unit);
            }

            result.SetMarginLeft(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region max_height
        static CssStatus PropDispCascade_max_height(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_max_height(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_max_height(CssSelectState state)
        {
            state.Computed.SetMaxHeight(CssMaxHeightEnum.CSS_MAX_HEIGHT_NONE, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_max_height(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetMaxHeight(ref length, ref unit);

            if (type == CssMaxHeightEnum.CSS_MAX_HEIGHT_INHERIT)
            {
                type = parent.GetMaxHeight(ref length, ref unit);
            }

            result.SetMaxHeight(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region max_width
        static CssStatus PropDispCascade_max_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_max_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_max_width(CssSelectState state)
        {
            state.Computed.SetMaxWidth(CssMaxWidthEnum.CSS_MAX_WIDTH_NONE, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_max_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetMaxWidth(ref length, ref unit);

            if (type == CssMaxWidthEnum.CSS_MAX_WIDTH_INHERIT)
            {
                type = parent.GetMaxWidth(ref length, ref unit);
            }

            result.SetMaxWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region min_height
        static CssStatus PropDispCascade_min_height(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_min_height(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_min_height(CssSelectState state)
        {
            state.Computed.SetMinHeight(CssMinHeightEnum.CSS_MIN_HEIGHT_AUTO, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_min_height(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetMinHeight(ref length, ref unit);

            if (type == CssMinHeightEnum.CSS_MIN_HEIGHT_INHERIT)
            {
                type = parent.GetMinHeight(ref length, ref unit);
            }

            result.SetMinHeight(type, length, unit);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region min_width
        static CssStatus PropDispCascade_min_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_min_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_min_width(CssSelectState state)
        {
            state.Computed.SetMinWidth(CssMinWidthEnum.CSS_MIN_WIDTH_AUTO, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_min_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetMinWidth(ref length, ref unit);

            if (type == CssMinWidthEnum.CSS_MIN_WIDTH_INHERIT)
            {
                type = parent.GetMinWidth(ref length, ref unit);
            }

            result.SetMinWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region orphans
        static CssStatus PropDispCascade_orphans(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_orphans(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_orphans(CssSelectState state)
        {
            state.Computed.SetOrphans(CssOrphansEnum.CSS_ORPHANS_SET, 2);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_orphans(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            int count = 0;
            var type = child.GetOrphans(ref count);

            if (type == CssOrphansEnum.CSS_ORPHANS_INHERIT)
            {
                type = parent.GetOrphans(ref count);
            }

            result.SetOrphans(type, count);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region outline_color
        static CssStatus PropDispCascade_outline_color(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_outline_color(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_outline_color(CssSelectState state)
        {
            state.Computed.SetOutlineColor(CssOutlineColorEnum.CSS_OUTLINE_COLOR_INVERT, new Color(0));
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_outline_color(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Color color = new Color(0);
            var type = child.GetOutlineColor(ref color);

            if (type == CssOutlineColorEnum.CSS_OUTLINE_COLOR_INHERIT)
            {
                type = parent.GetOutlineColor(ref color);
            }

            result.SetOutlineColor(type, color);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region outline_style
        static CssStatus PropDispCascade_outline_style(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_outline_style(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_outline_style(CssSelectState state)
        {
            state.Computed.SetOutlineStyle(CssOutlineStyleEnum.CSS_OUTLINE_STYLE_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_outline_style(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetOutlineStyle();

            if (type == CssOutlineStyleEnum.CSS_OUTLINE_STYLE_INHERIT)
            {
                type = parent.GetOutlineStyle();
            }

            result.SetOutlineStyle(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region outline_width
        static CssStatus PropDispCascade_outline_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_outline_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_outline_width(CssSelectState state)
        {
            state.Computed.SetOutlineWidth(CssOutlineWidthEnum.CSS_OUTLINE_WIDTH_MEDIUM,
                Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_outline_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetOutlineWidth(ref length, ref unit);

            if (type == CssOutlineWidthEnum.CSS_OUTLINE_WIDTH_INHERIT)
            {
                type = parent.GetOutlineWidth(ref length, ref unit);
            }

            result.SetOutlineWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region overflow_x
        static CssStatus PropDispCascade_overflow_x(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_overflow_x(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_overflow_x(CssSelectState state)
        {
            state.Computed.SetOverflowX(CssOverflowEnum.CSS_OVERFLOW_VISIBLE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_overflow_x(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetOverflowX();

            if (type == CssOverflowEnum.CSS_OVERFLOW_INHERIT)
            {
                type = parent.GetOverflowX();
            }

            result.SetOverflowX(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region padding_top
        static CssStatus PropDispCascade_padding_top(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_padding_top(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_padding_top(CssSelectState state)
        {
            state.Computed.SetPaddingTop(CssPaddingEnum.CSS_PADDING_SET, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_padding_top(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetPaddingTop(ref length, ref unit);

            if (type == CssPaddingEnum.CSS_PADDING_INHERIT)
            {
                type = parent.GetPaddingTop(ref length, ref unit);
            }

            result.SetPaddingTop(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region padding_right
        static CssStatus PropDispCascade_padding_right(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_padding_right(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_padding_right(CssSelectState state)
        {
            state.Computed.SetPaddingRight(CssPaddingEnum.CSS_PADDING_SET, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_padding_right(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetPaddingRight(ref length, ref unit);

            if (type == CssPaddingEnum.CSS_PADDING_INHERIT)
            {
                type = parent.GetPaddingRight(ref length, ref unit);
            }

            result.SetPaddingRight(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region padding_bottom
        static CssStatus PropDispCascade_padding_bottom(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_padding_bottom(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_padding_bottom(CssSelectState state)
        {
            state.Computed.SetPaddingBottom(CssPaddingEnum.CSS_PADDING_SET, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_padding_bottom(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetPaddingBottom(ref length, ref unit);

            if (type == CssPaddingEnum.CSS_PADDING_INHERIT)
            {
                type = parent.GetPaddingBottom(ref length, ref unit);
            }

            result.SetPaddingBottom(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region padding_left
        static CssStatus PropDispCascade_padding_left(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_padding_left(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_padding_left(CssSelectState state)
        {
            state.Computed.SetPaddingLeft(CssPaddingEnum.CSS_PADDING_SET, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_padding_left(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetPaddingLeft(ref length, ref unit);

            if (type == CssPaddingEnum.CSS_PADDING_INHERIT)
            {
                type = parent.GetPaddingLeft(ref length, ref unit);
            }

            result.SetPaddingLeft(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region page_break_after
        static CssStatus PropDispCascade_page_break_after(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_page_break_after(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_page_break_after(CssSelectState state)
        {
            state.Computed.SetPageBreakAfter(CssPageBreakAfterEnum.CSS_PAGE_BREAK_AFTER_AUTO);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_page_break_after(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetPageBreakAfter();

            if (type == CssPageBreakAfterEnum.CSS_PAGE_BREAK_AFTER_INHERIT)
            {
                type = parent.GetPageBreakAfter();
            }

            result.SetPageBreakAfter(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region page_break_before
        static CssStatus PropDispCascade_page_break_before(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_page_break_before(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_page_break_before(CssSelectState state)
        {
            state.Computed.SetPageBreakBefore(CssPageBreakBeforeEnum.CSS_PAGE_BREAK_BEFORE_AUTO);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_page_break_before(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetPageBreakBefore();

            if (type == CssPageBreakBeforeEnum.CSS_PAGE_BREAK_BEFORE_INHERIT)
            {
                type = parent.GetPageBreakBefore();
            }

            result.SetPageBreakBefore(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region page_break_inside
        static CssStatus PropDispCascade_page_break_inside(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_page_break_inside(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_page_break_inside(CssSelectState state)
        {
            state.Computed.SetPageBreakInside(CssPageBreakInsideEnum.CSS_PAGE_BREAK_INSIDE_AUTO);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_page_break_inside(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetPageBreakInside();

            if (type == CssPageBreakInsideEnum.CSS_PAGE_BREAK_INSIDE_INHERIT)
            {
                type = parent.GetPageBreakInside();
            }

            result.SetPageBreakInside(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region pause_after
        static CssStatus PropDispCascade_pause_after(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_pause_after(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_pause_after(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_pause_after(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region pause_before
        static CssStatus PropDispCascade_pause_before(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_pause_before(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_pause_before(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_pause_before(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region pitch_range
        static CssStatus PropDispCascade_pitch_range(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_pitch_range(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_pitch_range(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_pitch_range(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region pitch
        static CssStatus PropDispCascade_pitch(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_pitch(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_pitch(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_pitch(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region play_during
        static CssStatus PropDispCascade_play_during(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_play_during(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_play_during(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_play_during(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region position
        static CssStatus PropDispCascade_position(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_position(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_position(CssSelectState state)
        {
            state.Computed.SetPosition(CssPosition.CSS_POSITION_STATIC);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_position(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetPosition();

            if (type == CssPosition.CSS_POSITION_INHERIT)
            {
                type = parent.GetPosition();
            }

            result.SetPosition(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region quotes
        static CssStatus PropDispCascade_quotes(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_quotes(CssHint hint, ComputedStyle style)
        {
            style.SetQuotes((CssQuotesEnum)hint.Status, hint.Strings);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_quotes(CssSelectState state)
        {
            var hint = UserAgentDefaultForProperty(CssPropertiesEnum.CSS_PROP_QUOTES);
            PropDispSFH_quotes(hint, state.Computed);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_quotes(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            string[] urls = new string[0];
            string[] copy;

            var type = child.GetQuotes(ref urls);

            if (type == CssQuotesEnum.CSS_QUOTES_INHERIT || result != child)
            {
                if (type == CssQuotesEnum.CSS_QUOTES_INHERIT)
                    type = parent.GetQuotes(ref urls);

                if (urls.Length > 0)
                {
                    copy = new string[urls.Length];
                    for (int i = 0; i < urls.Length; i++)
                    {
                        copy[i] = urls[i];
                    }
                }
                else
                    copy = new string[0];

                result.SetQuotes(type, copy);
            }

            return CssStatus.CSS_OK;
        }
        #endregion

        #region richness
        static CssStatus PropDispCascade_richness(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_richness(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_richness(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_richness(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region right
        static CssStatus PropDispCascade_right(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_right(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_right(CssSelectState state)
        {
            state.Computed.SetRight(CssRightEnum.CSS_RIGHT_AUTO, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_right(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetRight(ref length, ref unit);

            if (type == CssRightEnum.CSS_RIGHT_INHERIT)
            {
                type = parent.GetRight(ref length, ref unit);
            }

            result.SetRight(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region speak_header
        static CssStatus PropDispCascade_speak_header(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_speak_header(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_speak_header(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_speak_header(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region speak_numeral
        static CssStatus PropDispCascade_speak_numeral(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_speak_numeral(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_speak_numeral(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_speak_numeral(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region speak_punctuation
        static CssStatus PropDispCascade_speak_punctuation(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_speak_punctuation(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_speak_punctuation(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_speak_punctuation(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region speak
        static CssStatus PropDispCascade_speak(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_speak(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_speak(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_speak(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region speech_rate
        static CssStatus PropDispCascade_speech_rate(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_speech_rate(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_speech_rate(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_speech_rate(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region stress
        static CssStatus PropDispCascade_stress(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_stress(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_stress(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_stress(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region table_layout
        static CssStatus PropDispCascade_table_layout(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_table_layout(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_table_layout(CssSelectState state)
        {
            state.Computed.SetTableLayout(CssTableLayoutEnum.CSS_TABLE_LAYOUT_AUTO);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_table_layout(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetTableLayout();

            if (type == CssTableLayoutEnum.CSS_TABLE_LAYOUT_INHERIT)
            {
                type = parent.GetTableLayout();
            }

            result.SetTableLayout(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region text_align
        static CssStatus PropDispCascade_text_align(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_text_align(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_text_align(CssSelectState state)
        {
            state.Computed.SetTextAlign(CssTextAlignEnum.CSS_TEXT_ALIGN_DEFAULT);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_text_align(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetTextAlign();

            if (type == CssTextAlignEnum.CSS_TEXT_ALIGN_INHERIT)
            {
                type = parent.GetTextAlign();
            }
            else if (type == CssTextAlignEnum.CSS_TEXT_ALIGN_INHERIT_IF_NON_MAGIC)
            {
                // This is purely for the benefit of HTML tables
                type = parent.GetTextAlign();

                /* If the parent's text-align is a magical one,
                 * then reset to the default value. Otherwise,
                 * inherit as normal. */
                if (type == CssTextAlignEnum.CSS_TEXT_ALIGN_LIBCSS_LEFT ||
                        type == CssTextAlignEnum.CSS_TEXT_ALIGN_LIBCSS_CENTER ||
                        type == CssTextAlignEnum.CSS_TEXT_ALIGN_LIBCSS_RIGHT)
                    type = CssTextAlignEnum.CSS_TEXT_ALIGN_DEFAULT;
            }

            result.SetTextAlign(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region text_decoration
        static CssStatus PropDispCascade_text_decoration(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            bool inherit = op.IsInherit();
            CssTextDecorationEnum value = CssTextDecorationEnum.CSS_TEXT_DECORATION_INHERIT;

            if (inherit == false)
            {
                var opValue = (OpTextDecoration)op.GetValue();
                if (opValue == OpTextDecoration.TEXT_DECORATION_NONE)
                {
                    value = CssTextDecorationEnum.CSS_TEXT_DECORATION_NONE;
                }
                else
                {
                    Debug.Assert((byte)value == 0);

                    if ((opValue & OpTextDecoration.TEXT_DECORATION_UNDERLINE) != 0)
                        value |= CssTextDecorationEnum.CSS_TEXT_DECORATION_UNDERLINE;
                    if ((opValue & OpTextDecoration.TEXT_DECORATION_OVERLINE) != 0)
                        value |= CssTextDecorationEnum.CSS_TEXT_DECORATION_OVERLINE;
                    if ((opValue & OpTextDecoration.TEXT_DECORATION_LINE_THROUGH) != 0)
                        value |= CssTextDecorationEnum.CSS_TEXT_DECORATION_LINE_THROUGH;
                    if ((opValue & OpTextDecoration.TEXT_DECORATION_BLINK) != 0)
                        value |= CssTextDecorationEnum.CSS_TEXT_DECORATION_BLINK;
                }
            }

            if (state.OutranksExisting(op, op.IsImportant(), inherit))
            {
                state.Computed.SetTextDecoration(value);
                return CssStatus.CSS_OK;
            }

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_text_decoration(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_text_decoration(CssSelectState state)
        {
            state.Computed.SetTextDecoration(CssTextDecorationEnum.CSS_TEXT_DECORATION_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_text_decoration(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetTextDecoration();

            if (type == CssTextDecorationEnum.CSS_TEXT_DECORATION_INHERIT)
            {
                type = parent.GetTextDecoration();
            }

            result.SetTextDecoration(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region text_indent
        static CssStatus PropDispCascade_text_indent(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_text_indent(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_text_indent(CssSelectState state)
        {
            state.Computed.SetTextIndent(CssTextIndentEnum.CSS_TEXT_INDENT_SET, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_text_indent(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetTextIndent(ref length, ref unit);

            if (type == CssTextIndentEnum.CSS_TEXT_INDENT_INHERIT)
            {
                type = parent.GetTextIndent(ref length, ref unit);
            }

            result.SetTextIndent(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region text_transform
        static CssStatus PropDispCascade_text_transform(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_text_transform(CssHint hint, ComputedStyle style)
        {
            style.SetTextTransform((CssTextTransformEnum)hint.Status);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_text_transform(CssSelectState state)
        {
            state.Computed.SetTextTransform(CssTextTransformEnum.CSS_TEXT_TRANSFORM_NONE);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_text_transform(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetTextTransform();

            if (type == CssTextTransformEnum.CSS_TEXT_TRANSFORM_INHERIT)
                type = parent.GetTextTransform();

            result.SetTextTransform(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region top
        static CssStatus PropDispCascade_top(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_top(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_top(CssSelectState state)
        {
            state.Computed.SetTop(CssTopEnum.CSS_TOP_AUTO, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_top(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetTop(ref length, ref unit);

            if (type == CssTopEnum.CSS_TOP_INHERIT)
            {
                type = parent.GetTop(ref length, ref unit);
            }

            result.SetTop(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region unicode_bidi
        static CssStatus PropDispCascade_unicode_bidi(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_unicode_bidi(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_unicode_bidi(CssSelectState state)
        {
            state.Computed.SetUnicodeBidi(CssUnicodeBidiEnum.CSS_UNICODE_BIDI_NORMAL);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_unicode_bidi(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetUnicodeBidi();

            if (type == CssUnicodeBidiEnum.CSS_UNICODE_BIDI_INHERIT)
            {
                type = parent.GetUnicodeBidi();
            }

            result.SetUnicodeBidi(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region vertical_align
        static CssStatus PropDispCascade_vertical_align(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_vertical_align(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_vertical_align(CssSelectState state)
        {
            state.Computed.SetVerticalAlign(CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_BASELINE, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_vertical_align(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetVerticalAlign(ref length, ref unit);

            if (type == CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_INHERIT)
            {
                type = parent.GetVerticalAlign(ref length, ref unit);
            }

            result.SetVerticalAlign(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region visibility
        static CssStatus PropDispCascade_visibility(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_visibility(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_visibility(CssSelectState state)
        {
            state.Computed.SetVisibility(CssVisibilityEnum.CSS_VISIBILITY_VISIBLE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_visibility(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetVisibility();

            if (type == CssVisibilityEnum.CSS_VISIBILITY_INHERIT)
            {
                type = parent.GetVisibility();
            }

            result.SetVisibility(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region voice_family
        static CssStatus PropDispCascade_voice_family(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_voice_family(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_voice_family(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_voice_family(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region volume
        static CssStatus PropDispCascade_volume(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_volume(CssHint hint, ComputedStyle style)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_volume(CssSelectState state)
        {
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_volume(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            return CssStatus.CSS_OK;
        }
        #endregion

        #region white_space
        static CssStatus PropDispCascade_white_space(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_white_space(CssHint hint, ComputedStyle style)
        {
            style.SetWhitespace((CssWhiteSpaceEnum)hint.Status);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_white_space(CssSelectState state)
        {
            state.Computed.SetWhitespace(CssWhiteSpaceEnum.CSS_WHITE_SPACE_NORMAL);

            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_white_space(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetWhitespace();

            if (type == CssWhiteSpaceEnum.CSS_WHITE_SPACE_INHERIT)
                type = parent.GetWhitespace();

            result.SetWhitespace(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region widows
        static CssStatus PropDispCascade_widows(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_widows(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_widows(CssSelectState state)
        {
            state.Computed.SetWidows(CssWidowsEnum.CSS_WIDOWS_SET, 2);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_widows(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            int count = 0;
            var type = child.GetWidows(ref count);

            if (type == CssWidowsEnum.CSS_WIDOWS_INHERIT)
            {
                type = parent.GetWidows(ref count);
            }

            result.SetWidows(type, count);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region width
        static CssStatus PropDispCascade_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_width(CssSelectState state)
        {
            state.Computed.SetWidth(CssWidth.CSS_WIDTH_AUTO, Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            CssWidth type = (CssWidth)child.GetWidth(ref length, ref unit);

            if (type == CssWidth.CSS_WIDTH_INHERIT)
            {
                type = parent.GetWidth(ref length, ref unit);
            }

            result.SetWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region word_spacing
        static CssStatus PropDispCascade_word_spacing(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_word_spacing(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_word_spacing(CssSelectState state)
        {
            state.Computed.SetWordSpacing(CssWordSpacingEnum.CSS_WORD_SPACING_NORMAL,
                Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_word_spacing(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetWordSpacing(ref length, ref unit);

            if (type == CssWordSpacingEnum.CSS_WORD_SPACING_INHERIT)
            {
                type = parent.GetWordSpacing(ref length, ref unit);
            }

            result.SetWordSpacing(type, length, unit);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region z_index
        static CssStatus PropDispCascade_z_index(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_z_index(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_z_index(CssSelectState state)
        {
            state.Computed.SetZIndex(CssZindexEnum.CSS_Z_INDEX_AUTO, 0);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_z_index(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            int index = 0;
            var type = child.GetZIndex(ref index);

            if (type == CssZindexEnum.CSS_Z_INDEX_INHERIT)
            {
                type = parent.GetZIndex(ref index);
            }

            result.SetZIndex(type, index);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region opacity
        static CssStatus PropDispCascade_opacity(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_opacity(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_opacity(CssSelectState state)
        {
            state.Computed.SetOpacity(CssOpacityEnum.CSS_OPACITY_SET, new Fixed(1));
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_opacity(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed opacity = Fixed.F_0;
            var type = child.GetOpacity(ref opacity);

            if (type == CssOpacityEnum.CSS_OPACITY_INHERIT)
            {
                type = parent.GetOpacity(ref opacity);
            }

            result.SetOpacity(type, opacity);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region break_after
        static CssStatus PropDispCascade_break_after(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_break_after(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_break_after(CssSelectState state)
        {
            state.Computed.SetBreakAfter(CssBreakAfterEnum.CSS_BREAK_AFTER_AUTO);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_break_after(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBreakAfter();

            if (type == CssBreakAfterEnum.CSS_BREAK_AFTER_INHERIT)
            {
                type = parent.GetBreakAfter();
            }

            result.SetBreakAfter(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region break_before
        static CssStatus PropDispCascade_break_before(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_break_before(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_break_before(CssSelectState state)
        {
            state.Computed.SetBreakBefore(CssBreakBeforeEnum.CSS_BREAK_BEFORE_AUTO);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_break_before(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBreakBefore();

            if (type == CssBreakBeforeEnum.CSS_BREAK_BEFORE_INHERIT)
            {
                type = parent.GetBreakBefore();
            }

            result.SetBreakBefore(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region break_inside
        static CssStatus PropDispCascade_break_inside(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_break_inside(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_break_inside(CssSelectState state)
        {
            state.Computed.SetBreakInside(CssBreakInsideEnum.CSS_BREAK_INSIDE_AUTO);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_break_inside(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBreakInside();

            if (type == CssBreakInsideEnum.CSS_BREAK_INSIDE_INHERIT)
            {
                type = parent.GetBreakInside();
            }

            result.SetBreakInside(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region column_count
        static CssStatus PropDispCascade_column_count(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_column_count(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_column_count(CssSelectState state)
        {
            state.Computed.SetColumnCount(CssColumnCountEnum.CSS_COLUMN_COUNT_AUTO, 0);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_column_count(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            int count = 0;
            var type = child.GetColumnCount(ref count);

            if (type == CssColumnCountEnum.CSS_COLUMN_COUNT_INHERIT)
            {
                type = parent.GetColumnCount(ref count);
            }

            result.SetColumnCount(type, count);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region column_fill
        static CssStatus PropDispCascade_column_fill(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_column_fill(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_column_fill(CssSelectState state)
        {
            state.Computed.SetColumnFill(CssColumnFillEnum.CSS_COLUMN_FILL_BALANCE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_column_fill(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetColumnFill();

            if (type == CssColumnFillEnum.CSS_COLUMN_FILL_INHERIT)
            {
                type = parent.GetColumnFill();
            }

            result.SetColumnFill(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region column_gap
        static CssStatus PropDispCascade_column_gap(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_column_gap(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_column_gap(CssSelectState state)
        {
            state.Computed.SetColumnGap(CssColumnGapEnum.CSS_COLUMN_GAP_NORMAL,
                new Fixed(1), CssUnit.CSS_UNIT_EM);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_column_gap(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = new Fixed(1);
            CssUnit unit = CssUnit.CSS_UNIT_EM;
            var type = child.GetColumnGap(ref length, ref unit);

            if (type == CssColumnGapEnum.CSS_COLUMN_GAP_INHERIT)
            {
                type = parent.GetColumnGap(ref length, ref unit);
            }

            result.SetColumnGap(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region column_rule_color
        static CssStatus PropDispCascade_column_rule_color(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_column_rule_color(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_column_rule_color(CssSelectState state)
        {
            state.Computed.SetColumnRuleColor(CssColumnRuleColorEnum.CSS_COLUMN_RULE_COLOR_CURRENT_COLOR,
                new Color(0));
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_column_rule_color(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Color color = new Color(0);
            var type = child.GetColumnRuleColor(ref color);

            if (type == CssColumnRuleColorEnum.CSS_COLUMN_RULE_COLOR_INHERIT)
            {
                type = parent.GetColumnRuleColor(ref color);
            }

            result.SetColumnRuleColor(type, color);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region column_rule_style
        static CssStatus PropDispCascade_column_rule_style(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_column_rule_style(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_column_rule_style(CssSelectState state)
        {
            state.Computed.SetColumnRuleStyle(CssColumnRuleStyleEnum.CSS_COLUMN_RULE_STYLE_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_column_rule_style(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetColumnRuleStyle();

            if (type == CssColumnRuleStyleEnum.CSS_COLUMN_RULE_STYLE_INHERIT)
            {
                type = parent.GetColumnRuleStyle();
            }

            result.SetColumnRuleStyle(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region column_rule_width
        static CssStatus PropDispCascade_column_rule_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_column_rule_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_column_rule_width(CssSelectState state)
        {
            state.Computed.SetColumnRuleWidth(CssColumnRuleWidthEnum.CSS_COLUMN_RULE_WIDTH_MEDIUM,
                Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_column_rule_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetColumnRuleWidth(ref length, ref unit);

            if (type == CssColumnRuleWidthEnum.CSS_COLUMN_RULE_WIDTH_INHERIT)
            {
                type = parent.GetColumnRuleWidth(ref length, ref unit);
            }

            result.SetColumnRuleWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region column_span
        static CssStatus PropDispCascade_column_span(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_column_span(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_column_span(CssSelectState state)
        {
            state.Computed.SetColumnSpan(CssColumnSpanEnum.CSS_COLUMN_SPAN_NONE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_column_span(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetColumnSpan();

            if (type == CssColumnSpanEnum.CSS_COLUMN_SPAN_INHERIT)
            {
                type = parent.GetColumnSpan();
            }

            result.SetColumnSpan(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region column_width
        static CssStatus PropDispCascade_column_width(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_column_width(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_column_width(CssSelectState state)
        {
            state.Computed.SetColumnWidth(CssColumnWidthEnum.CSS_COLUMN_WIDTH_AUTO,
                new Fixed(1), CssUnit.CSS_UNIT_EM);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_column_width(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetColumnWidth(ref length, ref unit);

            if (type == CssColumnWidthEnum.CSS_COLUMN_WIDTH_INHERIT)
            {
                type = parent.GetColumnWidth(ref length, ref unit);
            }

            result.SetColumnWidth(type, length, unit);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region writing_mode
        static CssStatus PropDispCascade_writing_mode(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_writing_mode(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_writing_mode(CssSelectState state)
        {
            state.Computed.SetWritingMode(CssWritingModeEnum.CSS_WRITING_MODE_HORIZONTAL_TB);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_writing_mode(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetWritingMode();

            if (type == CssWritingModeEnum.CSS_WRITING_MODE_INHERIT)
            {
                type = parent.GetWritingMode();
            }

            result.SetWritingMode(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region overflow_y
        static CssStatus PropDispCascade_overflow_y(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_overflow_y(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_overflow_y(CssSelectState state)
        {
            state.Computed.SetOverflowY(CssOverflowEnum.CSS_OVERFLOW_VISIBLE);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_overflow_y(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetOverflowY();

            if (type == CssOverflowEnum.CSS_OVERFLOW_INHERIT)
            {
                type = parent.GetOverflowY();
            }

            result.SetOverflowY(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region box_sizing
        static CssStatus PropDispCascade_box_sizing(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_box_sizing(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_box_sizing(CssSelectState state)
        {
            state.Computed.SetBoxSizing(CssBoxSizingEnum.CSS_BOX_SIZING_CONTENT_BOX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_box_sizing(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetBoxSizing();

            if (type == CssBoxSizingEnum.CSS_BOX_SIZING_INHERIT)
            {
                type = parent.GetBoxSizing();
            }

            result.SetBoxSizing(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region align_content
        static CssStatus PropDispCascade_align_content(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_align_content(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_align_content(CssSelectState state)
        {
            state.Computed.SetAlignContent(CssAlignContentEnum.CSS_ALIGN_CONTENT_STRETCH);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_align_content(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetAlignContent();

            if (type == CssAlignContentEnum.CSS_ALIGN_CONTENT_INHERIT)
            {
                type = parent.GetAlignContent();
            }

            result.SetAlignContent(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region align_items
        static CssStatus PropDispCascade_align_items(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_align_items(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_align_items(CssSelectState state)
        {
            state.Computed.SetAlignItems(CssAlignItemsEnum.CSS_ALIGN_ITEMS_STRETCH);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_align_items(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetAlignItems();

            if (type == CssAlignItemsEnum.CSS_ALIGN_ITEMS_INHERIT)
            {
                type = parent.GetAlignItems();
            }

            result.SetAlignItems(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region align_self
        static CssStatus PropDispCascade_align_self(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_align_self(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_align_self(CssSelectState state)
        {
            state.Computed.SetAlignSelf(CssAlignSelfEnum.CSS_ALIGN_SELF_AUTO);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_align_self(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetAlignSelf();

            if (type == CssAlignSelfEnum.CSS_ALIGN_SELF_INHERIT)
            {
                type = parent.GetAlignSelf();
            }

            result.SetAlignSelf(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region flex_basis
        static CssStatus PropDispCascade_flex_basis(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_flex_basis(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_flex_basis(CssSelectState state)
        {
            state.Computed.SetFlexBasis(CssFlexBasisEnum.CSS_FLEX_BASIS_AUTO,
                Fixed.F_0, CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_flex_basis(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed length = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;
            var type = child.GetFlexBasis(ref length, ref unit);

            if (type == CssFlexBasisEnum.CSS_FLEX_BASIS_INHERIT)
            {
                type = parent.GetFlexBasis(ref length, ref unit);
            }

            result.SetFlexBasis(type, length, unit);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region flex_direction
        static CssStatus PropDispCascade_flex_direction(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_flex_direction(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_flex_direction(CssSelectState state)
        {
            state.Computed.SetFlexDirection(CssFlexDirectionEnum.CSS_FLEX_DIRECTION_ROW);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_flex_direction(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetFlexDirection();

            if (type == CssFlexDirectionEnum.CSS_FLEX_DIRECTION_INHERIT)
            {
                type = parent.GetFlexDirection();
            }

            result.SetFlexDirection(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region flex_grow
        static CssStatus PropDispCascade_flex_grow(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_flex_grow(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_flex_grow(CssSelectState state)
        {
            state.Computed.SetFlexGrow(CssFlexGrowEnum.CSS_FLEX_GROW_SET, Fixed.F_0);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_flex_grow(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed flex_grow = Fixed.F_0;
            var type = child.GetFlexGrow(ref flex_grow);

            if (type == CssFlexGrowEnum.CSS_FLEX_GROW_INHERIT)
            {
                type = parent.GetFlexGrow(ref flex_grow);
            }

            result.SetFlexGrow(type, flex_grow);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region flex_shrink
        static CssStatus PropDispCascade_flex_shrink(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_flex_shrink(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_flex_shrink(CssSelectState state)
        {
            state.Computed.SetFlexShrink(CssFlexShrinkEnum.CSS_FLEX_SHRINK_SET, new Fixed(1));
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_flex_shrink(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            Fixed flex_shrink = Fixed.F_0;
            var type = child.GetFlexShrink(ref flex_shrink);

            if (type == CssFlexShrinkEnum.CSS_FLEX_SHRINK_INHERIT)
            {
                type = parent.GetFlexShrink(ref flex_shrink);
            }

            result.SetFlexShrink(type, flex_shrink);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region flex_wrap
        static CssStatus PropDispCascade_flex_wrap(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_flex_wrap(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_flex_wrap(CssSelectState state)
        {
            state.Computed.SetFlexWrap(CssFlexWrapEnum.CSS_FLEX_WRAP_NOWRAP);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_flex_wrap(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetFlexWrap();

            if (type == CssFlexWrapEnum.CSS_FLEX_WRAP_INHERIT)
            {
                type = parent.GetFlexWrap();
            }

            result.SetFlexWrap(type);

            return CssStatus.CSS_OK;
        }
        #endregion

        #region justify_content
        static CssStatus PropDispCascade_justify_content(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_justify_content(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_justify_content(CssSelectState state)
        {
            state.Computed.SetJustifyContent(CssJustifyContentEnum.CSS_JUSTIFY_CONTENT_FLEX_START);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_justify_content(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            var type = child.GetJustifyContent();

            if (type == CssJustifyContentEnum.CSS_JUSTIFY_CONTENT_INHERIT)
            {
                type = parent.GetJustifyContent();
            }

            result.SetJustifyContent(type);
            return CssStatus.CSS_OK;
        }
        #endregion

        #region order
        static CssStatus PropDispCascade_order(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispSFH_order(CssHint hint, ComputedStyle style)
        {
            Log.Unimplemented();
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispInitial_order(CssSelectState state)
        {
            state.Computed.SetOrder(CssOrderEnum.CSS_ORDER_SET, 0);
            return CssStatus.CSS_OK;
        }

        static CssStatus PropDispCompose_order(ComputedStyle parent, ComputedStyle child, ComputedStyle result)
        {
            int count = 0;
            var type = child.GetOrder(ref count);

            if (type == CssOrderEnum.CSS_ORDER_INHERIT)
            {
                type = parent.GetOrder(ref count);
            }

            result.SetOrder(type, count);

            return CssStatus.CSS_OK;
        }
        #endregion



    }
}
