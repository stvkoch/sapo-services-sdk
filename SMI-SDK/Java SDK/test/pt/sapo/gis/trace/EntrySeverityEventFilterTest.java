/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import java.util.Collection;
import java.util.ArrayList;
import org.junit.Before;
import org.junit.Test;
import pt.sapo.gis.trace.configuration.SeverityType;
import static org.junit.Assert.*;

/**
 *
 * @author GIS Team
 */
public class EntrySeverityEventFilterTest {
    
    EntrySeverityEventFilter instance;
    
    @Before
    public void setUp() {
        Collection<SeverityType> values = new ArrayList<SeverityType>();
        values.add(SeverityType.WARN);
        values.add(SeverityType.INFO);
        instance = new EntrySeverityEventFilter(values);
    }

    /**
     * Test of test method, of class EntrySeverityEventFilter.
     */
    @Test
    public void testTest() {
        System.out.println("test");
        Entry e = new Entry("", SeverityType.WARN);
        boolean result_1 = instance.test(e);
        e = new Entry("", SeverityType.DEBUG);
        boolean result_2 = instance.test(e);
        assertEquals(result_1, true);
        assertEquals(result_2, false);        
    }
}
