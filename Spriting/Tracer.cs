using System;
using System.Diagnostics;
using System.Threading;
using System.Web;

namespace Spriting
{
    public static class Tracer
    {
        [Conditional("DEBUG")]
        public static void Trace(string messageFormat, params object[] args)
        {
            if (GetCurrentTrustLevel() == AspNetHostingPermissionLevel.Unrestricted)
                LogImplementation(messageFormat, args);
        }

        private static void LogImplementation(string messageFormat, params object[] args)
        {
            if (args.Length == 0) messageFormat = messageFormat.Replace("{", "{{").Replace("}", "}}");
            if (System.Diagnostics.Trace.Listeners.Count <= 0) return;
            var msg = string.Format(messageFormat, args);
            System.Diagnostics.Trace.TraceInformation(string.Format("TIME--{0}::THREAD--{1}/{2}::MSG--{3}",
                                                                    DateTime.Now.TimeOfDay,
                                                                    Thread.CurrentThread.ManagedThreadId, Process.GetCurrentProcess().Id, msg));
        }

        // Based on 
        // http://blogs.msdn.com/b/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx
        static AspNetHostingPermissionLevel GetCurrentTrustLevel()
        {
            foreach (var trustLevel in
                    new[] {
                    AspNetHostingPermissionLevel.Unrestricted,
                    AspNetHostingPermissionLevel.High,
                    AspNetHostingPermissionLevel.Medium,
                    AspNetHostingPermissionLevel.Low,
                    AspNetHostingPermissionLevel.Minimal 
                })
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                }
                catch (Exception)
                {
                    continue;
                }

                return trustLevel;
            }

            return AspNetHostingPermissionLevel.None;
        }
    }
}
