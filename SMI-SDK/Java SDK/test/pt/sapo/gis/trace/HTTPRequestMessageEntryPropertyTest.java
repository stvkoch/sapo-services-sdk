/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author GIS Team
 */
public class HTTPRequestMessageEntryPropertyTest {
    
    HTTPRequestMessageEntryProperty instance;
            
    
    @Before
    public void setUp() {
        instance = new HTTPRequestMessageEntryProperty();
    }

    /**
     * Test of get/setPathAndQuery method, of class HTTPRequestMessageEntryProperty.
     */
    @Test
    public void testGetSetPathAndQuery() {
        System.out.println("get/setPathAndQuery");
        
        String value = "teste";
        instance.setPathAndQuery(value);
        String result = instance.getPathAndQuery();
        assertEquals(value, result);
        
    }   
    
    /**
     * Test of is/setSecure method, of class HTTPRequestMessageEntryProperty.
     */
    @Test
    public void testGetIsSecure() {
        System.out.println("is/setSecure");
        
        boolean value = false;
        instance.setSecure(value);
        boolean result = instance.isSecure();
        assertEquals(value, result);
        
    }
    
    /**
     * Test of get/setVerb method, of class HTTPRequestMessageEntryProperty.
     */
    @Test
    public void testGetSetVerb() {
        System.out.println("get/setVerb");
        
        String value = "teste";
        instance.setVerb(value);
        String result = instance.getVerb();
        assertEquals(value, result);
        
    }
   
}
