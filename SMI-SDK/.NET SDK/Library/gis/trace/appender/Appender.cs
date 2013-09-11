namespace pt.sapo.gis.trace.appender
{
    using System.Linq;
    using System.Collections.Generic;
    using pt.sapo.gis.trace.configuration;

    /**
     * @author GIS Team
     * Base class to appenders
     */
    public abstract class Appender
    {

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
                
                if(evt.type == eventType.entry){
                    if (evt.filters == null || evt.filters.Length == 0)
                        TraceManager.OnEntry += e => OnEntry(e);
                    else 
                        TraceManager.OnEntry += e => { if(evt.filters.Contains(e.Severity)) OnEntry(e); };
                }

                if(evt.type == eventType.trace){
                    if (evt.filters == null || evt.filters.Length == 0)
                        TraceManager.OnTrace += t => OnTrace(t);
                    else 
                        TraceManager.OnTrace += t => { if(t.GetSeverityTypes().Any(s => evt.filters.Contains(s))) OnTrace(t); };
                }
            }
            
        }

        public abstract void OnEntry(Entry e);
        public abstract void OnTrace(Trace t);
    }
}
