using pt.sapo.gis.trace;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using pt.sapo.gis.trace.configuration;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Tests
{

    static class TestUtils {

        public static bool IsAfter(this DateTime dt1, DateTime dt2)
        {
            return dt1.Subtract(dt2).TotalMilliseconds > 0;
        }
    }

    
    /// <summary>
    ///This is a test class for TraceTest and is intended
    ///to contain all TraceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TraceTest : BaseTest
    {
        /// <summary>
        ///A test for Trace Constructor
        ///</summary>
        [TestMethod()]
        public void TraceConstructorTest()
        {
            Guid id = Guid.NewGuid();
            string contextId = "ContextID";
            Trace target = new Trace(id, contextId);
            Assert.AreEqual(id, target.Id);
            Assert.AreEqual(contextId, target.ContextId);            
        }
                

        /// <summary>
        ///A test for BeginEntry
        ///</summary>
        [TestMethod()]
        public void BeginEntryTest()
        {
            var target = new Trace_Accessor();
            string description = "Entry description.";
            severityType severity = severityType.INFO;
            IDictionary<string, object> properties = new Dictionary<String, Object>() { { "prop1", "asd" } };
            Entry expected = new Entry(description, severity);
            Entry actual = target.BeginEntry(description, severity, properties);
            // Align offsets witch isn't predictable
            expected.Offset = actual.Offset;
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(target.Context.Count == 1);
            Assert.AreEqual(expected, target.Context.Peek());
        }

       
        /// <summary>
        ///A test for EndEntry
        ///</summary>
        [TestMethod()]
        public void EndEntryTest()
        {
            var target = new Trace_Accessor();
            string description = "Entry description.";
            severityType severity = severityType.INFO;
            IDictionary<string, object> properties = new Dictionary<String, Object>() { { "prop1", "asd" } };
            Entry expected = new Entry(description, severity);
            Entry actual = target.BeginEntry(description, severity, properties);
            Thread.Sleep(10);
            target.EndEntry();
            Assert.IsTrue(actual.Offset >= 0);
            Assert.IsTrue(actual.Duration > 0);
        }

        /// <summary>
        ///A test for Finish
        ///</summary>
        [TestMethod()]
        public void FinishTest()
        {
            var testStart = DateTime.UtcNow;
            Thread.Sleep(1);
            var target = new Trace_Accessor();
            Assert.IsTrue(target.Start.IsAfter(testStart));
            Thread.Sleep(10);
            target.Finish();
            Assert.IsTrue(target.End.IsAfter(target.Start));
        }

        /// <summary>
        ///A test for GetSeverityTypes
        ///</summary>
        [TestMethod()]
        public void GetSeverityTypesTest()
        {
            Trace target = new Trace();
            target.TraceEntry(new Entry("", severityType.DEBUG));
            target.BeginEntry("", severityType.DEBUG);
            target.TraceEntry(new Entry("", severityType.INFO));
            target.EndEntry();
            ISet<severityType> expected = new HashSet<severityType> { severityType.INFO, severityType.DEBUG };
            ISet<severityType> actual;
            actual = target.GetSeverityTypes();
            Assert.AreEqual(expected.Intersect(actual).Count(), expected.Count);
        }

        /// <summary>
        ///A test for TraceEntry
        ///</summary>
        [TestMethod()]
        public void TraceEntryTest()
        {
            Trace target = new Trace();
            Entry entry = new Entry();
            Thread.Sleep(10);
            target.TraceEntry(entry);
            Assert.IsTrue(entry.Offset > 0);
            Assert.IsTrue(entry.Duration == 0);
        }        
    }
}
