using SkiaSharpOpenGLBenchmark;
using SkiaSharpOpenGLBenchmark.css;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace SkiaSharpOpenGLBenchmark
{
    public struct HtmlStylesheet
    {
        public CssStylesheet Sheet;
        bool Modified;
        XmlNode Node;
        public bool Unused;

        public HtmlStylesheet()
        {
            Sheet = null;
            Modified = false;
            Node = null;
            Unused = false;
        }
    }

	public struct Rect
	{
        public int x0, y0; // Top left
        public int x1, y1; // Bottom right
    }

    // content.h:40
    public struct RedrawData
    {
        public int X; // coordinate for top-left of redraw
        public int Y; // coordinate for top-left of redraw

        // dimensions to render content at (for scaling contents with
        public int Width; // horizontal dimension
        public int Height; // vertical dimension

        // The background colour
        public uint BackgroundColor;

        // Scale for redraw
        //  (for scaling contents without intrinsic dimensions)
        public double Scale; // Scale factor for redraw

        public bool RepeatX; // whether content is tiled in x direction
        public bool RepeatY; // whether content is tiled in y direction
    }

    // private.h:93
    public class HtmlContent
    {
        //CQ Dom;

        bool IsReflowing; // Whether a layout (reflow) is in progress
		bool HadInitialLayout; // Whether an initial layout has been done
		bool IsReformat;

        BoxTree Layout;

        uint BackgroundColor;

        int Width; // Width dimension, if applicable.
		int Height; // Height dimension, if applicable.

		int AvailableWidth; // Viewport width.
		int AvailableHeight; // Viewport height.

		public LinkedList<HtmlStylesheet> Stylesheets; // Stylesheets. Each may be NULL.
        public CssSelectionContext SelectionCtx; // Style selection context
        public CssMedia Media; // Style selection media specification
        public CssUnitCtx UnitLenCtx; // CSS length conversion context for document.

        string Universal; // Universal selector

        public Plotter Plot; // Instead of font_func and other things

        // html.c:590
        public HtmlContent(Plotter plot)
        {
            // Init content
            Width = 0;
            Height = 0;

            // Init HTML data
            IsReflowing = false;
            IsReformat = false;

            Media.Type = CssMediaType.CSS_MEDIA_SCREEN;
            Universal = CssStrings.Universal;

            // Init stylesheets
            Stylesheets = new LinkedList<HtmlStylesheet>();

            /* stylesheet 0 is the base style sheet,
             * stylesheet 1 is the quirks mode style sheet,
             * stylesheet 2 is the adblocking stylesheet,
             * stylesheet 3 is the user stylesheet */

            LoadStylesheetFromFile("default.css");
            LoadStylesheetFromFile("quirks.css");
            LoadStylesheetFromFile("adblock.css");
            LoadStylesheetFromFile("user.css");

            Plot = plot;
        }

        public void LoadStylesheetFromFile(string file)
        {
            var contents = File.ReadAllText(file);
            if (contents.Length == 0) return;

            //var parser = new StylesheetParser();
            //var stylesheet = parser.Parse(contents);

            //Stylesheets.AddLast(stylesheet);

            var css = new CssStylesheet("", file, file, false);
            css.AppendData(contents);

            var hs = new HtmlStylesheet();
			hs.Sheet = css;
			Stylesheets.AddLast(hs);
		}

		// css.c:648
		void CreateSelectionContext()
        {
            SelectionCtx = new CssSelectionContext();

            // check that the base stylesheet loaded; layout fails without it
            if (Stylesheets.Count == 0)
            //if (Stylesheets.First.Value.Sheet == null)
            {
                Log.Unimplemented("Base stylesheet is missing");
            }

            // Add sheets to it
            int i = 0;
            foreach (var hsheet in Stylesheets)
            {
                CssOrigin origin = CssOrigin.CSS_ORIGIN_AUTHOR;

                /* Filter out stylesheets for non-screen media. */
                /* TODO: We should probably pass the sheet in anyway, and let
                 *       libcss handle the filtering.
                 */
                if (hsheet.Unused)
                {
                    i++;
                    continue;
                }

                if (i < (int)CssStylesheetEnum.USER)
                    origin = CssOrigin.CSS_ORIGIN_UA;
                else if (i < (int)CssStylesheetEnum.START)
                    origin = CssOrigin.CSS_ORIGIN_USER;

                var sheet = hsheet.Sheet;
                // FIXME: sheet = nscss_get_stylesheet(hsheet->sheet);

                if (sheet != null)
                {
                    // TODO: Pass the sheet's full media query, instead of "screen".
                    SelectionCtx.AppendSheet(sheet, origin, "screen");
                }

                i++;
            }
        }

        public static string FormatXMLString(string sUnformattedXML)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sUnformattedXML);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }

        // html.c:341 - html_finish_conversion(), kind of
        public void LoadDocument()
        {
            // file://S:/SAFE/Projects/NB/web/test2.html
            var contents = File.ReadAllText("test.html");
            if (contents.Length == 0) return;

            var parser = new SimpleHtmlParser();
            var doc = parser.ParseString("<p name=\"coolname\" class=\"mylink fancy\" id=\"idtest\">some text <a href=\"#\">and link</a></p><p>and another p</p>");
            //var doc = parser.ParseUrl("http://nginx.org/");
            //var doc = parser.ParseString(contents);

            //StringWriter stringWriter = new StringWriter();
            //XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            //doc.WriteTo(xmlTextWriter);
            //Console.WriteLine(stringWriter.ToString());

            //Console.WriteLine(doc.DocumentElement.OuterXml);

            //var n = Dom["body"];
            //var cstyle = n.Css("color");
            //var sheets = doc.GetElementsByTagName("style");

            // My CSS stuff
            var css = new CssStylesheet("", "http://nginx.org", "Useragent", false);
            //              0        10        20        30        40        50        60        70        80        90       100       110       120       130
            css.AppendData("html { color: red; font-size: 16px; } .mylink { color: blue; } head { color: green; } #idtest { color: #112233; } a { color: #00ff55; }");

            var hsheet = new HtmlStylesheet();
            hsheet.Sheet = css;
            Stylesheets.AddLast(hsheet);
            //css.Select(SelectionCtx, CssOrigin.CSS_ORIGIN_UA);

            var media = new CssMedia();
            var unitctx = new CssUnitCtx();
            var body = doc.GetElementsByTagName("body")[0].ChildNodes[0];
            //Console.WriteLine(body.OuterXml);
			//Console.WriteLine(doc.OuterXml);
			//var sc = new CssSelectState(body, null, ref media, ref unitctx);

			// FIXME: Testin
			GetDimensions();

            /* If we already have a selection context, then we have already
             * "finished" conversion.  We can get here twice if e.g. some JS
             * adds a new stylesheet, and the stylesheet gets added after
             * the HTML content is initially finished.
             *
             * If we didn't do this, the HTML content would try to rebuild the
             * box tree for the html content when this new stylesheet is ready.
             * NetSurf has no concept of dynamically changing documents, so this
             * would break badly.
             */
            //if (SelectionCtx != null)
            //{
            //NSLOG(netsurf, INFO,
            //        "Ignoring style change: NS layout is static.");
            //return;
            //}

            // create new css selection context
            CreateSelectionContext();

            Layout = new BoxTree(this);
            var html = doc.GetElementsByTagName("html")[0];
			Console.WriteLine(html.OuterXml);
			Layout.DomToBox(html); // Root is set there in the end as ctx->content->layout

            // Get window's dimensions
            int width = 1000;
            int height = 1000;
            Reformat(width, height);

			var rd = new RedrawData();
			rd.Scale = 1;
			rd.Width = width;
			rd.Height = height;

			var clip = new Rect()
			{
				x0 = 0,
				y0 = 0,
				x1 = width,
				y1 = height
			};

			Redraw(rd, clip, false, false);
        }

        // content.c:323 content_reformat()
        // content.c:332 content__reformat()
        // content/handlers/html/html.c:1038
        public void Reformat(int width, int height)
        {
            AvailableWidth = width;
            AvailableHeight = height;

            IsReflowing = true;
            UnitLenCtx.ViewportWidth = Device2CssPixel(new Fixed(width), UnitLenCtx.DeviceDpi);
			UnitLenCtx.ViewportHeight = Device2CssPixel(new Fixed(height), UnitLenCtx.DeviceDpi);

			LayoutDocument(width, height);

            var layout = Layout.RootBox;

			// width and height are at least margin box of document
			Width = layout.X + layout.Padding[(byte)BoxSide.LEFT] + layout.Width +
				layout.Padding[(byte)BoxSide.RIGHT] + layout.Border[(byte)BoxSide.RIGHT].Width +
				layout.Margin[(byte)BoxSide.RIGHT];
			Height = layout.Y + layout.Padding[(byte)BoxSide.TOP] + layout.Height +
				layout.Padding[(byte)BoxSide.BOTTOM] + layout.Border[(byte)BoxSide.BOTTOM].Width +
				layout.Margin[(byte)BoxSide.BOTTOM];

			// if boxes overflow right or bottom edge, expand to contain it
			if (Width < layout.X + layout.DescendantX1)
				Width = layout.X + layout.DescendantX1;
			if (Height < layout.Y + layout.DescendantY1)
				Height = layout.Y + layout.DescendantY1;

			//selection_reinit(htmlc->sel);

			IsReflowing = false;
			HadInitialLayout = true;
		}

        // html_redraw_box_has_background()
        // redraw.c:77
        static bool RedrawBoxHasBackground(Box box)
        {
            if (box.Background != 0)
                return true;

            if (box.Style != null)
            {
                Color colour;

                box.Style.ComputedBackgroundColor(out colour);

                if (colour.IsTransparent() == false)
                    return true;
            }

            return false;
        }

        // html_redraw_find_bg_box
        // redraw.c:100
        /**
         * Find the background box for a box
         *
         * \param box  Box to find background box for
         * \return Pointer to background box, or NULL if there is none
         */
        static Box RedrawFindBgBox(Box box)
        {
            /* Thanks to backwards compatibility, CSS defines the following:
             *
             * + If the box is for the root element and it has a background,
             *   use that (and then process the body box with no special case)
             * + If the box is for the root element and it has no background,
             *   then use the background (if any) from the body element as if
             *   it were specified on the root. Then, when the box for the body
             *   element is processed, ignore the background.
             * + For any other box, just use its own styling.
             */
            if (box.Parent == null)
            {
                /* Root box */
                if (box.RedrawBoxHasBackground())
                    return box;

                /* No background on root box: consider body box, if any */
                if (box.Children != null)
                {
                    if (box.Children.RedrawBoxHasBackground())
                        return box.Children;
                }
            }
            else if (box.Parent != null && box.Parent.Parent == null)
            {
                /* Body box: only render background if root has its own */
                if (box.RedrawBoxHasBackground() &&
                        box.Parent.RedrawBoxHasBackground())
                    return box;
            }
            else
            {
                /* Any other box */
                if (box.RedrawBoxHasBackground())
                    return box;
            }

            return null;
        }

		// text_redraw()
		// redraw.c:172
		/**
		 * Redraw a short text string, complete with highlighting
		 * (for selection/search)
		 *
		 * \param utf8_text pointer to UTF-8 text string
		 * \param utf8_len  length of string, in bytes
		 * \param offset    byte offset within textual representation
		 * \param space     width of space that follows string (0 = no space)
		 * \param fstyle    text style to use (pass text size unscaled)
		 * \param x         x ordinate at which to plot text
		 * \param y         y ordinate at which to plot text
		 * \param clip      pointer to current clip rectangle
		 * \param height    height of text string
		 * \param scale     current display scale (1.0 = 100%)
		 * \param excluded  exclude this text string from the selection
		 * \param c         Content being redrawn.
		 * \param sel       Selection context
		 * \param search    Search context
		 * \param ctx	    current redraw context
		 * \return true iff successful and redraw should proceed
		 */
		bool TextRedraw(string utf8_text,
				int offset,
				int space,
				PlotFontStyle fstyle,
				int x,
				int y,
				Rect clip,
				int height,
				double scale,
				bool excluded)
		{
            bool highlighted = false;

            // FIXME: All other edge-cases unimplemented
            Plot.Text(fstyle, x, y + (int)(height * 0.75 * scale), utf8_text);
            return true;
		}

        // html_redraw_background()
        // redraw.c:600
        bool RedrawBackground(int x, int y, Box box, double scale, Rect clip, uint background_colour,
            Box background, CssUnitCtx unit_len_ctx)
        {
            Log.Unimplemented();
            return true;
        }

        // html_redraw_inline_background()
		// redraw.c:829
        bool RedrawInlineBackground(int x, int y, Box box, double scale, Rect clip,
			Rect b, bool first, bool last, uint background_colour, CssUnitCtx unit_len_ctx)
        {
            Log.Unimplemented();
            return true;
        }

        // html_redraw_borders()
        // redraw_border.c:
        bool RedrawBorders(Box box, int x_parent, int y_parent, int p_width, int p_height, Rect clip, double scale)
        {
            Log.Unimplemented();
            return true;
        }

        // html_redraw_inline_borders()
        // redraw_borders.c:713
        bool
        RedrawInlineBorders(Box box, Rect b, Rect clip, double scale, bool first, bool last)
        {
            Log.Unimplemented();
            return true;
        }

        // html_redraw_text_box
		// redraw.c:1137
		bool RedrawTextBox(Box box, int x, int y, Rect clip, double scale,
				uint current_background_color)
		{
			bool excluded = false; //(box->object != NULL);
			PlotFontStyle fstyle;

            fstyle = box.Style.FontPlotStyle(UnitLenCtx);
			fstyle.Background = current_background_color;

			if (!TextRedraw(box.Text,
					 //box->length,
					 0, //box.ByteOffset,
					 box.Space,
					 fstyle,
					 x, y,
					 clip,
					 box.Height,
					 scale,
					 excluded/*,
					 (struct content *)html,
					 html->sel,
					 ctx*/))
				return false;

			return true;
		}

        // html_redraw_box_children()
        // redraw.c:1187
        /**
         * Draw the various children of a box.
         *
         * \param  html	     html content
         * \param  box	     box to draw children of
         * \param  x_parent  coordinate of parent box
         * \param  y_parent  coordinate of parent box
         * \param  clip      clip rectangle
         * \param  scale     scale for redraw
         * \param  current_background_color  background colour under this box
         * \param  ctx	     current redraw context
         * \return true if successful, false otherwise
         */
        bool RedrawBoxChildren(Box box, int x_parent, int y_parent, Rect clip, double scale, uint current_background_color)
        {
			Box c;

			for (c = box.Children; c != null; c = c.Next)
			{
				if (c.Type != BoxType.BOX_FLOAT_LEFT && c.Type != BoxType.BOX_FLOAT_RIGHT)
					if (!RedrawBox(c, x_parent + box.X - box.ScrollX.GetOffset(),
							y_parent + box.Y - box.ScrollY.GetOffset(),
							clip, scale, current_background_color, true))
						return false;
			}
			for (c = box.float_children; c!=null; c = c.next_float)
				if (!RedrawBox(c,
						x_parent + box.X - box.ScrollX.GetOffset(),
						y_parent + box.Y - box.ScrollY.GetOffset(),
						clip, scale, current_background_color,
						true))
					return false;

            return true;
        }

		static PlotStyle PlotStyleContentEdge = new PlotStyle()
		{
			StrokeType = PlotOperationType.PLOT_OP_TYPE_SOLID,
			StrokeColour = 0x00ff0000,
			StrokeWidth = 1 << 10
		};

        static PlotStyle PlotStylePaddingEdge = new PlotStyle()
        {
            StrokeType = PlotOperationType.PLOT_OP_TYPE_SOLID,
            StrokeColour = 0x000000ff,
            StrokeWidth = 1 << 10
        };

        static PlotStyle PlotStyleMarginEdge = new PlotStyle()
        {
            StrokeType = PlotOperationType.PLOT_OP_TYPE_SOLID,
            StrokeColour = 0x0000ffff,
            StrokeWidth = 1 << 10
        };

        // html_redraw_box()
        // content/handlers/html/redraw.c:1236
        bool RedrawBox(Box box, int x_parent, int y_parent, Rect clip, double scale,
				uint current_background_color, bool redrawDebug)
		{
			//const struct plotter_table *plot = ctx->plot;
			int x, y;
			int width, height;
			int padding_left, padding_top, padding_width, padding_height;
			int border_left, border_top, border_right, border_bottom;
			var r = new Rect();
            var rect = new Rect();
			int x_scrolled, y_scrolled;
			Box bg_box;
			CssComputedClipRect css_rect = new CssComputedClipRect();
			CssOverflowEnum overflow_x = CssOverflowEnum.CSS_OVERFLOW_VISIBLE;
            CssOverflowEnum overflow_y = CssOverflowEnum.CSS_OVERFLOW_VISIBLE;
			//dom_exception exc;
			XmlNodeType tag_type;

			//if (html_redraw_printing && (box->flags & PRINTED))
			//return true;

			Log.Print(LogChannel.Layout, $"Redraw {box.GetHashCode()}, type {box.Type}, text: {(string.IsNullOrEmpty(box.Text) ? "" : box.Text)}");

			//if (box.Type == BoxType.BOX_TEXT && box.Text == "some text")
				//Debug.Assert(false);

			if (box.Style != null)
			{
				overflow_x = box.Style.ComputedOverflowX();
				overflow_y = box.Style.ComputedOverflowY();
            }

			// avoid trivial FP maths 
			if (scale == 1.0) {
				x = x_parent + box.X;
				y = y_parent + box.Y;
				width = box.Width;
				height = box.Height;
				padding_left = box.Padding[(int)BoxSide.LEFT];
				padding_top = box.Padding[(int)BoxSide.TOP];
				padding_width = padding_left + box.Width + box.Padding[(int)BoxSide.RIGHT];
				padding_height = padding_top + box.Height +
						box.Padding[(int)BoxSide.BOTTOM];
				border_left = box.Border[(int)BoxSide.LEFT].Width;
				border_top = box.Border[(int)BoxSide.TOP].Width;
				border_right = box.Border[(int)BoxSide.RIGHT].Width;
				border_bottom = box.Border[(int)BoxSide.BOTTOM].Width;
			} else {
				x = (int)((x_parent + box.X) * scale);
				y = (int)((y_parent + box.Y) * scale);
				width = (int)(box.Width * scale);
				height = (int)(box.Height * scale);
				// left and top padding values are normally zero,
				// so avoid trivial FP maths 
				padding_left = box.Padding[(int)BoxSide.LEFT] != 0 ? (int)(box.Padding[(int)BoxSide.LEFT] * scale) : 0;
				padding_top = box.Padding[(int)BoxSide.TOP] != 0 ? (int)(box.Padding[(int)BoxSide.TOP] * scale)	: 0;
				padding_width = (box.Padding[(int)BoxSide.LEFT] + box.Width + (int)(box.Padding[(int)BoxSide.RIGHT] * scale));
				padding_height = (box.Padding[(int)BoxSide.TOP] + box.Height + (int)(box.Padding[(int)BoxSide.BOTTOM] * scale));
				border_left = (int)(box.Border[(int)BoxSide.LEFT].Width * scale);
				border_top = (int)(box.Border[(int)BoxSide.TOP].Width * scale);
				border_right = (int)(box.Border[(int)BoxSide.RIGHT].Width * scale);
				border_bottom = (int)(box.Border[(int)BoxSide.BOTTOM].Width * scale);
			}

			// calculate rectangle covering this box and descendants 
			if (box.Style != null &&
				overflow_x != CssOverflowEnum.CSS_OVERFLOW_VISIBLE &&
				box.Parent != null)
			{
				// box contents clipped to box size 
				r.x0 = x - border_left;
				r.x1 = x + padding_width + border_right;
			} else
			{
				// box contents can hang out of the box; use descendant box 
				if (scale == 1.0) {
					r.x0 = x + box.DescendantX0;
					r.x1 = x + box.DescendantX1 + 1;
				} else {
					r.x0 = x + (int)(box.DescendantX0 * scale);
					r.x1 = x + (int)(box.DescendantX1 * scale) + 1;
				}
				if (box.Parent == null)
				{
					// root element 
					int margin_left, margin_right;
					if (scale == 1.0) {
						margin_left = box.Margin[(int)BoxSide.LEFT];
						margin_right = box.Margin[(int)BoxSide.RIGHT];
					} else {
						margin_left = (int)(box.Margin[(int)BoxSide.LEFT] * scale);
						margin_right = (int)(box.Margin[(int)BoxSide.RIGHT] * scale);
					}
					r.x0 = x - border_left - margin_left < r.x0 ?
							x - border_left - margin_left : r.x0;
					r.x1 = x + padding_width + border_right +
							margin_right > r.x1 ?
							x + padding_width + border_right +
							margin_right : r.x1;
				}
			}

			// calculate rectangle covering this box and descendants 
			if (box.Style != null && overflow_y != CssOverflowEnum.CSS_OVERFLOW_VISIBLE &&	box.Parent != null)
			{
				// box contents clipped to box size 
				r.y0 = y - border_top;
				r.y1 = y + padding_height + border_bottom;
			} else
			{
				// box contents can hang out of the box; use descendant box 
				if (scale == 1.0) {
					r.y0 = y + box.DescendantY0;
					r.y1 = y + box.DescendantY1 + 1;
				} else {
					r.y0 = y + (int)(box.DescendantY0 * scale);
					r.y1 = y + (int)(box.DescendantY1 * scale) + 1;
				}
				if (box.Parent == null)
				{
					// root element 
					int margin_top, margin_bottom;
					if (scale == 1.0) {
						margin_top = box.Margin[(int)BoxSide.TOP];
						margin_bottom = box.Margin[(int)BoxSide.BOTTOM];
					} else {
						margin_top = (int)(box.Margin[(int)BoxSide.TOP] * scale);
						margin_bottom = (int)(box.Margin[(int)BoxSide.BOTTOM] * scale);
					}
					r.y0 = y - border_top - margin_top < r.y0 ?
							y - border_top - margin_top : r.y0;
					r.y1 = y + padding_height + border_bottom +
							margin_bottom > r.y1 ?
							y + padding_height + border_bottom +
							margin_bottom : r.y1;
				}
			}

			// return if the rectangle is completely outside the clip rectangle 
			if (clip.y1 < r.y0 || r.y1 < clip.y0 ||
					clip.x1 < r.x0 || r.x1 < clip.x0)
			{
				return true;
			}

			//if the rectangle is under the page bottom but it can fit in a page,
			// don't print it now
			/*
			if (html_redraw_printing)
			{
				if (r.y1 > html_redraw_printing_border) {
					if (r.y1 - r.y0 <= html_redraw_printing_border &&
							(box.Type == BoxType.BOX_TEXT ||
							box.Type == BoxType.BOX_TABLE_CELL
							|| box->object || box->gadget)) {
						//remember the highest of all points from the
						not printed elements
						if (r.y0 < html_redraw_printing_top_cropped)
							html_redraw_printing_top_cropped = r.y0;
						return true;
					}
				}
				else box.Flags |= PRINTED; //it won't be printed anymore
			}*/

			// if visibility is hidden render children only 
			if (box.Style != null && box.Style.ComputedVisibility() == CssVisibilityEnum.CSS_VISIBILITY_HIDDEN)
			{
				/*if ((ctx->plot->group_start) &&
					(ctx->plot->group_start(ctx, "hidden box") != NSERROR_OK))
					return false;*/
				if (!RedrawBoxChildren(box, x_parent, y_parent,	r, scale, current_background_color))
					return false;
				//return ((!ctx->plot->group_end) || (ctx->plot->group_end(ctx) == NSERROR_OK));
				return true;
			}

			/*if ((ctx->plot->group_start) &&
				(ctx->plot->group_start(ctx,"vis box") != NSERROR_OK)) {
				return false;
			}*/

			if (box.Style != null &&
                    box.Style.ComputedPosition() ==	CssPosition.CSS_POSITION_ABSOLUTE &&
                    box.Style.ComputedClip(ref css_rect) == CssClipEnum.CSS_CLIP_RECT)
			{
				// We have an absolutly positioned box with a clip rect 
				if (css_rect.LeftAuto == false)
					r.x0 = x - border_left + UnitLenCtx.Len2DevicePx(box.Style, css_rect.Left, css_rect.Lunit).ToInt();

				if (css_rect.TopAuto == false)
					r.y0 = y - border_top + UnitLenCtx.Len2DevicePx(box.Style, css_rect.Top, css_rect.Tunit).ToInt();

				if (css_rect.RightAuto == false)
					r.x1 = x - border_left + UnitLenCtx.Len2DevicePx(box.Style, css_rect.Right, css_rect.Runit).ToInt();

				if (css_rect.BottomAuto == false)
					r.y1 = y - border_top + UnitLenCtx.Len2DevicePx(box.Style, css_rect.Bottom, css_rect.Bunit).ToInt();

				// find intersection of clip rectangle and box 
				if (r.x0 < clip.x0) r.x0 = clip.x0;
				if (r.y0 < clip.y0) r.y0 = clip.y0;
				if (clip.x1 < r.x1) r.x1 = clip.x1;
				if (clip.y1 < r.y1) r.y1 = clip.y1;
				// Nothing to do for invalid rectangles 
				if (r.x0 >= r.x1 || r.y0 >= r.y1)
				{
					// not an error 
					//return ((!ctx->plot->group_end) ||
					//(ctx->plot->group_end(ctx) == NSERROR_OK));
					return true;
				}
				// clip to it 
				Plot.Clip(r);

			} else if (box.Type == BoxType.BOX_BLOCK || box.Type == BoxType.BOX_INLINE_BLOCK ||
					box.Type == BoxType.BOX_TABLE_CELL /*|| box->object*/)
			{
				// find intersection of clip rectangle and box 
				if (r.x0 < clip.x0) r.x0 = clip.x0;
				if (r.y0 < clip.y0) r.y0 = clip.y0;
				if (clip.x1 < r.x1) r.x1 = clip.x1;
				if (clip.y1 < r.y1) r.y1 = clip.y1;
				// no point trying to draw 0-width/height boxes 
				if (r.x0 == r.x1 || r.y0 == r.y1)
				{
					// not an error 
					//return ((!ctx->plot->group_end) ||
					//(ctx->plot->group_end(ctx) == NSERROR_OK));
					return true;
				}
				// clip to it 
				Plot.Clip(r);
			} else {
				// clip box is fine, clip to it 
				r = clip;
				Plot.Clip(r);
			}

			// background colour and image for block level content and replaced
			// inlines 

			bg_box = RedrawFindBgBox(box);

			// bg_box == null implies that this box should not have
			// its background rendered. Otherwise filter out linebreaks,
			// optimize away non-differing inlines, only plot background
			// for BoxType.BOX_TEXT it's in an inline 
			if (bg_box != null && bg_box.Type != BoxType.BOX_BR &&
					bg_box.Type != BoxType.BOX_TEXT &&
					bg_box.Type != BoxType.BOX_INLINE_END &&
					(bg_box.Type != BoxType.BOX_INLINE /*|| bg_box->object ||
					bg_box.Flags & IFRAME */ || (box.Flags & BoxFlags.REPLACE_DIM) != 0 /*||
					(bg_box->gadget != null &&
					(bg_box->gadget->type == GADGET_TEXTAREA ||
					bg_box->gadget->type == GADGET_TEXTBOX ||
					bg_box->gadget->type == GADGET_PASSWORD))*/))
			{
				// find intersection of clip box and border edge 
				Rect p;
				p.x0 = x - border_left < r.x0 ? r.x0 : x - border_left;
				p.y0 = y - border_top < r.y0 ? r.y0 : y - border_top;
				p.x1 = x + padding_width + border_right < r.x1 ?
						x + padding_width + border_right : r.x1;
				p.y1 = y + padding_height + border_bottom < r.y1 ?
						y + padding_height + border_bottom : r.y1;
				if (box.Parent == null)
				{
					// Root element, special case:
					// background covers margins too 
					int m_left, m_top, m_right, m_bottom;
					if (scale == 1.0) {
						m_left = box.Margin[(int)BoxSide.LEFT];
						m_top = box.Margin[(int)BoxSide.TOP];
						m_right = box.Margin[(int)BoxSide.RIGHT];
						m_bottom = box.Margin[(int)BoxSide.BOTTOM];
					} else {
						m_left = (int)(box.Margin[(int)BoxSide.LEFT] * scale);
						m_top = (int)(box.Margin[(int)BoxSide.TOP] * scale);
						m_right = (int)(box.Margin[(int)BoxSide.RIGHT] * scale);
						m_bottom = (int)(box.Margin[(int)BoxSide.BOTTOM] * scale);
					}
					p.x0 = p.x0 - m_left < r.x0 ? r.x0 : p.x0 - m_left;
					p.y0 = p.y0 - m_top < r.y0 ? r.y0 : p.y0 - m_top;
					p.x1 = p.x1 + m_right < r.x1 ? p.x1 + m_right : r.x1;
					p.y1 = p.y1 + m_bottom < r.y1 ? p.y1 + m_bottom : r.y1;
				}
				// valid clipping rectangles only 
				if ((p.x0 < p.x1) && (p.y0 < p.y1)) {
					// plot background 
					if (!RedrawBackground(x, y, box, scale, p, current_background_color, bg_box, UnitLenCtx))
						return false;
					// restore previous graphics window 
					Plot.Clip(r);
				}
			}

			// borders for block level content and replaced inlines 
			if (box.Style != null &&
				box.Type != BoxType.BOX_TEXT &&
				box.Type != BoxType.BOX_INLINE_END &&
				(box.Type != BoxType.BOX_INLINE /*|| box->object ||
				 box.Flags & IFRAME*/ || (box.Flags & BoxFlags.REPLACE_DIM) != 0 /*||
				 (box->gadget != null &&
				  (box->gadget->type == GADGET_TEXTAREA ||
				   box->gadget->type == GADGET_TEXTBOX ||
				   box->gadget->type == GADGET_PASSWORD))*/) &&
				(border_top != 0 || border_right != 0 || border_bottom != 0 || border_left != 0))
			{
				if (!RedrawBorders(box, x_parent, y_parent,
						padding_width, padding_height, r, scale))
					return false;
			}

			// backgrounds and borders for non-replaced inlines 
			if (box.Style != null && box.Type == BoxType.BOX_INLINE && box.InlineEnd != null &&
					(RedrawBoxHasBackground(box) ||
                    (border_top != 0 || border_right != 0 || border_bottom != 0 || border_left != 0)))
			{
				// inline backgrounds and borders span other boxes and may
				// wrap onto separate lines 
				Box ib;
				Rect b; // border edge rectangle 
				Rect p; // clipped rect 
				bool first = true;
				int ib_x;
				int ib_y = y;
				int ib_p_width;
				int ib_b_left, ib_b_right;

				b.x0 = x - border_left;
				b.x1 = x + padding_width + border_right;
				b.y0 = y - border_top;
				b.y1 = y + padding_height + border_bottom;

				p.x0 = b.x0 < r.x0 ? r.x0 : b.x0;
				p.x1 = b.x1 < r.x1 ? b.x1 : r.x1;
				p.y0 = b.y0 < r.y0 ? r.y0 : b.y0;
				p.y1 = b.y1 < r.y1 ? b.y1 : r.y1;
				for (ib = box; ib != null; ib = ib.Next)
				{
					// to get extents of rectangle(s) associated with
					// inline, cycle though all boxes in inline, skipping
					// over floats 
					if (ib.Type == BoxType.BOX_FLOAT_LEFT ||
							ib.Type == BoxType.BOX_FLOAT_RIGHT)
						continue;
					if (scale == 1.0) {
						ib_x = x_parent + ib.X;
						ib_y = y_parent + ib.Y;
						ib_p_width = ib.Padding[(int)BoxSide.LEFT] + ib.Width + ib.Padding[(int)BoxSide.RIGHT];
						ib_b_left = ib.Border[(int)BoxSide.LEFT].Width;
						ib_b_right = ib.Border[(int)BoxSide.RIGHT].Width;
					} else {
						ib_x = (int)((x_parent + ib.X) * scale);
						ib_y = (int)((y_parent + ib.Y) * scale);
						ib_p_width = (int)((ib.Padding[(int)BoxSide.LEFT] + ib.Width + ib.Padding[(int)BoxSide.RIGHT]) * scale);
						ib_b_left = (int)(ib.Border[(int)BoxSide.LEFT].Width * scale);
						ib_b_right = (int)(ib.Border[(int)BoxSide.RIGHT].Width * scale);
					}

					if (((ib.Flags & BoxFlags.NEW_LINE) != 0) && ib != box)
					{
						// inline element has wrapped, plot background
						// and borders 
						if (!RedrawInlineBackground(
								x, y, box, scale, p, b,
								first, false,
								current_background_color,
								UnitLenCtx))
							return false;
						// restore previous graphics window 
						Plot.Clip(r);
						if (!RedrawInlineBorders(box, b, r,
								scale, first, false))
							return false;
						// reset coords 
						b.x0 = ib_x - ib_b_left;
						b.y0 = ib_y - border_top - padding_top;
						b.y1 = ib_y + padding_height - padding_top +
								border_bottom;

						p.x0 = b.x0 < r.x0 ? r.x0 : b.x0;
						p.y0 = b.y0 < r.y0 ? r.y0 : b.y0;
						p.y1 = b.y1 < r.y1 ? b.y1 : r.y1;

						first = false;
					}

					// increase width for current box 
					b.x1 = ib_x + ib_p_width + ib_b_right;
					p.x1 = b.x1 < r.x1 ? b.x1 : r.x1;

					if (ib == box.InlineEnd)
						// reached end of BoxType.BOX_INLINE span 
						break;
				}
				// plot background and borders for last rectangle of
				// the inline 
				if (!RedrawInlineBackground(x, ib_y, box, scale, p, b,
						first, true, current_background_color,
						UnitLenCtx))
					return false;
				// restore previous graphics window 
				Plot.Clip(r);

				if (!RedrawInlineBorders(box, b, r, scale, first, true))
					return false;

			}

			// Debug outlines 
			if (redrawDebug) {
				int margin_left, margin_right;
				int margin_top, margin_bottom;
				if (scale == 1.0) {
					// avoid trivial fp maths 
					margin_left = box.Margin[(int)BoxSide.LEFT];
					margin_top = box.Margin[(int)BoxSide.TOP];
					margin_right = box.Margin[(int)BoxSide.RIGHT];
					margin_bottom = box.Margin[(int)BoxSide.BOTTOM];
				} else {
					margin_left = (int)(box.Margin[(int)BoxSide.LEFT] * scale);
					margin_top = (int)(box.Margin[(int)BoxSide.TOP] * scale);
					margin_right = (int)(box.Margin[(int)BoxSide.RIGHT] * scale);
					margin_bottom = (int)(box.Margin[(int)BoxSide.BOTTOM] * scale);
				}
				// Content edge -- blue 
				rect.x0 = x + padding_left;
				rect.y0 = y + padding_top;
				rect.x1 = x + padding_left + width;
				rect.y1 = y + padding_top + height;
				Plot.Rectangle(PlotStyleContentEdge, rect);

				// Padding edge -- red 
				rect.x0 = x;
				rect.y0 = y;
				rect.x1 = x + padding_width;
				rect.y1 = y + padding_height;
				Plot.Rectangle(PlotStylePaddingEdge, rect);

				// Margin edge -- yellow 
				rect.x0 = x - border_left - margin_left;
				rect.y0 = y - border_top - margin_top;
				rect.x1 = x + padding_width + border_right + margin_right;
				rect.y1 = y + padding_height + border_bottom + margin_bottom;
				Plot.Rectangle(PlotStyleMarginEdge, rect);
			}

			// clip to the padding edge for objects, or boxes with overflow hidden
			// or scroll, unless it's the root element 
			if (box.Parent != null) {
				bool need_clip = false;
				if (/*box->object || box.Flags & IFRAME ||*/
						(overflow_x != CssOverflowEnum.CSS_OVERFLOW_VISIBLE &&
						 overflow_y != CssOverflowEnum.CSS_OVERFLOW_VISIBLE)) {
					r.x0 = x;
					r.y0 = y;
					r.x1 = x + padding_width;
					r.y1 = y + padding_height;
					if (r.x0 < clip.x0) r.x0 = clip.x0;
					if (r.y0 < clip.y0) r.y0 = clip.y0;
					if (clip.x1 < r.x1) r.x1 = clip.x1;
					if (clip.y1 < r.y1) r.y1 = clip.y1;
					if (r.x1 <= r.x0 || r.y1 <= r.y0) {
						//return (!ctx->plot->group_end ||
						//	(ctx->plot->group_end(ctx) == NSERROR_OK));
						return true;
					}
					need_clip = true;

				} else if (overflow_x != CssOverflowEnum.CSS_OVERFLOW_VISIBLE) {
					r.x0 = x;
					r.y0 = clip.y0;
					r.x1 = x + padding_width;
					r.y1 = clip.y1;
					if (r.x0 < clip.x0) r.x0 = clip.x0;
					if (clip.x1 < r.x1) r.x1 = clip.x1;
					if (r.x1 <= r.x0) {
						//return (!ctx.plot.group_end ||
						//							(ctx.plot.group_end(ctx) == NSERROR_OK));
						return true;
					}
					need_clip = true;

				} else if (overflow_y != CssOverflowEnum.CSS_OVERFLOW_VISIBLE) {
					r.x0 = clip.x0;
					r.y0 = y;
					r.x1 = clip.x1;
					r.y1 = y + padding_height;
					if (r.y0 < clip.y0) r.y0 = clip.y0;
					if (clip.y1 < r.y1) r.y1 = clip.y1;
					if (r.y1 <= r.y0) {
						//return (!ctx.plot.group_end ||
						//(ctx.plot.group_end(ctx) == NSERROR_OK));
						return true;
					}
					need_clip = true;
				}

				if (need_clip &&
					(box.Type == BoxType.BOX_BLOCK ||
					 box.Type == BoxType.BOX_INLINE_BLOCK ||
					 box.Type == BoxType.BOX_TABLE_CELL /*|| box->object*/))
				{
					Plot.Clip(r);
				}
			}

			// text decoration 
			if ((box.Type != BoxType.BOX_TEXT) &&
				box.Style != null &&
				box.Style.ComputedTextDecoration() != CssTextDecorationEnum.CSS_TEXT_DECORATION_NONE)
			{
				/*
				if (!html_redraw_text_decoration(box, x_parent, y_parent,
						scale, current_background_color, ctx))
					return false;
				*/
				Log.Unimplemented();
			}

			if (box.Node != null)
			{
				tag_type = box.Node.NodeType;
			} else {
				tag_type = XmlNodeType.None;
			}
			/*
			if (box->object && width != 0 && height != 0) {
				struct content_redraw_data obj_data;

				x_scrolled = x - scrollbar_get_offset(box->scroll_x) * scale;
				y_scrolled = y - scrollbar_get_offset(box->scroll_y) * scale;

				obj_data.x = x_scrolled + padding_left;
				obj_data.y = y_scrolled + padding_top;
				obj_data.width = width;
				obj_data.height = height;
				obj_data.background_colour = current_background_color;
				obj_data.scale = scale;
				obj_data.repeat_x = false;
				obj_data.repeat_y = false;

				if (content_get_type(box->object) == CONTENT_HTML) {
					obj_data.x /= scale;
					obj_data.y /= scale;
				}

				if (!content_redraw(box->object, &obj_data, &r, ctx)) {
					// Show image fail 
					// Unicode (U+FFFC) 'OBJECT REPLACEMENT CHARACTER' 
					const char *obj = "\xef\xbf\xbc";
					int obj_width;
					int obj_x = x + padding_left;
					nserror res;

					rect.x0 = x + padding_left;
					rect.y0 = y + padding_top;
					rect.x1 = x + padding_left + width - 1;
					rect.y1 = y + padding_top + height - 1;
					res = ctx->plot->rectangle(ctx, plot_style_broken_object, &rect);
					if (res != NSERROR_OK) {
						return false;
					}

					res = guit->layout->width(plot_fstyle_broken_object,
								  obj,
								  sizeof(obj) - 1,
								  &obj_width);
					if (res != NSERROR_OK) {
						obj_x += 1;
					} else {
						obj_x += width / 2 - obj_width / 2;
					}

					if (ctx->plot->text(ctx,
								plot_fstyle_broken_object,
								obj_x, y + padding_top + (int)(height * 0.75),
								obj, sizeof(obj) - 1) != NSERROR_OK)
						return false;
				}
			} else if (tag_type == DOM_HTML_ELEMENT_TYPE_CANVAS &&
				   box.Node != null &&
				   box.Flags & BoxFlags.REPLACE_DIM)
			{
				// Canvas to draw 
				/*
				struct bitmap *bitmap = null;
				exc = dom_node_get_user_data(box->node,
								 corestring_dom___ns_key_canvas_node_data,
								 &bitmap);
				if (exc != DOM_NO_ERR) {
					bitmap = null;
				}
				if (bitmap != null &&
					ctx->plot->bitmap(ctx, bitmap, x + padding_left, y + padding_top,
							  width, height, current_background_color,
							  BITMAPF_NONE) != NSERROR_OK)
					return false;
				Log.Unimplemented();
			} else if (box->iframe) {
				// Offset is passed to browser window redraw unscaled 
				browser_window_redraw(box->iframe,
						x + padding_left,
						y + padding_top, &r, ctx);

			} else if (box->gadget && box->gadget->type == GADGET_CHECKBOX) {
				if (!html_redraw_checkbox(x + padding_left, y + padding_top,
						width, height, box->gadget->selected, ctx))
					return false;

			} else if (box->gadget && box->gadget->type == GADGET_RADIO) {
				if (!html_redraw_radio(x + padding_left, y + padding_top,
						width, height, box->gadget->selected, ctx))
					return false;

			} else if (box->gadget && box->gadget->type == GADGET_FILE) {
				if (!html_redraw_file(x + padding_left, y + padding_top,
						width, height, box, scale,
						current_background_color, &html->unit_len_ctx, ctx))
					return false;

			} else if (box->gadget &&
					(box->gadget->type == GADGET_TEXTAREA ||
					box->gadget->type == GADGET_PASSWORD ||
					box->gadget->type == GADGET_TEXTBOX)) {
				textarea_redraw(box->gadget->data.text.ta, x, y,
						current_background_color, scale, &r, ctx);

			} else*/
			if (!string.IsNullOrEmpty(box.Text))
			{
				if (!RedrawTextBox(box, x, y, r, scale, current_background_color))
					return false;
			} else {
                if (!RedrawBoxChildren(box, x_parent, y_parent, r, scale, current_background_color))
					return false;
			}

			if (box.Type == BoxType.BOX_BLOCK || box.Type == BoxType.BOX_INLINE_BLOCK ||
					box.Type == BoxType.BOX_TABLE_CELL || box.Type == BoxType.BOX_INLINE)
			{
				Plot.Clip(clip);
			}

			// list marker
			/*
			if (box->list_marker) {
				if (!html_redraw_box(html, box->list_marker,
						x_parent + box.X -
						scrollbar_get_offset(box->scroll_x),
						y_parent + box.Y -
						scrollbar_get_offset(box->scroll_y),
						clip, scale, current_background_color, ctx))
					return false;
			}*/

			// scrollbars
			/*
			if (((box.Style && box.Type != BoxType.BOX_BR &&
				  box.Type != BoxType.BOX_TABLE && box.Type != BoxType.BOX_INLINE &&
				  (box->gadget == null || box->gadget->type != GADGET_TEXTAREA) &&
				  (overflow_x == CssOverflowEnum.CSS_OVERFLOW_SCROLL ||
				   overflow_x == CssOverflowEnum.CSS_OVERFLOW_AUTO ||
				   overflow_y == CssOverflowEnum.CSS_OVERFLOW_SCROLL ||
				   overflow_y == CssOverflowEnum.CSS_OVERFLOW_AUTO)) ||
				 (box->object && content_get_type(box->object) ==
				  CONTENT_HTML)) && box.Parent != null)
				{
				nserror res;
				bool has_x_scroll = (overflow_x == CssOverflowEnum.CSS_OVERFLOW_SCROLL);
				bool has_y_scroll = (overflow_y == CssOverflowEnum.CSS_OVERFLOW_SCROLL);

				has_x_scroll |= (overflow_x == CssOverflowEnum.CSS_OVERFLOW_AUTO) &&
						box_hscrollbar_present(box);
				has_y_scroll |= (overflow_y == CssOverflowEnum.CSS_OVERFLOW_AUTO) &&
						box_vscrollbar_present(box);

				res = box_handle_scrollbars((struct content *)html,
								box, has_x_scroll, has_y_scroll);
				if (res != NSERROR_OK) {
					NSLOG(netsurf, INFO, "%s", messages_get_errorcode(res));
					return false;
				}

				if (box->scroll_x != null)
					scrollbar_redraw(box->scroll_x,
							x_parent + box.X,
							y_parent + box.Y + box.Padding[(int)BoxSide.TOP] +
							box.Height + box.Padding[(int)BoxSide.BOTTOM] -
							SCROLLBAR_WIDTH, clip, scale, ctx);
				if (box->scroll_y != null)
					scrollbar_redraw(box->scroll_y,
							x_parent + box.X + box.Padding[(int)BoxSide.LEFT] +
							box.Width + box.Padding[(int)BoxSide.RIGHT] -
							SCROLLBAR_WIDTH,
							y_parent + box.Y, clip, scale, ctx);
			}*/

			if (box.Type == BoxType.BOX_BLOCK || box.Type == BoxType.BOX_INLINE_BLOCK ||
				box.Type == BoxType.BOX_TABLE_CELL || box.Type == BoxType.BOX_INLINE)
			{
				Plot.Clip(clip);
			}

			//return ((!plot->group_end) || (ctx->plot->group_end(ctx) == NSERROR_OK));
			return true;
		}

        // html_redraw()
        // content/handlers/html/redraw.c:1944
        /**
         * Draw a CONTENT_HTML using the current set of plotters (plot).
         *
         * \param  c	 content of type CONTENT_HTML
         * \param  data	 redraw data for this content redraw
         * \param  clip	 current clip region
         * \param  ctx	 current redraw context
         * \return true if successful, false otherwise
         *
         * x, y, clip_[xy][01] are in target coordinates.
         */
        public void Redraw(RedrawData data, Rect clip, bool interactive, bool backgroundImages)
        {
	        //html_content *html = (html_content *) c;
	        //struct box *box;
	        //bool result = true;
	        bool select, select_only;
	        PlotStyle pstyle_fill_bg = new PlotStyle() {
		        FillType = PlotOperationType.PLOT_OP_TYPE_SOLID,
		        FillColour = data.BackgroundColor,
	        };

	        var box = Layout.RootBox;
	        Debug.Assert(box != null);

	        /* The select menu needs special treating because, when opened, it
	         * reaches beyond its layout box.
	         */
	        select = false;
	        select_only = false;
	        if (interactive && false /*&& VisibleSelectMenu != null*/)
            {
                /*
		        struct form_control *control = html->visible_select_menu;
		        select = true;
		        // check if the redraw rectangle is completely inside of the select menu
		        select_only = form_clip_inside_select_menu(control,
				        data->scale, clip);
                */
	        }

	        if (!select_only)
            {
                // clear to background colour
                Plot.Clip(clip);

		        if (BackgroundColor != Colormap.Transparent)
			        pstyle_fill_bg.FillColour = BackgroundColor;

                Plot.Rectangle(pstyle_fill_bg, clip);

		        RedrawBox(box, data.X, data.Y, clip,
				        data.Scale, pstyle_fill_bg.FillColour, true);
	        }

	        if (select) {
                /*
		        int menu_x, menu_y;
		        box = html->visible_select_menu->box;
		        box_coords(box, &menu_x, &menu_y);

		        menu_x -= box->border[LEFT].width;
		        menu_y += box->height + box->border[BOTTOM].width +
				        box->padding[BOTTOM] + box->padding[TOP];
		        result &= form_redraw_select_menu(html->visible_select_menu,
				        data->x + menu_x, data->y + menu_y,
				        data->scale, clip, ctx);
                */
                Log.Unimplemented();
	        }
        }

		// layout_find_dimensions()
		// layout.c:1262
		/**
		 * Calculate width, height, and thickness of margins, paddings, and borders.
		 *
		 * \param  unit_len_ctx          Length conversion context
		 * \param  available_width  width of containing block
		 * \param  viewport_height  height of viewport in pixels or -ve if unknown
		 * \param  box		    current box
		 * \param  style	    style giving width, height, margins, paddings,
		 *                          and borders
		 * \param  width            updated to width, may be NULL
		 * \param  height           updated to height, may be NULL
		 * \param  max_width        updated to max-width, may be NULL
		 * \param  min_width        updated to min-width, may be NULL
		 * \param  max_height       updated to max-height, may be NULL
		 * \param  min_height       updated to min-height, may be NULL
		 * \param  margin	    filled with margins, may be NULL
		 * \param  padding	    filled with paddings, may be NULL
		 * \param  border	    filled with border widths, may be NULL
		 */
		public static void
		LayoutFindDimensions(CssUnitCtx unit_len_ctx,
					   int available_width,
					   int viewport_height,
					   Box box,
					   ComputedStyle style,
					   out int width,
					   out int height,
					   out int max_width,
					   out int min_width,
					   out int max_height,
					   out int min_height,
					   int[] margin,
					   int[] padding,
					   BoxBorder[] border)
		{
			Box containing_block = null;
			uint i;

			//if (width != 0)
			{
				Fixed value = Fixed.F_0;
				CssUnit unit = CssUnit.CSS_UNIT_PX;

				var wtype = style.ComputedWidth(ref value, ref unit);

				if (wtype == CssWidth.CSS_WIDTH_SET)
				{
					if (unit == CssUnit.CSS_UNIT_PCT)
					{
						width = value.PercentageToInt(available_width);
					}
					else
					{
						width = unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
					}
				}
				else
				{
					width = int.MinValue;
				}

				if (width != int.MinValue)
				{
					box.LayoutHandleBoxSizing(unit_len_ctx, available_width, true, ref width);
				}
			}

			//if (height != 0)
			{
				Fixed value = Fixed.F_0;
				CssUnit unit = CssUnit.CSS_UNIT_PX;

				var htype = style.ComputedHeight(ref value, ref unit);

				if (htype == CssHeightEnum.CSS_HEIGHT_SET)
				{
					if (unit == CssUnit.CSS_UNIT_PCT)
					{
						CssHeightEnum cbhtype = CssHeightEnum.CSS_HEIGHT_INHERIT;

						if (box.Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE &&
							box.Parent != null)
						{
							// Box is absolutely positioned
							Debug.Assert(box.float_container != null);
							containing_block = box.float_container;
						}
						else if (box.float_container != null &&
							box.Style.ComputedPosition() != CssPosition.CSS_POSITION_ABSOLUTE &&
							(box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_LEFT ||
							 box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_RIGHT))
						{
							// Box is a float
							Debug.Assert(box.Parent != null &&
								box.Parent.Parent != null &&
								box.Parent.Parent.Parent != null);

							containing_block = box.Parent.Parent.Parent;
						}
						else if (box.Parent != null && box.Parent.Type != BoxType.BOX_INLINE_CONTAINER)
						{
							/* Box is a block level element */
							containing_block = box.Parent;
						}
						else if (box.Parent != null && box.Parent.Type == BoxType.BOX_INLINE_CONTAINER)
						{
							// Box is an inline block
							Debug.Assert(box.Parent.Parent != null);
							containing_block = box.Parent.Parent;
						}

						if (containing_block != null)
						{
							Fixed f = Fixed.F_0;
							CssUnit u = CssUnit.CSS_UNIT_PX;

							cbhtype = containing_block.Style.ComputedHeight(ref f, ref u);
						}

						if (containing_block != null &&
							containing_block.Height != int.MinValue &&
							(box.Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
							cbhtype == CssHeightEnum.CSS_HEIGHT_SET))
						{
							/* Box is absolutely positioned or its
							 * containing block has a valid
							 * specified height.
							 * (CSS 2.1 Section 10.5) */
							height = value.PercentageToInt(containing_block.Height);
						}
						else if ((box.Parent == null || box.Parent.Parent == null) && viewport_height >= 0)
						{
							/* If root element or it's child
							 * (HTML or BODY) */
							height = value.PercentageToInt(viewport_height);
						}
						else
						{
							/* precentage height not permissible
							 * treat height as auto */
							height = int.MinValue;
						}
					}
					else
					{
						height = unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
					}
				}
				else
				{
					height = int.MinValue;
				}

				if (height != int.MinValue)
				{
					box.LayoutHandleBoxSizing(unit_len_ctx, available_width, false, ref height);
				}
			}

			//if (max_width != 0)
			{
				Fixed value = Fixed.F_0;
				CssUnit unit = CssUnit.CSS_UNIT_PX;

				var type = style.ComputedMaxWidth(ref value, ref unit);

				if (type == CssMaxWidthEnum.CSS_MAX_WIDTH_SET)
				{
					if (unit == CssUnit.CSS_UNIT_PCT)
					{
						max_width = value.PercentageToInt(available_width);
					}
					else
					{
						max_width = unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
					}
				}
				else
				{
					// Inadmissible
					max_width = -1;
				}

				if (max_width != -1)
				{
					box.LayoutHandleBoxSizing(unit_len_ctx, available_width, true, ref max_width);
				}
			}

			//if (min_width != 0)
			{
				Fixed value = Fixed.F_0;
				CssUnit unit = CssUnit.CSS_UNIT_PX;

				CssMinWidthEnum type = BoxTree.ns_computed_min_width(style, ref value, ref unit);

				if (type == CssMinWidthEnum.CSS_MIN_WIDTH_SET)
				{
					if (unit == CssUnit.CSS_UNIT_PCT)
					{
						min_width = value.PercentageToInt(available_width);
					}
					else
					{
						min_width = unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
					}
				}
				else
				{
					/* Inadmissible */
					min_width = 0;
				}

				if (min_width != 0)
				{
					box.LayoutHandleBoxSizing(unit_len_ctx, available_width, true, ref min_width);
				}
			}

			//if (max_height != 0)
			{
				Fixed value = Fixed.F_0;
				CssUnit unit = CssUnit.CSS_UNIT_PX;

				var type = style.ComputedMaxHeight(ref value, ref unit);

				if (type == CssMaxHeightEnum.CSS_MAX_HEIGHT_SET)
				{
					if (unit == CssUnit.CSS_UNIT_PCT)
					{
						/* TODO: handle percentage */
						max_height = -1;
					}
					else
					{
						max_height = unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
					}
				}
				else
				{
					/* Inadmissible */
					max_height = -1;
				}
			}

			//if (min_height != 0)
			{
				Fixed value = Fixed.F_0;
				CssUnit unit = CssUnit.CSS_UNIT_PX;

				var type = BoxTree.ns_computed_min_height(style, ref value, ref unit);

				if (type == CssMinHeightEnum.CSS_MIN_HEIGHT_SET)
				{
					if (unit == CssUnit.CSS_UNIT_PCT)
					{
						/* TODO: handle percentage */
						min_height = 0;
					}
					else
					{
						min_height = unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
					}
				}
				else
				{
					// Inadmissible
					min_height = 0;
				}
			}

			for (i = 0; i != 4; i++)
			{
				if (margin.Length != 0)
				{
					CssMarginEnum type = CssMarginEnum.CSS_MARGIN_AUTO;
					Fixed value = Fixed.F_0;
					CssUnit unit = CssUnit.CSS_UNIT_PX;

					type = style.ComputedMarginSides[i](ref value, ref unit);

					if (type == CssMarginEnum.CSS_MARGIN_SET)
					{
						if (unit == CssUnit.CSS_UNIT_PCT)
						{
							margin[i] = value.PercentageToInt(available_width);
						}
						else
						{
							margin[i] = unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
						}
					}
					else
					{
						margin[i] = int.MinValue;
					}
				}

				if (padding.Length != 0)
				{
					Fixed value = Fixed.F_0;
					CssUnit unit = CssUnit.CSS_UNIT_PX;

					style.ComputedPaddingSide[i](ref value, ref unit);

					if (unit == CssUnit.CSS_UNIT_PCT)
					{
						padding[i] = value.PercentageToInt(available_width);
					}
					else
					{
						padding[i] = unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();
					}
				}

				// Table cell borders are populated in table.c
				if (border.Length != 0 && box.Type != BoxType.BOX_TABLE_CELL)
				{
					var bstyle = CssBorderStyleEnum.CSS_BORDER_STYLE_NONE;
					Color color;
					Fixed value = Fixed.F_0;
					CssUnit unit = CssUnit.CSS_UNIT_PX;

					style.ComputedBorderSideWidth[i](ref value, ref unit);
					bstyle = style.ComputedBorderSideStyle[i]();
					style.ComputedBorderSideColor[i](out color);

					border[i].Style = bstyle;
					border[i].BorderColor = color;

					if (bstyle == CssBorderStyleEnum.CSS_BORDER_STYLE_HIDDEN ||
							bstyle == CssBorderStyleEnum.CSS_BORDER_STYLE_NONE)
						/* spec unclear: following Mozilla */
						border[i].Width = 0;
					else
						border[i].Width = unit_len_ctx.Len2DevicePx(style, value, unit).ToInt();

					/* Special case for border-collapse: make all borders
					 * on table/table-row-group/table-row zero width. */
					if (style.ComputedBorderCollapse() ==
							CssBorderCollapseEnum.CSS_BORDER_COLLAPSE_COLLAPSE &&
							(box.Type == BoxType.BOX_TABLE ||
							 box.Type == BoxType.BOX_TABLE_ROW_GROUP ||
							 box.Type == BoxType.BOX_TABLE_ROW))
						border[i].Width = 0;
				}
			}
		}

        // layout_next_margin_block()
        // layout.c:1571
        /**
         * Find next block that current margin collapses to.
         *
         * \param  unit_len_ctx  Length conversion context
         * \param  box    box to start tree-order search from (top margin is included)
         * \param  block  box responsible for current block fromatting context
         * \param  viewport_height  height of viewport in px
         * \param  max_pos_margin  updated to to maximum positive margin encountered
         * \param  max_neg_margin  updated to to maximum negative margin encountered
         * \return  next box that current margin collapses to, or NULL if none.
         */
        static Box
        LayoutNextMarginBlock(CssUnitCtx unit_len_ctx,
                     Box box,
                     Box block,
                     int viewport_height,
                     ref int max_pos_margin,
                     ref int max_neg_margin)
        {
            int w, h, xw, nw, xh, nh; // Not used

            Debug.Assert(block != null);

            while (box != null)
            {

                if (box.Type == BoxType.BOX_INLINE_CONTAINER || (box.Style != null &&
                        (box.Style.ComputedPosition() != CssPosition.CSS_POSITION_ABSOLUTE &&
                         box.Style.ComputedPosition() != CssPosition.CSS_POSITION_FIXED)))
                {
                    /* Not positioned */

                    /* Get margins */
                    if (box.Style != null)
                    {
                        LayoutFindDimensions(unit_len_ctx,
                                box.Parent.Width,
                                viewport_height, box,
                                box.Style,
                                out w, out h, out xw, out nw,
                                out xh, out nh, box.Margin,
                                box.Padding, box.Border);

                        /* Apply top margin */
                        if (max_pos_margin < box.Margin[(int)BoxSide.TOP])
                            max_pos_margin = box.Margin[(int)BoxSide.TOP];
                        else if (max_neg_margin < -box.Margin[(int)BoxSide.TOP])
                            max_neg_margin = -box.Margin[(int)BoxSide.TOP];
                    }

                    /* Check whether box is the box current margin collapses
			         * to */
                    if ((box.Flags & BoxFlags.MAKE_HEIGHT) != 0 ||
                            box.Border[(int)BoxSide.TOP].Width != 0 ||
                            box.Padding[(int)BoxSide.TOP] != 0 ||
                            (box.Style != null &&
                            box.Style.ComputedOverflowY() != CssOverflowEnum.CSS_OVERFLOW_VISIBLE) ||
                            (box.Type == BoxType.BOX_INLINE_CONTAINER &&
                            !box.IsFirstChild()))
                    {
                        /* Collapse to this box; return it */
                        return box;
                    }
                }


                /* Find next box */
                if (box.Type == BoxType.BOX_BLOCK /*&& !box->object*/ && box.Children != null &&
                        box.Style != null &&
                        box.Style.ComputedOverflowY() != CssOverflowEnum.CSS_OVERFLOW_VISIBLE)
                {
                    /* Down into children. */
                    box = box.Children;
                }
                else
                {
                    if (box.Next == null)
                    {
                        /* No more siblings:
				         * Go up to first ancestor with a sibling. */
                        do
                        {
                            /* Apply bottom margin */
                            if (max_pos_margin <
                                    box.Margin[(int)BoxSide.BOTTOM])
                                max_pos_margin =
                                    box.Margin[(int)BoxSide.BOTTOM];
                            else if (max_neg_margin <
                                    -box.Margin[(int)BoxSide.BOTTOM])
                                max_neg_margin =
                                    -box.Margin[(int)BoxSide.BOTTOM];

                            box = box.Parent;
                        } while (box != block && box.Next == null);

                        if (box == block)
                        {
                            /* Margins don't collapse with stuff
					         * outside the block formatting context
					         */
                            return block;
                        }
                    }

                    /* Apply bottom margin */
                    if (max_pos_margin < box.Margin[(int)BoxSide.BOTTOM])
                        max_pos_margin = box.Margin[(int)BoxSide.BOTTOM];
                    else if (max_neg_margin < -box.Margin[(int)BoxSide.BOTTOM])
                        max_neg_margin = -box.Margin[(int)BoxSide.BOTTOM];

                    /* To next sibling. */
                    box = box.Next;

                    /* Get margins */
                    if (box.Style != null)
                    {
                        LayoutFindDimensions(unit_len_ctx,
                                box.Parent.Width,
                                viewport_height, box,
                                box.Style,
                                out w, out h, out xw, out nw,
                                out xh, out nh, box.Margin,
                                box.Padding, box.Border);
                    }
                }
            }

            return null;
        }

        // layout.c:1682
        // layout_clear()
        /**
         * Find y coordinate which clears all floats on left and/or right.
         *
         * \param  fl	  first float in float list
         * \param  clear  type of clear
         * \return  y coordinate relative to ancestor box for floats
         */
        public static int LayoutClear(Box fl, CssClearEnum clear)
        {
	        int y = 0;
	        for (; fl!=null; fl = fl.next_float) {
		        if ((clear == CssClearEnum.CSS_CLEAR_LEFT || clear == CssClearEnum.CSS_CLEAR_BOTH) &&
				        fl.Type == BoxType.BOX_FLOAT_LEFT)
			        if (y < fl.Y + fl.Height)
				        y = fl.Y + fl.Height;
		        if ((clear == CssClearEnum.CSS_CLEAR_RIGHT || clear == CssClearEnum.CSS_CLEAR_BOTH) &&
				        fl.Type == BoxType.BOX_FLOAT_RIGHT)
			        if (y < fl.Y + fl.Height)
				        y = fl.Y + fl.Height;
	        }
	        return y;
        }

        // find_sides
        // layout.c:1716
        /**
         * Find left and right edges in a vertical range.
         *
         * \param  fl	  first float in float list
         * \param  y0	  start of y range to search
         * \param  y1	  end of y range to search
         * \param  x0	  start left edge, updated to available left edge
         * \param  x1	  start right edge, updated to available right edge
         * \param  left	  returns float on left if present
         * \param  right  returns float on right if present
         */
        public static void
        FindSides(Box fl,
	           int y0, int y1,
	           ref int x0, ref int x1,
	           out Box left,
	           out Box right)
        {
	        int fy0, fy1, fx0, fx1;

	        Log.Print(LogChannel.Layout, $"y0 {y0}, y1 {y1}, x0 {x0}, x1 {x1}");

	        left = right = null;
	        for (; fl!=null; fl = fl.next_float) {
		        fy1 = fl.Y + fl.Height;
		        if (fy1 < y0) {
			        /* Floats are sorted in order of decreasing bottom pos.
			         * Past here, all floats will be too high to concern us.
			         */
			        return;
		        }
		        fy0 = fl.Y;
		        if (y0 < fy1 && fy0 <= y1) {
			        if (fl.Type == BoxType.BOX_FLOAT_LEFT) {
				        fx1 = fl.X + fl.Width;
				        if (x0 < fx1) {
					        x0 = fx1;
					        left = fl;
				        }
			        } else {
                        fx0 = fl.X;
				        if (fx0 < x1) {
					        x1 = fx0;
					        right = fl;
				        }
			        }
		        }
	        }

	        Log.Print(LogChannel.Layout,  $"x0 {x0}, x1 {x1}, left {(left == null ? "null" : left.GetHashCode())}, right {(right == null ? "null" : right.GetHashCode())}");
        }

        // layout_block_find_dimensions()
        // content/handlers/html/layout.c:1916
        /**
		 * Compute dimensions of box, margins, paddings, and borders for a block-level
		 * element.
		 *
		 * \param  unit_len_ctx          Length conversion context
		 * \param  available_width  Max width available in pixels
		 * \param  viewport_height  Height of viewport in pixels or -ve if unknown
		 * \param  lm		    min left margin required to avoid floats in px.
		 *				zero if not applicable
		 * \param  rm		    min right margin required to avoid floats in px.
		 *				zero if not applicable
		 * \param  box		    box to find dimensions of. updated with new width,
		 *			    height, margins, borders and paddings
		 *
		 * See CSS 2.1 10.3.3, 10.3.4, 10.6.2, and 10.6.3.
		 */
        void LayoutBlockFindDimensions(CssUnitCtx unit_len_ctx,
						 int available_width,
						 int viewport_height,
						 int lm,
						 int rm,
						 Box box)
		{
			int width, max_width, min_width;
			int height, max_height, min_height;
			//int *margin = box->margin;
			//int *padding = box->padding;
			//struct box_border *border = box->border;
			ComputedStyle style = box.Style;

			LayoutFindDimensions(unit_len_ctx, available_width, viewport_height, box,
					style, out width, out height, out max_width, out min_width,
					out max_height, out min_height, box.Margin, box.Padding, box.Border);

			/*if (box->object && !(box->flags & REPLACE_DIM) &&
					content_get_type(box->object) != CONTENT_HTML) {
				// block-level replaced element, see 10.3.4 and 10.6.2
				layout_get_object_dimensions(box, &width, &height,
						min_width, max_width, min_height, max_height);
			}*/

			box.Width = box.LayoutSolveWidth(available_width, width, lm, rm, max_width, min_width);
			box.Height = height;

			if (box.Margin[(int)BoxSide.TOP] == int.MinValue) // AUTO
				box.Margin[(int)BoxSide.TOP] = 0;
			if (box.Margin[(int)BoxSide.BOTTOM] == int.MinValue) // AUTO
				box.Margin[(int)BoxSide.BOTTOM] = 0;
		}

        // layout_block_context()
        // layout.c:3963
        /**
         * Layout a block formatting context.
         *
         * \param  block	    BLOCK, INLINE_BLOCK, or TABLE_CELL to layout
         * \param  viewport_height  Height of viewport in pixels or -ve if unknown
         * \param  content	    Memory pool for any new boxes
         * \return  true on success, false on memory exhaustion
         *
         * This function carries out layout of a block and its children, as described
         * in CSS 2.1 9.4.1.
         */
        bool
        LayoutBlockContext(Box block,
             int viewport_height,
             HtmlContent content)
        {
            Box box;
            int cx, cy;  /**< current coordinates */
            int max_pos_margin = 0;
            int max_neg_margin = 0;
            int y = 0;
            int lm, rm;
            Box margin_collapse = null;
            bool in_margin = false;
            Fixed gadget_size;
            CssUnit gadget_unit; /* Checkbox / radio buttons */

            Debug.Assert(block.Type == BoxType.BOX_BLOCK ||

                    block.Type == BoxType.BOX_INLINE_BLOCK ||

                    block.Type == BoxType.BOX_TABLE_CELL);
            Debug.Assert(block.Width != int.MaxValue);
            Debug.Assert(block.Width != int.MinValue);

            block.float_children = null;
            //block.cached_place_below_level = 0;
            //block.clear_level = 0;

            /* special case if the block contains an object */
            /*if (block->object) {
				int temp_width = block.Width;
				if (!layout_block_object(block))
					return false;
				layout_get_object_dimensions(block, &temp_width,
						&block.Height, INT_MIN, INT_MAX,
						INT_MIN, INT_MAX);
				return true;
			} else*/ if ((block.Flags & BoxFlags.REPLACE_DIM) != 0)
			{
				return true;
			}

		/* special case if the block contains an radio button or checkbox */
		/*if (block->gadget && (block->gadget.Type == GADGET_RADIO ||
				block->gadget.Type == GADGET_CHECKBOX))
		{
			// form checkbox or radio button
			// if width or height is AUTO, set it to 1em
			gadget_unit = CSS_UNIT_EM;
			gadget_size = INTTOFIX(1);
			if (block.Height == AUTO)
				block.Height = FIXTOINT(CssUnit_len2device_px(
						block->style,
						content.UnitLenCtx,
						gadget_size, gadget_unit));
		}*/

		box = block.Children;
		/* set current coordinates to top-left of the block */
		cx = 0;
		y = cy = block.Padding[(int)BoxSide.TOP];
		if (box != null)
			box.Y = block.Padding[(int)BoxSide.TOP];

            /* Step through the descendants of the block in depth-first order, but
             * not into the children of boxes which aren't blocks. For example, if
             * the tree passed to this function looks like this (box.Type shown):
             *
             *  block -> BOX_BLOCK
             *             BOX_BLOCK * (1)
             *               BOX_INLINE_CONTAINER * (2)
             *                 BOX_INLINE
             *                 BOX_TEXT
             *                 ...
             *             BOX_BLOCK * (3)
             *               BOX_TABLE * (4)
             *                 BOX_TABLE_ROW
             *                   BOX_TABLE_CELL
             *                     ...
             *                   BOX_TABLE_CELL
             *                     ...
             *               BOX_BLOCK * (5)
             *                 BOX_INLINE_CONTAINER * (6)
             *                   BOX_TEXT
             *                   ...
             * then the while loop will visit each box marked with *, setting box
             * to each in the order shown. */
            while (box != null)
            {

                CssOverflowEnum overflow_x = CssOverflowEnum.CSS_OVERFLOW_VISIBLE;
                CssOverflowEnum overflow_y = CssOverflowEnum.CSS_OVERFLOW_VISIBLE;

                Debug.Assert(box.Type == BoxType.BOX_BLOCK || box.Type == BoxType.BOX_TABLE ||
                        box.Type == BoxType.BOX_INLINE_CONTAINER);

                /* Tables are laid out before being positioned, because the
                 * position depends on the width which is calculated in
                 * table layout. Blocks and inline containers are positioned
                 * before being laid out, because width is not dependent on
                 * content, and the position is required during layout for
                 * correct handling of floats.
                 */

                if (box.Style != null &&
                (box.Style.ComputedPosition() == CssPosition.CSS_POSITION_ABSOLUTE ||
                 box.Style.ComputedPosition() == CssPosition.CSS_POSITION_FIXED))
                {
                    box.X = box.Parent.Padding[(int)BoxSide.LEFT];
                    /* absolute positioned; this element will establish
                     * its own block context when it gets laid out later,
                     * so no need to look at its children now. */
                    goto advance_to_next_box;
                }

                /* If we don't know which box the current margin collapses
                 * through to, find out.  Update the pos/neg margin values. */
                if (margin_collapse == null)
                {
                    margin_collapse = LayoutNextMarginBlock(
                            content.UnitLenCtx, box, block,
                            viewport_height,
                            ref max_pos_margin, ref max_neg_margin);
                    /* We have a margin that has not yet been applied. */
                    in_margin = true;
                }

                /* Clearance. */
                y = 0;
                if (box.Style != null && box.Style.ComputedClear() != CssClearEnum.CSS_CLEAR_NONE)
                    y = LayoutClear(block.float_children, box.Style.ComputedClear());

                /* Find box's overflow properties */
                if (box.Style != null)
                {
                    overflow_x = box.Style.ComputedOverflowX();
                    overflow_y = box.Style.ComputedOverflowY();
                }

                /* Blocks establishing a block formatting context get minimum
                 * left and right margins to avoid any floats. */
                lm = rm = 0;

                if (box.Type == BoxType.BOX_BLOCK || (box.Flags & BoxFlags.IFRAME) != 0)
                {
                    if (/*!box->object && !(box.Flags & BoxFlags.IFRAME) &&*/
                            (box.Flags & BoxFlags.REPLACE_DIM) == 0 &&
                            box.Style != null &&
                            (overflow_x != CssOverflowEnum.CSS_OVERFLOW_VISIBLE ||
                             overflow_y != CssOverflowEnum.CSS_OVERFLOW_VISIBLE))
                    {
                        /* box establishes new block formatting context
                         * so available width may be diminished due to
                         * floats. */
                        int x0, x1, top;

                        Box left, right;
                        top = cy + max_pos_margin - max_neg_margin;
                        top = (top > y) ? top : y;
                        x0 = cx;
                        x1 = cx + box.Parent.Width -
                                box.Parent.Padding[(int)BoxSide.LEFT] -
                                box.Parent.Padding[(int)BoxSide.RIGHT];
                        FindSides(block.float_children, top, top, ref x0, ref x1, out left, out right);
                        /* calculate min required left & right margins
                         * needed to avoid floats */
                        lm = x0 - cx;
                        rm = cx + box.Parent.Width -
                                box.Parent.Padding[(int)BoxSide.LEFT] -
                                box.Parent.Padding[(int)BoxSide.RIGHT] -
                                x1;
                    }
                    LayoutBlockFindDimensions(content.UnitLenCtx, box.Parent.Width, viewport_height, lm, rm, box);
                    if (box.Type == BoxType.BOX_BLOCK && (box.Flags & BoxFlags.IFRAME) == 0)
                    {
                        box.LayoutBlockAddScrollbar(BoxSide.RIGHT);
                        box.LayoutBlockAddScrollbar(BoxSide.BOTTOM);
                    }
                }
                else if (box.Type == BoxType.BOX_TABLE)
                {
                    Log.Unimplemented();
                    /*
                    if (box.Style != null)
                    {

                        CssWidth wtype;
                        Fixed width = Fixed.F_0;
                        CssUnit unit = CssUnit.CSS_UNIT_PX;

                        wtype = css_computed_width(box.Style, &width,
                                &unit);

                        if (wtype == CSS_WIDTH_AUTO)
                        {
                            // max available width may be
                            // diminished due to floats.
                            int x0, x1, top;

                            Box left, right;
                            top = cy + max_pos_margin -
                                    max_neg_margin;
                            top = (top > y) ? top : y;
                            x0 = cx;
                            x1 = cx + box.Parent.Width -
                                box.Parent.Padding[(int)BoxSide.LEFT] -
                                box.Parent.Padding[(int)BoxSide.RIGHT];
                            FindSides(block->float_children,
                                top, top, ref x0, ref x1,
                                out left, out right);
                            // calculate min required left & right
                            // margins needed to avoid floats
                            lm = x0 - cx;
                            rm = cx + box.Parent.Width -
                                box.Parent.Padding[(int)BoxSide.LEFT] -
                                box.Parent.Padding[(int)BoxSide.RIGHT] -
                                x1;
                        }
                    }
                    if (!layout_table(box, box.Parent.Width - lm - rm, content))
                        return false;
                    layout_solve_width(box, box.Parent.Width, box.Width,
                            lm, rm, -1, -1);
                    */
                }

                /* Position box: horizontal. */
                box.X = box.Parent.Padding[(int)BoxSide.LEFT] + box.Margin[(int)BoxSide.LEFT] +
                        box.Border[(int)BoxSide.LEFT].Width;
                cx += box.X;

                /* Position box: vertical. */
                if (box.Border[(int)BoxSide.TOP].Width != 0)
                {
                    box.Y += box.Border[(int)BoxSide.TOP].Width;
                    cy += box.Border[(int)BoxSide.TOP].Width;
                }

                /* Vertical margin */
                if (((box.Type == BoxType.BOX_BLOCK && (box.Flags & BoxFlags.HAS_HEIGHT) != 0) ||
                     box.Type == BoxType.BOX_TABLE ||
                     (box.Type == BoxType.BOX_INLINE_CONTAINER &&
                      !box.IsFirstChild()) ||
                     margin_collapse == box) &&
                    in_margin == true)
                {
                    /* Margin goes above this box. */
                    cy += max_pos_margin - max_neg_margin;
                    box.Y += max_pos_margin - max_neg_margin;

                    /* Current margin has been applied. */
                    in_margin = false;
                    max_pos_margin = max_neg_margin = 0;
                }

                /* Handle clearance */
                if (box.Type != BoxType.BOX_INLINE_CONTAINER &&
                        (y > 0) && (cy < y))
                {
                    /* box clears something*/
                    box.Y += y - cy;
                    cy = y;
                }

                /* Unless the box has an overflow style of visible, the box
                 * establishes a new block context. */
                if (box.Type == BoxType.BOX_BLOCK && box.Style != null &&
                        (overflow_x != CssOverflowEnum.CSS_OVERFLOW_VISIBLE ||
                         overflow_y != CssOverflowEnum.CSS_OVERFLOW_VISIBLE))
                {
                    LayoutBlockContext(box, viewport_height, content);

                    cy += box.Padding[(int)BoxSide.TOP];

                    if (box.Height == int.MinValue)
                    {
                        box.Height = 0;
                        box.LayoutBlockAddScrollbar(BoxSide.BOTTOM);
                    }

                    cx -= box.X;
                    cy += box.Height + box.Padding[(int)BoxSide.BOTTOM] +
                            box.Border[(int)BoxSide.BOTTOM].Width;
                    y = box.Y + box.Padding[(int)BoxSide.TOP] + box.Height +
                            box.Padding[(int)BoxSide.BOTTOM] +
                            box.Border[(int)BoxSide.BOTTOM].Width;

                    /* Skip children, because they are done in the new
                     * block context */
                    goto advance_to_next_box;
                }

                Log.Print(LogChannel.Layout, $"box {box.GetHashCode()}, cx {cx}, cy {cy}");

                /* Layout (except tables). */
                /*if (box->object) {
                    if (!layout_block_object(box))
                        return false;

                } else*/
                if (box.Type == BoxType.BOX_INLINE_CONTAINER)
                {
                    box.Width = box.Parent.Width;
                    if (!box.LayoutInlineContainer(box.Width, block, cx, cy, content))
                        return false;

                }
                else if (box.Type == BoxType.BOX_TABLE)
                {
                    /* Move down to avoid floats if necessary. */
                    int x0, x1;

                    Box left, right;
                    y = cy;
                    while (true)
                    {

                        CssWidth wtype;
                        Fixed width = Fixed.F_0;
                        CssUnit unit = CssUnit.CSS_UNIT_PX;

                        wtype = box.Style.ComputedWidth(ref width, ref unit);

                        x0 = cx;
                        x1 = cx + box.Parent.Width;
                        FindSides(block.float_children, y,
                                y + box.Height,
                                ref x0, ref x1, out left, out right);
                        if (wtype == CssWidth.CSS_WIDTH_AUTO)
                            break;
                        if (box.Width <= x1 - x0)
                            break;
                        if (left==null && right==null)
                            break;
                        else if (left==null)
                            y = right.Y + right.Height + 1;
                        else if (right==null)
                            y = left.Y + left.Height + 1;
                        else if (left.Y + left.Height <
                                right.Y + right.Height)
                            y = left.Y + left.Height + 1;
                        else
                            y = right.Y + right.Height + 1;
                    }
                    box.X += x0 - cx;
                    cx = x0;
                    box.Y += y - cy;
                    cy = y;
                }

                /* Advance to next box. */
                if (box.Type == BoxType.BOX_BLOCK /*&& !box->object && !(box->iframe)*/ &&
                        box.Children != null)
                {
                    /* Down into children. */

                    if (box == margin_collapse)
                    {
                        /* Current margin collapsed though to this box.
                         * Unset margin_collapse. */
                        margin_collapse = null;
                    }

                    y = box.Padding[(int)BoxSide.TOP];
                    box = box.Children;
                    box.Y = y;
                    cy += y;
                    continue;
                }
                else if (box.Type == BoxType.BOX_BLOCK /*|| box->object*/ ||
                        (box.Flags & BoxFlags.IFRAME) != 0)
					cy += box.Padding[(int)BoxSide.TOP];

                if (box.Type == BoxType.BOX_BLOCK && box.Height == int.MinValue)
                {
                    box.Height = 0;
                    box.LayoutBlockAddScrollbar(BoxSide.BOTTOM);
                }

                cy += box.Height + box.Padding[(int)BoxSide.BOTTOM] +
                        box.Border[(int)BoxSide.BOTTOM].Width;
                cx -= box.X;
                y = box.Y + box.Padding[(int)BoxSide.TOP] + box.Height +
                        box.Padding[(int)BoxSide.BOTTOM] +
                        box.Border[(int)BoxSide.BOTTOM].Width;

                advance_to_next_box:
                if (box.Next == null)
                {
                    /* No more siblings:
                     * up to first ancestor with a sibling. */

                    do
                    {
                        if (box == margin_collapse)
                        {
                            /* Current margin collapsed though to
                             * this box.  Unset margin_collapse. */
                            margin_collapse = null;
                        }

                        /* Apply bottom margin */
                        if (max_pos_margin < box.Margin[(int)BoxSide.BOTTOM])
                            max_pos_margin = box.Margin[(int)BoxSide.BOTTOM];
                        else if (max_neg_margin < -box.Margin[(int)BoxSide.BOTTOM])
                            max_neg_margin = -box.Margin[(int)BoxSide.BOTTOM];

                        box = box.Parent;
                        if (box == block)
                            break;

                        /* Margin is invalidated if this is a box
                         * margins can't collapse through. */
                        if (box.Type == BoxType.BOX_BLOCK &&
                                (box.Flags & BoxFlags.MAKE_HEIGHT) != 0)
                        {
                            margin_collapse = null;
                            in_margin = false;
                            max_pos_margin = max_neg_margin = 0;
                        }

                        if (box.Height == int.MinValue)
                        {
                            box.Height = y - box.Padding[(int)BoxSide.TOP];

                            if (box.Type == BoxType.BOX_BLOCK)
                                box.LayoutBlockAddScrollbar(BoxSide.BOTTOM);
                        }
                        else
                            cy += box.Height -
                                    (y - box.Padding[(int)BoxSide.TOP]);

                        /* Apply any min-height and max-height to
                         * boxes in normal flow */
                        if (box.Style != null &&
                            box.Style.ComputedPosition() != CssPosition.CSS_POSITION_ABSOLUTE &&
                                box.LayoutApplyMinmaxHeight(content.UnitLenCtx, null))
                        {
                            /* Height altered */
                            /* Set current cy */
                            cy += box.Height -
                                    (y - box.Padding[(int)BoxSide.TOP]);
                        }

                        cy += box.Padding[(int)BoxSide.BOTTOM] + box.Border[(int)BoxSide.BOTTOM].Width;
                        cx -= box.X;
                        y = box.Y + box.Padding[(int)BoxSide.TOP] + box.Height +
                                box.Padding[(int)BoxSide.BOTTOM] +
                                box.Border[(int)BoxSide.BOTTOM].Width;

                    } while (box.Next == null);
                    if (box == block)
                        break;
                }

                /* To next sibling. */

                if (box == margin_collapse)
                {
                    /* Current margin collapsed though to this box.
                     * Unset margin_collapse. */
                    margin_collapse = null;
                }

                if (max_pos_margin < box.Margin[(int)BoxSide.BOTTOM])
                    max_pos_margin = box.Margin[(int)BoxSide.BOTTOM];
                else if (max_neg_margin < -box.Margin[(int)BoxSide.BOTTOM])
                    max_neg_margin = -box.Margin[(int)BoxSide.BOTTOM];

                box = box.Next;
                box.Y = y;
            }

            /* Account for bottom margin of last contained block */
            cy += max_pos_margin - max_neg_margin;

            /* Increase height to contain any floats inside (CSS 2.1 10.6.7). */
            for (box = block.float_children; box != null; box = box.next_float)
            {
                y = box.Y + box.Height + box.Padding[(int)BoxSide.BOTTOM] +
                        box.Border[(int)BoxSide.BOTTOM].Width + box.Margin[(int)BoxSide.BOTTOM];
                if (cy < y)
                    cy = y;
            }

            if (block.Height == int.MinValue)
            {
                block.Height = cy - block.Padding[(int)BoxSide.TOP];
                if (block.Type == BoxType.BOX_BLOCK)
                    block.LayoutBlockAddScrollbar(BoxSide.BOTTOM);
            }

            if (block.Style != null && block.Style.ComputedPosition() != CssPosition.CSS_POSITION_ABSOLUTE)
            {
                /* Block is in normal flow */
                block.LayoutApplyMinmaxHeight(content.UnitLenCtx, null);
            }

            /*
			if (block->gadget &&
					(block->gadget.Type == GADGET_TEXTAREA ||
					block->gadget.Type == GADGET_PASSWORD ||
					block->gadget.Type == GADGET_TEXTBOX))
			{
				plot_font_style_t fstyle;
				int ta_width = block.Padding[(int)BoxSide.LEFT] + block.Width +
						block.Padding[(int)BoxSide.RIGHT];
				int ta_height = block.Padding[(int)BoxSide.TOP] + block.Height +
						block.Padding[(int)BoxSide.BOTTOM];
				font_plot_style_from_css(content.UnitLenCtx,
						block->style, &fstyle);
				fstyle.background = NS_TRANSPARENT;
				textarea_set_layout(block->gadget->data.text.ta,
						&fstyle, ta_width, ta_height,
						block.Padding[(int)BoxSide.TOP], block.Padding[(int)BoxSide.RIGHT],
						block.Padding[(int)BoxSide.BOTTOM], block.Padding[(int)BoxSide.LEFT]);
			}*/

            return true;
		}


		// layout_compute_relative_offset()
		// layout.c:5443
		/**
		 * Compute a box's relative offset as per CSS 2.1 9.4.3
		 *
		 * \param  unit_len_ctx  Length conversion context
		 * \param  box	Box to compute relative offsets for.
		 * \param  x	Receives relative offset in x.
		 * \param  y	Receives relative offset in y.
		 */
		static void LayoutComputeRelativeOffset(
		CssUnitCtx unit_len_ctx,
		Box box,
		out int x,
		out int y)
		{
			int left, right, top, bottom;
			Box containing_block;

			Debug.Assert(box != null && box.Parent != null && box.Style != null &&
					box.Style.ComputedPosition() == CssPosition.CSS_POSITION_RELATIVE);

			if (box.float_container != null &&
					(box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_LEFT ||
					box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_RIGHT))
			{
				containing_block = box.float_container;
			}
			else
			{
				containing_block = box.Parent;
			}

			box.LayoutComputeOffsets(unit_len_ctx, containing_block, out top, out right, out bottom, out left);

			if (left == int.MinValue && right == int.MinValue)
				left = right = 0;
			else if (left == int.MinValue)
				/* left is auto => computed = -right */
				left = -right;
			else if (right == int.MinValue)
				/* right is auto => computed = -left */
				right = -left;
			else
			{
				/* over constrained => examine direction property
				 * of containing block */
				if (containing_block.Style != null &&
						containing_block.Style.ComputedDirection() == CssDirectionEnum.CSS_DIRECTION_RTL)
				{
					/* right wins */
					left = -right;
				}
				else
				{
					/* assume LTR in all other cases */
					right = -left;
				}
			}

			Debug.Assert(left == -right);

			if (top == int.MinValue && bottom == int.MinValue)
			{
				top = bottom = 0;
			}
			else if (top == int.MinValue)
			{
				top = -bottom;
			}
			else
			{
				/* bottom is int.MinValue, or neither are int.MinValue */
				bottom = -top;
			}

			Log.Print(LogChannel.Layout, $"left {left}, right {right}, top {top}, bottom {bottom}");

			x = left;
			y = top;
		}

		// layout_position_relative
		// layout.c:5525
		/**
		 * Adjust positions of relatively positioned boxes.
		 *
		 * \param  unit_len_ctx  Length conversion context
		 * \param  root  box to adjust the position of
		 * \param  fp    box which forms the block formatting context for children of
		 *		 "root" which are floats
		 * \param  fx    x offset due to intervening relatively positioned boxes
		 *               between current box, "root", and the block formatting context
		 *               box, "fp", for float children of "root"
		 * \param  fy    y offset due to intervening relatively positioned boxes
		 *               between current box, "root", and the block formatting context
		 *               box, "fp", for float children of "root"
		 */
		static void
		LayoutPositionRelative(
				CssUnitCtx unit_len_ctx,
				Box root,
				Box fp,
				int fx,
				int fy)
		{
			Box box; /* for children of "root" */
			Box fn;  /* for block formatting context box for children of
					  * "box" */
			Box fc;  /* for float children of the block formatting context,
					  * "fp" */
			int x, y;    /* for the offsets resulting from any relative
					  * positioning on the current block */
			int fnx, fny;    /* for affsets which apply to flat children of "box" */

			/**\todo ensure containing box is large enough after moving boxes */

			Debug.Assert(root != null);

			/* Normal children */
			for (box = root.Children; box!=null; box = box.Next)
			{

				if (box.Type == BoxType.BOX_TEXT)
					continue;

				/* If relatively positioned, get offsets */
				if (box.Style != null && box.Style.ComputedPosition() == CssPosition.CSS_POSITION_RELATIVE)
					LayoutComputeRelativeOffset(unit_len_ctx, box, out x, out y);
				else
					x = y = 0;

				/* Adjust float coordinates.
				 * (note float x and y are relative to their block formatting
				 * context box and not their parent) */
				if (box.Style != null && (box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_LEFT ||
						box.Style.ComputedFloat() == CssFloat.CSS_FLOAT_RIGHT) &&
						(fx != 0 || fy != 0))
				{
					/* box is a float and there is a float offset to
					 * apply */
					for (fc = fp.float_children; fc != null; fc = fc.next_float)
					{
						if (box == fc.Children)
						{
							/* Box is floated in the block
							 * formatting context block, fp.
							 * Apply float offsets. */
							box.X += fx;
							box.Y += fy;
							fx = fy = 0;
						}
					}
				}

				if (box.float_children != null)
				{
					fn = box;
					fnx = fny = 0;
				}
				else
				{
					fn = fp;
					fnx = fx + x;
					fny = fy + y;
				}

				/* recurse first */
				LayoutPositionRelative(unit_len_ctx, box, fn, fnx, fny);

				/* Ignore things we're not interested in. */
				if (box.Style == null || (box.Style != null &&
						box.Style.ComputedPosition() != CssPosition.CSS_POSITION_RELATIVE))
					continue;

				box.X += x;
				box.Y += y;

				/* Handle INLINEs - their "children" are in fact
				 * the sibling boxes between the INLINE and
				 * INLINE_END boxes */
				if (box.Type == BoxType.BOX_INLINE && box.InlineEnd != null)
				{
					Box b;
					for (b = box.Next; b!=null && b != box.InlineEnd;	b = b.Next)
					{
						b.X += x;
						b.Y += y;
					}
				}
			}
		}

		// layout_get_box_bbox()
		// layout.c:5627
		/**
		 * Find a box's bounding box relative to itself, i.e. the box's border edge box
		 *
		 * \param  unit_len_ctx  Length conversion context
		 * \param  box      box find bounding box of
		 * \param  desc_x0  updated to left of box's bbox
		 * \param  desc_y0  updated to top of box's bbox
		 * \param  desc_x1  updated to right of box's bbox
		 * \param  desc_y1  updated to bottom of box's bbox
		 */
		static void
		LayoutGetBoxBbox(
				CssUnitCtx unit_len_ctx,
				Box box,
				ref int desc_x0, ref int desc_y0,
				ref int desc_x1, ref int desc_y1)
		{
			desc_x0 = -box.Border[(int)BoxSide.LEFT].Width;
			desc_y0 = -box.Border[(int)BoxSide.TOP].Width;
			desc_x1 = box.Padding[(int)BoxSide.LEFT] + box.Width + box.Padding[(int)BoxSide.RIGHT] +
					box.Border[(int)BoxSide.RIGHT].Width;
			desc_y1 = box.Padding[(int)BoxSide.TOP] + box.Height + box.Padding[(int)BoxSide.BOTTOM] +
					box.Border[(int)BoxSide.BOTTOM].Width;

			/* To stop the top of text getting clipped when css line-height is
			 * reduced, we increase the top of the descendant bbox. */
			if (box.Type == BoxType.BOX_BLOCK && box.Style != null &&
					box.Style.ComputedOverflowY() == CssOverflowEnum.CSS_OVERFLOW_VISIBLE /*&& box->object == NULL*/)
			{
				Fixed font_size = Fixed.F_0;
				CssUnit font_unit = CssUnit.CSS_UNIT_PT;
				int text_height;

				box.Style.ComputedFontSize(ref font_size, ref font_unit);
				text_height = unit_len_ctx.Len2DevicePx(box.Style, font_size, font_unit).RawValue;
				text_height = new Fixed(text_height * 3 / 4, true).ToInt();
				desc_y0 = (desc_y0 < -text_height) ? desc_y0 : -text_height;
			}
		}

		// layout_update_descendant_bbox()
		// layout.c:5670
		/**
		 * Apply changes to box descendant_[xy][01] values due to given child.
		 *
		 * \param  unit_len_ctx  Length conversion context
		 * \param  box      box to update
		 * \param  child    a box, which may affect box's descendant bbox
		 * \param  off_x    offset to apply to child->x coord to treat as child of box
		 * \param  off_y    offset to apply to child->y coord to treat as child of box
		 */
		static void
		LayoutUpdateDescendantBbox(
				CssUnitCtx unit_len_ctx,
				Box box,
				Box child,
				int off_x,
				int off_y)
		{
			int child_desc_x0 = 0, child_desc_y0 = 0, child_desc_x1 = 0, child_desc_y1 = 0;

			/* get coordinates of child relative to box */
			int child_x = child.X - off_x;
			int child_y = child.Y - off_y;

			bool html_object = false; //(child->object && content_get_type(child->object) == CONTENT_HTML);

			var overflow_x = CssOverflowEnum.CSS_OVERFLOW_VISIBLE;
			var overflow_y = CssOverflowEnum.CSS_OVERFLOW_VISIBLE;

			if (child.Style != null)
			{
				overflow_x = child.Style.ComputedOverflowX();
				overflow_y = child.Style.ComputedOverflowY();
			}

			/* Get child's border edge */
			LayoutGetBoxBbox(unit_len_ctx, child,
					ref child_desc_x0, ref child_desc_y0,
					ref child_desc_x1, ref child_desc_y1);

			if (overflow_x == CssOverflowEnum.CSS_OVERFLOW_VISIBLE &&
					html_object == false) {
				/* get child's descendant bbox relative to box */
				child_desc_x0 = child.DescendantX0;
				child_desc_x1 = child.DescendantX1;
			}
			if (overflow_y == CssOverflowEnum.CSS_OVERFLOW_VISIBLE &&
					html_object == false) {
				/* get child's descendant bbox relative to box */
				child_desc_y0 = child.DescendantY0;
				child_desc_y1 = child.DescendantY1;
			}

			child_desc_x0 += child_x;
			child_desc_y0 += child_y;
			child_desc_x1 += child_x;
			child_desc_y1 += child_y;

			/* increase box's descendant bbox to contain descendants */
			if (child_desc_x0 < box.DescendantX0)
				box.DescendantX0 = child_desc_x0;
			if (child_desc_y0 < box.DescendantY0)
				box.DescendantY0 = child_desc_y0;
			if (box.DescendantX1 < child_desc_x1)
				box.DescendantX1 = child_desc_x1;
			if (box.DescendantY1 < child_desc_y1)
				box.DescendantY1 = child_desc_y1;
		}

		// layout_calculate_descendant_bboxes()
		// layout.c:5733
		/**
		 * Recursively calculate the descendant_[xy][01] values for a laid-out box tree
		 * and inform iframe browser windows of their size and position.
		 *
		 * \param  unit_len_ctx  Length conversion context
		 * \param  box      tree of boxes to update
		 */
		static void LayoutCalculateDescendantBboxes(
				CssUnitCtx unit_len_ctx,
				Box box)
		{
			Box child;

			Debug.Assert(box.Width != int.MaxValue);
			Debug.Assert(box.Height != int.MinValue);
			/* Debug.Assert((box.width >= 0) && (box.height >= 0)); */

			// Initialise box's descendant box to border edge box
			LayoutGetBoxBbox(unit_len_ctx, box,
					ref box.DescendantX0, ref box.DescendantY0,
					ref box.DescendantX1, ref box.DescendantY1);

			// Extend it to contain HTML contents if box is replaced
			/*if (box.object && content_get_type(box.object) == CONTENT_HTML) {
				if (box.descendant_x1 < content_get_width(box.object))
					box.descendant_x1 = content_get_width(box.object);
				if (box.descendant_y1 < content_get_height(box.object))
					box.descendant_y1 = content_get_height(box.object);
			}

			if (box.iframe != NULL) {
				int x, y;
				box_coords(box, &x, &y);

				browser_window_set_position(box.iframe, x, y);
				browser_window_set_dimensions(box.iframe,
						box.width, box.height);
				browser_window_reformat(box.iframe, true,
						box.width, box.height);
			}*/

			if (box.Type == BoxType.BOX_INLINE || box.Type == BoxType.BOX_TEXT)
				return;

			if (box.Type == BoxType.BOX_INLINE_END)
			{
				box = box.InlineEnd;
				for (child = box.Next; child!=null;
						child = child.Next)
				{
					if (child.Type == BoxType.BOX_FLOAT_LEFT ||
							child.Type == BoxType.BOX_FLOAT_RIGHT)
						continue;

					LayoutUpdateDescendantBbox(unit_len_ctx, box, child, box.X, box.Y);

					if (child == box.InlineEnd)
						break;
				}
				return;
			}

			if ((box.Flags & BoxFlags.REPLACE_DIM) != 0)
				/* Box's children aren't displayed if the box is replaced */
				return;

			for (child = box.Children; child!=null; child = child.Next)
			{
				if (child.Type == BoxType.BOX_FLOAT_LEFT ||
						child.Type == BoxType.BOX_FLOAT_RIGHT)
					continue;

				LayoutCalculateDescendantBboxes(unit_len_ctx, child);

				if (box.Style != null && box.Style.ComputedOverflowX() == CssOverflowEnum.CSS_OVERFLOW_HIDDEN &&
						box.Style.ComputedOverflowY() == CssOverflowEnum.CSS_OVERFLOW_HIDDEN)
					continue;

				LayoutUpdateDescendantBbox(unit_len_ctx, box, child, 0, 0);
			}

			for (child = box.float_children; child != null; child = child.next_float)
			{
				Debug.Assert(child.Type == BoxType.BOX_FLOAT_LEFT ||
						child.Type == BoxType.BOX_FLOAT_RIGHT);

				LayoutCalculateDescendantBboxes(unit_len_ctx, child);

				LayoutUpdateDescendantBbox(unit_len_ctx, box, child, 0, 0);
			}

			/*
			if (box.ListMarker) {
				child = box.list_marker;
				layout_calculate_descendant_bboxes(unit_len_ctx, child);

				layout_update_descendant_bbox(unit_len_ctx, box, child, 0, 0);
			}*/
		}

		// content/handlers/html/layout.c:5823
		public void LayoutDocument(int width, int height)
        {
            Log.Print(LogChannel.Layout, $"Doing layout to {width}x{height} of {"url"}");

            var doc = Layout.RootBox;

            doc.LayoutMinmaxBlock(this);

			LayoutBlockFindDimensions(UnitLenCtx, width, height, 0, 0, doc);
			doc.X = doc.Margin[(int)BoxSide.LEFT] + doc.Border[(int)BoxSide.LEFT].Width;
			doc.Y = doc.Margin[(int)BoxSide.TOP] + doc.Border[(int)BoxSide.TOP].Width;
			width -= doc.Margin[(int)BoxSide.LEFT] + doc.Border[(int)BoxSide.LEFT].Width +
					doc.Padding[(int)BoxSide.LEFT] + doc.Padding[(int)BoxSide.RIGHT] +
					doc.Border[(int)BoxSide.RIGHT].Width + doc.Margin[(int)BoxSide.RIGHT];
			if (width < 0)
			{
				width = 0;
			}
			doc.Width = width;

            var ret = LayoutBlockContext(doc, height, this);

			// make <html> and <body> fill available height
			if (doc.Y + doc.Padding[(int)BoxSide.TOP] + doc.Height + doc.Padding[(int)BoxSide.BOTTOM] +
					doc.Border[(int)BoxSide.BOTTOM].Width + doc.Margin[(int)BoxSide.BOTTOM] < height)
			{
				doc.Height = height - (doc.Y + doc.Padding[(int)BoxSide.TOP] +
						doc.Padding[(int)BoxSide.BOTTOM] +
						doc.Border[(int)BoxSide.BOTTOM].Width +
						doc.Margin[(int)BoxSide.BOTTOM]);
				if (doc.Children != null)
					doc.Children.Height = doc.Height -
							(doc.Children.Margin[(int)BoxSide.TOP] +
							 doc.Children.Border[(int)BoxSide.TOP].Width +
							 doc.Children.Padding[(int)BoxSide.TOP] +
							 doc.Children.Padding[(int)BoxSide.BOTTOM] +
							 doc.Children.Border[(int)BoxSide.BOTTOM].Width +
							 doc.Children.Margin[(int)BoxSide.BOTTOM]);
			}

			//layout_lists(content, doc);
			doc.LayoutPositionAbsolute(doc, 0, 0, this);
			LayoutPositionRelative(UnitLenCtx, doc, doc, 0, 0);

			LayoutCalculateDescendantBboxes(UnitLenCtx, doc);
		}

        // css_unit_css2device_px()
        // unit.h:88
        public static Fixed Css2DevicePixel(
		        Fixed css_pixels,
		        Fixed device_dpi)
        {
	        return css_pixels * device_dpi / Fixed.F_96;
        }

		// css_unit_device2css_px()
		// unit.h:99
		public static Decimal Device2CssPixel(int devicePx, int deviceDpi)
        {
            return (Decimal)devicePx * 96 / deviceDpi;
        }
        public static Fixed Device2CssPixel(Fixed devicePx, Fixed deviceDpi)
        {
            return (devicePx * Fixed.F_96) / deviceDpi;
        }

        // content/handlers/html/html.c:306
        void GetDimensions()
        {
            // TODO: Get windows dimensions
            Fixed w = new Fixed(1000);
            Fixed h = new Fixed(1000);
            Fixed screenDpi = new Fixed(144);

            UnitLenCtx.ViewportWidth = Device2CssPixel(w, screenDpi);
            UnitLenCtx.ViewportHeight = Device2CssPixel(h, screenDpi);
            UnitLenCtx.DeviceDpi = screenDpi;
        }
    }
}
