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
        string Id;

        // Next sibling box, or NULL.
        Box Next;

        // Previous sibling box, or NULL.
        Box Prev;

        // First child box, or NULL.
        Box Children;

        // Last child box, or NULL.
        public Box Last;

        // Parent box, or NULL.
        Box Parent;

        // INLINE_END box corresponding to this INLINE box, or INLINE
        // box corresponding to this INLINE_END box.
        Box inline_end;


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
        string Text;

        // Length of text.
        // size_t length;

        // Width of space after current text (depends on font and size).
        int space;

        // Byte offset within a textual representation of this content.
        //size_t byte_offset;


        // Link, or NULL.
        //struct nsurl *href;

        // Link target, or NULL.
        string Target;

        // Title, or NULL.
        string Title;


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
            //box->space = 0;
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

    }

    internal class BoxTree
    {
        // Context
        XmlNode ctx_n; // ctx->n. Should not be used really
        Box RootBox; // ctx->root_box

        Dictionary<XmlNode, Box> NodeBoxLookup; // Used to find box for a given node

        Box Layout; // html->content->layout (root of the whatever), probably needs to be moved

        HtmlContent Content; // FIXME: This is incorrect and needs to be removed

        CssProps Properties; // Not sure it's the right place either

        public BoxTree(HtmlContent content)
        {
            NodeBoxLookup = new Dictionary<XmlNode, Box>();
            Content = content;
            Properties = new CssProps();
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
                Log.Unimplemented("libcss/src/select/select.c:1189 - css_select_style()");
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
                    Properties.SetInitial(state, i, CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE, parent);
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
            // STUB

            // Select style for node
            var styles = CssSelectStyle(node, ref unitCtx, ref media, inlineStyle);

            // If there's a parent style, compose with partial to obtain
            // complete computed style for element
            if (parentStyle != null)
            {
                Log.Unimplemented("content/handlers/css/select.c:253 - nscss_get_style()");

                /* Complete the computed style, by composing with the parent
                 * element's style */
                /*
                error = css_computed_style_compose(ctx->parent_style,
                        styles->styles[CSS_PSEUDO_ELEMENT_NONE],
                        unit_len_ctx, &composed);
                if (error != CSS_OK)
                {
                    css_select_results_destroy(styles);
                    return NULL;
                }

                // Replace select_results style with composed style
                css_computed_style_destroy(
                        styles->styles[CSS_PSEUDO_ELEMENT_NONE]);
                styles->styles[CSS_PSEUDO_ELEMENT_NONE] = composed;*/
            }
            Log.Unimplemented("content/handlers/css/select.c:253 - nscss_get_style()");
            
            /*
            for (pseudo_element = CSS_PSEUDO_ELEMENT_NONE + 1;
                    pseudo_element < CSS_PSEUDO_ELEMENT_COUNT;
                    pseudo_element++)
            {

                if (pseudo_element == CSS_PSEUDO_ELEMENT_FIRST_LETTER ||
                        pseudo_element == CSS_PSEUDO_ELEMENT_FIRST_LINE)
                    // TODO: Handle first-line and first-letter pseudo
                    //       element computed style completion
                    continue;

                if (styles->styles[pseudo_element] == NULL)
                    // There were no rules concerning this pseudo element
                    continue;

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
                styles->styles[pseudo_element] = composed;
            }*/

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

            //if (node.Style.Count > 0)
            //  RootBox = box;

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

                        css_display = ns_computed_display_static(box->style);

                        // Set box type from computed display
                        if ((css_computed_position(box->style) == CSS_POSITION_ABSOLUTE ||
                             css_computed_position(box->style) == CSS_POSITION_FIXED) &&
                                (css_display == CSS_DISPLAY_INLINE ||
                                 css_display == CSS_DISPLAY_INLINE_BLOCK ||
                                 css_display == CSS_DISPLAY_INLINE_TABLE ||
                                 css_display == CSS_DISPLAY_INLINE_FLEX))
                        {
                            // Special case for absolute positioning: make absolute inlines
                            // into inline block so that the boxes are constructed in an
                            // inline container as if they were not absolutely positioned.
                            // Layout expects and handles this.
                            box->type = box_map[CSS_DISPLAY_INLINE_BLOCK];
                        }
                        else if (props.node_is_root)
                        {
                            // Special case for root element: force it to BLOCK, or the
                            // rest of the layout will break.
                            box->type = BOX_BLOCK;
                        }
                        else
                        {
                            // Normal mapping
                            box->type = box_map[ns_computed_display(box->style,
                                    props.node_is_root)];

                            if (props.containing_block->type == BOX_FLEX ||
                                props.containing_block->type == BOX_INLINE_FLEX)
                            {
                                // Blockification
                                switch (box->type)
                                {
                                    case BOX_INLINE_FLEX:
                                        box->type = BOX_FLEX;
                                        break;
                                    case BOX_INLINE_BLOCK:
                                        box->type = BOX_BLOCK;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

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

            /*
            if (box->type == BOX_NONE || (ns_computed_display(box->style,
                    props.node_is_root) == CSS_DISPLAY_NONE &&
                    props.node_is_root == false))
            {
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

                return true;
            }
    */
            // Attach DOM node to box
            AttachNodeToBox(node, box);


            // Attach box to DOM node/
            box.AttachToNode(node);

            /*
                    if (props.inline_container == NULL &&
                            (box->type == BOX_INLINE ||
                             box->type == BOX_BR ||
                             box->type == BOX_INLINE_BLOCK ||
                             box->type == BOX_INLINE_FLEX ||
                             (box__style_is_float(box) &&
                              !box__containing_block_is_flex(&props))) &&
                            props.node_is_root == false)
                    {
                        // Found an inline child of a block without a current container
                        // (i.e. this box is the first child of its parent, or was
                        // preceded by block-level siblings)
                        assert(props.containing_block != NULL &&
                                "Box must have containing block.");

                        props.inline_container = box_create(NULL, NULL, false, NULL,
                                NULL, NULL, NULL, ctx->bctx);
                        if (props.inline_container == NULL)
                            return false;

                        props.inline_container->type = BOX_INLINE_CONTAINER;

                        props.ContainingBlock.AddChild(props.InlineContainer);
                    }
            */

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

            if (box.Type == BoxType.BOX_INLINE || box.Type == BoxType.BOX_BR ||
                    box.Type == BoxType.BOX_INLINE_FLEX ||
                    box.Type == BoxType.BOX_INLINE_BLOCK)
            {
                // Inline container must exist, as we'll have
                // created it above if it didn't
                //assert(props.inline_container != NULL);

                props.InlineContainer.AddChild(box);
            }
            else
            {
                /*
                if (ns_computed_display(box->style, props.node_is_root) ==
                        CSS_DISPLAY_LIST_ITEM)
                {
                    // List item: compute marker
                    if (box_construct_marker(box, props.title, ctx,
                            props.containing_block) == false)
                        return false;
                }*/

                /*if (props.node_is_root == false &&
                        box__containing_block_is_flex(&props) == false &&
                        (css_computed_float(box->style) == CSS_FLOAT_LEFT ||
                        css_computed_float(box->style) == CSS_FLOAT_RIGHT))
                {
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
                    flt.AddChild(box);
                }
                else*/
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
            //const css_computed_content_item* c_item;

            // Nothing to generate if the parent box is not a block
            if (box.Type != BoxType.BOX_BLOCK)
                return;

            Log.Unimplemented("box_construct.c:308 - box_construct_generate");

            /* To determine if an element has a pseudo element, we select
	         * for it and test to see if the returned style's content
	         * property is set to normal. */
            /*
            if (style == NULL ||
			        css_computed_content(style, &c_item) ==
			        CSS_CONTENT_NORMAL) {
		        // No pseudo element
		        return;
	        }

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
                /*
		        // Insert INLINE_END into containing block
		        struct box *inline_end;
		        bool has_children;
                dom_exception err;

                err = dom_node_has_child_nodes(n, &has_children);
		        if (err != DOM_NO_ERR)
			        return;

		        if (has_children == false || (box->flags & CONVERT_CHILDREN) == 0)
                {
			        // No children, or didn't want children converted
			        return;
		        }

		        if (props.inline_container == NULL)
                {
			        // Create inline container if we don't have one
			        props.inline_container = box_create(NULL, NULL, false,
                            NULL, NULL, NULL, NULL, content->bctx);
			        if (props.inline_container == NULL)
				        return;

			        props.inline_container->type = BOX_INLINE_CONTAINER;

			        box_add_child(props.containing_block,
                            props.inline_container);
                }

                inline_end = box_create(NULL, box->style, false,
                        box->href, box->target, box->title,
                        box->id == NULL? NULL :

                        lwc_string_ref(box->id), content->bctx);

                if (inline_end != NULL)
                {
                    inline_end->type = BOX_INLINE_END;

			        assert(props.inline_container != NULL);

                    box_add_child(props.inline_container, inline_end);

                    box->inline_end = inline_end;
	                inline_end->inline_end = box;
	            }*/
            }
            else if (((int)box.Flags & (int)BoxFlags.IS_REPLACED) == 0)
            {
                // Handle the :after pseudo element
                //box_construct_generate(n, content, box, box->styles->styles[CSS_PSEUDO_ELEMENT_AFTER]);
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

            //assert(props.containing_block != NULL);
            /*
                err = dom_characterdata_get_data(ctx->n, &content);
	        if (err != DOM_NO_ERR || content == NULL)
		        return false;

	        if (css_computed_white_space(props.parent_style) ==
			        CSS_WHITE_SPACE_NORMAL ||
			        css_computed_white_space(props.parent_style) ==
			        CSS_WHITE_SPACE_NOWRAP)
            {
		        char* text;

                text = squash_whitespace(dom_string_data(content));

                dom_string_unref(content);

		        if (text == NULL)
			        return false;

		        // if the text is just a space, combine it with the preceding
		         // text node, if any
		        if (text[0] == ' ' && text[1] == 0)
                {
			        if (props.inline_container != NULL)
                    {
				        assert(props.inline_container->last != NULL);

                        props.inline_container->last->space = UNKNOWN_WIDTH;
			        }

                    free(text);

			        return true;
		        }

                if (props.inline_container == NULL)
                {
                    // Child of a block without a current container
                    // (i.e. this box is the first child of its parent, or
                    // was preceded by block-level siblings)
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
                }

                // \todo Dropping const here is not clever
                box = box_create(NULL,
                        (css_computed_style*)props.parent_style,
                        false, props.href, props.target, props.title,
                        NULL, ctx->bctx);
                if (box == NULL)
                {
                    free(text);
                    return false;
                }

                box->type = BOX_TEXT;

                box->text = talloc_strdup(ctx->bctx, text);
                free(text);
                if (box->text == NULL)
                    return false;

                box->length = strlen(box->text);

                // strip ending space char off
                if (box->length > 1 && box->text[box->length - 1] == ' ')
                {
                    box->space = UNKNOWN_WIDTH;
                    box->length--;
                }

                if (css_computed_text_transform(props.parent_style) !=
                        CSS_TEXT_TRANSFORM_NONE)
                    box_text_transform(box->text, box->length,
                        css_computed_text_transform(
                            props.parent_style));

                box_add_child(props.inline_container, box);

                if (box->text[0] == ' ')
                {
                    box->length--;

                    memmove(box->text, &box->text[1], box->length);

                    if (box->prev != NULL)
                        box->prev->space = UNKNOWN_WIDTH;
                }
	        }
            else
            {
                    // white-space: pre 
                    char* text;
                    size_t text_len = dom_string_byte_length(content);
                    size_t i;
                    char* current;

                        enum css_white_space_e white_space =
                                css_computed_white_space(props.parent_style);

                // note: pre-wrap/pre-line are unimplemented
                assert(white_space == CSS_WHITE_SPACE_PRE ||
                        white_space == CSS_WHITE_SPACE_PRE_LINE ||
                        white_space == CSS_WHITE_SPACE_PRE_WRAP);

                text = malloc(text_len + 1);
                dom_string_unref(content);

                if (text == NULL)
                    return false;

                memcpy(text, dom_string_data(content), text_len);
                text[text_len] = '\0';

                // TODO: Handle tabs properly
                for (i = 0; i < text_len; i++)
                    if (text[i] == '\t')
                        text[i] = ' ';

                if (css_computed_text_transform(props.parent_style) !=
                        CSS_TEXT_TRANSFORM_NONE)
                    box_text_transform(text, strlen(text),
                        css_computed_text_transform(
                                props.parent_style));

                current = text;

                // swallow a single leading new line
                if (props.containing_block->flags & PRE_STRIP)
                {
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
                    }
                    props.containing_block->flags &= ~PRE_STRIP;
                }

                do
                {
                    size_t len = strcspn(current, "\r\n");

                    char old = current[len];

                    current[len] = 0;

                    if (props.inline_container == NULL)
                    {
                        // Child of a block without a current container
                        // (i.e. this box is the first child of its
                        // parent, or was preceded by block-level
                        // siblings)
                        props.inline_container = box_create(NULL, NULL,
                                false, NULL, NULL, NULL, NULL,
                                ctx->bctx);
                        if (props.inline_container == NULL)
                        {
                            free(text);
                            return false;
                        }

                        props.inline_container->type =
                                BOX_INLINE_CONTAINER;

                        box_add_child(props.containing_block,
                                props.inline_container);
                    }

                    // \todo Dropping const isn't clever
                    box = box_create(NULL,
                        (css_computed_style*)props.parent_style,
                        false, props.href, props.target, props.title,
                        NULL, ctx->bctx);
                    if (box == NULL)
                    {
                        free(text);
                        return false;
                    }

                    box->type = BOX_TEXT;

                    box->text = talloc_strdup(ctx->bctx, current);
                    if (box->text == NULL)
                    {
                        free(text);
                        return false;
                    }

                    box->length = strlen(box->text);

                    box_add_child(props.inline_container, box);

                    current[len] = old;

                    current += len;

                    if (current[0] != '\0')
                    {
                        // Linebreak: create new inline container
                        props.inline_container = box_create(NULL, NULL,
                                false, NULL, NULL, NULL, NULL,
                                ctx->bctx);
                        if (props.inline_container == NULL)
                        {
                            free(text);
                            return false;
                        }

                        props.inline_container->type =
                                BOX_INLINE_CONTAINER;

                        box_add_child(props.containing_block,
                                props.inline_container);

                        if (current[0] == '\r' && current[1] == '\n')
                            current += 2;
                        else
                            current++;
                    }
                } while (*current);

                free(text);
	        }
*/
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

        // box_construct.c:1298
        public void DomToBox(XmlNode n)
        {
            // https://github.com/netsurf-browser/netsurf/blob/8615964c3fd381ef6d9a20487b9120135182dfd1/content/handlers/html/box_construct.c

            // Once
            ctx_n = n;
            RootBox = null;

            // convert_xml_to_box()
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
                    if (next.NodeType == XmlNodeType.Element)
                        break;

                    if (next.NodeType == XmlNodeType.Text)
                    {
                        ctx_n = next;

                        Console.WriteLine("{0}", next.ToString());
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
                    /*
                    struct box root;

			        memset(&root, 0, sizeof(root));

                    root.type = BOX_BLOCK;
    		        root.children = root.last = ctx->root_box;
			        root.children->parent = &root;

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
        }
    }
}
