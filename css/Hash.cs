using SkiaSharpOpenGLBenchmark.css;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkiaSharpOpenGLBenchmark.css
{
    // hash.h:24
    public struct CssHashSelectionRequirments
    {
        public CssQname Qname;         // Element name, or universal "*"
        public string Class;           // Name of class, or NULL
        public string Id;              // Name of id, or NULL
        public string Uni;             // Universal element string "*"
        public CssMedia Media;         // Media spec we're selecting for
        public CssUnitCtx UnitCtx;     // Document unit conversion context.
        public CssBloom NodeBloom;     // Node's bloom filter
    }

    class CssHashEntry
    {
        public CssSelector Sel;
        public CssBloom SelChainBloom; // multiple, for some reason
        //CssHashEntry Next;

        public CssHashEntry()
        {
            Sel = null;
        }

        //FIXME: Maybe move those to CssBloom class?

        // Add a selector detail to the bloom filter, if the detail is relevant.
        // hash.c:690
        void ChainBloomAddDetail(CssSelectorDetail d)
        {
            // d is const here!

            switch (d.Type)
            {
                case CssSelectorType.CSS_SELECTOR_ELEMENT:
                    // Don't add universal element selector to bloom
                    if (d.Qname.Name.Length == 1 &&
                        d.Qname.Name[0] == '*')
                    {
                        return;
                    }
                    goto case CssSelectorType.CSS_SELECTOR_CLASS;
                // Fall through
                case CssSelectorType.CSS_SELECTOR_CLASS:
                case CssSelectorType.CSS_SELECTOR_ID:
                    // Element, Id and Class names always have the insensitive
                    // string set at css_selector_detail creation time.
                    if (d.Qname.Name != null)
                        SelChainBloom.AddHash(d.Qname.Name.HashCaseless());

                    break;

                default:
                    break;
            }
        }

        // Generate a selector chain's bloom filter
        // hash.c:727
        public void ChainBloomGenerate()
        {
            SelChainBloom = new CssBloom();

            CssSelector s = Sel;
            // Work back through selector chain...
            do
            {
                // ...looking for Ancestor/Parent combinators
                if (s.Data[0].Comb == CssCombinator.CSS_COMBINATOR_ANCESTOR ||
                    s.Data[0].Comb == CssCombinator.CSS_COMBINATOR_PARENT)
                {
                    foreach (var d in s.Data)
                    {
                        if (!d.Negate)
                        {
                            ChainBloomAddDetail(d);
                        }
                    }
                }

                s = s.Combinator;
            } while (s != null);
        }
    }

    /*
    struct CssHash
    {
        int Nslots;
        CssHashEntry Slots;
    }*/

    // libcss/src/select/hash.c:
    public class CssSelectorHash
    {
        List<CssHashEntry> Elements;
        List<CssHashEntry> Classes;
        List<CssHashEntry> Ids;
        List<CssHashEntry> Universal;

        // hash.c:119
        public CssSelectorHash()
        {
            Elements = new List<CssHashEntry>();
            Classes = new List<CssHashEntry>();
            Ids = new List<CssHashEntry>();
            Universal = new List<CssHashEntry>();
        }

        // hash.c:232
        public void Insert(CssSelector selector)
        {
            string name;

            // Work out which hash to insert into
            if ((name = IdName(selector)) != null)
            {
                // Named ID
                InsertIntoChain(ref Ids, selector);
            }
            else if ((name = ClassName(selector)) != null)
            {
                // Named class
                InsertIntoChain(ref Classes, selector);
            }
            else if (selector.Data[0].Qname.Name.Length != 1 ||
                    selector.Data[0].Qname.Name[0] != '*')
            {
                // Named element
                InsertIntoChain(ref Elements, selector);
            }
            else
            {
                // Universal chain
                InsertIntoChain(ref Universal, selector);
            }
        }

        // hash.c:280
        public void Remove(CssSelector selector)
        {
            Log.Unimplemented();
        }

        // hash.c:336
        public void Find(CssHashSelectionRequirments req, out CssSelector matched, out int indexInChain)
        {
            matched = null;
            indexInChain = -1;

            // Slots optimisation of hash lists is omitted
            if (Elements.Any() && Elements[0].Sel != null)
            {
                // Search through chain for first match
                foreach (var head in Elements)
                {
                    var match = System.String.Equals(
                        req.Qname.Name,
                        head.Sel.Data[0].Qname.Name,
                        StringComparison.OrdinalIgnoreCase);

                    if (match && head.Sel.Rule.HasBytecode())
                    {
                        if (req.NodeBloom.InBloom(head.SelChainBloom) &&
                            req.Media.RuleGoodForMedia(head.Sel.Rule, req.UnitCtx))
                        {
                            // Found a match
                            matched = head.Sel;
                            indexInChain = Elements.IndexOf(head);
                            break;
                        }

                    }

                }
            }
        }

        // Find the first selector that has a class that matches name
        // hash.c:406
        public void FindByClass(CssHashSelectionRequirments req, out CssSelector matched)
        {
            matched = null;

            // Let's omit the slots optimisation of hash lists
            if (Classes.Any() && Classes[0].Sel != null)
            {
                Log.Unimplemented();

                // Search through chain for first match
                foreach (var c in Classes)
                {
                    string n;
                    bool match = false;
                    /*
                    n = _class_name(head->sel);
			        if (n != NULL)
                    {
				        lerror = lwc_string_isequal(req->class->insensitive, n->insensitive, &match);
				        if (lerror != lwc_error_ok)
					        return css_error_from_lwc_error(lerror);

				        if (match && RULE_HAS_BYTECODE(head))
                        {
					        if (css_bloom_in_bloom(head->sel_chain_bloom, req->node_bloom) &&
					            _chain_good_for_element_name(head->sel, &(req->qname), req->uni) &&
					            mq_rule_good_for_media(head->sel->rule, req->unit_ctx,req->media))
                            {
						        // Found a match
                                matched = c;
						        break;
					        }
        				}
		        	}*/
                }

                //if (head == NULL)
                //    head = &empty_slot;
            }

            //(*iterator) = _iterate_classes;
            //(*matched) = (const css_selector**) head;
        }

        // hash.c:488
        public void FindById()
        {
            Log.Unimplemented();
        }

        // hash.c:568
        public void FindUniversal()
        {
            Log.Unimplemented();
        }

        // TODO: Maybe move those to CssSelector class?
        // hash.c:632
        string ClassName(CssSelector selector)
        {
            string name = null;

            foreach (var detail in selector.Data)
            {
                // Ignore :not(.class)
                if (detail.Type == CssSelectorType.CSS_SELECTOR_CLASS && detail.Negate == false)
                {
                    name = detail.Qname.Name;
                    break;
                }
            }

            return name;
        }

        // hash.c:659
        string IdName(CssSelector selector)
        {
            string name = null;

            foreach (var detail in selector.Data)
            {
                // Ignore :not(#id)
                if (detail.Type == CssSelectorType.CSS_SELECTOR_ID && detail.Negate == false)
                {
                    name = detail.Qname.Name;
                    break;
                }
            }

            return name;
        }

        // hash.c:788
        void InsertIntoChain(ref List<CssHashEntry> list, CssSelector selector)
        {
            var entry = new CssHashEntry();
            entry.Sel = selector;
            entry.ChainBloomGenerate();

            if (!list.Any())
            {
                list.Add(entry);
            } else
            {
                int index = -1;

                // Find place to insert entry
                for (int i=0;i<list.Count; i++)
                {
                    var search = list[i];
                    index = i;

                    // Sort by ascending specificity
                    if (search.Sel.Specificity > selector.Specificity)
                        break;

                    // Sort by ascending rule index
                    if (search.Sel.Specificity == selector.Specificity &&
                        search.Sel.Rule.Index > selector.Rule.Index)
                        break;
                }

                if (index < 1)
                {
                    // Insert as the first element
                    list.Insert(0, entry);
                }
                else
                {
                    // Insert at index
                    list.Insert(index, entry);
                }

                /*
# ifdef PRINT_CHAIN_BLOOM_DETAILS
                print_chain_bloom_details(entry->sel_chain_bloom);
#endif
                */
            }
        }

        // _iterate_elements
        // hash.c:905
        public CssSelector FindNextElement(CssHashSelectionRequirments req, ref int indexInChain)
        {
            // The end
            if (indexInChain >= Elements.Count - 1)
            {
                indexInChain = -1;
                return null;
            }

            // Iterate to the next one
            indexInChain++;

            // Slots optimisation of hash lists is omitted
            if (Elements.Any() && Elements[0].Sel != null)
            {
                // Search through chain for first match
                for (int i = indexInChain; i < Elements.Count; i++)
                {
                    var head = Elements[i];

                    if (head.Sel == null)
                        return null;

                    var match = System.String.Equals(
                        req.Qname.Name,
                        head.Sel.Data[0].Qname.Name,
                        StringComparison.OrdinalIgnoreCase);

                    if (match && head.Sel.Rule.HasBytecode())
                    {
                        if (req.NodeBloom.InBloom(head.SelChainBloom) &&
                            req.Media.RuleGoodForMedia(head.Sel.Rule, req.UnitCtx))
                        {
                            // Found a match
                            indexInChain = i;
                            return head.Sel;
                        }

                    }

                }
            }

            indexInChain = -1;
            return null;
        }
    }
}
