using System;
using System.Collections.Generic;
using System.Linq;
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

    internal struct CssSelectorDetailValue
    {
        public string Str;
        public int a;
        public int b;      // Data for x = an + b
    }

    internal struct CssSelectorDetail
    {
        public CssQname Qname;

        public CssSelectorDetailValue Value; // Detail value

        public CssSelectorType Type;      // Type of selector
        public CssCombinator Comb;      // Type of combinator
        public byte Next;      // Another selector detail follows
        public CssSelectorDetailValueType ValueType; // Type of value field
        public bool Negate;    // Detail match is inverted
    }

    public class CssSelector
    {
        CssSelector Combinator;                 // Combining selector
        public CssRule Rule;                           // Owning rule
        CssSelectorSpecificity Specificity;     // Specificity of selector, enum CssSelectorSpecificity
        CssSelectorDetail Data;		            // Selector data

        // stylesheet.c:788
        public CssSelector(ref CssQname qname, bool inlineStyle)
        {
            if (qname.Name == null)
                throw new ArgumentException("Parameter cannot be null", nameof(qname.Name));

            Data.Type = CssSelectorType.CSS_SELECTOR_ELEMENT;
            Data.Qname.Namespace = qname.Namespace;
            Data.Qname.Name = qname.Name;
            Data.Value.Str = null;
            Data.ValueType = CssSelectorDetailValueType.CSS_SELECTOR_DETAIL_VALUE_STRING;

            if (inlineStyle)
            {
                Specificity = CssSelectorSpecificity.CSS_SPECIFICITY_A;
            }
            else
            {
                // Initial specificity -- 1 for an element, 0 for universal
                if (qname.Name.Length != 1 || qname.Name[0] != '*')
                {
                    Specificity = CssSelectorSpecificity.CSS_SPECIFICITY_D;
                }
                else
                {
                    Specificity = 0;
                }
            }

            Data.Comb = CssCombinator.CSS_COMBINATOR_NONE;
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
