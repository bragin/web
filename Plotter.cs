using SkiaSharp;
using SkiaSharpOpenGLBenchmark.css;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SkiaSharpOpenGLBenchmark
{
    // Type of plot operation
    public enum PlotOperationType
    {
	    PLOT_OP_TYPE_NONE = 0, // No operation
	    PLOT_OP_TYPE_SOLID, // Solid colour
	    PLOT_OP_TYPE_DOT, // Dotted plot
	    PLOT_OP_TYPE_DASH, // Dashed plot
    }

    // plot_style.h:76
    // Plot style for stroke/fill plotters
    public struct PlotStyle
    {
        public PlotOperationType StrokeType; // Stroke plot type
        public int StrokeWidth; // Width of stroke, in pixels
        public uint StrokeColour; // Colour of stroke
        public PlotOperationType FillType; // Fill plot type
        public uint FillColour; // Colour of fill
    }

    // plot_style.h:88
    // Generic font family type
    public enum PlotFontGenericFamily
    {
        PLOT_FONT_FAMILY_SANS_SERIF = 0,
        PLOT_FONT_FAMILY_SERIF,
        PLOT_FONT_FAMILY_MONOSPACE,
        PLOT_FONT_FAMILY_CURSIVE,
        PLOT_FONT_FAMILY_FANTASY,
        PLOT_FONT_FAMILY_COUNT // Number of generic families
    }

    // Font plot flags
    public enum PlotFontFlags
    {
        FONTF_NONE = 0,
        FONTF_ITALIC = 1,
        FONTF_OBLIQUE = 2,
        FONTF_SMALLCAPS = 4,
    }

    // plot_style.h:111
    public struct PlotFontStyle
    {
        public string[] Families;
        public PlotFontGenericFamily Family; // Generic family to plot with
        public int Size; // Font size, in pt
        public int Weight; // Font weight: value in range [100,900] as per CSS
        public PlotFontFlags Flags; // Font flags
        public uint Background; // Background colour to blend to, if appropriate
        public uint Foreground; // Colour of text
    }

    // frontends/windows/plot.c
    public class Plotter
    {

        public const int PlotStyleRadix = 10; // 22:10 fixed point
        public const int PlotStyleScale = 1 << PlotStyleRadix; // Scaling factor for plot styles

        public SKSurface Surface;

        public Plotter()
        {
            // Create surface used for plotting
            SKImageInfo info = new SKImageInfo(1000, 1000, SKColorType.Rgba8888);
            Surface = SKSurface.Create(info);

            // Test output
            Surface.Canvas.Clear(SKColor.Parse("#c0c0c0"));

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(0xffccddee)
            };

            var rect = new SKRect(100,100,110,110);

            Surface.Canvas.DrawRect(rect, paint);


            /*var paint2 = new SKPaint()
			{
				TextSize = 16,
				Color = SKColors.Yellow,
				IsAntialias = true,
				Typeface = SKTypeface.FromFamilyName(
				"Arial",
				SKFontStyleWeight.Normal,
				SKFontStyleWidth.Normal,
				SKFontStyleSlant.Upright)
			};

			Surface.Canvas.DrawText("Privet!", 100, 100, paint2);*/

        }

        public void Rectangle(PlotStyle pstyle, Rect r)
        {
            //Log.Unimplemented($"({r.x0},{r.y0})-({r.x1},{r.y1})");

			var paint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = new SKColor((byte)(pstyle.StrokeColour & 0xff),
				(byte)((pstyle.StrokeColour & 0xff00) >> 8),
				(byte)((pstyle.StrokeColour & 0xff0000) >> 16))
            };

            var rect = new SKRect(r.x0, r.y0, r.x1, r.y1);

            Surface.Canvas.DrawRect(rect, paint);
        }

        public void Line()
        {
            Log.Unimplemented();
        }

        public void Polygon()
        {
            Log.Unimplemented();
        }

        public void Clip(Rect r)
        {
            Log.Unimplemented($"({r.x0},{r.y0})-({r.x1},{r.y1})");

            SKRect rect = new SKRect(r.x0, r.y0, r.x1, r.y1);
            //Surface.Canvas.ClipRect(rect);
        }

		// text()
		// plot.c:984
		public void Text(PlotFontStyle fstyle, int x, int y, string text)
		{
			Log.Unimplemented($"Draw text '{text}' at ({x},{y})");

            //(style->flags & FONTF_ITALIC) ? TRUE : FALSE

            //int nHeight = -MulDiv(style->size, GetDeviceCaps(hdc, LOGPIXELSY), 72 * PLOT_STYLE_SCALE);
            int dpi = 144;
            float nHeight = fstyle.Size / PlotStyleScale * dpi / 72;

            var paint2 = new SKPaint()
			{
				TextSize = nHeight,
				//				Color = new SKColor((byte)(fstyle.Foreground & 0xff),
				//					(byte)((fstyle.Foreground & 0xff00) >> 8),
				//					(byte)((fstyle.Foreground & 0xff0000) >> 16)),
				Color = new SKColor((byte)((fstyle.Foreground & 0xff0000) >> 16),
					(byte)((fstyle.Foreground & 0xff00) >> 8),
					(byte)(fstyle.Foreground & 0xff)),

				IsAntialias = true,
				Typeface = SKTypeface.FromFamilyName(
					"Arial",
					SKFontStyleWeight.Normal,
					SKFontStyleWidth.Normal,
					SKFontStyleSlant.Upright)
			};

            Surface.Canvas.DrawText(text, x, y, paint2);
        }
        public void Disc()
        {
            Log.Unimplemented();
        }
        public void Arc()
        {
            Log.Unimplemented();
        }
        public void Bitmap()
        {
            Log.Unimplemented();
        }
        public void Path()
        {
            Log.Unimplemented();
        }

        // win32_font_width()
        // frontends/windows/font.c:186
        public void GetFontWidth(PlotFontStyle fstyle, string textSource, int length, ref int width)
        {
            int dpi = 144;
            float nHeight = fstyle.Size / PlotStyleScale * dpi / 72;

            string text;

            // Caller may want to measure only part of the string
            if (textSource.Length != length)
                text = textSource.Substring(0, length);
            else
                text = textSource;

            // Measure text
            var paint3 = new SKPaint()
            {
                TextSize = nHeight,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName(
                    "Arial",
                    SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright)
            };

            SKFontMetrics metrics;
            paint3.GetFontMetrics(out metrics);

            SKRect bounds = new SKRect();
            float textWidth = paint3.MeasureText(text, ref bounds);

            width = (int)Math.Round(textWidth);


			// HACK
            /*
            switch (text)
            {
                case "some text ":
                    width = 103;
                    break;
                case " ":
                    width = 7;
                    break;
                case "and link":
                    width = 85;
                    break;
                case "and another p":
                    width = 151;
                    break;
                default:
                    break;
            }*/

            Log.Print(LogChannel.Layout, $"Font width of '{text}' size {fstyle.Size} is {width}");


			// Size = 0x3000
			// "some tex", w = 103
			// " ", w = 7
			// "and link", w = 85
			// "and another p", w = 151
		}

        // font.c:244 win32_font_position()
        void GetFontPosition(PlotFontStyle fstyle, string text, int maxWidth, ref int char_offset, ref int actual_x)
        {
            int dpi = 144;
            float nHeight = fstyle.Size / PlotStyleScale * dpi / 72;

            //var length = text.Length;

            var paint = new SKPaint()
            {
                TextSize = nHeight,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName(
                    "Arial",
                    SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright)
            };

            // 1.
            // offset = max characters fitting into "x" width
            // s = dimensions of string (s.cx is used)
            int charCount = (int)paint.BreakText(text, maxWidth);

            // 2. Compute width and height of "text" string of length "offset"
            // actual_x = computed width
            // char_offset = offset
            float textWidth = paint.MeasureText(text.Substring(0, charCount));

            actual_x = (int)Math.Round(textWidth);
            char_offset = (int)charCount;
        }

        // font.c:302 win32_font_split()
        public void SplitText(PlotFontStyle fstyle, string text, int x, ref int offset, ref int actual_x)
        {
            int c_off;

            // get the offset into teh string on the proposed position
            GetFontPosition(fstyle, text, x, ref offset, ref actual_x);

            // return the whole string fits in the proposed length
            if (offset == text.Length)
                return;

            c_off = offset;

            // walk backwards through string looking for space to break on
            while ((text[offset] != ' ') &&
                   offset > 0)
            {
                offset--;
            }

            // walk forwards through string looking for space if back failed
            if (offset == 0)
            {
                offset = c_off;
                while ((offset < text.Length) &&
                       (text[offset] != ' '))
                {
                    offset++;
                }
            }

            // find the actual string width of the break
            GetFontWidth(fstyle, text, offset, ref actual_x);

            //Log.Print(LogChannel.Text, DEEPDEBUG,
            //"ret %d Split %u chars at %ipx: Split at char %i (%ipx) - %.*s",
            //res, length, x, *offset, *actual_x, *offset, string);

        }


        // Helpers
        // font.c:39
        /**
         * Map a generic CSS font family to a generic plot font family
         *
         * \param css Generic CSS font family
         * \return Plot font family
         */
        public static PlotFontGenericFamily FontGenericFamily(CssFontFamilyEnum css)
        {
            PlotFontGenericFamily plot;

	        switch (css) {
	        case CssFontFamilyEnum.CSS_FONT_FAMILY_SERIF:
		        plot = PlotFontGenericFamily.PLOT_FONT_FAMILY_SERIF;
		        break;
	        case CssFontFamilyEnum.CSS_FONT_FAMILY_MONOSPACE:
		        plot = PlotFontGenericFamily.PLOT_FONT_FAMILY_MONOSPACE;
		        break;
	        case CssFontFamilyEnum.CSS_FONT_FAMILY_CURSIVE:
		        plot = PlotFontGenericFamily.PLOT_FONT_FAMILY_CURSIVE;
		        break;
	        case CssFontFamilyEnum.CSS_FONT_FAMILY_FANTASY:
		        plot = PlotFontGenericFamily.PLOT_FONT_FAMILY_FANTASY;
		        break;
	        case CssFontFamilyEnum.CSS_FONT_FAMILY_SANS_SERIF:
	        default:
		        plot = PlotFontGenericFamily.PLOT_FONT_FAMILY_SANS_SERIF;
		        break;
	        }

	        return plot;
        }

        // font.c:71
        /**
         * Map a CSS font weight to a plot weight value
         *
         * \param css  CSS font weight
         * \return Plot weight
         */
        public static int FontWeight(CssFontWeightEnum css)
        {
	        int weight;

	        switch (css) {
				case CssFontWeightEnum.CSS_FONT_WEIGHT_100:
					weight = 100;
					break;
				case CssFontWeightEnum.CSS_FONT_WEIGHT_200:
					weight = 200;
					break;
				case CssFontWeightEnum.CSS_FONT_WEIGHT_300:
					weight = 300;
					break;
				case CssFontWeightEnum.CSS_FONT_WEIGHT_400:
				case CssFontWeightEnum.CSS_FONT_WEIGHT_NORMAL:
				default:
					weight = 400;
					break;
				case CssFontWeightEnum.CSS_FONT_WEIGHT_500:
					weight = 500;
					break;
				case CssFontWeightEnum.CSS_FONT_WEIGHT_600:
					weight = 600;
					break;
				case CssFontWeightEnum.CSS_FONT_WEIGHT_700:
				case CssFontWeightEnum.CSS_FONT_WEIGHT_BOLD:
					weight = 700;
					break;
				case CssFontWeightEnum.CSS_FONT_WEIGHT_800:
					weight = 800;
					break;
				case CssFontWeightEnum.CSS_FONT_WEIGHT_900:
					weight = 900;
					break;
	        }

	        return weight;
        }

        // font.c:119
        /**
         * Map a CSS font style and font variant to plot font flags
         *
         * \param style    CSS font style
         * \param variant  CSS font variant
         * \return Computed plot flags
         */
        public static PlotFontFlags FontFlags(CssFontStyleEnum style, CssFontVariantEnum variant)
        {
            PlotFontFlags flags = PlotFontFlags.FONTF_NONE;

	        if (style == CssFontStyleEnum.CSS_FONT_STYLE_ITALIC)
		        flags |= PlotFontFlags.FONTF_ITALIC;
	        else if (style == CssFontStyleEnum.CSS_FONT_STYLE_OBLIQUE)
		        flags |= PlotFontFlags.FONTF_OBLIQUE;

	        if (variant == CssFontVariantEnum.CSS_FONT_VARIANT_SMALL_CAPS)
		        flags |= PlotFontFlags.FONTF_SMALLCAPS;

	        return flags;
        }

    }
}
