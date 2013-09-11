namespace pt.sapo.gis.trace.appender.SMI
{
    using System;
    using System.Collections.Generic;
    using pt.sapo.gis.trace.configuration;

    public class SDBTraceAppenderConfigProperties : AppenderConfigProperties
    {
        private readonly String DEFAULT_PROTOCOL = "http";
        private readonly String DEFAULT_PATH = "";

        public SDBTraceAppenderConfigProperties(IDictionary<String, String> properties) : base(properties) { }

        public String Host { get { return this["host"]; } }

        public int? Port { get { return ContainsKey("port") ? new int?(Int32.Parse(this["port"])) : new int?(); } }

        public String Protocol { get { return ContainsKey("protocol") ? this["protocol"] : DEFAULT_PROTOCOL; } }

        public String Path { get { return ContainsKey("path") ? this["path"] : DEFAULT_PATH; } }

        public String Token { get { return this["token"]; } }
    }
}
