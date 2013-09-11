using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pt.sapo.gis.trace.configuration;

namespace pt.sapo.gis.trace.appender.SMI
{
    public class SMILoggerAppenderConfigProperties : AppenderConfigProperties
    {
        public SMILoggerAppenderConfigProperties(IDictionary<String, String> properties) : base(properties) { }

        public String DefaultSourceId { get { return ContainsKey("defaultSourceId") ? this["defaultSourceId"] : null; } }
    }
}
