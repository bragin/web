﻿using System;
using System.Globalization;
using System.Linq;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;

// THis file comes from ExCSS almost unmodified

namespace SkiaSharpOpenGLBenchmark
{
    internal static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string SquashWhitespace(this string s)
        {
            StringBuilder result = new StringBuilder();
            bool inWhitespace = false;

            foreach (char ch in s)
            {
                if (char.IsWhiteSpace(ch))
                {
                    if (!inWhitespace)
                    {
                        // Append a single space if encountering the first whitespace character
                        result.Append(' ');
                        inWhitespace = true;
                    }
                }
                else
                {
                    result.Append(ch);
                    inWhitespace = false;
                }
            }

            return result.ToString();
        }

        public static bool Has(this string value, char chr, int index = 0)
        {
            return value != null && value.Length > index && value[index] == chr;
        }

        public static bool Contains(this string[] list, string element,
            StringComparison comparison = StringComparison.Ordinal)
        {
            return list.Any(t => t.Equals(element, comparison));
        }

        public static bool Is(this string current, string other)
        {
            return string.Equals(current, other, StringComparison.Ordinal);
        }

        public static bool Isi(this string current, string other)
        {
            return string.Equals(current, other, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsOneOf(this string element, string item1, string item2)
        {
            return element.Is(item1) || element.Is(item2);
        }

        // Hash a string, returning a 32bit value.  The hash algorithm used is
        // Fowler Noll Vo -a very fast and simple hash, ideal for short strings.
        // See http://en.wikipedia.org/wiki/Fowler_Noll_Vo_hash for more details.
        // libwapcaplet.c:217

        public static uint HashCaseless(this string str)
        {
            // It looks like only ASCII chars are supported, but it should be good enough
            uint z = 0x811c9dc5;
            var len = str.Length;

            for (int  i = 0; i < len; i++)
            {
                z *= 0x01000193;
                z ^= (byte)Char.ToLower(str[i]);
            }

            return z;
        }


        /*
        public static string StylesheetString(this string value)
        {
            var builder = Pool.NewStringBuilder();
            builder.Append(Symbols.DoubleQuote);

            if (!string.IsNullOrEmpty(value))
                for (var i = 0; i < value.Length; i++)
                {
                    var character = value[i];

                    switch (character)
                    {
                        case Symbols.Null:
                            throw new ParseException("Unable to parse null symbol");
                        case Symbols.DoubleQuote:
                        case Symbols.ReverseSolidus:
                            builder.Append(Symbols.ReverseSolidus).Append(character);
                            break;
                        default:
                            if (character.IsInRange(Symbols.StartOfHeading, Symbols.UnitSeparator)
                                || character == Symbols.CurlyBracketOpen)
                            {
                                builder.Append(Symbols.ReverseSolidus)
                                    .Append(character.ToHex())
                                    .Append(i + 1 != value.Length ? " " : "");
                            }
                            else
                            {
                                builder.Append(character);
                            }

                            break;
                    }
                }

            builder.Append(Symbols.DoubleQuote);
            return builder.ToPool();
        }*/

        public static string StylesheetFunction(this string value, string argument)
        {
            return string.Concat(value, "(", argument, ")");
        }
        /*
        public static string StylesheetUrl(this string value)
        {
            var argument = value.StylesheetString();
            return FunctionNames.Url.StylesheetFunction(argument);
        }*/

        public static string StylesheetUnit(this string value, out float result)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var firstLetter = value.Length;

                while (!value[firstLetter - 1].IsDigit() && --firstLetter > 0)
                {
                    // Intentional empty.
                }

                var parsed = float.TryParse(value.Substring(0, firstLetter), NumberStyles.Any,
                    CultureInfo.InvariantCulture, out result);

                if (firstLetter > 0 && parsed) return value.Substring(firstLetter);
            }

            result = default;
            return null;
        }
    }
}