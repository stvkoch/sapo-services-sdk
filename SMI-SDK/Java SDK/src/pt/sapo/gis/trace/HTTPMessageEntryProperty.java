package pt.sapo.gis.trace;

import java.util.HashMap;
import java.util.Map;

/**
* @author GIS Team 
 * Entry property to wrap an HTTP message
 */
public class HTTPMessageEntryProperty extends MessageEntryProperty {
    public static final String HEADERS_PROPERTY_NAME = "headers";
    public static final String BODY_PROPERTY_NAME = "body";
    
    public Map<String, String> getHeaders() {
        return (Map<String, String>) this.get(HEADERS_PROPERTY_NAME);
    }

    public void setHeaders(Map<String, String> headers) {
        this.put(HEADERS_PROPERTY_NAME, headers);
    }

    public void addHeader(String name, String value) {
        if(getHeaders() == null)
            setHeaders(new HashMap<String, String>());
        getHeaders().put(name, value);
    }
    
    public String getBody() {
        return (String) this.get(BODY_PROPERTY_NAME);
    }

    public void setBody(String body) {               
        this.put(BODY_PROPERTY_NAME, body);
    }    
}