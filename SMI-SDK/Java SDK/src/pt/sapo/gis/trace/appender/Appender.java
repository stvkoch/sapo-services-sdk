package pt.sapo.gis.trace.appender;

import pt.sapo.gis.exception.EventCallException;
import java.util.List;
import pt.sapo.gis.trace.TraceSeverityEventFilter;
import pt.sapo.gis.ICallback;
import pt.sapo.gis.trace.Entry;
import pt.sapo.gis.trace.EntrySeverityEventFilter;
import pt.sapo.gis.trace.Trace;
import pt.sapo.gis.trace.TraceManager;
import pt.sapo.gis.trace.configuration.Event;
import pt.sapo.gis.trace.configuration.EventType;
import pt.sapo.gis.trace.configuration.SeverityType;

/**
 * @author GIS Team
 * Base class to appenders
 */
public abstract class Appender {

    protected EntrySeverityEventFilter entrySeverityFilter = null;
    protected TraceSeverityEventFilter traceSeverityFilter = null;
    
    /**
     * Entry events callbacks
     */ 
    protected ICallback<Entry> entryCallback = new ICallback<Entry>() {

        @Override
        public void call(Entry e) throws EventCallException {
            entryEventHandler(e);
        }
    };
    
    /** 
     * Trace events callbacks
     */
    protected ICallback<Trace> traceCallback = new ICallback<Trace>() {

        @Override
        public void call(Trace t) throws EventCallException {
            traceEventHandler(t);
        }
    };

    /**     
     * Register the appender on trace events indicated on configuration object
     * @param config the appender configuration object
     */
    public Appender(pt.sapo.gis.trace.configuration.Appender config) {
        if (config.getEvents() != null) {
            for (Event event : config.getEvents().getList()) {
                List<SeverityType> severities = null;

                if (event.getFilters() != null) {
                    severities = event.getFilters().getSeverityFilters();
                }

                if (event.getType().equals(EventType.ENTRY)) {
                    if (severities != null) {
                        entrySeverityFilter = new EntrySeverityEventFilter(severities);
                    }
                    TraceManager.getOnEntryEvent().registerListener(entryCallback, entrySeverityFilter);
                }
                if (event.getType().equals(EventType.TRACE)) {
                    if (severities != null) {
                        traceSeverityFilter = new TraceSeverityEventFilter(severities);
                    }
                    TraceManager.getOnTraceEvent().registerListener(traceCallback, traceSeverityFilter);
                }
            }
        }
    }

    protected abstract void entryEventHandler(Entry e) throws EventCallException;

    protected abstract void traceEventHandler(Trace t) throws EventCallException;
}
