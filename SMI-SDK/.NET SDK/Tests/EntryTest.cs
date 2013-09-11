using pt.sapo.gis.trace;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using pt.sapo.gis.trace.configuration;
using System.Collections.Generic;

namespace Tests
{
    
    
    /// <summary>
    ///This is a test class for EntryTest and is intended
    ///to contain all EntryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EntryTest : BaseTest
    {
        /// <summary>
        ///A test for Entry Constructor
        ///</summary>
        [TestMethod()]
        public void EntryConstructorTest()
        {
            string description = "Some descriptions.";
            severityType severity = severityType.ERROR;
            IDictionary<string, object> properties = new Dictionary<String, Object>() { {"prop1", "asd"} };
            Entry target = new Entry(description, severity, properties);
            Assert.AreSame(description, target.Description);
            Assert.AreEqual(severity, target.Severity);
            Assert.AreEqual(1, target.Properties.Count);
            Assert.IsTrue(target.Properties.ContainsKey("prop1"));
            Assert.AreEqual("asd", target.Properties["prop1"]);
        }                
    }
}
