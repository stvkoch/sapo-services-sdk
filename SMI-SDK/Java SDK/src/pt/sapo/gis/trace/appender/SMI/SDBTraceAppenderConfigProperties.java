package pt.sapo.gis.trace.appender.SMI;

import java.util.List;
import pt.sapo.gis.trace.configuration.Appender.Property;
import pt.sapo.gis.trace.configuration.AppenderConfigProperties;

public class SDBTraceAppenderConfigProperties extends AppenderConfigProperties{
    private final String DEFAULT_PROTOCOL = "http";
    private final Integer DEFAULT_PORT = -1;
    private final String DEFAULT_PATH = "";
    
    public SDBTraceAppenderConfigProperties(List<Property> properties){         
        super(properties);
    }
    
    public String getHost(){
        return propertiesMap.get("host");
    }
    
    public Integer getPort(){
        Integer i = null;
        i= DEFAULT_PORT;
        if(propertiesMap.containsKey("port"))
            i = Integer.parseInt(propertiesMap.get("port"));
        return i;
    }

    public String getProtocol(){        
        if(propertiesMap.containsKey("protocol"))
            return propertiesMap.get("protocol");
        return DEFAULT_PROTOCOL;
    }

    public String getPath(){
        if(propertiesMap.containsKey("url"))
            return propertiesMap.get("url");
        return DEFAULT_PATH;
    }

    public String getToken(){
        return propertiesMap.get("token");
    }

}
