/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author GIS Team
 */
public class HTTPRequestMessageTest {
    
    HTTPRequestMessageEntryProperty instance = new HTTPRequestMessageEntryProperty();
        
     /**
     * Test of getPathAndQuery setPathAndQuery method, of class HTTPRequestMessageEntryProperty.
     */
    @Test
    public void testGetSetPathAndQuery() {
        System.out.println("getter/setter PathAndQuery");
        
        String value = "xpto";
        instance.setPathAndQuery(value);
        String result = instance.getPathAndQuery();
        
        assertEquals(value, result);
    }
    
     /**
     * Test of getVerb setVerb method, of class HTTPRequestMessageEntryProperty.
     */
    @Test
    public void testGetSetVerb() {
        System.out.println("getter/setter Verb");
        
        String value = "xpto";
        instance.setVerb(value);
        String result = instance.getVerb();
        
        assertEquals(value, result);
    }
    
     /**
     * Test of getSecure setSecure method, of class HTTPRequestMessageEntryProperty.
     */
    @Test
    public void testGetSetSecure() {
        System.out.println("getter/setter Secure");
        
        boolean value = false;
        instance.setSecure(value);
        boolean result = instance.isSecure();
        
        assertEquals(value, result);
    }
    
}
