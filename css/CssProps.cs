﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SkiaSharpOpenGLBenchmark.css
{
    public enum CssBackgroundAttachment : uint
    {
        CSS_BACKGROUND_ATTACHMENT_INHERIT = 0x0,
        CSS_BACKGROUND_ATTACHMENT_FIXED = 0x1,
        CSS_BACKGROUND_ATTACHMENT_SCROLL = 0x2
    }

    // properties.h:194
    public enum CssBackgroundPositionEnum : byte
    {
        CSS_BACKGROUND_POSITION_INHERIT = 0x0,
        CSS_BACKGROUND_POSITION_SET = 0x1
    };

    // properties.h:312
    public enum CssColorEnum : byte
    {
        CSS_COLOR_INHERIT = 0x0,
        CSS_COLOR_COLOR = 0x1
    };

    // properties.h:491
    public enum CssFontFamilyEnum : byte
    {
        CSS_FONT_FAMILY_INHERIT = 0x0,
        /* Named fonts exist if pointer is non-NULL */
        CSS_FONT_FAMILY_SERIF = 0x1,
        CSS_FONT_FAMILY_SANS_SERIF = 0x2,
        CSS_FONT_FAMILY_CURSIVE = 0x3,
        CSS_FONT_FAMILY_FANTASY = 0x4,
        CSS_FONT_FAMILY_MONOSPACE = 0x5
    };

    public enum CssFontSizeEnum : byte
    {
        CSS_FONT_SIZE_INHERIT = 0x0,
        CSS_FONT_SIZE_XX_SMALL = 0x1,
        CSS_FONT_SIZE_X_SMALL = 0x2,
        CSS_FONT_SIZE_SMALL = 0x3,
        CSS_FONT_SIZE_MEDIUM = 0x4,
        CSS_FONT_SIZE_LARGE = 0x5,
        CSS_FONT_SIZE_X_LARGE = 0x6,
        CSS_FONT_SIZE_XX_LARGE = 0x7,
        CSS_FONT_SIZE_LARGER = 0x8,
        CSS_FONT_SIZE_SMALLER = 0x9,
        CSS_FONT_SIZE_DIMENSION = 0xa
    };


    public delegate CssStatus PropDispCascade();
    public delegate CssStatus PropDispSetFromHint(CssHint hint, ComputedStyle style);
    public delegate CssStatus PropDispInitial(CssSelectState state);
    public delegate CssStatus PropDispCompose();

    // dispatch.c:20
    public struct CssPropDispatch
    {
        public bool Inherited;

        public PropDispCascade Cascade;
        public PropDispSetFromHint SetFromHint;
        public PropDispInitial Initial;
        public PropDispCompose Compose;
    }

    public struct CssHintLength
    {
        public Fixed Value;
        public CssUnit Unit;

        public CssHintLength(Fixed val, CssUnit unit)
        {
            Value = val;
            Unit = unit;
        }
    }

    public class CssHint
    {
        //css_computed_clip_rect* clip;
        public Color Color;
        //css_computed_content_item* content;
        //css_computed_counter* counter;
        public Fixed Fixed;
        public int Integer;
        public CssHintLength Length;
        /*struct {
            css_hint_length h;
            css_hint_length v;
        } position;*/
        public string[] strings; // one string fits here

        public CssPropertiesEnum Prop; // Property index
        public byte Status;            // Property value

        public CssHint()
        {
            //strings = new string[1];
        }
    }

    public class CssProps
    {
        public CssPropDispatch[] Dispatch;

        // defaults, options.h:128
        PlotFontGenericFamily FontDefault = PlotFontGenericFamily.PLOT_FONT_FAMILY_SANS_SERIF;

        public CssProps()
        {
            const int numProps = (int)CssPropertiesEnum.CSS_N_PROPERTIES;
            Dispatch = new CssPropDispatch[numProps];

            int i;
            for (i = 0; i < numProps; i++)
            {
                if (i == (int)CssPropertiesEnum.CSS_PROP_AZIMUTH ||
                    i == (int)CssPropertiesEnum.CSS_PROP_BORDER_COLLAPSE ||
                    i == (int)CssPropertiesEnum.CSS_PROP_BORDER_SPACING ||
                    i == (int)CssPropertiesEnum.CSS_PROP_CAPTION_SIDE ||
                    i == (int)CssPropertiesEnum.CSS_PROP_COLOR ||
                    i == (int)CssPropertiesEnum.CSS_PROP_CURSOR ||
                    i == (int)CssPropertiesEnum.CSS_PROP_DIRECTION ||
                    i == (int)CssPropertiesEnum.CSS_PROP_ELEVATION ||
                    i == (int)CssPropertiesEnum.CSS_PROP_EMPTY_CELLS ||
                    (i >= (int)CssPropertiesEnum.CSS_PROP_FONT_FAMILY &&
                     i <= (int)CssPropertiesEnum.CSS_PROP_FONT_WEIGHT) ||
                    (i >= (int)CssPropertiesEnum.CSS_PROP_LETTER_SPACING &&
                     i <= (int)CssPropertiesEnum.CSS_PROP_LIST_STYLE_TYPE) ||
                     i == (int)CssPropertiesEnum.CSS_PROP_ORPHANS ||
                     i == (int)CssPropertiesEnum.CSS_PROP_PAGE_BREAK_INSIDE ||
                     i == (int)CssPropertiesEnum.CSS_PROP_PITCH_RANGE ||
                     i == (int)CssPropertiesEnum.CSS_PROP_PITCH ||
                     i == (int)CssPropertiesEnum.CSS_PROP_QUOTES ||
                     i == (int)CssPropertiesEnum.CSS_PROP_RICHNESS ||
                    (i >= (int)CssPropertiesEnum.CSS_PROP_SPEAK_HEADER &&
                     i <= (int)CssPropertiesEnum.CSS_PROP_STRESS) ||
                     i == (int)CssPropertiesEnum.CSS_PROP_TEXT_ALIGN ||
                     i == (int)CssPropertiesEnum.CSS_PROP_TEXT_TRANSFORM ||
                    (i >= (int)CssPropertiesEnum.CSS_PROP_VISIBILITY &&
                     i <= (int)CssPropertiesEnum.CSS_PROP_WIDOWS) ||
                     i == (int)CssPropertiesEnum.CSS_PROP_WORD_SPACING
                    )
                    Dispatch[i].Inherited = true;
                else
                    Dispatch[i].Inherited = false;

                Dispatch[i].Cascade = PropDispCascade_Default;
                Dispatch[i].SetFromHint = PropDispSetFromHint_Default;
                Dispatch[i].Initial = PropDispInitial_Default;
                Dispatch[i].Compose = PropDispCompose_Default;
            }

            // Set some dispatchers
            i = (int)CssPropertiesEnum.CSS_PROP_BACKGROUND_ATTACHMENT;
            Dispatch[i].Cascade = PropDispCascade_background_attachment;
            Dispatch[i].SetFromHint = PropDispSetFromHint_background_attachment;
            Dispatch[i].Initial = PropDispInitial_background_attachment;
            Dispatch[i].Compose = PropDispCompose_background_attachment;

            i = (int)CssPropertiesEnum.CSS_PROP_COLOR;
            Dispatch[i].Cascade = PropDispCascade_color;
            Dispatch[i].SetFromHint = PropDispSetFromHint_color;
            Dispatch[i].Initial = PropDispInitial_color;
            Dispatch[i].Compose = PropDispCompose_color;

            i = (int)CssPropertiesEnum.CSS_PROP_FONT_SIZE;
            Dispatch[i].Cascade = PropDispCascade_fontsize;
            Dispatch[i].SetFromHint = PropDispSetFromHint_fontsize;
            Dispatch[i].Initial = PropDispInitial_fontsize;
            Dispatch[i].Compose = PropDispCompose_fontsize;

        }
        // select.c:1814
        public void SetInitial(CssSelectState state, int i,
                               CssPseudoElement pseudo, XmlNode parent)
        {
            if (!Dispatch[i].Inherited ||
                (pseudo == CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE && parent == null))
            {
                Dispatch[i].Initial(state);
            }
        }

        // select.c:1693
        CssHint UserAgentDefaultForProperty(CssPropertiesEnum prop)
        {
            var hint = new CssHint();

            if (prop == CssPropertiesEnum.CSS_PROP_COLOR)
            {
                hint.Color = new Color(0xff000000);
                hint.Status = (byte)CssColorEnum.CSS_COLOR_COLOR;
            }
            else if (prop == CssPropertiesEnum.CSS_PROP_FONT_FAMILY)
            {
                //hint.Strings = null;
                switch (FontDefault)
                {
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_SANS_SERIF:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_SANS_SERIF;
                        break;
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_SERIF:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_SERIF;
                        break;
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_MONOSPACE:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_MONOSPACE;
                        break;
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_CURSIVE:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_CURSIVE;
                        break;
                    case PlotFontGenericFamily.PLOT_FONT_FAMILY_FANTASY:
                        hint.Status = (byte)CssFontFamilyEnum.CSS_FONT_FAMILY_FANTASY;
                        break;
                }
            }
            else if (prop == CssPropertiesEnum.CSS_PROP_QUOTES)
            {
                /** \todo Not exactly useful :) */
                //hint->data.strings = NULL;
                //hint->status = CSS_QUOTES_NONE;
                Console.WriteLine("UNIMPLEMENTED 193");
            }
            else if (prop == CssPropertiesEnum.CSS_PROP_VOICE_FAMILY)
            {
                /** \todo Fix this when we have voice-family done */
                //hint->data.strings = NULL;
                //hint->status = 0;
                Console.WriteLine("UNIMPLEMENTED 200");
            }
            else
            {
                throw new Exception("Invalid property");
                //return hint;
            }

            return hint;
        }

        #region background_attachment
        void SetBackgroundAttachment(ComputedStyle style, uint type)
        {
            const uint BACKGROUND_ATTACHMENT_INDEX = 11;
            const byte BACKGROUND_ATTACHMENT_SHIFT = 26;
            const uint BACKGROUND_ATTACHMENT_MASK = 0x0c000000;

            uint bits = style.i.bits[BACKGROUND_ATTACHMENT_INDEX];

            uint val = (type & 0x3) << BACKGROUND_ATTACHMENT_SHIFT;

            // 2bits: tt : type
            style.i.bits[BACKGROUND_ATTACHMENT_INDEX] = (bits & ~BACKGROUND_ATTACHMENT_MASK) | val;
        }

        CssStatus PropDispCascade_background_attachment()
        {
            Console.WriteLine("background_attachment UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispSetFromHint_background_attachment(CssHint hint, ComputedStyle style)
        {
            Console.WriteLine("background_attachment UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispInitial_background_attachment(CssSelectState state)
        {
            SetBackgroundAttachment(state.Computed,
                (uint)CssBackgroundAttachment.CSS_BACKGROUND_ATTACHMENT_SCROLL);
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispCompose_background_attachment()
        {
            Console.WriteLine("background_attachment UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        #endregion


        #region color
        CssStatus PropDispCascade_color()
        {
            Console.WriteLine("color UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispSetFromHint_color(CssHint hint, ComputedStyle style)
        {
            Console.WriteLine("color UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispInitial_color(CssSelectState state)
        {
            var hint = UserAgentDefaultForProperty(CssPropertiesEnum.CSS_PROP_COLOR);
            PropDispSetFromHint_color(hint, state.Computed);

            return CssStatus.CSS_OK;
        }
        CssStatus PropDispCompose_color()
        {
            Console.WriteLine("color UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        #endregion

        #region font-size
        // autogenerated_propset.h:1327
        CssStatus PropDispCascade_fontsize()
        {
            Console.WriteLine("UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispSetFromHint_fontsize(CssHint hint, ComputedStyle style)
        {
            style.SetFontSize((CssFontSizeEnum)hint.Status,
                    hint.Length.Value, hint.Length.Unit);
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispInitial_fontsize(CssSelectState state)
        {
            state.Computed.SetFontSize(CssFontSizeEnum.CSS_FONT_SIZE_MEDIUM,
                        new Fixed(0),
                        CssUnit.CSS_UNIT_PX);
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispCompose_fontsize()
        {
            Console.WriteLine("UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        #endregion


        #region Default handlers
        CssStatus PropDispCascade_Default()
        {
            Console.WriteLine("UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispSetFromHint_Default(CssHint hint, ComputedStyle style)
        {
            Console.WriteLine("UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispInitial_Default(CssSelectState state)
        {
            Console.WriteLine("UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        CssStatus PropDispCompose_Default()
        {
            Console.WriteLine("UNIMPLEMENTED");
            return CssStatus.CSS_OK;
        }
        #endregion
    }
}
