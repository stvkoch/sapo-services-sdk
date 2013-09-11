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
public class MessageEntryTest {
    
    /**
     * Test of setMessage getMessage method, of class MessageEntryProperty.
     */
    @Test
    public void testGetSetMessage() {
        System.out.println("getter/setter Message");
        
        MessageEntry instance = new MessageEntry();
        MessageEntryProperty actual = new HTTPRequestMessageEntryProperty();
        
        instance.setMessage(actual);
        MessageEntryProperty result = instance.getMessage();
        
        assertEquals(actual, result);
    }
}
