// See https://aka.ms/new-console-template for more information
using SkiaSharpOpenGLBenchmark;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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