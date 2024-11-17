using HtmlParserSharp;
using OpenTK;
using SkiaSharpOpenGLBenchmark.css;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SkiaSharpOpenGLBenchmark
{
    public enum BoxType : byte
    {
        BOX_BLOCK,
        BOX_INLINE_CONTAINER,
        BOX_INLINE,
        BOX_TABLE,
        BOX_TABLE_ROW,
        BOX_TABLE_CELL,
        BOX_TABLE_ROW_GROUP,
        BOX_FLOAT_LEFT,
        BOX_FLOAT_RIGHT,
        BOX_INLINE_BLOCK,
        BOX_BR,
        BOX_TEXT,
        BOX_INLINE_END,
        BOX_NONE,
        BOX_FLEX,
        BOX_INLINE_FLEX
    }

    public enum BoxFlags : int
    {
        NEW_LINE = 1 << 0,          // first inline on a new line
        STYLE_OWNED = 1 << 1,       // style is owned by this box
        PRINTED = 1 << 2,           // box has already been printed
        PRE_STRIP = 1 << 3,         // PRE tag needing leading newline stripped
        CLONE = 1 << 4,             // continuation of previous box from wrapping
        MEASURED = 1 << 5,          // text box width has been measured
        HAS_HEIGHT = 1 << 6,        // box has height (perhaps due to children)
        MAKE_HEIGHT = 1 << 7,       // box causes its own height
        NEED_MIN = 1 << 8,          // minimum width is required for layout
        REPLACE_DIM = 1 << 9,       // replaced element has given dimensions
        IFRAME = 1 << 10,           // box contains an iframe
        CONVERT_CHILDREN = 1 << 11, // wanted children converting
        IS_REPLACED = 1 << 12	    // box is a replaced element
    }

    // Transient properties for construction of current node
    public struct BoxConstructProps
    {
        // Style from which to inherit, or NULL if none
        public ComputedStyle ParentStyle;
        // Current link target, or NULL if none
        //struct nsurl *href;
        // Current frame target, or NULL if none
        public string Target;
        // Current title attribute, or NULL if none
        public string Title;
        // Identity of the current block-level container
        public Box ContainingBlock;
        // Current container for inlines, or NULL if none
        // \note If non-NULL, will be the last child of containing_block
        public Box InlineContainer;
        // Whether the current node is the root of the DOM tree
        public bool NodeIsRoot;
	};

	// Sides of a box
	public enum BoxSide : byte
    {
        TOP = 0,
        RIGHT = 1,
        BOTTOM = 2,
        LEFT = 3
    };

	// Container for box border details
	// box.h:102
	public struct BoxBorder
	{
		public CssBorderStyleEnum Style;   // border-style
		public Color BorderColor;            // border-color value
		public int Width;          // border-width (pixels)
	}

	// Node in box tree. All dimensions are in pixels
	public class Box
	{
        // Type of box
        public BoxType Type;

        // Box flags
        public BoxFlags Flags;

        // DOM node that generated this box or NULL
        public XmlNode Node;

        // Computed styles for elements and their pseudo elements.
        // NULL on non-element boxes.
        public CssSelectResults Styles;

        // Style for this box. 0 for INLINE_CONTAINER and
        //  FLOAT_*. Pointer into a box's 'styles' select results,
        //  except for implied boxes, where it is a pointer to an
        //  owned computed style.
        public ComputedStyle Style;

        // value of id attribute (or name for anchors)
        public string Id;

        // Next sibling box, or NULL.
        public Box Next;

        // Previous sibling box, or NULL.
        Box Prev;

        // First child box, or NULL.
        public Box Children;

        // Last child box, or NULL.
        public Box Last;

        // Parent box, or NULL.
        public Box Parent;

        // INLINE_END box corresponding to this INLINE box, or INLINE
        // box corresponding to this INLINE_END box.
        public Box InlineEnd;


        // First float child box, or NULL. Float boxes are in the tree
        // twice, in this list for the block box which defines the
        // area for floats, and also in the standard tree given by
        // children, next, prev, etc.
        public Box float_children;

        // Next sibling float box.
        public Box next_float;

        // If box is a float, points to box's containing block
        public Box float_container;

        // Level below which subsequent floats must be cleared.  This
        // is used only for boxes with float_children
        int clear_level;

        // Level below which floats have been placed.
        int cached_place_below_level;

        // Coordinate of left padding edge relative to parent box, or
        // relative to ancestor that contains this box in
        // float_children for FLOAT_.
        public int X;

        // Coordinate of top padding edge, relative as for x
        public int Y;

		// Width of content box (excluding padding etc.)
		public int Width;

		// Height of content box (excluding padding etc.)
		public int Height;

		/* These four variables determine the maximum extent of a box's
         * descendants. They are relative to the x,y coordinates of the box.
         *
         * Their use depends on the overflow CSS property:
         *
         * Overflow:	Usage:
         * visible	The content of the box is displayed within these dimensions
         * hidden	These are ignored. Content is plotted within the box dimensions
         * scroll	These are used to determine the extent of the scrollable area
         * auto		As "scroll"
         */
		public int DescendantX0;  // left edge of descendants
		public int DescendantY0;  // top edge of descendants
		public int DescendantX1;  // right edge of descendants
		public int DescendantY1;  // bottom edge of descendants

		// Margin: TOP, RIGHT, BOTTOM, LEFT.
		public int[] Margin = new int[4];

        // Padding: TOP, RIGHT, BOTTOM, LEFT.
        public int[] Padding = new int[4];

        // Border: TOP, RIGHT, BOTTOM, LEFT.
        public BoxBorder[] Border = new BoxBorder[4];

        // Horizontal scroll.
        public Scrollbar ScrollX;

        // Vertical scroll.
        public Scrollbar ScrollY;

        // Width of box taking all line breaks (including margins etc). Must be non-negative
        int MinWidth;

        // Width that would be taken with no line breaks. Must be non-negative
        public int MaxWidth;

        // Text, or NULL if none. Unterminated.
        public string Text;

        // Length of text.
        public int Length;

        // Width of space after current text (depends on font and size).
        public int Space;

        // Byte offset within a textual representation of this content.
        //size_t byte_offset;


        // Link, or NULL.
        //struct nsurl *href;

        // Link target, or NULL.
        public string Target;

        // Title, or NULL.
        public string Title;


        // Number of columns for TABLE / TABLE_CELL.
        int columns;

        // Number of rows for TABLE only.
        int rows;

        // Start column for TABLE_CELL only.
        int start_column;

        // Array of table column data for TABLE only.
        //struct column *col;

        // List item value
        int list_value;

        // List marker box if this is a list-item, or NULL.
        //struct box *list_marker;


        // Form control data, or NULL if not a form control.
        //struct form_control* gadget;

        // (Image)map to use with this object, or NULL if none
        //char* usemap;


        // Background image for this box, or NULL if none
        //struct hlcache_handle *background;
        public int Background;


        // Object in this box (usually an image), or NULL if none.
        //struct hlcache_handle* object;

        // Parameters for the object, or NULL.
        // struct object_params *object_params;


        // Iframe's browser_window, or NULL if none
        //struct browser_window *iframe;

        // FIXME: Temporary and should be moved away
        int SCROLLBAR_WIDTH = 16;

        // box_manipulate.c:92 - box_create()
        public Box(CssSelectResults styles,
                    ComputedStyle style,
                    bool style_owned,
                    /*struct nsurl */int href,
                    string target, string title, string id)
        {
            this.Type = BoxType.BOX_INLINE;
            this.Flags = 0; //box->flags = style_owned ? (box->flags | STYLE_OWNED) : box->flags;
            Styles = styles;
            Style = style;
            this.X = this.Y = 0;
            this.Width = Int32.MaxValue;
            this.Height = 0;
            this.DescendantX0 = this.DescendantY0 = 0;
            this.DescendantX1 = this.DescendantY1 = 0;
            for (int i = 0; i != 4; i++)
            {
                this.Margin[i] = this.Padding[i] = 0;
                this.Border[i].Width = 0;
            }
            //this.scroll_x = this.scroll_y = NULL;
            this.MinWidth = 0;
            this.MaxWidth = Int32.MaxValue;
            //box->byte_offset = 0;
            //box->text = NULL;
            //box->length = 0;
            Space = 0;
            //box->href = (href == NULL) ? NULL : nsurl_ref(href);
            //box->target = target;
            //box->title = title;
            this.columns = 1;
            this.rows = 1;
            this.start_column = 0;
            //box->next = NULL;
            //box->prev = NULL;
            //box->children = NULL;
            this.Last = null;
            //box->parent = NULL;
            //box->inline_end = NULL;
            //box->float_children = NULL;
            //box->float_container = NULL;
            //box->next_float = NULL;
            //box->cached_place_below_level = 0;
            //box->list_value = 1;
            //box->list_marker = NULL;
            //box->col = NULL;
            //box->gadget = NULL;
            //box->usemap = NULL;
            //box->id = id;
            Background = 0;
            //box->object = NULL;
            //box->object_params = NULL;
            //box->iframe = NULL;
            //box->node = NULL;

            ScrollX = new Scrollbar();
            ScrollY = new Scrollbar();
        }

        /**
         * mapping from CSS display to box type this table must be in sync
         * with libcss' css_display enum
         */
        // box_construct.c:93
        static public BoxType BoxMap(CssDisplay disp)
        {
            switch (disp)
            {
                case CssDisplay.CSS_DISPLAY_INHERIT:
                    return BoxType.BOX_BLOCK;
                case CssDisplay.CSS_DISPLAY_INLINE:
                    return BoxType.BOX_INLINE;
                case CssDisplay.CSS_DISPLAY_BLOCK:
                    return BoxType.BOX_BLOCK;
                case CssDisplay.CSS_DISPLAY_LIST_ITEM:
                    return BoxType.BOX_BLOCK;
                case CssDisplay.CSS_DISPLAY_RUN_IN:
                    return BoxType.BOX_INLINE;
                case CssDisplay.CSS_DISPLAY_INLINE_BLOCK:
                    return BoxType.BOX_INLINE_BLOCK;
                case CssDisplay.CSS_DISPLAY_TABLE:
                    return BoxType.BOX_TABLE;
                case CssDisplay.CSS_DISPLAY_INLINE_TABLE:
                    return BoxType.BOX_TABLE;
                case CssDisplay.CSS_DISPLAY_TABLE_ROW_GROUP:
                    return BoxType.BOX_TABLE_ROW_GROUP;
                case CssDisplay.CSS_DISPLAY_TABLE_HEADER_GROUP:
                    return BoxType.BOX_TABLE_ROW_GROUP;
                case CssDisplay.CSS_DISPLAY_TABLE_FOOTER_GROUP:
                    return BoxType.BOX_TABLE_ROW_GROUP;
                case CssDisplay.CSS_DISPLAY_TABLE_ROW:
                    return BoxType.BOX_TABLE_ROW;
                case CssDisplay.CSS_DISPLAY_TABLE_COLUMN_GROUP:
                    return BoxType.BOX_NONE;
                case CssDisplay.CSS_DISPLAY_TABLE_COLUMN:
                    return BoxType.BOX_NONE;
                case CssDisplay.CSS_DISPLAY_TABLE_CELL:
                    return BoxType.BOX_TABLE_CELL;
                case CssDisplay.CSS_DISPLAY_TABLE_CAPTION:
                    return BoxType.BOX_INLINE;
                case CssDisplay.CSS_DISPLAY_NONE:
                    return BoxType.BOX_NONE;
                default:
                    Debug.Assert(false); // must never happen
                    return BoxType.BOX_BLOCK;
            }
        }

        // box_construct.c:153 - box_extract_properties()
        static public BoxConstructProps ExtractProperties(XmlNode node, BoxTree bt)
        {
            BoxConstructProps props = new BoxConstructProps();
            props.NodeIsRoot = BoxTree.BoxIsRoot(node);

            // Extract properties from containing DOM node
            if (props.NodeIsRoot == false)
            {
                XmlNode current_node = node;
                XmlNode parent_node = null;
                Box parent_box;

                // Find ancestor node containing parent box
                while (true)
                {
                    parent_node = current_node.ParentNode;

                    parent_box = bt.BoxForNode(parent_node);

                    if (parent_box != null)
                    {
                        props.ParentStyle = parent_box.Style;
                        //props.href = parent_box.href;
                        props.Target = parent_box.Target;
                        props.Title = parent_box.Title;

                        break;
                    }
                    else
                    {
                        current_node = parent_node;
                        parent_node = null;
                    }
                }

                // Find containing block (may be parent)
                while (true)
                {
                    parent_node = current_node.ParentNode;
                    if (parent_node == null)
                        break;

                    var b = bt.BoxForNode(parent_node);

                    // Children of nodes that created an inline box
                    // will generate boxes which are attached as
                    // _siblings_ of the box generated for their
                    // parent node. Note, however, that we'll still
                    // use the parent node's styling as the parent
                    // style, above.
                    if (b != null && b.Type != BoxType.BOX_INLINE && b.Type != BoxType.BOX_BR)
                    {
                        props.ContainingBlock = b;
                        break;
                    }
                    else
                    {
                        current_node = parent_node;
                        parent_node = null;
                    }
                }
            }

            // Compute current inline container, if any
            if (props.ContainingBlock != null &&
                props.ContainingBlock.Last != null &&
                props.ContainingBlock.Last.Type == BoxType.BOX_INLINE_CONTAINER)
            {
                props.InlineContainer = props.ContainingBlock.Last;
            }

            return props;
        }

        public void AttachToNode(XmlNode n)
        {
            Node = n;
        }

        // box_manipulate.c:163 - box_add_child()
        public void AddChild(Box child)
        {
            // parent = this
            if (this.Children != null)
            {   /* has children already */
                this.Last.Next = child;
                child.Prev = this.Last;
            }
            else
            {           /* this is the first child */
                this.Children = child;
                child.Prev = null;
            }

            this.Last = child;
            child.Parent = this;
        }

        // box_is_first_child()
        // box_inspect.h:138
        // Check if layout box is a first child.
        public bool IsFirstChild()
        {
            return (Parent == null || this == Parent.Children);
        }

        #region Layout

        // layout_text_indent()
        // layout.c:250
        /**
         * Calculate the text-indent length.
         *
         * \param  style  style of block
         * \param  width  width of containing block
         * \return  length of indent
         */
        static int LayoutTextIndent(
		        CssUnitCtx unit_len_ctx,
		        ComputedStyle style, int width)
        {
	        Fixed value = Fixed.F_0;
	        CssUnit unit = CssUnit.CSS_UNIT_PX;

            style.ComputedTextIndent(ref value, ref unit);

	        if (unit == CssUnit.CSS_UNIT_PCT) {
		        return value.PercentageToInt(width);
	        } else {
		        return unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
	        }
        }


        /**
         * Determine width of margin, borders, and padding on one side of a box.
         *
         * \param unit_len_ctx  CSS length conversion context for document
         * \param style    style to measure
         * \param side     side of box to measure
         * \param margin   whether margin width is required
         * \param border   whether border width is required
         * \param padding  whether padding width is required
         * \param fixed    increased by sum of fixed margin, border, and padding
         * \param frac     increased by sum of fractional margin and padding
         */
        // layout.c:286
        void CalculateMbpWidth(CssUnitCtx unit_len_ctx,
                    ComputedStyle style,
                    uint side,
                    bool margin,
                    bool border,
                    bool padding,
                    ref int fixedPart,
                    ref double frac)
        {
            Fixed value = Fixed.F_0;
            CssUnit unit = CssUnit.CSS_UNIT_PX;

            Debug.Assert(style != null);

            // margin
            if (margin)
            {
                CssMarginEnum type;

                type = style.ComputedMarginSides[side](ref value, ref unit);
                if (type == CssMarginEnum.CSS_MARGIN_SET)
                {
                    if (unit == CssUnit.CSS_UNIT_PCT)
                    {
                        frac += (value / Fixed.F_100).ToInt();
                    }
                    else
                    {
                        fixedPart += unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
                    }
                }
            }

	        // border
	        if (border)
            {
		        if (style.ComputedBorderSideStyle[side]() != CssBorderStyleEnum.CSS_BORDER_STYLE_NONE)
                {
                    style.ComputedBorderSideWidth[side](ref value, ref unit);

			        fixedPart += unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
		        }
	        }

	        // padding
	        if (padding)
            {
                style.ComputedPaddingSide[side](ref value, ref unit);
		        if (unit == CssUnit.CSS_UNIT_PCT)
                {
			        frac += (value / Fixed.F_100).ToInt();
		        } else
                {
                    fixedPart += unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
		        }
	        }
        }

        // layout.c:531
        bool HasPercentageMaxWidth()
        {
            CssUnit unit = CssUnit.CSS_UNIT_PX;

            Fixed value = Fixed.F_0;

            var type = Style.ComputedMaxWidth(ref value, ref unit);
            return ((type == CssMaxWidthEnum.CSS_MAX_WIDTH_SET) && (unit == CssUnit.CSS_UNIT_PCT));
        }

        /**
         * Calculate minimum and maximum width of a line.
         *
         * \param first       a box in an inline container
         * \param line_min    updated to minimum width of line starting at first
         * \param line_max    updated to maximum width of line starting at first
         * \param first_line  true iff this is the first line in the inline container
         * \param line_has_height  updated to true or false, depending on line
         * \param font_func Font functions.
         * \return  first box in next line, or 0 if no more lines
         * \post  0 <= *line_min <= *line_max
         */
        // layout.c:562
        Box LayoutMinmaxLine(Box first,
		           ref int line_min,
		           ref int line_max,
		           bool first_line,
		           ref bool line_has_height,
		           /*const struct gui_layout_table *font_func*/
		           HtmlContent content)
        {
	        int min = 0, max = 0, width = 0, height, fixedSize;
	        double frac;
	        int i, j;
	        Box b;
	        Box block;
	        PlotFontStyle fstyle;
	        bool no_wrap;

	        Debug.Assert(first.Parent != null);
            Debug.Assert(first.Parent.Parent != null);
            Debug.Assert(first.Parent.Parent.Style != null);

	        block = first.Parent.Parent;
	        no_wrap = (block.Style.ComputedWhitespace() == CssWhiteSpaceEnum.CSS_WHITE_SPACE_NOWRAP ||
                    block.Style.ComputedWhitespace() == CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE);

	        line_has_height = false;

	        // corresponds to the pass 1 loop in layout_line()
	        for (b = first; b != null; b = b.Next)
            {
		        CssWidth wtype;
		        CssHeightEnum htype;
		        CssBoxSizingEnum bs;
		        Fixed value = Fixed.F_0;
		        CssUnit unit = CssUnit.CSS_UNIT_PX;

                Debug.Assert(b.Type == BoxType.BOX_INLINE || b.Type == BoxType.BOX_INLINE_BLOCK ||
				        b.Type == BoxType.BOX_FLOAT_LEFT ||
				        b.Type == BoxType.BOX_FLOAT_RIGHT ||
				        b.Type == BoxType.BOX_BR || b.Type == BoxType.BOX_TEXT ||
				        b.Type == BoxType.BOX_INLINE_END);

		        Log.Print(LogChannel.Layout, $"{b.GetHashCode()}: min {min}, max {max}");


                if (b.Type == BoxType.BOX_BR) {
			        b = b.Next;
			        break;
		        }

		        if (b.Type == BoxType.BOX_FLOAT_LEFT || b.Type == BoxType.BOX_FLOAT_RIGHT) {
			        Debug.Assert(b.Children != null);
                    if (b.Children.Type == BoxType.BOX_BLOCK)
                        b.Children.LayoutMinmaxBlock(/*font_func,*/ content);
                    else
                    {
                        //layout_minmax_table(b.Children, font_func, content);
                        Log.Unimplemented();
                    }

			        b.MinWidth = b.Children.MinWidth;
			        b.MaxWidth = b.Children.MaxWidth;
			        if (min < b.MinWidth)
				        min = b.MinWidth;
			        max += b.MaxWidth;
			        continue;
		        }

		        if (b.Type == BoxType.BOX_INLINE_BLOCK) {
			        b.LayoutMinmaxBlock(/*font_func,*/ content);
			        if (min < b.MinWidth)
				        min = b.MinWidth;
			        max += b.MaxWidth;

			        if (((int)b.Flags & (int)BoxFlags.HAS_HEIGHT) != 0)
				        line_has_height = true;
			        continue;
		        }

		        Debug.Assert(b.Style != null);
                fstyle = b.Style.FontPlotStyle(content.UnitLenCtx);

		        /*if (b.Type == BoxType.BOX_INLINE && !b->object &&
				        !(b.Flags & REPLACE_DIM) &&
				        !(b.Flags & IFRAME)) {
			        fixed = frac = 0;
			        calculate_mbp_width(&content->unit_len_ctx,
					        b->style, LEFT, true, true, true,
					        &fixed, &frac);
			        if (!b->inline_end)
				        calculate_mbp_width(&content->unit_len_ctx,
						        b->style, RIGHT,
						        true, true, true,
						        &fixed, &frac);
			        if (0 < fixed)
				        max += fixed;
			        *line_has_height = true;
			        // \todo  update min width, consider fractional extra
		        } else*/ if (b.Type == BoxType.BOX_INLINE_END)
                {
			        fixedSize = 0;
                    frac = 0;
			        CalculateMbpWidth(content.UnitLenCtx,
					        b.InlineEnd.Style, (uint)BoxSide.RIGHT,
					        true, true, true,
					        ref fixedSize, ref frac);
			        if (0 < fixedSize)
				        max += fixedSize;

			        if (b.Next != null)
                    {
				        if (b.Space == int.MaxValue)
                        {
                            //font_func->width(&fstyle, " ", 1, &b.Space);
                            Log.Unimplemented("font_func.Width()");
				        }
				        max += b.Space;
			        }

			        line_has_height = true;
			        continue;
		        }

		        if (/*!b->object &&*/
                    (((int)b.Flags & (int) BoxFlags.IFRAME) == 0) &&
                    /*!b.Gadget &&*/ (((int)b.Flags & (int)BoxFlags.REPLACE_DIM) == 0))
                {
			        // inline non-replaced, 10.3.1 and 10.6.1
			        bool no_wrap_box;
			        if (string.IsNullOrEmpty(b.Text))
				        continue;

			        no_wrap_box = (b.Style.ComputedWhitespace() == CssWhiteSpaceEnum.CSS_WHITE_SPACE_NOWRAP ||
                            b.Style.ComputedWhitespace() ==  CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE);

                    if (b.Width == int.MaxValue)
                    {
				        /** \todo handle errors */

				        /* If it's a select element, we must use the
				         * width of the widest option text */
                        /*
				        if (b.Parent.Parent.Gadget && b->parent->parent->gadget.Type == GADGET_SELECT)
                        {
					        int opt_maxwidth = 0;
					        struct form_option *o;

					        for (o = b->parent->parent->gadget->data.select.items; o; o = o.Next) {
						        int opt_width;
						        font_func->width(&fstyle,
								        o.Text,
								        strlen(o.Text),
								        &opt_width);

						        if (opt_maxwidth < opt_width)
							        opt_maxwidth =opt_width;
					        }

					        b->width = opt_maxwidth;
					        if (nsoption_bool(core_select_menu))
						        b->width += SCROLLBAR_WIDTH;

				        } else*/ {
                            //font_func->width(&fstyle, b.Text, b->length, &b->width);
                            content.Plot.GetFontWidth(fstyle, b.Text, ref b.Width);
					        b.Flags |= BoxFlags.MEASURED;
				        }
			        }
			        max += b.Width;
			        if (b.Next != null)
                    {
				        if (b.Space == int.MaxValue)
                        {
                            //font_func->width(&fstyle, " ", 1, &b.Space);
                            content.Plot.GetFontWidth(fstyle, " ", ref b.Space);
                        }
				        max += b.Space;
			        }

			        if (no_wrap) {
				        /* Don't wrap due to block style,
				         * so min is the same as max */
				        min = max;

			        } else if (no_wrap_box) {
				        /* This inline box can't be wrapped,
				         * for min, consider box's width */
				        if (min < b.Width)
					        min = b.Width;

			        } else if (((int)b.Parent.Flags & (int)BoxFlags.NEED_MIN) != 0)
                    {
				        /* If we care what the minimum width is,
				         * calculate it.  (It's only needed if we're
				         * shrinking-to-fit.) */
				        /* min = widest single word */
				        i = 0;
				        do {
					        for (j = i; j != b.Length && b.Text[j] != ' '; j++) ;
                            //font_func->width(&fstyle, b.Text + i, j - i, &width);
                            Log.Unimplemented("font_func->width()");
                            if (min < width)
						        min = width;
					        i = j + 1;
				        } while (j != b.Length);
			        }

			        line_has_height = true;

			        continue;
		        }

		        /* inline replaced, 10.3.2 and 10.6.2 */
		        Debug.Assert(b.Style != null);

		        /* calculate box width */
		        wtype = b.Style.ComputedWidth(ref value, ref unit);
		        bs = block.Style.ComputedBoxSizing();
		        if (wtype == CssWidth.CSS_WIDTH_SET)
                {
			        if (unit == CssUnit.CSS_UNIT_PCT)
                    {
                        width = int.MinValue; //AUTO;
			        } else {
				        width = content.UnitLenCtx.Len2DevicePx(b.Style, value, unit).ToInt();

				        if (bs == CssBoxSizingEnum.CSS_BOX_SIZING_BORDER_BOX) {
                            fixedSize = 0; frac = 0;
					        CalculateMbpWidth(content.UnitLenCtx,
							        block.Style, (uint)BoxSide.LEFT,
							        false, true, true,
							        ref fixedSize, ref frac);
                            CalculateMbpWidth(content.UnitLenCtx,
							        block.Style, (uint)BoxSide.RIGHT,
							        false, true, true,
							        ref fixedSize, ref frac);
					        if (width < fixedSize) {
						        width = fixedSize;
					        }
				        }
				        if (width < 0)
					        width = 0;
			        }
		        } else {
			        width = int.MinValue; //AUTO;
                }

		        /* height */
		        htype = b.Style.ComputedHeight(ref value, ref unit);
		        if (htype == CssHeightEnum.CSS_HEIGHT_SET) {
			        height = content.UnitLenCtx.Len2DevicePx(b.Style, value, unit).ToInt();
		        } else {
			        height = int.MinValue; //AUTO;
                }

                /*if (b.Object || (b.Flags & REPLACE_DIM))
                {
			        if (b.Object)
                    {
				        int temp_height = height;
				        layout_get_object_dimensions(b,
						        &width, &temp_height,
						        INT_MIN, INT_MAX,
						        INT_MIN, INT_MAX);
			        }

                fixedSize = 0;  frac = 0;
			        if (bs == CSS_BoxType.BOX_SIZING_BORDER_BOX) {
				        calculate_mbp_width(&content->unit_len_ctx,
						        b->style, LEFT,
						        true, false, false,
						        &fixedSize, &frac);
				        calculate_mbp_width(&content->unit_len_ctx,
						        b->style, RIGHT,
						        true, false, false,
						        &fixedSize, &frac);
			        } else {
				        calculate_mbp_width(&content->unit_len_ctx,
						        b->style, LEFT,
						        true, true, true,
						        &fixedSize, &frac);
				        calculate_mbp_width(&content->unit_len_ctx,
						        b->style, RIGHT,
						        true, true, true,
						        &fixedSize, &frac);
			        }
			        if (0 < width + fixedSize)
				        width += fixedSize;
		        } else if (b.Flags & IFRAME)
                {
			        // TODO: handle percentage widths properl
			        if (width == int.MinValue) // AUTO
				        width = 400;

                    fixedSize = 0; frac = 0;
			        if (bs == CSS_BoxType.BOX_SIZING_BORDER_BOX) {
				        calculate_mbp_width(&content->unit_len_ctx,
						        b->style, LEFT,
						        true, false, false,
						        &fixedSize, &frac);
				        calculate_mbp_width(&content->unit_len_ctx,
						        b->style, RIGHT,
						        true, false, false,
						        &fixedSize, &frac);
			        } else {
				        calculate_mbp_width(&content->unit_len_ctx,
						        b->style, LEFT,
						        true, true, true,
						        &fixedSize, &frac);
				        calculate_mbp_width(&content->unit_len_ctx,
						        b->style, RIGHT,
						        true, true, true,
						        &fifixedSizexed, &frac);
			        }

			        if (0 < width + fixedSize)
				        width += fixedSize;

		        } else*/
                {
                    // form control with no object
                    if (width == int.MinValue) // AUTO
				        width = content.UnitLenCtx.Len2DevicePx(
						        b.Style,
						        Fixed.F_1, CssUnit.CSS_UNIT_EM).ToInt();
		        }

		        if (min < width && !b.HasPercentageMaxWidth())
			        min = width;
		        if (width > 0)
			        max += width;

		        line_has_height = true;
	        }

	        if (first_line)
            {
		        /* todo: handle percentage values properly */
		        /* todo: handle text-indent interaction with floats */
		        int text_indent = LayoutTextIndent(content.UnitLenCtx,
				        first.Parent.Parent.Style, 100);
		        min = (min + text_indent < 0) ? 0 : min + text_indent;
		        max = (max + text_indent < 0) ? 0 : max + text_indent;
	        }

	        line_min = min;
	        line_max = max;

	        Log.Print(LogChannel.Layout, $"line_min {min}, line_max {max}");

	        Debug.Assert(b != first);
	        Debug.Assert(0 <= line_min);
	        Debug.Assert(line_min <= line_max);

	        return b;
        }

        /**
         * Calculate minimum and maximum width of an inline container.
         *
         * \param inline_container  box of type INLINE_CONTAINER
         * \param[out] has_height set to true if container has height
         * \param font_func Font functions.
         * \post  inline_container->min_width and inline_container->max_width filled in,
         *        0 <= inline_container->min_width <= inline_container->max_width
         */
        // layout.c:919
        public void LayoutMinmaxInlineContainer(ref bool has_height,
			               //const struct gui_layout_table *font_func,
			               HtmlContent content)
        {
	        Box child;
	        int line_min = 0, line_max = 0;
	        int min = 0, max = 0;
	        bool first_line = true;
	        bool line_has_height = false;

	        Debug.Assert(Type == BoxType.BOX_INLINE_CONTAINER);

	        // check if the widths have already been calculated
	        if (MaxWidth != int.MaxValue)
		        return;

	        has_height = false;

	        for (child = Children; child != null; ) {
		        child = LayoutMinmaxLine(child, ref line_min, ref line_max,
				        first_line, ref line_has_height, /* font_func,*/ content);
		        if (min < line_min)
			        min = line_min;
		        if (max < line_max)
			        max = line_max;
		        first_line = false;
		        has_height |= line_has_height;
	        }

	        MinWidth = min;
	        MaxWidth = max;

	        Debug.Assert(0 <= MinWidth && MinWidth <= MaxWidth);
        }


        /**
         * Calculate minimum and maximum width of a block.
         *
         * \param block  box of type BLOCK, INLINE_BLOCK, or TABLE_CELL
         * \param font_func font functions
         * \param content The HTML content being layed out.
         * \post  block->min_width and block->max_width filled in,
         *        0 <= block->min_width <= block->max_width
         */
        // layout.c:968
        public void LayoutMinmaxBlock(/*const struct gui_layout_table *font_func,*/ HtmlContent content)
        {
            Box child;
            int min = 0, max = 0;
            int extra_fixed = 0;
            double extra_frac = 0;
            CssWidth wtype = CssWidth.CSS_WIDTH_AUTO;
            Fixed width = Fixed.F_0;
            CssUnit wunit = CssUnit.CSS_UNIT_PX;
            CssHeightEnum htype = CssHeightEnum.CSS_HEIGHT_AUTO;
            Fixed height = Fixed.F_0;
            CssUnit hunit = CssUnit.CSS_UNIT_PX;
            CssBoxSizingEnum bs = CssBoxSizingEnum.CSS_BOX_SIZING_CONTENT_BOX;
            bool child_has_height = false;

            Debug.Assert(Type == BoxType.BOX_BLOCK ||
                    Type == BoxType.BOX_INLINE_BLOCK ||
                    Type == BoxType.BOX_TABLE_CELL);

            // check if the widths have already been calculated
            if (MaxWidth != int.MaxValue)
                return;

            if (Style != null)
            {
                wtype = Style.ComputedWidth(ref width, ref wunit);
                htype = Style.ComputedHeight(ref height, ref hunit);
                bs = Style.ComputedBoxSizing();
            }

            // set whether the minimum width is of any interest for this box
            if (((Parent != null && (Parent.Type == BoxType.BOX_FLOAT_LEFT ||
                    Parent.Type == BoxType.BOX_FLOAT_RIGHT)) ||
                    Type == BoxType.BOX_INLINE_BLOCK) &&
                    wtype != CssWidth.CSS_WIDTH_SET)
            {
                // box shrinks to fit; need minimum width
                Flags |= BoxFlags.NEED_MIN;
            }
            else if (Type == BoxType.BOX_TABLE_CELL)
            {
                // box shrinks to fit; need minimum width
                Flags |= BoxFlags.NEED_MIN;
            }
            else if (Parent != null &&
                (((int)Parent.Flags & (int)BoxFlags.NEED_MIN) != 0) &&
                    wtype != CssWidth.CSS_WIDTH_SET)
            {
                // box inside shrink-to-fit context; need minimum width
                Flags |= BoxFlags.NEED_MIN;
            }

            /*
	        if (Gadget && (block->gadget->type == GADGET_TEXTBOX ||
			        block->gadget->type == GADGET_PASSWORD ||
			        block->gadget->type == GADGET_FILE ||
			        block->gadget->type == GADGET_TEXTAREA) &&
			        Style && wtype == CSS_WIDTH_AUTO) {
		        Fixed size = INTTOFIX(10);
		        CssUnit unit = CssUnit.CSS_UNIT_EM;

		        min = max = FIXTOINT(css_unit_len2device_px(Style,
				        &content->unit_len_ctx, size, unit));

		        Flags |= BoxFlags.HAS_HEIGHT;
	        }

	        if (block->gadget && (block->gadget->type == GADGET_RADIO ||
			        block->gadget->type == GADGET_CHECKBOX) &&
			        Style && wtype == CSS_WIDTH_AUTO) {
		        css_fixed size = INTTOFIX(1);
				CssUnit unit = CssUnit.CSS_UNIT_EM;

		        // form checkbox or radio button
		        // if width is AUTO, set it to 1em
		        min = max = FIXTOINT(css_unit_len2device_px(Style,
				        &content->unit_len_ctx, size, unit));

		        Flags |= BoxFlags.HAS_HEIGHT;
	        }*/

            /*if (block->object) {
		        if (content_get_type(block->object) == CONTENT_HTML) {
			        layout_minmax_block(html_get_box_tree(block->object),
					        font_func, content);
			        min = html_get_box_tree(block->object).MinWidth;
			        max = html_get_box_tree(block->object).MaxWidth;
		        } else {
			        min = max = content_get_width(block->object);
		        }

		        Flags |= BoxFlags.HAS_HEIGHT;
	        } else*/
            if (((int)Flags & (int)BoxFlags.IFRAME) != 0)
            {
                // \todo do we need to know the min/max width of the iframe's content?
                Flags |= BoxFlags.HAS_HEIGHT;
            }
            else
            {
                // recurse through children
                for (child = Children; child != null; child = child.Next)
                {
                    switch (child.Type)
                    {
                        case BoxType.BOX_BLOCK:
                            child.LayoutMinmaxBlock(/*font_func,*/ content);
                            if (((int)child.Flags & (int)BoxFlags.HAS_HEIGHT) != 0)
                                child_has_height = true;
                            break;
                        case BoxType.BOX_INLINE_CONTAINER:
                            if (((int)Flags & (int)BoxFlags.NEED_MIN) != 0)
                                child.Flags |= BoxFlags.NEED_MIN;

                            child.LayoutMinmaxInlineContainer(ref child_has_height,/* font_func,*/ content);
                            if (child_has_height && child == child.Parent.Children)
                            {
                                Flags |= BoxFlags.MAKE_HEIGHT;
                            }
                            break;
                        case BoxType.BOX_TABLE:
                            /*
							layout_minmax_table(child, font_func, content);
							// todo: fix for zero height tables
							child_has_height = true;
							child.Flags |= BoxFlags.MAKE_HEIGHT;
							*/
                            Log.Unimplemented();
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }
                    Debug.Assert(child.MaxWidth != int.MaxValue);

                    if (child.Style != null &&
                            (child.Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
                            child.Style.ComputedPosition() == CssPosition.CSS_POSITION_FIXED))
                    {
                        // This child is positioned out of normal flow,
                        // so it will have no affect on width
                        continue;
                    }

                    if (min < child.MinWidth)
                        min = child.MinWidth;
                    if (max < child.MaxWidth)
                        max = child.MaxWidth;

                    if (child_has_height)
                        Flags |= BoxFlags.HAS_HEIGHT;
                }
            }

            if (max < min)
            {
                //box_dump(stderr, block, 0, true);
                Debug.Assert(false);
            }

            // fixed width takes priority
            if (Type != BoxType.BOX_TABLE_CELL && wtype == CssWidth.CSS_WIDTH_SET &&
                    wunit != CssUnit.CSS_UNIT_PCT)
            {
                /*
		        min = max = FIXTOINT(css_unit_len2device_px(Style,
				        &content->unit_len_ctx, width, wunit));
		        if (bs == CSS_BOX_SIZING_BORDER_BOX) {
			        int border_box_fixed = 0;
			        float border_box_frac = 0;
			        calculate_mbp_width(&content->unit_len_ctx,
					        Style, LEFT,
					        false, true, true,
					        &border_box_fixed, &border_box_frac);
			        calculate_mbp_width(&content->unit_len_ctx,
					        Style, RIGHT,
					        false, true, true,
					        &border_box_fixed, &border_box_frac);
			        if (min < border_box_fixed) {
				        min = max = border_box_fixed;
			        }
		        }*/
                Log.Unimplemented();
            }

            if (htype == CssHeightEnum.CSS_HEIGHT_SET && hunit != CssUnit.CSS_UNIT_PCT &&
                    height > Fixed.F_0)
            {
                Flags |= BoxFlags.MAKE_HEIGHT;
                Flags |= BoxFlags.HAS_HEIGHT;
            }

            // add margins, border, padding to min, max widths
            // Note: we don't know available width here so percentage margin
            // and paddings are wrong.
            if (bs == CssBoxSizingEnum.CSS_BOX_SIZING_BORDER_BOX && wtype == CssWidth.CSS_WIDTH_SET)
            {
                // Border and padding included in width, so just get margin
		        CalculateMbpWidth(content.UnitLenCtx,
				        Style, (uint)BoxSide.LEFT, true, false, false,
				        ref extra_fixed, ref extra_frac);
                CalculateMbpWidth(content.UnitLenCtx,
				        Style, (uint)BoxSide.RIGHT, true, false, false,
				        ref extra_fixed, ref extra_frac);
            }
            else
            {
                CalculateMbpWidth(content.UnitLenCtx,
				        Style, (uint)BoxSide.LEFT, true, true, true,
				        ref extra_fixed, ref extra_frac);
                CalculateMbpWidth(content.UnitLenCtx,
				        Style, (uint)BoxSide.RIGHT, true, true, true,
				        ref extra_fixed, ref extra_frac);
            }
            if (extra_fixed < 0)
                extra_fixed = 0;
            if (extra_frac < 0)
                extra_frac = 0;
            if (1.0 <= extra_frac)
                extra_frac = 0.9;
            if (Style != null &&
                    (Style.ComputedFloat() == CssFloat.CSS_FLOAT_LEFT ||
                    Style.ComputedFloat() == CssFloat.CSS_FLOAT_RIGHT))
            {
                // floated boxs
                MinWidth = min + extra_fixed;
                MaxWidth = max + extra_fixed;
            }
            else
            {
                // not floated
                MinWidth = (int)((min + extra_fixed) / (1.0 - extra_frac));
                MaxWidth = (int)((max + extra_fixed) / (1.0 - extra_frac));
            }

            Debug.Assert(0 <= MinWidth && MinWidth <= MaxWidth);
        }

		// layout_handle_box_sizing()
		// layout.c:1204
		/**
		 * Adjust a specified width or height for the box-sizing property.
		 *
		 * This turns the specified dimension into a content-box dimension.
		 *
		 * \param  unit_len_ctx          Length conversion context
		 * \param  box		    gadget to adjust dimensions of
		 * \param  available_width  width of containing block
		 * \param  setwidth	    set true if the dimension to be tweaked is a width,
		 *				else set false for a height
		 * \param  dimension	    current value for given width/height dimension.
		 *				updated to new value after consideration of
		 *				gadget properties.
		 */
		public void LayoutHandleBoxSizing(
				CssUnitCtx unit_len_ctx,
				int available_width,
				bool setwidth,
				ref int dimension)
		{
			Debug.Assert(Style != null);

			var bs = Style.ComputedBoxSizing();

			if (bs == CssBoxSizingEnum.CSS_BOX_SIZING_BORDER_BOX)
			{
				int orig = dimension;
				int fixedPart = 0;
				double frac = 0;

				CalculateMbpWidth(unit_len_ctx, Style, setwidth ? (uint)BoxSide.LEFT : (uint)BoxSide.TOP,
						false, true, true, ref fixedPart, ref frac);
				CalculateMbpWidth(unit_len_ctx, Style, setwidth ? (uint)BoxSide.RIGHT : (uint)BoxSide.BOTTOM,
						false, true, true, ref fixedPart, ref frac);
				orig -= (int)(frac * available_width) + fixedPart;
				dimension = orig > 0 ? orig : 0;
			}
		}

		// layout_solve_width()
        // layout.c:1779
		/**
		 * Solve the width constraint as given in CSS 2.1 section 10.3.3.
		 *
		 * \param  box              Box to solve constraint for
		 * \param  available_width  Max width available in pixels
		 * \param  width	    Current box width
		 * \param  lm		    Min left margin required to avoid floats in px.
		 *				zero if not applicable
		 * \param  rm		    Min right margin required to avoid floats in px.
		 *				zero if not applicable
		 * \param  max_width	    Box max-width ( -ve means no max-width to apply)
		 * \param  min_width	    Box min-width ( <=0 means no min-width to apply)
		 * \return		    New box width
		 *
		 * \post \a box's left/right margins will be updated.
		 */
		public int LayoutSolveWidth(int available_width,
		   int width,
		   int lm,
		   int rm,
		   int max_width,
		   int min_width)
        {
			bool auto_width = false;

			/* Increase specified left/right margins */
			if (Margin[(int)BoxSide.LEFT] != int.MinValue && Margin[(int)BoxSide.LEFT] < lm &&
					Margin[(int)BoxSide.LEFT] >= 0)
				Margin[(int)BoxSide.LEFT] = lm;
			if (Margin[(int)BoxSide.RIGHT] != int.MinValue && Margin[(int)BoxSide.RIGHT] < rm &&
					Margin[(int)BoxSide.RIGHT] >= 0)
				Margin[(int)BoxSide.RIGHT] = rm;

			/* Find width */
			if (width == int.MinValue)
			{
				int margin_left = Margin[(int)BoxSide.LEFT];
				int margin_right = Margin[(int)BoxSide.RIGHT];

				if (margin_left == int.MinValue)
				{
					margin_left = lm;
				}
				if (margin_right == int.MinValue)
				{
					margin_right = rm;
				}

				width = available_width -
						(margin_left + Border[(int)BoxSide.LEFT].Width +
						Padding[(int)BoxSide.LEFT] + Padding[(int)BoxSide.RIGHT] +
						Border[(int)BoxSide.RIGHT].Width + margin_right);
				width = width < 0 ? 0 : width;
				auto_width = true;
			}

			if (max_width >= 0 && width > max_width)
			{
				/* max-width is admissable and width exceeds max-width */
				width = max_width;
				auto_width = false;
			}

			if (min_width > 0 && width < min_width)
			{
				/* min-width is admissable and width is less than max-width */
				width = min_width;
				auto_width = false;
			}

			/* Width was auto, and unconstrained by min/max width, so we're done */
			if (auto_width)
			{
				/* any other 'auto' become 0 or the minimum required values */
				if (Margin[(int)BoxSide.LEFT] == int.MinValue)
				{
					Margin[(int)BoxSide.LEFT] = lm;
				}
				if (Margin[(int)BoxSide.RIGHT] == int.MinValue)
				{
					Margin[(int)BoxSide.RIGHT] = rm;
				}
				return width;
			}

			/* Width was not auto, or was constrained by min/max width
			 * Need to compute left/right margins */

			/* HTML alignment (only applies to over-constrained boxes) */
			if (Margin[(int)BoxSide.LEFT] != int.MinValue && Margin[(int)BoxSide.RIGHT] != int.MinValue &&
					Parent != null && Parent.Style != null)
			{
				switch (Parent.Style.ComputedTextAlign())
				{
					case CssTextAlignEnum.CSS_TEXT_ALIGN_LIBCSS_RIGHT:
						Margin[(int)BoxSide.LEFT] = int.MinValue;
						Margin[(int)BoxSide.RIGHT] = 0;
						break;
					case CssTextAlignEnum.CSS_TEXT_ALIGN_LIBCSS_CENTER:
						Margin[(int)BoxSide.LEFT] = Margin[(int)BoxSide.RIGHT] = int.MinValue;
						break;
					case CssTextAlignEnum.CSS_TEXT_ALIGN_LIBCSS_LEFT:
						Margin[(int)BoxSide.LEFT] = 0;
						Margin[(int)BoxSide.RIGHT] = int.MinValue;
						break;
					default:
						/* Leave it alone; no HTML alignment */
						break;
				}
			}

			if (Margin[(int)BoxSide.LEFT] == int.MinValue && Margin[(int)BoxSide.RIGHT] == int.MinValue)
			{
				/* make the margins equal, centering the element */
				Margin[(int)BoxSide.LEFT] = Margin[(int)BoxSide.RIGHT] =
						(available_width - lm - rm -
						(Border[(int)BoxSide.LEFT].Width + Padding[(int)BoxSide.LEFT] +
						width + Padding[(int)BoxSide.RIGHT] +
						Border[(int)BoxSide.RIGHT].Width)) / 2;

				if (Margin[(int)BoxSide.LEFT] < 0)
				{
					Margin[(int)BoxSide.RIGHT] += Margin[(int)BoxSide.LEFT];
					Margin[(int)BoxSide.LEFT] = 0;
				}

				Margin[(int)BoxSide.LEFT] += lm;

			}
			else if (Margin[(int)BoxSide.LEFT] == int.MinValue)
			{
				Margin[(int)BoxSide.LEFT] = available_width - lm -
						(Border[(int)BoxSide.LEFT].Width + Padding[(int)BoxSide.LEFT] +
						width + Padding[(int)BoxSide.RIGHT] +
						Border[(int)BoxSide.RIGHT].Width + Margin[(int)BoxSide.RIGHT]);
				Margin[(int)BoxSide.LEFT] = Margin[(int)BoxSide.LEFT] < lm
						? lm : Margin[(int)BoxSide.LEFT];
			}
			else
			{
				/* margin-right auto or "over-constrained" */
				Margin[(int)BoxSide.RIGHT] = available_width - rm -
						(Margin[(int)BoxSide.LEFT] + Border[(int)BoxSide.LEFT].Width +
						 Padding[(int)BoxSide.LEFT] + width +
						 Padding[(int)BoxSide.RIGHT] +
						 Border[(int)BoxSide.RIGHT].Width);
			}

			return width;
		}

        // box_vscrollbar_present()
        // box_inspect.c:821
        bool Vscrollbar_present()
        {
	        return Padding[(int)BoxSide.TOP] +
		        Height +
		        Padding[(int)BoxSide.BOTTOM] +
		        Border[(int)BoxSide.BOTTOM].Width < DescendantY1;
        }

        // box_hscrollbar_present()
        // box_inspect.c:831
        bool Hscrollbar_present()
        {
	        return Padding[(int)BoxSide.LEFT] +
		        Width +
		        Padding[(int)BoxSide.RIGHT] +
		        Border[(int)BoxSide.RIGHT].Width < DescendantX1;
        }

        // LayoutBlockAddScrollbar()
        // layout.c:1953
        /**
		 * Manipulate a block's [RB]padding/height/width to accommodate scrollbars
		 *
		 * \param  box	  Box to apply scrollbar space too. Must be BOX_BLOCK.
		 * \param  which  Which scrollbar to make space for. Must be RIGHT or BOTTOM.
		 */
        public void LayoutBlockAddScrollbar(BoxSide which)
        {
            CssOverflowEnum overflow_x, overflow_y;

            Debug.Assert(Type == BoxType.BOX_BLOCK && (which == BoxSide.RIGHT || which == BoxSide.BOTTOM));

            if (Style == null)
                return;

            overflow_x = Style.ComputedOverflowX();
            overflow_y = Style.ComputedOverflowY();

            if (which == BoxSide.BOTTOM &&
                    (overflow_x == CssOverflowEnum.CSS_OVERFLOW_SCROLL ||
                     overflow_x == CssOverflowEnum.CSS_OVERFLOW_AUTO /*||
					(box->object &&
					 content_get_type(box->object) == CONTENT_HTML)*/))
            {
                /* make space for scrollbar, unless height is int.MinValue */
                if (Height != int.MinValue &&
                        (overflow_x == CssOverflowEnum.CSS_OVERFLOW_SCROLL ||
                        Hscrollbar_present()))
                {
                    Padding[(int)BoxSide.BOTTOM] += SCROLLBAR_WIDTH;
                }

            }
            else if (which == BoxSide.RIGHT &&
                    (overflow_y == CssOverflowEnum.CSS_OVERFLOW_SCROLL ||
                     overflow_y == CssOverflowEnum.CSS_OVERFLOW_AUTO /*||
					(box->object &&
					 content_get_type(box->object) == CONTENT_HTML)*/))
            {
                /* make space for scrollbars, unless width is int.MinValue */
                CssHeightEnum htype;
                Fixed height = Fixed.F_0;
                CssUnit hunit = CssUnit.CSS_UNIT_PX;
                htype = Style.ComputedHeight(ref height, ref hunit);

                if (which == BoxSide.RIGHT && Width != int.MinValue &&
                        htype == CssHeightEnum.CSS_HEIGHT_SET &&
                        (overflow_y == CssOverflowEnum.CSS_OVERFLOW_SCROLL ||
                        Vscrollbar_present()))
                {
                    Width -= SCROLLBAR_WIDTH;
                    Padding[(int)BoxSide.RIGHT] += SCROLLBAR_WIDTH;
                }
            }
        }

        // layout_apply_minmax_heigh()
        // layout.c:2603
        /**
         * Manimpulate box height according to CSS min-height and max-height properties
         *
         * \param  unit_len_ctx      CSS length conversion context for document.
         * \param  box		block to modify with any min-height or max-height
         * \param  container	containing block for absolutely positioned elements, or
         *			NULL for non absolutely positioned elements.
         * \return		whether the height has been changed
         */
        public bool LayoutApplyMinmaxHeight(
                CssUnitCtx unit_len_ctx,
                Box container)
        {
            int h;
            Box containing_block = null;
            bool updated = false;

            /* Find containing block for percentage heights */
            if (Style != null && Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE)
            {
                /* Box is absolutely positioned */
                Debug.Assert(container != null);
                containing_block = container;
            }
            else if (float_container != null && Style != null &&
                    (Style.ComputedFloat() == CssFloat.CSS_FLOAT_LEFT ||
                     Style.ComputedFloat() == CssFloat.CSS_FLOAT_RIGHT))
            {
                /* Box is a float */
                Debug.Assert(Parent != null && Parent.Parent != null &&
                        Parent.Parent.Parent != null);
                containing_block = Parent.Parent.Parent;
            }
            else if (Parent != null && Parent.Type != BoxType.BOX_INLINE_CONTAINER)
            {
                /* Box is a block level element */
                containing_block = Parent;
            }
            else if (Parent != null && Parent.Type == BoxType.BOX_INLINE_CONTAINER)
            {
                /* Box is an inline block */
                Debug.Assert(Parent.Parent != null);
                containing_block = Parent.Parent;
            }

            if (Style != null)
            {
                CssHeightEnum htype = CssHeightEnum.CSS_HEIGHT_AUTO;
                Fixed value = Fixed.F_0;
                CssUnit unit = CssUnit.CSS_UNIT_PX;

                if (containing_block != null)
                {
                    htype = containing_block.Style.ComputedHeight(ref value, ref unit);
                }

                /* max-height */
                if (Style.ComputedMaxHeight(ref value, ref unit) == CssMaxHeightEnum.CSS_MAX_HEIGHT_SET)
                {
                    if (unit == CssUnit.CSS_UNIT_PCT)
                    {
                        if (containing_block != null &&
                            containing_block.Height != int.MinValue &&
                            (Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
                                htype == CssHeightEnum.CSS_HEIGHT_SET))
                        {
                            /* Box is absolutely positioned or its
					         * containing block has a valid
					         * specified height. (CSS 2.1
					         * Section 10.5) */
                            h = value.PercentageToInt(containing_block.Height);
                            if (h < Height)
                            {
                                Height = h;
                                updated = true;
                            }
                        }
                    }
                    else
                    {
                        h = unit_len_ctx.Len2DevicePx(Style, value, unit).ToInt();
                        if (h < Height)
                        {
                            Height = h;
                            updated = true;
                        }
                    }
                }

                /* min-height */
                if (BoxTree.ns_computed_min_height(Style, ref value, ref unit) == CssMinHeightEnum.CSS_MIN_HEIGHT_SET)
                {
                    if (unit == CssUnit.CSS_UNIT_PCT)
                    {
                        if (containing_block != null &&
                            containing_block.Height != int.MinValue &&
                            (Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
                                htype == CssHeightEnum.CSS_HEIGHT_SET))
                        {
                            /* Box is absolutely positioned or its
					         * containing block has a valid
					         * specified height. (CSS 2.1
					         * Section 10.5) */
                            h = value.PercentageToInt(containing_block.Height);
                            if (h > Height)
                            {
                                Height = h;
                                updated = true;
                            }
                        }
                    }
                    else
                    {
                        h = unit_len_ctx.Len2DevicePx(Style, value, unit).ToInt();
                        if (h > Height)
                        {
                            Height = h;
                            updated = true;
                        }
                    }
                }
            }
            return updated;
        }

        // layout_line()
        // layout.c:3147
        /**
         * Position a line of boxes in inline formatting context.
         *
         * \param  first   box at start of line
         * \param  width   available width on input, updated with actual width on output
         *                 (may be incorrect if the line gets split?)
         * \param  y	   coordinate of top of line, updated on exit to bottom
         * \param  cx	   coordinate of left of line relative to cont
         * \param  cy	   coordinate of top of line relative to cont
         * \param  cont	   ancestor box which defines horizontal space, for floats
         * \param  indent  apply any first-line indent
         * \param  has_text_children  at least one TEXT in the inline_container
         * \param  next_box  updated to first box for next line, or 0 at end
         * \param  content  memory pool for any new boxes
         * \return  true on success, false on memory exhaustion
         */
        static bool
        LayoutLine(Box first,
	            ref int width,
	            ref int y,
	            int cx,
	            int cy,
	            Box cont,
	            bool indent,
	            bool has_text_children,
	            HtmlContent content,
	            out Box next_box)
        {
            int height, used_height;
            int x0 = 0;
            int x1 = width;
            int x, h, x_previous;
            int fy = cy;
            Box left;
            Box right;
            Box b;
            Box split_box = null;
            Box d;
            Box br_box = null;
            bool move_y = false;
            bool place_below = false;
            int space_before = 0, space_after = 0;
            uint inline_count = 0;
            uint i;
            //const struct gui_layout_table *font_func = content->font_func;
            PlotFontStyle fstyle;

            next_box = null; // To prevent uninitialized value return

            Log.Print(LogChannel.Layout,
                      $"first {first.GetHashCode()}, first->text '{first.Text}', width {width}, y {y}, cx {cx}, cy {cy}");

            /* find sides at top of line */
            x0 += cx;
            x1 += cx;
            HtmlContent.FindSides(cont.float_children, cy, cy, ref x0, ref x1, out left, out right);
            x0 -= cx;
            x1 -= cx;

            if (indent)
                x0 += LayoutTextIndent(content.UnitLenCtx, first.Parent.Parent.Style, width);

            if (x1 < x0)
                x1 = x0;

            /* get minimum line height from containing block.
			 * this is the line-height if there are text children and also in the
			 * case of an initially empty text input */
            if (has_text_children /*|| first.Parent.Parent->gadget*/)
                used_height = height = first.Parent.Parent.Style.LineHeight(content.UnitLenCtx);
            else
                /* inline containers with no text are usually for layout and
				 * look better with no minimum line-height */
                used_height = height = 0;

            /* pass 1: find height of line assuming sides at top of line: loop
			 * body executed at least once
			 * keep in sync with the loop in layout_minmax_line() */

            Log.Print(LogChannel.Layout, $"x0 {x0}, x1 {x1}, x1 - x0 {x1 - x0}");


            for (x = 0, b = first; x <= x1 - x0 && b != null; b = b.Next)
            {
                int min_width, max_width, min_height, max_height;

                Debug.Assert(b.Type == BoxType.BOX_INLINE || b.Type == BoxType.BOX_INLINE_BLOCK ||
                        b.Type == BoxType.BOX_FLOAT_LEFT ||
                        b.Type == BoxType.BOX_FLOAT_RIGHT ||
                        b.Type == BoxType.BOX_BR || b.Type == BoxType.BOX_TEXT ||
                        b.Type == BoxType.BOX_INLINE_END);


                Log.Print(LogChannel.Layout, $"pass 1: b {b.GetHashCode()}, x {x}");


                if (b.Type == BoxType.BOX_BR)
                    break;

                if (b.Type == BoxType.BOX_FLOAT_LEFT || b.Type == BoxType.BOX_FLOAT_RIGHT)
                    continue;
                if (b.Type == BoxType.BOX_INLINE_BLOCK &&
                        (b.Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
                         b.Style.ComputedPosition() == CssPosition.CSS_POSITION_FIXED))
                    continue;

                Debug.Assert(b.Style != null);
                fstyle = b.Style.FontPlotStyle(content.UnitLenCtx);

                x += space_after;

                if (b.Type == BoxType.BOX_INLINE_BLOCK)
                {
                    if (b.MaxWidth != int.MaxValue)
                    {
                        /*if (!layout_float(b, width, content))
                            return false;*/
                        Log.Unimplemented();
                    }

                    h = b.Border[(int)BoxSide.TOP].Width + b.Padding[(int)BoxSide.TOP] + b.Height +
                            b.Padding[(int)BoxSide.BOTTOM] +
                            b.Border[(int)BoxSide.BOTTOM].Width;
                    if (height < h)
                        height = h;
                    x += b.Margin[(int)BoxSide.LEFT] + b.Border[(int)BoxSide.LEFT].Width +
                            b.Padding[(int)BoxSide.LEFT] + b.Width +
                            b.Padding[(int)BoxSide.RIGHT] +
                            b.Border[(int)BoxSide.RIGHT].Width +
                            b.Margin[(int)BoxSide.RIGHT];
                    space_after = 0;
                    continue;
                }

                if (b.Type == BoxType.BOX_INLINE)
                {
                    int dummy0, dummy1, dummy2, dummy3, dummy4, dummy5;
                    /* calculate borders, margins, and padding */
                    HtmlContent.LayoutFindDimensions(content.UnitLenCtx, width, -1, b, b.Style,
                        out dummy0, out dummy1, out dummy2, out dummy3,
                        out dummy4, out dummy5, b.Margin, b.Padding, b.Border);
                    for (i = 0; i != 4; i++)
                        if (b.Margin[i] == int.MinValue)
                            b.Margin[i] = 0;
                    x += b.Margin[(int)BoxSide.LEFT] + b.Border[(int)BoxSide.LEFT].Width +
                            b.Padding[(int)BoxSide.LEFT];
                    if (b.InlineEnd != null)
                    {
                        b.InlineEnd.Margin[(int)BoxSide.RIGHT] = b.Margin[(int)BoxSide.RIGHT];
                        b.InlineEnd.Padding[(int)BoxSide.RIGHT] =
                                b.Padding[(int)BoxSide.RIGHT];
                        b.InlineEnd.Border[(int)BoxSide.RIGHT] =
                                b.Border[(int)BoxSide.RIGHT];
                    }
                    else
                    {
                        x += b.Padding[(int)BoxSide.RIGHT] +
                                b.Border[(int)BoxSide.RIGHT].Width +
                                b.Margin[(int)BoxSide.RIGHT];
                    }
                }
                else if (b.Type == BoxType.BOX_INLINE_END)
                {
                    b.Width = 0;
                    if (b.Space == int.MaxValue)
                    {
                        content.Plot.GetFontWidth(fstyle, " ", ref b.Space);
                        /** \todo handle errors */
                    }
                    space_after = b.Space;

                    x += b.Padding[(int)BoxSide.RIGHT] + b.Border[(int)BoxSide.RIGHT].Width +
                            b.Margin[(int)BoxSide.RIGHT];
                    continue;
                }

                if (/*!b->object &&*/ (b.Flags & BoxFlags.IFRAME) == 0 /*&& !b->gadget*/ &&
                        (b.Flags & BoxFlags.REPLACE_DIM) == 0)
                {
                    /* inline non-replaced, 10.3.1 and 10.6.1 */
                    var lhStyle = b.Style != null ? b.Style : b.Parent.Parent.Style;
                    b.Height = lhStyle.LineHeight(content.UnitLenCtx);
                    if (height < b.Height)
                        height = b.Height;

                    if (string.IsNullOrEmpty(b.Text))
                    {
                        b.Width = 0;
                        space_after = 0;
                        continue;
                    }

                    if (b.Width == int.MaxValue)
                    {
                        /** \todo handle errors */

                        /* If it's a select element, we must use the
                         * width of the widest option text */
                        /*if (b.Parent.Parent->gadget && b.Parent.Parent->gadget->type == GADGET_SELECT)
                        {
                            int opt_maxwidth = 0;
                                    struct form_option *o;

                            for (o = b.Parent.Parent->gadget->data.select.items; o; o = o.Next)
                            {
                                int opt_width;
                                font_func.Width(&fstyle,
                                        o.Text,
                                        strlen(o.Text),
                                        &opt_width);

                                if (opt_maxwidth < opt_width)
                                    opt_maxwidth = opt_width;
                            }
                            b.Width = opt_maxwidth;
                            if (nsoption_bool(core_select_menu))
                                b.Width += SCROLLBAR_WIDTH;
                        } else*/
                        {
                            content.Plot.GetFontWidth(fstyle, b.Text, ref b.Width);
                            b.Flags |= BoxFlags.MEASURED;
                        }
                    }

                    /* If the current text has not been measured (i.e. its
					 * width was estimated after splitting), and it fits on
					 * the line, measure it properly, so next box is placed
					 * correctly. */
                    if (!string.IsNullOrEmpty(b.Text) && (x + b.Width < x1 - x0) && (b.Flags & BoxFlags.MEASURED) == 0 && b.Next != null)
                    {
                        content.Plot.GetFontWidth(fstyle, b.Text, ref b.Width);
                        b.Flags |= BoxFlags.MEASURED;
                    }

                    x += b.Width;
                    if (b.Space == int.MaxValue)
                    {
                        content.Plot.GetFontWidth(fstyle, " ", ref b.Space);
                        /** \todo handle errors */
                    }
                    space_after = b.Space;
                    continue;
                }

                space_after = 0;

                /* inline replaced, 10.3.2 and 10.6.2 */
                Debug.Assert(b.Style != null);

                int[] marginDummy = new int[4];
                BoxBorder[] borderDummy = new BoxBorder[4];
                int[] paddingDummy = new int[4];
                HtmlContent.LayoutFindDimensions(content.UnitLenCtx,
                width, -1, b, b.Style,
                out b.Width, out b.Height,
                out max_width, out min_width,
                out max_height, out min_height,
                marginDummy, paddingDummy, borderDummy);

                /*if (b->object && !(b.Flags & BoxFlags.REPLACE_DIM)) {
                    layout_get_object_dimensions(b, &b.Width, &b.Height,
                            min_width, max_width,
                            min_height, max_height);
                } else*/
                if ((b.Flags & BoxFlags.IFRAME) != 0)
                {
                    /* TODO: should we look at the content dimensions? */
                    if (b.Width == int.MinValue)
                        b.Width = 400;
                    if (b.Height == int.MinValue)
                        b.Height = 300;

                    /* We reformat the iframe browser window to new
                     * dimensions in pass 2 */
                }
                else
                {
                    /* form control with no object */
                    if (b.Width == int.MinValue)
                        b.Width = content.UnitLenCtx.Len2DevicePx(b.Style, new Fixed(1), CssUnit.CSS_UNIT_EM).ToInt();
                    if (b.Height == int.MinValue)
                        b.Height = content.UnitLenCtx.Len2DevicePx(b.Style, new Fixed(1), CssUnit.CSS_UNIT_EM).ToInt();
                }

                /* Reformat object to new box size */
                /*if (b.object && content_get_type(b.object) == CONTENT_HTML &&
                    b.Width != content_get_available_width(b.object))
                {
                    Fixed value = Fixed.F_0;
                    CssUnit unit = CssUnit.CSS_UNIT_PX;

                    enum css_height_e htype = css_computed_height(b.Style, &value, &unit);

                    content_reformat(b->object, false, b.Width, b.Height);

		            if (htype == CSS_HEIGHT_int.MinValue)
			            b.Height = content_get_height(b->object);
                }*/

                if (height < b.Height)
                    height = b.Height;

                x += b.Width;
            }

            /* find new sides using this height */
            x0 = cx;
            x1 = cx + width;
            HtmlContent.FindSides(cont.float_children, cy, cy + height, ref x0, ref x1, out left, out right);
            x0 -= cx;
            x1 -= cx;

            if (indent)
                x0 += LayoutTextIndent(content.UnitLenCtx, first.Parent.Parent.Style, width);

            if (x1 < x0)
                x1 = x0;

            space_after = space_before = 0;

            /* pass 2: place boxes in line: loop body executed at least once */

            Log.Print(LogChannel.Layout, $"x0 {x0}, x1 {x1}, x1 - x0 {x1 - x0}");

            for (x = x_previous = 0, b = first; x <= x1 - x0 && b != null; b = b.Next)
            {

                Log.Print(LogChannel.Layout, $"pass 2: b {b.GetHashCode()}, x {x}");

                if (b.Type == BoxType.BOX_INLINE_BLOCK &&
                        (b.Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
                         b.Style.ComputedPosition() == CssPosition.CSS_POSITION_FIXED))
                {
                    b.X = x + space_after;

                }
                else if (b.Type == BoxType.BOX_INLINE ||
                        b.Type == BoxType.BOX_INLINE_BLOCK ||
                        b.Type == BoxType.BOX_TEXT ||
                        b.Type == BoxType.BOX_INLINE_END)
                {
                    Debug.Assert(b.Width != int.MaxValue);

                    x_previous = x;
                    x += space_after;
                    b.X = x;

                    if ((b.Type == BoxType.BOX_INLINE && b.InlineEnd == null) ||
                            b.Type == BoxType.BOX_INLINE_BLOCK)
                    {
                        b.X += b.Margin[(int)BoxSide.LEFT] + b.Border[(int)BoxSide.LEFT].Width;
                        x = b.X + b.Padding[(int)BoxSide.LEFT] + b.Width +
                                b.Padding[(int)BoxSide.RIGHT] +
                                b.Border[(int)BoxSide.RIGHT].Width +
                                b.Margin[(int)BoxSide.RIGHT];
                    }
                    else if (b.Type == BoxType.BOX_INLINE)
                    {
                        b.X += b.Margin[(int)BoxSide.LEFT] + b.Border[(int)BoxSide.LEFT].Width;
                        x = b.X + b.Padding[(int)BoxSide.LEFT] + b.Width;
                    }
                    else if (b.Type == BoxType.BOX_INLINE_END)
                    {
                        b.Height = b.InlineEnd.Height;
                        x += b.Padding[(int)BoxSide.RIGHT] +
                                b.Border[(int)BoxSide.RIGHT].Width +
                                b.Margin[(int)BoxSide.RIGHT];
                    }
                    else
                    {
                        x += b.Width;
                    }

                    space_before = space_after;
                    if (/*b->object ||*/ (b.Flags & BoxFlags.REPLACE_DIM)!=0 ||
                        (b.Flags & BoxFlags.IFRAME) != 0)
                        space_after = 0;
                    else if (!string.IsNullOrEmpty(b.Text) || b.Type == BoxType.BOX_INLINE_END)
                    {
                        if (b.Space == int.MaxValue)
                        {
                            fstyle=b.Style.FontPlotStyle(content.UnitLenCtx);
                            /** \todo handle errors */
                            content.Plot.GetFontWidth(fstyle, " ", ref b.Space);
                        }
                        space_after = b.Space;
                    }
                    else
                    {
                        space_after = 0;
                    }
                    split_box = b;
                    move_y = true;
                    inline_count++;
                }
                else if (b.Type == BoxType.BOX_BR)
                {
                    b.X = x;
                    b.Width = 0;
                    br_box = b;
                    b = b.Next;
                    split_box = null;
                    move_y = true;
                    break;

                }
                else
                {
                    /* float */
                    Log.Print(LogChannel.Layout, $"float {b.GetHashCode()}");
                    Log.Unimplemented();
                    /*
                    d = b.Children;
                    d.float_children = null;
                    d.cached_place_below_level = 0;
                    b.float_container = d.float_container = cont;

                    if (!layout_float(d, *width, content))
                        return false;

                    Log.Print(LogChannel.Layout,
                          $"{d.GetHashCode()} : {d.Margin[(int)BoxSide.TOP]} {d.Border[(int)BoxSide.TOP].Width}");

                    d.X = d.Margin[(int)BoxSide.LEFT] + d.Border[(int)BoxSide.LEFT].Width;
                    d.Y = d.Margin[(int)BoxSide.TOP] + d.Border[(int)BoxSide.TOP].Width;
                    b.Width = d.Margin[(int)BoxSide.LEFT] + d.Border[(int)BoxSide.LEFT].Width +
                            d.Padding[(int)BoxSide.LEFT] + d.Width +
                            d.Padding[(int)BoxSide.RIGHT] +
                            d.Border[(int)BoxSide.RIGHT].Width +
                            d.Margin[(int)BoxSide.RIGHT];
                    b.Height = d.Margin[(int)BoxSide.TOP] + d.Border[(int)BoxSide.TOP].Width +
                            d.Padding[(int)BoxSide.TOP] + d.Height +
                            d.Padding[(int)BoxSide.BOTTOM] +
                            d.Border[(int)BoxSide.BOTTOM].Width +
                            d.Margin[(int)BoxSide.BOTTOM];

                    if (b.Width > (x1 - x0) - x)
                        place_below = true;
                    if (d.Style != null && (d.Style.ComputedClear() == CssClearEnum.CSS_CLEAR_NONE ||
                            (d.Style.ComputedClear() == CssClearEnum.CSS_CLEAR_LEFT && left == null) ||
                            (d.Style.ComputedClear() == CssClearEnum.CSS_CLEAR_RIGHT && right == null) ||
                            (d.Style.ComputedClear() == CssClearEnum.CSS_CLEAR_BOTH && left == null && right == null)) &&
                            (!place_below ||
                            (left == null && right == null && x == 0)) &&
                            cy >= cont.clear_level &&
                            cy >= cont.cached_place_below_level)
                    {
                        // + not cleared or,
                        //   cleared and there are no floats to clear
                        // + fits without needing to be placed below or,
                        //   this line is empty with no floats
                        // + current y, cy, is below the clear level
                        //
                        // Float affects current line
                        if (b.Type == BoxType.BOX_FLOAT_LEFT)
                        {
                            b.X = cx + x0;
                            if (b.Width > 0)
                                x0 += b.Width;
                            left = b;
                        }
                        else
                        {
                            b.X = cx + x1 - b.Width;
                            if (b.Width > 0)
                                x1 -= b.Width;
                            right = b;
                        }
                        b.Y = cy;
                    }
                    else
                    {
                        // cleared or doesn't fit on line
                        // place below into next available space
                        int fcy = (cy > cont.clear_level) ? cy :
                                cont.clear_level;
                        fcy = (fcy > cont.cached_place_below_level) ?
                                fcy :
                                cont.cached_place_below_level;
                        fy = (fy > fcy) ? fy : fcy;
                        fy = (fy == cy) ? fy + height : fy;

                        place_float_below(b, *width, cx, fy, cont);
                        fy = b.Y;
                        if (d.Style != null && (
                                (d.Style.ComputedClear() == CssClearEnum.CSS_CLEAR_LEFT && left != null) ||
                                (d.Style.ComputedClear() == CssClearEnum.CSS_CLEAR_RIGHT && right != null) ||
                                (d.Style.ComputedClear() == CssClearEnum.CSS_CLEAR_BOTH && (left != null || right != null))))
                        {
                            // to be cleared below existing floats
                            if (b.Type == BoxType.BOX_FLOAT_LEFT)
                                b.X = cx;
                            else
                                b.X = cx + width - b.Width;

                            fcy = HtmlContent.LayoutClear(cont.float_children, d.Style.ComputedClear());
                            if (fcy > cont.clear_level)
                                cont.clear_level = fcy;
                            if (b.Y < fcy)
                                b.Y = fcy;
                        }
                        if (b.Type == BoxType.BOX_FLOAT_LEFT)
                            left = b;
                        else
                            right = b;
                    }
                    add_float_to_container(cont, b);

                    split_box = null;*/
                }
            }

            if (x1 - x0 < x && split_box != null)
            {
                /* the last box went over the end */
                uint split = 0;
                int w=0;
                bool no_wrap = split_box.Style.ComputedWhitespace() == CssWhiteSpaceEnum.CSS_WHITE_SPACE_NOWRAP ||
                        split_box.Style.ComputedWhitespace() == CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE;

                x = x_previous;

                if (!no_wrap &&
                    (split_box.Type == BoxType.BOX_INLINE ||
                     split_box.Type == BoxType.BOX_TEXT) &&
                    //!split_box->object &&
                    (split_box.Flags & BoxFlags.REPLACE_DIM) == 0 &&
                    (split_box.Flags & BoxFlags.IFRAME) == 0 &&
                    /*!split_box->gadget &&*/ !string.IsNullOrEmpty(split_box.Text))
                {
                    Log.Unimplemented();
                    /*
                    font_plot_style_from_css(&content.UnitLenCtx,
                            split_box.Style, &fstyle);
                    // \todo handle errors
                    font_func->split(&fstyle,
                             split_box.Text,
                             split_box.Length,
                             x1 - x0 - x - space_before,
                             &split,
                             &w);
                    */
                }

                /* split == 0 implies that text can't be split */

                if (split == 0)
                    w = split_box.Width;


                Log.Print(LogChannel.Layout,
                      $"splitting: split_box {split_box.GetHashCode()} \"{split_box.Text}\", spilt {split}, w {w}, left {left.GetHashCode()}, right {right.GetHashCode()}, inline_count {inline_count}");
                Log.Unimplemented();
                /*
                if ((split == 0 || x1 - x0 <= x + space_before + w) &&
                        left==null && right==null && inline_count == 1)
                {
                    // first word of box doesn't fit, but no floats and
                    // first box on line so force in
                    if (split == 0 || split == split_box.Length)
                    {
                        // only one word in this box, or not text
                        // or white-space:nowrap
                        b = split_box.Next;
                    }
                    else
                    {
                        // cut off first word for this line
                        if (!layout_text_box_split(content, &fstyle, split_box, split, w))
                            return false;
                        b = split_box.Next;
                    }
                    x += space_before + w;

                    Log.Print(LogChannel.Layout, $"forcing");

                }
                else if ((split == 0 || x1 - x0 <= x + space_before + w) &&
                    inline_count == 1)
                {
                    // first word of first box doesn't fit, but a float is
                    // taking some of the width so move below it
                    Debug.Assert(left!=null || right!=null);
                    used_height = 0;
                    if (left!=null)
                    {

                        Log.Print(LogChannel.Layout,
                              $"cy {cy}, left.Y {left.Y}, left.Height {left.Height}");

                        used_height = left.Y + left.Height - cy + 1;

                        Log.Print(LogChannel.Layout, $"used_height {used_height}");

                    }
                    if (right!=null && used_height < right.Y + right.Height - cy + 1)
                        used_height = right.Y + right.Height - cy + 1;

                    if (used_height < 0)
                        used_height = 0;

                    b = split_box;

                    Log.Print(LogChannel.Layout, "moving below float");

                }
                else if (split == 0 || x1 - x0 <= x + space_before + w)
                {
                    // first word of box doesn't fit so leave box for next line
                    b = split_box;

                    Log.Print(LogChannel.Layout, "leaving for next line");
                }
                else
                {
                    // fit as many words as possible
                    Debug.Assert(split != 0);

                    Log.Print(LogChannel.Layout,
                          $"'{split_box.Text}' {x1 - x0} {split} {w}");

                    if (split != split_box.Length)
                    {
                        if (!layout_text_box_split(content, &fstyle, split_box, split, w))
                            return false;
                        b = split_box.Next;
                    }
                    x += space_before + w;

                    Log.Print(LogChannel.Layout, "fitting words");

                }
                move_y = true;*/
            }

            /* set positions */
            switch (first.Parent.Parent.Style.ComputedTextAlign())
            {
                case CssTextAlignEnum.CSS_TEXT_ALIGN_RIGHT:
                case CssTextAlignEnum.CSS_TEXT_ALIGN_LIBCSS_RIGHT:
                    x0 = x1 - x;
                    break;
                case CssTextAlignEnum.CSS_TEXT_ALIGN_CENTER:
                case CssTextAlignEnum.CSS_TEXT_ALIGN_LIBCSS_CENTER:
                    x0 = (x0 + (x1 - x)) / 2;
                    break;
                case CssTextAlignEnum.CSS_TEXT_ALIGN_LEFT:
                case CssTextAlignEnum.CSS_TEXT_ALIGN_LIBCSS_LEFT:
                case CssTextAlignEnum.CSS_TEXT_ALIGN_JUSTIFY:
                    /* leave on left */
                    break;
                case CssTextAlignEnum.CSS_TEXT_ALIGN_DEFAULT:
                    /* None; consider text direction */
                    switch (first.Parent.Parent.Style.ComputedDirection())
                    {
                        case CssDirectionEnum.CSS_DIRECTION_LTR:
                            /* leave on left */
                            break;
                        case CssDirectionEnum.CSS_DIRECTION_RTL:
                            x0 = x1 - x;
                            break;
                    }
                    break;
            }

            for (d = first; d != b; d = d.Next)
            {
                d.Flags &= ~BoxFlags.NEW_LINE;

                if (d.Type == BoxType.BOX_INLINE_BLOCK &&
                        (d.Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
                         d.Style.ComputedPosition() == CssPosition.CSS_POSITION_FIXED))
                {
                    /* positioned inline-blocks:
                     * set static position (x,y) only, rest of positioning
                     * is handled later */
                    d.X += x0;
                    d.Y = y;
                    continue;
                }
                else if ((d.Type == BoxType.BOX_INLINE &&
                        //((d->object || d->gadget) == false) &&
                        (d.Flags & BoxFlags.IFRAME) == 0 &&
                        (d.Flags & BoxFlags.REPLACE_DIM) == 0) ||
                        d.Type == BoxType.BOX_BR ||
                        d.Type == BoxType.BOX_TEXT ||
                        d.Type == BoxType.BOX_INLINE_END)
                {
                /* regular (non-replaced) inlines */
                d.X += x0;
                d.Y = y - d.Padding[(int)BoxSide.TOP];

                if (d.Type == BoxType.BOX_TEXT && d.Height > used_height)
                {
                    /* text */
                    used_height = d.Height;
                }
            } else if ((d.Type == BoxType.BOX_INLINE) ||
                    d.Type == BoxType.BOX_INLINE_BLOCK)
            {
                /* replaced inlines and inline-blocks */
                d.X += x0;
                d.Y = y + d.Border[(int)BoxSide.TOP].Width + d.Margin[(int)BoxSide.TOP];
                h = d.Margin[(int)BoxSide.TOP] + d.Border[(int)BoxSide.TOP].Width +
                        d.Padding[(int)BoxSide.TOP] + d.Height +
                        d.Padding[(int)BoxSide.BOTTOM] +
                        d.Border[(int)BoxSide.BOTTOM].Width +
                        d.Margin[(int)BoxSide.BOTTOM];
                if (used_height < h)
                    used_height = h;
            }
        }

            first.Flags |= BoxFlags.NEW_LINE;

            Debug.Assert(b != first || (move_y && 0 < used_height && (left != null || right != null)));

            /* handle vertical-align by adjusting box y values */
            /** \todo  proper vertical alignment handling */
            for (d = first; d != b; d = d.Next)
            {
                if ((d.Type == BoxType.BOX_INLINE && d.InlineEnd != null) ||
                        d.Type == BoxType.BOX_BR ||
                        d.Type == BoxType.BOX_TEXT ||
                        d.Type == BoxType.BOX_INLINE_END)
                {
                    Fixed value = Fixed.F_0;
                    CssUnit unit = CssUnit.CSS_UNIT_PX;
                    switch (d.Style.ComputedVerticalAlign(ref value, ref unit))
                    {
                        case CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_SUPER:
                        case CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_TOP:
                        case CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_TEXT_TOP:
                            /* already at top */
                            break;
                        case CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_SUB:
                        case CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_BOTTOM:
                        case CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_TEXT_BOTTOM:
                            d.Y += used_height - d.Height;
                            break;
                        default:
                        case CssVerticalAlignEnum.CSS_VERTICAL_ALIGN_BASELINE:
                            d.Y += (int)(0.75 * (used_height - d.Height));
                            break;
                    }
                }
            }

            /* handle clearance for br */
            if (br_box != null && br_box.Style.ComputedClear() != CssClearEnum.CSS_CLEAR_NONE)
            {
                int clear_y = HtmlContent.LayoutClear(cont.float_children, br_box.Style.ComputedClear());
                if (used_height < clear_y - cy)
                    used_height = clear_y - cy;
            }

            if (move_y)
                y += used_height;
            next_box = b;
            width = x; /* return actual width */
            return true;
        }

        // layout_inline_container()
        // layout.c:3885
        /**
         * Layout lines of text or inline boxes with floats.
         *
         * \param box inline container box
         * \param width horizontal space available
         * \param cont ancestor box which defines horizontal space, for floats
         * \param cx box position relative to cont
         * \param cy box position relative to cont
         * \param content  memory pool for any new boxes
         * \return true on success, false on memory exhaustion
         */
        public bool LayoutInlineContainer(int width,
		        Box cont, int cx, int cy, HtmlContent content)
        {
	        bool first_line = true;
	        bool has_text_children;
	        Box c, next;
	        int y = 0;
	        int curwidth,maxwidth = width;

	        Debug.Assert(Type == BoxType.BOX_INLINE_CONTAINER);

	        Log.Print(LogChannel.Layout,
	              $"inline_container {GetHashCode()}, width {width}, cont {cont.GetHashCode()}, cx {cx}, cy {cy}");


	        has_text_children = false;
	        for (c = Children; c!=null; c = c.Next)
            {
		        bool is_pre = false;

		        if (Style != null) {
			        CssWhiteSpaceEnum whitespace;

			        whitespace = c.Style.ComputedWhitespace();

			        is_pre = (whitespace == CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE ||
				        whitespace == CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE_LINE ||
				        whitespace == CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE_WRAP);
		        }

		        if ((/*!c->object &&*/ (c.Flags & BoxFlags.REPLACE_DIM) == 0 &&
				        (c.Flags & BoxFlags.IFRAME) == 0 &&
				        !string.IsNullOrEmpty(c.Text) && (c.Length != 0 || is_pre)) ||
				        c.Type == BoxType.BOX_BR)
			        has_text_children = true;
	        }

	        /** \todo fix wrapping so that a box with horizontal scrollbar will
	         * shrink back to 'width' if no word is wider than 'width' (Or just set
	         * curwidth = width and have the multiword lines wrap to the min width)
	         */
	        for (c = Children; c!=null; ) {

		        Log.Print(LogChannel.Layout, $"c {c.GetHashCode()}");

		        curwidth = Width;
		        if (!LayoutLine(c, ref curwidth, ref y, cx, cy + y, cont, first_line,
				        has_text_children, content, out next))
			        return false;
		        maxwidth = Math.Max(maxwidth,curwidth);
		        c = next;
		        first_line = false;
	        }

	        Width = maxwidth;
	        Height = y;

	        return true;
        }

        // layout_compute_offsets()
        // layout.c:4912
        /**
		 * Compute box offsets for a relatively or absolutely positioned box with
		 * respect to a box.
		 *
		 * \param  unit_len_ctx           Length conversion context
		 * \param  box               box to compute offsets for
		 * \param  containing_block  box to compute percentages with respect to
		 * \param  top               updated to top offset, or AUTO
		 * \param  right             updated to right offset, or AUTO
		 * \param  bottom            updated to bottom offset, or AUTO
		 * \param  left              updated to left offset, or AUTO
		 *
		 * See CSS 2.1 9.3.2. containing_block must have width and height.
		 */
        public void
		LayoutComputeOffsets(CssUnitCtx unit_len_ctx,
			   Box containing_block,
			   out int top,
			   out int right,
			   out int bottom,
			   out int left)
		{
			Fixed value = Fixed.F_0;
			CssUnit unit = CssUnit.CSS_UNIT_PX;

			Debug.Assert(containing_block.Width != int.MaxValue &&
					containing_block.Width != int.MinValue &&
					containing_block.Height != int.MinValue);

			/* left */
			var type1 = Style.ComputedLeft(ref value, ref unit);
			if (type1 == CssLeftEnum.CSS_LEFT_SET)
			{
				if (unit == CssUnit.CSS_UNIT_PCT)
				{
					left = value.PercentageToInt(containing_block.Width);
				}
				else
				{
					left = unit_len_ctx.Len2DevicePx(Style, value, unit).ToInt();
				}
			}
			else
			{
				left = int.MinValue;
			}

			/* right */
			var type2 = Style.ComputedRight(ref value, ref unit);
			if (type2 == CssRightEnum.CSS_RIGHT_SET)
			{
				if (unit == CssUnit.CSS_UNIT_PCT)
				{
					right = value.PercentageToInt(containing_block.Width);
				}
				else
				{
					right = unit_len_ctx.Len2DevicePx(Style, value, unit).ToInt();
				}
			}
			else
			{
				right = int.MinValue;
			}

			/* top */
			var type3 = Style.ComputedTop(ref value, ref unit);
			if (type3 == CssTopEnum.CSS_TOP_SET)
			{
				if (unit == CssUnit.CSS_UNIT_PCT)
				{
					top = value.PercentageToInt(containing_block.Height);
				}
				else
				{
					top = unit_len_ctx.Len2DevicePx(Style, value, unit).ToInt();
				}
			}
			else
			{
				top = int.MinValue;
			}

			/* bottom */
			var type4 = Style.ComputedBottom(ref value, ref unit);
			if (type4 == CssBottomEnum.CSS_BOTTOM_SET)
			{
				if (unit == CssUnit.CSS_UNIT_PCT)
				{
					bottom = value.PercentageToInt(containing_block.Height);
				}
				else
				{
					bottom = unit_len_ctx.Len2DevicePx(Style, value, unit).ToInt();
				}
			}
			else
			{
				bottom = int.MinValue;
			}
		}

		// layout_position_absolute()
		// layout.c:5377
		/**
         * Recursively layout and position absolutely positioned boxes.
         *
         * \param  box               tree of boxes to layout
         * \param  containing_block  current containing block
         * \param  cx                position of box relative to containing_block
         * \param  cy                position of box relative to containing_block
         * \param  content           memory pool for any new boxes
         * \return  true on success, false on memory exhaustion
         */
		public bool LayoutPositionAbsolute(Box containing_block,
					 int cx, int cy,
					 HtmlContent content)
		{
			Box c;

			for (c = Children; c != null; c = c.Next)
			{
				if ((c.Type == BoxType.BOX_BLOCK || c.Type == BoxType.BOX_TABLE ||
						c.Type == BoxType.BOX_INLINE_BLOCK) &&
						(c.Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
						 c.Style.ComputedPosition() == CssPosition.CSS_POSITION_FIXED))
				{
                    //if (!layout_absolute(c, containing_block, cx, cy, content))
                    //return false;
                    Log.Unimplemented();
					if (!c.LayoutPositionAbsolute(c, 0, 0, content))
						return false;
				}
				else if (c.Style != null && c.Style.ComputedPosition() == CssPosition.CSS_POSITION_RELATIVE)
				{
					if (!c.LayoutPositionAbsolute(c, 0, 0, content))
						return false;
				}
				else
				{
					int px, py;
					if (c.Style != null && (c.Style.ComputedFloat() == CssFloat.CSS_FLOAT_LEFT ||
							c.Style.ComputedFloat() == CssFloat.CSS_FLOAT_RIGHT))
					{
						/* Float x/y coords are relative to nearest
				         * ansestor with float_children, rather than
				         * relative to parent. Need to get x/y relative
				         * to parent */
						Box p;
						px = c.X;
						py = c.Y;
						for (p = Parent; p != null && p.float_children == null; p = p.Parent)
						{
							px -= p.X;
							py -= p.Y;
						}
					}
					else
					{
						/* Not a float, so box x/y coords are relative
	                     * to parent */
						px = c.X;
						py = c.Y;
					}
					if (!c.LayoutPositionAbsolute(containing_block, cx + px, cy + py, content))
						return false;
				}
			}

			return true;
		}

        // html_redraw_box_has_background()
        // redraw.c:77
        /**
         * Determine if a box has a background that needs drawing
         *
         * \param box  Box to consider
         * \return True if box has a background, false otherwise.
         */
        public bool RedrawBoxHasBackground()
        {
            //if (Background != null)
            if (Background != 0)
                return true;

            if (Style != null)
            {
                Color colour;

                Style.ComputedBackgroundColor(out colour);

                if (colour.IsTransparent() == false)
                    return true;
            }

            return false;
        }
        #endregion

        // box_inspect.c:648
        // Print a box tree to a string
        public void Dump(StreamWriter sw, int depth, bool style)
        {
            string res = "";
            int i;
            //struct box *c, *prev;

            for (i = 0; i != depth; i++)
            {
                sw.Write("  ");
            }

            sw.Write($"{GetHashCode().ToString("X")} ");

            sw.Write($"x{X} y{Y} w{Width} h{Height} ");

            if (MaxWidth != /*UNKNOWN_MAX_WIDTH*/int.MaxValue) {
                sw.Write($"min{MinWidth} max{MaxWidth} ");
            }

            sw.Write($"({DescendantX0} {DescendantY0} {DescendantX1} {DescendantY1}) ");

            sw.Write($"m({Margin[0]} {Margin[3]} {Margin[2]} {Margin[1]}) "); // top left bottom right

            switch (Type)
            {
                case BoxType.BOX_BLOCK:
                    sw.Write("BLOCK ");
                    break;

                case BoxType.BOX_INLINE_CONTAINER:
                    sw.Write("INLINE_CONTAINER ");
                    break;

                case BoxType.BOX_INLINE:
                    sw.Write("INLINE ");
                    break;

                case BoxType.BOX_INLINE_END:
                    sw.Write("INLINE_END ");
                    break;

                case BoxType.BOX_INLINE_BLOCK:
                    sw.Write("INLINE_BLOCK ");
                    break;
                /*
                            case BoxType.BOX_TABLE:
                                fprintf(stream, "TABLE [columns %i] ", box->columns);
                                break;

                            case BoxType.BOX_TABLE_ROW:
                                sw.Write("TABLE_ROW ");
                                break;

                            case BoxType.BOX_TABLE_CELL:
                                fprintf(stream, "TABLE_CELL [columns %i, start %i, rows %i] ",
                                    box->columns,
                                    box->start_column,
                                    box->rows);
                                break;
                */
                case BoxType.BOX_TABLE_ROW_GROUP:
                    sw.Write("TABLE_ROW_GROUP ");
                    break;

                case BoxType.BOX_FLOAT_LEFT:
                    sw.Write("FLOAT_LEFT ");
                    break;

                case BoxType.BOX_FLOAT_RIGHT:
                    sw.Write("FLOAT_RIGHT ");
                    break;

                case BoxType.BOX_BR:
                    sw.Write("BR ");
                    break;

                case BoxType.BOX_TEXT:
                    sw.Write("TEXT ");
                    break;

                default:
                    sw.Write("Unknown box type ");
                    break;
            }

            if (Text != null)
                sw.Write($"{0} '{Text}'");

            if (Space != 0)
                sw.Write("space ");
            /*if (box->object) {
                fprintf(stream, "(object '%s') ",
                    nsurl_access(hlcache_handle_get_url(box->object)));
            }
            if (box->iframe) {
                sw.Write("(iframe) ");
            }
            if (box->gadget)
                sw.Write("(gadget) ");*/
            if (style && Style != null)
                Style.Dump(sw);
            /*if (href)
                fprintf(stream, " -> '%s'", nsurl_access(box->href));*/
            if (Target != null)
                sw.Write($" |{Target}|");
            if (Title != null)
                sw.Write($" [{Title}]");
            if (Id != null)
                sw.Write($" ID:{Id}");
            if (Type == BoxType.BOX_INLINE || Type == BoxType.BOX_INLINE_END)
                sw.Write($" inline_end {(InlineEnd == null ? "null" : InlineEnd.GetHashCode())}");
            /*if (box->float_children)
                fprintf(stream, " float_children %p", box->float_children);
            if (box->next_float)
                fprintf(stream, " next_float %p", box->next_float);
            if (box->float_container)
                fprintf(stream, " float_container %p", box->float_container);
            if (box->col) {
                sw.Write(" (columns");
                for (i = 0; i != box->columns; i++) {
                    fprintf(stream, " (%s %s %i %i %i)",
                        ((const char *[]) {
                            "UNKNOWN",
                            "FIXED",
                            "AUTO",
                            "PERCENT",
                            "RELATIVE"
                                })
                        [box->col[i].type],
                        ((const char *[]) {
                            "normal",
                            "positioned"})
                        [box->col[i].positioned],
                        box->col[i].width,
                        box->col[i].min, box->col[i].max);
                }
                sw.Write(")");
            }
            */
            if (Node != null) {
                sw.Write($" <{Node.Name}>");
            }
            sw.WriteLine();

            /*
            if (box->list_marker) {
                for (i = 0; i != depth; i++)
                    fprintf(stream, "  ");
                fprintf(stream, "list_marker:\n");
                box_dump(stream, box->list_marker, depth + 1, style);
            }
            */

            Box c;
            for (c = Children; (c != null) && (c.Next != null); c = c.Next)
                ;

            if (Last != c)
                sw.WriteLine($"warning: box->last {"ptr"} (should be {"ptr"}) (box ${"ptr"})"); // box->last, c, box

            Box prev;
            for (prev = null, c = Children; c != null; prev = c, c = c.Next)
            {
                if (c.Parent != this)
                    sw.WriteLine($"warning: box->parent {(c.Parent != null ? c.Parent.GetHashCode() : "null") } (should be {GetHashCode().ToString("X")}) (box on next line)"); // c->parent, box
                if (c.Prev != prev)
                    sw.WriteLine("warning: box->prev %p (should be %p) (box on next line)"); //c->prev, prev);

                c.Dump(sw, depth + 1, style); // style
            }
        }
    }

    public class BoxTree
    {
        // Context
        XmlNode ctx_n; // ctx->n. Should not be used really
        public Box RootBox; // ctx->root_box

        Dictionary<XmlNode, Box> NodeBoxLookup; // Used to find box for a given node

        //Box Layout; // html->content->layout (root of the whatever), shouldn't be used really

        HtmlContent Content; // FIXME: This is incorrect and needs to be removed

        public BoxTree(HtmlContent content)
        {
            NodeBoxLookup = new Dictionary<XmlNode, Box>();
            Content = content;
        }

        public Box BoxForNode(XmlNode node)
        {
            if (node == null) return null;

            if (!NodeBoxLookup.ContainsKey(node))
                return null;
            else
                return NodeBoxLookup[node];
        }

        void AttachNodeToBox(XmlNode n, Box b)
        {
            NodeBoxLookup.Add(n, b);
        }

        // libcss/src/select/select.c:1189 - css_select_style()
        public CssSelectResults CssSelectStyle(XmlNode node, ref CssUnitCtx unitCtx,
                                               ref CssMedia media, CssStylesheet inlineStyle)
        {
            // STUB
            // BoxGetStyle() -> nscssGetStyle() -> CssSelectStyle()

            var parent = node.ParentNode;

            var state = new CssSelectState(node, parent, ref media, ref unitCtx);

            int nhints = 0;

            var styles0 = new ComputedStyle(); // 0 == CSS_PSEUDO_ELEMENT_NONE

            // Fetch presentational hints

            // Check if we can share another node's style

            //Console.WriteLine($"style:\t{state.Element.Name}\tSELECTED\n");

            // Not sharing; need to select.
            // Base element style is guaranteed to exist
            state.Results.Styles[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE] = new ComputedStyle();

            // Apply any hints
            if (nhints > 0)
            {
                Log.Unimplemented("Applying hints");
            }

            /* Iterate through the top-level stylesheets, selecting styles
             * from those which apply to our current media requirements and
             * are not disabled */
            foreach (var s in Content.SelectionCtx.Sheets)
            {
                //if (mq__list_match(s.media, unit_ctx, media) &&
                //    s.sheet->disabled == false)
                {
                    state.SelectFromSheet(Content.SelectionCtx, s.Sheet, s.Origin);
                }

            }

            // Consider any inline style for the node
            if (inlineStyle != null)
            {
                Log.Unimplemented("inlineStyle != null");
            }

            // Fix up any remaining unset properties

            // Base element
            state.CurrentPseudo = CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE;
            state.Computed = state.Results.Styles[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE];
            int i;
            for (i = 0; i < (int)CssPropertiesEnum.CSS_N_PROPERTIES; i++)
            {
                ref var prop = ref state.Props[i][(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE];

                /* If the property is still unset or it's set to inherit
                 * and we're the root element, then set it to its initial
                 * value. */
                if (!prop.Set || (parent == null && prop.Inherit))
                {
                    CssProps.SetInitial(state, i, CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE, parent);
                }
            }

            // Pseudo elements, if any

            /* If this is the root element, then we must ensure that all
             * length values are absolute, display and float are correctly
             * computed, and the default border-{top,right,bottom,left}-color
             * is set to the computed value of color. */
            if (parent == null)
            {
                // Only compute absolute values for the base element
                var style = state.Results.Styles[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE];
                style.ComputeAbsoluteValues(null, unitCtx);
            }


            // Steal the results from the selection state, so they don't get
            // freed when the selection state is finalised
            return state.Results;

        }

        // content/handlers/css/select.c:253 - nscss_get_style()
        private CssSelectResults nscssGetStyle(ComputedStyle parentStyle, ComputedStyle rootStyle,
            XmlNode node, ref CssMedia media, ref CssUnitCtx unitCtx, CssStylesheet inlineStyle)
        {
            // Select style for node
            var styles = CssSelectStyle(node, ref unitCtx, ref media, inlineStyle);

            // If there's a parent style, compose with partial to obtain
            // complete computed style for element
            if (parentStyle != null)
            {
                /* Complete the computed style, by composing with the parent
                 * element's style */
                var composed = new ComputedStyle(parentStyle, styles.Styles[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE], unitCtx);

                // Replace select_results style with composed style
                styles.Styles[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE] = composed;
            }

            for (int pseudo_element = (int)CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE + 1;
                    pseudo_element < (int)CssPseudoElement.CSS_PSEUDO_ELEMENT_COUNT;
                    pseudo_element++)
            {

                if (pseudo_element == (int)CssPseudoElement.CSS_PSEUDO_ELEMENT_FIRST_LETTER ||
                        pseudo_element == (int)CssPseudoElement.CSS_PSEUDO_ELEMENT_FIRST_LINE)
                    // TODO: Handle first-line and first-letter pseudo
                    //       element computed style completion
                    continue;

                if (styles.Styles[pseudo_element] == null)
                    // There were no rules concerning this pseudo element
                    continue;

                Log.Unimplemented();
                /*
                // Complete the pseudo element's computed style, by composing
                // with the base element's style
                error = css_computed_style_compose(
                        styles->styles[CSS_PSEUDO_ELEMENT_NONE],
                        styles->styles[pseudo_element],
                        unit_len_ctx, &composed);
                if (error != CSS_OK)
                {
                    // TODO: perhaps this shouldn't be quite so
                    // catastrophic?
                    css_select_results_destroy(styles);
                    return NULL;
                }

                // Replace select_results style with composed style
                css_computed_style_destroy(styles->styles[pseudo_element]);
                styles->styles[pseudo_element] = composed;*/
            }

            return styles;
        }

        // box_construct.c:245 - box_get_style()
        private CssSelectResults BoxGetStyle(ComputedStyle parentStyle, ComputedStyle rootStyle, XmlNode node)
        {
            CssSelectResults styles;
            CssStylesheet inlineStyle = null;
            /*
	        dom_string *s;
	        dom_exception err;
	        css_stylesheet *inline_style = NULL;
	        nscss_select_ctx ctx;

	        // Firstly, construct inline stylesheet, if any
            err = dom_element_get_attribute(n, corestring_dom_style, &s);
            if (err != DOM_NO_ERR)
                return NULL;*/

            /*
            if (s != NULL)
            {
                inline_style = nscss_create_inline_style(
                        (const uint8_t*) dom_string_data(s),
				dom_string_byte_length(s),
				c->encoding,
				nsurl_access(c->base_url),
				c->quirks != DOM_DOCUMENT_QUIRKS_MODE_NONE);

                dom_string_unref(s);

                if (inline_style == NULL)
                    return NULL;
            }

            // Populate selection context
            ctx.ctx = c->select_ctx;
            ctx.quirks = (c->quirks == DOM_DOCUMENT_QUIRKS_MODE_FULL);
            ctx.base_url = c->base_url;
            ctx.universal = c->universal;
            ctx.root_style = root_style;
            ctx.parent_style = parent_style;

            // Select style for element
            styles = nscss_get_style(&ctx, n, &c->media, &c->unit_len_ctx,
                    inline_style);
            */

            // Select style for element
            styles = nscssGetStyle(
                parentStyle,
                rootStyle,
                node,
                ref Content.Media,
                ref Content.UnitLenCtx,
                inlineStyle);

            return styles;
        }

        /**
         * Temporary helper wrappers for for libcss computed style getter, while
         * we don't support flexbox related property values.
         */

        // utils.h:34
        public static CssDisplay ns_computed_display(ComputedStyle style, bool root)
        {
            var value = style.ComputedDisplay(root);

            if (value == CssDisplay.CSS_DISPLAY_FLEX)
            {
                return CssDisplay.CSS_DISPLAY_BLOCK;

            }
            else if (value == CssDisplay.CSS_DISPLAY_INLINE_FLEX)
            {
                return CssDisplay.CSS_DISPLAY_INLINE_BLOCK;
            }

            return value;
        }

        // utils.h:50
        public static CssDisplay ns_computed_display_static(ComputedStyle style)
        {
            var value = style.ComputedDisplayStatic();

	        if (value == CssDisplay.CSS_DISPLAY_FLEX) {
		        return CssDisplay.CSS_DISPLAY_BLOCK;

	        } else if (value == CssDisplay.CSS_DISPLAY_INLINE_FLEX) {
		        return CssDisplay.CSS_DISPLAY_INLINE_BLOCK;
	        }

            return value;
        }

        // utils.h:69
        public static CssMinHeightEnum ns_computed_min_height(ComputedStyle style,
            ref Fixed length, ref CssUnit unit)
        {
            var value = style.ComputedMinHeight(ref length, ref unit);

            if (value == CssMinHeightEnum.CSS_MIN_HEIGHT_AUTO)
            {
                value = CssMinHeightEnum.CSS_MIN_HEIGHT_SET;
                length = Fixed.F_0;
                unit = CssUnit.CSS_UNIT_PX;
            }

            return value;
        }

        // utils.h:85
        public static CssMinWidthEnum ns_computed_min_width(ComputedStyle style,
            ref Fixed length, ref CssUnit unit)
        {
            var value = style.ComputedMinWidth(ref length, ref unit);

            if (value == CssMinWidthEnum.CSS_MIN_WIDTH_AUTO)
            {
                value = CssMinWidthEnum.CSS_MIN_WIDTH_SET;
                length = Fixed.F_0;
                unit = CssUnit.CSS_UNIT_PX;
            }

            return value;
        }

        // box_construct.c:451 - box_construct_element()
        private bool BoxConstructElement(XmlNode node, ref bool convertChildren)
        {
            // Construct the box tree for an XML element
            /*dom_string* title0, *s;
            lwc_string* id = NULL;

            enum css_display_e css_display;
	        struct box *box = NULL, *old_box;
	        css_select_results* styles = NULL;
            lwc_string* bgimage_uri;
            dom_exception err;*/
            ComputedStyle rootStyle = null;

            // assert(ctx->n != NULL);
            var props = Box.ExtractProperties(node, this);

            if (props.ContainingBlock != null)
            {
                // In case the containing block is a pre block, we clear
                // the PRE_STRIP flag since it is not used if we follow
                // the pre with a tag
                props.ContainingBlock.Flags &= ~BoxFlags.PRE_STRIP;
            }

            if (props.NodeIsRoot == false)
            {
                rootStyle = RootBox.Style;
            }

            //styles = box_get_style(ctx->content, props.parent_style, root_style, ctx->n);
            var styles = BoxGetStyle(props.ParentStyle, rootStyle, node);
            if (styles == null)
                return false;

            /*
            // Extract title attribute, if present
            err = dom_element_get_attribute(ctx->n, corestring_dom_title, &title0);
            if (err != DOM_NO_ERR)
                return false;

            if (title0 != NULL)
            {
                char* t = squash_whitespace(dom_string_data(title0));

                dom_string_unref(title0);

                if (t == NULL)
                    return false;

                props.title = talloc_strdup(ctx->bctx, t);

                free(t);

                if (props.title == NULL)
                    return false;
            }*/

            // Extract id attribute, if present
            /*
            err = dom_element_get_attribute(ctx->n, corestring_dom_id, &s);
	        if (err != DOM_NO_ERR)
		        return false;

	        if (s != NULL) {
		        err = dom_string_intern(s, &id);
		        if (err != DOM_NO_ERR)
			        id = NULL;

		        dom_string_unref(s);
	        }
             */

            //var box = new Box(styles, styles->styles[CSS_PSEUDO_ELEMENT_NONE], false,
            //      props.href, props.target, props.title, id, ctx->bctx);
            var box = new Box(
                styles,
                styles.Styles[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE],
                false,
                0,
                "",
                props.Title,
                node.Attributes["id"] != null ? node.Attributes["id"].Value : null);

            if (node.Attributes["id"] != null)
                Console.WriteLine("Node id: {0}", node.Attributes["id"].Value);

            // If this is the root box, add it to the context/
            if (props.NodeIsRoot)
                RootBox = box;
            /*
                        // Deal with colspan/rowspan
                        err = dom_element_get_attribute(ctx->n, corestring_dom_colspan, &s);
                        if (err != DOM_NO_ERR)
                            return false;

                        if (s != NULL)
                        {
                            const char* val = dom_string_data(s);

                            if ('0' <= val[0] && val[0] <= '9')
                                box->columns = strtol(val, NULL, 10);

                            dom_string_unref(s);
                        }

                        err = dom_element_get_attribute(ctx->n, corestring_dom_rowspan, &s);
                        if (err != DOM_NO_ERR)
                            return false;

                        if (s != NULL)
                        {
                            const char* val = dom_string_data(s);

                            if ('0' <= val[0] && val[0] <= '9')
                                box->rows = strtol(val, NULL, 10);

                            dom_string_unref(s);
                        }
            */
            var cssDisplay = ns_computed_display_static(box.Style);

            // Set box type from computed display
            if ((box.Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
                 box.Style.ComputedPosition() == CssPosition.CSS_POSITION_FIXED) &&
                    (cssDisplay == CssDisplay.CSS_DISPLAY_INLINE ||
                     cssDisplay == CssDisplay.CSS_DISPLAY_INLINE_BLOCK ||
                     cssDisplay == CssDisplay.CSS_DISPLAY_INLINE_TABLE ||
                     cssDisplay == CssDisplay.CSS_DISPLAY_INLINE_FLEX))
            {
                // Special case for absolute positioning: make absolute inlines
                // into inline block so that the boxes are constructed in an
                // inline container as if they were not absolutely positioned.
                // Layout expects and handles this.
                box.Type = Box.BoxMap(CssDisplay.CSS_DISPLAY_INLINE_BLOCK);
            }
            else if (props.NodeIsRoot)
            {
                // Special case for root element: force it to BLOCK, or the
                // rest of the layout will break.
                box.Type = BoxType.BOX_BLOCK;
            }
            else
            {
                // Normal mapping
                box.Type = Box.BoxMap(ns_computed_display(box.Style, props.NodeIsRoot));

                if (props.ContainingBlock.Type == BoxType.BOX_FLEX ||
                    props.ContainingBlock.Type == BoxType.BOX_INLINE_FLEX)
                {
                    // Blockification
                    switch (box.Type)
                    {
                        case BoxType.BOX_INLINE_FLEX:
                            box.Type = BoxType.BOX_FLEX;
                            break;
                        case BoxType.BOX_INLINE_BLOCK:
                            box.Type = BoxType.BOX_BLOCK;
                            break;
                        default:
                            break;
                    }
                }
            }


            /*
                                    if (convert_special_elements(ctx->n,
                                                     ctx->content,
                                                     box,
                                                     convert_children) == false)
                                    {
                                        return false;
                                    }
                                    */

            // Handle the :before pseudo element
            if (((int)box.Flags & (int)BoxFlags.IS_REPLACED) == 0)
            {
                BoxConstructGenerate(
                    node,
                    box,
                    box.Styles.Styles[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_BEFORE]);
            }

            if (box.Type == BoxType.BOX_NONE ||
                (ns_computed_display(box.Style, props.NodeIsRoot) == CssDisplay.CSS_DISPLAY_NONE &&
                    props.NodeIsRoot == false))
            {
                Log.Unimplemented();
                box = null;
                /*
                css_select_results_destroy(styles);
                box->styles = NULL;
                box->style = NULL;

                // Invalidate associated gadget, if any
                if (box->gadget != NULL)
                {
                    box->gadget->box = NULL;
                    box->gadget = NULL;
                }

                // Can't do this, because the lifetimes of boxes and gadgets
	            // are inextricably linked. Fortunately, talloc will save us
	            // (for now)
                // box_free_box(box);*/

                convertChildren = false;

                return true;
            }

            // Attach DOM node to box
            AttachNodeToBox(node, box);


            // Attach box to DOM node/
            box.AttachToNode(node);

            if (props.InlineContainer == null &&
                (box.Type == BoxType.BOX_INLINE ||
                    box.Type == BoxType.BOX_BR ||
                    box.Type == BoxType.BOX_INLINE_BLOCK ||
                    box.Type == BoxType.BOX_INLINE_FLEX ||
                    box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_LEFT ||
                    box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_RIGHT) &&
                props.NodeIsRoot == false)
            {
                // Found an inline child of a block without a current container
                // (i.e. this box is the first child of its parent, or was
                // preceded by block-level siblings)
                Debug.Assert(props.ContainingBlock != null); // "Box must have containing block."

                props.InlineContainer = new Box(null, null, false, 0, "", "", null);
                props.InlineContainer.Type = BoxType.BOX_INLINE_CONTAINER;

                props.ContainingBlock.AddChild(props.InlineContainer);
            }

            // Kick off fetch for any background image
            /*
            if (css_computed_background_image(box->style, &bgimage_uri) ==
                    CSS_BACKGROUND_IMAGE_IMAGE && bgimage_uri != NULL &&
                    nsoption_bool(background_images) == true)
            {
                nsurl* url;
                nserror error;

                // TODO: we get a url out of libcss as a lwc string, but
                 //      earlier we already had it as a nsurl after we
                 //      nsurl_joined it.  Can this be improved?
                 //      For now, just making another nsurl.
                error = nsurl_create(lwc_string_data(bgimage_uri), &url);
                if (error == NSERROR_OK)
                {
                    // Fetch image if we got a valid URL
                    if (html_fetch_object(ctx->content,
                                  url,
                                  box,
                                  image_types,
                                  true) == false)
                    {
                        nsurl_unref(url);
                        return false;
                    }
                    nsurl_unref(url);
                }
            }*/

            if (convertChildren)
                box.Flags |= BoxFlags.CONVERT_CHILDREN;

            if (box.Type == BoxType.BOX_INLINE ||
                box.Type == BoxType.BOX_BR ||
                box.Type == BoxType.BOX_INLINE_FLEX ||
                box.Type == BoxType.BOX_INLINE_BLOCK)
            {
                // Inline container must exist, as we'll have
                // created it above if it didn't
                Debug.Assert(props.InlineContainer != null);

                props.InlineContainer.AddChild(box);
            }
            else
            {
                if (ns_computed_display(box.Style, props.NodeIsRoot) == CssDisplay.CSS_DISPLAY_LIST_ITEM)
                {
                    // List item: compute marker
                    /*
                    if (box_construct_marker(box, props.title, ctx,
                            props.containing_block) == false)
                        return false;*/
                    Log.Unimplemented();
                }

                if (props.NodeIsRoot == false &&
                        /*box__containing_block_is_flex(&props) == false &&*/
                        (box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_LEFT ||
                        box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_RIGHT))
                {
                    Log.Unimplemented();
                    /*
                    // Float: insert a float between the parent and box.
                    struct box *flt = box_create(NULL, NULL, false,
                                props.href, props.target, props.title,
                                NULL, ctx->bctx);
                    if (flt == NULL)
                        return false;

                    if (css_computed_float(box->style) == CSS_FLOAT_LEFT)
                        flt->type = BOX_FLOAT_LEFT;
                    else
                        flt->type = BOX_FLOAT_RIGHT;

                    props.InlineContainer.AddChild(flt);
                    flt.AddChild(box);*/
                }
                else
                {
                    // Non-floated block-level box: add to containing block
                    // if there is one. If we're the root box, then there
                    // won't be.
                    if (props.ContainingBlock != null)
                        props.ContainingBlock.AddChild(box);
                }
            }
            return true;
        }

        // box_construct.c:308 - box_construct_generate
        private void BoxConstructGenerate(XmlNode n, Box box, ComputedStyle style)
        {
            Box gen = null;
            //enum css_display_e computed_display;
            CssComputedContentItem[] c_item = new CssComputedContentItem[1];

            // Nothing to generate if the parent box is not a block
            if (box.Type != BoxType.BOX_BLOCK)
                return;

            /* To determine if an element has a pseudo element, we select
	         * for it and test to see if the returned style's content
	         * property is set to normal. */
            if (style == null ||
                style.ComputedContent(ref c_item) == CssContent.CSS_CONTENT_NORMAL)
            {
                // No pseudo element
                return;
            }

            Log.Unimplemented("box.Type == BoxType.BOX_BLOCK");

            /*
	        // create box for this element
	        computed_display = ns_computed_display(style, box_is_root(n));
	        if (computed_display == CSS_DISPLAY_BLOCK ||
			        computed_display == CSS_DISPLAY_TABLE) {
		        // currently only support block level boxes

		        // \todo Not wise to drop const from the computed style
		        gen = box_create(NULL, (css_computed_style*) style,
				        false, NULL, NULL, NULL, NULL, content->bctx);
		        if (gen == NULL) {
			        return;
		        }

		        // set box type from computed display
		        gen->type = box_map[ns_computed_display(style, box_is_root(n))];

		        box_add_child(box, gen);
            }*/
        }

        // box_construct.c:732 - box_construct_element_after()
        private void ConstructElementAfter(XmlNode n)
        {
            var box = BoxForNode(n);

            var props = Box.ExtractProperties(n, this);

            if (box.Type == BoxType.BOX_INLINE || box.Type == BoxType.BOX_BR)
            {
                // Insert INLINE_END into containing block
                if (n.ChildNodes.Count == 0 || (box.Flags & BoxFlags.CONVERT_CHILDREN) == 0)
                {
                    // No children, or didn't want children converted
                    return;
                }

                if (props.InlineContainer == null)
                {
                    // Create inline container if we don't have one
                    props.InlineContainer = new Box(null, null, false,
                            0, String.Empty, String.Empty, String.Empty);

                    props.InlineContainer.Type = BoxType.BOX_INLINE_CONTAINER;

                    props.ContainingBlock.AddChild(props.InlineContainer);
                }

                var inlineEnd = new Box(null, box.Style, false, /*box.Href*/0, box.Target, box.Title, box.Id);

                inlineEnd.Type = BoxType.BOX_INLINE_END;

			    Debug.Assert(props.InlineContainer != null);
                props.InlineContainer.AddChild(inlineEnd);
                box.InlineEnd = inlineEnd;
	            inlineEnd.InlineEnd = box;
            }
            else if (((int)box.Flags & (int)BoxFlags.IS_REPLACED) == 0)
            {
                // Handle the :after pseudo element
                BoxConstructGenerate(n, box, box.Styles.Styles[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_AFTER]);
            }
        }

        // box_construct.c:952 - box_construct_text()
        private bool BoxConstructText(XmlNode n)
        {
            /*
            struct box_construct_props props;
	        struct box *box = NULL;
	        dom_string* content;
            dom_exception err;

            assert(ctx->n != NULL);*/

            var props = Box.ExtractProperties(n, this);

            Debug.Assert(props.ContainingBlock != null);

            var content = n.Value;
            if (string.IsNullOrEmpty(content))
                return false;

            if (props.ParentStyle.ComputedWhitespace() == CssWhiteSpaceEnum.CSS_WHITE_SPACE_NORMAL ||
                props.ParentStyle.ComputedWhitespace() == CssWhiteSpaceEnum.CSS_WHITE_SPACE_NOWRAP)
            {
                var text = content.SquashWhitespace();
                if (string.IsNullOrEmpty(text))
                    return false;
                // if the text is just a space, combine it with the preceding
                // text node, if any
                if (text == " ")
                {
                    if (props.InlineContainer != null)
                    {
                        //assert(props.inline_container->last != NULL);

                        props.InlineContainer.Last.Space = int.MaxValue; //UNKNOWN_WIDTH;
                    }

                    return true;
                }

                if (props.InlineContainer == null)
                {
                    // Child of a block without a current container
                    // (i.e. this box is the first child of its parent, or
                    // was preceded by block-level siblings)
                    props.InlineContainer = new Box(null, null, false,
                        0, string.Empty, string.Empty, string.Empty);

                    props.InlineContainer.Type = BoxType.BOX_INLINE_CONTAINER;

                    props.ContainingBlock.AddChild(props.InlineContainer);
                }

                // \todo Dropping const here is not clever
                var box = new Box(null, props.ParentStyle, false, /*props.href*/0, props.Target, props.Title, string.Empty);
                box.Type = BoxType.BOX_TEXT;
                box.Text = text;
                box.Length = box.Text.Length;

                // strip ending space char off
                if (box.Length > 1 && box.Text[box.Length - 1] == ' ')
                {
                    box.Space = int.MaxValue;//UNKNOWN_WIDTH;
                    box.Length--;
                    box.Text = box.Text.Substring(0, box.Length);
                }

                if (props.ParentStyle.ComputedTextTransform() != CssTextTransformEnum.CSS_TEXT_TRANSFORM_NONE)
                    box.Text = TextTransform(box.Text, props.ParentStyle.ComputedTextTransform());

                props.InlineContainer.AddChild(box);

                if (box.Text[0] == ' ')
                {
                    box.Length--;

                    Log.Unimplemented();
                    //memmove(box->text, &box->text[1], box->length);

                    //if (box->prev != NULL)
                    //  box->prev->space = UNKNOWN_WIDTH;
                }
            }
            else
            {
                // white-space: pre 

                var text_len = content.Length;
                var white_space = props.ParentStyle.ComputedWhitespace();

                // note: pre-wrap/pre-line are unimplemented
                //Debug.Assert(white_space == CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE ||
                //        white_space == CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE_LINE ||
                //        white_space == CssWhiteSpaceEnum.CSS_WHITE_SPACE_PRE_WRAP);

                // TODO: Handle tabs properly
                var text = content.Replace('\t', ' ');

                if (props.ParentStyle.ComputedTextTransform() != CssTextTransformEnum.CSS_TEXT_TRANSFORM_NONE)
                {
                    //box_text_transform(text, strlen(text), css_computed_text_transform(props.parent_style));
                    Log.Unimplemented();
                }

                int current = 0;

                // swallow a single leading new line
                if (((uint)props.ContainingBlock.Flags & (uint)BoxFlags.PRE_STRIP) != 0)
                {
                    Log.Unimplemented();
                    /*
                    switch (*current)
                    {
                        case '\n':
                            current++;
                            break;
                        case '\r':
                            current++;
                            if (*current == '\n')
                                current++;
                            break;
                    }*/

                    props.ContainingBlock.Flags = (BoxFlags)((uint)props.ContainingBlock.Flags & ~(uint)BoxFlags.PRE_STRIP);
                }

                do
                {
                    int len = text.IndexOfAny(new char[] { '\r', '\n' }, current);
                    if (len == -1)
                    {
                        len = text.Length;
                    }
                    else
                    {
                        len -= current; // Adjust length to be local to the current index
                    }

                    if (props.InlineContainer == null)
                    {
                        // Child of a block without a current container
                        // (i.e. this box is the first child of its
                        // parent, or was preceded by block-level
                        // siblings)
                        props.InlineContainer = new Box(null, null, false, 0, null, null, null);
                        props.InlineContainer.Type = BoxType.BOX_INLINE_CONTAINER;
                        props.ContainingBlock.AddChild(props.InlineContainer);
                    }

                    // \todo Dropping const isn't clever
                    var box = new Box(null,
                                  props.ParentStyle,
                                  false,
                                  0, //props.href,
                                  props.Target,
                                  props.Title,
                                  null);

                    box.Type = BoxType.BOX_TEXT;
                    box.Text = text.Substring(current, len);
                    box.Length = box.Text.Length;
                    props.InlineContainer.AddChild(box);

                    //current[len] = old;

                    current += len;

                    if (current < text.Length)
                    {
                        // Linebreak: create new inline container
                        props.InlineContainer = new Box(null, null, false, 0, null, null, null);
                        props.InlineContainer.Type = BoxType.BOX_INLINE_CONTAINER;
                        props.ContainingBlock.AddChild(props.InlineContainer);

                        if (text[current] == '\r' || text[current] == '\n')
                            current += 2;
                        else
                            current++;
                    }
                } while (current < text.Length);
            }

            return true;
        }

        public static bool BoxIsRoot(XmlNode n)
        {
            var p = n.ParentNode;

            if (p == null)
                return true;

            if (p.NodeType == XmlNodeType.Document)
                return true;

            return false;
        }

        // box_construct.c:804
        private XmlNode NextNode(XmlNode n, bool ConvertChildren)
        {
            XmlNode next = null;
            bool HasChildren = false;

            if (n.ChildNodes != null && n.ChildNodes.Count > 0)
                HasChildren = true;

            if (ConvertChildren && HasChildren)
            {
                next = n.ChildNodes[0];
            }
            else
            {
                next = n.NextSibling;

                if (next != null)
                {
                    if (BoxForNode(n) != null)
                    {
                        ConstructElementAfter(n);
                    }
                }
                else
                {
                    if (BoxForNode(n) != null)
                    {
                        ConstructElementAfter(n);
                    }

                    while (BoxIsRoot(n) == false)
                    {
                        XmlNode parent = null;
                        XmlNode parent_next = null;

                        parent = n.ParentNode;

                        parent_next = parent.NextSibling;

                        if (parent_next != null)
                        {
                            break;
                        }

                        n = parent;
                        parent = null;

                        if (BoxForNode(n) != null)
                        {
                            ConstructElementAfter(n);
                        }
                    }

                    if (BoxIsRoot(n) == false)
                    {
                        XmlNode parent = null;

                        parent = n.ParentNode;

                        next = parent.NextSibling;

                        if (BoxForNode(parent) != null)
                        {
                            ConstructElementAfter(parent);
                        }
                    }

                }
            }

            return next;
        }

		// box_construct.c:917
		string TextTransform(string s, CssTextTransformEnum tt)
        {
			switch (tt)
			{
				case CssTextTransformEnum.CSS_TEXT_TRANSFORM_UPPERCASE:
					/*for (i = 0; i < len; ++i)
						if ((unsigned char) s[i] < 0x80)
					s[i] = ascii_to_upper(s[i]);
					break;*/
					Log.Unimplemented();
					return s;
				case CssTextTransformEnum.CSS_TEXT_TRANSFORM_LOWERCASE:
					/*for (i = 0; i < len; ++i)
						if ((unsigned char) s[i] < 0x80)
					s[i] = ascii_to_lower(s[i]);
					break;*/
					Log.Unimplemented();
					return s;
				case CssTextTransformEnum.CSS_TEXT_TRANSFORM_CAPITALIZE:
                    /*
					if ((unsigned char) s[0] < 0x80)
				s[0] = ascii_to_upper(s[0]);
					for (i = 1; i < len; ++i)
						if ((unsigned char) s[i] < 0x80 &&
								ascii_is_space(s[i - 1]))
					s[i] = ascii_to_upper(s[i]);
					break;*/
                    Log.Unimplemented();
                    return s;
				default:
                    return s;
			}
		}

		// box_construct.c:1215 - convert_xml_to_box()
		// Convert an ELEMENT node to a box tree fragment,
		// then schedule conversion of the next ELEMENT node
		void ConvertXmlToBox()
        {
            XmlNode next;
            bool convert_children;
            int num_processed = 0;
            int max_processed_before_yield = 1000;

            //Console.WriteLine("{0}", n.Length);
            //n.ChildNodes.Count
            //n.ChildNodes[i]

            do
            {
                convert_children = true;

                Debug.Assert(ctx_n != null);

                ctx_n.Dump(); // AB

                if (BoxConstructElement(ctx_n, ref convert_children) == false)
                {
                    // ctx->cb(ctx->content, false);
                    Console.WriteLine("Error in BoxConstructElement");
                    return;
                }

                // Find next element to process, converting text nodes as we go
                next = NextNode(ctx_n, convert_children);
                while (next != null)
                {
                    next.Dump(); // AB

                    if (next.NodeType == XmlNodeType.Element)
                        break;

                    if (next.NodeType == XmlNodeType.Text)
                    {
                        ctx_n = next;

                        Console.WriteLine("{0}", next.Value);
                        if (BoxConstructText(ctx_n) == false)
                        {
                            //ctx->cb(ctx->content, false);
                            return;
                        }
                    }

                    next = NextNode(next, true);
                }

                ctx_n = next;

                if (next == null)
                {
                    // Conversion complete
                    Box root = new Box(null, null, false, 0, "", "", "");
                    root.Width = 0;
                    root.MaxWidth = 0;
                    root.Type = BoxType.BOX_BLOCK;
                    root.Children = RootBox;
                    root.Last = RootBox;
                    root.Children.Parent = root;

                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (StreamWriter sw = new StreamWriter(stream))
                        {
                            root.Dump(sw, 0, true);
                        }
                        Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));
                    }

                    /*
			        // \todo Remove box_normalise_block
			        if (box_normalise_block(&root, ctx->root_box,
					        ctx->content) == false) {
				        ctx->cb(ctx->content, false);
                    } else {*/
                    //Layout = root.Children;
                    //Layout.Parent = null;

                    //ctx->cb(ctx->content, true);
                    /*}

                    assert(ctx->n == NULL);
                    free(ctx);*/

                    RootBox.Parent = null;

                    return;
                }
            } while (++num_processed < max_processed_before_yield);

            // More work to do: schedule a continuation
            //guit->misc->schedule(0, (void*)convert_xml_to_box, ctx);
            Console.WriteLine("Recursively calling ConvertXmlToBox()!!!");
            ConvertXmlToBox();
        }

        // box_construct.c:1322 - dom_to_box()
        public void DomToBox(XmlNode n)
        {
            // https://github.com/netsurf-browser/netsurf/blob/8615964c3fd381ef6d9a20487b9120135182dfd1/content/handlers/html/box_construct.c

            // Once
            ctx_n = n;
            RootBox = null;

            ConvertXmlToBox();
        }
    }
}
