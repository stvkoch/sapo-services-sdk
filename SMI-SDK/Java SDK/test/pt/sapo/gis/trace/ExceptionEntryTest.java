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
public class ExceptionEntryTest {
    
    /**
     * Test of setException getException method, of class ExceptionEntry.
     */
    @Test
    public void testGetSetException() {
        System.out.println("getter/setter Exception");
        
        ExceptionEntry instance = new ExceptionEntry();
        Exception actual = new Exception("test exception");
        
        instance.setException(actual);
        Exception result = instance.getException();
        
        assertEquals(actual, result);        
    }    
}
