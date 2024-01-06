﻿using SkiaSharpOpenGLBenchmark.css;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace SkiaSharpOpenGLBenchmark.css
{
    public struct PropState
    {
        public CssSelectorSpecificity Specificity;   // Specificity of property in result
        public bool Set;           // Whether property is set in result
        public CssOrigin Origin;   // Origin of property in result
        public bool Important;     // Importance of property in result
        public bool Inherit;       // Property is set to inherit
    }

    public enum CssNodeFlags : short
    {
        CSS_NODE_FLAGS_NONE = 0,
        CSS_NODE_FLAGS_HAS_HINTS = (1 << 0),
        CSS_NODE_FLAGS_HAS_INLINE_STYLE = (1 << 1),
        CSS_NODE_FLAGS_PSEUDO_CLASS_ACTIVE = (1 << 2),
        CSS_NODE_FLAGS_PSEUDO_CLASS_FOCUS = (1 << 3),
        CSS_NODE_FLAGS_PSEUDO_CLASS_HOVER = (1 << 4),
        CSS_NODE_FLAGS_PSEUDO_CLASS_LINK = (1 << 5),
        CSS_NODE_FLAGS_PSEUDO_CLASS_VISITED = (1 << 6),
        CSS_NODE_FLAGS_TAINT_PSEUDO_CLASS = (1 << 7),
        CSS_NODE_FLAGS_TAINT_ATTRIBUTE = (1 << 8),
        CSS_NODE_FLAGS_TAINT_SIBLING = (1 << 9),
        CSS_NODE_FLAGS__PSEUDO_CLASSES_MASK =
                (CSS_NODE_FLAGS_PSEUDO_CLASS_ACTIVE |
                 CSS_NODE_FLAGS_PSEUDO_CLASS_FOCUS |
                 CSS_NODE_FLAGS_PSEUDO_CLASS_HOVER |
                 CSS_NODE_FLAGS_PSEUDO_CLASS_LINK |
                 CSS_NODE_FLAGS_PSEUDO_CLASS_VISITED),
    }

    public class CssNodeData
    {
        CssSelectResults Partial;
        public CssBloom Bloom;
        CssNodeFlags Flags;

        public CssNodeData()
        {
            Flags = CssNodeFlags.CSS_NODE_FLAGS_NONE;
            Bloom = null;
            Partial = null;
        }
    }

    enum CssSelectRuleEnum
    {
        CSS_SELECT_RULE_SRC_ELEMENT,
        CSS_SELECT_RULE_SRC_CLASS,
        CSS_SELECT_RULE_SRC_ID,
        CSS_SELECT_RULE_SRC_UNIVERSAL
    };

    class CssSelectRuleSource
    {
        public CssSelectRuleEnum Source;
        public int Class;

        public CssSelectRuleSource()
        {
            Source = CssSelectRuleEnum.CSS_SELECT_RULE_SRC_ELEMENT;
            Class = 0;
        }
    }

    // libcss/src/select/select.h:64
    public class CssSelectState
    {
        XmlNode Node;               // Node we're selecting for
        CssMedia Media;             // Currently active media spec
        CssUnitCtx UnitCtx;         // Unit conversion context

        public CssSelectResults Results; // Result set to populate

        public CssPseudoElement CurrentPseudo;  // Current pseudo element
        public ComputedStyle Computed;     // Computed style to populate

        //css_select_handler* handler;    // Handler functions
        //void* pw;                 // Client data for handlers

        CssStylesheet Sheet;        // Current sheet being processed

        CssOrigin CurrentOrigin;    // Origin of current sheet
        CssSelectorSpecificity CurrentSpecificity;    // Specificity of current rule

        public CssQname Element;           // Element we're selecting for
        string Id;                  // Node id, if any
        string[] Classes;       // Node classes, if any
        //uint nClasses;              // Number of classes

        //reject_item reject_cache[128];  // Reject cache (filled from end)
        //reject_item* next_reject;   // Next free slot in reject cache

        CssNodeData NodeData;	// Data we'll store on node

        public PropState[][] Props; //prop_state props[CSS_N_PROPERTIES][CSS_PSEUDO_ELEMENT_COUNT];

        public CssProps Properties; // Not sure it's the right place either

        // select.c:1065
        public CssSelectState(XmlNode node, XmlNode parent, ref CssMedia media, ref CssUnitCtx unitCtx)
        {
            Node = node;
            Media = media;
            UnitCtx = unitCtx;

            // Allocate the result set
            Results = new CssSelectResults();

            NodeData = new CssNodeData();

            NodeData.Bloom = GetParentBloom(parent);

            // Get node's name
            Element = new CssQname();
            Element.Name = node.Name;
            Element.Namespace = null; // FIXME: check if it's correct

            // Get node's ID, if any
            Id = node.Attributes["id"] != null ? node.Attributes["id"].Value : null;

            // Get node's classes, if any
            var nodeAttr = node.Attributes["class"];
            if (nodeAttr != null)
            {
                Classes = nodeAttr.Value.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                Classes = null;
            }

            // TODO: Node pseudo classes
            Log.Unimplemented("Node pseudo classes");

            // Props
            Props = new PropState[(int)CssPropertiesEnum.CSS_N_PROPERTIES][];
            for (int i = 0; i < (int)CssPropertiesEnum.CSS_N_PROPERTIES; i++)
                Props[i] = new PropState[(int)CssPseudoElement.CSS_PSEUDO_ELEMENT_COUNT];

            // Css Properties dispatch handlers
            Properties = new CssProps();
        }

        // select.c:548
        CssBloom GetParentBloom(XmlNode parent)
        {
            CssBloom bloom = null;
            CssNodeData nodeData;

            // Get parent node's bloom filter
            if (parent != null)
            {
                // Get parent bloom filter

                nodeData = parent.GetNodeData();
                if (nodeData != null)
                {
                    bloom = nodeData.Bloom;
                }
            }

            if (bloom == null)
            {
                // Need to create parent bloom

                if (parent != null)
                {
                    /* TODO:
                     * Build & set the parent node's bloom properly.
                     * This will speed up the case where DOM change
                     * has caused bloom to get deleted.  For now we
                     * fall back to a fully satruated bloom filter,
                     * which is slower but perfectly valid.
                     */
                    bloom = new CssBloom();
                    nodeData = new CssNodeData();
                    nodeData.Bloom = bloom;
                    parent.SetNodeData(nodeData);

                }
            }
            else
            {
                /* No ancestors; empty bloom filter */
                /* The parent bloom is owned by the parent node's
                 * node data.  However, for the root node, there is
                 * no parent node to own the bloom filter.
                 * As such, we just use a pointer to static storage
                 * so calling code doesn't need to worry about
                 * whether the returned parent bloom is owned
                 * by something or not.
                 * Note, parent bloom is only read from, and not
                 * written to. */
                //static css_bloom empty_bloom[CSS_BLOOM_SIZE];
                bloom = new CssBloom();
            }

            return bloom;
        }

        // libcss/src/select/select.c:1835
        public void SelectFromSheet(CssSelectionContext ctx, CssStylesheet sheet, CssOrigin origin)
        {
            const int IMPORT_STACK_SIZE = 256;
            LinkedListNode<CssRule>[] importStack = new LinkedListNode<CssRule>[IMPORT_STACK_SIZE];
            int sp = 0;
            var s = sheet;
            var rule = s.RuleList.First;

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
                    Log.Unimplemented();
                }
                else
                {
                    // Gone past import rules in this sheet

                    // Process this sheet
                    Sheet = s;
                    CurrentOrigin = origin;

                    MatchSelectorsInSheet(ctx, s);

                    // Find next sheet to process
                    if (sp > 0)
                    {
                        sp--;
                        rule = importStack[sp].Next;
                        s = importStack[sp].Value.ParentSheet;
                    }
                    else
                    {
                        s = null;
                    }
                }

                if (s == null) break;
            } while (true);
        }

        // select.c:2015
        bool SelectorsPending(CssSelector node, CssSelector id, CssSelector[] classes, CssSelector univ)
        {
            bool pending = false;

            pending |= node != null;
            pending |= id != null;
            pending |= univ != null;

            if (classes != null)
            {
                for (int i = 0; i < classes.Length; i++)
                    pending |= classes[i] != null;
            }

            return pending;
        }

        // select.c:2033
        bool SelectorLessSpecific(CssSelector refd, CssSelector cand)
        {
            bool result = true;

            if (cand == null)
                return false;

            if (refd == null)
                return true;

            // Sort by specificity
            if (cand.Specificity < refd.Specificity)
            {
                result = true;
            }
            else if (refd.Specificity < cand.Specificity)
            {
                result = false;
            }
            else
            {
                // Then by rule index -- earliest wins
                if (cand.Rule.Index < refd.Rule.Index)
                    result = true;
                else
                    result = false;
            }

            return result;
        }

        // select.c:2058
        CssSelector SelectorNext(
            CssSelector node,
            CssSelector id,
            CssSelector[] classes,
            CssSelector univ,
            CssSelectRuleSource src)
        {
            CssSelector ret = null;

            if (SelectorLessSpecific(ret, node))
            {
                ret = node;
                src.Source = CssSelectRuleEnum.CSS_SELECT_RULE_SRC_ELEMENT;
            }

            if (SelectorLessSpecific(ret, id))
            {
                ret = id;
                src.Source = CssSelectRuleEnum.CSS_SELECT_RULE_SRC_ID;
            }

            if (SelectorLessSpecific(ret, univ)) {
                ret = univ;
                src.Source = CssSelectRuleEnum.CSS_SELECT_RULE_SRC_UNIVERSAL;
            }

            if (classes != null && classes.Length > 0)
            {
                for (int i = 0; i < classes.Length; i++)
                {
                    if (SelectorLessSpecific(ret, classes[i]))
                    {
                        ret = classes[i];
                        src.Source = CssSelectRuleEnum.CSS_SELECT_RULE_SRC_CLASS;
                        src.Class = i;
                    }
                }
            }

            return ret;
        }

        // select.c:2095
        void MatchSelectorsInSheet(CssSelectionContext ctx, CssStylesheet sheet)
        {
            // Set up general selector chain requirments
            CssHashSelectionRequirments req = new CssHashSelectionRequirments();
            req.Media = Media;
            req.UnitCtx = UnitCtx;
            req.NodeBloom = NodeData.Bloom;
            req.Uni = CssStrings.Universal;

            Log.Unimplemented("Media and UnitCtx are copied into req instead of being ref'd");

            CssSelector nodeSelectors;
            int nodeSelectorsIndex;
            CssSelector[] classSelectors = null;
            CssSelector idSelectors = null;
            CssSelector univSelectors = null;

            // Find hash chain that applies to current node
            req.Qname = Element;
            sheet.Selectors.Find(req, out nodeSelectors, out nodeSelectorsIndex);
            // node_iterator -> iterateElements

            int i;
            if (Classes != null && Classes.Length > 0)
            {
                // Find hash chains for node classes
                classSelectors = new CssSelector[Classes.Length];

                for (i = 0; i < Classes.Length; i++)
                {
                    req.Class = Classes[i];
                    sheet.Selectors.FindByClass(req, out classSelectors[i]);
                }
            }
            // class_iterator

            if (Id != null)
            {
                Log.Unimplemented();
                // Find hash chain for node ID
                /*
                req.id = state->id;
                error = css__selector_hash_find_by_id(sheet->selectors,
                        &req, &id_iterator, &id_selectors);
                if (error != CSS_OK)
                    goto cleanup;
                */
                // id_iterator
            }
/*
            // Find hash chain for universal selector
            error = css__selector_hash_find_universal(sheet->selectors, &req,
                    &univ_iterator, &univ_selectors);
            if (error != CSS_OK)
                goto cleanup;
*/
            // univ_iterator

            CssSelectRuleSource src = new CssSelectRuleSource();

            // Process matching selectors, if any
            while (SelectorsPending(nodeSelectors, idSelectors, classSelectors, univSelectors))
            {
                // Selectors must be matched in ascending order of specificity
                // and rule index. (c.f. css__outranks_existing())
                //
                // Pick the least specific/earliest occurring selector.
                var selector = SelectorNext(nodeSelectors, idSelectors, classSelectors, univSelectors, src);
                // We know there are selectors pending, so should have a
                // selector here
                Debug.Assert(selector != null);

                // Match and handle the selector chain
                MatchSelectorChain(ctx, selector);

                // Advance to next selector in whichever chain we extracted
                // the processed selector from.
                switch (src.Source)
                {
                    case CssSelectRuleEnum.CSS_SELECT_RULE_SRC_ELEMENT:
                        nodeSelectors = sheet.Selectors.FindNextElement(req, ref nodeSelectorsIndex);
                        break;

                    case CssSelectRuleEnum.CSS_SELECT_RULE_SRC_ID:
                        //error = id_iterator(&req, id_selectors, &id_selectors);
                        break;

                    case CssSelectRuleEnum.CSS_SELECT_RULE_SRC_UNIVERSAL:
                        //error = univ_iterator(&req, univ_selectors, &univ_selectors);
                        break;

                    case CssSelectRuleEnum.CSS_SELECT_RULE_SRC_CLASS:
                        //req.class = state->classes[src.class];
                        //error = class_iterator(&req, class_selectors[src.class], &class_selectors[src.class]);
                        break;

                    default:
                        break;
                }
            }
        }

        // select.c:2245
        void MatchSelectorChain(CssSelectionContext ctx, CssSelector selector)
        {
            bool match = false, may_optimise = true;
            //bool rejectedByCache;

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write("matching: ");
                    selector.DumpChain(sw);
                    sw.WriteLine();
                }
                Console.Write(Encoding.UTF8.GetString(stream.ToArray()));
            }

            /*
	        const css_selector *s = selector;
	        void *node = state->node;
	        const css_selector_detail *detail = &s->data;
            */

            /* Match the details of the first selector in the chain.
	         *
	         * Note that pseudo elements will only appear as details of
	         * the first selector in the chain, as the parser will reject
	         * any selector chains containing pseudo elements anywhere
	         * else.
	         */
            CssPseudoElement pseudo = CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE;
            var detailsEnum = selector.Data.GetEnumerator();
            var detailsNotEmpty = detailsEnum.MoveNext(); // Position it to the first element
            if (detailsNotEmpty)
                match = MatchDetails(ctx, Node, detailsEnum, out pseudo);
            else
                match = true;

            // Details don't match, so reject selector chain
            if (match == false)
                return;

            // Iterate up the selector chain, matching combinators
            CssSelector s = selector;
            do
            {
                //void* next_node = NULL;

                // Consider any combinator on this selector
                if (s.Data[0].Comb != CssCombinator.CSS_COMBINATOR_NONE &&
                        s.Combinator.Data[0].Qname.Name != CssStrings.Universal)
                {
                    // Named combinator
                    /*
                    may_optimise &=
                    (s->data.comb == CSS_COMBINATOR_ANCESTOR ||
                    s->data.comb == CSS_COMBINATOR_PARENT);

                    error = match_named_combinator(ctx, s->data.comb,
                            s->combinator, state, node, &next_node);
                    if (error != CSS_OK)
                        return error;

                    // No match for combinator, so reject selector chain
                    if (next_node == NULL)
                        return CSS_OK;*/
                    Log.Unimplemented();
                }
                else if (s.Data[0].Comb != CssCombinator.CSS_COMBINATOR_NONE)
                {
                    // Universal combinator
                    /*
                    may_optimise &=
                    (s->data.comb == CSS_COMBINATOR_ANCESTOR ||
                         s->data.comb == CSS_COMBINATOR_PARENT);

                    error = match_universal_combinator(ctx, s->data.comb,
                            s->combinator, state, node,
                            may_optimise, &rejected_by_cache,
                            &next_node);
                    if (error != CSS_OK)
                        return error;

                    // No match for combinator, so reject selector chain
                    if (next_node == NULL)
                    {
                        if (may_optimise && s == selector &&
                                rejected_by_cache == false)
                        {
                            update_reject_cache(state, s->data.comb,
                                    s->combinator);
                        }

                        return CSS_OK;
                    }*/
                    Log.Unimplemented();
                }

                // Details matched, so progress to combining selector
                s = s.Combinator;
                //node = next_node;
            } while (s != null);

            // If we got here, then the entire selector chain matched, so cascade
            CurrentSpecificity = selector.Specificity;

            // Ensure that the appropriate computed style exists
            if (Results.Styles[(int)pseudo] == null)
            {
                Results.Styles[(int)pseudo] = new ComputedStyle();
            }

            CurrentPseudo = pseudo;
            Computed = Results.Styles[(int)pseudo];

            CascadeStyle(selector.Rule.Style);
        }

        // select.c:2525
        bool MatchDetails(CssSelectionContext ctx, XmlNode node, List<CssSelectorDetail>.Enumerator details, out CssPseudoElement pseudoElement)
        {
            CssPseudoElement pseudo = CssPseudoElement.CSS_PSEUDO_ELEMENT_NONE;
            bool match = true;

            /* Skip the element selector detail, which is always first.
             * (Named elements are handled by match_named_combinator, so the
             * element selector detail always matches here.) */
            var nextExists = details.MoveNext();
            if (!nextExists)
            {
                pseudoElement = pseudo;
                return match;
            }

            // We match by default (if there are no details other than the element selector, then we must match)
            // match = true;

            /** \todo Some details are easier to test than others (e.g. dashmatch
             * actually requires looking at data rather than simply comparing
             * pointers). Should we consider sorting the detail list such that the
             * simpler details come first (and thus the expensive match routines
             * can be avoided unless absolutely necessary)? */
            Log.Unimplemented("Code needs implementing and testing");
            /*
            while (detail != NULL)
            {
                error = match_detail(ctx, node, detail, state, match, &pseudo);
                if (error != CSS_OK)
                    return error;

                // Detail doesn't match, so reject selector chain
                if (*match == false)
                    return CSS_OK;

                if (detail->next)
                    detail++;
                else
                    detail = NULL;
            }*/

            // Return the applicable pseudo element, if required
            pseudoElement = pseudo;
            return match;
        }

        // select.c:2833
        void CascadeStyle(CssStyle style)
        {
            //css_style s = *style;
            int used = style.Used;
            int bi = 0; // index in the bytecode

            while (used > 0)
            {
                //opcode_t op;
                //css_code_t opv = *s.bytecode;

                var opcode = style.Bytecode[bi];
                var op = (int)opcode.GetOpcode();
                var opv = opcode.GetOPV();

                //advance_bytecode(&s, sizeof(opv));
                used--;
                bi++;

                //error = prop_dispatch[op].cascade(opv, &s, state);
                Properties.Dispatch[op].Cascade(opcode, style, ref bi, ref used, this);
            }
        }

        // select.c:2856
        public bool OutranksExisting(OpCode op, bool important, bool inherit)
        {
            //prop_state* existing = &state->props[op][state->current_pseudo];
            var existing = Props[(int)op.GetOpcode()][(int)CurrentPseudo];
            bool outranks = false;

            /* Sorting on origin & importance gives the following:
             *
             *           | UA, - | UA, i | USER, - | USER, i | AUTHOR, - | AUTHOR, i
             *           |----------------------------------------------------------
             * UA    , - |   S       S       Y          Y         Y           Y
             * UA    , i |   S       S       Y          Y         Y           Y
             * USER  , - |   -       -       S          Y         Y           Y
             * USER  , i |   -       -       -          S         -           -
             * AUTHOR, - |   -       -       -          Y         S           Y
             * AUTHOR, i |   -       -       -          Y         -           S
             *
             * Where the columns represent the origin/importance of the property
             * being considered and the rows represent the origin/importance of
             * the existing property.
             *
             * - means that the existing property must be preserved
             * Y means that the new property must be applied
             * S means that the specificities of the rules must be considered.
             *
             * If specificities are considered, the highest specificity wins.
             * If specificities are equal, then the rule defined last wins.
             *
             * We have no need to explicitly consider the ordering of rules if
             * the specificities are the same because:
             *
             * a) We process stylesheets in order
             * b) The selector hash chains within a sheet are ordered such that
             *    more specific rules come after less specific ones and, when
             *    specificities are identical, rules defined later occur after
             *    those defined earlier.
             *
             * Therefore, where we consider specificity, below, the property
             * currently being considered will always be applied if its specificity
             * is greater than or equal to that of the existing property.
             */

            if (existing.Set == false)
            {
                // Property hasn't been set before, new one wins
                outranks = true;
            }
            else
            {
                Debug.Assert(CssOrigin.CSS_ORIGIN_UA < CssOrigin.CSS_ORIGIN_USER);
                Debug.Assert(CssOrigin.CSS_ORIGIN_USER < CssOrigin.CSS_ORIGIN_AUTHOR);

                if (existing.Origin < CurrentOrigin)
                {
                    /* New origin has more weight than existing one.
                     * Thus, new property wins, except when the existing
                     * one is USER, i. */
                    if (existing.Important == false || existing.Origin != CssOrigin.CSS_ORIGIN_USER)
                    {
                        outranks = true;
                    }
                }
                else if (existing.Origin == CurrentOrigin)
                {
                    /* Origins are identical, consider importance, except
                     * for UA stylesheets, when specificity is always
                     * considered (as importance is meaningless) */
                    if (existing.Origin == CssOrigin.CSS_ORIGIN_UA)
                    {
                        if (CurrentSpecificity >= existing.Specificity)
                        {
                            outranks = true;
                        }
                    }
                    else if (existing.Important == false && important)
                    {
                        // New is more important than old.
                        outranks = true;
                    }
                    else if (existing.Important && important == false)
                    {
                        // Old is more important than new
                    }
                    else
                    {
                        // Same importance, consider specificity
                        if (CurrentSpecificity >= existing.Specificity)
                        {
                            outranks = true;
                        }
                    }
                }
                else
                {
                    /* Existing origin has more weight than new one.
                     * Thus, existing property wins, except when the new
                     * one is USER, i. */
                    if (CurrentOrigin == CssOrigin.CSS_ORIGIN_USER && important)
                    {
                        outranks = true;
                    }
                }
            }

            if (outranks)
            {
                /* The new property is about to replace the old one.
                 * Update our state to reflect this. */
                existing.Set = true;
                existing.Specificity = CurrentSpecificity;
                existing.Origin = CurrentOrigin;
                existing.Important = important;
                existing.Inherit = inherit;
            }

            return outranks;
        }
    }
}
