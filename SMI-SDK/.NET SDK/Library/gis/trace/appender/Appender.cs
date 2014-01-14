namespace pt.sapo.gis.trace.appender
{
    using System.Linq;
    using System.Collections.Generic;
    using pt.sapo.gis.trace.configuration;
    using System;

    /**
     * @author GIS Team
     * Base class to appenders
     */
    public abstract class Appender : IDisposable
    {
        private GenericEventHandler<Entry> onEntry = null;
        private GenericEventHandler<Trace> onTrace = null;
        protected Dictionary<eventType, severityType[]> severities;

        public AppenderConfigProperties Properties { get; protected set; }

        /// <summary>
        /// Register the appender on trace events indicated on configuration object
        /// </summary>
        /// <param name="config">the appender configuration object</param>
        /// <exception cref="IntializatonException">When fail to initialize.</exception>
        public Appender(pt.sapo.gis.trace.configuration.appender config)
        {
            Properties = new AppenderConfigProperties(config.property);
            severities = config.events.Where(e => e.filters != null).ToDictionary(e => e.type, e => e.filters);
            foreach(@event evt in config.events){                
                switch(evt.type) {
                    case eventType.entry:
                        if (evt.filters == null || evt.filters.Length == 0)
                            onEntry = e => OnEntry(e);                            
                        else 
                            onEntry = e => { if(evt.filters.Contains(e.Severity)) OnEntry(e); };
                        TraceManager.OnEntry += onEntry;
                        break;
                    case eventType.trace:
                        if (evt.filters == null || evt.filters.Length == 0)
                            onTrace = t => OnTrace(t);
                        else 
                            onTrace = t => { if(t.GetSeverityTypes().Any(s => evt.filters.Contains(s))) OnTrace(t); };
                        TraceManager.OnTrace += onTrace;
                        break;
                }
            }
            
        }

        public abstract void OnEntry(Entry e);
        public abstract void OnTrace(Trace t);

        public void Dispose()
        {
            if (onEntry != null)
                TraceManager.OnEntry -= onEntry;
            if (onTrace != null)
                TraceManager.OnTrace -= onTrace;
        }

        ~Appender()
        {
            Dispose();
        }
    }
}
