/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import org.junit.Before;
import java.util.ArrayList;
import java.util.Collection;
import org.junit.Test;
import pt.sapo.gis.trace.configuration.SeverityType;
import static org.junit.Assert.*;


/**
 *
 * @author GIS Team
 */
public class TraceSeverityEventFilterTest {
    
    TraceSeverityEventFilter instance;

    @Before
    public void setUp() {
        Collection<SeverityType> values = new ArrayList<SeverityType>();
        values.add(SeverityType.WARN);
        values.add(SeverityType.INFO);
        instance = new TraceSeverityEventFilter(values);
    }


    /**
     * Test of test method, of class TraceSeverityEventFilter.
     */
    @Test
    public void testTest() {
        System.out.println("test");
        Trace t = new Trace();
        t.trace(new Entry("a", SeverityType.DEBUG));
        
        boolean result_1 = instance.test(t);
        t.trace(new Entry("a", SeverityType.WARN));
        boolean result_2 = instance.test(t);
        
        assertEquals(result_1, false);
        assertEquals(result_2, true);       
    }
}
