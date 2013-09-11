package pt.sapo.gis.trace;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import pt.sapo.gis.trace.configuration.SeverityType;

/**
 * @author GIS Team
 * Entry class that represents a trace entry
*/
public class Entry{
    private Trace parent;
    protected String type;
    protected String description;
    protected SeverityType severity;
    protected double offset;
    protected double duration;
    protected List<Entry> entries = new ArrayList<Entry>();
    private Map<String,Object> properties;


    /**
     * Get entry parent trace
     * @return the trace that contains the entry
    */    
    public Trace getParent() {
        return parent;
    }
    
    /**
     * Set entry parent trace
     */
    void setParent(Trace parent) {
        this.parent = parent;
    }    
    
    /**
     * Get entry sub entries
     * @return the entry sub entries list
    */
    public List<Entry> getEntries() {
        return Collections.unmodifiableList(entries);
    }

    /**
     * Add sub entry to entry
    */
    public void addEntries(Entry entry) {
        this.entries.add(entry);
    }

    /**
     * Get entry properties map
     * @return the entry properties map
    */
    public Map<String, Object> getProperties() {        
        return this.properties;
    }
    
    /**
     * Get an entry property
     * @param key property name
     * @return the property value
    */
    public Object getProperty(String key) {
        if(this.properties == null)
            return null;
        return this.properties.get(key);
    }
    
    /**
     * Set an entry property
     * @param key property name
     * @param value property value
    */
    public void setProperty(String key , Object value) {
         if(this.properties == null)
            this.properties = new HashMap<String, Object>();
        this.properties.put(key, value);
    }        
    
    /**
     * Get entry type
     * @return the entry type enum value
    */
    public String getType() {
        return type;
    }

    /**
     * Set entry type
     * 
    */
    public void setType(String type) {
        this.type = type;
    }

    /**
     * Get entry description
     * @return the entry description
    */
    public String getDescription() {
        return description;
    }

    /**
     * Set entry description
    */
    public void setDescription(String description) {
        this.description = description;
    }

    /**
     * Get entry duration
     * @return the entry time duration
    */
    public double getDuration() {
        return duration;
    }

    /**
     * Set entry duration time value
    */
    public void setDuration(double duration) {
        this.duration = duration;
    }

    /**
     * Get entry offset time
      @return the entry offset value
    */
    public double getOffset() {
        return offset;
    }

    /**
     * Set entry offset time
    */
    public void setOffset(double offset) {
        this.offset = offset;
    }
    
    /**
     * Get entry severity
     * @return the entry severity type enum value
    */
    public SeverityType getSeverity() {
        return severity;
    }

    /**
     * Set entry severity
    */
    public void setSeverity(SeverityType severity) {
        this.severity = severity;
    }   
       
    /**
     * Create a new entry
    */    
    public Entry(){}
    
    /**
     * Create a new entry
     *  @param  description the entry description
     *  @param  severity the entry severity enum value
    */  
    public Entry(String description, SeverityType severity){    
        this(description, severity, new HashMap<String,Object>());
    }
    
    /**
     * Create a new entry
     * @param description the entry description
     * @param severity the entry severity enum value
     * @param properties the entry properties
    */  
    public Entry(String description, SeverityType severity, Map<String, Object> properties){    
        this.description = description;
        this.severity = severity;     
        this.properties = new HashMap<String, Object>(properties);
    }    
}