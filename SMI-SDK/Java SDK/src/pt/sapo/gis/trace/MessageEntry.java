package pt.sapo.gis.trace;

import java.util.Map;
import pt.sapo.gis.trace.configuration.SeverityType;

/**
 * @author GIS Team
 * Class for entry with an message
 */
public class MessageEntry extends Entry{
    public static final String MESSAGE_PROPERTY_NAME = "message";
    
    public MessageEntryProperty getMessage() {
        return (MessageEntryProperty) getProperty(MESSAGE_PROPERTY_NAME);
    }

    public void setMessage(MessageEntryProperty message) {
        setProperty(MESSAGE_PROPERTY_NAME, message);
    }
    
    public MessageEntry(){
        super();
    }
    
     public MessageEntry(String description, SeverityType severity){    
        super(description, severity);
    }
    
    public MessageEntry(String description, SeverityType severity, Map<String,Object> properties){    
        super(description, severity, properties);
    }
       
}
