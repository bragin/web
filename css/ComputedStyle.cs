using SkiaSharpOpenGLBenchmark.css;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark
{
    public struct Color : IEquatable<Color>
    {
        uint rawValue;

        public uint Value   // property
        {
            get { return rawValue; }   // get method
            set { rawValue = value; }  // set method
        }

        public static bool operator ==(Color a, Color b)
        {
            return a.rawValue == b.rawValue;
        }

        public static bool operator !=(Color a, Color b)
        {
            return a.rawValue != b.rawValue;
        }

        public bool Equals(Color other)
        {
            return rawValue == other.rawValue;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Color?;

            if (other != null) return Equals(other.Value);

            return false;
        }

        public override string ToString()
        {
            //return value.ToString("X");
            return String.Format("{0:X8}", rawValue);
        }

        public Color(uint nv)
        {
            rawValue = nv;
        }
    }

    // computed.h:27
    public struct CssComputedCounter
    {
        string Name;
        Fixed Value;
    }

    // computed.h:32
    public struct CssComputedClipRect
    {
        public Fixed Top;
        public Fixed Right;
        public Fixed Bottom;
        public Fixed Left;

        public CssUnit Tunit;
        public CssUnit Runit;
        public CssUnit Bunit;
        public CssUnit Lunit;

        public bool TopAuto;
        public bool RightAuto;
        public bool BottomAuto;
        public bool LeftAuto;
    }

    // computed.h:49
    public enum CssComputedContentType : byte
    {
        CSS_COMPUTED_CONTENT_NONE = 0,
        CSS_COMPUTED_CONTENT_STRING = 1,
        CSS_COMPUTED_CONTENT_URI = 2,
        CSS_COMPUTED_CONTENT_COUNTER = 3,
        CSS_COMPUTED_CONTENT_COUNTERS = 4,
        CSS_COMPUTED_CONTENT_ATTR = 5,
        CSS_COMPUTED_CONTENT_OPEN_QUOTE = 6,
        CSS_COMPUTED_CONTENT_CLOSE_QUOTE = 7,
        CSS_COMPUTED_CONTENT_NO_OPEN_QUOTE = 8,
        CSS_COMPUTED_CONTENT_NO_CLOSE_QUOTE = 9
    };


    public enum CssPseudoElement : int
    {
        CSS_PSEUDO_ELEMENT_NONE = 0,
        CSS_PSEUDO_ELEMENT_FIRST_LINE = 1,
        CSS_PSEUDO_ELEMENT_FIRST_LETTER = 2,
        CSS_PSEUDO_ELEMENT_BEFORE = 3,
        CSS_PSEUDO_ELEMENT_AFTER = 4,

        CSS_PSEUDO_ELEMENT_COUNT = 5    // Number of pseudo elements
    }

    public class ComputedStyleI
    {
        /*
         * Property                       Size (bits)     Size (bytes)
         * ---                            ---             ---
         * align_content                    3             
         * align_items                      3             
         * align_self                       3             
         * background_attachment            2             
         * background_color                 2               4
         * background_image                 1             sizeof(ptr)
         * background_position              1 + 10          8
         * background_repeat                3             
         * border_bottom_color              2               4
         * border_bottom_style              4             
         * border_bottom_width              3 + 5           4
         * border_collapse                  2             
         * border_left_color                2               4
         * border_left_style                4             
         * border_left_width                3 + 5           4
         * border_right_color               2               4
         * border_right_style               4             
         * border_right_width               3 + 5           4
         * border_spacing                   1 + 10          8
         * border_top_color                 2               4
         * border_top_style                 4             
         * border_top_width                 3 + 5           4
         * bottom                           2 + 5           4
         * box_sizing                       2             
         * break_after                      4             
         * break_before                     4             
         * break_inside                     4             
         * caption_side                     2             
         * clear                            3             
         * clip                             6 + 20         16
         * color                            1               4
         * column_count                     2               4
         * column_fill                      2             
         * column_gap                       2 + 5           4
         * column_rule_color                2               4
         * column_rule_style                4             
         * column_rule_width                3 + 5           4
         * column_span                      2             
         * column_width                     2 + 5           4
         * direction                        2             
         * display                          5             
         * empty_cells                      2             
         * flex_basis                       2 + 5           4
         * flex_direction                   3             
         * flex_grow                        1               4
         * flex_shrink                      1               4
         * flex_wrap                        2             
         * float                            2             
         * font_size                        4 + 5           4
         * font_style                       2             
         * font_variant                     2             
         * font_weight                      4             
         * height                           2 + 5           4
         * justify_content                  3             
         * left                             2 + 5           4
         * letter_spacing                   2 + 5           4
         * line_height                      2 + 5           4
         * list_style_image                 1             sizeof(ptr)
         * list_style_position              2             
         * list_style_type                  6             
         * margin_bottom                    2 + 5           4
         * margin_left                      2 + 5           4
         * margin_right                     2 + 5           4
         * margin_top                       2 + 5           4
         * max_height                       2 + 5           4
         * max_width                        2 + 5           4
         * min_height                       2 + 5           4
         * min_width                        2 + 5           4
         * opacity                          1               4
         * order                            1               4
         * orphans                          1               4
         * outline_color                    2               4
         * outline_style                    4             
         * outline_width                    3 + 5           4
         * overflow_x                       3             
         * overflow_y                       3             
         * padding_bottom                   1 + 5           4
         * padding_left                     1 + 5           4
         * padding_right                    1 + 5           4
         * padding_top                      1 + 5           4
         * page_break_after                 3             
         * page_break_before                3             
         * page_break_inside                2             
         * position                         3             
         * right                            2 + 5           4
         * table_layout                     2             
         * text_align                       4             
         * text_decoration                  5             
         * text_indent                      1 + 5           4
         * text_transform                   3             
         * top                              2 + 5           4
         * unicode_bidi                     2             
         * vertical_align                   4 + 5           4
         * visibility                       2             
         * white_space                      3             
         * widows                           1               4
         * width                            2 + 5           4
         * word_spacing                     2 + 5           4
         * writing_mode                     2             
         * z_index                          2               4
         * 
         * Encode content as an array of content items, terminated with a blank entry.
         * 
         * content                          2             sizeof(ptr)
         * 
         * Encode counter_increment as an array of name, value pairs, terminated with a
         * blank entry.
         * 
         * counter_increment                1             sizeof(ptr)
         * 
         * Encode counter_reset as an array of name, value pairs, terminated with a
         * blank entry.
         * 
         * counter_reset                    1             sizeof(ptr)
         * 
         * Encode cursor uri(s) as an array of string objects, terminated with a blank
         * entry
         * 
         * cursor                           5             sizeof(ptr)
         * 
         * Encode font family as an array of string objects, terminated with a blank
         * entry.
         * 
         * font_family                      3             sizeof(ptr)
         * 
         * Encode quotes as an array of string objects, terminated with a blank entry.
         * 
         * quotes                           1             sizeof(ptr)
         * 
         * ---                            ---             ---
         *                                462 bits        228 + 8sizeof(ptr) bytes
         *                                ===================
         *                                286 + 8sizeof(ptr) bytes
         * 
         * Bit allocations:
         * 
         * 0  bbbbbbbboooooooorrrrrrrrdddddddd
         * border_left_width; border_top_width; border_bottom_width; border_right_width
         * 
         * 1  vvvvvvvvvooooooooccccccccmmmmmmm
         * vertical_align; outline_width; column_rule_width; margin_top
         * 
         * 2  ccccccccccccccccccccccccccpppppp
         * clip; padding_left
         * 
         * 3  mmmmmmmrrrrrrrwwwwwwwttttttddddd
         * max_height; right; width; text_indent; display
         * 
         * 4  fffffffmmmmmmmcccccccllllllltttt
         * flex_basis; min_height; column_gap; left; text_align
         * 
         * 5  cccccccmmmmmmmlllllllwwwwwwwbbbb
         * column_width; margin_bottom; line_height; word_spacing; break_inside
         * 
         * 6  hhhhhhhlllllllmmmmmmmaaaaaaabbbb
         * height; letter_spacing; min_width; margin_right; border_bottom_style
         * 
         * 7  tttttttmmmmmmmbbbbbbbaaaaaaaoooo
         * top; margin_left; bottom; max_width; border_top_style
         * 
         * 8  llllllppppppaaaaaaddddddtttttggg
         * list_style_type; padding_top; padding_right; padding_bottom;
         * text_decoration; page_break_after
         * 
         * 9  cccccbbbbooooffffrrrruuuullllnnn
         * cursor; break_before; border_left_style; font_weight; break_after;
         * outline_style; column_rule_style; font_family
         * 
         * 10 aaallliiipppbbccttoouuzzffeerrmm
         * align_content; align_items; align_self; position; border_bottom_color;
         * column_rule_color; table_layout; box_sizing; column_span; z_index;
         * flex_wrap; empty_cells; border_left_color; column_count
         * 
         * 11 ffoobbppaannccrrddeeuulliittUUvv
         * float; font_variant; background_attachment; page_break_inside;
         * background_color; font_style; content; border_top_color; border_collapse;
         * border_right_color; outline_color; column_fill; list_style_position;
         * caption_side; unicode_bidi; visibility
         * 
         * 12 bbbbbbbbbbbaaaaaaaaaaafffffffffl
         * border_spacing; background_position; font_size; flex_grow
         * 
         * 13 bbbboooaaawwwvvvtttcccpppjjjfffr
         * border_right_style; overflow_y; background_repeat; white_space; overflow_x;
         * text_transform; clear; page_break_before; justify_content; flex_direction;
         * order
         * 
         * 14 wwddlicobfqupr..................
         * writing_mode; direction; list_style_image; widows; counter_reset; orphans;
         * background_image; flex_shrink; quotes; counter_increment; opacity; color
         */
        public uint[] bits = new uint[15];

        public Color background_color;
        public string background_image;
        public Fixed background_position_a;
        public Fixed background_position_b;
        public Color border_bottom_color;
        public Fixed border_bottom_width;
        public Color border_left_color;
        public Fixed border_left_width;
        public Color border_right_color;
        public Fixed border_right_width;
        public Fixed border_spacing_a;
        public Fixed border_spacing_b;
        public Color border_top_color;
        public Fixed border_top_width;
        public Fixed bottom;
        public Fixed clip_a;
        public Fixed clip_b;
        public Fixed clip_c;
        public Fixed clip_d;
        public Color color;
        public int column_count;
        public Fixed column_gap;
        public Color column_rule_color;
        public Fixed column_rule_width;
        public Fixed column_width;
        public Fixed flex_basis;
        public Fixed flex_grow;
        public Fixed flex_shrink;
        public Fixed font_size;
        public Fixed height;
        public Fixed left;
        public Fixed letter_spacing;
        public Fixed line_height;
        public string list_style_image;
        public Fixed margin_bottom;
        public Fixed margin_left;
        public Fixed margin_right;
        public Fixed margin_top;
        public Fixed max_height;
        public Fixed max_width;
        public Fixed min_height;
        public Fixed min_width;
        public Fixed opacity;
        public int order;
        public int orphans;
        public Color outline_color;
        public Fixed outline_width;
        public Fixed padding_bottom;
        public Fixed padding_left;
        public Fixed padding_right;
        public Fixed padding_top;
        public Fixed right;
        public Fixed text_indent;
        public Fixed top;
        public Fixed vertical_align;
        public int widows;
        public Fixed width;
        public Fixed word_spacing;
        public int z_index;

        public ComputedStyleI()
        {
            /*
            background_color = new Color();
            background_image = "";
            background_position_a = new Fixed();
            background_position_b = new Fixed();
            border_bottom_color = new Color();
            border_bottom_width = new Fixed();
            border_left_color = new Color();
            border_left_width = new Fixed();
            border_right_color = new Color();
            border_right_width = new Fixed();
            border_spacing_a = new Fixed();
            border_spacing_b = new Fixed();
            border_top_color = new Color();
            border_top_width = new Fixed();
            bottom = new Fixed();
            clip_a = new Fixed();
            clip_b = new Fixed();
            clip_c = new Fixed();
            clip_d = new Fixed();
            color = new Color();
            column_count = 0;
            column_gap = new Fixed();
            column_rule_color = new Color();
            column_rule_width = new Fixed();
            column_width = new Fixed();
            flex_basis = new Fixed();
            flex_grow = new Fixed();
            flex_shrink = new Fixed();
            font_size = new Fixed();
            height = new Fixed();
            left = new Fixed();
            letter_spacing = new Fixed();
            line_height = new Fixed();
            list_style_image = "";
            margin_bottom = new Fixed();
            margin_left = new Fixed();
            margin_right = new Fixed();
            margin_top = new Fixed();
            max_height = new Fixed();
            max_width = new Fixed();
            min_height = new Fixed();
            min_width = new Fixed();
            opacity = new Fixed();
            order = 0;
            orphans = 0;

            outline_color = new Color();

            outline_width = new Fixed();
            padding_bottom = new Fixed();
            padding_left = new Fixed();
            padding_right = new Fixed();
            padding_top = new Fixed();
            right = new Fixed();
            text_indent = new Fixed();
            top = new Fixed();
            vertical_align = new Fixed();
            widows = 0;
            width = new Fixed();
            word_spacing = new Fixed();
            z_index = 0;*/
        }
    };

    // computed.h:62
    public struct CssComputedContentItem
    {
        public CssComputedContentType Type;

        public string Name; // aka attr, string, uri
        public string Sep;
        public byte Style;
    }

    public class ComputedStyle
    {
        public ComputedStyleI i;

        CssComputedContentItem[] Content;
        CssComputedCounter[] CounterIncrement;
        CssComputedCounter[] CounterReset;
        string[] Cursor;
        string[] FontFamily;
        string[] Quotes;

        int count;
        uint bin;

        public ComputedStyle()
        {
            bin = UInt32.MaxValue;
            i = new ComputedStyleI();
            Content = new CssComputedContentItem[1];
            CounterIncrement = new CssComputedCounter[1];
            CounterReset = new CssComputedCounter[1];
            Cursor = new string[1];
            FontFamily = new string[1];
            Quotes = new string[1];
        }

        // css_computed_style_compose
        // computed.c:253
        public ComputedStyle(ComputedStyle parent, ComputedStyle child, CssUnitCtx unitCtx)
        {
            CssStatus error;
            //ComputedStyle composed = new ComputedStyle();

            /* TODO:
             *   Make this function take a composition context, to allow us
             *   to avoid the churn of unnecesaraly allocating and freeing
             *   the memory to compose styles into.
             */
            bin = UInt32.MaxValue;
            i = new ComputedStyleI();

            // Iterate through the properties
            for (int i = 0; i < (int)CssPropertiesEnum.CSS_N_PROPERTIES; i++)
            {
                // Compose the property
                error = CssProps.Dispatch[i].Compose(parent, child, this);
                if (error != CssStatus.CSS_OK)
                    break;
            }

            // Finally, compute absolute values for everything
            ComputeAbsoluteValues(parent, unitCtx);

            //*result = composed;
            //return css__arena_intern_style(result);
        }

        // Property access indices and masks
        // 0  bbbbbbbboooooooorrrrrrrrdddddddd
        // border_left_width; border_top_width; border_bottom_width; border_right_width
        const uint BORDER_RIGHT_WIDTH_INDEX = 0;
        const byte BORDER_RIGHT_WIDTH_SHIFT = 0;
        const uint BORDER_RIGHT_WIDTH_MASK = 0xff;

        const uint BORDER_BOTTOM_WIDTH_INDEX = 0;
        const byte BORDER_BOTTOM_WIDTH_SHIFT = 8;
        const uint BORDER_BOTTOM_WIDTH_MASK = 0xff00;

        const uint BORDER_TOP_WIDTH_INDEX = 0;
        const byte BORDER_TOP_WIDTH_SHIFT = 16;
        const uint BORDER_TOP_WIDTH_MASK = 0xff0000;

        const uint BORDER_LEFT_WIDTH_INDEX = 0;
        const byte BORDER_LEFT_WIDTH_SHIFT = 24;
        const uint BORDER_LEFT_WIDTH_MASK = 0xff000000;

        // 1  vvvvvvvvvooooooooccccccccmmmmmmm
        // vertical_align; outline_width; column_rule_width; margin_top
        const uint MARGIN_TOP_INDEX = 1;
        const byte MARGIN_TOP_SHIFT = 0;
        const uint MARGIN_TOP_MASK = 0x7f;

        const uint COLUMN_RULE_WIDTH_INDEX = 1;
        const byte COLUMN_RULE_WIDTH_SHIFT = 7;
        const uint COLUMN_RULE_WIDTH_MASK = 0x7f80;

        const uint OUTLINE_WIDTH_INDEX = 1;
        const byte OUTLINE_WIDTH_SHIFT = 15;
        const uint OUTLINE_WIDTH_MASK = 0x7f8000;

        const uint VERTICAL_ALIGN_INDEX = 1;
        const byte VERTICAL_ALIGN_SHIFT = 23;
        const uint VERTICAL_ALIGN_MASK = 0xff800000;

        // 2  ccccccccccccccccccccccccccpppppp
        // clip; padding_left
        const uint PADDING_LEFT_INDEX = 2;
        const byte PADDING_LEFT_SHIFT = 0;
        const uint PADDING_LEFT_MASK = 0x3f;

        const uint CLIP_INDEX = 2;
        const byte CLIP_SHIFT = 6;
        const uint CLIP_MASK = 0xffffffc0;

        // 3 mmmmmmmrrrrrrrwwwwwwwttttttddddd
        // max_height; right; width; text_indent; display
        const uint DISPLAY_INDEX = 3;
        const byte DISPLAY_SHIFT = 0;
        const uint DISPLAY_MASK = 0x1f;

        const uint TEXT_INDENT_INDEX = 3;
        const byte TEXT_INDENT_SHIFT = 5;
        const uint TEXT_INDENT_MASK = 0x7e0;

        const uint WIDTH_INDEX = 3;
        const byte WIDTH_SHIFT = 11;
        const uint WIDTH_MASK = 0x3f800;

        const uint RIGHT_INDEX = 3;
        const byte RIGHT_SHIFT = 18;
        const uint RIGHT_MASK = 0x1fc0000;

        const uint MAX_HEIGHT_INDEX = 3;
        const byte MAX_HEIGHT_SHIFT = 25;
        const uint MAX_HEIGHT_MASK = 0xfe000000;

        // 4 fffffffmmmmmmmcccccccllllllltttt
        // flex_basis; min_height; column_gap; left; text_align
        const uint TEXT_ALIGN_INDEX = 4;
        const byte TEXT_ALIGN_SHIFT = 0;
        const uint TEXT_ALIGN_MASK = 0xf;

        const uint LEFT_INDEX = 4;
        const byte LEFT_SHIFT = 4;
        const uint LEFT_MASK = 0x7f0;

        const uint COLUMN_GAP_INDEX = 4;
        const byte COLUMN_GAP_SHIFT = 11;
        const uint COLUMN_GAP_MASK = 0x3f800;

        const uint MIN_HEIGHT_INDEX = 4;
        const byte MIN_HEIGHT_SHIFT = 18;
        const uint MIN_HEIGHT_MASK = 0x1fc0000;

        const uint FLEX_BASIS_INDEX = 4;
        const byte FLEX_BASIS_SHIFT = 25;
        const uint FLEX_BASIS_MASK = 0xfe000000;

        // 5 cccccccmmmmmmmlllllllwwwwwwwbbbb
        // column_width; margin_bottom; line_height; word_spacing; break_inside
        const uint BREAK_INSIDE_INDEX = 5;
        const byte BREAK_INSIDE_SHIFT = 0;
        const uint BREAK_INSIDE_MASK = 0xf;

        const uint WORD_SPACING_INDEX = 5;
        const byte WORD_SPACING_SHIFT = 4;
        const uint WORD_SPACING_MASK = 0x7f0;

        const uint LINE_HEIGHT_INDEX = 5;
        const byte LINE_HEIGHT_SHIFT = 11;
        const uint LINE_HEIGHT_MASK = 0x3f800;

        const uint MARGIN_BOTTOM_INDEX = 5;
        const byte MARGIN_BOTTOM_SHIFT = 18;
        const uint MARGIN_BOTTOM_MASK = 0x1fc0000;

        const uint COLUMN_WIDTH_INDEX = 5;
        const byte COLUMN_WIDTH_SHIFT = 25;
        const uint COLUMN_WIDTH_MASK = 0xfe000000;

        // 6 hhhhhhhlllllllmmmmmmmaaaaaaabbbb
        // height; letter_spacing; min_width; margin_right; border_bottom_style
        const uint BORDER_BOTTOM_STYLE_INDEX = 6;
        const byte BORDER_BOTTOM_STYLE_SHIFT = 0;
        const uint BORDER_BOTTOM_STYLE_MASK = 0xf;

        const uint MARGIN_RIGHT_INDEX = 6;
        const byte MARGIN_RIGHT_SHIFT = 4;
        const uint MARGIN_RIGHT_MASK = 0x7f0;

        const uint MIN_WIDTH_INDEX = 6;
        const byte MIN_WIDTH_SHIFT = 11;
        const uint MIN_WIDTH_MASK = 0x3f800;

        const uint LETTER_SPACING_INDEX = 6;
        const byte LETTER_SPACING_SHIFT = 18;
        const uint LETTER_SPACING_MASK = 0x1fc0000;

        const uint HEIGHT_INDEX = 6;
        const byte HEIGHT_SHIFT = 25;
        const uint HEIGHT_MASK = 0xfe000000;

        // 7  tttttttmmmmmmmbbbbbbbaaaaaaaoooo
        // top; margin_left; bottom; max_width; border_top_style
        const uint BORDER_TOP_STYLE_INDEX = 7;
        const byte BORDER_TOP_STYLE_SHIFT = 0;
        const uint BORDER_TOP_STYLE_MASK = 0xf;

        const uint MAX_WIDTH_INDEX = 7;
        const byte MAX_WIDTH_SHIFT = 4;
        const uint MAX_WIDTH_MASK = 0x7f0;

        const uint BOTTOM_INDEX = 7;
        const byte BOTTOM_SHIFT = 11;
        const uint BOTTOM_MASK = 0x3f800;

        const uint MARGIN_LEFT_INDEX = 7;
        const byte MARGIN_LEFT_SHIFT = 18;
        const uint MARGIN_LEFT_MASK = 0x1fc0000;

        const uint TOP_INDEX = 7;
        const byte TOP_SHIFT = 25;
        const uint TOP_MASK = 0xfe000000;

        // 8  llllllppppppaaaaaaddddddtttttggg
        // list_style_type; padding_top; padding_right; padding_bottom;
        // text_decoration; page_break_after
        const uint PAGE_BREAK_AFTER_INDEX = 8;
        const byte PAGE_BREAK_AFTER_SHIFT = 0;
        const uint PAGE_BREAK_AFTER_MASK = 0x7;

        const uint TEXT_DECORATION_INDEX = 8;
        const byte TEXT_DECORATION_SHIFT = 3;
        const uint TEXT_DECORATION_MASK = 0xf8;

        const uint PADDING_BOTTOM_INDEX = 8;
        const byte PADDING_BOTTOM_SHIFT = 8;
        const uint PADDING_BOTTOM_MASK = 0x3f00;

        const uint PADDING_RIGHT_INDEX = 8;
        const byte PADDING_RIGHT_SHIFT = 14;
        const uint PADDING_RIGHT_MASK = 0xfc000;

        const uint PADDING_TOP_INDEX = 8;
        const byte PADDING_TOP_SHIFT = 20;
        const uint PADDING_TOP_MASK = 0x3f00000;

        const uint LIST_STYLE_TYPE_INDEX = 8;
        const byte LIST_STYLE_TYPE_SHIFT = 26;
        const uint LIST_STYLE_TYPE_MASK = 0xfc000000;

        // 9  cccccbbbbooooffffrrrruuuullllnnn
        // cursor; break_before; border_left_style; font_weight; break_after;
        // outline_style; column_rule_style; font_family
        const uint FONT_FAMILY_INDEX = 9;
        const byte FONT_FAMILY_SHIFT = 0;
        const uint FONT_FAMILY_MASK = 0x7;

        const uint COLUMN_RULE_STYLE_INDEX = 9;
        const byte COLUMN_RULE_STYLE_SHIFT = 3;
        const uint COLUMN_RULE_STYLE_MASK = 0x78;

        const uint OUTLINE_STYLE_INDEX = 9;
        const byte OUTLINE_STYLE_SHIFT = 7;
        const uint OUTLINE_STYLE_MASK = 0x780;

        const uint BREAK_AFTER_INDEX = 9;
        const byte BREAK_AFTER_SHIFT = 11;
        const uint BREAK_AFTER_MASK = 0x7800;

        const uint FONT_WEIGHT_INDEX = 9;
        const byte FONT_WEIGHT_SHIFT = 15;
        const uint FONT_WEIGHT_MASK = 0x78000;

        const uint BORDER_LEFT_STYLE_INDEX = 9;
        const byte BORDER_LEFT_STYLE_SHIFT = 19;
        const uint BORDER_LEFT_STYLE_MASK = 0x780000;

        const uint BREAK_BEFORE_INDEX = 9;
        const byte BREAK_BEFORE_SHIFT = 23;
        const uint BREAK_BEFORE_MASK = 0x7800000;

        const uint CURSOR_INDEX = 9;
        const byte CURSOR_SHIFT = 27;
        const uint CURSOR_MASK = 0xf8000000;

        // 10 aaallliiipppbbccttoouuzzffeerrmm
        // align_content; align_items; align_self; position; border_bottom_color;
        // column_rule_color; table_layout; box_sizing; column_span; z_index;
        // flex_wrap; empty_cells; border_left_color; column_count
        const uint COLUMN_COUNT_INDEX = 10;
        const byte COLUMN_COUNT_SHIFT = 0;
        const uint COLUMN_COUNT_MASK = 0x3;

        const uint BORDER_LEFT_COLOR_INDEX = 10;
        const byte BORDER_LEFT_COLOR_SHIFT = 2;
        const uint BORDER_LEFT_COLOR_MASK = 0xc;

        const uint EMPTY_CELLS_INDEX = 10;
        const byte EMPTY_CELLS_SHIFT = 4;
        const uint EMPTY_CELLS_MASK = 0x30;

        const uint FLEX_WRAP_INDEX = 10;
        const byte FLEX_WRAP_SHIFT = 6;
        const uint FLEX_WRAP_MASK = 0xc0;

        const uint Z_INDEX_INDEX = 10;
        const byte Z_INDEX_SHIFT = 8;
        const uint Z_INDEX_MASK = 0x300;

        const uint COLUMN_SPAN_INDEX = 10;
        const byte COLUMN_SPAN_SHIFT = 10;
        const uint COLUMN_SPAN_MASK = 0xc00;

        const uint BOX_SIZING_INDEX = 10;
        const byte BOX_SIZING_SHIFT = 12;
        const uint BOX_SIZING_MASK = 0x3000;

        const uint TABLE_LAYOUT_INDEX = 10;
        const byte TABLE_LAYOUT_SHIFT = 14;
        const uint TABLE_LAYOUT_MASK = 0xc000;

        const uint COLUMN_RULE_COLOR_INDEX = 10;
        const byte COLUMN_RULE_COLOR_SHIFT = 16;
        const uint COLUMN_RULE_COLOR_MASK = 0x30000;

        const uint BORDER_BOTTOM_COLOR_INDEX = 10;
        const byte BORDER_BOTTOM_COLOR_SHIFT = 18;
        const uint BORDER_BOTTOM_COLOR_MASK = 0xc0000;

        const uint POSITION_INDEX = 10;
        const byte POSITION_SHIFT = 20;
        const uint POSITION_MASK = 0x700000;

        const uint ALIGN_SELF_INDEX = 10;
        const byte ALIGN_SELF_SHIFT = 23;
        const uint ALIGN_SELF_MASK = 0x3800000;

        const uint ALIGN_ITEMS_INDEX = 10;
        const byte ALIGN_ITEMS_SHIFT = 26;
        const uint ALIGN_ITEMS_MASK = 0x1c000000;

        const uint ALIGN_CONTENT_INDEX = 10;
        const byte ALIGN_CONTENT_SHIFT = 29;
        const uint ALIGN_CONTENT_MASK = 0xe0000000;

        // 11 ffoobbppaannccrrddeeuulliittUUvv
        // float; font_variant; background_attachment; page_break_inside;
        // background_color; font_style; content; border_top_color; border_collapse;
        // border_right_color; outline_color; column_fill; list_style_position;
        // caption_side; unicode_bidi; visibility
        const uint VISIBILITY_INDEX = 11;
        const byte VISIBILITY_SHIFT = 0;
        const uint VISIBILITY_MASK = 0x3;

        const uint UNICODE_BIDI_INDEX = 11;
        const byte UNICODE_BIDI_SHIFT = 2;
        const uint UNICODE_BIDI_MASK = 0xc;

        const uint CAPTION_SIDE_INDEX = 11;
        const byte CAPTION_SIDE_SHIFT = 4;
        const uint CAPTION_SIDE_MASK = 0x30;

        const uint LIST_STYLE_POSITION_INDEX = 11;
        const byte LIST_STYLE_POSITION_SHIFT = 6;
        const uint LIST_STYLE_POSITION_MASK = 0xc0;

        const uint COLUMN_FILL_INDEX = 11;
        const byte COLUMN_FILL_SHIFT = 8;
        const uint COLUMN_FILL_MASK = 0x300;

        const uint OUTLINE_COLOR_INDEX = 11;
        const byte OUTLINE_COLOR_SHIFT = 10;
        const uint OUTLINE_COLOR_MASK = 0xc00;

        const uint BORDER_RIGHT_COLOR_INDEX = 11;
        const byte BORDER_RIGHT_COLOR_SHIFT = 12;
        const uint BORDER_RIGHT_COLOR_MASK = 0x3000;

        const uint BORDER_COLLAPSE_INDEX = 11;
        const byte BORDER_COLLAPSE_SHIFT = 14;
        const uint BORDER_COLLAPSE_MASK = 0xc000;

        const uint BORDER_TOP_COLOR_INDEX = 11;
        const byte BORDER_TOP_COLOR_SHIFT = 16;
        const uint BORDER_TOP_COLOR_MASK = 0x30000;

        const uint CONTENT_INDEX = 11;
        const byte CONTENT_SHIFT = 18;
        const uint CONTENT_MASK = 0xc0000;

        const uint FONT_STYLE_INDEX = 11;
        const byte FONT_STYLE_SHIFT = 20;
        const uint FONT_STYLE_MASK = 0x300000;

        const uint BACKGROUND_COLOR_INDEX = 11;
        const byte BACKGROUND_COLOR_SHIFT = 22;
        const uint BACKGROUND_COLOR_MASK = 0xc00000;

        const uint PAGE_BREAK_INSIDE_INDEX = 11;
        const byte PAGE_BREAK_INSIDE_SHIFT = 24;
        const uint PAGE_BREAK_INSIDE_MASK = 0x3000000;

        const uint BACKGROUND_ATTACHMENT_INDEX = 11;
        const byte BACKGROUND_ATTACHMENT_SHIFT = 26;
        const uint BACKGROUND_ATTACHMENT_MASK = 0x0c000000;

        const uint FONT_VARIANT_INDEX = 11;
        const byte FONT_VARIANT_SHIFT = 28;
        const uint FONT_VARIANT_MASK = 0x30000000;

        const uint FLOAT_INDEX = 11;
        const byte FLOAT_SHIFT = 30;
        const uint FLOAT_MASK = 0xc0000000;

        // 12 bbbbbbbbbbbaaaaaaaaaaafffffffffl
        // border_spacing; background_position; font_size; flex_grow
        const uint FLEX_GROW_INDEX = 12;
        const byte FLEX_GROW_SHIFT = 0;
        const uint FLEX_GROW_MASK = 0x1;

        const uint FONT_SIZE_INDEX = 12;
        const byte FONT_SIZE_SHIFT = 1;
        const uint FONT_SIZE_MASK = 0x3fe;

        const uint BACKGROUND_POSITION_INDEX = 12;
        const byte BACKGROUND_POSITION_SHIFT = 10;
        const uint BACKGROUND_POSITION_MASK = 0x1ffc00;

        const uint BORDER_SPACING_INDEX = 12;
        const byte BORDER_SPACING_SHIFT = 21;
        const uint BORDER_SPACING_MASK = 0xffe00000;

        // 13 bbbboooaaawwwvvvtttcccpppjjjfffr
        // border_right_style; overflow_y; background_repeat; white_space; overflow_x;
        // text_transform; clear; page_break_before; justify_content; flex_direction;
        // order
        const uint ORDER_INDEX = 13;
        const byte ORDER_SHIFT = 0;
        const uint ORDER_MASK = 0x1;

        const uint FLEX_DIRECTION_INDEX = 13;
        const byte FLEX_DIRECTION_SHIFT = 1;
        const uint FLEX_DIRECTION_MASK = 0xe;

        const uint JUSTIFY_CONTENT_INDEX = 13;
        const byte JUSTIFY_CONTENT_SHIFT = 4;
        const uint JUSTIFY_CONTENT_MASK = 0x70;

        const uint PAGE_BREAK_BEFORE_INDEX = 13;
        const byte PAGE_BREAK_BEFORE_SHIFT = 7;
        const uint PAGE_BREAK_BEFORE_MASK = 0x380;

        const uint CLEAR_INDEX = 13;
        const byte CLEAR_SHIFT = 10;
        const uint CLEAR_MASK = 0x1c00;

        const uint TEXT_TRANSFORM_INDEX = 13;
        const byte TEXT_TRANSFORM_SHIFT = 13;
        const uint TEXT_TRANSFORM_MASK = 0xe000;

        const uint OVERFLOW_X_INDEX = 13;
        const byte OVERFLOW_X_SHIFT = 16;
        const uint OVERFLOW_X_MASK = 0x70000;

        const uint WHITE_SPACE_INDEX = 13;
        const byte WHITE_SPACE_SHIFT = 19;
        const uint WHITE_SPACE_MASK = 0x380000;

        const uint BACKGROUND_REPEAT_INDEX = 13;
        const byte BACKGROUND_REPEAT_SHIFT = 22;
        const uint BACKGROUND_REPEAT_MASK = 0x1c00000;

        const uint OVERFLOW_Y_INDEX = 13;
        const byte OVERFLOW_Y_SHIFT = 25;
        const uint OVERFLOW_Y_MASK = 0xe000000;

        const uint BORDER_RIGHT_STYLE_INDEX = 13;
        const byte BORDER_RIGHT_STYLE_SHIFT = 28;
        const uint BORDER_RIGHT_STYLE_MASK = 0xf0000000;

        // 14 wwddlicobfqupr..................
        // writing_mode; direction; list_style_image; widows; counter_reset; orphans;
        // background_image; flex_shrink; quotes; counter_increment; opacity; color
        const uint COLOR_INDEX = 14;
        const byte COLOR_SHIFT = 18;
        const uint COLOR_MASK = 0x40000;

        const uint OPACITY_INDEX = 14;
        const byte OPACITY_SHIFT = 19;
        const uint OPACITY_MASK = 0x80000;

        const uint COUNTER_INCREMENT_INDEX = 14;
        const byte COUNTER_INCREMENT_SHIFT = 20;
        const uint COUNTER_INCREMENT_MASK = 0x100000;

        const uint QUOTES_INDEX = 14;
        const byte QUOTES_SHIFT = 21;
        const uint QUOTES_MASK = 0x200000;

        const uint FLEX_SHRINK_INDEX = 14;
        const byte FLEX_SHRINK_SHIFT = 22;
        const uint FLEX_SHRINK_MASK = 0x400000;

        const uint BACKGROUND_IMAGE_INDEX = 14;
        const byte BACKGROUND_IMAGE_SHIFT = 23;
        const uint BACKGROUND_IMAGE_MASK = 0x800000;

        const uint ORPHANS_INDEX = 14;
        const byte ORPHANS_SHIFT = 24;
        const uint ORPHANS_MASK = 0x1000000;

        const uint COUNTER_RESET_INDEX = 14;
        const byte COUNTER_RESET_SHIFT = 25;
        const uint COUNTER_RESET_MASK = 0x2000000;

        const uint WIDOWS_INDEX = 14;
        const byte WIDOWS_SHIFT = 26;
        const uint WIDOWS_MASK = 0x4000000;

        const uint LIST_STYLE_IMAGE_INDEX = 14;
        const byte LIST_STYLE_IMAGE_SHIFT = 27;
        const uint LIST_STYLE_IMAGE_MASK = 0x8000000;

        const uint DIRECTION_INDEX = 14;
        const byte DIRECTION_SHIFT = 28;
        const uint DIRECTION_MASK = 0x30000000;

        const uint WRITING_MODE_INDEX = 14;
        const byte WRITING_MODE_SHIFT = 30;
        const uint WRITING_MODE_MASK = 0xc0000000;



        private delegate byte GetForAbsLen(ComputedStyle style, out Fixed len1, out CssUnit unit1, out Fixed len2, out CssUnit unit2);

        private delegate void SetForAbsLen(ComputedStyle style, byte type, Fixed len1, CssUnit unit1, Fixed len2, CssUnit unit2);


        // autogenerated_propget.h:13
        public byte GetAlignContent()
        {
            uint bits = i.bits[ALIGN_CONTENT_INDEX];
            bits &= ALIGN_CONTENT_MASK;
            bits >>= ALIGN_CONTENT_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetAlignContent(byte type)
        {
            uint bits = i.bits[ALIGN_CONTENT_INDEX];

            /* 3bits: ttt : type */
            i.bits[ALIGN_CONTENT_INDEX] = (bits & ~ALIGN_CONTENT_MASK) | (((uint)type & 0x7) << ALIGN_CONTENT_SHIFT);
        }

        // autogenerated_propget.h:30
        public byte GetAlignItems()
        {
            uint bits = i.bits[ALIGN_ITEMS_INDEX];
            bits &= ALIGN_ITEMS_MASK;
            bits >>= ALIGN_ITEMS_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetAlignItems(byte type)
        {
            uint bits = i.bits[ALIGN_ITEMS_INDEX];

            /* 3bits: ttt : type */
            i.bits[ALIGN_ITEMS_INDEX] = (bits & ~ALIGN_ITEMS_MASK) | (((uint)type & 0x7) << ALIGN_ITEMS_SHIFT);
        }

        // autogenerated_propget.h:47
        public byte GetAlignSelf()
        {
            uint bits = i.bits[ALIGN_SELF_INDEX];
            bits &= ALIGN_SELF_MASK;
            bits >>= ALIGN_SELF_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetAlignSelf(byte type)
        {
            uint bits = i.bits[ALIGN_SELF_INDEX];

            /* 3bits: ttt : type */
            i.bits[ALIGN_SELF_INDEX] = (bits & ~ALIGN_SELF_MASK) | (((uint)type & 0x7) << ALIGN_SELF_SHIFT);
        }

        // autogenerated_propget.h:64
        public CssBackgroundAttachment GetBackgroundAttachment()
        {
            uint bits = i.bits[BACKGROUND_ATTACHMENT_INDEX];
            bits &= BACKGROUND_ATTACHMENT_MASK;
            bits >>= BACKGROUND_ATTACHMENT_SHIFT;

            /* 2bits: tt : type */

            return (CssBackgroundAttachment)(bits & 0x3);
        }
        public void SetBackgroundAttachment(CssBackgroundAttachment type)
        {
            uint bits = i.bits[BACKGROUND_ATTACHMENT_INDEX];

            uint val = ((uint)type & 0x3) << BACKGROUND_ATTACHMENT_SHIFT;

            // 2bits: tt : type
            i.bits[BACKGROUND_ATTACHMENT_INDEX] = (bits & ~BACKGROUND_ATTACHMENT_MASK) | val;
        }


        // autogenerated_propget.h:80
        public CssBackgroundColorEnum GetBackgroundColor(out Color color)
        {
            uint bits = i.bits[BACKGROUND_COLOR_INDEX];
            bits &= BACKGROUND_COLOR_MASK;
            bits >>= BACKGROUND_COLOR_SHIFT;

            /* 2bits: tt : type */
            color = i.background_color;

            return (CssBackgroundColorEnum)((byte)(bits & 0x3));
        }

        // autogenerated_propset.h:97
        public void SetBackgroundColor(CssBackgroundColorEnum type, Color color)
        {
            uint bits = i.bits[BACKGROUND_COLOR_INDEX];
            /* 2bits: tt : type */

            i.bits[BACKGROUND_COLOR_INDEX] = (bits & ~BACKGROUND_COLOR_MASK) | (((uint)type & 0x3) << BACKGROUND_COLOR_SHIFT);

            i.background_color = color;
        }

        // autogenerated_propget.h:120
        public CssBackgroundImageEnum GetBackgroundImage(out string image)
        {
            uint bits = i.bits[BACKGROUND_IMAGE_INDEX];
            bits &= BACKGROUND_IMAGE_MASK;
            bits >>= BACKGROUND_IMAGE_SHIFT;

            /* 1bit: t : type */
            image = i.background_image;

            return (CssBackgroundImageEnum)(bits & 0x1);
        }

        // autogenerated_propset.h:120
        public void SetBackgroundImage(CssBackgroundImageEnum type, string image)
        {
            uint bits = i.bits[BACKGROUND_IMAGE_INDEX];

            /* 1bit: t : type */
            i.bits[BACKGROUND_IMAGE_INDEX] = (bits & ~BACKGROUND_IMAGE_MASK) |
                (((uint)type & 0x1) << BACKGROUND_IMAGE_SHIFT);

            if (!string.IsNullOrEmpty(image))
            {
                i.background_image = image;
            }
            else
            {
                i.background_image = "";
            }
        }

        // autogenerated_propget.h:121
        public CssBackgroundPositionEnum GetBackgroundPosition(out Fixed len1, out CssUnit unit1, out Fixed len2, out CssUnit unit2)
        {
            uint bits = i.bits[BACKGROUND_POSITION_INDEX];
            bits &= BACKGROUND_POSITION_MASK;
            bits >>= BACKGROUND_POSITION_SHIFT;

            /* 11bits: aaaaabbbbbt : unit_a | unit_b | type */
            if ((bits & 0x1) == (byte)CssBackgroundPositionEnum.CSS_BACKGROUND_POSITION_SET)
            {
                len1 = i.background_position_a;
                len2 = i.background_position_b;
                unit1 = (CssUnit)(bits >> 6);
                unit2 = (CssUnit)((bits & 0x3e) >> 1);
            }
            else
            {
                throw new Exception("Wrong call");
            }

            return (CssBackgroundPositionEnum)(bits & 0x1);
        }

        // autogenerated_propset.h:155
        public void SetBackgroundPosition(byte type, Fixed len1, CssUnit unit1, Fixed len2, CssUnit unit2)
        {
            var bits = i.bits[BACKGROUND_POSITION_INDEX];

            /* 11bits: aaaaabbbbbt : unit_a | unit_b | type */
            i.bits[BACKGROUND_POSITION_INDEX] =
                (bits & ~BACKGROUND_POSITION_MASK) | ((((uint)type & 0x1)
                    | ((uint)unit1 << 1) | ((uint)unit2 << 6)) << BACKGROUND_POSITION_SHIFT);

            i.background_position_a = len1;
            i.background_position_b = len2;
        }

        // autogenerated_propget.h:143
        public CssBackgroundRepeat GetBackgroundRepeat()
        {
            uint bits = i.bits[BACKGROUND_REPEAT_INDEX];
            bits &= BACKGROUND_REPEAT_MASK;
            bits >>= BACKGROUND_REPEAT_SHIFT;

            /* 3bits: ttt : type */
            return (CssBackgroundRepeat)(bits & 0x7);
        }

        // autogenerated_propset.h:179
        public void SetBackgroundRepeat(CssBackgroundRepeat type)
        {
            var bits = i.bits[BACKGROUND_REPEAT_INDEX];

            /* 3bits: ttt : type */
            i.bits[BACKGROUND_REPEAT_INDEX] = (bits & ~BACKGROUND_REPEAT_MASK) |
                (((uint)type & 0x7) << BACKGROUND_REPEAT_SHIFT);
        }

        public byte GetBorderBottomColor(ref Color color)
        {
            uint bits = i.bits[BORDER_BOTTOM_COLOR_INDEX];
            bits &= BORDER_BOTTOM_COLOR_MASK;
            bits >>= BORDER_BOTTOM_COLOR_SHIFT;

            /* 2bits: tt : type */
            color = i.border_bottom_color;

            return (byte)(bits & 0x3);
        }
        public void SetBorderBottomColor(byte type, Color color)
        {
            uint bits = i.bits[BORDER_BOTTOM_COLOR_INDEX];

            /* 2bits: tt : type */
            i.bits[BORDER_BOTTOM_COLOR_INDEX] =
                (bits & ~BORDER_BOTTOM_COLOR_MASK) | (((uint)type & 0x3) << BORDER_BOTTOM_COLOR_SHIFT);

            i.border_bottom_color = color;
        }

        public byte GetBorderBottomStyle()
        {
            uint bits = i.bits[BORDER_BOTTOM_STYLE_INDEX];
            bits &= BORDER_BOTTOM_STYLE_MASK;
            bits >>= BORDER_BOTTOM_STYLE_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetBorderBottomStyle(byte type)
        {
            uint bits = i.bits[BORDER_BOTTOM_STYLE_INDEX];

            /* 4bits: tttt : type */
            i.bits[BORDER_BOTTOM_STYLE_INDEX] = (bits & ~BORDER_BOTTOM_STYLE_MASK) | (((uint)type & 0xf)
                    << BORDER_BOTTOM_STYLE_SHIFT);
        }

        public CssBorderWidthEnum GetBorderBottomWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[BORDER_BOTTOM_WIDTH_INDEX];
            bits &= BORDER_BOTTOM_WIDTH_MASK;
            bits >>= BORDER_BOTTOM_WIDTH_SHIFT;

            /* 8bits: uuuuuttt : unit | type */
            CssBorderWidthEnum bw = (CssBorderWidthEnum)(bits & 0x7);
            if (bw == CssBorderWidthEnum.CSS_BORDER_WIDTH_WIDTH)
            {
                length = i.border_bottom_width;
                unit = (CssUnit)(bits >> 3);
            }

            return bw;
        }
        public void SetBorderBottomWidth(byte type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[BORDER_BOTTOM_WIDTH_INDEX];

            /* 8bits: uuuuuttt : unit | type */
            i.bits[BORDER_BOTTOM_WIDTH_INDEX] = (bits & ~BORDER_BOTTOM_WIDTH_MASK) | ((((uint)type & 0x7)
                    | ((uint)unit << 3)) << BORDER_BOTTOM_WIDTH_SHIFT);

            i.border_bottom_width = length;
        }

        public byte GetBorderCollapse()
        {
            uint bits = i.bits[BORDER_COLLAPSE_INDEX];
            bits &= BORDER_COLLAPSE_MASK;
            bits >>= BORDER_COLLAPSE_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetBorderCollapse(byte type)
        {
            uint bits = i.bits[BORDER_COLLAPSE_INDEX];

            /* 2bits: tt : type */
            i.bits[BORDER_COLLAPSE_INDEX] = (bits & ~BORDER_COLLAPSE_MASK) | (((uint)type & 0x3) <<
                    BORDER_COLLAPSE_SHIFT);
        }

        public byte GetBorderLeftColor(ref Color color)
        {
            uint bits = i.bits[BORDER_LEFT_COLOR_INDEX];
            bits &= BORDER_LEFT_COLOR_MASK;
            bits >>= BORDER_LEFT_COLOR_SHIFT;

            /* 2bits: tt : type */
            color = i.border_left_color;

            return (byte)(bits & 0x3);
        }
        public void SetBorderLeftColor(byte type, Color color)
        {
            uint bits = i.bits[BORDER_LEFT_COLOR_INDEX];

            /* 2bits: tt : type */
            i.bits[BORDER_LEFT_COLOR_INDEX] = (bits & ~BORDER_LEFT_COLOR_MASK) | (((uint)type & 0x3) <<
                    BORDER_LEFT_COLOR_SHIFT);

            i.border_left_color = color;
        }

        public byte GetBorderLeftStyle()
        {
            uint bits = i.bits[BORDER_LEFT_STYLE_INDEX];
            bits &= BORDER_LEFT_STYLE_MASK;
            bits >>= BORDER_LEFT_STYLE_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetBorderLeftStyle(byte type)
        {
            uint bits = i.bits[BORDER_LEFT_STYLE_INDEX];

            /* 4bits: tttt : type */
            i.bits[BORDER_LEFT_STYLE_INDEX] = (bits & ~BORDER_LEFT_STYLE_MASK) | (((uint)type & 0xf) <<
                    BORDER_LEFT_STYLE_SHIFT);
        }

        public CssBorderWidthEnum GetBorderLeftWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[BORDER_LEFT_WIDTH_INDEX];
            bits &= BORDER_LEFT_WIDTH_MASK;
            bits >>= BORDER_LEFT_WIDTH_SHIFT;

            /* 8bits: uuuuuttt : unit | type */
            var bw = (CssBorderWidthEnum)(bits & 0x7);
            if (bw == CssBorderWidthEnum.CSS_BORDER_WIDTH_WIDTH)
            {
                length = i.border_left_width;
                unit = (CssUnit)(bits >> 3);
            }

            return bw;
        }
        public void SetBorderLeftWidth(byte type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[BORDER_LEFT_WIDTH_INDEX];

            /* 8bits: uuuuuttt : unit | type */
            i.bits[BORDER_LEFT_WIDTH_INDEX] = (bits & ~BORDER_LEFT_WIDTH_MASK) | ((((uint)type & 0x7) | (
                    (uint)unit << 3)) << BORDER_LEFT_WIDTH_SHIFT);

            i.border_left_width = length;
        }

        public byte GetBorderRightColor(ref Color color)
        {
            uint bits = i.bits[BORDER_RIGHT_COLOR_INDEX];
            bits &= BORDER_RIGHT_COLOR_MASK;
            bits >>= BORDER_RIGHT_COLOR_SHIFT;

            /* 2bits: tt : type */
            color = i.border_right_color;

            return (byte)(bits & 0x3);

        }
        public void SetBorderRightColor(byte type, Color color)
        {
            uint bits = i.bits[BORDER_RIGHT_COLOR_INDEX];

            /* 2bits: tt : type */
            i.bits[BORDER_RIGHT_COLOR_INDEX] = (bits & ~BORDER_RIGHT_COLOR_MASK) |
                (((uint)type & 0x3) << BORDER_RIGHT_COLOR_SHIFT);

            i.border_right_color = color;
        }

        public byte GetBorderRightStyle()
        {
            uint bits = i.bits[BORDER_RIGHT_STYLE_INDEX];
            bits &= BORDER_RIGHT_STYLE_MASK;
            bits >>= BORDER_RIGHT_STYLE_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetBorderRightStyle(CssBorderWidthEnum type)
        {
            uint bits = i.bits[BORDER_RIGHT_STYLE_INDEX];

            /* 4bits: tttt : type */
            i.bits[BORDER_RIGHT_STYLE_INDEX] = (bits & ~BORDER_RIGHT_STYLE_MASK) |
                (((uint)type & 0xf) << BORDER_RIGHT_STYLE_SHIFT);
        }

        public CssBorderWidthEnum GetBorderRightWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[BORDER_RIGHT_WIDTH_INDEX];
            bits &= BORDER_RIGHT_WIDTH_MASK;
            bits >>= BORDER_RIGHT_WIDTH_SHIFT;

            /* 8bits: uuuuuttt : unit | type */
            var bw = (CssBorderWidthEnum)(bits & 0x7);
            if (bw == CssBorderWidthEnum.CSS_BORDER_WIDTH_WIDTH)
            {
                length = i.border_right_width;
                unit = (CssUnit)(bits >> 3);
            }

            return bw;
        }
        public void SetBorderRightWidth(byte type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[BORDER_RIGHT_WIDTH_INDEX];

            /* 8bits: uuuuuttt : unit | type */
            i.bits[BORDER_RIGHT_WIDTH_INDEX] = (bits & ~BORDER_RIGHT_WIDTH_MASK) |
                ((((uint)type & 0x7) | ((uint)unit << 3)) << BORDER_RIGHT_WIDTH_SHIFT);

            i.border_right_width = length;
        }

        public CssBorderSpacingEnum GetBorderSpacing(ref Fixed lengthA, ref CssUnit unitA, ref Fixed lengthB, ref CssUnit unitB)
        {
            uint bits = i.bits[BORDER_SPACING_INDEX];
            bits &= BORDER_SPACING_MASK;
            bits >>= BORDER_SPACING_SHIFT;

            /* 11bits: aaaaabbbbbt : unit_a | unit_b | type */
            var bs = (CssBorderSpacingEnum)(bits & 0x1);
            if (bs == CssBorderSpacingEnum.CSS_BORDER_SPACING_SET)
            {
                lengthA = i.border_spacing_a;
                lengthB = i.border_spacing_b;
                unitA = (CssUnit)(bits >> 6);
                unitB = (CssUnit)((bits & 0x3e) >> 1);
            }

            return bs;
        }
        public void SetBorderSpacing(CssBorderSpacingEnum type, Fixed lengthA, CssUnit unitA, Fixed lengthB, CssUnit unitB)
        {
            uint bits = i.bits[BORDER_SPACING_INDEX];

            /* 11bits: aaaaabbbbbt : unit_a | unit_b | type */
            i.bits[BORDER_SPACING_INDEX] = (bits & ~BORDER_SPACING_MASK) |
                ((((uint)type & 0x1) | ((uint)unitB << 1) | ((uint)unitA << 6)) << BORDER_SPACING_SHIFT);

            i.border_spacing_a = lengthA;

            i.border_spacing_b = lengthB;
        }

        public byte GetBorderTopColor(ref Color color)
        {
            uint bits = i.bits[BORDER_TOP_COLOR_INDEX];
            bits &= BORDER_TOP_COLOR_MASK;
            bits >>= BORDER_TOP_COLOR_SHIFT;

            /* 2bits: tt : type */
            color = i.border_top_color;

            return (byte)(bits & 0x3);
        }
        public void SetBorderTopColor(byte type, Color color)
        {
            uint bits = i.bits[BORDER_TOP_COLOR_INDEX];
            /* 2bits: tt : type */
            i.bits[BORDER_TOP_COLOR_INDEX] = (bits & ~BORDER_TOP_COLOR_MASK) |
                (((uint)type & 0x3) << BORDER_TOP_COLOR_SHIFT);

            i.border_top_color = color;
        }

        public byte GetBorderTopStyle()
        {
            uint bits = i.bits[BORDER_TOP_STYLE_INDEX];
            bits &= BORDER_TOP_STYLE_MASK;
            bits >>= BORDER_TOP_STYLE_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetBorderTopStyle(byte type)
        {
            uint bits = i.bits[BORDER_TOP_STYLE_INDEX];

            /* 4bits: tttt : type */
            i.bits[BORDER_TOP_STYLE_INDEX] = (bits & ~BORDER_TOP_STYLE_MASK) | (((uint)type & 0xf) <<
                    BORDER_TOP_STYLE_SHIFT);
        }

        public CssBorderWidthEnum GetBorderTopWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[BORDER_TOP_WIDTH_INDEX];
            bits &= BORDER_TOP_WIDTH_MASK;
            bits >>= BORDER_TOP_WIDTH_SHIFT;

            /* 8bits: uuuuuttt : unit | type */
            var bw = (CssBorderWidthEnum)(bits & 0x7);
            if (bw == CssBorderWidthEnum.CSS_BORDER_WIDTH_WIDTH)
            {
                length = i.border_right_width;
                unit = (CssUnit)(bits >> 3);
            }

            return bw;
        }
        public void SetBorderTopWidth(CssBorderWidthEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[BORDER_TOP_WIDTH_INDEX];

            /* 8bits: uuuuuttt : unit | type */
            i.bits[BORDER_TOP_WIDTH_INDEX] = (bits & ~BORDER_TOP_WIDTH_MASK) |
                ((((uint)type & 0x7) | ((uint)unit << 3)) << BORDER_TOP_WIDTH_SHIFT);

            i.border_top_width = length;
        }

        public CssBottomEnum GetBottom(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[BOTTOM_INDEX];
            bits &= BOTTOM_MASK;
            bits >>= BOTTOM_SHIFT;

            /* 7bits: uuuuutt : units | type */
            var val = (CssBottomEnum)(bits & 0x3);
            if (val == CssBottomEnum.CSS_BOTTOM_SET)
            {
                length = i.bottom;
                unit = (CssUnit)(bits >> 2);
            }

            return val;

        }
        public void SetBottom(CssBottomEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[BOTTOM_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[BOTTOM_INDEX] = (bits & ~BOTTOM_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << BOTTOM_SHIFT);

            i.bottom = length;
        }

        public byte GetBottomBits()
        {
            uint bits = i.bits[BOTTOM_INDEX];
            bits &= BOTTOM_MASK;
            bits >>= BOTTOM_SHIFT;

            /* 7bits: uuuuutt : units | type */
            return (byte)bits;
        }
        public byte GetBoxSizing()
        {
            uint bits = i.bits[BOX_SIZING_INDEX];
            bits &= BOX_SIZING_MASK;
            bits >>= BOX_SIZING_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetBoxSizing(byte type)
        {
            uint bits = i.bits[BOX_SIZING_INDEX];

            /* 2bits: tt : type */
            i.bits[BOX_SIZING_INDEX] = (bits & ~BOX_SIZING_MASK) | (((uint)type & 0x3) <<
                    BOX_SIZING_SHIFT);
        }

        public byte GetBreakAfter()
        {
            uint bits = i.bits[BREAK_AFTER_INDEX];
            bits &= BREAK_AFTER_MASK;
            bits >>= BREAK_AFTER_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetBreakAfter(byte type)
        {
            uint bits = i.bits[BREAK_AFTER_INDEX];

            /* 4bits: tttt : type */
            i.bits[BREAK_AFTER_INDEX] = (bits & ~BREAK_AFTER_MASK) | (((uint)type & 0xf) <<
                    BREAK_AFTER_SHIFT);
        }

        public byte GetBreakBefore()
        {
            uint bits = i.bits[BREAK_BEFORE_INDEX];
            bits &= BREAK_BEFORE_MASK;
            bits >>= BREAK_BEFORE_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetBreakBefore(byte type)
        {
            uint bits = i.bits[BREAK_BEFORE_INDEX];

            /* 4bits: tttt : type */
            i.bits[BREAK_BEFORE_INDEX] = (bits & ~BREAK_BEFORE_MASK) | (((uint)type & 0xf) <<
                    BREAK_BEFORE_SHIFT);
        }

        public byte GetBreakInside()
        {
            uint bits = i.bits[BREAK_INSIDE_INDEX];
            bits &= BREAK_INSIDE_MASK;
            bits >>= BREAK_INSIDE_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetBreakInside(byte type)
        {
            uint bits = i.bits[BREAK_INSIDE_INDEX];

            /* 4bits: tttt : type */
            i.bits[BREAK_INSIDE_INDEX] = (bits & ~BREAK_INSIDE_MASK) | (((uint)type & 0xf) <<
                    BREAK_INSIDE_SHIFT);
        }

        public byte GetCaptionSide()
        {
            uint bits = i.bits[CAPTION_SIDE_INDEX];
            bits &= CAPTION_SIDE_MASK;
            bits >>= CAPTION_SIDE_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetCaptionSide(byte type)
        {
            uint bits = i.bits[CAPTION_SIDE_INDEX];

            /* 2bits: tt : type */
            i.bits[CAPTION_SIDE_INDEX] = (bits & ~CAPTION_SIDE_MASK) | (((uint)type & 0x3) <<
                    CAPTION_SIDE_SHIFT);
        }

        public byte GetClear()
        {
            uint bits = i.bits[CLEAR_INDEX];
            bits &= CLEAR_MASK;
            bits >>= CLEAR_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetClear(byte type)
        {
            uint bits = i.bits[CLEAR_INDEX];

            /* 3bits: ttt : type */
            i.bits[CLEAR_INDEX] = (bits & ~CLEAR_MASK) | (((uint)type & 0x7) << CLEAR_SHIFT);
        }

        public CssClipEnum GetClip(ref CssComputedClipRect rect)
        {
            uint bits = i.bits[CLIP_INDEX];
            bits &= CLIP_MASK;
            bits >>= CLIP_SHIFT;

            /*
            26bits: tt tttr rrrr bbbb blll llTR BLyy:
            units: top | right | bottom | left
            opcodes: top | right | bottom | left | type
            */
            var val = (CssClipEnum)(bits & 0x3);
            if (val == CssClipEnum.CSS_CLIP_RECT)
            {
                rect.LeftAuto = (bits & 0x4) != 0;
                rect.BottomAuto = (bits & 0x8) != 0;
                rect.RightAuto = (bits & 0x10) != 0;
                rect.TopAuto = (bits & 0x20) != 0;

                rect.Top = i.clip_a;
                rect.Tunit = (CssUnit)(bits & 0x3e00000 >> 21);

                rect.Right = i.clip_b;
                rect.Runit = (CssUnit)(bits & 0x1f0000 >> 16);

                rect.Bottom = i.clip_c;
                rect.Bunit = (CssUnit)((bits & 0xf800) >> 11);

                rect.Left = i.clip_d;
                rect.Lunit = (CssUnit)((bits & 0x7c0) >> 6);
            }

            return val;
        }
        public void SetClip(CssClipEnum type, CssComputedClipRect rect)
        {
            uint bits = i.bits[CLIP_INDEX];

            /*
            26bits: tt tttr rrrr bbbb blll llTR BLyy:
            units: top | right | bottom | left
            opcodes: top | right | bottom | left | type
            */
            i.bits[CLIP_INDEX] = (bits & ~CLIP_MASK) | (((uint)type & 0x3) << CLIP_SHIFT);

            if (type == CssClipEnum.CSS_CLIP_RECT)
            {
                i.bits[CLIP_INDEX] |= (uint)(((rect.TopAuto ? 0x20 : 0) |
                        (rect.RightAuto ? 0x10 : 0) |
                        (rect.BottomAuto ? 0x8 : 0) |
                        (rect.LeftAuto ? 0x4 : 0)) << CLIP_SHIFT);

                i.bits[CLIP_INDEX] |= ((((uint)rect.Tunit << 5) | (uint)rect.Runit)
                        << (CLIP_SHIFT + 16));

                i.bits[CLIP_INDEX] |= ((((uint)rect.Bunit << 5) | (uint)rect.Lunit)
                        << (CLIP_SHIFT + 6));

                i.clip_a = rect.Top;
                i.clip_b = rect.Right;
                i.clip_c = rect.Bottom;
                i.clip_d = rect.Left;
            }
        }

        // autogenerated_propget.h:611
        public byte GetColor(out Color color)
        {
            uint bits = i.bits[COLOR_INDEX];
            bits &= COLOR_MASK;
            bits >>= COLOR_SHIFT;

            /* 1bit: t : type */
            color = i.color;

            return (byte)(bits & 0x1);
        }

        // autogenerated_propset.h:703
        public void SetColor(byte type, Color color)
        {
            uint bits = i.bits[COLOR_INDEX];

            // 1bit: t : type
            i.bits[COLOR_INDEX] = (bits & ~COLOR_MASK) | (((uint)type & 0x1) << COLOR_SHIFT);

            i.color = color;
        }

        public byte GetColumnCount(ref int val)
        {
            uint bits = i.bits[COLUMN_COUNT_INDEX];
            bits &= COLUMN_COUNT_MASK;
            bits >>= COLUMN_COUNT_SHIFT;

            /* 2bits: tt : type */
            val = i.column_count;

            return (byte)(bits & 0x3);
        }
        public void SetColumnCount(byte type, int integer)
        {
            uint bits = i.bits[COLUMN_COUNT_INDEX];

            /* 2bits: tt : type */
            i.bits[COLUMN_COUNT_INDEX] = (bits & ~COLUMN_COUNT_MASK) | (((uint)type & 0x3) <<
                    COLUMN_COUNT_SHIFT);

            i.column_count = integer;
        }

        public byte GetColumnFill()
        {
            uint bits = i.bits[COLUMN_FILL_INDEX];
            bits &= COLUMN_FILL_MASK;
            bits >>= COLUMN_FILL_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetColumnFill(byte type)
        {
            uint bits = i.bits[COLUMN_FILL_INDEX];
            /* 2bits: tt : type */
            i.bits[COLUMN_FILL_INDEX] = (bits & ~COLUMN_FILL_MASK) | (((uint)type & 0x3) <<
                    COLUMN_FILL_SHIFT);
        }

        public CssColumnGapEnum GetColumnGap(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[COLUMN_GAP_INDEX];
            bits &= COLUMN_GAP_MASK;
            bits >>= COLUMN_GAP_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssColumnGapEnum)(bits & 0x3);
            if (val == CssColumnGapEnum.CSS_COLUMN_GAP_SET)
            {
                length = i.column_gap;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetColumnGap(CssColumnGapEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[COLUMN_GAP_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[COLUMN_GAP_INDEX] = (bits & ~COLUMN_GAP_MASK) | ((((uint)type & 0x3) | ((uint)unit << 2)) << COLUMN_GAP_SHIFT);

            i.column_gap = length;
        }

        public CssColumnRuleColorEnum GetColumnRuleColor(ref Color color)
        {
            uint bits = i.bits[COLUMN_RULE_COLOR_INDEX];
            bits &= COLUMN_RULE_COLOR_MASK;
            bits >>= COLUMN_RULE_COLOR_SHIFT;

            /* 2bits: tt : type */
            color = i.column_rule_color;

            return (CssColumnRuleColorEnum)(bits & 0x3);
        }
        public void SetColumnRuleColor(CssColumnRuleColorEnum type, Color color)
        {
            uint bits = i.bits[COLUMN_RULE_COLOR_INDEX];

            /* 2bits: tt : type */
            i.bits[COLUMN_RULE_COLOR_INDEX] = (bits & ~COLUMN_RULE_COLOR_MASK) | (((uint)type & 0x3) <<
                    COLUMN_RULE_COLOR_SHIFT);

            i.column_rule_color = color;
        }

        public byte GetColumnRuleStyle()
        {
            uint bits = i.bits[COLUMN_RULE_STYLE_INDEX];
            bits &= COLUMN_RULE_STYLE_MASK;
            bits >>= COLUMN_RULE_STYLE_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetColumnRuleStyle(byte type)
        {
            uint bits = i.bits[COLUMN_RULE_STYLE_INDEX];

            /* 4bits: tttt : type */
            i.bits[COLUMN_RULE_STYLE_INDEX] = (bits & ~COLUMN_RULE_STYLE_MASK) | (((uint)type & 0xf) <<
                    COLUMN_RULE_STYLE_SHIFT);
        }

        public CssColumnRuleWidthEnum GetColumnRuleWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[COLUMN_RULE_WIDTH_INDEX];
            bits &= COLUMN_RULE_WIDTH_MASK;
            bits >>= COLUMN_RULE_WIDTH_SHIFT;

            /* 8bits: uuuuuttt : unit | type */
            var val = (CssColumnRuleWidthEnum)(bits & 0x7);
            if (val == CssColumnRuleWidthEnum.CSS_COLUMN_RULE_WIDTH_WIDTH)
            {
                length = i.column_rule_width;
                unit = (CssUnit)(bits >> 3);
            }

            return val;
        }
        public void SetColumnRuleWidth(CssColumnRuleWidthEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[COLUMN_RULE_WIDTH_INDEX];

            /* 8bits: uuuuuttt : unit | type */
            i.bits[COLUMN_RULE_WIDTH_INDEX] = (bits & ~COLUMN_RULE_WIDTH_MASK) | ((((uint)type & 0x7) | (
                    (uint)unit << 3)) << COLUMN_RULE_WIDTH_SHIFT);

            i.column_rule_width = length;
        }

        public byte GetColumnSpan()
        {
            uint bits = i.bits[COLUMN_SPAN_INDEX];
            bits &= COLUMN_SPAN_MASK;
            bits >>= COLUMN_SPAN_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetColumnSpan(byte type)
        {
            uint bits = i.bits[COLUMN_SPAN_INDEX];

            /* 2bits: tt : type */
            i.bits[COLUMN_SPAN_INDEX] = (bits & ~COLUMN_SPAN_MASK) | (((uint)type & 0x3) <<
                    COLUMN_SPAN_SHIFT);
        }

        public CssColumnWidthEnum GetColumnWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[COLUMN_WIDTH_INDEX];
            bits &= COLUMN_WIDTH_MASK;
            bits >>= COLUMN_WIDTH_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssColumnWidthEnum)(bits & 0x3);
            if (val == CssColumnWidthEnum.CSS_COLUMN_WIDTH_SET)
            {
                length = i.column_width;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetColumnWidth(CssColumnWidthEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[COLUMN_WIDTH_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[COLUMN_WIDTH_INDEX] = (bits & ~COLUMN_WIDTH_MASK) | ((((uint)type & 0x3) | ((uint)unit
                    << 2)) << COLUMN_WIDTH_SHIFT);

            i.column_width = length;
        }

        // autogenerated_propget.h:785
        public CssContent GetContent(ref CssComputedContentItem[] contentItem)
        {
            uint bits = i.bits[CONTENT_INDEX];
            bits &= CONTENT_MASK;
            bits >>= CONTENT_SHIFT;

            /* 2bits: tt : type */
            var val = (CssContent)(bits & 0x3);
            if (val == CssContent.CSS_CONTENT_SET)
            {
                contentItem = Content;
            }

            return val;
        }
        // autogenerated_propset.h:903
        public void SetContent(CssContent type, ref CssComputedContentItem[] content)
        {
            uint bits = i.bits[CONTENT_INDEX];

            /* 2bits: type */
            //oldcontent = style->content;

            Log.Unimplemented("content needs testplementing");

            i.bits[CONTENT_INDEX] = (bits & ~CONTENT_MASK) | (((uint)type & 0x3) << CONTENT_SHIFT);

            //for (c = Content; c.Type != CssContent.CSS_COMPUTED_CONTENT_NONE; c++)
            foreach (var c in content)
            {
                switch (c.Type)
                {
                    case CssComputedContentType.CSS_COMPUTED_CONTENT_STRING:
                        //c.Name = lwc_string_ref(c->data.string);
                        break;
                    case CssComputedContentType.CSS_COMPUTED_CONTENT_URI:
                        //c.Name = lwc_string_ref(c->data.uri);
                        break;
                    case CssComputedContentType.CSS_COMPUTED_CONTENT_ATTR:
                        //c.Name = lwc_string_ref(c->data.attr);
                        break;
                    case CssComputedContentType.CSS_COMPUTED_CONTENT_COUNTER:
                        //c->data.counter.name = lwc_string_ref(c->data.counter.name);
                        break;
                    case CssComputedContentType.CSS_COMPUTED_CONTENT_COUNTERS:
                        //c->data.counters.name = lwc_string_ref(c->data.counters.name);
                        //c->data.counters.sep = lwc_string_ref(c->data.counters.sep);
                        break;
                    default:
                        break;
                }
            }

            Content = content;
        }

        public byte GetCounterIncrement(ref CssComputedCounter[] counterArr)
        {
            uint bits = i.bits[COUNTER_INCREMENT_INDEX];
            bits &= COUNTER_INCREMENT_MASK;
            bits >>= COUNTER_INCREMENT_SHIFT;

            /* 1bit: t : type */
            counterArr = CounterIncrement;

            return (byte)(bits & 0x1);
        }
        public void SetCounterIncrement(byte type, CssComputedCounter[] counterArr)
        {
            uint bits = i.bits[COUNTER_INCREMENT_INDEX];
            Log.Unimplemented("set counter_increment");
        }

        public byte GetCounterReset(ref CssComputedCounter[] counterArr)
        {
            uint bits = i.bits[COUNTER_RESET_INDEX];
            bits &= COUNTER_RESET_MASK;
            bits >>= COUNTER_RESET_SHIFT;

            /* 1bit: t : type */
            counterArr = CounterReset;

            return (byte)(bits & 0x1);
        }
        public void SetCounterReset(byte type, CssComputedCounter[] counterArr)
        {
            uint bits = i.bits[COUNTER_RESET_INDEX];
            Log.Unimplemented("set counter_increment");
        }

        public byte GetCursor(string[] stringArr)
        {
            uint bits = i.bits[CURSOR_INDEX];
            bits &= CURSOR_MASK;
            bits >>= CURSOR_SHIFT;

            /* 5bits: ttttt : type */
            stringArr = Cursor;

            return (byte)(bits & 0x1f);
        }
        public void SetCursor(byte type, string[] stringArr)
        {
            uint bits = i.bits[CURSOR_INDEX];

            /* 5bits: ttttt : type */
            i.bits[CURSOR_INDEX] = (bits & ~CURSOR_MASK) | (((uint)type & 0x1f) << CURSOR_SHIFT);

            Log.Unimplemented();
        }

        public byte GetDirection()
        {
            uint bits = i.bits[DIRECTION_INDEX];
            bits &= DIRECTION_MASK;
            bits >>= DIRECTION_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetDirection(byte type)
        {
            uint bits = i.bits[DIRECTION_INDEX];

            /* 2bits: tt : type */
            i.bits[DIRECTION_INDEX] = (bits & ~DIRECTION_MASK) | (((uint)type & 0x3) << DIRECTION_SHIFT);
        }

        // autogenerated_propget.h:880
        public CssDisplay GetDisplay()
        {
            uint bits = i.bits[DISPLAY_INDEX];
            bits &= DISPLAY_MASK;
            bits >>= DISPLAY_SHIFT;

            /* 5bits: ttttt : type */
            return (CssDisplay)(bits & 0x1f);
        }

        // autogenerated_propset.h:1118
        public void SetDisplay(CssDisplay type)
        {
            uint bits = i.bits[DISPLAY_INDEX];

            /* 5bits: ttttt : type */
            i.bits[DISPLAY_INDEX] = (bits & ~DISPLAY_MASK) | (((uint)type & 0x1f) << DISPLAY_SHIFT);
        }

        public byte GetEmptyCells()
        {
            uint bits = i.bits[EMPTY_CELLS_INDEX];
            bits &= EMPTY_CELLS_MASK;
            bits >>= EMPTY_CELLS_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetEmptyCells(byte type)
        {
            uint bits = i.bits[EMPTY_CELLS_INDEX];

            /* 2bits: tt : type */
            i.bits[EMPTY_CELLS_INDEX] = (bits & ~EMPTY_CELLS_MASK) | (((uint)type & 0x3) << EMPTY_CELLS_SHIFT);
        }

        public CssFlexBasisEnum GetFlexBasis(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[FLEX_BASIS_INDEX];
            bits &= FLEX_BASIS_MASK;
            bits >>= FLEX_BASIS_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssFlexBasisEnum)(bits & 0x3);
            if (val == CssFlexBasisEnum.CSS_FLEX_BASIS_SET)
            {
                length = i.flex_basis;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetFlexBasis(CssFlexBasisEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[FLEX_BASIS_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[FLEX_BASIS_INDEX] = (bits & ~FLEX_BASIS_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << FLEX_BASIS_SHIFT);

            i.flex_basis = length;
        }

        public byte GetFlexDirection()
        {
            uint bits = i.bits[FLEX_DIRECTION_INDEX];
            bits &= FLEX_DIRECTION_MASK;
            bits >>= FLEX_DIRECTION_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetFlexDirection(byte type)
        {
            uint bits = i.bits[FLEX_DIRECTION_INDEX];

            /* 3bits: ttt : type */
            i.bits[FLEX_DIRECTION_INDEX] = (bits & ~FLEX_DIRECTION_MASK) | (((uint)type & 0x7) <<
                    FLEX_DIRECTION_SHIFT);
        }

        public CssFlexGrowEnum GetFlexGrow(Fixed value)
        {
            uint bits = i.bits[FLEX_GROW_INDEX];
            bits &= FLEX_GROW_MASK;
            bits >>= FLEX_GROW_SHIFT;

            /* 1bit: t : type */
            var en = (CssFlexGrowEnum)(bits & 0x1);
            if (en == CssFlexGrowEnum.CSS_FLEX_GROW_SET)
            {
                value = i.flex_grow;
            }

            return en;
        }
        public void SetFlexGrow(CssFlexGrowEnum type, Fixed value)
        {
            uint bits = i.bits[FLEX_GROW_INDEX];

            /* 1bit: t : type */
            i.bits[FLEX_GROW_INDEX] = (bits & ~FLEX_GROW_MASK) | (((uint)type & 0x1) <<
                    FLEX_GROW_SHIFT);

            i.flex_grow = value;
        }

        public CssFlexShrinkEnum GetFlexShrink(Fixed value)
        {
            uint bits = i.bits[FLEX_SHRINK_INDEX];
            bits &= FLEX_SHRINK_MASK;
            bits >>= FLEX_SHRINK_SHIFT;

            /* 1bit: t : type */
            var en = (CssFlexShrinkEnum)(bits & 0x1);
            if (en == CssFlexShrinkEnum.CSS_FLEX_SHRINK_SET)
            {
                value = i.flex_shrink;
            }

            return en;
        }
        public void SetFlexShrink(CssFlexShrinkEnum type, Fixed value)
        {
            uint bits = i.bits[FLEX_SHRINK_INDEX];

            /* 1bit: t : type */
            i.bits[FLEX_SHRINK_INDEX] = (bits & ~FLEX_SHRINK_MASK) | (((uint)type & 0x1) <<
                    FLEX_SHRINK_SHIFT);

            i.flex_shrink = value;
        }

        public byte GetFlexWrap()
        {
            uint bits = i.bits[FLEX_WRAP_INDEX];
            bits &= FLEX_WRAP_MASK;
            bits >>= FLEX_WRAP_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetFlexWrap(byte type)
        {
            uint bits = i.bits[FLEX_WRAP_INDEX];

            /* 2bits: tt : type */
            i.bits[FLEX_WRAP_INDEX] = (bits & ~FLEX_WRAP_MASK) | (((uint)type & 0x3) <<
                    FLEX_WRAP_SHIFT);
        }

        // autogenerated_props:1012
        byte GetFloat()
        {
            uint bits = i.bits[FLOAT_INDEX];
            bits &= FLOAT_MASK;
            bits >>= FLOAT_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetFloat(byte type)
        {
            uint bits = i.bits[FLOAT_INDEX];

            /* 2bits: tt : type */
            i.bits[FLOAT_INDEX] = (bits & ~FLOAT_MASK) | (((uint)type & 0x3) << FLOAT_SHIFT);
        }

        public byte GetFontFamily(ref string[] stringArr)
        {
            uint bits = i.bits[FONT_FAMILY_INDEX];
            bits &= FONT_FAMILY_MASK;
            bits >>= FONT_FAMILY_SHIFT;

            /* 3bits: ttt : type */
            stringArr = FontFamily;

            return (byte)(bits & 0x7);
        }
        public void SetFontFamily(byte type, string[] stringArr)
        {
            uint bits = i.bits[FONT_FAMILY_INDEX];
            Log.Unimplemented();
        }

        // autogenerated_propget.h:1048
        public CssFontSizeEnum GetFontSize(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[FONT_SIZE_INDEX];
            bits &= FONT_SIZE_MASK;
            bits >>= FONT_SIZE_SHIFT;

            // 9bits: uuuuutttt : unit | type
            if ((bits & 0xf) == (byte)CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION)
            {
                length = i.font_size;
                unit = (CssUnit)(bits >> 4);
            }

            return (CssFontSizeEnum)(bits & 0xf);
        }

        // autogenerated_propset.h:1327
        public void SetFontSize(CssFontSizeEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[FONT_SIZE_INDEX];

            // 9bits: uuuuutttt : unit | type
            i.bits[FONT_SIZE_INDEX] =
                (bits & ~FONT_SIZE_MASK) | ((((uint)type & 0xf) | ((uint)unit << 4)) << FONT_SIZE_SHIFT);

            i.font_size = length;
        }

        public byte GetFontStyle()
        {
            uint bits = i.bits[FONT_STYLE_INDEX];
            bits &= FONT_STYLE_MASK;
            bits >>= FONT_STYLE_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetFontStyle(byte type)
        {
            uint bits = i.bits[FONT_STYLE_INDEX];

            /* 2bits: tt : type */
            i.bits[FONT_STYLE_INDEX] = (bits & ~FONT_STYLE_MASK) | (((uint)type & 0x3) << FONT_STYLE_SHIFT);
        }

        public byte GetFontVariant()
        {
            uint bits = i.bits[FONT_VARIANT_INDEX];
            bits &= FONT_VARIANT_MASK;
            bits >>= FONT_VARIANT_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetFontVariant(byte type)
        {
            uint bits = i.bits[FONT_VARIANT_INDEX];

            /* 2bits: tt : type */
            i.bits[FONT_VARIANT_INDEX] = (bits & ~FONT_VARIANT_MASK) | (((uint)type & 0x3) << FONT_VARIANT_SHIFT);
        }

        public byte GetFontWeight()
        {
            uint bits = i.bits[FONT_WEIGHT_INDEX];
            bits &= FONT_WEIGHT_MASK;
            bits >>= FONT_WEIGHT_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetFontWeight(byte type)
        {
            uint bits = i.bits[FONT_WEIGHT_INDEX];

            /* 4bits: tttt : type */
            i.bits[FONT_WEIGHT_INDEX] = (bits & ~FONT_WEIGHT_MASK) | (((uint)type & 0xf) <<
                    FONT_WEIGHT_SHIFT);
        }

        public CssHeightEnum GetHeight(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[HEIGHT_INDEX];
            bits &= HEIGHT_MASK;
            bits >>= HEIGHT_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssHeightEnum)(bits & 0x3);
            if (val == CssHeightEnum.CSS_HEIGHT_SET)
            {
                length = i.height;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetHeight(CssHeightEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[HEIGHT_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[HEIGHT_INDEX] = (bits & ~HEIGHT_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << HEIGHT_SHIFT);

            i.height = length;
        }

        public byte GetJustifyContent()
        {
            uint bits = i.bits[JUSTIFY_CONTENT_INDEX];
            bits &= JUSTIFY_CONTENT_MASK;
            bits >>= JUSTIFY_CONTENT_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetJustifyContent(byte type)
        {
            uint bits = i.bits[JUSTIFY_CONTENT_INDEX];

            /* 3bits: ttt : type */
            i.bits[JUSTIFY_CONTENT_INDEX] = (bits & ~JUSTIFY_CONTENT_MASK) | (((uint)type & 0x7) <<
                    JUSTIFY_CONTENT_SHIFT);
        }

        public CssLeftEnum GetLeft(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[LEFT_INDEX];
            bits &= LEFT_MASK;
            bits >>= LEFT_SHIFT;

            /* 7bits: uuuuutt : units | type */
            var val = (CssLeftEnum)(bits & 0x3);
            if (val == CssLeftEnum.CSS_LEFT_SET)
            {
                length = i.left;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetLeft(CssLeftEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[LEFT_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[LEFT_INDEX] = (bits & ~LEFT_MASK) | ((((uint)type & 0x3) | ((uint)unit << 2))
                    << LEFT_SHIFT);

            i.left = length;
        }

        public byte GetLeftBits()
        {
            uint bits = i.bits[LEFT_INDEX];
            bits &= LEFT_MASK;
            bits >>= LEFT_SHIFT;

            /* 7bits: uuuuutt : units | type */
            return (byte)bits;
        }
        public CssLetterSpacingEnum GetLetterSpacing(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[LETTER_SPACING_INDEX];
            bits &= LETTER_SPACING_MASK;
            bits >>= LETTER_SPACING_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssLetterSpacingEnum)(bits & 0x3);
            if (val == CssLetterSpacingEnum.CSS_LETTER_SPACING_SET)
            {
                length = i.letter_spacing;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetLetterSpacing(CssLetterSpacingEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[LETTER_SPACING_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[LETTER_SPACING_INDEX] = (bits & ~LETTER_SPACING_MASK) | ((((uint)type & 0x3) | (
                    (uint)unit << 2)) << LETTER_SPACING_SHIFT);

            i.letter_spacing = length;
        }

        public CssLineHeightEnum GetLineHeight(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[LINE_HEIGHT_INDEX];
            bits &= LINE_HEIGHT_MASK;
            bits >>= LINE_HEIGHT_SHIFT;

            /* 7bits: uuuuutt : units | type */
            var val = (CssLineHeightEnum)(bits & 0x3);
            if (val == CssLineHeightEnum.CSS_LINE_HEIGHT_NUMBER ||
                val == CssLineHeightEnum.CSS_LINE_HEIGHT_DIMENSION)
            {
                length = i.line_height;
            }

            if (val == CssLineHeightEnum.CSS_LINE_HEIGHT_DIMENSION)
            {
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetLineHeight(CssLineHeightEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[LINE_HEIGHT_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[LINE_HEIGHT_INDEX] = (bits & ~LINE_HEIGHT_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << LINE_HEIGHT_SHIFT);

            i.line_height = length;
        }

        public byte GetListStyleImage(ref string value)
        {
            uint bits = i.bits[LIST_STYLE_IMAGE_INDEX];
            bits &= LIST_STYLE_IMAGE_MASK;
            bits >>= LIST_STYLE_IMAGE_SHIFT;

            /* 1bit: t : type */
            value = i.list_style_image;

            return (byte)(bits & 0x1);
        }
        public void SetListStyleImage(byte type, string value)
        {
            uint bits = i.bits[LIST_STYLE_IMAGE_INDEX];

            /* 1bit: t : type */
            i.bits[LIST_STYLE_IMAGE_INDEX] = (bits & ~LIST_STYLE_IMAGE_MASK) | (((uint)type & 0x1) <<
                    LIST_STYLE_IMAGE_SHIFT);

            i.list_style_image = value;
        }

        public byte GetListStylePosition()
        {
            uint bits = i.bits[LIST_STYLE_POSITION_INDEX];
            bits &= LIST_STYLE_POSITION_MASK;
            bits >>= LIST_STYLE_POSITION_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetListStylePosition(byte type)
        {
            uint bits = i.bits[LIST_STYLE_POSITION_INDEX];

            /* 2bits: tt : type */
            i.bits[LIST_STYLE_POSITION_INDEX] = (bits & ~LIST_STYLE_POSITION_MASK) |
                (((uint)type & 0x3) << LIST_STYLE_POSITION_SHIFT);
        }

        public byte GetListStyleType()
        {
            uint bits = i.bits[LIST_STYLE_TYPE_INDEX];
            bits &= LIST_STYLE_TYPE_MASK;
            bits >>= LIST_STYLE_TYPE_SHIFT;

            /* 6bits: tttttt : type */

            return (byte)(bits & 0x3f);
        }
        public void SetListStyleType(byte type)
        {
            uint bits = i.bits[LIST_STYLE_TYPE_INDEX];

            /* 6bits: tttttt : type */
            i.bits[LIST_STYLE_TYPE_INDEX] = (bits & ~LIST_STYLE_TYPE_MASK) |
                (((uint)type & 0x3f) << LIST_STYLE_TYPE_SHIFT);
        }

        // autogenerated_propget.h:1297
        public CssMarginEnum GetMarginBottom(ref Fixed length, ref CssUnit unit)
        {

            uint bits = i.bits[MARGIN_BOTTOM_INDEX];
            bits &= MARGIN_BOTTOM_MASK;
            bits >>= MARGIN_BOTTOM_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            if ((CssMarginEnum)(bits & 0x3) == CssMarginEnum.CSS_MARGIN_SET)
            {
                length = i.margin_bottom;
                unit = (CssUnit)(bits >> 2);
            }

            return (CssMarginEnum)(bits & 0x3);
        }

        public CssMarginEnum GetMarginLeft(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[MARGIN_LEFT_INDEX];
            bits &= MARGIN_LEFT_MASK;
            bits >>= MARGIN_LEFT_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            if ((CssMarginEnum)(bits & 0x3) == CssMarginEnum.CSS_MARGIN_SET)
            {
                length = i.margin_left;
                unit = (CssUnit)(bits >> 2);
            }

            return (CssMarginEnum)(bits & 0x3);
        }

        public CssMarginEnum GetMarginRight(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[MARGIN_RIGHT_INDEX];
            bits &= MARGIN_RIGHT_MASK;
            bits >>= MARGIN_RIGHT_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            if ((CssMarginEnum)(bits & 0x3) == CssMarginEnum.CSS_MARGIN_SET)
            {
                length = i.margin_right;
                unit = (CssUnit)(bits >> 2);
            }

            return (CssMarginEnum)(bits & 0x3);
        }

        public CssMarginEnum GetMarginTop(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[MARGIN_TOP_INDEX];
            bits &= MARGIN_TOP_MASK;
            bits >>= MARGIN_TOP_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            if ((CssMarginEnum)(bits & 0x3) == CssMarginEnum.CSS_MARGIN_SET)
            {
                length = i.margin_top;
                unit = (CssUnit)(bits >> 2);
            }

            return (CssMarginEnum)(bits & 0x3);
        }

        // autogenerated_propset.h:1596
        public void SetMarginBottom(CssMarginEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[MARGIN_BOTTOM_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[MARGIN_BOTTOM_INDEX] = (bits & ~MARGIN_BOTTOM_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << MARGIN_BOTTOM_SHIFT);

            i.margin_bottom = length;
        }

        public void SetMarginLeft(CssMarginEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[MARGIN_LEFT_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[MARGIN_LEFT_INDEX] = (bits & ~MARGIN_LEFT_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << MARGIN_LEFT_SHIFT);

            i.margin_left = length;
        }
        public void SetMarginRight(CssMarginEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[MARGIN_RIGHT_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[MARGIN_RIGHT_INDEX] = (bits & ~MARGIN_RIGHT_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << MARGIN_RIGHT_SHIFT);

            i.margin_right = length;
        }
        public void SetMarginTop(CssMarginEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[MARGIN_TOP_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[MARGIN_TOP_INDEX] = (bits & ~MARGIN_TOP_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << MARGIN_TOP_SHIFT);

            i.margin_top = length;
        }
        public CssMaxHeightEnum GetMaxHeight(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[MAX_HEIGHT_INDEX];
            bits &= MAX_HEIGHT_MASK;
            bits >>= MAX_HEIGHT_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssMaxHeightEnum)(bits & 0x3);
            if (val == CssMaxHeightEnum.CSS_MAX_HEIGHT_SET)
            {
                length = i.max_height;
                unit = (CssUnit)(bits >> 2);
            }

            return val;

        }
        public void SetMaxHeight(CssMaxHeightEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[MAX_HEIGHT_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[MAX_HEIGHT_INDEX] = (bits & ~MAX_HEIGHT_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << MAX_HEIGHT_SHIFT);

            i.max_height = length;
        }

        public CssMaxWidthEnum GetMaxWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[MAX_WIDTH_INDEX];
            bits &= MAX_WIDTH_MASK;
            bits >>= MAX_WIDTH_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssMaxWidthEnum)(bits & 0x3);
            if (val == CssMaxWidthEnum.CSS_MAX_WIDTH_SET)
            {
                length = i.max_width;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetMaxWidth(CssMaxWidthEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[MAX_WIDTH_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[MAX_WIDTH_INDEX] = (bits & ~MAX_WIDTH_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << MAX_WIDTH_SHIFT);

            i.max_width = length;
        }

        public CssMinHeightEnum GetMinHeight(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[MIN_HEIGHT_INDEX];
            bits &= MIN_HEIGHT_MASK;
            bits >>= MIN_HEIGHT_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssMinHeightEnum)(bits & 0x3);
            if (val == CssMinHeightEnum.CSS_MIN_HEIGHT_SET)
            {
                length = i.min_height;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetMinHeight(CssMinHeightEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[MIN_HEIGHT_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[MIN_HEIGHT_INDEX] = (bits & ~MIN_HEIGHT_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << MIN_HEIGHT_SHIFT);

            i.min_height = length;
        }

        public CssMinWidthEnum GetMinWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[MIN_WIDTH_INDEX];
            bits &= MIN_WIDTH_MASK;
            bits >>= MIN_WIDTH_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssMinWidthEnum)(bits & 0x3);
            if (val == CssMinWidthEnum.CSS_MIN_WIDTH_SET)
            {
                length = i.min_width;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetMinWidth(CssMinWidthEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[MIN_WIDTH_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[MIN_WIDTH_INDEX] = (bits & ~MIN_WIDTH_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << MIN_WIDTH_SHIFT);

            i.min_width = length;
        }

        public CssOpacityEnum GetOpacity(ref Fixed value)
        {
            uint bits = i.bits[OPACITY_INDEX];
            bits &= OPACITY_MASK;
            bits >>= OPACITY_SHIFT;

            /* 1bit: t : type */
            var en = (CssOpacityEnum)(bits & 0x1);
            if (en == CssOpacityEnum.CSS_OPACITY_SET)
            {
                value = i.opacity;
            }

            return en;
        }
        public void SetOpacity(CssOpacityEnum type, Fixed value)
        {
            uint bits = i.bits[OPACITY_INDEX];

            /* 1bit: t : type */
            i.bits[OPACITY_INDEX] = (bits & ~OPACITY_MASK) | (((uint)type & 0x1) <<
                    OPACITY_SHIFT);

            i.opacity = value;
        }

        public CssOrderEnum GetOrder(ref int value)
        {
            uint bits = i.bits[ORDER_INDEX];
            bits &= ORDER_MASK;
            bits >>= ORDER_SHIFT;

            /* 1bit: t : type */
            var type = (CssOrderEnum)(bits & 0x1);
            if (type == CssOrderEnum.CSS_ORDER_SET)
            {
                value = i.order;
            }

            return type;
        }
        public void SetOrder(CssOrderEnum type, int value)
        {
            uint bits = i.bits[ORDER_INDEX];

            /* 1bit: t : type */
            i.bits[ORDER_INDEX] = (bits & ~ORDER_MASK) | (((uint)type & 0x1) << ORDER_SHIFT);

            i.order = value;
        }

        public byte GetOrphans(ref int value)
        {
            uint bits = i.bits[ORPHANS_INDEX];
            bits &= ORPHANS_MASK;
            bits >>= ORPHANS_SHIFT;

            /* 1bit: t : type */
            value = i.orphans;

            return (byte)(bits & 0x1);
        }
        public void SetOrphans(byte type, int value)
        {
            uint bits = i.bits[ORPHANS_INDEX];

            /* 1bit: t : type */
            i.bits[ORPHANS_INDEX] = (bits & ~ORPHANS_MASK) | (((uint)type & 0x1) <<
                    ORPHANS_SHIFT);

            i.orphans = value;
        }

        public CssOutlineColorEnum GetOutlineColor(ref Color color)
        {
            uint bits = i.bits[OUTLINE_COLOR_INDEX];
            bits &= OUTLINE_COLOR_MASK;
            bits >>= OUTLINE_COLOR_SHIFT;

            /* 2bits: tt : type */
            var val = (CssOutlineColorEnum)(bits & 0x3);
            if (val == CssOutlineColorEnum.CSS_OUTLINE_COLOR_COLOR)
            {
                color = i.outline_color;
            }

            return val;
        }
        public void SetOutlineColor(CssOutlineColorEnum type, Color color)
        {
            uint bits = i.bits[OUTLINE_COLOR_INDEX];

            /* 2bits: tt : type */
            i.bits[OUTLINE_COLOR_INDEX] = (bits & ~OUTLINE_COLOR_MASK) | (((uint)type & 0x3) <<
                    OUTLINE_COLOR_SHIFT);

            i.outline_color = color;
        }

        public byte GetOutlineStyle()
        {
            uint bits = i.bits[OUTLINE_STYLE_INDEX];
            bits &= OUTLINE_STYLE_MASK;
            bits >>= OUTLINE_STYLE_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetOutlineStyle(byte type)
        {
            uint bits = i.bits[OUTLINE_STYLE_INDEX];

            /* 4bits: tttt : type */
            i.bits[OUTLINE_STYLE_INDEX] = (bits & ~OUTLINE_STYLE_MASK) | (((uint)type & 0xf) <<
                    OUTLINE_STYLE_SHIFT);
        }

        public CssOutlineWidthEnum GetOutlineWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[OUTLINE_WIDTH_INDEX];
            bits &= OUTLINE_WIDTH_MASK;
            bits >>= OUTLINE_WIDTH_SHIFT;

            /* 8bits: uuuuuttt : unit | type */
            var val = (CssOutlineWidthEnum)(bits & 0x7);
            if (val == CssOutlineWidthEnum.CSS_OUTLINE_WIDTH_WIDTH)
            {
                length = i.outline_width;
                unit = (CssUnit)(bits >> 3);
            }

            return val;
        }
        public void SetOutlineWidth(CssOutlineWidthEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[OUTLINE_WIDTH_INDEX];

            /* 8bits: uuuuuttt : unit | type */
            i.bits[OUTLINE_WIDTH_INDEX] = (bits & ~OUTLINE_WIDTH_MASK) | ((((uint)type & 0x7) |
                ((uint)unit << 3)) << OUTLINE_WIDTH_SHIFT);

            i.outline_width = length;
        }

        public byte GetOverflowX()
        {
            uint bits = i.bits[OVERFLOW_X_INDEX];
            bits &= OVERFLOW_X_MASK;
            bits >>= OVERFLOW_X_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetOverflowX(byte type)
        {
            uint bits = i.bits[OVERFLOW_X_INDEX];

            /* 3bits: ttt : type */
            i.bits[OVERFLOW_X_INDEX] = (bits & ~OVERFLOW_X_MASK) | (((uint)type & 0x7) <<
                    OVERFLOW_X_SHIFT);
        }

        public byte GetOverflowY()
        {
            uint bits = i.bits[OVERFLOW_Y_INDEX];
            bits &= OVERFLOW_Y_MASK;
            bits >>= OVERFLOW_Y_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetOverflowY(byte type)
        {
            uint bits = i.bits[OVERFLOW_Y_INDEX];

            /* 3bits: ttt : type */
            i.bits[OVERFLOW_Y_INDEX] = (bits & ~OVERFLOW_Y_MASK) | (((uint)type & 0x7) <<
                    OVERFLOW_Y_SHIFT);
        }

        public CssPaddingEnum GetPaddingBottom(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[PADDING_BOTTOM_INDEX];
            bits &= PADDING_BOTTOM_MASK;
            bits >>= PADDING_BOTTOM_SHIFT;

            /* 6bits: uuuuut : unit | type */
            var val = (CssPaddingEnum)(bits & 0x1);
            if (val == CssPaddingEnum.CSS_PADDING_SET)
            {
                length = i.padding_bottom;
                unit = (CssUnit)(bits >> 1);
            }

            return val;
        }
        public void SetPaddingBottom(CssPaddingEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[PADDING_BOTTOM_INDEX];

            /* 6bits: uuuuut : unit | type */
            i.bits[PADDING_BOTTOM_INDEX] = (bits & ~PADDING_BOTTOM_MASK) | ((((uint)type & 0x1) |
                ((uint)unit << 1)) << PADDING_BOTTOM_SHIFT);

            i.padding_bottom = length;
        }

        public CssPaddingEnum GetPaddingLeft(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[PADDING_LEFT_INDEX];
            bits &= PADDING_LEFT_MASK;
            bits >>= PADDING_LEFT_SHIFT;

            /* 6bits: uuuuut : unit | type */
            var val = (CssPaddingEnum)(bits & 0x1);
            if (val == CssPaddingEnum.CSS_PADDING_SET)
            {
                length = i.padding_bottom;
                unit = (CssUnit)(bits >> 1);
            }

            return val;
        }
        public void SetPaddingLeft(CssPaddingEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[PADDING_LEFT_INDEX];

            /* 6bits: uuuuut : unit | type */
            i.bits[PADDING_LEFT_INDEX] = (bits & ~PADDING_LEFT_MASK) | ((((uint)type & 0x1) |
                ((uint)unit << 1)) << PADDING_LEFT_SHIFT);

            i.padding_left = length;
        }

        public CssPaddingEnum GetPaddingRight(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[PADDING_RIGHT_INDEX];
            bits &= PADDING_RIGHT_MASK;
            bits >>= PADDING_RIGHT_SHIFT;

            /* 6bits: uuuuut : unit | type */
            var val = (CssPaddingEnum)(bits & 0x1);
            if (val == CssPaddingEnum.CSS_PADDING_SET)
            {
                length = i.padding_bottom;
                unit = (CssUnit)(bits >> 1);
            }

            return val;
        }
        public void SetPaddingRight(CssPaddingEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[PADDING_RIGHT_INDEX];

            /* 6bits: uuuuut : unit | type */
            i.bits[PADDING_RIGHT_INDEX] = (bits & ~PADDING_RIGHT_MASK) | ((((uint)type & 0x1) |
                ((uint)unit << 1)) << PADDING_RIGHT_SHIFT);

            i.padding_right = length;
        }

        public CssPaddingEnum GetPaddingTop(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[PADDING_TOP_INDEX];
            bits &= PADDING_TOP_MASK;
            bits >>= PADDING_TOP_SHIFT;

            /* 6bits: uuuuut : unit | type */
            var val = (CssPaddingEnum)(bits & 0x1);
            if (val == CssPaddingEnum.CSS_PADDING_SET)
            {
                length = i.padding_bottom;
                unit = (CssUnit)(bits >> 1);
            }

            return val;
        }
        public void SetPaddingTop(CssPaddingEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[PADDING_TOP_INDEX];

            /* 6bits: uuuuut : unit | type */
            i.bits[PADDING_TOP_INDEX] = (bits & ~PADDING_TOP_MASK) | ((((uint)type & 0x1) |
                ((uint)unit << 1)) << PADDING_TOP_SHIFT);

            i.padding_top = length;
        }

        public byte GetPageBreakAfter()
        {
            uint bits = i.bits[PAGE_BREAK_AFTER_INDEX];
            bits &= PAGE_BREAK_AFTER_MASK;
            bits >>= PAGE_BREAK_AFTER_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetPageBreakAfter(byte type)
        {
            uint bits = i.bits[PAGE_BREAK_AFTER_INDEX];

            /* 3bits: ttt : type */
            i.bits[PAGE_BREAK_AFTER_INDEX] = (bits & ~PAGE_BREAK_AFTER_MASK) |
                (((uint)type & 0x7) << PAGE_BREAK_AFTER_SHIFT);
        }

        public byte GetPageBreakBefore()
        {
            uint bits = i.bits[PAGE_BREAK_BEFORE_INDEX];
            bits &= PAGE_BREAK_BEFORE_MASK;
            bits >>= PAGE_BREAK_BEFORE_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetPageBreakBefore(byte type)
        {
            uint bits = i.bits[PAGE_BREAK_BEFORE_INDEX];

            /* 3bits: ttt : type */
            i.bits[PAGE_BREAK_BEFORE_INDEX] = (bits & ~PAGE_BREAK_BEFORE_MASK) |
                (((uint)type & 0x7) << PAGE_BREAK_BEFORE_SHIFT);
        }

        public byte GetPageBreakInside()
        {
            uint bits = i.bits[PAGE_BREAK_INSIDE_INDEX];
            bits &= PAGE_BREAK_INSIDE_MASK;
            bits >>= PAGE_BREAK_INSIDE_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetPageBreakInside(byte type)
        {
            uint bits = i.bits[PAGE_BREAK_INSIDE_INDEX];

            /* 2bits: tt : type */
            i.bits[PAGE_BREAK_INSIDE_INDEX] = (bits & ~PAGE_BREAK_INSIDE_MASK) | (((uint)type & 0x3) <<
                    PAGE_BREAK_INSIDE_SHIFT);
        }

        // autogenerated_props:1765
        byte GetPosition()
        {
            uint bits = i.bits[POSITION_INDEX];
            bits &= POSITION_MASK;
            bits >>= POSITION_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }
        public void SetPosition(byte type)
        {
            uint bits = i.bits[POSITION_INDEX];

            /* 3bits: ttt : type */
            i.bits[POSITION_INDEX] = (bits & ~POSITION_MASK) | (((uint)type & 0x7) << POSITION_SHIFT);
        }

        public byte GetQuotes(ref string[] stringArr)
        {
            uint bits = i.bits[QUOTES_INDEX];
            bits &= QUOTES_MASK;
            bits >>= QUOTES_SHIFT;

            /* 1bit: t : type */
            stringArr = Quotes;

            return (byte)(bits & 0x1);
        }
        public void SetQuotes(byte type, string[] stringArr)
        {
            uint bits = i.bits[QUOTES_INDEX];
            Log.Unimplemented();
        }

        public CssRightEnum GetRight(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[RIGHT_INDEX];
            bits &= RIGHT_MASK;
            bits >>= RIGHT_SHIFT;

            /* 7bits: uuuuutt : units | type */
            var val = (CssRightEnum)(bits & 0x3);
            if (val == CssRightEnum.CSS_RIGHT_SET)
            {
                length = i.right;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetRight(CssRightEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[RIGHT_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[RIGHT_INDEX] = (bits & ~RIGHT_MASK) | ((((uint)type & 0x3) | ((uint)unit << 2))
                    << RIGHT_SHIFT);

            i.right = length;
        }

        public byte GetRightBits()
        {
            uint bits = i.bits[RIGHT_INDEX];
            bits &= RIGHT_MASK;
            bits >>= RIGHT_SHIFT;
            return (byte)bits;
        }

        public byte GetTableLayout()
        {
            uint bits = i.bits[TABLE_LAYOUT_INDEX];
            bits &= TABLE_LAYOUT_MASK;
            bits >>= TABLE_LAYOUT_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetTableLayout(byte type)
        {
            uint bits = i.bits[TABLE_LAYOUT_INDEX];

            /* 2bits: tt : type */
            i.bits[TABLE_LAYOUT_INDEX] = (bits & ~TABLE_LAYOUT_MASK) | (((uint)type & 0x3) <<
                    TABLE_LAYOUT_SHIFT);
        }

        public byte GetTextAlign()
        {
            uint bits = i.bits[TEXT_ALIGN_INDEX];
            bits &= TEXT_ALIGN_MASK;
            bits >>= TEXT_ALIGN_SHIFT;

            /* 4bits: tttt : type */

            return (byte)(bits & 0xf);
        }
        public void SetTextAlign(byte type)
        {
            uint bits = i.bits[TEXT_ALIGN_INDEX];

            /* 4bits: tttt : type */
            i.bits[TEXT_ALIGN_INDEX] = (bits & ~TEXT_ALIGN_MASK) | (((uint)type & 0xf) <<
                    TEXT_ALIGN_SHIFT);
        }

        public byte GetTextDecoration()
        {
            uint bits = i.bits[TEXT_DECORATION_INDEX];
            bits &= TEXT_DECORATION_MASK;
            bits >>= TEXT_DECORATION_SHIFT;

            /* 5bits: ttttt : type */

            return (byte)(bits & 0x1f);
        }
        public void SetTextDecoration(byte type)
        {
            uint bits = i.bits[TEXT_DECORATION_INDEX];

            /* 5bits: ttttt : type */
            i.bits[TEXT_DECORATION_INDEX] = (bits & ~TEXT_DECORATION_MASK) |
                (((uint)type & 0x1f) << TEXT_DECORATION_SHIFT);
        }

        public CssTextIndentEnum GetTextIndent(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[TEXT_INDENT_INDEX];
            bits &= TEXT_INDENT_MASK;
            bits >>= TEXT_INDENT_SHIFT;

            /* 6bits: uuuuut : unit | type */
            var val = (CssTextIndentEnum)(bits & 0x1);
            if (val == CssTextIndentEnum.CSS_TEXT_INDENT_SET)
            {
                length = i.text_indent;
                unit = (CssUnit)(bits >> 1);
            }

            return val;
        }
        public void SetTextIndent(CssTextIndentEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[TEXT_INDENT_INDEX];

            /* 6bits: uuuuut : unit | type */
            i.bits[TEXT_INDENT_INDEX] = (bits & ~TEXT_INDENT_MASK) | ((((uint)type & 0x1) |
                ((uint)unit << 1)) << TEXT_INDENT_SHIFT);

            i.text_indent = length;
        }

        // autogenerated_propget.h:1908
        public CssTextTransformEnum GetTextTransform()
        {
            uint bits = i.bits[TEXT_TRANSFORM_INDEX];
            bits &= TEXT_TRANSFORM_MASK;
            bits >>= TEXT_TRANSFORM_SHIFT;

            /* 3bits: ttt : type */

            return (CssTextTransformEnum)(bits & 0x7);
        }

        public void SetTextTransform(CssTextTransformEnum type)
        {
            uint bits = i.bits[TEXT_TRANSFORM_INDEX];

            /* 3bits: ttt : type */
            i.bits[TEXT_TRANSFORM_INDEX] = (bits & ~TEXT_TRANSFORM_MASK) | (((uint)type & 0x7) << TEXT_TRANSFORM_SHIFT);
        }

        public CssTopEnum GetTop(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[TOP_INDEX];
            bits &= TOP_MASK;
            bits >>= TOP_SHIFT;

            /* 7bits: uuuuutt : units | type */
            var val = (CssTopEnum)(bits & 0x3);
            if (val == CssTopEnum.CSS_TOP_SET)
            {
                length = i.top;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetTop(CssTopEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[TOP_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[TOP_INDEX] = (bits & ~TOP_MASK) | ((((uint)type & 0x3) | ((uint)unit << 2))
                    << TOP_SHIFT);

            i.top = length;
        }
        public byte GetTopBits()
        {
            uint bits = i.bits[TOP_INDEX];
            bits &= TOP_MASK;
            bits >>= TOP_SHIFT;
            return (byte)bits;
        }

        public byte GetUnicodeBidi()
        {
            uint bits = i.bits[UNICODE_BIDI_INDEX];
            bits &= UNICODE_BIDI_MASK;
            bits >>= UNICODE_BIDI_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetUnicodeBidi(byte type)
        {
            uint bits = i.bits[UNICODE_BIDI_INDEX];

            /* 2bits: tt : type */
            i.bits[UNICODE_BIDI_INDEX] = (bits & ~UNICODE_BIDI_MASK) | (((uint)type & 0x3) <<
                    UNICODE_BIDI_SHIFT);
        }

        public CssVerticalAlignEnum GetVerticalAlign(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[VERTICAL_ALIGN_INDEX];
            bits &= VERTICAL_ALIGN_MASK;
            bits >>= VERTICAL_ALIGN_SHIFT;

            /* 9bits: uuuuutttt : unit | type */
            var val = (CssVerticalAlignEnum)(bits & 0xf);
            if (val == CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_SET)
            {
                length = i.vertical_align;
                unit = (CssUnit)(bits >> 4);
            }

            return val;
        }
        public void SetVerticalAlign(CssVerticalAlignEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[VERTICAL_ALIGN_INDEX];

            /* 9bits: uuuuutttt : unit | type */
            i.bits[VERTICAL_ALIGN_INDEX] = (bits & ~VERTICAL_ALIGN_MASK) | ((((uint)type & 0xf) |
                ((uint)unit << 4)) << VERTICAL_ALIGN_SHIFT);

            i.vertical_align = length;
        }

        public byte GetVisibility()
        {
            uint bits = i.bits[VISIBILITY_INDEX];
            bits &= VISIBILITY_MASK;
            bits >>= VISIBILITY_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetVisibility(byte type)
        {
            uint bits = i.bits[VISIBILITY_INDEX];

            /* 2bits: tt : type */
            i.bits[VISIBILITY_INDEX] = (bits & ~VISIBILITY_MASK) | (((uint)type & 0x3) <<
                    VISIBILITY_SHIFT);
        }

        // autogenerated_propget.h:2013
        public CssWhiteSpaceEnum GetWhitespace()
        {
            uint bits = i.bits[WHITE_SPACE_INDEX];
            bits &= WHITE_SPACE_MASK;
            bits >>= WHITE_SPACE_SHIFT;

            /* 3bits: ttt : type */

            return (CssWhiteSpaceEnum)(bits & 0x7);
        }

        // autogenerated_propset.h:2384

        public void SetWhitespace(CssWhiteSpaceEnum type)
        {
            uint bits = i.bits[WHITE_SPACE_INDEX];

            /* 3bits: ttt : type */
            i.bits[WHITE_SPACE_INDEX] = (bits & ~WHITE_SPACE_MASK) | (((uint)type & 0x7) << WHITE_SPACE_SHIFT);
        }

        public byte GetWidows(ref int value)
        {
            uint bits = i.bits[WIDOWS_INDEX];
            bits &= WIDOWS_MASK;
            bits >>= WIDOWS_SHIFT;

            /* 1bit: t : type */
            value = i.widows;

            return (byte)(bits & 0x1);
        }
        public void SetWidows(byte type, int value)
        {
            uint bits = i.bits[WIDOWS_INDEX];

            /* 1bit: t : type */
            i.bits[WIDOWS_INDEX] = (bits & ~WIDOWS_MASK) | (((uint)type & 0x1) <<
                    WIDOWS_SHIFT);

            i.widows = value;
        }

        // autogenerated_propget.h:2051
        public CssWidth GetWidth(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[WIDTH_INDEX];
            bits &= WIDTH_MASK;
            bits >>= WIDTH_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            byte width = (byte)(bits & 0x3);
            if (width == (byte)CssWidth.CSS_WIDTH_SET)
            {
                length = i.width;
                unit = (CssUnit)(bits >> 2);
            }

            return (CssWidth)width;
        }
        // autogenerated_propset.h:2429
        public void SetWidth(CssWidth type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[WIDTH_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[WHITE_SPACE_INDEX] = (bits & ~WIDTH_MASK) | ((((uint)type & 0x3) | ((uint)unit << 2)) << WIDTH_SHIFT);

            i.width = length;
        }
        public CssWordSpacingEnum GetWordSpacing(ref Fixed length, ref CssUnit unit)
        {
            uint bits = i.bits[WORD_SPACING_INDEX];
            bits &= WORD_SPACING_MASK;
            bits >>= WORD_SPACING_SHIFT;

            /* 7bits: uuuuutt : unit | type */
            var val = (CssWordSpacingEnum)(bits & 0x3);
            if (val == CssWordSpacingEnum.CSS_WORD_SPACING_SET)
            {
                length = i.word_spacing;
                unit = (CssUnit)(bits >> 2);
            }

            return val;
        }
        public void SetWordSpacing(CssWordSpacingEnum type, Fixed length, CssUnit unit)
        {
            uint bits = i.bits[WORD_SPACING_INDEX];

            /* 7bits: uuuuutt : unit | type */
            i.bits[WORD_SPACING_INDEX] = (bits & ~WORD_SPACING_MASK) | ((((uint)type & 0x3) |
                ((uint)unit << 2)) << WORD_SPACING_SHIFT);

            i.word_spacing = length;
        }

        public byte GetWritingMode()
        {
            uint bits = i.bits[WRITING_MODE_INDEX];
            bits &= WRITING_MODE_MASK;
            bits >>= WRITING_MODE_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }
        public void SetWritingMode(byte type)
        {
            uint bits = i.bits[WRITING_MODE_INDEX];

            /* 2bits: tt : type */
            i.bits[WRITING_MODE_INDEX] = (bits & ~WRITING_MODE_MASK) | (((uint)type & 0x3) <<
                    WRITING_MODE_SHIFT);
        }
        public byte GetZIndex(ref int value)
        {
            uint bits = i.bits[Z_INDEX_INDEX];
            bits &= Z_INDEX_MASK;
            bits >>= Z_INDEX_SHIFT;

            /* 2bits: tt : type */
            value = i.z_index;

            return (byte)(bits & 0x3);
        }
        public void SetZIndex(byte type, int value)
        {
            uint bits = i.bits[Z_INDEX_INDEX];

            /* 2bits: tt : type */
            i.bits[Z_INDEX_INDEX] = (bits & ~Z_INDEX_MASK) | (((uint)type & 0x3) <<
                    Z_INDEX_SHIFT);

            i.z_index = value;
        }

        // --------------------------------------------------------------------

        // computed.c:1094
        public void ComputeAbsoluteValues(ComputedStyle parent, CssUnitCtx unitCtx)
        {
            CssHint psize = null, size = new CssHint(), ex_size = new CssHint();
            //css_error error;

            size.Status = (byte)GetFontSize(ref size.Length.Value, ref size.Length.Unit);

            // Get reference font-size for relative sizes.
            if (parent != null)
            {
                psize = new CssHint();
                psize.Status = (byte)parent.GetFontSize(ref psize.Length.Value, ref psize.Length.Unit);
                if (psize.Status != (byte)CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION)
                {
                    throw new Exception("Bad Param");
                    //return CSS_BADPARM;
                }
                //refLength = &psize.Length;
                ComputeAbsoluteFontSize(
                        psize.Length,
                        unitCtx.RootStyle,
                        unitCtx.FontSizeDefault,
                        size);
            }
            else
            {
                ComputeAbsoluteFontSize(
                        unitCtx.RootStyle,
                        unitCtx.FontSizeDefault,
                        size);
            }

            SetFontSize((CssFontSizeEnum)size.Status, size.Length.Value, size.Length.Unit);

            // Compute the size of an ex unit
            ex_size.Status = (byte)CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION;
            ex_size.Length.Value = new Fixed(1);
            ex_size.Length.Unit = CssUnit.CSS_UNIT_EX;

            ComputeAbsoluteFontSize(
                    size.Length,
                    unitCtx.RootStyle,
                    unitCtx.FontSizeDefault,
                    ex_size);

            // Convert ex size into ems
            if (size.Length.Value.RawValue != 0)
                ex_size.Length.Value = ex_size.Length.Value / size.Length.Value;
            else
                ex_size.Length.Value.RawValue = 0;

            ex_size.Length.Unit = CssUnit.CSS_UNIT_EM;

            // Fix up background-position
            ComputeAbsoluteLengthPair(ex_size.Length, "background_position");
            /*
                        // Fix up background-color
                        compute_absolute_color(style,
                                get_background_color,
                                set_background_color);

                        // Fix up border-{top,right,bottom,left}-color
                        compute_border_colors(style);

                        // Fix up border-{top,right,bottom,left}-width
                        compute_absolute_border_width(style, &ex_size.data.length);

                        // Fix up sides
                        compute_absolute_sides(style, &ex_size.data.length);

                        // Fix up height
                        compute_absolute_length(style, &ex_size.data.length,
                                get_height, set_height);

                        // Fix up line-height (must be before vertical-align)
                        compute_absolute_line_height(style, &ex_size.data.length);

                        // Fix up margins
                        compute_absolute_margins(style, &ex_size.data.length);

                        // Fix up max-height
                        compute_absolute_length(style, &ex_size.data.length,
                                get_max_height, set_max_height);

                        // Fix up max-width
                        compute_absolute_length(style, &ex_size.data.length,
                                get_max_width, set_max_width);

                        // Fix up min-height
                        compute_absolute_length(style, &ex_size.data.length,
                                get_min_height, set_min_height);

                        // Fix up min-width
                        compute_absolute_length(style, &ex_size.data.length,
                                get_min_width, set_min_width);

                        // Fix up padding
                        compute_absolute_padding(style, &ex_size.data.length);

                        // Fix up text-indent
                        compute_absolute_length(style, &ex_size.data.length,
                                get_text_indent, set_text_indent);

                        // Fix up vertical-align
                        compute_absolute_vertical_align(style, &ex_size.data.length);

                        // Fix up width
                        compute_absolute_length(style, &ex_size.data.length,
                                get_width, set_width);

                        // Fix up flex-basis
                        compute_absolute_length(style, &ex_size.data.length,
                                get_flex_basis, set_flex_basis);
            */
            // Fix up border-spacing
            ComputeAbsoluteLengthPair(ex_size.Length, "border_spacing");
            /*
                        // Fix up clip
                        compute_absolute_clip(style, &ex_size.data.length);

                        // Fix up letter-spacing
                        compute_absolute_length(style,
                                &ex_size.data.length,
                                get_letter_spacing,
                                set_letter_spacing);

                        // Fix up outline-color
                        compute_absolute_color(style,
                                get_outline_color,
                                set_outline_color);

                        // Fix up outline-width
                        compute_absolute_border_side_width(style,
                                &ex_size.data.length,
                                get_outline_width,
                            set_outline_width);

                        // Fix up word-spacing
                        compute_absolute_length(style,
                                &ex_size.data.length,
                                get_word_spacing,
                                set_word_spacing);

                        // Fix up column-rule-width
                        compute_absolute_border_side_width(style,
                                &ex_size.data.length,
                                get_column_rule_width,
                                set_column_rule_width);

                        // Fix up column-width
                        compute_absolute_length(style,
                                &ex_size.data.length,
                                get_column_width,
                                set_column_width);

                        // Fix up column-gap
                        compute_absolute_length(style,
                                &ex_size.data.length,
                                get_column_gap,
                                set_column_gap);
            */
            Log.Unimplemented("majority of properties fixups");
        }

        //FIXME: Maybe should be in Unit
        // unit.c:403
        void ComputeAbsoluteFontSize(ComputedStyle rootStyle, Fixed fontSizeDefault, CssHint size)
        {
            var refLen = new CssHintLength(fontSizeDefault, CssUnit.CSS_UNIT_PX);
            ComputeAbsoluteFontSize(refLen, rootStyle, fontSizeDefault, size);
        }
        void ComputeAbsoluteFontSize(CssHintLength refLen, ComputedStyle rootStyle, Fixed fontSizeDefault, CssHint size)
        {
            // refLen must be absolute
            Debug.Assert(refLen.Unit != CssUnit.CSS_UNIT_EM);
            Debug.Assert(refLen.Unit != CssUnit.CSS_UNIT_EX);
            Debug.Assert(refLen.Unit != CssUnit.CSS_UNIT_PCT);

            Debug.Assert(size.Status != (byte)CssFontSizeEnum.CSS_FONT_SIZE_INHERIT);

            switch ((CssFontSizeEnum)size.Status)
            {
                case CssFontSizeEnum.CSS_FONT_SIZE_XX_SMALL: // Fall-through.
                    goto case CssFontSizeEnum.CSS_FONT_SIZE_XX_LARGE;
                case CssFontSizeEnum.CSS_FONT_SIZE_X_SMALL:  // Fall-through.
                    goto case CssFontSizeEnum.CSS_FONT_SIZE_XX_LARGE;
                case CssFontSizeEnum.CSS_FONT_SIZE_SMALL:    // Fall-through.
                    goto case CssFontSizeEnum.CSS_FONT_SIZE_XX_LARGE;
                case CssFontSizeEnum.CSS_FONT_SIZE_MEDIUM:   // Fall-through.
                    goto case CssFontSizeEnum.CSS_FONT_SIZE_XX_LARGE;
                case CssFontSizeEnum.CSS_FONT_SIZE_LARGE:    // Fall-through.
                    goto case CssFontSizeEnum.CSS_FONT_SIZE_XX_LARGE;
                case CssFontSizeEnum.CSS_FONT_SIZE_X_LARGE:  // Fall-through.
                    goto case CssFontSizeEnum.CSS_FONT_SIZE_XX_LARGE;
                case CssFontSizeEnum.CSS_FONT_SIZE_XX_LARGE:
                    {
                        Fixed[] factors = {
                            new Fixed(0.5625),
                            new Fixed(0.6250),
                            new Fixed(0.8125),
                            new Fixed(1.0000),
                            new Fixed(1.1250),
                            new Fixed(1.5000),
                            new Fixed(2.0000)
                        };
                        Debug.Assert((byte)CssFontSizeEnum.CSS_FONT_SIZE_INHERIT == 0);
                        Debug.Assert((byte)CssFontSizeEnum.CSS_FONT_SIZE_XX_SMALL == 1);

                        size.Length.Value = factors[size.Status - 1] * fontSizeDefault;
                        size.Length.Unit = CssUnit.CSS_UNIT_PX;
                        size.Status = (byte)CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION;
                        break;
                    }

                case CssFontSizeEnum.CSS_FONT_SIZE_LARGER:
                    size.Length.Value = refLen.Value * new Fixed(1.2);
                    size.Length.Unit = refLen.Unit;
                    size.Status = (byte)CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION;
                    break;

                case CssFontSizeEnum.CSS_FONT_SIZE_SMALLER:
                    size.Length.Value = refLen.Value / new Fixed(1.2);
                    size.Length.Unit = refLen.Unit;
                    size.Status = (byte)CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION;
                    break;

                case CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION:
                    // Convert any relative units to absolute.
                    switch (size.Length.Unit)
                    {
                        case CssUnit.CSS_UNIT_PCT:
                            size.Length.Value = (size.Length.Value * refLen.Value) / new Fixed(100);
                            size.Length.Unit = refLen.Unit;
                            break;

                        case CssUnit.CSS_UNIT_EM: // Fall-through
                            goto case CssUnit.CSS_UNIT_CH;
                        case CssUnit.CSS_UNIT_EX: // Fall-through
                            goto case CssUnit.CSS_UNIT_CH;
                        case CssUnit.CSS_UNIT_CH:
                            // Parent relative units.
                            size.Length.Value = size.Length.Value * refLen.Value;

                            switch (size.Length.Unit)
                            {
                                case CssUnit.CSS_UNIT_EX:
                                    size.Length.Value = size.Length.Value * new Fixed(0.6);
                                    break;

                                case CssUnit.CSS_UNIT_CH:
                                    size.Length.Value = size.Length.Value * new Fixed(0.4);
                                    break;

                                default:
                                    break;
                            }
                            size.Length.Unit = refLen.Unit;
                            break;

                        case CssUnit.CSS_UNIT_REM:
                            {
                                // Root element relative units.
                                //refLen = CssUnitgGetFontSize(rootStyle, fontSizeDefault); // unit.c:374
                                //var fs = new CssHintLength(fontSizeDefault, CssUnit.CSS_UNIT_PX);
                                refLen.Value = fontSizeDefault;
                                refLen.Unit = CssUnit.CSS_UNIT_PX;
                                var fstatus = rootStyle.GetFontSize(ref refLen.Value, ref refLen.Unit);

                                size.Length.Unit = refLen.Unit;
                                size.Length.Value = size.Length.Value * refLen.Value;
                                break;
                            }
                        default:
                            break;
                    }
                    //goto default;
                    break;

                default:
                    break;
            }
        }

        // --------------------------------------------------------------------
        // --------------------------------------------------------------------
        // --------------------------------------------------------------------
        #region Property Accessors

        public CssLetterSpacingEnum ComputedLetterSpacing(ref Fixed length, ref CssUnit unit)
        {
            return GetLetterSpacing(ref length, ref unit);
        }

        public CssOutlineColorEnum ComputedOutlineColor(ref Color color)
        {
            return GetOutlineColor(ref color);
        }

        public CssOutlineWidthEnum ComputedOutlineWidth(ref Fixed length, ref CssUnit unit)
        {
            /* This property is in the uncommon block, so we need to handle
	         * absolute value calculation for initial value (medium) here. */
            if (GetOutlineWidth(ref length, ref unit) == CssOutlineWidthEnum.CSS_OUTLINE_WIDTH_MEDIUM)
            {
                length = new Fixed(2);
                unit = CssUnit.CSS_UNIT_PX;
            }

            return CssOutlineWidthEnum.CSS_OUTLINE_WIDTH_WIDTH;
        }

        public CssBorderSpacingEnum ComputedBorderSpacing(ref Fixed hlength, ref CssUnit hunit,
                ref Fixed vlength, ref CssUnit vunit)
        {
            return GetBorderSpacing(ref hlength, ref hunit, ref vlength, ref vunit);
        }

        public CssWordSpacingEnum ComputedWordSpacing(ref Fixed length, ref CssUnit unit)
        {
            return GetWordSpacing(ref length, ref unit);
        }

        public byte ComputedWritingMode()
        {
            return GetWritingMode();
        }

        public byte ComputedCounterIncrement(ref CssComputedCounter[] counters)
        {
            return GetCounterIncrement(ref counters);
        }

        public byte ComputedCounterReset(ref CssComputedCounter[] counters)
        {
            return GetCounterReset(ref counters);
        }

        public byte ComputedCursor(string[] urls)
        {
            return GetCursor(urls);
        }

        public CssClipEnum ComputedClip(ref CssComputedClipRect rect)
        {
            return GetClip(ref rect);
        }

        // computed.c:358
        public CssContent ComputedContent(ref CssComputedContentItem[] content)
        {
            return (CssContent)GetContent(ref content);
        }

        public CssVerticalAlignEnum ComputedVerticalAlign(ref Fixed length, ref CssUnit unit)
        {
            return GetVerticalAlign(ref length, ref unit);
        }

        // computed.c:372
        public CssFontSizeEnum ComputedFontSize(ref Fixed length, ref CssUnit unit)
        {
            return (CssFontSizeEnum)GetFontSize(ref length, ref unit);
        }
        public CssBorderWidthEnum ComputedBorderTopWidth(
                ref Fixed length, ref CssUnit unit)
        {
            return GetBorderTopWidth(ref length, ref unit);
        }

        public CssBorderWidthEnum ComputedBorderRightWidth(
                ref Fixed length, ref CssUnit unit)
        {
            return GetBorderRightWidth(ref length, ref unit);
        }

        public CssBorderWidthEnum ComputedBorderBottomWidth(
                ref Fixed length, ref CssUnit unit)
        {
            return GetBorderBottomWidth(ref length, ref unit);
        }

        public CssBorderWidthEnum ComputedBorderLeftWidth(
                ref Fixed length, ref CssUnit unit)
        {
            return GetBorderLeftWidth(ref length, ref unit);
        }

        public CssBackgroundImageEnum ComputedBackground_image(out string url)
        {
            return GetBackgroundImage(out url);
        }

        // computed.c:406
        public CssColorEnum ComputedColor(out Color color)
        {
            return (CssColorEnum)GetColor(out color);
        }

        public byte ComputedListStyle_image(ref string url)
        {
            return GetListStyleImage(ref url);
        }

        public byte Computed_quotes(string[] quotes)
        {
            return GetQuotes(ref quotes);
        }

        public CssTopEnum ComputedTop(ref Fixed length, ref CssUnit unit)
        {
            var position = ComputedPosition();
            var top = GetTop(ref length, ref unit);

            /* Fix up, based on computed position */
            if (position == CssPosition.CSS_POSITION_STATIC)
            {
                /* Static -> auto */
                top = CssTopEnum.CSS_TOP_AUTO;
            }
            else if (position == CssPosition.CSS_POSITION_RELATIVE)
            {
                /* Relative -> follow $9.4.3 */
                byte bottom = GetBottomBits();

                if (top == CssTopEnum.CSS_TOP_AUTO &&
                    (bottom & 0x3) == (byte)CssBottomEnum.CSS_BOTTOM_AUTO)
                {
                    /* Both auto => 0px */
                    length = Fixed.F_0;
                    unit = CssUnit.CSS_UNIT_PX;
                }
                else if (top == CssTopEnum.CSS_TOP_AUTO)
                {
                    /* Top is auto => -bottom */
                    length = -i.bottom;
                    unit = (CssUnit)(bottom >> 2);
                }

                top = CssTopEnum.CSS_TOP_SET;
            }

            return top;
        }

        public CssRightEnum ComputedRight(ref Fixed length, ref CssUnit unit)
        {
            var position = ComputedPosition();
            var right = GetRight(ref length, ref unit);

            /* Fix up, based on computed position */
            if (position == CssPosition.CSS_POSITION_STATIC)
            {
                /* Static -> auto */
                right = CssRightEnum.CSS_RIGHT_AUTO;
            }
            else if (position == CssPosition.CSS_POSITION_RELATIVE)
            {
                /* Relative -> follow $9.4.3 */
                byte left = GetLeftBits();

                if (right == CssRightEnum.CSS_RIGHT_AUTO && (left & 0x3) == (byte)CssLeftEnum.CSS_LEFT_AUTO)
                {
                    /* Both auto => 0px */
                    length = Fixed.F_0;
                    unit = CssUnit.CSS_UNIT_PX;
                }
                else if (right == CssRightEnum.CSS_RIGHT_AUTO)
                {
                    /* Right is auto => -left */
                    length = -i.left;
                    unit = (CssUnit)(left >> 2);
                }
                else
                {
                    /** \todo Consider containing block's direction
                     * if overconstrained */
                }

                right = CssRightEnum.CSS_RIGHT_SET;
            }

            return right;
        }

        public CssBottomEnum ComputedBottom(
                ref Fixed length, ref CssUnit unit)
        {
            var position = ComputedPosition();
            var bottom = GetBottom(ref length, ref unit);

            /* Fix up, based on computed position */
            if (position == CssPosition.CSS_POSITION_STATIC)
            {
                /* Static -> auto */
                bottom = CssBottomEnum.CSS_BOTTOM_AUTO;
            }
            else if (position == CssPosition.CSS_POSITION_RELATIVE)
            {
                /* Relative -> follow $9.4.3 */
                byte top = GetTopBits();

                if (bottom == CssBottomEnum.CSS_BOTTOM_AUTO && (top & 0x3) == (byte)CssTopEnum.CSS_TOP_AUTO)
                {
                    /* Both auto => 0px */
                    length = Fixed.F_0;
                    unit = CssUnit.CSS_UNIT_PX;
                }
                else if (bottom == CssBottomEnum.CSS_BOTTOM_AUTO ||
                        (top & 0x3) != (byte)CssTopEnum.CSS_TOP_AUTO)
                {
                    /* Bottom is auto or top is not auto => -top */
                    length = -i.top;
                    unit = (CssUnit)(top >> 2);
                }

                bottom = CssBottomEnum.CSS_BOTTOM_SET;
            }

            return bottom;
        }

        public CssLeftEnum ComputedLeft(ref Fixed length, ref CssUnit unit)
        {
            var position = ComputedPosition();
            var left = GetLeft(ref length, ref unit);

            /* Fix up, based on computed position */
            if (position == CssPosition.CSS_POSITION_STATIC)
            {
                /* Static -> auto */
                left = CssLeftEnum.CSS_LEFT_AUTO;
            }
            else if (position == CssPosition.CSS_POSITION_RELATIVE)
            {
                /* Relative -> follow $9.4.3 */
                byte right = GetRightBits();

                if (left == CssLeftEnum.CSS_LEFT_AUTO && (right & 0x3) == (byte)CssRightEnum.CSS_RIGHT_AUTO)
                {
                    /* Both auto => 0px */
                    length = Fixed.F_0;
                    unit = CssUnit.CSS_UNIT_PX;
                }
                else if (left == CssLeftEnum.CSS_LEFT_AUTO)
                {
                    /* Left is auto => -right */
                    length = -i.right;
                    unit = (CssUnit)(right >> 2);
                }
                else
                {
                    /** \todo Consider containing block's direction
                     * if overconstrained */
                }

                left = CssLeftEnum.CSS_LEFT_SET;
            }

            return left;
        }

        public byte ComputedBorderTopColor(ref Color color)
        {
            return GetBorderTopColor(ref color);
        }

        public byte ComputedBorderRightColor(ref Color color)
        {
            return GetBorderRightColor(ref color);
        }

        public byte ComputedBorderBottomColor(ref Color color)
        {
            return GetBorderBottomColor(ref color);
        }

        public byte ComputedBorderLeftColor(ref Color color)
        {
            return GetBorderLeftColor(ref color);
        }

        public byte ComputedBoxSizing()
        {
            return GetBoxSizing();
        }

        public CssHeightEnum Computed_height(
                ref Fixed length, ref CssUnit unit)
        {
            return GetHeight(ref length, ref unit);
        }

        public CssLineHeightEnum ComputedLine_height(
                ref Fixed length, ref CssUnit unit)
        {
            return GetLineHeight(ref length, ref unit);
        }

        // computed.c:592
        public CssBackgroundColorEnum ComputedBackgroundColor(out Color color)
        {
            return (CssBackgroundColorEnum)GetBackgroundColor(out color);
        }

        public byte Computed_z_index(
                ref int z_index)
        {
            return GetZIndex(ref z_index);
        }

        public CssMarginEnum ComputedMarginTop(
                ref Fixed length, ref CssUnit unit)
        {
            return GetMarginTop(ref length, ref unit);
        }

        public CssMarginEnum ComputedMarginRight(
                ref Fixed length, ref CssUnit unit)
        {
            return GetMarginRight(ref length, ref unit);
        }

        public CssMarginEnum ComputedMarginBottom(
                ref Fixed length, ref CssUnit unit)
        {
            return GetMarginBottom(ref length, ref unit);
        }

        public CssMarginEnum ComputedMarginLeft(
                ref Fixed length, ref CssUnit unit)
        {
            return GetMarginLeft(ref length, ref unit);
        }

        public CssBackgroundAttachment ComputedBackgroundAttachment()
        {
            return GetBackgroundAttachment();
        }

        public byte ComputedBorderCollapse()
        {
            return GetBorderCollapse();
        }

        public byte ComputedCaptionSide()
        {
            return GetCaptionSide();
        }

        public byte Computed_direction()
        {
            return GetDirection();
        }

        public CssMaxHeightEnum ComputedMaxHeight(ref Fixed length, ref CssUnit unit)
        {
            return GetMaxHeight(ref length, ref unit);
        }

        public CssMaxWidthEnum Computed_maxWidth(ref Fixed length, ref CssUnit unit)
        {
            return GetMaxWidth(ref length, ref unit);
        }
        // computed.c:660
        public CssWidth ComputedWidth(ref Fixed length, ref CssUnit unit)
        {
            return (CssWidth)GetWidth(ref length, ref unit);
        }

        public byte ComputedEmptyCells()
        {
            return GetEmptyCells();
        }

        // computed.c:671
        public CssFloat ComputedFloat()
        {
            var position = ComputedPosition();
            var value = (CssFloat)GetFloat();

            // Fix up as per $9.7:2
            if (position == CssPosition.CSS_POSITION_ABSOLUTE ||
                    position == CssPosition.CSS_POSITION_FIXED)
                return CssFloat.CSS_FLOAT_NONE;

            return value;
        }

        public byte ComputedFontStyle()
        {
            return GetFontStyle();
        }

        public CssMinHeightEnum ComputedMinHeight(ref Fixed length, ref CssUnit unit)
        {
            var min_height = GetMinHeight(ref length, ref unit);

            if (min_height == CssMinHeightEnum.CSS_MIN_HEIGHT_AUTO)
            {
                var display = GetDisplay();

                if (display != CssDisplay.CSS_DISPLAY_FLEX &&
                    display != CssDisplay.CSS_DISPLAY_INLINE_FLEX)
                {
                    min_height = CssMinHeightEnum.CSS_MIN_HEIGHT_SET;
                    length = Fixed.F_0;
                    unit = CssUnit.CSS_UNIT_PX;
                }
            }

            return min_height;
        }

        public CssMinWidthEnum ComputedMinWidth(
                ref Fixed length, ref CssUnit unit)
        {
            var min_width = GetMinWidth(ref length, ref unit);

            if (min_width == CssMinWidthEnum.CSS_MIN_WIDTH_AUTO)
            {
                var display = GetDisplay();

                if (display != CssDisplay.CSS_DISPLAY_FLEX &&
                    display != CssDisplay.CSS_DISPLAY_INLINE_FLEX)
                {
                    min_width = CssMinWidthEnum.CSS_MIN_WIDTH_SET;
                    length = Fixed.F_0;
                    unit = CssUnit.CSS_UNIT_PX;
                }
            }

            return min_width;
        }

        public CssBackgroundRepeat ComputedBackgroundRepeat()
        {
            return GetBackgroundRepeat();
        }

        public byte ComputedClear()
        {
            return GetClear();
        }

        public CssPaddingEnum ComputedPaddingTop(
                ref Fixed length, ref CssUnit unit)
        {
            return GetPaddingTop(ref length, ref unit);
        }

        public CssPaddingEnum ComputedPaddingRight(
                ref Fixed length, ref CssUnit unit)
        {
            return GetPaddingRight(ref length, ref unit);
        }

        public CssPaddingEnum ComputedPaddingBottom(
                ref Fixed length, ref CssUnit unit)
        {
            return GetPaddingBottom(ref length, ref unit);
        }

        public CssPaddingEnum ComputedPaddingLeft(
                ref Fixed length, ref CssUnit unit)
        {
            return GetPaddingLeft(ref length, ref unit);
        }

        public byte ComputedOverflowX()
        {
            return GetOverflowX();
        }

        public byte ComputedOverflowY()
        {
            return GetOverflowY();
        }

        // computed.c:771
        public CssPosition ComputedPosition()
        {
            return (CssPosition)GetPosition();
        }
        public CssOpacityEnum ComputedOpacity(ref Fixed opacity)
        {
            return GetOpacity(ref opacity);
        }

        // computed.c:782 css_text_transform
        public CssTextTransformEnum ComputedTextTransform()
        {
            return GetTextTransform();
        }
        public CssTextIndentEnum ComputedTextIndent(ref Fixed length, ref CssUnit unit)
        {
            return GetTextIndent(ref length, ref unit);
        }

        // computed.c:794 css_computed_white_space
        public CssWhiteSpaceEnum ComputedWhitespace()
        {
            return GetWhitespace();
        }

        public CssBackgroundPositionEnum ComputedBackgroundPosition(
                out Fixed hlength, out CssUnit hunit,
                out Fixed vlength, out CssUnit vunit)
        {
            return GetBackgroundPosition(out hlength, out hunit, out vlength, out vunit);
        }

        public byte ComputedBreakAfter()
        {
            return GetBreakAfter();
        }

        public byte ComputedBreakBefore()
        {
            return GetBreakBefore();
        }

        public byte ComputedBreakInside()
        {
            return GetBreakInside();
        }

        public byte ComputedColumnCount(
                ref int column_count)
        {
            return GetColumnCount(ref column_count);
        }

        public byte ComputedColumnFill()
        {
            return GetColumnFill();
        }

        public CssColumnGapEnum ComputedColumn_gap(
                ref Fixed length, ref CssUnit unit)
        {
            return GetColumnGap(ref length, ref unit);
        }

        public CssColumnRuleColorEnum ComputedColumnRuleColor(ref Color color)
        {
            /* This property is in the uncommon block, so we need to handle
             * absolute value calculation for initial value (currentColor) here. */
            if (GetColumnRuleColor(ref color) ==
                CssColumnRuleColorEnum.CSS_COLUMN_RULE_COLOR_CURRENT_COLOR)
            {
                ComputedColor(out color);
            }
            return CssColumnRuleColorEnum.CSS_COLUMN_RULE_COLOR_COLOR;
        }

        public byte ComputedColumnRuleStyle()
        {
            return GetColumnRuleStyle();
        }

        public CssColumnRuleWidthEnum ComputedColumnRuleWidth(
                ref Fixed length, ref CssUnit unit)
        {
            /* This property is in the uncommon block, so we need to handle
             * absolute value calculation for initial value (medium) here. */
            if (GetColumnRuleWidth(ref length, ref unit) ==
                    CssColumnRuleWidthEnum.CSS_COLUMN_RULE_WIDTH_MEDIUM)
            {
                length = new Fixed(2);
                unit = CssUnit.CSS_UNIT_PX;
            }

            return CssColumnRuleWidthEnum.CSS_COLUMN_RULE_WIDTH_WIDTH;
        }

        public byte ComputedColumnSpan()
        {
            return GetColumnSpan();
        }

        public CssColumnWidthEnum ComputedColumnWidth(
                ref Fixed length, ref CssUnit unit)
        {
            return GetColumnWidth(ref length, ref unit);
        }

        // computed.c:879
        public CssDisplay ComputedDisplay(bool root)
        {
            var position = ComputedPosition();
            var display = (CssDisplay)GetDisplay();

            // Return computed display as per $9.7

            if (display == CssDisplay.CSS_DISPLAY_NONE)
                return display; /* 1. */

            if ((position == CssPosition.CSS_POSITION_ABSOLUTE ||
                    position == CssPosition.CSS_POSITION_FIXED) /* 2. */ ||
                    ComputedFloat() != CssFloat.CSS_FLOAT_NONE /* 3. */ ||
                    root /* 4. */)
            {
                if (display == CssDisplay.CSS_DISPLAY_INLINE_TABLE)
                {
                    return CssDisplay.CSS_DISPLAY_TABLE;
                }
                else if (display == CssDisplay.CSS_DISPLAY_INLINE_FLEX)
                {
                    return CssDisplay.CSS_DISPLAY_FLEX;
                }
                else if (display == CssDisplay.CSS_DISPLAY_INLINE ||
                        display == CssDisplay.CSS_DISPLAY_RUN_IN ||
                        display == CssDisplay.CSS_DISPLAY_TABLE_ROW_GROUP ||
                        display == CssDisplay.CSS_DISPLAY_TABLE_COLUMN ||
                        display == CssDisplay.CSS_DISPLAY_TABLE_COLUMN_GROUP ||
                        display == CssDisplay.CSS_DISPLAY_TABLE_HEADER_GROUP ||
                        display == CssDisplay.CSS_DISPLAY_TABLE_FOOTER_GROUP ||
                        display == CssDisplay.CSS_DISPLAY_TABLE_ROW ||
                        display == CssDisplay.CSS_DISPLAY_TABLE_CELL ||
                        display == CssDisplay.CSS_DISPLAY_TABLE_CAPTION ||
                        display == CssDisplay.CSS_DISPLAY_INLINE_BLOCK)
                {
                    return CssDisplay.CSS_DISPLAY_BLOCK;
                }
            }

            /* 5. */
            return display;
        }

        // computed.c:917
        public CssDisplay ComputedDisplayStatic()
        {
            return (CssDisplay)GetDisplay();
        }
        public byte ComputedFontVariant()
        {
            return GetFontVariant();
        }

        public byte ComputedTextDecoration()
        {
            return GetTextDecoration();
        }

        public byte ComputedFontFamily(string[] names)
        {
            return GetFontFamily(ref names);
        }

        public byte ComputedBorderTopStyle()
        {
            return GetBorderTopStyle();
        }

        public byte ComputedBorderRightStyle()
        {
            return GetBorderRightStyle();
        }

        public byte ComputedBorderBottomStyle()
        {
            return GetBorderBottomStyle();
        }

        public byte ComputedBorderLeftStyle()
        {
            return GetBorderLeftStyle();
        }

        public byte ComputedFontWeight()
        {
            return GetFontWeight();
        }

        public byte ComputedListStyleType()
        {
            return GetListStyleType();
        }

        public byte ComputedOutlineStyle()
        {
            return GetOutlineStyle();
        }

        public byte ComputedTableLayout()
        {
            return GetTableLayout();
        }

        public byte ComputedUnicodeBidi()
        {
            return GetUnicodeBidi();
        }

        public byte ComputedVisibility()
        {
            return GetVisibility();
        }

        public byte ComputedListStylePosition()
        {
            return GetListStylePosition();
        }

        public byte ComputedTextAlign()
        {
            return GetTextAlign();
        }

        public byte ComputedPageBreakAfter()
        {
            return GetPageBreakAfter();
        }

        public byte ComputedPageBreakBefore()
        {
            return GetPageBreakBefore();
        }

        public byte ComputedPageBreakInside()
        {
            return GetPageBreakInside();
        }

        public byte ComputedOrphans(
                ref int orphans)
        {
            return GetOrphans(ref orphans);
        }

        public byte ComputedWidows(ref int widows)
        {
            return GetWidows(ref widows);
        }

        public byte ComputedAlignContent()
        {
            return GetAlignContent();
        }

        public byte ComputedAlign_items()
        {
            return GetAlignItems();
        }

        public byte ComputedAlignSelf()
        {
            return GetAlignSelf();
        }

        public CssFlexBasisEnum ComputedFlexBasis(ref Fixed length, ref CssUnit unit)
        {
            return GetFlexBasis(ref length, ref unit);
        }

        public byte ComputedFlexDirection()
        {
            return GetFlexDirection();
        }

        public CssFlexGrowEnum ComputedFlex_grow(ref Fixed number)
        {
            return GetFlexGrow(number);
        }

        public CssFlexShrinkEnum ComputedFlexShrink(ref Fixed number)
        {
            return GetFlexShrink(number);
        }

        public byte ComputedFlexWrap()
        {
            return GetFlexWrap();
        }

        public byte Computed_justifyContent()
        {
            return GetJustifyContent();
        }

        public CssOrderEnum Computed_order(ref int order)
        {
            return GetOrder(ref order);
        }
        #endregion

        // computed.c:1747
        void ComputeAbsoluteLengthPair(CssHintLength ex_size, string propsName)
        {
            Fixed length1 = new Fixed(0), length2 = new Fixed(0);
            CssUnit unit1 = new CssUnit(), unit2 = new CssUnit();

            byte type;

            // FIXME: This doesn't look good and needs to be done somehow differently
            switch (propsName)
            {
                case "background_position":
                    type = (byte)GetBackgroundPosition(out length1, out unit1, out length2, out unit2);
                    break;
                case "border_spacing":
                    //type = GetBackgroundPosition(out length1, out unit1, out length2, out unit2);
                    Log.Unimplemented("border_spacing");
                    type = 0xff;
                    break;
                default:
                    throw new Exception("Unsupported type");
            }

            if ((CssBackgroundPositionEnum)type != CssBackgroundPositionEnum.CSS_BACKGROUND_POSITION_SET)
            {
                // Everything is allright
                return;
            }

            if (unit1 == CssUnit.CSS_UNIT_EX)
            {
                length1 = length1 * ex_size.Value;
                unit1 = ex_size.Unit;
            }

            if (unit2 == CssUnit.CSS_UNIT_EX)
            {
                length2 = length2 * ex_size.Value;
                unit2 = ex_size.Unit;
            }

            switch (propsName)
            {
                case "background_position":
                    SetBackgroundPosition(type, length1, unit1, length2, unit2);
                    break;
                case "border_spacing":
                    //SetBorderSpacing(type, length1, unit1, length2, unit2);
                    Log.Unimplemented("border_spacing");
                    break;
                default:
                    throw new Exception("Unsupported type");
            }
        }

        // dump.c:63
        static string DumpCssNumber(Fixed val)
        {
            return val.ToString();
        }

        static string DumpCssUnit(Fixed val, CssUnit unit)
        {
            string res = DumpCssNumber(val);

            switch (unit)
            {
                case CssUnit.CSS_UNIT_PX:
                    res += "px";
                    break;
                case CssUnit.CSS_UNIT_EX:
                    res += "ex";
                    break;
                case CssUnit.CSS_UNIT_EM:
                    res += "em";
                    break;
                case CssUnit.CSS_UNIT_IN:
                    res += "in";
                    break;
                case CssUnit.CSS_UNIT_CM:
                    res += "cm";
                    break;
                case CssUnit.CSS_UNIT_MM:
                    res += "mm";
                    break;
                case CssUnit.CSS_UNIT_PT:
                    res += "pt";
                    break;
                case CssUnit.CSS_UNIT_PC:
                    res += "pc";
                    break;
                case CssUnit.CSS_UNIT_PCT:
                    res += "%%";
                    break;
                case CssUnit.CSS_UNIT_DEG:
                    res += "deg";
                    break;
                case CssUnit.CSS_UNIT_GRAD:
                    res += "grad";
                    break;
                case CssUnit.CSS_UNIT_RAD:
                    res += "rad";
                    break;
                case CssUnit.CSS_UNIT_MS:
                    res += "ms";
                    break;
                case CssUnit.CSS_UNIT_S:
                    res += "s";
                    break;
                case CssUnit.CSS_UNIT_HZ:
                    res += "Hz";
                    break;
                case CssUnit.CSS_UNIT_KHZ:
                    res += "kHz";
                    break;
                case CssUnit.CSS_UNIT_CH:
                    res += "ch";
                    break;
                case CssUnit.CSS_UNIT_REM:
                    res += "rem";
                    break;
                case CssUnit.CSS_UNIT_LH:
                    res += "lh";
                    break;
                case CssUnit.CSS_UNIT_VH:
                    res += "vh";
                    break;
                case CssUnit.CSS_UNIT_VW:
                    res += "vw";
                    break;
                case CssUnit.CSS_UNIT_VI:
                    res += "vi";
                    break;
                case CssUnit.CSS_UNIT_VB:
                    res += "vb";
                    break;
                case CssUnit.CSS_UNIT_VMIN:
                    res += "vmin";
                    break;
                case CssUnit.CSS_UNIT_VMAX:
                    res += "vmax";
                    break;
                case CssUnit.CSS_UNIT_Q:
                    res += "q";
                    break;
                default:
                    break;
            }

            return res;
        }

        // dump.c:150
        public void Dump(StreamWriter sw)
        {
            /*
                byte val;
                CssColor color = 0;
                lwc_string *url = NULL;
                css_computed_clip_rect rect = { 0, 0, 0, 0, CSS_UNIT_PX, CSS_UNIT_PX,
                                CSS_UNIT_PX, CSS_UNIT_PX, true, true,
                                true, true };
                const css_computed_content_item *content = NULL;
                const css_computed_counter *counter = NULL;
                lwc_string **string_list = NULL;
                int32_t zindex = 0;*/
            Fixed len1 = new Fixed(0);
            Fixed len2 = new Fixed(0);
            CssUnit unit1 = CssUnit.CSS_UNIT_PX, unit2 = CssUnit.CSS_UNIT_PX;

            sw.Write("{ ");

            // background-attachment
            /*
        val = css_computed_backgroundAttachment(style);
        switch (val)
        {
            case CSS_BACKGROUND_ATTACHMENT_FIXED:
                sw.Write("background-attachment: fixed ");
                break;
            case CSS_BACKGROUND_ATTACHMENT_SCROLL:
                sw.Write("background-attachment: scroll ");
                break;
            default:
                break;
        }
*/
            // background-color
            var val2 = ComputedBackgroundColor(out Color color);
            switch (val2)
            {
                case CssBackgroundColorEnum.CSS_BACKGROUND_COLOR_COLOR:
                    sw.Write($"background-color: #{color.ToString()} ");
                    break;
                default:
                    break;
            }

            /* background-image
            val = css_computed_background_image(style, &url);
            if (val == CSS_BACKGROUND_IMAGE_IMAGE && url != NULL)
            {
                fprintf(stream, "background-image: url('%.*s') ",
                        (int)lwc_string_length(url),
                        lwc_string_data(url));
            }
            else if (val == CSS_BACKGROUND_IMAGE_NONE)
            {
                sw.Write("background-image: none ");
            }

            /* background-position
            val = css_computed_background_position(style, &len1, &unit1,
                    &len2, &unit2);
            if (val == CSS_BACKGROUND_POSITION_SET)
            {
                fprintf(stream, "background-position: ");
                dump_css_unit(stream, len1, unit1);
                fprintf(stream, " ");
                dump_css_unit(stream, len2, unit2);
                sw.Write(" ");
            }

            /* background-repeat
            val = css_computed_background_repeat(style);
            switch (val)
            {
                case CSS_BACKGROUND_REPEAT_REPEAT_X:
                    sw.Write("background-repeat: repeat-x ");
                    break;
                case CSS_BACKGROUND_REPEAT_REPEAT_Y:
                    sw.Write("background-repeat: repeat-y ");
                    break;
                case CSS_BACKGROUND_REPEAT_REPEAT:
                    sw.Write("background-repeat: repeat ");
                    break;
                case CSS_BACKGROUND_REPEAT_NO_REPEAT:
                    sw.Write("background-repeat: no-repeat ");
                    break;
                default:
                    break;
            }

            /* border-collapse
            val = css_computed_border_collapse(style);
            switch (val)
            {
                case CSS_BORDER_COLLAPSE_SEPARATE:
                    sw.Write("border-collapse: separate ");
                    break;
                case CSS_BORDER_COLLAPSE_COLLAPSE:
                    sw.Write("border-collapse: collapse ");
                    break;
                default:

                    break;
            }

            /* border-spacing
            val = css_computed_border_spacing(style, &len1, &unit1, &len2, &unit2);
            if (val == CSS_BORDER_SPACING_SET)
            {
                sw.Write("border-spacing: ");
                dump_css_unit(stream, len1, unit1);
                sw.Write(" ");
                dump_css_unit(stream, len2, unit2);
                sw.Write(" ");
            }

            /* border-top-color
            val = css_computed_border_top_color(style, &color);
            switch (val)
            {
                case CSS_BORDER_COLOR_COLOR:
                    sw.Write("border-top-color: #%08x ", color);
                    break;
                default:
                    break;
            }

            /* border-right-color
            val = css_computed_border_right_color(style, &color);
            switch (val)
            {
                case CSS_BORDER_COLOR_COLOR:
                    fprintf(stream, "border-right-color: #%08x ", color);
                    break;
                default:
                    break;
            }

            /* border-bottom-color
            val = css_computed_border_bottom_color(style, &color);
            switch (val)
            {
                case CSS_BORDER_COLOR_COLOR:
                    fprintf(stream, "border-bottom-color: #%08x ", color);
                    break;
                default:
                    break;
            }

            /* border-left-color
            val = css_computed_border_left_color(style, &color);
            switch (val)
            {
                case CSS_BORDER_COLOR_COLOR:
                    fprintf(stream, "border-left-color: #%08x ", color);
                    break;
                default:
                    break;
            }

            /* border-top-style
            val = css_computed_border_top_style(style);
            switch (val)
            {
                case CSS_BORDER_STYLE_NONE:
                    sw.Write("border-top-style: none ");
                    break;
                case CSS_BORDER_STYLE_HIDDEN:
                    sw.Write("border-top-style: hidden ");
                    break;
                case CSS_BORDER_STYLE_DOTTED:
                    sw.Write("border-top-style: dotted ");
                    break;
                case CSS_BORDER_STYLE_DASHED:
                    sw.Write("border-top-style: dashed ");
                    break;
                case CSS_BORDER_STYLE_SOLID:
                    sw.Write("border-top-style: solid ");
                    break;
                case CSS_BORDER_STYLE_DOUBLE:
                    sw.Write("border-top-style: double ");
                    break;
                case CSS_BORDER_STYLE_GROOVE:
                    sw.Write("border-top-style: groove ");
                    break;
                case CSS_BORDER_STYLE_RIDGE:
                    sw.Write("border-top-style: ridge ");
                    break;
                case CSS_BORDER_STYLE_INSET:
                    sw.Write("border-top-style: inset ");
                    break;
                case CSS_BORDER_STYLE_OUTSET:
                    sw.Write("border-top-style: outset ");
                    break;
                default:
                    break;
            }

            /* border-right-style
            val = css_computed_border_right_style(style);
            switch (val)
            {
                case CSS_BORDER_STYLE_NONE:
                    sw.Write("border-right-style: none ");
                    break;
                case CSS_BORDER_STYLE_HIDDEN:
                    sw.Write("border-right-style: hidden ");
                    break;
                case CSS_BORDER_STYLE_DOTTED:
                    sw.Write("border-right-style: dotted ");
                    break;
                case CSS_BORDER_STYLE_DASHED:
                    sw.Write("border-right-style: dashed ");
                    break;
                case CSS_BORDER_STYLE_SOLID:
                    sw.Write("border-right-style: solid ");
                    break;
                case CSS_BORDER_STYLE_DOUBLE:
                    sw.Write("border-right-style: double ");
                    break;
                case CSS_BORDER_STYLE_GROOVE:
                    sw.Write("border-right-style: groove ");
                    break;
                case CSS_BORDER_STYLE_RIDGE:
                    sw.Write("border-right-style: ridge ");
                    break;
                case CSS_BORDER_STYLE_INSET:
                    sw.Write("border-right-style: inset ");
                    break;
                case CSS_BORDER_STYLE_OUTSET:
                    sw.Write("border-right-style: outset ");
                    break;
                default:
                    break;
            }

            /* border-bottom-style
            val = css_computed_border_bottom_style(style);
            switch (val)
            {
                case CSS_BORDER_STYLE_NONE:
                    sw.Write("border-bottom-style: none ");
                    break;
                case CSS_BORDER_STYLE_HIDDEN:
                    sw.Write("border-bottom-style: hidden ");
                    break;
                case CSS_BORDER_STYLE_DOTTED:
                    sw.Write("border-bottom-style: dotted ");
                    break;
                case CSS_BORDER_STYLE_DASHED:
                    sw.Write("border-bottom-style: dashed ");
                    break;
                case CSS_BORDER_STYLE_SOLID:
                    sw.Write("border-bottom-style: solid ");
                    break;
                case CSS_BORDER_STYLE_DOUBLE:
                    sw.Write("border-bottom-style: double ");
                    break;
                case CSS_BORDER_STYLE_GROOVE:
                    sw.Write("border-bottom-style: groove ");
                    break;
                case CSS_BORDER_STYLE_RIDGE:
                    sw.Write("border-bottom-style: ridge ");
                    break;
                case CSS_BORDER_STYLE_INSET:
                    sw.Write("border-bottom-style: inset ");
                    break;
                case CSS_BORDER_STYLE_OUTSET:
                    sw.Write("border-bottom-style: outset ");
                    break;
                default:
                    break;
            }

            /* border-left-style 
            val = css_computed_border_left_style(style);
            switch (val)
            {
                case CSS_BORDER_STYLE_NONE:
                    sw.Write("border-left-style: none ");
                    break;
                case CSS_BORDER_STYLE_HIDDEN:
                    sw.Write("border-left-style: hidden ");
                    break;
                case CSS_BORDER_STYLE_DOTTED:
                    sw.Write("border-left-style: dotted ");
                    break;
                case CSS_BORDER_STYLE_DASHED:
                    sw.Write("border-left-style: dashed ");
                    break;
                case CSS_BORDER_STYLE_SOLID:
                    sw.Write("border-left-style: solid ");
                    break;
                case CSS_BORDER_STYLE_DOUBLE:
                    sw.Write("border-left-style: double ");
                    break;
                case CSS_BORDER_STYLE_GROOVE:
                    sw.Write("border-left-style: groove ");
                    break;
                case CSS_BORDER_STYLE_RIDGE:
                    sw.Write("border-left-style: ridge ");
                    break;
                case CSS_BORDER_STYLE_INSET:
                    sw.Write("border-left-style: inset ");
                    break;
                case CSS_BORDER_STYLE_OUTSET:
                    sw.Write("border-left-style: outset ");
                    break;
                default:
                    break;
            }

            /* border-top-width
            val = css_computed_border_top_width(style, &len1, &unit1);
            switch (val)
            {
                case CSS_BORDER_WIDTH_THIN:
                    sw.Write("border-top-width: thin ");
                    break;
                case CSS_BORDER_WIDTH_MEDIUM:
                    sw.Write("border-top-width: medium ");
                    break;
                case CSS_BORDER_WIDTH_THICK:
                    sw.Write("border-top-width: thick ");
                    break;
                case CSS_BORDER_WIDTH_WIDTH:
                    sw.Write("border-top-width: ");
                    dump_css_unit(stream, len1, unit1);
                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* border-right-width
            val = css_computed_border_right_width(style, &len1, &unit1);
            switch (val)
            {
                case CSS_BORDER_WIDTH_THIN:
                    sw.Write("border-right-width: thin ");
                    break;
                case CSS_BORDER_WIDTH_MEDIUM:
                    sw.Write("border-right-width: medium ");
                    break;
                case CSS_BORDER_WIDTH_THICK:
                    sw.Write("border-right-width: thick ");
                    break;
                case CSS_BORDER_WIDTH_WIDTH:
                    sw.Write("border-right-width: ");
                    dump_css_unit(stream, len1, unit1);
                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* border-bottom-width
            val = css_computed_border_bottom_width(style, &len1, &unit1);
            switch (val)
            {
                case CSS_BORDER_WIDTH_THIN:
                    sw.Write("border-bottom-width: thin ");
                    break;
                case CSS_BORDER_WIDTH_MEDIUM:
                    sw.Write("border-bottom-width: medium ");
                    break;
                case CSS_BORDER_WIDTH_THICK:
                    sw.Write("border-bottom-width: thick ");
                    break;
                case CSS_BORDER_WIDTH_WIDTH:
                    sw.Write("border-bottom-width: ");
                    dump_css_unit(stream, len1, unit1);
                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* border-left-width
            val = css_computed_border_left_width(style, &len1, &unit1);
            switch (val)
            {
                case CSS_BORDER_WIDTH_THIN:
                    sw.Write("border-left-width: thin ");
                    break;
                case CSS_BORDER_WIDTH_MEDIUM:
                    sw.Write("border-left-width: medium ");
                    break;
                case CSS_BORDER_WIDTH_THICK:
                    sw.Write("border-left-width: thick ");
                    break;
                case CSS_BORDER_WIDTH_WIDTH:
                    sw.Write("border-left-width: ");
                    dump_css_unit(stream, len1, unit1);
                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* bottom
            val = css_computed_bottom(style, &len1, &unit1);
            switch (val)
            {
                case CSS_BOTTOM_AUTO:
                    sw.Write("bottom: auto ");
                    break;
                case CSS_BOTTOM_SET:
                    sw.Write("bottom: ");
                    dump_css_unit(stream, len1, unit1);
                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* caption-side
            val = css_computed_caption_side(style);
            switch (val)
            {
                case CSS_CAPTION_SIDE_TOP:
                    sw.Write("caption_side: top ");
                    break;
                case CSS_CAPTION_SIDE_BOTTOM:
                    sw.Write("caption_side: bottom ");
                    break;
                default:
                    break;
            }

            /* clear
            val = css_computed_clear(style);
            switch (val)
            {
                case CSS_CLEAR_NONE:
                    sw.Write("clear: none ");
                    break;
                case CSS_CLEAR_LEFT:
                    sw.Write("clear: left ");
                    break;
                case CSS_CLEAR_RIGHT:
                    sw.Write("clear: right ");
                    break;
                case CSS_CLEAR_BOTH:
                    sw.Write("clear: both ");
                    break;
                default:
                    break;
            }

            /* clip
            val = css_computed_clip(style, &rect);
            switch (val)
            {
                case CSS_CLIP_AUTO:
                    sw.Write("clip: auto ");
                    break;
                case CSS_CLIP_RECT:
                    sw.Write("clip: rect( ");

                    if (rect.top_auto)
                        sw.Write("auto");
                    else
                        dump_css_unit(stream, rect.top, rect.tunit);
                    sw.Write(", ");

                    if (rect.right_auto)
                        sw.Write("auto");
                    else
                        dump_css_unit(stream, rect.right, rect.runit);
                    sw.Write(", ");

                    if (rect.bottom_auto)
                        sw.Write("auto");
                    else
                        dump_css_unit(stream, rect.bottom, rect.bunit);
                    sw.Write(", ");

                    if (rect.left_auto)
                        sw.Write("auto");
                    else
                        dump_css_unit(stream, rect.left, rect.lunit);
                    sw.Write(") ");
                    break;
                default:
                    break;
            }
            */
            // color
            var val50 = ComputedColor(out color);
            if (val50 == CssColorEnum.CSS_COLOR_COLOR)
            {
                sw.Write($"color: #{color.ToString()} ");
            }

            /* content
            val = css_computed_content(style, &content);
            switch (val)
            {
                case CSS_CONTENT_NONE:
                    sw.Write("content: none ");
                    break;
                case CSS_CONTENT_NORMAL:
                    sw.Write("content: normal ");
                    break;
                case CSS_CONTENT_SET:
                    sw.Write("content:");

                    while (content->type != CSS_COMPUTED_CONTENT_NONE)
                    {
                        sw.Write(" ");

                        switch (content->type)
                        {
                            case CSS_COMPUTED_CONTENT_STRING:
                                fprintf(stream, "\"%.*s\"",
                                        (int)lwc_string_length(
                                        content->data.string),
                                        lwc_string_data(
                                        content->data.string));
                                break;
                            case CSS_COMPUTED_CONTENT_URI:
                                fprintf(stream, "uri(\"%.*s\")",
                                        (int)lwc_string_length(
                                        content->data.uri),
                                        lwc_string_data(
                                        content->data.uri));
                                break;
                            case CSS_COMPUTED_CONTENT_COUNTER:
                                fprintf(stream, "counter(%.*s)",
                                        (int)lwc_string_length(
                                        content->data.counter.name),
                                        lwc_string_data(
                                        content->data.counter.name));
                                break;
                            case CSS_COMPUTED_CONTENT_COUNTERS:
                                fprintf(stream, "counters(%.*s, \"%.*s\")",
                                        (int)lwc_string_length(
                                        content->data.counters.name),
                                        lwc_string_data(
                                        content->data.counters.name),
                                        (int)lwc_string_length(
                                        content->data.counters.sep),
                                        lwc_string_data(
                                        content->data.counters.sep));
                                break;
                            case CSS_COMPUTED_CONTENT_ATTR:
                                fprintf(stream, "attr(%.*s)",
                                        (int)lwc_string_length(
                                        content->data.attr),
                                        lwc_string_data(
                                        content->data.attr));
                                break;
                            case CSS_COMPUTED_CONTENT_OPEN_QUOTE:
                                fprintf(stream, "open-quote");
                                break;
                            case CSS_COMPUTED_CONTENT_CLOSE_QUOTE:
                                sw.Write("close-quote");
                                break;
                            case CSS_COMPUTED_CONTENT_NO_OPEN_QUOTE:
                                sw.Write("no-open-quote");
                                break;
                            case CSS_COMPUTED_CONTENT_NO_CLOSE_QUOTE:
                                sw.Write("no-close-quote");
                                break;
                        }

                        content++;
                    }

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* counter-increment
            val = css_computed_counter_increment(style, &counter);
            if ((val == CSS_COUNTER_INCREMENT_NONE) || (counter == NULL))
            {
                sw.Write("counter-increment: none ");
            }
            else
            {
                sw.Write("counter-increment:");

                while (counter->name != NULL)
                {
                    fprintf(stream, " %.*s ",
                            (int)lwc_string_length(counter->name),
                            lwc_string_data(counter->name));

                    dump_css_fixed(stream, counter->value);

                    counter++;
                }

                sw.Write(" ");
            }

            /* counter-reset
            val = css_computed_counter_reset(style, &counter);
            if ((val == CSS_COUNTER_RESET_NONE) || (counter == NULL))
            {
                sw.Write("counter-reset: none ");
            }
            else
            {
                sw.Write("counter-reset:");

                while (counter->name != NULL)
                {
                    fprintf(stream, " %.*s ",
                            (int)lwc_string_length(counter->name),
                            lwc_string_data(counter->name));

                    dump_css_fixed(stream, counter->value);

                    counter++;
                }

                sw.Write(" ");
            }

            /* cursor
            val = css_computed_cursor(style, &string_list);
            fprintf(stream, "cursor:");

            if (string_list != NULL)
            {
                while (*string_list != NULL)
                {
                    sw.Write(" url\"%.*s\")",
                            (int)lwc_string_length(*string_list),
                            lwc_string_data(*string_list));

                    string_list++;
                }
            }
            switch (val)
            {
                case CSS_CURSOR_AUTO:
                    sw.Write(" auto ");
                    break;
                case CSS_CURSOR_CROSSHAIR:
                    sw.Write(" crosshair ");
                    break;
                case CSS_CURSOR_DEFAULT:
                    sw.Write(" default ");
                    break;
                case CSS_CURSOR_POINTER:
                    sw.Write(" pointer ");
                    break;
                case CSS_CURSOR_MOVE:
                    sw.Write(" move ");
                    break;
                case CSS_CURSOR_E_RESIZE:
                    sw.Write(" e-resize ");
                    break;
                case CSS_CURSOR_NE_RESIZE:
                    sw.Write(" ne-resize ");
                    break;
                case CSS_CURSOR_NW_RESIZE:
                    sw.Write(" nw-resize ");
                    break;
                case CSS_CURSOR_N_RESIZE:
                    sw.Write(" n-resize ");
                    break;
                case CSS_CURSOR_SE_RESIZE:
                    sw.Write(" se-resize ");
                    break;
                case CSS_CURSOR_SW_RESIZE:
                    sw.Write(" sw-resize ");
                    break;
                case CSS_CURSOR_S_RESIZE:
                    sw.Write(" s-resize ");
                    break;
                case CSS_CURSOR_W_RESIZE:
                    sw.Write(" w-resize ");
                    break;
                case CSS_CURSOR_TEXT:
                    sw.Write(" text ");
                    break;
                case CSS_CURSOR_WAIT:
                    sw.Write(" wait ");
                    break;
                case CSS_CURSOR_HELP:
                    sw.Write(" help ");
                    break;
                case CSS_CURSOR_PROGRESS:
                    sw.Write(" progress ");
                    break;
                default:
                    break;
            }

            /* direction
            val = css_computed_direction(style);
            switch (val)
            {
                case CSS_DIRECTION_LTR:
                    sw.Write("direction: ltr ");
                    break;
                case CSS_DIRECTION_RTL:
                    sw.Write("direction: rtl ");
                    break;
                default:
                    break;
            }
                */
            // display
            var val100 = BoxTree.ns_computed_display_static(this);
            switch (val100)
            {
                case CssDisplay.CSS_DISPLAY_INLINE:
                    sw.Write("display: inline ");
                    break;
                case CssDisplay.CSS_DISPLAY_BLOCK:
                    sw.Write("display: block ");
                    break;
                case CssDisplay.CSS_DISPLAY_LIST_ITEM:
                    sw.Write("display: list-item ");
                    break;
                case CssDisplay.CSS_DISPLAY_RUN_IN:
                    sw.Write("display: run-in ");
                    break;
                case CssDisplay.CSS_DISPLAY_INLINE_BLOCK:
                    sw.Write("display: inline-block ");
                    break;
                case CssDisplay.CSS_DISPLAY_TABLE:
                    sw.Write("display: table ");
                    break;
                case CssDisplay.CSS_DISPLAY_INLINE_TABLE:
                    sw.Write("display: inline-table ");
                    break;
                case CssDisplay.CSS_DISPLAY_TABLE_ROW_GROUP:
                    sw.Write("display: table-row-group ");
                    break;
                case CssDisplay.CSS_DISPLAY_TABLE_HEADER_GROUP:
                    sw.Write("display: table-header-group ");
                    break;
                case CssDisplay.CSS_DISPLAY_TABLE_FOOTER_GROUP:
                    sw.Write("display: table-footer-group ");
                    break;
                case CssDisplay.CSS_DISPLAY_TABLE_ROW:
                    sw.Write("display: table-row ");
                    break;
                case CssDisplay.CSS_DISPLAY_TABLE_COLUMN_GROUP:
                    sw.Write("display: table-column-group ");
                    break;
                case CssDisplay.CSS_DISPLAY_TABLE_COLUMN:
                    sw.Write("display: table-column ");
                    break;
                case CssDisplay.CSS_DISPLAY_TABLE_CELL:
                    sw.Write("display: table-cell ");
                    break;
                case CssDisplay.CSS_DISPLAY_TABLE_CAPTION:
                    sw.Write("display: table-caption ");
                    break;
                case CssDisplay.CSS_DISPLAY_NONE:
                    sw.Write("display: none ");
                    break;
                default:
                    break;
            }

            /* empty-cells
            val = css_computed_empty_cells(style);
            switch (val)
            {
                case CSS_EMPTY_CELLS_SHOW:
                    sw.Write("empty-cells: show ");
                    break;
                case CSS_EMPTY_CELLS_HIDE:
                    sw.Write("empty-cells: hide ");
                    break;
                default:
                    break;
            }
            */
            // float
            var val150 = ComputedFloat();
            switch (val150)
            {
                case CssFloat.CSS_FLOAT_LEFT:
                    sw.Write("float: left ");
                    break;
                case CssFloat.CSS_FLOAT_RIGHT:
                    sw.Write("float: right ");
                    break;
                case CssFloat.CSS_FLOAT_NONE:
                    sw.Write("float: none ");
                    break;
                default:
                    break;
            }

            /* font-family
            val = css_computed_font_family(style, &string_list);
            if (val != CSS_FONT_FAMILY_INHERIT)
            {
                sw.Write("font-family:");

                if (string_list != NULL)
                {
                    while (*string_list != NULL)
                    {
                        sw.Write(" \"%.*s\"",
                            (int)lwc_string_length(*string_list),
                            lwc_string_data(*string_list));

                        string_list++;
                    }
                }
                switch (val)
                {
                    case CSS_FONT_FAMILY_SERIF:
                        sw.Write(" serif ");
                        break;
                    case CSS_FONT_FAMILY_SANS_SERIF:
                        sw.Write(" sans-serif ");
                        break;
                    case CSS_FONT_FAMILY_CURSIVE:
                        sw.Write(" cursive ");
                        break;
                    case CSS_FONT_FAMILY_FANTASY:
                        sw.Write(" fantasy ");
                        break;
                    case CSS_FONT_FAMILY_MONOSPACE:
                        sw.Write(" monospace ");
                        break;
                }
            } */

            // font-size
            var val2331 = ComputedFontSize(ref len1, ref unit1);
            switch (val2331)
            {
                case CssFontSizeEnum.CSS_FONT_SIZE_XX_SMALL:
                    sw.Write("font-size: xx-small ");
                    break;
                case CssFontSizeEnum.CSS_FONT_SIZE_X_SMALL:
                    sw.Write("font-size: x-small ");
                    break;
                case CssFontSizeEnum.CSS_FONT_SIZE_SMALL:
                    sw.Write("font-size: small ");
                    break;
                case CssFontSizeEnum.CSS_FONT_SIZE_MEDIUM:
                    sw.Write("font-size: medium ");
                    break;
                case CssFontSizeEnum.CSS_FONT_SIZE_LARGE:
                    sw.Write("font-size: large ");
                    break;
                case CssFontSizeEnum.CSS_FONT_SIZE_X_LARGE:
                    sw.Write("font-size: x-large ");
                    break;
                case CssFontSizeEnum.CSS_FONT_SIZE_XX_LARGE:
                    sw.Write("font-size: xx-large ");
                    break;
                case CssFontSizeEnum.CSS_FONT_SIZE_LARGER:
                    sw.Write("font-size: larger ");
                    break;
                case CssFontSizeEnum.CSS_FONT_SIZE_SMALLER:
                    sw.Write("font-size: smaller ");
                    break;
                case CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION:
                    sw.Write("font-size: ");

                    sw.Write(DumpCssUnit(len1, unit1));

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* font-style
            val = css_computed_font_style(style);
            switch (val)
            {
                case CSS_FONT_STYLE_NORMAL:
                    sw.Write("font-style: normal ");
                    break;
                case CSS_FONT_STYLE_ITALIC:
                    sw.Write("font-style: italic ");
                    break;
                case CSS_FONT_STYLE_OBLIQUE:
                    sw.Write("font-style: oblique ");
                    break;
                default:
                    break;
            }

            /* font-variant
            val = css_computed_font_variant(style);
            switch (val)
            {
                case CSS_FONT_VARIANT_NORMAL:
                    sw.Write("font-variant: normal ");
                    break;
                case CSS_FONT_VARIANT_SMALL_CAPS:
                    sw.Write("font-variant: small-caps ");
                    break;
                default:
                    break;
            }

            /* font-weight
            val = css_computed_font_weight(style);
            switch (val)
            {
                case CSS_FONT_WEIGHT_NORMAL:
                    sw.Write("font-weight: normal ");
                    break;
                case CSS_FONT_WEIGHT_BOLD:
                    sw.Write("font-weight: bold ");
                    break;
                case CSS_FONT_WEIGHT_BOLDER:
                    sw.Write("font-weight: bolder ");
                    break;
                case CSS_FONT_WEIGHT_LIGHTER:
                    sw.Write("font-weight: lighter ");
                    break;
                case CSS_FONT_WEIGHT_100:
                    sw.Write("font-weight: 100 ");
                    break;
                case CSS_FONT_WEIGHT_200:
                    sw.Write("font-weight: 200 ");
                    break;
                case CSS_FONT_WEIGHT_300:
                    sw.Write("font-weight: 300 ");
                    break;
                case CSS_FONT_WEIGHT_400:
                    sw.Write("font-weight: 400 ");
                    break;
                case CSS_FONT_WEIGHT_500:
                    sw.Write("font-weight: 500 ");
                    break;
                case CSS_FONT_WEIGHT_600:
                    sw.Write("font-weight: 600 ");
                    break;
                case CSS_FONT_WEIGHT_700:
                    sw.Write("font-weight: 700 ");
                    break;
                case CSS_FONT_WEIGHT_800:
                    sw.Write("font-weight: 800 ");
                    break;
                case CSS_FONT_WEIGHT_900:
                    sw.Write("font-weight: 900 ");
                    break;
                default:
                    break;
            }

            /* height
            val = css_computed_height(style, &len1, &unit1);
            switch (val)
            {
                case CSS_HEIGHT_AUTO:
                    sw.Write("height: auto ");
                    break;
                case CSS_HEIGHT_SET:
                    sw.Write("height: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* left
            val = css_computed_left(style, &len1, &unit1);
            switch (val)
            {
                case CSS_LEFT_AUTO:
                    sw.Write("left: auto ");
                    break;
                case CSS_LEFT_SET:
                    sw.Write("left: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* letter-spacing
            val = css_computed_letter_spacing(style, &len1, &unit1);
            switch (val)
            {
                case CSS_LETTER_SPACING_NORMAL:
                    sw.Write("letter-spacing: normal ");
                    break;
                case CSS_LETTER_SPACING_SET:
                    sw.Write("letter-spacing: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* line-height
            val = css_computed_line_height(style, &len1, &unit1);
            switch (val)
            {
                case CSS_LINE_HEIGHT_NORMAL:
                    sw.Write("line-height: normal ");
                    break;
                case CSS_LINE_HEIGHT_NUMBER:
                    sw.Write("line-height: ");

                    dump_css_fixed(stream, len1);

                    sw.Write(" ");
                    break;
                case CSS_LINE_HEIGHT_DIMENSION:
                    sw.Write("line-height: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* list-style-image
            val = css_computed_list_style_image(style, &url);
            if (url != NULL)
            {
                fprintf(stream, "list-style-image: url('%.*s') ",
                        (int)lwc_string_length(url),
                        lwc_string_data(url));
            }
            else if (val == CSS_LIST_STYLE_IMAGE_NONE)
            {
                sw.Write("list-style-image: none ");
            }

            /* list-style-position
            val = css_computed_list_style_position(style);
            switch (val)
            {
                case CSS_LIST_STYLE_POSITION_INSIDE:
                    sw.Write("list-style-position: inside ");
                    break;
                case CSS_LIST_STYLE_POSITION_OUTSIDE:
                    sw.Write("list-style-position: outside ");
                    break;
                default:
                    break;
            }

            /* list-style-type
            val = css_computed_list_style_type(style);
            switch (val)
            {
                case CSS_LIST_STYLE_TYPE_DISC:
                    sw.Write("list-style-type: disc ");
                    break;
                case CSS_LIST_STYLE_TYPE_CIRCLE:
                    sw.Write("list-style-type: circle ");
                    break;
                case CSS_LIST_STYLE_TYPE_SQUARE:
                    sw.Write("list-style-type: square ");
                    break;
                case CSS_LIST_STYLE_TYPE_DECIMAL:
                    sw.Write("list-style-type: decimal ");
                    break;
                case CSS_LIST_STYLE_TYPE_DECIMAL_LEADING_ZERO:
                    sw.Write("list-style-type: decimal-leading-zero ");
                    break;
                case CSS_LIST_STYLE_TYPE_LOWER_ROMAN:
                    sw.Write("list-style-type: lower-roman ");
                    break;
                case CSS_LIST_STYLE_TYPE_UPPER_ROMAN:
                    sw.Write("list-style-type: upper-roman ");
                    break;
                case CSS_LIST_STYLE_TYPE_LOWER_GREEK:
                    sw.Write("list-style-type: lower-greek ");
                    break;
                case CSS_LIST_STYLE_TYPE_LOWER_LATIN:
                    sw.Write("list-style-type: lower-latin ");
                    break;
                case CSS_LIST_STYLE_TYPE_UPPER_LATIN:
                    sw.Write("list-style-type: upper-latin ");
                    break;
                case CSS_LIST_STYLE_TYPE_ARMENIAN:
                    sw.Write("list-style-type: armenian ");
                    break;
                case CSS_LIST_STYLE_TYPE_GEORGIAN:
                    sw.Write("list-style-type: georgian ");
                    break;
                case CSS_LIST_STYLE_TYPE_LOWER_ALPHA:
                    sw.Write("list-style-type: lower-alpha ");
                    break;
                case CSS_LIST_STYLE_TYPE_UPPER_ALPHA:
                    sw.Write("list-style-type: upper-alpha ");
                    break;
                case CSS_LIST_STYLE_TYPE_NONE:
                    sw.Write("list-style-type: none ");
                    break;
                default:
                    break;
            }

            /* margin-top
            val = css_computed_margin_top(style, &len1, &unit1);
            switch (val)
            {
                case CSS_MARGIN_AUTO:
                    sw.Write("margin-top: auto ");
                    break;
                case CSS_MARGIN_SET:
                    sw.Write("margin-top: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* margin-right
            val = css_computed_margin_right(style, &len1, &unit1);
            switch (val)
            {
                case CSS_MARGIN_AUTO:
                    sw.Write("margin-right: auto ");
                    break;
                case CSS_MARGIN_SET:
                    sw.Write("margin-right: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* margin-bottom
            val = css_computed_margin_bottom(style, &len1, &unit1);
            switch (val)
            {
                case CSS_MARGIN_AUTO:
                    sw.Write("margin-bottom: auto ");
                    break;
                case CSS_MARGIN_SET:
                    sw.Write("margin-bottom: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* margin-left
            val = css_computed_margin_left(style, &len1, &unit1);
            switch (val)
            {
                case CSS_MARGIN_AUTO:
                    sw.Write("margin-left: auto ");
                    break;
                case CSS_MARGIN_SET:
                    sw.Write("margin-left: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* max-height
            val = css_computed_max_height(style, &len1, &unit1);
            switch (val)
            {
                case CSS_MAX_HEIGHT_NONE:
                    sw.Write("max-height: none ");
                    break;
                case CSS_MAX_HEIGHT_SET:
                    sw.Write("max-height: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* max-width
            val = css_computed_max_width(style, &len1, &unit1);
            switch (val)
            {
                case CSS_MAX_WIDTH_NONE:
                    sw.Write("max-width: none ");
                    break;
                case CSS_MAX_WIDTH_SET:
                    sw.Write("max-width: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* min-height
            val = ns_computed_min_height(style, &len1, &unit1);
            switch (val)
            {
                case CSS_MIN_HEIGHT_SET:
                    sw.Write("min-height: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* min-width
            val = ns_computed_min_width(style, &len1, &unit1);
            switch (val)
            {
                case CSS_MIN_WIDTH_SET:
                    sw.Write("min-width: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* opacity
            val = css_computed_opacity(style, &len1);
            switch (val)
            {
                case CSS_OPACITY_SET:
                    sw.Write("opacity: ");

                    dump_css_fixed(stream, len1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* outline-color
            val = css_computed_outline_color(style, &color);
            switch (val)
            {
                case CSS_OUTLINE_COLOR_INVERT:
                    sw.Write("outline-color: invert ");
                    break;
                case CSS_OUTLINE_COLOR_COLOR:
                    sw.Write("outline-color: #%08x ", color);
                    break;
                default:
                    break;
            }

            /* outline-style
            val = css_computed_outline_style(style);
            switch (val)
            {
                case CSS_OUTLINE_STYLE_NONE:
                    sw.Write("outline-style: none ");
                    break;
                case CSS_OUTLINE_STYLE_DOTTED:
                    sw.Write("outline-style: dotted ");
                    break;
                case CSS_OUTLINE_STYLE_DASHED:
                    sw.Write("outline-style: dashed ");
                    break;
                case CSS_OUTLINE_STYLE_SOLID:
                    sw.Write("outline-style: solid ");
                    break;
                case CSS_OUTLINE_STYLE_DOUBLE:
                    sw.Write("outline-style: double ");
                    break;
                case CSS_OUTLINE_STYLE_GROOVE:
                    sw.Write("outline-style: groove ");
                    break;
                case CSS_OUTLINE_STYLE_RIDGE:
                    sw.Write("outline-style: ridge ");
                    break;
                case CSS_OUTLINE_STYLE_INSET:
                    sw.Write("outline-style: inset ");
                    break;
                case CSS_OUTLINE_STYLE_OUTSET:
                    sw.Write("outline-style: outset ");
                    break;
                default:
                    break;
            }

            /* outline-width
            val = css_computed_outline_width(style, &len1, &unit1);
            switch (val)
            {
                case CSS_OUTLINE_WIDTH_THIN:
                    sw.Write("outline-width: thin ");
                    break;
                case CSS_OUTLINE_WIDTH_MEDIUM:
                    sw.Write("outline-width: medium ");
                    break;
                case CSS_OUTLINE_WIDTH_THICK:
                    sw.Write("outline-width: thick ");
                    break;
                case CSS_OUTLINE_WIDTH_WIDTH:
                    sw.Write("outline-width: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* overflow
            val = css_computed_overflow_x(style);
            switch (val)
            {
                case CSS_OVERFLOW_VISIBLE:
                    sw.Write("overflow-x: visible ");
                    break;
                case CSS_OVERFLOW_HIDDEN:
                    sw.Write("overflow-x: hidden ");
                    break;
                case CSS_OVERFLOW_SCROLL:
                    sw.Write("overflow-x: scroll ");
                    break;
                case CSS_OVERFLOW_AUTO:
                    sw.Write("overflow-x auto ");
                    break;
                default:
                    break;
            }

            /* overflow
            val = css_computed_overflow_y(style);
            switch (val)
            {
                case CSS_OVERFLOW_VISIBLE:
                    sw.Write("overflow-y: visible ");
                    break;
                case CSS_OVERFLOW_HIDDEN:
                    sw.Write("overflow-y: hidden ");
                    break;
                case CSS_OVERFLOW_SCROLL:
                    sw.Write("overflow-y: scroll ");
                    break;
                case CSS_OVERFLOW_AUTO:
                    sw.Write("overflow-y: auto ");
                    break;
                default:
                    break;
            }

            /* padding-top
            val = css_computed_padding_top(style, &len1, &unit1);
            switch (val)
            {
                case CSS_PADDING_SET:
                    sw.Write("padding-top: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* padding-right
            val = css_computed_padding_right(style, &len1, &unit1);
            switch (val)
            {
                case CSS_PADDING_SET:
                    sw.Write("padding-right: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* padding-bottom
            val = css_computed_padding_bottom(style, &len1, &unit1);
            switch (val)
            {
                case CSS_PADDING_SET:
                    sw.Write("padding-bottom: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            /* padding-left
            val = css_computed_padding_left(style, &len1, &unit1);
            switch (val)
            {
                case CSS_PADDING_SET:
                    sw.Write("padding-left: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }
            */
            // position
            var val200 = ComputedPosition();
            switch (val200)
            {
                case CssPosition.CSS_POSITION_STATIC:
                    sw.Write("position: static ");
                    break;
                case CssPosition.CSS_POSITION_RELATIVE:
                    sw.Write("position: relative ");
                    break;
                case CssPosition.CSS_POSITION_ABSOLUTE:
                    sw.Write("position: absolute ");
                    break;
                case CssPosition.CSS_POSITION_FIXED:
                    sw.Write("position: fixed ");
                    break;
                default:
                    break;
            }

            /* quotes
            val = css_computed_quotes(style, &string_list);
            if (val == CSS_QUOTES_STRING && string_list != NULL)
            {
                sw.Write("quotes:");

                while (*string_list != NULL)
                {
                    fprintf(stream, " \"%.*s\"",
                        (int)lwc_string_length(*string_list),
                        lwc_string_data(*string_list));

                    string_list++;
                }

                sw.Write(" ");
            }
            else
            {
                switch (val)
                {
                    case CSS_QUOTES_NONE:
                        sw.Write("quotes: none ");
                        break;
                    default:
                        break;
                }
            }

            /* right
            val = css_computed_right(style, &len1, &unit1);
            switch (val)
            {
                case CSS_RIGHT_AUTO:
                    sw.Write("right: auto ");
                    break;
                case CSS_RIGHT_SET:
                    sw.Write("right: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            // table-layout
            val = css_computed_table_layout(style);
            switch (val)
            {
                case CSS_TABLE_LAYOUT_AUTO:
                    sw.Write("table-layout: auto ");
                    break;
                case CSS_TABLE_LAYOUT_FIXED:
                    sw.Write("table-layout: fixed ");
                    break;
                default:
                    break;
            }

            // text-align
            val = css_computed_text_align(style);
            switch (val)
            {
                case CSS_TEXT_ALIGN_LEFT:
                    sw.Write("text-align: left ");
                    break;
                case CSS_TEXT_ALIGN_RIGHT:
                    sw.Write("text-align: right ");
                    break;
                case CSS_TEXT_ALIGN_CENTER:
                    sw.Write("text-align: center ");
                    break;
                case CSS_TEXT_ALIGN_JUSTIFY:
                    sw.Write("text-align: justify ");
                    break;
                case CSS_TEXT_ALIGN_DEFAULT:
                    sw.Write("text-align: default ");
                    break;
                case CSS_TEXT_ALIGN_LIBCSS_LEFT:
                    sw.Write("text-align: -libcss-left ");
                    break;
                case CSS_TEXT_ALIGN_LIBCSS_CENTER:
                    sw.Write("text-align: -libcss-center ");
                    break;
                case CSS_TEXT_ALIGN_LIBCSS_RIGHT:
                    sw.Write("text-align: -libcss-right ");
                    break;
                default:
                    break;
            }

            // text-decoration
            val = css_computed_text_decoration(style);
            if (val == CSS_TEXT_DECORATION_NONE)
            {
                sw.Write("text-decoration: none ");
            }
            else
            {
                sw.Write("text-decoration:");

                if (val & CSS_TEXT_DECORATION_BLINK)
                {
                    sw.Write(" blink");
                }
                if (val & CSS_TEXT_DECORATION_LINE_THROUGH)
                {
                    sw.Write(" line-through");
                }
                if (val & CSS_TEXT_DECORATION_OVERLINE)
                {
                    sw.Write(" overline");
                }
                if (val & CSS_TEXT_DECORATION_UNDERLINE)
                {
                    sw.Write(" underline");
                }

                sw.Write(" ");
            }

            // text-indent
            val = css_computed_text_indent(style, &len1, &unit1);
            switch (val)
            {
                case CSS_TEXT_INDENT_SET:
                    sw.Write("text-indent: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            // text-transform
            val = css_computed_text_transform(style);
            switch (val)
            {
                case CSS_TEXT_TRANSFORM_CAPITALIZE:
                    sw.Write("text-transform: capitalize ");
                    break;
                case CSS_TEXT_TRANSFORM_UPPERCASE:
                    sw.Write("text-transform: uppercase ");
                    break;
                case CSS_TEXT_TRANSFORM_LOWERCASE:
                    sw.Write("text-transform: lowercase ");
                    break;
                case CSS_TEXT_TRANSFORM_NONE:
                    sw.Write("text-transform: none ");
                    break;
                default:
                    break;
            }

            // top
            val = css_computed_top(style, &len1, &unit1);
            switch (val)
            {
                case CSS_TOP_AUTO:
                    sw.Write("top: auto ");
                    break;
                case CSS_TOP_SET:
                    sw.Write("top: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            // unicode-bidi
            val = css_computed_unicode_bidi(style);
            switch (val)
            {
                case CSS_UNICODE_BIDI_NORMAL:
                    sw.Write("unicode-bidi: normal ");
                    break;
                case CSS_UNICODE_BIDI_EMBED:
                    sw.Write("unicode-bidi: embed ");
                    break;
                case CSS_UNICODE_BIDI_BIDI_OVERRIDE:
                    sw.Write("unicode-bidi: bidi-override ");
                    break;
                default:
                    break;
            }

            // vertical-align
            val = css_computed_vertical_align(style, &len1, &unit1);
            switch (val)
            {
                case CSS_VERTICAL_ALIGN_BASELINE:
                    sw.Write("vertical-align: baseline ");
                    break;
                case CSS_VERTICAL_ALIGN_SUB:
                    sw.Write("vertical-align: sub ");
                    break;
                case CSS_VERTICAL_ALIGN_SUPER:
                    sw.Write("vertical-align: super ");
                    break;
                case CSS_VERTICAL_ALIGN_TOP:
                    sw.Write("vertical-align: top ");
                    break;
                case CSS_VERTICAL_ALIGN_TEXT_TOP:
                    sw.Write("vertical-align: text-top ");
                    break;
                case CSS_VERTICAL_ALIGN_MIDDLE:
                    sw.Write("vertical-align: middle ");
                    break;
                case CSS_VERTICAL_ALIGN_BOTTOM:
                    sw.Write("vertical-align: bottom ");
                    break;
                case CSS_VERTICAL_ALIGN_TEXT_BOTTOM:
                    sw.Write("vertical-align: text-bottom ");
                    break;
                case CSS_VERTICAL_ALIGN_SET:
                    sw.Write("vertical-align: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            // visibility
            val = css_computed_visibility(style);
            switch (val)
            {
                case CSS_VISIBILITY_VISIBLE:
                    sw.Write("visibility: visible ");
                    break;
                case CSS_VISIBILITY_HIDDEN:
                    sw.Write("visibility: hidden ");
                    break;
                case CSS_VISIBILITY_COLLAPSE:
                    sw.Write("visibility: collapse ");
                    break;
                default:
                    break;
            }
            */
            // white-space
            var val299 = ComputedWhitespace();
            switch (val299)
            {
                case CssWhiteSpaceEnum.CSS_WHITE_SPACE_NORMAL:
                    sw.Write("white-space: normal ");
                    break;
                case CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE:
                    sw.Write("white-space: pre ");
                    break;
                case CssWhiteSpaceEnum.CSS_WHITE_SPACE_NOWRAP:
                    sw.Write("white-space: nowrap ");
                    break;
                case CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE_WRAP:
                    sw.Write("white-space: pre-wrap ");
                    break;
                case CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE_LINE:
                    sw.Write("white-space: pre-line ");
                    break;
                default:
                    break;
            }

            // width
            var val300 = ComputedWidth(ref len1, ref unit1);
            switch (val300)
            {
                case CssWidth.CSS_WIDTH_AUTO:
                    sw.Write("width: auto ");
                    break;
                case CssWidth.CSS_WIDTH_SET:
                    sw.Write("width: ");

                    sw.Write(DumpCssUnit(len1, unit1));

                    sw.Write(" ");
                    break;
                default:
                    break;
            }
            /*
            // word-spacing
            val = css_computed_word_spacing(style, &len1, &unit1);
            switch (val)
            {
                case CSS_WORD_SPACING_NORMAL:
                    sw.Write("word-spacing: normal ");
                    break;
                case CSS_WORD_SPACING_SET:
                    sw.Write("word-spacing: ");

                    dump_css_unit(stream, len1, unit1);

                    sw.Write(" ");
                    break;
                default:
                    break;
            }

            // z-index
            val = css_computed_z_index(style, &zindex);
            switch (val)
            {
                case CSS_Z_INDEX_AUTO:
                    sw.Write("z-index: auto ");
                    break;
                case CSS_Z_INDEX_SET:
                    fprintf(stream, "z-index: %d ", zindex);
                    break;
                default:
                    break;
            }
            */
            sw.Write("}");
        }
    }

    public class CssSelectResults
    {
        public ComputedStyle[] Styles = new ComputedStyle[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_COUNT];
    }
}
