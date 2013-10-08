using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Web;
using System.Net;
using pt.sapo.gis.trace.appender.SMI;
using System.Text.RegularExpressions;
using pt.sapo.gis.trace.sdb;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using System.ServiceModel.Channels;

namespace pt.sapo.gis.trace.wcf
{
    public class TraceMessageInspector : IDispatchMessageInspector
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Regex actionRegularExpression = new Regex("^https?://[^/]+/(.+)/([^/]+)$");

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
            try
            {
                String action = OperationContext.Current.IncomingMessageHeaders.Action;
                String activityId = null;
                String context = null;
                Trace trace = null;
                Guid traceId = Guid.NewGuid();
                if(OperationContext.Current.RequestContext.RequestMessage.Properties.ContainsKey(HttpRequestMessageProperty.Name))
                {
                    HttpRequestMessageProperty httpProperty = (HttpRequestMessageProperty)OperationContext.Current.RequestContext.RequestMessage.Properties[HttpRequestMessageProperty.Name];
                    if (action == null)
                    {
                        action = httpProperty.Headers.Get("SOAPAction");
                    }
                    activityId = httpProperty.Headers.Get("ESBActivityId");
                    context = httpProperty.Headers.Get("ESBTraceContext");
                }
                if (context != null)
                {
                    try
                    {
                        var traceContext = Newtonsoft.Json.Linq.JObject.Load(new JsonTextReader(new StreamReader(new MemoryStream(Convert.FromBase64String(context)))));
                        Guid.TryParse(traceContext.Value<String>("LinkedTraceId"), out traceId);
                    }
                    catch (Exception e){
                        log.ErrorFormat("Fail to parse 'ESBTraceContext: {0}{1}{2}", e.Message, Environment.NewLine, e.StackTrace);
                    }
                    
                }
                trace = TraceManager.BeginTrace(traceId, context);

                if (activityId != null)
                {
                    trace.Properties.Add(SMILoggerAppender.REPORT_ACTIVITY_ID_PROPERTY, activityId);
                }

                if (action != null)
                {
                    var match = actionRegularExpression.Match(action);
                    if (match.Success)
                    {
                        trace.Properties[SDBTraceProperties.SERVICE_NAME_PROPERTY] = match.Groups[1].Value;
                        trace.Properties[SDBTraceProperties.SERVICE_OPERATION_NAME_PROPERTY] = match.Groups[2].Value;
                    }
                }

                if (HttpContext.Current != null)
                {
                    trace.ServerInfo = IPAddress.Parse(HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]);
                    trace.ClientInfo = IPAddress.Parse(GetClientIP());
                }

            }
            catch (Exception e)
            {
                log.ErrorFormat("Fail on trace initialize: {0}{1}{2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            try
            {
                var trace = TraceManager.Trace;
                if (trace.Properties.ContainsKey(SDBTraceProperties.TRACE_SUCCESS_PROPERTY) == false)
                {
                    trace.Properties[SDBTraceProperties.TRACE_SUCCESS_PROPERTY] = reply.IsFault == false;
                }
                if (trace.Properties.ContainsKey(SDBTraceProperties.TRACE_RESULT_PROPERTY) == false)
                {
                    var buffer = reply.CreateBufferedCopy(Int32.MaxValue);
                    reply = buffer.CreateMessage();
                    var writer = new StringWriter();
                    buffer.CreateMessage().WriteMessage(new XmlTextWriter(writer));
                    trace.Properties[SDBTraceProperties.TRACE_RESULT_PROPERTY] = writer.ToString();
                }
                TraceManager.EndTrace();
            }
            catch (Exception e)
            {
                log.ErrorFormat("Fail on trace end: {0}{1}{2}", e.Message, Environment.NewLine, e.StackTrace);
            }
        }
    }
}
