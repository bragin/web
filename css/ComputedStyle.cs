using SkiaSharpOpenGLBenchmark.css;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark
{
    public struct Color : IEquatable<Color>
    {
        uint value;

        public static bool operator ==(Color a, Color b)
        {
            return a.value == b.value;
        }

        public static bool operator !=(Color a, Color b)
        {
            return a.value != b.value;
        }

        public bool Equals(Color other)
        {
            return value == other.value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Color?;

            if (other != null) return Equals(other.Value);

            return false;
        }

        public override string ToString()
        {
            return value.ToString("X");
        }

        public Color(uint nv)
        {
            value = nv;
        }
    }

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
        int column_count;
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


    public class ComputedStyle
    {
        public ComputedStyleI i;
        int count;
        uint bin;

        public ComputedStyle()
        {
            bin = UInt32.MaxValue;
            i = new ComputedStyleI();
        }

        private delegate byte GetForAbsLen(ComputedStyle style, out Fixed len1, out CssUnit unit1, out Fixed len2, out CssUnit unit2);

        private delegate void SetForAbsLen(ComputedStyle style, byte type, Fixed len1, CssUnit unit1, Fixed len2, CssUnit unit2);

        public void SetFontSize(CssFontSizeEnum type, Fixed length, CssUnit unit)
        {
            const uint FONT_SIZE_INDEX = 12;
            const byte FONT_SIZE_SHIFT = 1;
            const uint FONT_SIZE_MASK = 0x3fe;

            uint bits = i.bits[FONT_SIZE_INDEX];

            // 9bits: uuuuutttt : unit | type
            i.bits[FONT_SIZE_INDEX] =
                (bits & ~FONT_SIZE_MASK) | ((((uint)type & 0xf) | ((uint)unit << 4)) << FONT_SIZE_SHIFT);

            i.font_size = length;
        }

        // autogenerated_propget.h:880
        public byte GetDisplay()
        {
            const uint DISPLAY_INDEX = 3;
            const byte DISPLAY_SHIFT = 0;
            const uint DISPLAY_MASK = 0x1f;

            uint bits = i.bits[DISPLAY_INDEX];
            bits &= DISPLAY_MASK;
            bits >>= DISPLAY_SHIFT;


            /* 5bits: ttttt : type */
            return (byte)(bits & 0x1f);
        }

        // autogenerated_props:1012
        byte GetFloat()
        {
            const uint FLOAT_INDEX = 11;
            const byte FLOAT_SHIFT = 30;
            const uint FLOAT_MASK = 0xc0000000;

            uint bits = i.bits[FLOAT_INDEX];
            bits &= FLOAT_MASK;
            bits >>= FLOAT_SHIFT;

            /* 2bits: tt : type */

            return (byte)(bits & 0x3);
        }

        // autogenerated_propget.h:1048
        public byte GetFontSize(ref Fixed length, ref CssUnit unit)
        {
            const uint FONT_SIZE_INDEX = 12;
            const byte FONT_SIZE_SHIFT = 1;
            const uint FONT_SIZE_MASK = 0x3fe;

            uint bits = i.bits[FONT_SIZE_INDEX];
            bits &= FONT_SIZE_MASK;
            bits >>= FONT_SIZE_SHIFT;

            // 9bits: uuuuutttt : unit | type
            if ((bits & 0xf) == (byte)CssFontSizeEnum.CSS_FONT_SIZE_DIMENSION)
            {
                length = i.font_size;
                unit = (CssUnit)(bits >> 4);
            }

            return (byte)(bits & 0xf);
        }

        // autogenerated_props:1765
        byte GetPosition()
        {
            const uint POSITION_INDEX = 10;
            const byte POSITION_SHIFT = 20;
            const uint POSITION_MASK = 0x700000;

            uint bits = i.bits[POSITION_INDEX];
            bits &= POSITION_MASK;
            bits >>= POSITION_SHIFT;

            /* 3bits: ttt : type */

            return (byte)(bits & 0x7);
        }

        // autogenerated_props.h:121
        byte GetBackgroundPosition(out Fixed len1, out CssUnit unit1, out Fixed len2, out CssUnit unit2)
        {
            const uint BACKGROUND_POSITION_INDEX = 12;
            const byte BACKGROUND_POSITION_SHIFT = 10;
            const uint BACKGROUND_POSITION_MASK = 0x1ffc00;

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

            return (byte)(bits & 0x1);
        }

        // autogenerated_propset.h:155
        void SetBackgroundPosition(byte type, Fixed len1, CssUnit unit1, Fixed len2, CssUnit unit2)
        {
            const uint BACKGROUND_POSITION_INDEX = 12;
            const byte BACKGROUND_POSITION_SHIFT = 10;
            const uint BACKGROUND_POSITION_MASK = 0x1ffc00;

            var bits = i.bits[BACKGROUND_POSITION_INDEX];

            /* 11bits: aaaaabbbbbt : unit_a | unit_b | type */
            i.bits[BACKGROUND_POSITION_INDEX] =
                (bits & ~BACKGROUND_POSITION_MASK) | ((((uint)type & 0x1)
                    | ((uint)unit1 << 1) | ((uint)unit2 << 6)) << BACKGROUND_POSITION_SHIFT);

            i.background_position_a = len1;
            i.background_position_b = len2;
        }

        // computed.c:1094
        public void ComputeAbsoluteValues(ComputedStyle parent, CssUnitCtx unitCtx)
        {
            CssHint psize = null, size = new CssHint(), ex_size = new CssHint();
            //css_error error;

            size.Status = GetFontSize(ref size.Length.Value, ref size.Length.Unit);

            // Get reference font-size for relative sizes.
            if (parent != null)
            {
                psize = new CssHint();
                psize.Status = parent.GetFontSize(ref psize.Length.Value, ref psize.Length.Unit);
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


        #region Property Accessors

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

        // computed.c:771
        public CssPosition ComputedPosition()
        {
            return (CssPosition)GetPosition();
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
                    type = GetBackgroundPosition(out length1, out unit1, out length2, out unit2);
                    break;
                case "border_spacing":
                    //type = GetBackgroundPosition(out length1, out unit1, out length2, out unit2);
                    Log.Unimplemented("border_spacing");
                    type = 0xff;
                    break;
                default:
                    throw new Exception("Unsupported type");
            }

            if (type != (byte)CssBackgroundPositionEnum.CSS_BACKGROUND_POSITION_SET)
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
    }

    public class CssSelectResults
    {
        public ComputedStyle[] Styles = new ComputedStyle[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_COUNT];
    }
}
