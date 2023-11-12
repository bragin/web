using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DebugTools {
    public static class Log {
        public static void Unimplemented(
            string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            System.Diagnostics.Trace.WriteLine($"{sourceFilePath}: {sourceLineNumber}: {memberName}: Unimplemented {message}");
            // See https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=net-6.0
            // System.Diagnostics.Trace.WriteLine("member name: " + memberName);
            // System.Diagnostics.Trace.WriteLine("source file path: " + sourceFilePath);
            // System.Diagnostics.Trace.WriteLine("source line number: " + sourceLineNumber);
        }
    }
}