/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import java.util.HashMap;
import java.util.Map;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author GIS Team
 */
public class HTTPMessageEntryPropertyTest {
    
    HTTPMessageEntryProperty instance = new HTTPMessageEntryProperty();
    Map<String, String> headers = new HashMap<String, String>();
    String body = "test body";
    
    /**
     * Test of setHeaders getHeaders method, of class HTTPMessage.
     */
    @Test
    public void testGetSetHeaders() {
        System.out.println("getter/setter Headers");
        
        instance.setHeaders(headers);
        Map<String, String> result = instance.getHeaders();
        
        assertEquals(headers, result);        
    }
    
     /**
     * Test of setBody getBody method, of class HTTPMessage.
     */
    @Test
    public void testGetSetBody() {
        System.out.println("getter/setter Body");
        
        instance.setBody(body);
        String result = instance.getBody();
        
        assertEquals(body, result);
    }
      
    /**
     * Test of addHeader method, of class HTTPMessage.
     */
    @Test
    public void testAddHeaders() {
        System.out.println("addHeaders");
        
        instance.addHeader("headerName", "headerValue");
        int n = instance.getHeaders().size();
        String result = instance.getHeaders().get("headerName");
        
        assertEquals(1, n);
        assertEquals("headerValue", result);
    }

}
