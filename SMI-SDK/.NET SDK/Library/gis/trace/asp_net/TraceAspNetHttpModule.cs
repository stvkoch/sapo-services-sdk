using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using pt.sapo.gis.trace.appender.SMI;
using System.Net;

namespace pt.sapo.gis.trace.asp_net
{
    public class TraceAspNetHttpModule : IHttpModule
    {

        private static String GetClientIP()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (source, args) =>
            {
                String actityId = HttpContext.Current.Request.Headers["ESBActivityId"];
                String contextId = HttpContext.Current.Request.Headers["ESBTraceContext"];
                var trace = TraceManager.BeginTrace(contextId);
                trace.Properties.Add(SMILoggerAppender.REPORT_ACTIVITY_ID_PROPERTY, actityId);
                trace.ServerInfo = IPAddress.Parse(HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]);
                trace.ClientInfo = IPAddress.Parse(GetClientIP());
            };
            context.Error += (source, args) =>
            {
                var error = HttpContext.Current.Server.GetLastError();
                var entry = new Entry
                {
                    Type = "UNKNOWN",
                    Severity = configuration.severityType.ERROR,
                    Description = String.Format("Applicarion Error: {0}", error.Message)
                };
                var parentEntry = entry;
                while (error.InnerException != null)
                {
                    error = error.InnerException;
                    var innerEntry = new Entry
                    {
                        Type = "UNKNOWN",
                        Severity = configuration.severityType.ERROR,
                        Description = error.Message
                    };
                    parentEntry.Entries.Add(innerEntry);
                    parentEntry = innerEntry;
                }
                TraceManager.Trace.TraceEntry(entry);
            };                
                
            context.EndRequest += (source, args) => TraceManager.EndTrace();
        }

        public void Dispose()
        {
        }
    }
}
