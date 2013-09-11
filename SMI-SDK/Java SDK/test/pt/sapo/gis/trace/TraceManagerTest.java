/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

import java.net.URLDecoder;
import org.junit.Before;
import org.junit.Test;
import static org.junit.Assert.*;
import pt.sapo.gis.ICallback;
import pt.sapo.gis.exception.EventCallException;

/**
 *
 * @author GIS Team
 */
public class TraceManagerTest {
    
    Trace trace;
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
        System.setProperty("trace.configFile", URLDecoder.decode(getClass().getClassLoader().getResource("pt/sapo/gis/trace/trace.xml").toExternalForm()).replaceFirst("file:/", ""));
        trace = TraceManager.BeginTrace(traceId, null);
        TraceManager.getOnTraceEvent().registerListener(traceCallback);
        TraceManager.getOnEntryEvent().registerListener(entryCallback);
    }
    
    /**
     * Test of GetTrace method, of class TraceManager.
     */
    @Test
    public void testGetTrace() {
        System.out.println("GetTrace");
        
        Trace result = TraceManager.GetTrace();
        assertNotNull(result);
    }

    /**
     * Test of EndTrace method, of class TraceManager.
     */
    @Test
    public void testEndTrace() {
        
        TraceManager.EndTrace();
        
        assertEquals(traceCallbackResult, traceId);
    }

    
}
