using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    public enum CssRuleType
    {
        CSS_RULE_UNKNOWN,
        CSS_RULE_SELECTOR,
        CSS_RULE_CHARSET,
        CSS_RULE_IMPORT,
        CSS_RULE_MEDIA,
        CSS_RULE_FONT_FACE,
        CSS_RULE_PAGE
    }

    public enum CssRuleParentType
    {
        CSS_RULE_PARENT_STYLESHEET,
        CSS_RULE_PARENT_RULE
    }

    // stylesheet.h:113
    public class CssRule
    {
        // Base part
        //public CssRule Parent; // containing rule or owning stylesheet (defined by ParentType)
        public CssStylesheet ParentSheet;
        public uint Index; // index in sheet
        public uint Items; // number of items (selectors) in rule
        public CssRuleType Type;
        public CssRuleParentType ParentType;

        // Selector
        CssStyle Style;
        public List<CssSelector> Selectors; //CssSelector[] Selectors;

        // Media
        //css_mq_query* media;
        //css_rule* first_child;
        //css_rule* last_child;

        // Font face
        //CssFontFace FontFace;

        // Rule page
        // same as Selector, but with 1 element array

        // Rule import
        string Url;
        //CssStyleSheet Sheet;

        // Charset
        string Encoding;

        public CssRule(CssRuleType type)
        {
            Index = 0;
            Items = 0;
            Type = type;
            ParentType = CssRuleParentType.CSS_RULE_PARENT_RULE;
            ParentSheet = null;
            Url = "";
            Encoding = "";
            Selectors = new List<CssSelector>();
        }

        // stylesheet.c:1220
        public void AddSelector(CssSelector selector)
        {
            if (Type != CssRuleType.CSS_RULE_SELECTOR)
            {
                Console.WriteLine("ERROR: Adding selector to non-selector rule!");
            }

            // Insert into rule's selector list
            Selectors.Add(selector);
            Items++;

            // Set selector's rule field
            selector.Rule = this;
        }

        // stylesheet.c:1257
        public void AppendStyle(CssStyle style)
        {
            if (Type != CssRuleType.CSS_RULE_SELECTOR && Type != CssRuleType.CSS_RULE_PAGE)
            {
                Console.WriteLine("ERROR: Trying to add style to the wrong type of rule");
                return;
            }

            if (Style != null)
            {
                Style.MergeStyle(style);
            }
            else
            {
                // No current style, so use this one
                Style = style;

                // Add to the sheet's size
                //sheet->size += (style->used* sizeof(css_code_t));
            }
        }

        // hash.c:82
        public bool HasBytecode()
        {
            // No bytecode if rule body is empty or wholly invalid --
            // Only interested in rules with bytecode

            if (Style != null)
                return true;
            else
                return false;
        }
    }
}
