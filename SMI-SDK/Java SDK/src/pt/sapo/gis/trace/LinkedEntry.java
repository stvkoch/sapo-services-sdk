package pt.sapo.gis.trace;

import java.util.Map;
import pt.sapo.gis.trace.configuration.SeverityType;

/**
 * @author GIS Team
 * Class for entry with an linked trace Id
 */
public class LinkedEntry extends Entry{
    public static final String LINKED_TRACE_ID_PROPERTY = "linkedTraceId";
    
    public String getLinkedTraceId() {        
        return (String) getProperty(LINKED_TRACE_ID_PROPERTY);
    }
    
    public final void setLinkedTraceId(String linkedTraceId) {
        setProperty(LINKED_TRACE_ID_PROPERTY, linkedTraceId);
    }
    
    public LinkedEntry(){
        super();
    }
    
    public LinkedEntry(String description, SeverityType severity){    
        super(description, severity);
    }
    
    public LinkedEntry(String description, SeverityType severity, Map<String,Object> properties){    
        super(description, severity, properties);
    }
}