using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;

namespace SkiaSharpOpenGLBenchmark.css
{
    public struct PropState
    {
        public uint Specificity;   // Specificity of property in result
        public bool Set;           // Whether property is set in result
        public CssOrigin Origin;   // Origin of property in result
        public bool Important;     // Importance of property in result
        public bool Inherit;       // Property is set to inherit
    }

    public class CssSelectState
    {
        IDomObject Node;            // Node we're selecting for
        CssMedia Media;             // Currently active media spec
        CssUnitCtx UnitCtx;         // Unit conversion context

        public CssSelectResults    Results; // Result set to populate

        public CssPseudoElement CurrentPseudo;  // Current pseudo element
        public ComputedStyle Computed;     // Computed style to populate

        //css_select_handler* handler;    // Handler functions
        //void* pw;                 // Client data for handlers

        CssStylesheet Sheet;        // Current sheet being processed

        CssOrigin CurrentOrigin;    // Origin of current sheet
        uint CurrentSpecificity;    // Specificity of current rule

        public CssQname Element;           // Element we're selecting for
        string Id;                  // Node id, if any
        string[] Classes;           // Node classes, if any
                                    //uint nClasses;              // Number of classes

        //reject_item reject_cache[128];  // Reject cache (filled from end)
        //reject_item* next_reject;   // Next free slot in reject cache

        //struct css_node_data *node_data;	// Data we'll store on node

        public PropState[][] Props; //prop_state props[CSS_N_PROPERTIES][CSS_PSEUDO_ELEMENT_COUNT];

        // select.c:1065
        public CssSelectState(IDomObject node, IDomObject parent, ref CssMedia media, ref CssUnitCtx unitCtx)
        {
            Node = node;
            Media = media;
            UnitCtx = unitCtx;
            Id = node.Id;

            // Allocate the result set
            Results = new CssSelectResults();

            Element = new CssQname();
            Element.Name = node.Name;
            Element.Namespace = null;

            // Get node's classes, if any
            Classes = node.Classes.ToArray();

            // TODO: Node pseudo classes

            // Props
            Props = new PropState[(int)CssPropertiesEnum.CSS_N_PROPERTIES][];
            for (int i = 0; i < (int)CssPropertiesEnum.CSS_N_PROPERTIES; i++)
                Props[i] = new PropState[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_COUNT];
        }

        // libcss/src/select/select.c:1835
        public void SelectFromSheet(CssSelectionContext ctx, CssStylesheet sheet, CssOrigin origin)
        {
            const int IMPORT_STACK_SIZE = 256;
            CssRule[] ImportStack = new CssRule[IMPORT_STACK_SIZE];
            int sp = 0;
            var s = sheet;
            var rule = s.RuleList.First;

            Console.WriteLine("UNIMPLEMENTED 187");

            do
            {
                // Find first non-charset rule, if we're at the list head
                if (rule == s.RuleList.First)
                {
                    while (rule != null && rule.Value.Type == CssRuleType.CSS_RULE_CHARSET)
                        rule = rule.Next;
                }

                if (rule != null && rule.Value.Type == CssRuleType.CSS_RULE_IMPORT)
                {
                    Console.WriteLine("UNIMPLEMENTED 245");
                }
                else
                {
                    // Gone past import rules in this sheet

                    // Process this sheet
                    //state->sheet = s;
                    //state->current_origin = origin;

                    //error = match_selectors_in_sheet(ctx, s, state);
                    //if (error != CSS_OK)
                    //  return error;

                    // Find next sheet to process
                    /*if (sp > 0)
                    {
                        sp--;
                        rule = import_stack[sp]->next;
                        s = import_stack[sp]->parent;
                    }
                    else
                    {
                        s = NULL;
                    }*/
                }

                if (s == null) break;
            } while (true);
        }
    }
}
