using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pt.sapo.gis.trace.configuration;

namespace pt.sapo.gis.trace.appender {

    /// <summary>
    /// Trace SDK Appender that logs all traces that exceed a determinated amount of total time.
    /// Logs are stored in files.
    /// </summary>    
    public class TimeoutFileLogAppender : FileAppender{

        public new TimeoutFileLogAppenderConfigProperties Properties { get; protected set; }

        public TimeoutFileLogAppender(pt.sapo.gis.trace.configuration.appender config)
            : base(config) {
            this.Properties = new TimeoutFileLogAppenderConfigProperties(base.Properties);
            base.Properties = this.Properties;        
        }

        public override void OnTrace(Trace t) {
            if(Properties.Timeout == -1 || (t.End-t.Start).TotalMilliseconds > Properties.Timeout)
                base.OnTrace(t);
        }
    }
}
