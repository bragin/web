using HtmlParserSharp;
using SkiaSharpOpenGLBenchmark.css;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
            sw.WriteLine($"css_error css__parse_{parserId.Key}(css_language *c,");
            sw.WriteLine("        const parserutils_vector *vector, int *ctx,");
            sw.WriteLine($"        css_style *result{s})");
            sw.WriteLine("{");
        }

        static void OutputFooter(StreamWriter sw)
        {
            sw.WriteLine("    if (errpr != CSS_OK)");
            sw.WriteLine("        *ctx = orig_ctx;");
            sw.WriteLine("");
            sw.WriteLine("    return error;");
            sw.WriteLine("}");
            sw.WriteLine("");
        }

        static void OutputIdent(StreamWriter sw, bool onlyIdent, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> IDENT)
        {
            int ident_count;

            for (ident_count = 0; ident_count < IDENT.Count; ident_count++)
            {
                var ckv = IDENT[ident_count];

                sw.Write("if (");
                if (!onlyIdent)
                {
                    sw.Write(
                    "(token->type == CSS_TOKEN_IDENT) && ");
                }
                sw.WriteLine(
                        $"(lwc_string_caseless_isequal(token->idata, c->strings[{ckv.Key}], &match) == lwc_error_ok && match)) {{");
                if (ckv.Key == "INHERIT")
                {
                    sw.WriteLine(
                        $"\t\t\terror = css_stylesheet_style_inherit(result, {parseId.Value});"
                        );
                }
                else
                {
                    sw.WriteLine(
                        $"\t\t\terror = css__stylesheet_style_appendOPV(result, {parseId.Value}, {ckv.Value});"
                    );
                }
                sw.Write(
                "\t} else ");
            }
        }

        static void OutputNumber(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> kvlist)
        {
            var ckv = kvlist[0];
            int ident_count;


            sw.WriteLine("if (token->type == CSS_TOKEN_NUMBER) {");
            sw.WriteLine("\t\tcss_fixed num = 0;");
            sw.WriteLine("\t\tsize_t consumed = 0;\n");
            sw.WriteLine($"\t\tnum = css__number_from_lwc_string(token->idata, {ckv.Key}, &consumed);");
            sw.WriteLine("\t\t/* Invalid if there are trailing characters */");
            sw.WriteLine("\t\tif (consumed != lwc_string_length(token->idata)) {");
            sw.WriteLine("\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\treturn CSS_INVALID;");
            sw.WriteLine("\t\t}");

            for (ident_count = 1; ident_count < kvlist.Count; ident_count++)
            {
                var ulkv = kvlist[ident_count];

                if (ulkv.Key == "RANGE")
                {
                    sw.WriteLine($"\t\tif ({ulkv.Value}) {{");
                    sw.WriteLine("\t\t\t*ctx = orig_ctx;");
                    sw.WriteLine("\t\t\treturn CSS_INVALID;");
                    sw.WriteLine("\t\t}\n");
                }

            }

            sw.WriteLine($"\t\terror = css__stylesheet_style_appendOPV(result, {parseId.Value}, 0, {ckv.Value});");
            sw.WriteLine("\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\treturn error;");
            sw.WriteLine("\t\t}\n");
            sw.WriteLine("\t\terror = css__stylesheet_style_append(result, num);");
            sw.Write("\t} else ");
        }

        static void OutputUri(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> kvlist)
        {
            sw.WriteLine("if (token->type == CSS_TOKEN_URI) {");
            sw.WriteLine("		lwc_string *uri = NULL;");
            sw.WriteLine("		uint32_t uri_snumber;");
            sw.WriteLine();
            sw.WriteLine("		error = c->sheet->resolve(c->sheet->resolve_pw,");
            sw.WriteLine("				c->sheet->url,");
            sw.WriteLine("				token->idata, &uri);");
            sw.WriteLine("		if (error != CSS_OK) {");
            sw.WriteLine("			*ctx = orig_ctx;");
            sw.WriteLine("			return error;");
            sw.WriteLine("		}");
            sw.WriteLine();
            sw.WriteLine("		error = css__stylesheet_string_add(c->sheet, uri, &uri_snumber);");
            sw.WriteLine("		if (error != CSS_OK) {");
            sw.WriteLine("			*ctx = orig_ctx;");
            sw.WriteLine("			return error;");
            sw.WriteLine("		}");
            sw.WriteLine();
            sw.WriteLine($"		error = css__stylesheet_style_appendOPV(result, {parseId.Value}, 0, {kvlist[0].Value});");
            sw.WriteLine("		if (error != CSS_OK) {");
            sw.WriteLine("			*ctx = orig_ctx;");
            sw.WriteLine("			return error;");
            sw.WriteLine("		}");
            sw.WriteLine();
            sw.WriteLine("		error = css__stylesheet_style_append(result, uri_snumber);");
            sw.Write("	} else ");
        }

        static void OutputColor(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> COLOR)
        {
            sw.WriteLine("{");
            sw.WriteLine("\t\tuint16_t value = 0;");
            sw.WriteLine("\t\tuint32_t color = 0;");
            sw.WriteLine("\t\t*ctx = orig_ctx;\n");
            sw.WriteLine("\t\terror = css__parse_colour_specifier(c, vector, ctx, &value, &color);");
            sw.WriteLine("\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\treturn error;");
            sw.WriteLine("\t\t}\n");
            sw.WriteLine($"\t\terror = css__stylesheet_style_appendOPV(result, {parseId.Value}, 0, value);");
            sw.WriteLine("\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\treturn error;");
            sw.WriteLine("\t\t}");
            sw.WriteLine();
            sw.WriteLine("\t\tif (value == COLOR_SET)");
            sw.WriteLine("\t\t\terror = css__stylesheet_style_append(result, color);");
            sw.WriteLine("\t}\n");
        }

        static void OutputLengthUnit(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> kvlist)
        {
            var ckv = kvlist[0];
            int ident_count;

            sw.WriteLine("{");
            sw.WriteLine("\t\tcss_fixed length = 0;");
            sw.WriteLine("\t\tuint32_t unit = 0;");
            sw.WriteLine("\t\t*ctx = orig_ctx;\n");
            sw.WriteLine($"\t\terror = css__parse_unit_specifier(c, vector, ctx, {ckv.Key}, &length, &unit);");
            sw.WriteLine("\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\treturn error;");
            sw.WriteLine("\t\t}\n");

            for (ident_count = 1; ident_count < kvlist.Count; ident_count++)
            {
                //struct keyval *ulkv = kvlist->item[ident_count];
                var ulkv = kvlist[ident_count];

                if (ulkv.Key == "ALLOW")
                {
                    sw.WriteLine($"\t\tif (({ulkv.Value}) == false) {{");
                    sw.WriteLine("\t\t\t*ctx = orig_ctx;");
                    sw.WriteLine("\t\t\treturn CSS_INVALID;");
                    sw.WriteLine("\t\t}\n");
                }
                else if (ulkv.Key == "DISALLOW")
                {
                    sw.WriteLine($"\t\tif ({ulkv.Value}) {{");
                    sw.WriteLine("\t\t\t*ctx = orig_ctx;");
                    sw.WriteLine("\t\t\treturn CSS_INVALID;");
                    sw.WriteLine("\t\t}\n");
                }
                else if (ulkv.Key == "MASK")
                {
                    sw.WriteLine($"\t\tif ((unit & {ulkv.Value} ) == 0) {{");
                    sw.WriteLine("\t\t\t*ctx = orig_ctx;");
                    sw.WriteLine("\t\t\treturn CSS_INVALID;");
                    sw.WriteLine("\t\t}\n");
                }
                else if (ulkv.Key == "RANGE")
                {
                    sw.WriteLine($"\t\tif (length {ulkv.Value}) {{");
                    sw.WriteLine("\t\t\t*ctx = orig_ctx;");
                    sw.WriteLine("\t\t\treturn CSS_INVALID;");
                    sw.WriteLine("\t\t}\n");
                }
            }

            sw.WriteLine($"\t\terror = css__stylesheet_style_appendOPV(result, {parseId.Value}, 0, {ckv.Value});");
            sw.WriteLine("\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\treturn error;");
            sw.WriteLine("\t\t}");
            sw.WriteLine();
            sw.WriteLine("\t\terror = css__stylesheet_style_vappend(result, 2, length, unit);");
            sw.WriteLine("\t}\n");
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

            sw.WriteLine("{");
            sw.WriteLine($"\t\terror = css__stylesheet_style_appendOPV(result, {parseId.Value}, 0, {ckv.Value});");
            sw.WriteLine("\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\treturn error;");
            sw.WriteLine("\t\t}\n");
            sw.WriteLine("\t\twhile ((token != NULL) && (token->type == CSS_TOKEN_IDENT)) {");
            sw.WriteLine("\t\t\tuint32_t snumber;");
            sw.WriteLine("\t\t\tcss_fixed num;");
            sw.WriteLine("\t\t\tint pctx;\n");
            sw.WriteLine("\t\t\terror = css__stylesheet_string_add(c->sheet, lwc_string_ref(token->idata), &snumber);");
            sw.WriteLine("\t\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\t\treturn error;");
            sw.WriteLine("\t\t\t}\n");
            sw.WriteLine("\t\t\terror = css__stylesheet_style_append(result, snumber);");
            sw.WriteLine("\t\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\t\treturn error;");
            sw.WriteLine("\t\t\t}\n");
            sw.WriteLine("\t\t\tconsumeWhitespace(vector, ctx);\n");
            sw.WriteLine("\t\t\tpctx = *ctx;");
            sw.WriteLine("\t\t\ttoken = parserutils_vector_iterate(vector, ctx);");
            sw.WriteLine("\t\t\tif ((token != NULL) && (token->type == CSS_TOKEN_NUMBER)) {");
            sw.WriteLine("\t\t\t\tsize_t consumed = 0;\n");
            sw.WriteLine("\t\t\t\tnum = css__number_from_lwc_string(token->idata, true, &consumed);");
            sw.WriteLine("\t\t\t\tif (consumed != lwc_string_length(token->idata)) {");
            sw.WriteLine("\t\t\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\t\t\treturn CSS_INVALID;");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t\tconsumeWhitespace(vector, ctx);\n");
            sw.WriteLine("\t\t\t\tpctx = *ctx;");
            sw.WriteLine("\t\t\t\ttoken = parserutils_vector_iterate(vector, ctx);");
            sw.WriteLine("\t\t\t} else {");
            sw.WriteLine($"\t\t\t\tnum = INTTOFIX({ikv.Key});");
            sw.WriteLine("\t\t\t}\n");
            sw.WriteLine("\t\t\terror = css__stylesheet_style_append(result, num);");
            sw.WriteLine("\t\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\t\treturn error;");
            sw.WriteLine("\t\t\t}\n");
            sw.WriteLine("\t\t\tif (token == NULL)");
            sw.WriteLine("\t\t\t\tbreak;\n");
            sw.WriteLine("\t\t\tif (token->type == CSS_TOKEN_IDENT) {");
            sw.WriteLine($"\t\t\t\terror = css__stylesheet_style_append(result, {ckv.Value});");
            sw.WriteLine("\t\t\t\tif (error != CSS_OK) {");
            sw.WriteLine("\t\t\t\t\t*ctx = orig_ctx;");
            sw.WriteLine("\t\t\t\t\treturn error;");
            sw.WriteLine("\t\t\t\t}");
            sw.WriteLine("\t\t\t} else {");
            sw.WriteLine("\t\t\t\t*ctx = pctx; /* rewind one token back */");
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t}\n");
            sw.WriteLine($"\t\terror = css__stylesheet_style_append(result, {ikv.Value});");
            sw.WriteLine("\t}\n");
        }

        static void OutputInvalidcss(StreamWriter sw)
        {
            sw.WriteLine("{\n\t\terror = CSS_INVALID;\n\t}\n");
        }

        static void OutputWrap(StreamWriter sw, KeyValuePair<string, string> parseId, List<KeyValuePair<string, string>> WRAP)
        {
            var ckv = WRAP[0];
            sw.WriteLine($"	return {ckv.Value}(c, vector, ctx, result, {parseId.Value});\n}}");
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
                //output_token_type_check(outputf, do_token_check, &IDENT, &URI, &NUMBER);

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

                // Debug using "color" only
                var name = line.Split(':')[0];
                if (name != "border_top")
                    continue;

                using (var stream = File.Create($"autogenerated/autogenerated_{name}.c"))
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
