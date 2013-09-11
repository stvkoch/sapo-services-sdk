package pt.sapo.gis.trace.appender.SMI;

import java.util.List;
import pt.sapo.gis.trace.configuration.Appender.Property;
import pt.sapo.gis.trace.configuration.AppenderConfigProperties;

public class BrokerSMILoggerAppenderConfigProperties extends SMILoggerAppenderConfigProperties{    
    
    
    public BrokerSMILoggerAppenderConfigProperties(List<Property> properties){         
        super(properties);
    }
    
    public String getHost(){
        return propertiesMap.get("host");
    }
            
    public Integer getPort(){        
        Integer i = null;
        if(propertiesMap.containsKey("port"))
            i = Integer.parseInt(propertiesMap.get("port"));
        return i;
    }
    
    public String getTopic(){
        return propertiesMap.get("topic");
    }
       
}
