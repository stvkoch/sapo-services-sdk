/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author GIS Team
 */
public class HTTPResponseMessageEntryPropertyTest {
    
    HTTPResponseMessageEntryProperty instance;
    
       
    @Before
    public void setUp() {
        instance = new HTTPResponseMessageEntryProperty();
    }

     /**
     * Test of get/setStatusCode method, of class HTTPResponseMessageEntryProperty.
     */
    @Test
    public void testGetSetStatusCode() {
        System.out.println("get/setStatusCode");
        
        int value = 400;
        instance.setStatusCode(value);
        int result = instance.getStatusCode();
        assertEquals(value, result);        
    }   
    
 
         /**
     * Test of get/setReasonPhrase method, of class HTTPResponseMessageEntryProperty.
     */
    @Test
    public void testGetSetReasonPhrase() {
        System.out.println("get/setReasonPhrase");
        
        String value = "ERROR";
        instance.setReasonPhrase(value);
        String result = instance.getReasonPhrase();
        assertEquals(value, result);        
    } 
}
