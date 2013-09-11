package pt.sapo.gis.trace;

import java.util.Map;
import pt.sapo.gis.trace.configuration.SeverityType;

/**
 * @author GIS Team
 * Class for entry with an exception
 */
public class ExceptionEntry extends Entry{
    public static final String EXCEPTION_PROPERTY_NAME = "exception";
            
    public Exception getException() {
        return ((ExceptionEntryProperty) getProperty(EXCEPTION_PROPERTY_NAME)).getException();
    }

    public final void setException(Exception e) {                
        setProperty(EXCEPTION_PROPERTY_NAME,  new ExceptionEntryProperty(e));
    }
    
    public ExceptionEntry(){}
    
    public ExceptionEntry(String description, SeverityType severity, Exception exception){    
        super(description, severity);
        setException(exception);
    }
    
    public ExceptionEntry(String description, SeverityType severity, Map<String,Object> properties, Exception exception){    
        super(description, severity, properties);
        setException(exception);
    }
}
