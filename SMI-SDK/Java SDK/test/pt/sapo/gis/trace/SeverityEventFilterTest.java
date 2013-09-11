/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import pt.sapo.gis.trace.configuration.SeverityType;
import java.util.ArrayList;
import java.util.Collection;
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
public class SeverityEventFilterTest {
    
    private static SeverityEventFilter filter;    

    @BeforeClass
    public static void setUpClass() throws Exception {
        Collection<SeverityType> severities = new ArrayList<SeverityType>();
        severities.add(SeverityType.INFO);
        severities.add(SeverityType.ERROR);
        severities.add(SeverityType.WARN);
        
        filter = new EntrySeverityEventFilter(severities);
    }

    @AfterClass
    public static void tearDownClass() throws Exception {
    }
    
    @Before
    public void setUp() {
    }
    
    @After
    public void tearDown() {
    }

    /**
     * Test of contains method, of class SeverityEventFilter.
     */
    @Test
    public void testContains() {
        System.out.println("contains");
        boolean result_1 = filter.contains(SeverityType.INFO);
        boolean result_2 = filter.contains(SeverityType.DEBUG);
        
        assertTrue(result_1);
        assertFalse(result_2);        
    }

    /**
     * Test of containsAny method, of class SeverityEventFilter.
     */
    @Test
    public void testContainsAny() {
        System.out.println("containsAny");
        
        Collection<SeverityType> severities = new ArrayList<SeverityType>();
        severities.add(SeverityType.DEBUG);
        severities.add(SeverityType.FATAL);
        
        boolean result_1 = filter.containsAny(severities);
        
        severities.add(SeverityType.WARN);
        boolean result_2 = filter.containsAny(severities);
        
        assertFalse(result_1); 
        assertTrue(result_2);
    }

    /**
     * Test of containsAll method, of class SeverityEventFilter.
     */
    @Test
    public void testContainsAll() {
        Collection<SeverityType> severities = new ArrayList<SeverityType>();
        severities.add(SeverityType.WARN);
        severities.add(SeverityType.ERROR);
        
        boolean result_1 = filter.containsAll(severities);
        
        severities.add(SeverityType.FATAL);
        boolean result_2 = filter.containsAll(severities);
        
        assertTrue(result_1);
        assertFalse(result_2);  
    }
}
