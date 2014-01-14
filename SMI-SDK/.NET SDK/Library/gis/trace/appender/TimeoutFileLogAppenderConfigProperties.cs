using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pt.sapo.gis.trace.configuration {
    /// <summary>
    /// Configuration Object to TimeoutFileLogAppender
    /// </summary>
    public class TimeoutFileLogAppenderConfigProperties : FileAppenderConfigProperties {
        public TimeoutFileLogAppenderConfigProperties(IDictionary<String, String> properties) 
            : base(properties) { }

        /// <summary>
        /// Timeout in miliseconds co compare with trace total time (-1 to skip check and log all traces).
        /// Default: -1
        /// </summary>
        public int Timeout { get { return ContainsKey("timeout") ?int.Parse(this["timeout"]):-1; } }

        /// <summary>
        /// Path to write log files
                
    }
}
