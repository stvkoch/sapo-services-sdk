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
public class LinkedEntryTest {
    
    /**
     * Test of setLinkedTraceId getLinkedTraceId method, of class LinkedEntry.
     */
    @Test
    public void testGetSetLinkedTraceId() {
        System.out.println("getter/setter LinkedTraceId");
        
        LinkedEntry instance = new LinkedEntry();
                
        String taceId = "aaa-bbb-ccc";
        instance.setLinkedTraceId(taceId);
        String result = instance.getLinkedTraceId();
        
        assertEquals(taceId, result);
    }

}
