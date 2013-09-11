package pt.sapo.gis.trace;

import java.util.Map;
import java.util.Set;
import pt.sapo.gis.trace.configuration.SeverityType;

/**
* @author GIS Team 
 * Trace interface
 */
public interface ITrace {    
    void trace(Entry entry);
    Entry beginEntry();
    Entry beginEntry(String description, SeverityType severity);
    Entry beginEntry(String description, SeverityType severity, Map<String, Object> properties);
    Entry beginEntry(Entry entry);
    void endEntry();
    void finish();
    Set<SeverityType> getSeverityTypes();
    
}
