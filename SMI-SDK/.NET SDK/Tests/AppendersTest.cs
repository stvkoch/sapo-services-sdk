using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pt.sapo.gis.trace;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using pt.sapo.gis.trace.appender.SMI;
using System.Net;
using System.Configuration;
using pt.sapo.gis.trace.configuration;
using System.Threading.Tasks;
using System.Xml.Serialization;
using pt.sapo.sdb.smi;
using System.Xml;
using Newtonsoft.Json;
using pt.sapo.sdb.trace;

namespace Tests
{
    /// <summary>
    /// Summary description for AppendersTest
    /// </summary>
    [TestClass]
    public class AppendersTest : BaseTest
    {
        [TestMethod]
        public void BrokerSMIAppenderTest()
        {
            var trace = TraceManager.BeginTrace();
            var brokerAppender = TraceManager_Accessor.appenders.Where(a => a is pt.sapo.gis.trace.appender.SMI.BrokerSMILoggerAppender).FirstOrDefault() as pt.sapo.gis.trace.appender.SMI.BrokerSMILoggerAppender;

            var broker = new SapoBrokerClient.BrokerClient(new SapoBrokerClient.HostInfo(brokerAppender.Properties.Host, brokerAppender.Properties.Port));
            var subscription = new SapoBrokerClient.Subscription(brokerAppender.Properties.Topic, SapoBrokerClient.NetAction.DestinationType.TOPIC);
            JObject report = null;
            var ev = new ManualResetEvent(false);
            subscription.OnMessage += m =>
            {
                var str = new StringWriter();
                var jsonSer = new Newtonsoft.Json.JsonSerializer();
                report = JObject.Parse(Encoding.UTF8.GetString(m.Message.Payload));
                ev.Set();
            };
            broker.Subscribe(subscription);
            var e = new pt.sapo.gis.trace.Entry
            {
                Type = "UNKNOWN",
                Description = "Who knowns...",
                Severity = pt.sapo.gis.trace.configuration.severityType.FATAL,
                Properties = new Dictionary<String, Object> { { "a", "1" }, { "b", "foo" } }
            };
            trace.TraceEntry(e);
            if (ev.WaitOne(TimeSpan.FromSeconds(10)) == false)
                Assert.Fail("Broker message doesn't arrive.");
            Assert.IsNotNull(report["SourceID"]);
            var defaultSourceId = TraceManager_Accessor.appenders.Where(a => a is BrokerSMILoggerAppender).Cast<BrokerSMILoggerAppender>().First().Properties.DefaultSourceId;
            Assert.AreEqual(defaultSourceId, report["SourceID"]);
            Assert.IsNotNull(report["DateTime"]);
            var state = report["State"];
            Assert.IsNotNull(state);
            Assert.IsNotNull(state["Health"]);
            Assert.AreEqual(pt.sapo.sdb.smi.HealthState.OperationalWithFailures.ToString(), state["Health"]);
            Assert.IsNotNull(state["Execution"]);
            Assert.AreEqual(pt.sapo.sdb.smi.ExecutionState.Active.ToString(), state["Execution"]);
            Assert.IsNotNull(state["Failures"]);
            Assert.AreEqual(1, state["Failures"].Count());
            var failure = state["Failures"].First();
            Assert.IsNotNull(failure["FailureID"]);
            Assert.AreEqual(e.Type.ToString(), failure["FailureID"]);
            Assert.AreEqual(defaultSourceId, failure["SourceID"]);
            var details = failure["Details"];
            Assert.IsNotNull(details);
            Assert.IsNotNull(details["a"]);
            Assert.AreEqual(JTokenType.String, details["a"].Type);
            Assert.AreEqual("1", details["a"]);
            Assert.IsNotNull(details["b"]);
            Assert.AreEqual(JTokenType.String, details["b"].Type);
            Assert.AreEqual("foo", details["b"]);
            Assert.IsNotNull(report["Metrics"]);
            Assert.IsNotNull(report["ActivityID"]);
            Assert.IsNotNull(report["AgentID"]);
        }

