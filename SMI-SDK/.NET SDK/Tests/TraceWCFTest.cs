using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using System.ServiceModel.Description;
using pt.sapo.gis.trace;
using pt.sapo.gis.trace.wcf;
using pt.sapo.gis.trace.sdb;
using System.Xml.Linq;
using System.ServiceModel.Channels;
using pt.sapo.gis.trace.appender.SMI;

namespace Tests
{
    [ServiceContract]
    public interface IHelloWorldService
    {
        [OperationContract]
        string SayHello(string name);

        [OperationContract]
        string SayFaultHello(string name);
    }

    public class HelloWorldService : IHelloWorldService
    {
        public string SayHello(string name)
        {
            TraceManager.Trace.TraceEntry(new Entry("Saied hello to " + name, pt.sapo.gis.trace.configuration.severityType.INFO));
            return string.Format("Hello, {0}", name);
        }

        public string SayFaultHello(string name)
        {
            throw new FaultException(string.Format("Hello, {0}", name));
        }
    }

    /// <summary>
    /// Summary description for TraceWCFTest
    /// </summary>
    [TestClass]
    public class TraceWCFTest : BaseTest
    {
    
        [TestMethod]
        public void TestSayHello()
        {
            String name = "Anonymous";
            Trace actual = null;
            TraceManager.OnTrace += t => actual = t;
            String resultText = null;
            var traceId = Guid.NewGuid();
            String esbContext = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{{\"LinkedTraceId\":\"{0}\",\"OwnerTraceId\":\"e4b9e2cd-5090-4761-ae97-7a83298897bc\"}}", traceId)));
            var factory = new ChannelFactory<IHelloWorldService>("*");            
            
            using (ServiceHost host = new ServiceHost(typeof(HelloWorldService)))
            {
                host.Open();

                var channel = factory.CreateChannel();
                try
                {
                    using (var context = new OperationContextScope((IContextChannel)channel))
                    {
                        var httpRequestProperty = new HttpRequestMessageProperty();
                        httpRequestProperty.Headers.Add("ESBTraceContext", esbContext);
                        httpRequestProperty.Headers.Add("ESBActivityId", "ActivityId");
                        OperationContext.Current.OutgoingMessageProperties.Add(HttpRequestMessageProperty.Name, httpRequestProperty);
                        resultText = channel.SayHello(name);
                        Assert.IsNotNull(resultText);
                        Assert.AreEqual("Hello, Anonymous", resultText);
                    }
                }
                catch {
                    ((IDisposable)channel).Dispose();
                }

                host.Close();
            }
            Assert.IsNotNull(actual);

            Assert.AreEqual(traceId, actual.Id);
            
            Assert.IsTrue(actual.Properties.ContainsKey(SMILoggerAppender.REPORT_ACTIVITY_ID_PROPERTY));
            Assert.AreEqual("ActivityId",  actual.Properties[SMILoggerAppender.REPORT_ACTIVITY_ID_PROPERTY]);

            Assert.IsTrue(actual.Properties.ContainsKey(SDBTraceProperties.SERVICE_NAME_PROPERTY));
            Assert.AreEqual(typeof(IHelloWorldService).Name, actual.Properties[SDBTraceProperties.SERVICE_NAME_PROPERTY]);

            Assert.IsTrue(actual.Properties.ContainsKey(SDBTraceProperties.SERVICE_OPERATION_NAME_PROPERTY));
            Assert.AreEqual("SayHello", actual.Properties[SDBTraceProperties.SERVICE_OPERATION_NAME_PROPERTY]);

            Assert.IsNotNull(actual.Entries);
            Assert.AreEqual(1, actual.Entries.Count);

            Assert.IsTrue(actual.Properties.ContainsKey(SDBTraceProperties.TRACE_SUCCESS_PROPERTY));
            Assert.IsTrue((bool)actual.Properties[SDBTraceProperties.TRACE_SUCCESS_PROPERTY]);
            
            Assert.IsTrue(actual.Properties.ContainsKey(SDBTraceProperties.TRACE_RESULT_PROPERTY));
            var envelope = XElement.Parse((String)actual.Properties[SDBTraceProperties.TRACE_RESULT_PROPERTY]);
            Assert.IsNotNull(envelope);
            var body = envelope.Element("{http://schemas.xmlsoap.org/soap/envelope/}Body");
            Assert.IsNotNull(body);
            var response = body.Element("{http://tempuri.org/}SayHelloResponse");
            Assert.IsNotNull(response);
            var result = response.Element("{http://tempuri.org/}SayHelloResult");
            Assert.IsNotNull(result);
            Assert.AreEqual("Hello, Anonymous", result.Value);
        }

        [TestMethod]
        public void TestSayFaultHello()
        {
            String name = "Anonymous";
            Trace actual = null;
            TraceManager.OnTrace += t => actual = t;

            Uri baseAddress = new Uri("http://localhost:12345/hello");

            var factory = new ChannelFactory<IHelloWorldService>(new BasicHttpBinding(), new EndpointAddress(baseAddress));


            using (ServiceHost host = new ServiceHost(typeof(HelloWorldService), baseAddress))
            {
                host.AddServiceEndpoint(typeof(IHelloWorldService), new BasicHttpBinding(), baseAddress);
                foreach (var endpoint in host.Description.Endpoints)
                {
                    endpoint.Behaviors.Add(new TraceEndpointBehavior());
                }
                host.Open();

                var channel = factory.CreateChannel();
                try
                {
                    channel.SayFaultHello(name);
                }
                catch
                {
                    ((IDisposable)channel).Dispose();
                }

                host.Close();
            }
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Entries);
            Assert.AreEqual(0, actual.Entries.Count);
            Assert.IsTrue(actual.Properties.ContainsKey(SDBTraceProperties.TRACE_SUCCESS_PROPERTY));
            Assert.IsFalse((bool)actual.Properties[SDBTraceProperties.TRACE_SUCCESS_PROPERTY]);
            Assert.IsTrue(actual.Properties.ContainsKey(SDBTraceProperties.TRACE_RESULT_PROPERTY));

            var envelope = XElement.Parse((String)actual.Properties[SDBTraceProperties.TRACE_RESULT_PROPERTY]);
            Assert.IsNotNull(envelope);
            var body = envelope.Element("{http://schemas.xmlsoap.org/soap/envelope/}Body");
            Assert.IsNotNull(body);
            var fault = body.Element("{http://schemas.xmlsoap.org/soap/envelope/}Fault");
            Assert.IsNotNull(fault);
            var faultstring = fault.Element("faultstring");
            Assert.IsNotNull(faultstring);
            Assert.AreEqual("Hello, Anonymous", faultstring.Value);
        }
    }
}
