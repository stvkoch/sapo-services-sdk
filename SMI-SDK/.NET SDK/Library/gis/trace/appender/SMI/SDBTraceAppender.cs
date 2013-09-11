namespace pt.sapo.gis.trace.appender.SMI
{
    using System;
    using System.Linq;
    using System.Net;
    
    using System.Collections.Generic;
    using pt.sapo.sdb.trace;
    using pt.sapo.gis.exception;
    using System.Text;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using Newtonsoft.Json;

    /**
     *@author GIS Team
     * Appender to send traces to SAPO SDB trace service

     */
    public class SDBTraceAppender : Appender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public new SDBTraceAppenderConfigProperties Properties { get; protected set; }
        /// <summary>
        /// 
        /// </summary>    
        /// <param name="config"></param>    
        /// <exception cref="IntializatonException">When failing to initialize.</exception>
        public SDBTraceAppender(pt.sapo.gis.trace.configuration.appender config)
            : base(config)
        {
            this.Properties = new SDBTraceAppenderConfigProperties(base.Properties);
            base.Properties = Properties;
        }

        private Occurrence ToSDBTrace(pt.sapo.gis.trace.Trace t)
        {
            var occurrence = new Occurrence {
                //Id = t.Id,
                Start = t.Start,
                End = t.End
            };

            if (t.ServerInfo != null)
            {
                var host = Dns.GetHostEntry(t.ServerInfo);
                occurrence.Server = new Server
                {
                    Name = host.HostName,
                    Address = t.ServerInfo.ToString(),
                };
            }

            if (t.ClientInfo != null)
            {
                occurrence.Client = new Client {
                    Address = t.ClientInfo.ToString()
                };
            }

            Entry entry;
            occurrence.Entries = new List<Entry>();
            foreach (pt.sapo.gis.trace.Entry e in t.Entries)
            {
                entry = ToSDBEntry(e, occurrence.Messages);
                occurrence.Entries.Add(entry);
            }
            return occurrence;
        }

        private static String[] SPECIAL_PROPERTIES = new String[] { MessageEntry.MESSAGE_PROPERTY_NAME, ExceptionEntry.EXCEPTION_PROPERTY, LinkedEntry.LINKED_TRACE_ID_PROPERTY };
        private Entry ToSDBEntry(pt.sapo.gis.trace.Entry e, IList<Message> messages)
        {
            var entry = new Entry();
            entry.Description = e.Description;
            entry.Severity = (Severity)Enum.Parse(typeof(Severity), e.Severity.ToString(), true);
            entry.Offset = e.Offset;
            entry.Duration = e.Duration;
            entry.Properties = e.Properties.Where(p => SPECIAL_PROPERTIES.Contains(p.Key) == false).ToNameValueCollection();

            if (e.Properties.ContainsKey(MessageEntry.MESSAGE_PROPERTY_NAME))
            {
                MessageEntryProperty m = (MessageEntryProperty)e.Properties[MessageEntry.MESSAGE_PROPERTY_NAME];
                Message message = ToSDBMessage(m);
                messages.Add(message);
                entry.MessageNumber = messages.IndexOf(message);                
            }

            if (e.Properties.ContainsKey(ExceptionEntry.EXCEPTION_PROPERTY))
            {
                System.Exception ex = (System.Exception)e.Properties[ExceptionEntry.EXCEPTION_PROPERTY];
                entry.Exception = new sdb.trace.Exception { Type = ex.GetType().FullName, StackTrace = ex.StackTrace };
            }

            if (e.Properties.ContainsKey(LinkedEntry.LINKED_TRACE_ID_PROPERTY))
            {
                String link = (String)e.Properties[LinkedEntry.LINKED_TRACE_ID_PROPERTY];
                entry.LinkedTraceId = link;
            }

            if (entry.Properties.Count == 0)
                entry.Properties = null;

            Entry innerEntry;
            entry.Entries = new List<Entry>();
            foreach (pt.sapo.gis.trace.Entry ie in e.Entries)
            {
                innerEntry = ToSDBEntry(ie, messages);
                entry.Entries.Add(innerEntry);
            }
            return entry;
        }

        private Message ToSDBMessage(MessageEntryProperty m)
        {
            var message = new Message();
            var httpMessage = m as HTTPMessageEntryProperty;

            if (httpMessage != null)
            {
                // fill headers
                message.Headers = new NameValueCollection();
                foreach (var header in httpMessage.Headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }

                // fill body
                // TODO: validate string encoding.
                message.Body = Encoding.UTF8.GetBytes(httpMessage.Body);
            }

            // fill request
            var httpRequestMessage = httpMessage as HTTPRequestMessageEntryProperty;
            if (httpRequestMessage != null)
            {
                message.Request = new Request
                {
                    Secure = httpRequestMessage.Url.Scheme == "https",
                    Verb = httpRequestMessage.Verb,
                    PathAndQuery = httpRequestMessage.Url.PathAndQuery
                };
            }

            // fill response
            var httpResponseMessage = httpMessage as HTTPResponseMessageEntryProperty;
            if (httpResponseMessage != null)
            {
                message.Response = new Response
                {
                    StatusCode = httpResponseMessage.StatusCode,
                    ReasonPhrase = httpResponseMessage.ReasonPhrase
                };
            }

            return message;
        }

        public override void OnEntry(pt.sapo.gis.trace.Entry e) { }

        public override void OnTrace(pt.sapo.gis.trace.Trace t)
        {
            if (t.ContextId != null)
            {
                var request = (HttpWebRequest)WebRequest.Create(String.Format("{0}://{1}{2}/{3}?traceContext={4}", Properties.Protocol, Properties.Host, Properties.Port.HasValue ? ":" + Properties.Port.Value : String.Empty, Properties.Path.TrimStart('/'), t.ContextId));
                request.Method = "POST";
                request.Headers.Add("Authorization", String.Format("ESB Token=\"{0}\"", Properties.Token));
                request.ContentType = "application/json";
                using (var str = new StreamWriter(request.GetRequestStream()))
                {
                    new Newtonsoft.Json.JsonSerializer()
                        .Serialize(str, ToSDBTrace(t));                    
                }
                try
                {
                    using (var resposnse = request.GetResponse())
                    {
                        Console.WriteLine(" ============= SDBTraceAppender ==============");
                        var traceContext = Newtonsoft.Json.Linq.JObject.Load(new JsonTextReader(new StreamReader(new MemoryStream(Convert.FromBase64String(t.ContextId)))));
                        Console.WriteLine("https://backoffice.services.bk.sapo.pt/Trace/{0}", traceContext["LinkedTraceId"]);
                    }
                }
                catch (WebException e)
                {
                    var response = (HttpWebResponse)e.Response;
                    using(var reader = new StreamReader(response.GetResponseStream())) {
                        log.ErrorFormat("SDB trace publish fail: [{0}-{1}] {2}", (int)response.StatusCode, response.StatusDescription, reader.ReadToEnd());
                    }
                }
            }            
        }
    }
}
