package pt.sapo.gis;

import pt.sapo.gis.exception.EventCallException;
import pt.sapo.gis.Event.Listener;
import java.util.Collection;
import org.junit.Before;
import org.junit.Test;
import static org.junit.Assert.*;

public class EventTest {
    
    private Event<String> instance;
    private String result;
    private String otherResult;
    
    ICallback<String> callback = new ICallback<String>() {
        @Override
        public void call(String t) throws EventCallException {
            result = t;
        }
    };
    
    ICallback<String> otherCallback = new ICallback<String>() {
        @Override
        public void call(String t) throws EventCallException {
            otherResult = t;
        }
    };
    
    IEventFilter<String> filter = new IEventFilter<String>() {        
        @Override
        public Boolean test(String t) {
            return t == "go";
        }
    };
        
    @Before
    public void setUp() {
        instance = new Event<String>();
        result = "";
        otherResult = "";
    }

    /**
     * Test of getListeners method, of class Event.
     */
    @Test
    public void testGetListeners() {
        System.out.println("getListeners");       
        
        Collection listeners = instance.getListeners();
        assertNotNull(listeners);        
    }

    /**
     * Test of notify method, of class Event.
     */
    @Test
    public void testNotify() throws Exception {
        System.out.println("notify");
        instance.registerListener(callback, filter);
        
        Collection listeners = instance.getListeners();
        try {
            instance.notify("go");
            instance.notify("no go");
        } catch (Exception ex) {
            fail(ex.getMessage());
        }
        assertEquals("go", result);
    }
    
    /**
     * Test of registerListener method, of class Event.
     */
    @Test
    public void testRegisterListener_ICallback() {
        System.out.println("registerListener");
               
        instance.registerListener(callback);
        
        Collection listeners = instance.getListeners();
        try {
            instance.notify("go");
            instance.notify("no go");
        } catch (Exception ex) {
            fail(ex.getMessage());
        }
        assertEquals("no go", result);
        
        assertEquals(1, listeners.size());
        assertNull(((Listener)listeners.toArray()[0]).filter);
    }

    /**
     * Test of registerListener method, of class Event.
     */
    @Test
    public void testRegisterListener_ICallback_IEventFilter() {
        System.out.println("registerListener");
        
        instance.registerListener(callback, filter);
        
        Collection listeners = instance.getListeners();
        try {
            instance.notify("go");
            instance.notify("no go");
        } catch (Exception ex) {
            fail(ex.getMessage());
        }
        assertEquals(1, listeners.size());
        assertNotNull(((Listener)listeners.toArray()[0]).filter);
        assertEquals("go", result);        
    }

    /**
     * Test of unregisterListener method, of class Event.
     */
    @Test
    public void testUnregisterListener_ICallback() {
        System.out.println("unregisterListener");
        
        instance.registerListener(callback);
        instance.registerListener(otherCallback);
        
        boolean res = instance.unregisterListener(callback);
        
        Collection listeners = instance.getListeners();
        
        try {
            instance.notify("go");            
        } catch (Exception ex) {
            fail(ex.getMessage());
        }
        
        assertTrue(res);
        assertEquals(1, listeners.size());
        assertEquals("", result);
        assertEquals("go", otherResult);
    }

    /**
     * Test of unregisterListener method, of class Event.
     */
    @Test
    public void testUnregisterListener_ICallback_IEventFilter() {
        instance.registerListener(callback, filter);
        instance.registerListener(otherCallback, filter);
        
        instance.unregisterListener(callback, filter);
        
        Collection listeners = instance.getListeners();
        
        try {
            instance.notify("go");
        } catch (Exception ex) {
            fail(ex.getMessage());
        }
        assertEquals(1, listeners.size());
        assertEquals("", result);
        assertEquals("go", otherResult);
    }

    
}
