using System;
using System.Collections.Generic;
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
    }

    public class CssSelectResults
    {
        public ComputedStyle[] Styles = new ComputedStyle[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_COUNT];
    }
}
