/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace.appender.SMI;

import java.util.List;
import pt.sapo.gis.trace.configuration.Appender.Property;
import pt.sapo.gis.trace.configuration.AppenderConfigProperties;

/**
 *
 * @author GIS Team
 */
public class SMILoggerAppenderConfigProperties extends AppenderConfigProperties{
    
    public SMILoggerAppenderConfigProperties(List<Property> properties){         
        super(properties);
    }
    
     public String getDefaultSourceID(){
        return propertiesMap.get("defaultSourceId");
    }
}
