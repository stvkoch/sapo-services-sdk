using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Web;
using System.Net;
using pt.sapo.gis.trace.appender.SMI;

namespace pt.sapo.gis.trace.wcf
{
    public class TraceMessageInspector : IDispatchMessageInspector
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

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {                
            
            String activityId = null;
            String contextId = null;
            if (HttpContext.Current != null)
            {
                activityId = HttpContext.Current.Request.Headers["ESBActivityId"];
                contextId = HttpContext.Current.Request.Headers["ESBTraceContext"];
            }
            var trace = TraceManager.BeginTrace(contextId);
            trace.Properties.Add(SMILoggerAppender.REPORT_ACTIVITY_ID_PROPERTY, activityId);
            if (HttpContext.Current != null)
            {
                trace.ServerInfo = IPAddress.Parse(HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]);
                trace.ClientInfo = IPAddress.Parse(GetClientIP());
            }
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            TraceManager.EndTrace();
        }
    }
}
