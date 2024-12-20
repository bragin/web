using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SkiaSharpOpenGLBenchmark
{
    public enum LogChannel
    {
        Events,
        Layout,
        Html,
        Css
    }

    public static class Log {
        public static void Unimplemented(
            string message = "",
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Console.WriteLine($"{sourceFilePath}:{sourceLineNumber} {memberName}(): UNIMPLEMENTED {message}");
            // See https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=net-6.0
            // System.Diagnostics.Trace.WriteLine("member name: " + memberName);
            // System.Diagnostics.Trace.WriteLine("source file path: " + sourceFilePath);
            // System.Diagnostics.Trace.WriteLine("source line number: " + sourceLineNumber);
        }

        public static void Print(
            LogChannel channel,
			string message = "",
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            // Skip events
            if (channel == LogChannel.Events) return;

            Console.WriteLine($"{channel.ToString()} {sourceFilePath}:{sourceLineNumber} {memberName}(): {message}");
        }
}
}