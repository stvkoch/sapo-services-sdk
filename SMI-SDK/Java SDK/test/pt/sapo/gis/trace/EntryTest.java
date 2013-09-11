/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import java.util.List;
import org.junit.Before;
import org.junit.Test;
import static org.junit.Assert.*;
import pt.sapo.gis.trace.configuration.SeverityType;

/**
 *
 * @author GIS Team
 */
public class EntryTest {
    
    Entry instance;
    
    @Before
    public void setUp() {
        instance = new Entry("teste", SeverityType.ERROR);        
    }
        
    /**
     * Test of getEntries method, of class Entry.
     */
    @Test(expected = UnsupportedOperationException.class)  
    public void testGetEntries() {
        System.out.println("getEntries");
        
        List result = instance.getEntries();
        
        result.add(new Entry());        
    }

    /**
     * Test of addEntries method, of class Entry.
     */
    @Test
    public void testAddEntries() {
        System.out.println("addEntries");
        
        instance.addEntries(new Entry());
        int value = instance.getEntries().size();
        
        assertEquals(value, 1);        
    }

    /**
     * Test of setProperty method, of class Entry.
     */
    @Test
    public void testGetSetProperty() {
        System.out.println("get/setProperty");
        String key = "key";
        Object value = "teste";
        instance.setProperty(key, value);
        String result = (String)instance.getProperty(key);
        
        assertEquals(value, result);
    }
    
    /**
     * Test of get/setDescription method, of class Entry.
     */
    @Test
    public void testGetSetDescription() {
        System.out.println("get/setDescription");
        String value = "teste";
        instance.setDescription( value);
        String result = instance.getDescription();
        
        assertEquals(value, result);
    }
    
       /**
     * Test of get/setType( method, of class Entry.
     */
    @Test
    public void testGetSetType() {
        System.out.println("get/setType");
        
        String value = "UNKNOWN";
        instance.setType( value);
        String result = instance.getType();
        assertEquals(value, result);
    }
    
    /**
     * Test of get/setOffset( method, of class Entry.
     */
    @Test
    public void testGetSetOffset() {
        System.out.println("get/setOffset");
        
        double value = 10.2;
        instance.setOffset(value);
        double result = instance.getOffset();
        assertEquals(value, result, 0);        
    }
   
    /**
     * Test of get/setDuration( method, of class Entry.
     */
    @Test
    public void testGetSetDuration() {
        System.out.println("get/setDuration");
        
        double value = 10.2;
        instance.setDuration(value);
        double result = instance.getDuration();
        assertEquals(value, result, 0);        
    }
    
    /**
     * Test of get/setParent( method, of class Entry.
     */
    @Test
    public void testGetSetParent() {
        System.out.println("get/setParent");
        
        Trace value = new Trace();
        instance.setParent(value);
        Trace result = instance.getParent();
        assertEquals(value, result);
    }
    
     /**
     * Test of get/setSeverity( method, of class Entry.
     */
    @Test
    public void testGetSetSeverity() {
        System.out.println("get/setSeverity");
        
        SeverityType value = SeverityType.ERROR;
        instance.setSeverity(value);
        SeverityType result = instance.getSeverity();
        assertEquals(value, result);        
    }
}
