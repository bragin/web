using HtmlParserSharp;
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

    public enum BoxSide
    {
        TOP, RIGHT, BOTTOM, LEFT
    }

    // Transient properties for construction of current node
    internal struct BoxConstructProps
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



    // Node in box tree. All dimensions are in pixels
    internal class Box
    {
        // Type of box
        public BoxType Type;

        // Box flags
        public BoxFlags Flags;

        // DOM node that generated this box or NULL
        XmlNode Node;

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
        Box Next;

        // Previous sibling box, or NULL.
        Box Prev;

        // First child box, or NULL.
        public Box Children;

        // Last child box, or NULL.
        public Box Last;

        // Parent box, or NULL.
        Box Parent;

        // INLINE_END box corresponding to this INLINE box, or INLINE
        // box corresponding to this INLINE_END box.
        public Box InlineEnd;


        // First float child box, or NULL. Float boxes are in the tree
        // twice, in this list for the block box which defines the
        // area for floats, and also in the standard tree given by
        // children, next, prev, etc.
        Box float_children;

        // Next sibling float box.
        Box next_float;

        // If box is a float, points to box's containing block
        Box float_container;

        // Level below which subsequent floats must be cleared.  This
        // is used only for boxes with float_children
        int clear_level;

        // Level below which floats have been placed.
        int cached_place_below_level;

        // Coordinate of left padding edge relative to parent box, or
        // relative to ancestor that contains this box in
        // float_children for FLOAT_.
        int x;

        // Coordinate of top padding edge, relative as for x
        int y;

        // Width of content box (excluding padding etc.)
        int width;

        // Height of content box (excluding padding etc.)
        int height;

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
        int descendant_x0;  /**< left edge of descendants */
        int descendant_y0;  /**< top edge of descendants */
        int descendant_x1;  /**< right edge of descendants */
        int descendant_y1;  /**< bottom edge of descendants */

        // Margin: TOP, RIGHT, BOTTOM, LEFT.
        int[] margin = new int[4];

        // Padding: TOP, RIGHT, BOTTOM, LEFT.
        int[] padding = new int[4];

        // Border: TOP, RIGHT, BOTTOM, LEFT.
        //struct box_border border[4];

        // Horizontal scroll.
        //struct scrollbar *scroll_x;

        // Vertical scroll.
        //struct scrollbar *scroll_y;

        // Width of box taking all line breaks (including margins etc). Must be non-negative
        int min_width;

        // Width that would be taken with no line breaks. Must be non-negative
        int max_width;

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


        // Object in this box (usually an image), or NULL if none.
        //struct hlcache_handle* object;

        // Parameters for the object, or NULL.
        // struct object_params *object_params;


        // Iframe's browser_window, or NULL if none
        //struct browser_window *iframe;

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
            this.x = this.y = 0;
            this.width = Int32.MaxValue;
            this.height = 0;
            this.descendant_x0 = this.descendant_y0 = 0;
            this.descendant_x1 = this.descendant_y1 = 0;
            for (int i = 0; i != 4; i++)
            {
                this.margin[i] = this.padding[i] = 0;
                //this.border[i].width = 0;
            }
            //this.scroll_x = this.scroll_y = NULL;
            this.min_width = 0;
            this.max_width = Int32.MaxValue;
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
            //box->background = NULL;
            //box->object = NULL;
            //box->object_params = NULL;
            //box->iframe = NULL;
            //box->node = NULL;
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

            sw.Write($"x{x} y{y} w{width} h{height} ");

            /*if (box->max_width != UNKNOWN_MAX_WIDTH) {
                sw.Write("min%i max%i ", box->min_width, box->max_width);
            }*/

            sw.Write($"({descendant_x0} {descendant_y0} {descendant_x1} {descendant_y1}) ");

            sw.Write($"m({margin[0]} {margin[3]} {margin[2]} {margin[1]}) "); // top left bottom right

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

    internal class BoxTree
    {
        // Context
        XmlNode ctx_n; // ctx->n. Should not be used really
        Box RootBox; // ctx->root_box

        Dictionary<XmlNode, Box> NodeBoxLookup; // Used to find box for a given node

        Box Layout; // html->content->layout (root of the whatever), probably needs to be moved

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

            Console.WriteLine($"style:\t{state.Element.Name}\tSELECTED\n");

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

        // box_construct.c:468 - box_construct_element()
        private bool BoxConstructElement(XmlNode node, ref bool ConvertChildren)
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
                // box_free_box(box);

                *convert_children = false;

                return true;*/
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

            if (ConvertChildren)
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
                    /*
                    props.inline_container = box_create(NULL, NULL, false,
                            NULL, NULL, NULL, NULL, content->bctx);
                    if (props.inline_container == NULL)
                        return;

                    props.inline_container->type = BOX_INLINE_CONTAINER;

                    box_add_child(props.containing_block,
                            props.inline_container);
                    */
                    Log.Unimplemented();
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
                    /*
                    props.inline_container = box_create(NULL, NULL, false,
                            NULL, NULL, NULL, NULL, ctx->bctx);
                    if (props.inline_container == NULL)
                    {
                        free(text);
                        return false;
                    }

                    props.inline_container->type = BOX_INLINE_CONTAINER;

                    box_add_child(props.containing_block,
                            props.inline_container);
                    */
                    Log.Unimplemented();
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
                    box.Text = box.Text.Substring(0, box.Length - 1);
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
                    props.ContainingBlock.AddChild(box);

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

		// box_construct.c:1193 - convert_xml_to_box()
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
                    root.Type = BoxType.BOX_BLOCK;
                    root.Children = RootBox;
                    root.Last = RootBox;

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
                    } else {
				        ctx->content->layout = root.children;
				        ctx->content->layout->parent = NULL;

				        ctx->cb(ctx->content, true);
                    }

                    assert(ctx->n == NULL);
                    free(ctx);*/


                    return;
                }
            } while (++num_processed < max_processed_before_yield);

            // More work to do: schedule a continuation
            //guit->misc->schedule(0, (void*)convert_xml_to_box, ctx);
            Console.WriteLine("Recursively calling ConvertXmlToBox()!!!");
            ConvertXmlToBox();
        }

        // box_construct.c:1298 - dom_to_box()
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
