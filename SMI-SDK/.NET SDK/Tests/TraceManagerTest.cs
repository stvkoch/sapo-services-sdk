using pt.sapo.gis.trace;
using pt.sapo.gis.trace.appender.SMI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Tests
{
    /// <summary>
    ///This is a test class for TraceManagerTest and is intended
    ///to contain all TraceManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TraceManagerTest : BaseTest
    {
        /// <summary>
        ///A test for BeginTrace
        ///</summary>
        [TestMethod()]
        public void TraceConfigurationTest()
        {
            Assert.IsNotNull(TraceManager_Accessor.appenders);
        }

        /// <summary>
        ///A test for BeginTrace
        ///</summary>
        [TestMethod()]
        public void BeginTraceTest()
        {
            var trace = TraceManager.BeginTrace();
            Thread.Sleep(5);
            trace.TraceEntry(new Entry
            {
                Type = "UNKNOWN",
                Description = "Who knowns...",
                Severity = pt.sapo.gis.trace.configuration.severityType.FATAL,
                Properties = new Dictionary<String, Object> { {"a", 1}, {"b", new Dictionary<String,object> { {"3",1},{"2", 5},{"54", 5} }}}
            });
            //Assert.AreEqual(report[""]
        }

        /// <summary>
        ///A test for parallel environment Trace reference copy using TraceEnabledTask
        ///</summary>
        [TestMethod()]
        public void TraceParallelEnvironmentReferenceTest()
        {
            Trace expected = TraceManager.BeginTrace();
            Trace actual = null;
            var task = new TraceEnabledTask(() => actual = TraceManager.Trace);
            task.Start();
            task.Wait();
            TraceManager.EndTrace();
            Assert.AreSame(expected, actual);
        }

        /// <summary>
        ///A test for parallel environment concurrent Entry contexts
        ///</summary>
        [TestMethod()]
        public void TraceParallelEnvironmentContextTest()
        {
            var mainEvent = new AutoResetEvent(false);
            var taskEvent = new AutoResetEvent(false);
            Trace expected = TraceManager.BeginTrace();
            var task = new TraceEnabledTask (() => {
                taskEvent.WaitOne();
                TraceManager.Trace.BeginEntry("task root entry", pt.sapo.gis.trace.configuration.severityType.INFO);
                mainEvent.Set();
                taskEvent.WaitOne();
                TraceManager.Trace.BeginEntry("task child entry", pt.sapo.gis.trace.configuration.severityType.WARN);
                mainEvent.Set();
                taskEvent.WaitOne();
                TraceManager.Trace.EndEntry();
                mainEvent.Set();
                taskEvent.WaitOne();
                TraceManager.Trace.EndEntry();
                mainEvent.Set();                
            });
            task.Start();

            TraceManager.Trace.BeginEntry("main root entry", pt.sapo.gis.trace.configuration.severityType.INFO);
            taskEvent.Set();
            mainEvent.WaitOne();
            TraceManager.Trace.BeginEntry("main child entry", pt.sapo.gis.trace.configuration.severityType.WARN);
            taskEvent.Set();
            mainEvent.WaitOne();
            TraceManager.Trace.EndEntry();
            taskEvent.Set();
            mainEvent.WaitOne();
            TraceManager.Trace.EndEntry();
            taskEvent.Set();
            mainEvent.WaitOne();

            task.Wait();
            TraceManager.EndTrace();            
        }
    }
}
