package pt.sapo.gis.trace;

import java.net.InetAddress;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.Stack;
import java.util.logging.Level;
import java.util.UUID;
import java.util.logging.Logger;
import pt.sapo.gis.Timer;
import pt.sapo.gis.Event;
import pt.sapo.gis.IEvent;
import pt.sapo.gis.trace.configuration.SeverityType;

/**
 * @author GIS Team
 * Class that implements ITrace interface
 */
public class Trace implements ITrace {

    private final Event<Entry> onEntry = new Event<Entry>();
    private final Event<Trace> onTrace = new Event<Trace>();
    private final List<Entry> entries = new ArrayList<Entry>();
    
    private String id;
    private String contextId;
    private Date start;
    private Date end;
    protected InetAddress serverInfo;
    protected InetAddress clientInfo;        
    protected Timer timer;
    
    //protected Stack<TimerEntry> context = new Stack<TimerEntry>();
    protected static final ThreadLocal<Stack<TimerEntry>> localContext = new ThreadLocal<Stack<TimerEntry>>(){                   
       /* @Override
        protected Stack<TimerEntry> initialValue() {
                return new Stack<TimerEntry>();
             }*/
    };
    
    private Map<String, Object> properties = new HashMap<String, Object>();

    public Map<String, Object> getProperties() {
        return properties;
    }

    public void setProperties(Map<String, Object> properties) {
        this.properties = properties;
    }
    
    public Object getProperty(String key) {
        return properties.get(key);
    }
    
    public void setProperty(String key, Object value) {
        this.properties.put(key, value);
    }

    public InetAddress getClientInfo() {
        return clientInfo;
    }

    public void setClientInfo(InetAddress clientInfo) {
        this.clientInfo = clientInfo;
    }

    public InetAddress getServerInfo() {
        return serverInfo;
    }

    public void setServerInfo(InetAddress serverInfo) {
        this.serverInfo = serverInfo;
    }

    public Date getEnd() {
        return end;
    }

    /**
     * Gets the entries list
     * @return the a read only list of trace entries
     */
    public List<Entry> getEntries() {
        synchronized(entries){
            return Collections.unmodifiableList(entries);
        }
    }   
    
    /**
     * Gets the trace id
     * @return the trace identifier
     */
    public String getId() {
        return id;
    }
    /**
     * Gets the trace id
     * @return the trace context identifier
     */
    public String getContextId() {
        return contextId;
    }
    
    /**
     * Gets the start date
     * @return the trace start date
     */
    public Date getStart() {
        return start;
    }

    /**
     * Gets the size of context
     * @return the number of entries in current context
     */
    public int getContextSize(){
        return localContext.get().size();
    }
    
    /**
     * Creates a new trace     
     */
    public Trace() {
        this(UUID.randomUUID().toString(), null);
    }

    /**
     * Creates a new trace     
     * @param id the trace identifier
     * @param contextId the trace context identifier
     */
    public Trace(String id, String contextId) {
        if (id == null) {
            id = UUID.randomUUID().toString();
        }
        this.id = id;
        this.contextId = contextId;
        this.start = new Date();
        this.timer = new Timer().Start();   
        localContext.set(new Stack<TimerEntry>());
    }

    /**
     * Trace a new entry
     * @param entry the entry to add to the trace
     */
    @Override
    public void trace(Entry entry) {
        entry.setParent(this);
        entry.offset = this.timer.ElapsedMilis();
        addEntry(entry);
    }
    
    private void addEntry(Entry entry){
      if (localContext.get().empty()) {
            synchronized(entries){
                entries.add(entry);
            }
        } else {
            localContext.get().peek().entry.entries.add(entry);
        }
        try {
            onEntry.notify(entry);
        } catch (Exception ex) {
            Logger.getLogger(Trace.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    /**
     * Start a new entry context. Every entry added after this will be
     * added as sub entry of this one.     
     */
    @Override
    public Entry beginEntry() {        
        return beginEntry(new Entry());        
    }

    /**
     * Start a new entry context. Every entry added after this will be
     * added as sub entry of this one.     
     * @param description the new entry description
     * @param severity the new entry severity type
     */
    @Override
    public Entry beginEntry(String description, SeverityType severity) {
        return beginEntry(new Entry(description, severity));
        
    }

    /**
     * Start a new entry context. Every entry added after this will be
     * added as sub entry of this one.     
     * @param description the new entry description
     * @param severity the new entry severity type
     * @param properties the new entry properties map
     */
    @Override
    public Entry beginEntry(String description, SeverityType severity, Map<String, Object> properties) {
        return beginEntry(new Entry(description, severity, properties));        
    }

    /**
     * Start a new entry context. Every entry added after this will be
     * added as sub entry of this one.     
     * @param entry the entry to start the new context     
     */
    @Override
    public Entry beginEntry(Entry entry) {        
        Timer entryTimer = new Timer().Start();
        entry.setParent(this);
        entry.offset = entryTimer.ElapsedMilis();
        localContext.get().push(new TimerEntry(entryTimer, entry));
        return entry;
    }
    
    /**
     * Ends the current entry context, adding it to the trace entries list 
     * or to the parent entry if it was created inside a context     
     */
    @Override
    public void endEntry() {
        TimerEntry current = localContext.get().pop();
        double milis = current.timer.Stop().ElapsedMilis();
        current.entry.duration = milis;
        addEntry(current.entry);
    }

    /**
     * Gets OnEntry event
     * @return the object that implements IEvent of an Entry
     */
    public IEvent<Entry> getOnEntryEvent() {
        return this.onEntry;
    }

    /**
     * Gets OnEntry event
     * @return the object that implements IEvent of an Trace
     */
    public IEvent<Trace> getOnTraceEvent() {
        return this.onTrace;
    }
    /**
     * Finishes the trace closing all the open context entries     
     */
    @Override
    public void finish() {
        this.timer.Stop();
        this.end = new Date();
        while (!localContext.get().empty()) {
            this.endEntry();
        }
        try {
            onTrace.notify(this);
        } catch (Exception ex) {
            Logger.getLogger(Trace.class.getName()).log(Level.SEVERE, null, ex);
        }
        finally{
            localContext.remove();
        }
    }

    /**
     * Gets all severity types that exists on trace entries, including sub entries
     * @return the severity types collection set
     */
    @Override
    public Set<SeverityType> getSeverityTypes() {
        Set<SeverityType> severities = new HashSet<SeverityType>();
        synchronized(entries){
            for (Entry e : this.entries) {
                GetEntrySeverityTypes(e, severities);
            }
        }
        return severities;
    }

    private void GetEntrySeverityTypes(Entry entry, Set<SeverityType> severities) {
        severities.add(entry.getSeverity());
        for (Entry e : entry.getEntries()) {
            GetEntrySeverityTypes(e, severities);
        }
    }
}