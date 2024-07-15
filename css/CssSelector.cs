using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    public enum CssSelectorSpecificity : int
    {
        CSS_SPECIFICITY_A = 0x01000000,
        CSS_SPECIFICITY_B = 0x00010000,
        CSS_SPECIFICITY_C = 0x00000100,
        CSS_SPECIFICITY_D = 0x00000001
    }

    public enum CssSelectorType
    {
        CSS_SELECTOR_ELEMENT,
        CSS_SELECTOR_CLASS,
        CSS_SELECTOR_ID,
        CSS_SELECTOR_PSEUDO_CLASS,
        CSS_SELECTOR_PSEUDO_ELEMENT,
        CSS_SELECTOR_ATTRIBUTE,
        CSS_SELECTOR_ATTRIBUTE_EQUAL,
        CSS_SELECTOR_ATTRIBUTE_DASHMATCH,
        CSS_SELECTOR_ATTRIBUTE_INCLUDES,
        CSS_SELECTOR_ATTRIBUTE_PREFIX,
        CSS_SELECTOR_ATTRIBUTE_SUFFIX,
        CSS_SELECTOR_ATTRIBUTE_SUBSTRING
    }

    public enum CssCombinator
    {
        CSS_COMBINATOR_NONE,
        CSS_COMBINATOR_ANCESTOR,
        CSS_COMBINATOR_PARENT,
        CSS_COMBINATOR_SIBLING,
        CSS_COMBINATOR_GENERIC_SIBLING
    }

    public enum CssSelectorDetailValueType
    {
        CSS_SELECTOR_DETAIL_VALUE_STRING,
        CSS_SELECTOR_DETAIL_VALUE_NTH
    }

    public struct CssSelectorDetailValue
    {
        public string Str;
        public int a;
        public int b;      // Data for x = an + b
    }

    public struct CssSelectorDetail
    {
        public CssQname Qname;

        public CssSelectorDetailValue Value; // Detail value

        public CssSelectorType Type;      // Type of selector
        public CssCombinator Comb;      // Type of combinator
        //public bool Next;      // Another selector detail follows
        public CssSelectorDetailValueType ValueType; // Type of value field
        public bool Negate;    // Detail match is inverted

        // stylesheet.c:917
        public CssSelectorDetail(CssSelectorType type,
            ref CssQname qname,
            ref CssSelectorDetailValue value,
            CssSelectorDetailValueType valueType,
            bool negate)
        {
            Type = type;
            Qname = qname;
            Value = value;
            ValueType = valueType;
            Negate = negate;
        }

        public CssSelectorDetail()
        {
            Type = CssSelectorType.CSS_SELECTOR_ELEMENT;
            Qname = new CssQname();
            Value = new CssSelectorDetailValue();
            ValueType = CssSelectorDetailValueType.CSS_SELECTOR_DETAIL_VALUE_STRING;
            Negate = false;
        }
    }

    public class CssSelector
    {
        public CssSelector Combinator;          // Combining selector
        public CssRule Rule;                    // Owning rule
        public uint Specificity;                // Specificity of selector, enum CssSelectorSpecificity
        public List<CssSelectorDetail> Data;    // Selector data

        // stylesheet.c:788
        public CssSelector(ref CssQname qname, bool inlineStyle)
        {
            if (qname.Name == null)
                throw new ArgumentException("Parameter cannot be null", nameof(qname.Name));

            Data = new List<CssSelectorDetail>();
            var detail = new CssSelectorDetail();

            detail.Type = CssSelectorType.CSS_SELECTOR_ELEMENT;
            detail.Qname.Namespace = qname.Namespace;
            detail.Qname.Name = qname.Name;
            detail.Value.Str = null;
            detail.ValueType = CssSelectorDetailValueType.CSS_SELECTOR_DETAIL_VALUE_STRING;
            detail.Comb = CssCombinator.CSS_COMBINATOR_NONE;

            Data.Insert(0, detail);

            if (inlineStyle)
            {
                Specificity = (uint)CssSelectorSpecificity.CSS_SPECIFICITY_A;
            }
            else
            {
                // Initial specificity -- 1 for an element, 0 for universal
                if (qname.Name.Length != 1 || qname.Name[0] != '*')
                {
                    Specificity = (uint)CssSelectorSpecificity.CSS_SPECIFICITY_D;
                }
                else
                {
                    Specificity = 0;
                }
            }

            Combinator = null;
        }

        // stylesheet.c:943
        public void AppendSpecific(CssSelectorDetail detail)
        {
            // Update parent's specificity
            switch (detail.Type)
            {
                case CssSelectorType.CSS_SELECTOR_CLASS:
                case CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS:
                case CssSelectorType.CSS_SELECTOR_ATTRIBUTE:
                case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_EQUAL:
                case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_DASHMATCH:
                case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_INCLUDES:
                case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_PREFIX:
                case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_SUFFIX:
                case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_SUBSTRING:
                    Specificity += (uint)CssSelectorSpecificity.CSS_SPECIFICITY_C;
                    break;
                case CssSelectorType.CSS_SELECTOR_ID:
                    Specificity += (uint)CssSelectorSpecificity.CSS_SPECIFICITY_B;
                    break;
                case CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT:
                case CssSelectorType.CSS_SELECTOR_ELEMENT:
                    Specificity += (uint)CssSelectorSpecificity.CSS_SPECIFICITY_D;
                    break;
            }

            Data.Add(detail);
        }

        /* Combine a pair of selectors. a is the first operand, this is the second
         * For example, given A + B, the combinator field of B would point at A,
         * with a combinator type of CSS_COMBINATOR_SIBLING.Thus, given B, we can
         * find its combinator.It is not possible to find B given A.
         */
        // stylesheet.c:1021 css__stylesheet_selector_combine()
        public CssStatus Combine(CssCombinator type, CssSelector a)
        {

            // Ensure that there is no existing combinator on B
            Debug.Assert(Combinator == null);

            // A must not contain a pseudo element
            //for (det = &a->data; det != NULL;)
            foreach (var det in a.Data)
            {
                if (det.Type == CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT)
                    return CssStatus.CSS_INVALID;
            }

            Combinator = a;

            // FIXME Can't modify directly,
            //Data[0].Comb = type;
            var d = Data[0];
            d.Comb = type;
            Data[0] = d;

            // And propagate A's specificity to B
            Specificity += a.Specificity;

            return CssStatus.CSS_OK;
        }


        // select.c:2960
        public void DumpChain(StreamWriter sw)
        {
            var comb = Data[0].Comb;
            if (comb != CssCombinator.CSS_COMBINATOR_NONE)
                Combinator.DumpChain(sw);

            if (comb == CssCombinator.CSS_COMBINATOR_ANCESTOR)
                sw.Write(" ");
            else if (comb == CssCombinator.CSS_COMBINATOR_SIBLING)
                sw.Write(" + ");
            else if (comb == CssCombinator.CSS_COMBINATOR_PARENT)
                sw.Write(" > ");

            int i = 0;
            foreach (var detail in Data)
            {
                switch (detail.Type)
                {
                    case CssSelectorType.CSS_SELECTOR_ELEMENT:
                        if (detail.Qname.Name.Length == 1 &&
                            detail.Qname.Name[0] == '*' &&
                            /*detail->next == 1*/ i < (Data.Count - 1))
                        {
                            break;
                        }
                        sw.Write(detail.Qname.Name);
                        break;
                    case CssSelectorType.CSS_SELECTOR_CLASS:
                        sw.Write($".{detail.Qname.Name}");
                        break;
                    case CssSelectorType.CSS_SELECTOR_ID:
                        sw.Write($"#{detail.Qname.Name}");
                        break;
                    case CssSelectorType.CSS_SELECTOR_PSEUDO_CLASS:
                        goto case CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT;
                    case CssSelectorType.CSS_SELECTOR_PSEUDO_ELEMENT:
                        sw.Write($":{detail.Qname.Name}");

                        if (detail.Value.Str != null)
                        {
                            sw.Write($"({detail.Value.Str})");
                        }
                        break;
                    case CssSelectorType.CSS_SELECTOR_ATTRIBUTE:
                        sw.Write($"[{detail.Qname.Name}]");
                        break;
                    case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_EQUAL:
                        sw.Write($"[{detail.Qname.Name}=\"{detail.Value.Str}\"]");
                        break;
                    case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_DASHMATCH:
                        sw.Write($"[{detail.Qname.Name}=\"{detail.Value.Str}\"]");
                        break;
                    case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_INCLUDES:
                        sw.Write($"[{detail.Qname.Name}~=\"{detail.Value.Str}\"]");
                        break;
                    case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_PREFIX:
                        sw.Write($"[{detail.Qname.Name}^=\"{detail.Value.Str}\"]");
                        break;
                    case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_SUFFIX:
                        sw.Write($"[{detail.Qname.Name}=\"{detail.Value.Str}\"]");
                        break;
                    case CssSelectorType.CSS_SELECTOR_ATTRIBUTE_SUBSTRING:
                        sw.Write($"[{detail.Qname.Name}*=\"{detail.Value.Str}\"]");
                        break;
                }
                i++;
            }
        }
    }

    public struct CssQname
    {
        /**
             * Namespace URI:
             *
             * NULL for no namespace
             * '*' for any namespace (including none)
             * URI for a specific namespace
             */
        public string Namespace;

        // Local part of qualified name
        public string Name;
    }
}