        [TestMethod]
        public void SDBSMIAppenderTest()
        {
            SDBTraceAppender sdbAppender = null;
            sdbAppender = TraceManager_Accessor.appenders.Where(a => a is SDBTraceAppender).FirstOrDefault() as SDBTraceAppender;
            Assert.IsNotNull(sdbAppender);
            Occurrence occurrence = null;
            var server = new HttpListener();
            server.Prefixes.Add(String.Format("{0}://{1}{2}/{3}/", sdbAppender.Properties.Protocol, sdbAppender.Properties.Host, sdbAppender.Properties.Port.HasValue ? ":" + sdbAppender.Properties.Port.Value : String.Empty, sdbAppender.Properties.Path));
            server.Start();
            var t = Task.Factory.StartNew(() => {
                var context = server.GetContext();
                Assert.AreEqual(String.Format("ESB Token=\"{0}\"", sdbAppender.Properties.Token), context.Request.Headers["Authorization"]);
                var ser = new XmlSerializer(typeof(ManagementReport));
                String json = null;
                using (var requestStream = context.Request.InputStream) {
                    json = new StreamReader(requestStream).ReadToEnd();
                    
                }
                occurrence = new JsonSerializer().Deserialize<Occurrence>(new JsonTextReader(new StringReader(json)));
                context.Response.Close();
            });
            var traceId = Guid.NewGuid();
            String contextId = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{{\"LinkedTraceId\":\"{0}\",\"OwnerTraceId\":\"e4b9e2cd-5090-4761-ae97-7a83298897bc\"}}", traceId)));
            TraceManager.BeginTrace(traceId, contextId);
            TraceManager.Trace.TraceEntry(new pt.sapo.gis.trace.ExceptionEntry ("FAILURE1", severityType.ERROR, new ApplicationException("Wazaaaaaaa!")));
            TraceManager.EndTrace();
            if (t.Wait(TimeSpan.FromSeconds(10)) == false)
                Assert.Fail("Timedout while wating for SDB trace request.");
            server.Stop();
            Assert.IsNotNull(occurrence);
            Assert.IsNotNull(occurrence.Entries);
            Assert.AreEqual(1, occurrence.Entries.Count);
            Assert.AreEqual(Severity.Error, occurrence.Entries.First().Severity);
        }

        // TODO: Setup valid credentials
        //[TestMethod]
        public void RealSDBSMIAppenderTest()
        {
            var id = Guid.NewGuid();
            String contextId = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{{\"LinkedTraceId\":\"{0}\",\"OwnerTraceId\":\"e4b9e2cd-5090-4761-ae97-7a83298897bc\"}}", id)));
            TraceManager.BeginTrace(id, contextId);
            TraceManager.Trace.TraceEntry(new pt.sapo.gis.trace.Entry
            {   
                Severity = severityType.ERROR,
                Type = "UNKNOWN",
                Description = "FAILURE1"
            });            
            TraceManager.EndTrace();
        }

        [TestMethod]
        public void FileAppenderTest() {            
            string filename = string.Format("Test_{0}.log", DateTime.Now.ToString("yyyyMMdd"));
            
            TraceManager.BeginTrace();
            TraceManager.Trace.TraceEntry(new pt.sapo.gis.trace.Entry("Teste 1", severityType.INFO));
            TraceManager.Trace.BeginEntry(new pt.sapo.gis.trace.Entry("Teste 2", severityType.WARN));
            TraceManager.Trace.BeginEntry(new pt.sapo.gis.trace.Entry("Teste 2.1", severityType.WARN));
            TraceManager.Trace.TraceEntry(new pt.sapo.gis.trace.Entry("Teste 2.1.1", severityType.INFO));
            TraceManager.Trace.EndEntry();
            TraceManager.Trace.EndEntry();
            TraceManager.EndTrace();

            Assert.IsTrue(File.Exists(filename));
        }

        [TestMethod]
        public void TimeoutFileLogAppenderTest() {            
            var date = DateTime.Now.ToString("yyyyMMdd");
            string filenameST = string.Format("ShortTimeout_{0}.log", date);
            string filenameLT = string.Format("LongTimeout_{0}.log", date);
            string filenameNT = string.Format("NoTimeout_{0}.log", date);

            TraceManager.BeginTrace();
            TraceManager.Trace.TraceEntry(new pt.sapo.gis.trace.Entry("Timeout", severityType.INFO));
            Thread.Sleep(5);
            TraceManager.EndTrace();

            Assert.IsTrue(File.Exists(filenameST));
            Assert.IsFalse(File.Exists(filenameLT));
            Assert.IsTrue(File.Exists(filenameNT));
        }
    }
}

