﻿using HtmlParserSharp;
using SkiaSharpOpenGLBenchmark.css;
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

    // private.h:93
    public class HtmlContent
    {
        //CQ Dom;

        bool IsReflowing;
        bool IsReformat;

        BoxTree Layout;

        int BackgroundColor;

        int Width;
        int Height;

        public LinkedList<HtmlStylesheet> Stylesheets; // Stylesheets. Each may be NULL.
        public CssSelectionContext SelectionCtx; // Style selection context
        public CssMedia Media; // Style selection media specification
        public CssUnitCtx UnitLenCtx; // CSS length conversion context for document.

        string Universal; // Universal selector


        // html.c:590
        public HtmlContent()
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
        }

        public void LoadStylesheetFromFile(string file)
        {
            var contents = File.ReadAllText(file);
            if (contents.Length == 0) return;

            //var parser = new StylesheetParser();
            //var stylesheet = parser.Parse(contents);

            //Stylesheets.AddLast(stylesheet);
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
            var parser = new SimpleHtmlParser();
            var doc = parser.ParseString("<a name=\"coolname\" class=\"mylink fancy\" id=\"idtest\">link</a>");
            // style=\"width: auto\"
            //var doc = parser.ParseUrl("http://nginx.org/");

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
            //css.AppendData("a { color: red !important; }");
            css.AppendData("a { color: red; }");

            var hsheet = new HtmlStylesheet();
            hsheet.Sheet = css;
            Stylesheets.AddLast(hsheet);
            //css.Select(SelectionCtx, CssOrigin.CSS_ORIGIN_UA);

            var media = new CssMedia();
            var unitctx = new CssUnitCtx();
            var body = doc.GetElementsByTagName("body")[0].ChildNodes[0];
            Console.WriteLine(body.OuterXml);
            var sc = new CssSelectState(body, null, ref media, ref unitctx);

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
            Layout.DomToBox(html); // Root is set there in the end as ctx->content->layout

            // Get window's dimensions
            int width = 1000;
            int height = 1000;
            Reformat(width, height);
        }

        // content/handlers/html/html.c:1038
        public void Reformat(int width, int height)
        {
            IsReflowing = true;

            LayoutDocument(width, height);
        }

        public void Redraw()
        {
            // redraw.c:1944
        }

        // content/handlers/html/layout.c:5823
        public void LayoutDocument(int width, int height)
        {

        }

        // unit.h:99
        public Decimal Device2CssPixel(int devicePx, int deviceDpi)
        {
            return (Decimal)devicePx * 96 / deviceDpi;
        }
        public Fixed Device2CssPixel(Fixed devicePx, Fixed deviceDpi)
        {
            return (devicePx * Fixed.F_96) / deviceDpi;
        }

        // content/handlers/html/html.c:306
        void GetDimensions()
        {
            // TODO: Get windows dimensions
            Fixed w = new Fixed(2858);
            Fixed h = new Fixed(1381);
            Fixed screenDpi = new Fixed(144);

            UnitLenCtx.ViewportWidth = Device2CssPixel(w, screenDpi);
            UnitLenCtx.ViewportHeight = Device2CssPixel(h, screenDpi);
            UnitLenCtx.DeviceDpi = screenDpi;
        }
    }
}
