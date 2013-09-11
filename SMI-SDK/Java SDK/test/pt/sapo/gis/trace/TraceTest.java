/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import pt.sapo.gis.exception.EventCallException;
import pt.sapo.gis.trace.configuration.SeverityType;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;
import org.junit.Before;
import org.junit.Test;
import pt.sapo.gis.ICallback;
import static org.junit.Assert.*;

/**
 *
 * @author GIS Team
 */
public class TraceTest {

    Trace instance;
    Entry entry_a = new Entry("entry A", SeverityType.INFO);
    Entry entry_b = new Entry("entry B", SeverityType.WARN);    
    String traceCallbackResult;
    String entryCallbackResult;
    String traceId = "T1";
    
    ICallback<Trace> traceCallback = new ICallback<Trace>() {
        @Override
        public void call(Trace t) throws EventCallException {
            traceCallbackResult = t.getId();
        }
    };
    
    ICallback<Entry> entryCallback = new ICallback<Entry>() {
        @Override
        public void call(Entry e) throws EventCallException {
            entryCallbackResult = e.getDescription();
        }
    };
      
    @Before
    public void setUp() {
        instance = new Trace(traceId, null);
        instance.getOnTraceEvent().registerListener(traceCallback);
        instance.getOnEntryEvent().registerListener(entryCallback);
        traceCallbackResult = "";
        entryCallbackResult = "";
    }

    /**
     * Test of trace method, of class Trace.
     */
    @Test
    public void testTrace() {
        System.out.println("trace");

        instance.trace(entry_a);
        int n = instance.getEntries().size();

        assertEquals(1, n);
        assertEquals(entryCallbackResult, entry_a.description);
    }

    /**
     * Test of beginEntry method, of class Trace.
     */
    @Test
    public void testBeginEntry_0args() {
        System.out.println("beginEntry");

        instance.beginEntry();

        assertEquals(1, instance.getContextSize());
    }

    /**
     * Test of beginEntry method, of class Trace.
     */
    @Test
    public void testBeginEntry_String_SeverityType() {
        System.out.println("beginEntry");

        instance.beginEntry("entry test", SeverityType.DEBUG);

        assertEquals(1, instance.getContextSize());
    }

    /**
     * Test of beginEntry method, of class Trace.
     */
    @Test
    public void testBeginEntry_3args() {
        System.out.println("beginEntry");

        Map<String, Object> properties = new HashMap<String, Object>();
        instance.beginEntry("entry test", SeverityType.DEBUG, properties);

        assertEquals(1, instance.getContextSize());
    }

    /**
     * Test of beginEntry method, of class Trace.
     */
    @Test
    public void testBeginEntry_Entry() {
        System.out.println("beginEntry");

        instance.beginEntry(entry_a);
        instance.beginEntry(entry_b);

        assertEquals(2, instance.getContextSize());
    }

    /**
     * Test of endEntry method, of class Trace.
     */
    @Test
    public void testEndEntry() {
        instance.beginEntry(entry_a);
        instance.beginEntry(entry_b);
        instance.endEntry();
        int nStack_1 = instance.getContextSize();
        int nEntryEntries = entry_a.getEntries().size();

        instance.endEntry();
        int nStack_2 = instance.getContextSize();
        int nEntries = instance.getEntries().size();

        assertEquals(1, nStack_1);
        assertEquals(1, nEntryEntries);

        assertEquals(0, nStack_2);
        assertEquals(1, nEntries);
        assertEquals(entryCallbackResult, entry_a.description);
    }

    /**
     * Test of finish method, of class Trace.
     */
    @Test
    public void testFinish() {
        instance.beginEntry(entry_a);
        instance.beginEntry(entry_b);
        instance.endEntry();
        instance.finish();

        int nEntries = instance.getEntries().size();
        int nSubEntries = instance.getEntries().get(0).entries.size();

        assertEquals(1, nEntries);
        assertEquals(traceCallbackResult, traceId);
        assertEquals(entryCallbackResult, entry_a.description);
    }

    /**
     * Test of getSeverityTypes method, of class Trace.
     */
    @Test
    public void testGetSeverityTypes() {
        instance.trace(entry_a);
        instance.trace(entry_b);

        Set<SeverityType> severities = new HashSet<SeverityType>();
        severities.add(SeverityType.INFO);
        severities.add(SeverityType.WARN);
        Set<SeverityType> result = instance.getSeverityTypes();

        assertEquals(severities, result);
    }
}
