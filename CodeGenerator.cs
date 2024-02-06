using HtmlParserSharp;
using SkiaSharpOpenGLBenchmark.css;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark
{
    public static class CodeGenerator
    {
        public static void GenerateCodeStubs()
        {
            string code = "";

            // Declaration of the array
            const int numProps = (int)CssPropertiesEnum.CSS_N_PROPERTIES;
            int i;
            for (i = 0; i < numProps; i++)
            {
                bool inherited = false;
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
                {
                    inherited = true;
                }

                var ps = ((CssPropertiesEnum)i).ToString();
                ps = ps.Substring(9).ToLowerInvariant();
                code += $"new CssPropDispatch {{ // 0x{i:X} {ps.ToString()}\n    Inherited = {inherited.ToString().ToLower()},\n    Cascade = PropDispCascade_{ps},\n    Compose = PropDispCompose_{ps},\n    Initial = PropDispInitial_{ps},\n    SetFromHint = PropDispSFH_{ps}\n}},\n";
            }

            // Handlers
            var hc = "";

            for (i = 0; i < numProps; i++)
            {
                var ps = ((CssPropertiesEnum)i).ToString();
                ps = ps.Substring(9).ToLowerInvariant();

                hc += $"#region {ps}\n";

                hc += $"        static CssStatus PropDispCascade_{ps}(OpCode op, CssStyle style, ref int bi, ref int used, CssSelectState state)\n";
                hc += "        {\n";
                hc += "            Log.Unimplemented();\n            return CssStatus.CSS_OK;\n        }\n\n";

                hc += $"        static CssStatus PropDispSFH_{ps}(CssHint hint, ComputedStyle style)\n";
                hc += "        {\n";
                hc += "            Log.Unimplemented();\n            return CssStatus.CSS_OK;\n        }\n\n";

                hc += $"        static CssStatus PropDispInitial_{ps}(CssSelectState state)\n";
                hc += "        {\n";
                hc += "            Log.Unimplemented();\n            return CssStatus.CSS_OK;\n        }\n\n";

                hc += $"        static CssStatus PropDispCompose_{ps}()\n";
                hc += "        {\n";
                hc += "            Log.Unimplemented();\n            return CssStatus.CSS_OK;\n        }\n";

                hc += "#endregion\n\n";
            }
        }

        /* Descriptors are space separated key:value pairs brackets () are
         * used to quote in values.
         *
         * Examples:
         * list_style_image:CSS_PROP_LIST_STYLE_IMAGE IDENT:( INHERIT: NONE:0,LIST_STYLE_IMAGE_NONE IDENT:) URI:LIST_STYLE_IMAGE_URI
         *
         * list_style_position:CSS_PROP_LIST_STYLE_POSITION IDENT:( INHERIT: INSIDE:0,LIST_STYLE_POSITION_INSIDE OUTSIDE:0,LIST_STYLE_POSITION_OUTSIDE IDENT:)
        */

        // example: css__stylesheet_style_appendOPV (style, Opcode, Flags, Value)

        // css_property_parser_gen.c

        static bool GetKeyVal(string descriptor, ref int pos, out KeyValuePair<string, string> keyVal)
        {
            int kvlen = 0;

            var endpos = descriptor.IndexOf(' ', pos); // single space separated pairs
            if (endpos == -1)
            {
                // no space, but might be the end of the input
                kvlen = descriptor.Length - pos;
                if (kvlen == 0)
                {
                    keyVal = new KeyValuePair<string, string>();
                    return true;
                }
            }
            else
            {
                kvlen = endpos - pos;
            }

            //nkeyval = calloc(1, sizeof(struct keyval) + kvlen + 1);
            //memcpy(nkeyval + 1, * pos, kvlen);
            //nkeyval->key = (char*) nkeyval + sizeof(struct keyval);
            var kv = descriptor.Substring(pos, kvlen).Split(":"); // split key and value on :
            var val = "";
            if (kv.Length > 1)
                val = kv[1];

            keyVal = new KeyValuePair<string, string>(kv[0], val);

            pos += kvlen; // update position

            // skip spaces
            while ((pos < descriptor.Length) &&
	               (descriptor[pos] == ' ')) {
		        pos++;
	        }

            return false;
        }

        static void OutputHeader(StreamWriter sw, string descriptor, KeyValuePair<string, string> parserId, bool isGeneric)
        {
            string s = isGeneric ? ", enum css_properties_e op" : "";

            sw.WriteLine("/* This file is autogenerated");
            sw.WriteLine(" *");
            sw.WriteLine(" * Generated from:");
            sw.WriteLine(" *");
            sw.WriteLine($" * {descriptor}");
            sw.WriteLine(" */");
            sw.WriteLine($"// Parse {parserId.Key}");

            string param = "";

            if (isGeneric) param = ", CssPropertiesEnum op";

            sw.WriteLine("namespace SkiaSharpOpenGLBenchmark.css\r\n{");
            sw.WriteLine("    public partial class CssPropertyParser {");

            sw.WriteLine($"        public CssStatus Parse_{parserId.Key}(List<CssToken> tokens, ref int index, CssStyle style{param})");
            sw.WriteLine("        {");
            /*
            sw.WriteLine($"css_error css__parse_{parserId.Key}(css_language *c,");
            sw.WriteLine("        const parserutils_vector *vector, int *ctx,");
            sw.WriteLine($"        css_style *result{s})");
            sw.WriteLine("{");*/
        }

        static void OutputTokenTypeCheck(
            StreamWriter sw,
            bool doTokenCheck,
            List<KeyValuePair<string, string>> IDENT,
            List<KeyValuePair<string, string>> URI,
            List<KeyValuePair<string, string>> NUMBER)
        {
            sw.WriteLine("            int origIndex = index;");
            sw.WriteLine("            CssStatus error = CssStatus.CSS_OK;");
            sw.WriteLine(); ;
            sw.WriteLine("            if (index >= tokens.Count)\r\n            {\r\n                Console.WriteLine(\"ERROR: Invalid CSS 659\");\r\n                return CssStatus.CSS_INVALID;\r\n            }\r\n\r\n            var token = tokens[index++];\r\n");
        }

        static void OutputFooter(StreamWriter sw)
        {
            /*
            sw.WriteLine("    if (errpr != CSS_OK)");
            sw.WriteLine("        *ctx = orig_ctx;");
            sw.WriteLine("");
            sw.WriteLine("    return error;");
            sw.WriteLine("}");
            sw.WriteLine("");*/

            sw.WriteLine("        return error;\r\n    }\r\n    }\r\n}");

        }

        static void OutputIdent(StreamWriter sw, bool onlyIdent, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> IDENT)
        {
            int ident_count;

            sw.Write("            ");

            for (ident_count = 0; ident_count < IDENT.Count; ident_count++)
            {
                var ckv = IDENT[ident_count];

                sw.Write("if (");
                if (!onlyIdent)
                {
                    sw.Write(
                    "(token.Type == CssTokenType.CSS_TOKEN_IDENT) &&\r\n                ");
                }
                sw.WriteLine(
                    $"System.String.Equals(token.iData, \"{ckv.Key.ToLower()}\", StringComparison.OrdinalIgnoreCase))\r\n            {{");

                var enumPrefix = (parseId.Value == "op") ? "" : "CssPropertiesEnum.";

                if (ckv.Key == "INHERIT")
                {
                    sw.WriteLine("                style.AppendStyle(");
                    sw.WriteLine("                    new OpCode(");
                    sw.WriteLine($"                        (ushort){enumPrefix}{parseId.Value},");
                    sw.WriteLine("                        (byte)OpCodeFlag.FLAG_INHERIT,");
                    sw.WriteLine("                        0)");
                    sw.WriteLine("                );");
                }
                else
                {
                    var v2 = ckv.Value.Split(',');
                    v2[1] = "OpCodeValues." + v2[1];
                    sw.WriteLine($"                style.AppendStyle(new OpCode((ushort){enumPrefix}{parseId.Value}, {v2[0]}, (ushort){v2[1]}));");
                }
                sw.Write("            } else ");
            }
        }

        static void OutputNumber(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> kvlist)
        {
            var ckv = kvlist[0];
            int ident_count;

            sw.WriteLine("if (token.Type == CssTokenType.CSS_TOKEN_NUMBER) {");
            sw.WriteLine("            int consumed;\n");
            sw.WriteLine($"            var num = CssStylesheet.NumberFromString(token.iData, {ckv.Key}, out consumed);");
            sw.WriteLine("            // Invalid if there are trailing characters");
            sw.WriteLine("            if (consumed != token.iData.Length) {");
            sw.WriteLine("                index = origIndex;");
            sw.WriteLine("                return CssStatus.CSS_INVALID;");
            sw.WriteLine("            }");

            for (ident_count = 1; ident_count < kvlist.Count; ident_count++)
            {
                var ulkv = kvlist[ident_count];

                if (ulkv.Key == "RANGE")
                {
                    sw.WriteLine($"            if ({ulkv.Value}) {{");
                    sw.WriteLine("                index = origIndex;");
                    sw.WriteLine("                return CssStatus.CSS_INVALID;");
                    sw.WriteLine("            }\n");
                }

            }

            var enumPrefix = (parseId.Value == "op") ? "" : "CssPropertiesEnum.";

            //sw.WriteLine($"\t\terror = css__stylesheet_style_appendOPV(result, {parseId.Value}, 0, {ckv.Value});");
            sw.WriteLine($"            style.AppendStyle(new OpCode((ushort){enumPrefix}{parseId.Value}, 0, (ushort)OpCodeValues.{ckv.Value}));");
            //sw.WriteLine("\t\terror = css__stylesheet_style_append(result, num);");
            sw.WriteLine("            style.AppendStyle(new OpCode((uint)num.RawValue));");
            sw.Write("\t} else ");
        }

        static void OutputUri(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> kvlist)
        {

            sw.WriteLine("if (token.Type == CssTokenType.CSS_TOKEN_URI) {");
            sw.WriteLine("                string uri;");
            sw.WriteLine("                uint uri_snumber;");
            sw.WriteLine();
            sw.WriteLine("                /*error = c->sheet->resolve(c->sheet->resolve_pw,");
            sw.WriteLine("                c->sheet->url,");
            sw.WriteLine("                token->idata, &uri);");
            sw.WriteLine("                */ Log.Unimplemented();");
            sw.WriteLine();
            sw.WriteLine("                /*css__stylesheet_string_add(c->sheet, uri, &uri_snumber);*/");
            sw.WriteLine();
            sw.WriteLine($"                style.AppendStyle(new OpCode((ushort)CssPropertiesEnum.{parseId.Value}, 0, (ushort)OpCodeValues.{kvlist[0].Value}));");

            sw.WriteLine();
            sw.WriteLine("                /*error = css__stylesheet_style_append(result, uri_snumber);*/");
            sw.Write("            } else ");
        }

        static void OutputColor(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> COLOR)
        {

            sw.WriteLine("{");
            sw.WriteLine("                ushort value = 0;");
            sw.WriteLine("                uint color = 0;");
            sw.WriteLine("                index = origIndex;");

            var enumPrefix = (parseId.Value == "op") ? "" : "CssPropertiesEnum.";

            sw.WriteLine("                CssStylesheet.ParseProperty_ColourSpecifier(tokens, ref index, out value, out color);\r\n");
            sw.WriteLine($"                style.AppendStyle(new OpCode((ushort){enumPrefix}{parseId.Value}, 0, value));\r\n");
            sw.WriteLine("                if (value == (ushort)OpColor.COLOR_SET)");
            sw.WriteLine($"                    style.AppendStyle(new OpCode(color));");
            sw.WriteLine("            }");
        }

        static void OutputLengthUnit(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> kvlist)
        {
            var ckv = kvlist[0];
            int ident_count;

            sw.WriteLine("{");
            sw.WriteLine("                Fixed length;");
            sw.WriteLine("                uint unit;");
            sw.WriteLine("                index = origIndex;\n");
            sw.WriteLine($"                error = CssStylesheet.ParseProperty_UnitSpecifier(tokens, ref index, OpcodeUnit.{ckv.Key.Substring(5)}, out length, out unit);");
            sw.WriteLine("                if (error != CssStatus.CSS_OK) {");
            sw.WriteLine("                    index = origIndex;");
            sw.WriteLine("                    return error;");
            sw.WriteLine("                }\n");

            for (ident_count = 1; ident_count < kvlist.Count; ident_count++)
            {
                //struct keyval *ulkv = kvlist->item[ident_count];
                var ulkv = kvlist[ident_count];

                if (ulkv.Key == "ALLOW")
                {
                    sw.WriteLine($"                if (({ulkv.Value}) == false) {{");
                    sw.WriteLine("                    index = origIndex;");
                    sw.WriteLine("                    return CssStatus.CSS_INVALID;");
                    sw.WriteLine("                }\n");
                }
                else if (ulkv.Key == "DISALLOW")
                {
                    sw.WriteLine($"                if ({ulkv.Value}) {{");
                    sw.WriteLine("                    index = origIndex;");
                    sw.WriteLine("                    return CssStatus.CSS_INVALID;");
                    sw.WriteLine("                }\n");
                }
                else if (ulkv.Key == "MASK")
                {
                    sw.WriteLine($"                if ((unit & (uint)UnitMask.{ulkv.Value} ) == 0) {{");
                    sw.WriteLine("                    index = origIndex;");
                    sw.WriteLine("                    return CssStatus.CSS_INVALID;");
                    sw.WriteLine("                }\n");
                }
                else if (ulkv.Key == "RANGE")
                {
                    sw.WriteLine($"                if (length.RawValue {ulkv.Value}) {{");
                    sw.WriteLine("                    index = origIndex;");
                    sw.WriteLine("                    return CssStatus.CSS_INVALID;");
                    sw.WriteLine("                }\n");
                }
            }

            //sw.WriteLine($"                error = css__stylesheet_style_appendOPV(result, {parseId.Value}, 0, {ckv.Value});");
            var enumPrefix = (parseId.Value == "op") ? "" : "CssPropertiesEnum.";
            sw.WriteLine($"                style.AppendStyle(new OpCode((ushort){enumPrefix}{parseId.Value}, 0, (ushort)OpCodeValues.{ckv.Value}));\r\n");
            sw.WriteLine();
            //sw.WriteLine("                error = css__stylesheet_style_vappend(result, 2, length, unit);");
            sw.WriteLine("                style.AppendStyle(new OpCode((uint)length.RawValue));");
            sw.WriteLine("                style.AppendStyle(new OpCode(unit));");
            sw.WriteLine("            }\n");
        }

        static void OutputIdentList(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> kvlist)
        {
            var ckv = kvlist[0]; // list type : opv value

            if (ckv.Key != "STRING_OPTNUM")
            {
                Console.WriteLine($"unknown IDENT list type {ckv.Key}");
                throw new InvalidOperationException();
            }

            if (kvlist.Count < 2)
            {
                Console.WriteLine($"Not enough parameters to IDENT list type {ckv.Key}");
                throw new InvalidOperationException();
            }

            // list of IDENT and optional numbers
            var ikv = kvlist[1]; // numeric default : end condition

            var enumPrefix = (parseId.Value == "op") ? "" : "CssPropertiesEnum.";

            sw.WriteLine("{");
            sw.WriteLine($"                style.AppendStyle(new OpCode((ushort){enumPrefix}{parseId.Value}, 0, (ushort)OpCodeValues.{ckv.Value}));");
            sw.WriteLine("                while (token.Type == CssTokenType.CSS_TOKEN_IDENT) {");
            sw.WriteLine("                    uint snumber = 0;");
            sw.WriteLine("                    Fixed num;");
            sw.WriteLine("                    int pctx;");
            sw.WriteLine("");
            sw.WriteLine("                    //error = css__stylesheet_string_add(c->sheet, lwc_string_ref(token->idata), &snumber);");
            sw.WriteLine("                    Log.Unimplemented();");
            sw.WriteLine("                    style.AppendStyle(new OpCode(snumber));");
            sw.WriteLine("                    CssStylesheet.ConsumeWhitespace(tokens, ref index);");
            sw.WriteLine();
            sw.WriteLine("                    pctx = index;");

            //sw.WriteLine("                    token = parserutils_vector_iterate(vector, ctx);");
            sw.WriteLine("                    bool tokenNull = false;");
            sw.WriteLine("                    if (index >= tokens.Count - 1) {");
            sw.WriteLine("                        tokenNull = true;");
            sw.WriteLine("                    }");
            sw.WriteLine("                    else token = tokens[index++];");

            sw.WriteLine("                    if (!tokenNull && token.Type == CssTokenType.CSS_TOKEN_NUMBER) {");
            sw.WriteLine("                        int consumed = 0;\n");
            //sw.WriteLine("                        num = css__number_from_lwc_string(token->idata, true, &consumed);");
            sw.WriteLine($"                        num = CssStylesheet.NumberFromString(token.iData, true, out consumed);");
            sw.WriteLine("                        if (consumed != token.iData.Length) {");
            sw.WriteLine("                            index = origIndex;");
            sw.WriteLine("                            return CssStatus.CSS_INVALID;");
            sw.WriteLine("                        }");
            sw.WriteLine("                        CssStylesheet.ConsumeWhitespace(tokens, ref index);");
            sw.WriteLine();
            sw.WriteLine("                        pctx = index;");
            //sw.WriteLine("                        token = parserutils_vector_iterate(vector, ctx);");
            sw.WriteLine("                        tokenNull = false;");
            sw.WriteLine("                        if (index >= tokens.Count - 1) {");
            sw.WriteLine("                            tokenNull = true;");
            sw.WriteLine("                        }");
            sw.WriteLine("                        else token = tokens[index++];");

            sw.WriteLine("                    } else {");
            sw.WriteLine($"                        num = new Fixed({ikv.Key});");
            sw.WriteLine("                    }");
            sw.WriteLine("");
            sw.WriteLine("                    style.AppendStyle(new OpCode((uint)num.RawValue));");
            sw.WriteLine("                    if (tokenNull)");
            sw.WriteLine("                        break;");
            sw.WriteLine("");
            sw.WriteLine("                    if (token.Type == CssTokenType.CSS_TOKEN_IDENT) {");
            //sw.WriteLine($"                    error = css__stylesheet_style_append(result, {ckv.Value});");
            sw.WriteLine($"                        style.AppendStyle(new OpCode((uint)OpCodeValues.{ckv.Value}));");
            sw.WriteLine("                    } else {");
            sw.WriteLine("                        index = pctx; // rewind one token back");
            sw.WriteLine("                    }");
            sw.WriteLine("                }");
            sw.WriteLine("");
            //sw.WriteLine($"                error = css__stylesheet_style_append(result, {ikv.Value});");
            sw.WriteLine($"                style.AppendStyle(new OpCode((uint)OpCodeValues.{ikv.Value}));");
            sw.WriteLine("            }");
            sw.WriteLine("");
        }

        static void OutputInvalidcss(StreamWriter sw)
        {
            sw.WriteLine("{\r\n                error = CssStatus.CSS_INVALID;\r\n            }\r\n");
        }

        static void OutputWrap(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> WRAP)
        {
            var ckv = WRAP[0];
            var fn = ckv.Value;
            fn = fn.Substring(11); // strip the "css__parse" prefix

            var classPrefix = "";
            var enumPrefix = "CssPropertiesEnum";

            if (fn == "border_side" || fn == "border_bottom" ||
                fn == "border_top" || fn == "border_left" || fn == "border_right")
            {
                classPrefix = "CssStylesheet.";
                enumPrefix = "BorderSide";
            }

            fn = "Parse_" + fn;

            sw.WriteLine($"            return {classPrefix}{fn}(tokens, ref index, style, {enumPrefix}.{parseId.Value});");
            sw.WriteLine("        }");
            sw.WriteLine("    }");
            sw.WriteLine("}");
        }

        static bool ProcessDescriptor(StreamWriter sw, string descriptor)
        {
            var str_INHERIT = "INHERIT";
            var ident_inherit = new KeyValuePair<string, string>(str_INHERIT, "");

            int curpos = 0;
            KeyValuePair<string, string> rkv; // current read key:val
            List<KeyValuePair<string, string>> curlist;
            bool do_token_check = true; // if the check for valid tokens is done
            bool only_ident = true; // if the only token type is ident
            bool isGeneric = false;

            var baselist = new List<KeyValuePair<string, string>>();
            var IDENT = new List<KeyValuePair<string, string>>();
            var IDENT_LIST = new List<KeyValuePair<string, string>>();
            var LENGTH_UNIT = new List<KeyValuePair<string, string>>();
            var URI = new List<KeyValuePair<string, string>>();
            var WRAP = new List<KeyValuePair<string, string>>();
            var NUMBER = new List<KeyValuePair<string, string>>();
            var COLOR = new List<KeyValuePair<string, string>>();

            curlist = baselist;

            while (curpos < descriptor.Length)
            {
                var err = GetKeyVal(descriptor, ref curpos, out rkv);
                if (err)
                {
                    Console.WriteLine($"Token error at offset {curpos}");
                    return true;
                }

                if (rkv.Key == "WRAP")
                {
                    WRAP.Add(rkv);
                    only_ident = false;
                }
                else if (rkv.Key == "NUMBER")
                {
                    if (rkv.Value[0] == '(')
                    {
                        curlist = NUMBER;
                    }
                    else if (rkv.Value[0] == ')')
                    {
                        curlist = baselist;
                    }
                    else
                    {
                        NUMBER.Add(rkv);
                    }
                    only_ident = false;
                }
                else if (rkv.Key == "IDENT")
                {
                    if (rkv.Value[0] == '(')
                    {
                        curlist = IDENT;
                    }
                    else if (rkv.Value[0] == ')')
                    {
                        curlist = baselist;
                    }
                    else if (rkv.Value == str_INHERIT)
                    {
                        IDENT.Add(ident_inherit);
                    }
                }
                else if (rkv.Key == "IDENT_LIST")
                {
                    if (rkv.Value[0] == '(')
                    {
                        curlist = IDENT_LIST;
                    }
                    else if (rkv.Value[0] == ')')
                    {
                        curlist = baselist;
                    }
                }
                else if (rkv.Key == "LENGTH_UNIT")
                {
                    if (rkv.Value[0] == '(')
                    {
                        curlist = LENGTH_UNIT;
                    }
                    else if (rkv.Value[0] == ')')
                    {
                        curlist = baselist;
                    }
                    only_ident = false;
                    do_token_check = false;
                }
                else if (rkv.Key == "COLOR")
                {
                    COLOR.Add(rkv);
                    do_token_check = false;
                    only_ident = false;
                }
                else if (rkv.Key == "URI")
                {
                    URI.Add(rkv);
                    only_ident = false;
                }
                else if (rkv.Key == "GENERIC")
                {
                    isGeneric = true;
                }
                else
                {
                    // just append to current list
                    curlist.Add(rkv);
                }
            }

            if (baselist.Count != 1)
            {
                Console.WriteLine($"Incorrect base element count (got {baselist.Count} expected 1)");
            }

            // header
            OutputHeader(sw, descriptor, baselist[0], isGeneric);

            if (WRAP.Count > 0)
            {
                OutputWrap(sw, baselist[0], WRAP);
            }
            else
            {
                // check token type is correct
                OutputTokenTypeCheck(sw, do_token_check, IDENT, URI, NUMBER);

                if (IDENT.Count > 0)
                    OutputIdent(sw, only_ident, baselist[0], IDENT);

                if (URI.Count > 0)
                    OutputUri(sw, baselist[0], URI);

                if (NUMBER.Count > 0)
                    OutputNumber(sw, baselist[0], NUMBER);

                // terminal blocks, these end the ladder ie no trailing else
                if (COLOR.Count > 0)
                {
                    OutputColor(sw, baselist[0], COLOR);
                }
                else if (LENGTH_UNIT.Count > 0)
                {
                    OutputLengthUnit(sw, baselist[0], LENGTH_UNIT);
                }
                else if (IDENT_LIST.Count > 0)
                {
                    OutputIdentList(sw, baselist[0], IDENT_LIST);
                }
                else
                {
                    OutputInvalidcss(sw);
                }

                OutputFooter(sw);
            }

            return false;
        }

        public static void GeneratePropertyParsers()
        {
            // Open properties.gen file and process it line-by-line

            //using (MemoryStream stream = new MemoryStream())
            var lines = File.ReadAllLines("properties.gen");
            for (var i = 0; i < lines.Length; i += 1)
            {
                var line = lines[i];

                // Skip blank lines and comments
                if (line.Length == 0 ||
                    line[0] == '#')
                    continue;

                // Replace some symbols
                line = line.Replace("F_100", "Fixed.F_100");
                line = line.Replace("INTTOFIX(100)", "Fixed.F_100");

                // Debug using "color" only
                var name = line.Split(':')[0];
                //if (name != "counter_increment")
                    //continue;

                using (var stream = File.Create($"autogenerated/autogenerated_{name}.cs"))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {

                        if (ProcessDescriptor(sw, line))
                            break;
                    }
                }
            }
        }
    }
}
