using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using pt.sapo.gis.trace;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Tests
{
    [TestClass]
    public abstract class BaseTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private MemoryAppender testAppender = new MemoryAppender();

        [ClassInitialize]
        public void ClassInitialize() {
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            hierarchy.Root.AddAppender(testAppender);
            hierarchy.Configured = true;
            hierarchy.RaiseConfigurationChanged(EventArgs.Empty);
        }

        [ClassCleanup]
        public void ClassCleanup() {
            (LogManager.GetRepository() as Hierarchy).Root.RemoveAppender(testAppender);
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            ClassInitialize();
            var binFolder = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.ToString();
            if (File.Exists(Path.Combine(binFolder, TestContext.TestName + ".config")))
            {
                var a = new AlternateAppConfig(TestContext.TestName + ".config");
                TestContext.Properties["ConfigFileSwitcher"] = a;
            }
            TraceManager_Accessor.LoadConfig();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            var errors = (from le in testAppender.GetEvents()
                         where le.Level >= Level.Error
                         select le.ExceptionObject != null ? le.ExceptionObject.Message : le.RenderedMessage).ToArray();
            if(errors.Length > 0)
                Assert.Fail("Log errors: {{{0}}}", String.Join(", ", errors));

            ClassCleanup();
            if (TestContext.Properties.Contains("ConfigFileSwitcher"))
            {
                ((AlternateAppConfig)TestContext.Properties["ConfigFileSwitcher"]).Dispose();
                TestContext.Properties.Remove("ConfigFileSwitcher");
            }
        }
    }
}
