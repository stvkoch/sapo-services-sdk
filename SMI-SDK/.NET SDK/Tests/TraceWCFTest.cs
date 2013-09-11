using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using System.ServiceModel.Description;
using pt.sapo.gis.trace;
using pt.sapo.gis.trace.wcf;

namespace Tests
{
    /// <summary>
    /// Summary description for TraceWCFTest
    /// </summary>
    [TestClass]
    public class TraceWCFTest : BaseTest
    {
        [ServiceContract]
        public interface IHelloWorldService
        {
            [OperationContract]
            string SayHello(string name);
        }

        public class HelloWorldService : IHelloWorldService
        {
            public string SayHello(string name)
            {
                TraceManager.Trace.TraceEntry(new Entry("Saied hello to " + name, pt.sapo.gis.trace.configuration.severityType.INFO));
                return string.Format("Hello, {0}", name);
            }
        }

        [TestMethod]
        public void TestMethod1()
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
                    channel.SayHello(name);
                }
                catch {
                    ((IDisposable)channel).Dispose();
                }

                host.Close();
            }
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Entries);
            Assert.AreEqual(1, actual.Entries.Count);
            Assert.AreEqual("Saied hello to " + name, actual.Entries[0].Description);
            Assert.AreEqual(pt.sapo.gis.trace.configuration.severityType.INFO, actual.Entries[0].Severity);
        }
    }
}
