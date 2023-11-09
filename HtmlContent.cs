using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsQuery;
//using ExCSS;
using SkiaSharpOpenGLBenchmark.css;

namespace SkiaSharpOpenGLBenchmark
{
    class HtmlStylesheet
    {
        CssStylesheet Sheet;
        bool Modified;
        IDomObject Node;

        public HtmlStylesheet()
        {
            Sheet = null;
            Modified = false;
            Node = null;
        }
    }

    // private.h:93
    class HtmlContent
    {
        CQ Dom;

        bool IsReflowing;
        bool IsReformat;

        BoxTree Layout;

        int BackgroundColor;

        int Width;
        int Height;

        public LinkedList<CssStylesheet> Stylesheets; // Stylesheets. Each may be NULL.
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
            Stylesheets = new LinkedList<CssStylesheet>();

            /* stylesheet 0 is the base style sheet,
             * stylesheet 1 is the quirks mode style sheet,
             * stylesheet 2 is the adblocking stylesheet,
             * stylesheet 3 is the user stylesheet */

            LoadStylesheetFromFile("..\\..\\default.css");
        }

        public void LoadStylesheetFromFile(string file)
        {
            var contents = File.ReadAllText(file);
            if (contents.Length == 0) return;

            //var parser = new StylesheetParser();
            //var stylesheet = parser.Parse(contents);

            //Stylesheets.AddLast(stylesheet);
        }

        // css.c:648 STUB
        void CreateSelectionContext()
        {
            SelectionCtx = new CssSelectionContext();

            // Add sheets to it
        }

        // html.c:341, kind of
        public void LoadDocument()
        {
            //Dom = CQ.CreateFromUrl("http://nginx.org/");
            Dom = CQ.CreateDocument("<a name=\"coolname\" class=\"mylink fancy\">link</a>");

            var n = Dom["body"];
            var cstyle = n.Css("color");
            var sheets = Dom["style"];

            // My CSS stuff
            var css = new CssStylesheet("", "http://nginx.org", "Useragent", false);
            //css.AppendData("a { color: red !important; }");
            css.AppendData("a { color: red; }");
            //css.Select(SelectionCtx, CssOrigin.CSS_ORIGIN_UA);

            var media = new CssMedia();
            var unitctx = new CssUnitCtx();
            var sc = new CssSelectState(Dom["html"][0].ChildNodes[1].ChildNodes[0], null, ref media, ref unitctx);


            // ExCSS stuff
            /*
            var parser = new StylesheetParser();
            var stylesheet = parser.Parse(sheets[0].InnerHTML);
            var rule = stylesheet.StyleRules.First() as StyleRule;
            var selector = rule.SelectorText; // Yields .someClass
            var color = rule.Style.Color; // rgb(255, 0, 0)
            var image = rule.Style.BackgroundImage; // url('/images/logo.png')
            */
            /*
            var imageUrl = stylesheet.RuleSets
                        .SelectMany(r => r.Declarations)
                        .FirstOrDefault(d => d.Name.Equals("background-image", StringComparison.InvariantCultureIgnoreCase))
                        .Term
                        .ToString(); // Finds the url('/images/logo.png') image url
            */

            // AngleSharp version
            /*
            var config = Configuration.Default
                .WithDefaultLoader();
            //.WithJs()
            //.WithConsoleLogger(context => new StandardConsoleLogger());

            var bc = BrowsingContext.New(config);
            var doc = bc.OpenAsync("http://nginx.org").Result;
            var body = doc.GetElementsByTagName("body");*/

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
            //Layout.DomToBox(dom["body"][0]);
            Layout.DomToBox(Dom["html"][0]); // Root is set there in the end as ctx->content->layout

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
