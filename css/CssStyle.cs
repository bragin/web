using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaSharpOpenGLBenchmark.css
{
    // stylesheet.h:29
    public class CssStyle
    {
        OpCode[] Bytecode; // Pointer to bytecode
        int Used; // number of code entries used
        int Allocated; // number of allocated code entries
        CssStylesheet Sheet; // Reference to containing sheet

        /* Note, CSS_STYLE_DEFAULT_SIZE must be a power of 2 */
        /* With a test set of NetSurf's homepage, BBC news, wikipedia, CNN, Ars, Google and El-Reg,
         * 16 seems to be a good medium between wastage and reallocs.
         */
        public static int CSS_STYLE_DEFAULT_SIZE = 16;


        // stylesheet.c:631
        public CssStyle(CssStylesheet sheet)
        {
            Sheet = sheet;

            // FIXME: Sheet CachedStyle is not supported

            Bytecode = new OpCode[CSS_STYLE_DEFAULT_SIZE];
            Allocated = CSS_STYLE_DEFAULT_SIZE;
            Used = 0;
        }

        // stylesheet.c:723
        public void AppendStyle(OpCode opcode)
        {
            if (Allocated == Used)
            {
                // space not available to append, extend allocation
                Array.Resize(ref Bytecode, Bytecode.Length * 2);
                Allocated = Bytecode.Length;
            }

            Bytecode[Used] = opcode;
            Used++;
        }

        // stylesheet.c:671
        public void MergeStyle(CssStyle style)
        {
            int NewcodeLen = Used + style.Used;

            if (NewcodeLen > Allocated)
            {
                NewcodeLen += CSS_STYLE_DEFAULT_SIZE - 1;
                NewcodeLen &= ~(CSS_STYLE_DEFAULT_SIZE - 1);
                Array.Resize(ref Bytecode, NewcodeLen);
                Allocated = NewcodeLen;
            }

            Array.Copy(style.Bytecode, 0, Bytecode, Used, style.Used);
            Used += style.Used;
        }

    }
}
