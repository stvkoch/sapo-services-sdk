/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace.configuration;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import pt.sapo.gis.trace.configuration.Appender.Property;

/**
 *
 * @author GIS Team
 */
public class AppenderConfigProperties {
    
    protected Map<String, String> propertiesMap = new HashMap<String, String>();
    
    public AppenderConfigProperties(List<Property> properties){         
        if(properties != null){            
            for(Property p : properties)
                propertiesMap.put(p.getName(), p.getContent());
        }
    }
}
