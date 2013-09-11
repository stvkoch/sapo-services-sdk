package pt.sapo.sdb.trace;

import java.util.Map;

/**
 *
 * @author GIS Team
 */
public class Message {  
    
    public String Body;
    public Map<String, String> Headers;
    public Request Request;
    public Response Response; 
}