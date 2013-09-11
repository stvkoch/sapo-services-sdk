namespace pt.sapo.gis.trace.appender.SMI
{
    using System;
    using System.Collections.Generic;
    using pt.sapo.gis.trace.configuration;

    public class BrokerSMILoggerAppenderConfigProperties : SMILoggerAppenderConfigProperties
    {
        public BrokerSMILoggerAppenderConfigProperties(IDictionary<String,String> properties) : base(properties) { }

        public String Host { get { return this["host"]; } }

        public int Port { get { return Int32.Parse(this["port"]); } }

        public String Topic { get { return this["topic"]; } }
    }
}
