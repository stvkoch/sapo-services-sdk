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
public class HTTPResponseMessageTest {
    
       
    HTTPResponseMessageEntryProperty instance = new HTTPResponseMessageEntryProperty();
       
     /**
     * Test of getStatusCode setStatusCode method, of class HTTPRequestMessageEntryProperty.
     */
    @Test
    public void testGetSetStatusCode() {
        System.out.println("getter/setter StatusCode");
        
        int value = 400;
        instance.setStatusCode(value);
        int result = instance.getStatusCode();
        
        assertEquals(value, result);
    }
         
     /**
     * Test of getReasonPhrase setReasonPhrase method, of class HTTPRequestMessageEntryProperty.
     */
    @Test
    public void testGetSetReasonPhrase() {
        System.out.println("getter/setter ReasonPhrase");
        
        String value = "xpto";
        instance.setReasonPhrase(value);
        String result = instance.getReasonPhrase();
        
        assertEquals(value, result);
    }
}
