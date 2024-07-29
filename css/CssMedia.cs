using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    // types.h:51
    // Stylesheet media types
    public enum CssMediaType
    {
        CSS_MEDIA_AURAL = (1 << 0),
        CSS_MEDIA_BRAILLE = (1 << 1),
        CSS_MEDIA_EMBOSSED = (1 << 2),
        CSS_MEDIA_HANDHELD = (1 << 3),
        CSS_MEDIA_PRINT = (1 << 4),
        CSS_MEDIA_PROJECTION = (1 << 5),
        CSS_MEDIA_SCREEN = (1 << 6),
        CSS_MEDIA_SPEECH = (1 << 7),
        CSS_MEDIA_TTY = (1 << 8),
        CSS_MEDIA_TV = (1 << 9),
        CSS_MEDIA_ALL = CSS_MEDIA_AURAL | CSS_MEDIA_BRAILLE |
                                      CSS_MEDIA_EMBOSSED | CSS_MEDIA_HANDHELD |
                                      CSS_MEDIA_PRINT | CSS_MEDIA_PROJECTION |
                                      CSS_MEDIA_SCREEN | CSS_MEDIA_SPEECH |
                                      CSS_MEDIA_TTY | CSS_MEDIA_TV
    }

    // types.h:82
    // CSS unit
    public enum CssUnit
    {
        CSS_UNIT_PX = 0x00,
        CSS_UNIT_EX = 0x01,
        CSS_UNIT_EM = 0x02,
        CSS_UNIT_IN = 0x03,
        CSS_UNIT_CM = 0x04,
        CSS_UNIT_MM = 0x05,
        CSS_UNIT_PT = 0x06,
        CSS_UNIT_PC = 0x07,
        CSS_UNIT_CH = 0x08,
        CSS_UNIT_REM = 0x09,
        CSS_UNIT_LH = 0x0a,
        CSS_UNIT_VH = 0x0b,
        CSS_UNIT_VW = 0x0c,
        CSS_UNIT_VI = 0x0d,
        CSS_UNIT_VB = 0x0e,
        CSS_UNIT_VMIN = 0x0f,
        CSS_UNIT_VMAX = 0x10,
        CSS_UNIT_Q = 0x11,

        CSS_UNIT_PCT = 0x15,    /* Percentage */

        CSS_UNIT_DEG = 0x16,
        CSS_UNIT_GRAD = 0x17,
        CSS_UNIT_RAD = 0x18,

        CSS_UNIT_MS = 0x19,
        CSS_UNIT_S = 0x1a,

        CSS_UNIT_HZ = 0x1b,
        CSS_UNIT_KHZ = 0x1c
    }

    // Media orientations
    public enum CssMediaOrientation
    {
        CSS_MEDIA_ORIENTATION_PORTRAIT = 0,
        CSS_MEDIA_ORIENTATION_LANDSCAPE = 1
    }

    public struct CssMedia
    {
        // Media type
        public CssMediaType Type;

        // Viewport / page media features
        public Fixed Width;  // In css pixels
        public Fixed Height; // In css pixels
        public Fixed AspectRatio;
        public CssMediaOrientation Orientation;

        // Display quality media features
        //css_media_resolution resolution;
        //css_media_scan scan;
        public Fixed Grid; // boolean: {0|1}
        //css_media_update_frequency update;
        //css_media_overflow_block overflow_block;
        //css_media_overflow_inline overflow_inline;

        // Color media features
        public Fixed Color;      // colour bpp (0 for monochrome)
        public Fixed ColorIndex;
        public Fixed Monochrome; // monochrome bpp (0 for colour)
        public Fixed InvertedColors; // boolean: {0|1}

        // Interaction media features
        //css_media_pointer pointer;
        //css_media_pointer any_pointer;
        //css_media_hover hover;
        //css_media_hover any_hover;

        // Environmental media features
        //css_media_light_level light_level;

        // Scripting media features
        //css_media_scripting scripting;

        // libcss/src/select/mq.h:207
        public bool RuleGoodForMedia(CssRule rule, CssUnitCtx unitCtx)
        {
            Log.Unimplemented("Media queries are ignored for now");
            return true;
        }
    }

    /**
     * LibCSS unit conversion context.
     *
     * The client callback is optional.  It is used for measuring "ch"
     * (glyph '0' advance) and "ex" (height of the letter 'x') units.
     * If a NULL pointer is given, LibCSS will use a fixed scaling of
     * the "em" unit.
     */
    public struct CssUnitCtx
    {
        // Viewport width in CSS pixels. Used if unit is vh, vw, vi, vb, vmin, or vmax.
        public Fixed ViewportWidth;
        // Viewport height in CSS pixels. Used if unit is vh, vw, vi, vb, vmin, or vmax.
        public Fixed ViewportHeight;
        // Client default font size in CSS pixels.
        public Fixed FontSizeDefault;
        // Client minimum font size in CSS pixels.  May be zero.
        public Fixed FontSizeMinimum;

        // DPI of the device the style is selected for.
        public Fixed DeviceDpi;
        /**
         * Computed style for the document root element, needed for rem units.
         * May be NULL, in which case font_size_default is used instead, as
         * would be the case if rem unit is used on the root element.
         */
        public ComputedStyle RootStyle;

        // Optional client private word for measure callback.
        //void* pw;

        //Optional client callback for font measuring.
        //const css_unit_len_measure measure;

        static public CssUnit FromOpcodeUnit(OpcodeUnit unit)
        {
            switch (unit)
            {
                case OpcodeUnit.PX: return CssUnit.CSS_UNIT_PX;
                case OpcodeUnit.EX: return CssUnit.CSS_UNIT_EX;
                case OpcodeUnit.EM: return CssUnit.CSS_UNIT_EM;
                case OpcodeUnit.IN: return CssUnit.CSS_UNIT_IN;
                case OpcodeUnit.CM: return CssUnit.CSS_UNIT_CM;
                case OpcodeUnit.MM: return CssUnit.CSS_UNIT_MM;
                case OpcodeUnit.PT: return CssUnit.CSS_UNIT_PT;
                case OpcodeUnit.PC: return CssUnit.CSS_UNIT_PC;
                case OpcodeUnit.CH: return CssUnit.CSS_UNIT_CH;
                case OpcodeUnit.REM: return CssUnit.CSS_UNIT_REM;
                case OpcodeUnit.LH: return CssUnit.CSS_UNIT_LH;
                case OpcodeUnit.VH: return CssUnit.CSS_UNIT_VH;
                case OpcodeUnit.VW: return CssUnit.CSS_UNIT_VW;
                case OpcodeUnit.VI: return CssUnit.CSS_UNIT_VI;
                case OpcodeUnit.VB: return CssUnit.CSS_UNIT_VB;
                case OpcodeUnit.VMIN: return CssUnit.CSS_UNIT_VMIN;
                case OpcodeUnit.VMAX: return CssUnit.CSS_UNIT_VMAX;
                case OpcodeUnit.Q: return CssUnit.CSS_UNIT_Q;
                case OpcodeUnit.PCT: return CssUnit.CSS_UNIT_PCT;
                case OpcodeUnit.DEG: return CssUnit.CSS_UNIT_DEG;
                case OpcodeUnit.GRAD: return CssUnit.CSS_UNIT_GRAD;
                case OpcodeUnit.RAD: return CssUnit.CSS_UNIT_RAD;
                case OpcodeUnit.MS: return CssUnit.CSS_UNIT_MS;
                case OpcodeUnit.S: return CssUnit.CSS_UNIT_S;
                case OpcodeUnit.HZ: return CssUnit.CSS_UNIT_HZ;
                case OpcodeUnit.KHZ: return CssUnit.CSS_UNIT_KHZ;
                default:
                    return 0;
            }
        }
    }
}
