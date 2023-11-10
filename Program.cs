﻿// See https://aka.ms/new-console-template for more information
using SkiaSharpOpenGLBenchmark;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Runtime.CompilerServices;

/*

Overview:
    CsQuery is used to parse HTML content (TODO: Convert to HtmlParser use only)
    UNUSED: ExCSS, AngleSharp code just for reference as CSS parsers
    HtmlContent.LoadDocument() - main invocation of HTML layout and rendered are there
    HtmlContent.GetDimensions() - hardcoded window size

    BoxTree - lots of unimplemented unmarked stuff

    CSS call chain:
    class BoxTree
    \/  BoxGetStyle() - stub
    \/  nscssGetStyle() - stub
    \/  CssSelectStyle() - currently implementing
 

    Example of function pointers array: CssParser.ParseFunc

    Now working on: class CssProps, implementing initial properties dispatcher for various needed props

Useful links:
    All possible array init syntices:
        https://stackoverflow.com/questions/5678216/all-possible-array-initialization-syntaxes
 */

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Console.WriteLine("Hello, World!");

        var c = new HtmlContent();
        c.LoadDocument();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetCurrentMethod()
    {
        var st = new StackTrace();
        var sf = st.GetFrame(1);

        return sf.GetMethod().Name;
    }
}