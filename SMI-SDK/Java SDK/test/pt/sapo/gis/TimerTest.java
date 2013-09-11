package pt.sapo.gis;

import java.util.concurrent.TimeUnit;
import org.junit.Before;
import org.junit.Test;
import static org.junit.Assert.*;

public class TimerTest {
    
    private Timer instance;
            
    public TimerTest() {
    }
    
    @Before
    public void setUp() {
        instance = new Timer();
    }
    
    /**
     * Test of IsRunning method, of class Timer.
     */
    @Test
    public void testIsRunning() {
        System.out.println("IsRunning");

        assertEquals(false, instance.IsRunning());
        instance.Start();
        assertEquals(true, instance.IsRunning());
    }

    /**
     * Test of Start method, of class Timer.
     */
    @Test
    public void testStart() {
        System.out.println("Start");
        instance.Start();
        assertEquals(true, instance.IsRunning());
        
        for(int i=0; i<1000; ++i);
        long elapsed_1 = instance.Elapsed(TimeUnit.NANOSECONDS);
        
        assertFalse(0 == elapsed_1);
        
        instance.Start();
        long elapsed_2 = instance.Elapsed(TimeUnit.NANOSECONDS);
        
        assertTrue(elapsed_2 > elapsed_1);
        assertEquals(true, instance.IsRunning());
    }
        
    /**
     * Test of Stop method, of class Timer.
     */
    @Test
    public void testStop() {
        System.out.println("Stop");
        
        instance.Start();
        assertEquals(true, instance.IsRunning());
        
        instance.Stop();
        assertEquals(false, instance.IsRunning());        
    }

    /**
     * Test of Elapsed method, of class Timer.
     */
    @Test
    public void testElapsed() {
        System.out.println("Elapsed");
        instance.Start();
        long elapsed_1 = instance.Elapsed(TimeUnit.NANOSECONDS);
        for(int i=0; i<1000; ++i);
        instance.Stop();
        long elapsed_2 = instance.Elapsed(TimeUnit.NANOSECONDS);
        
        assertTrue(elapsed_2 > elapsed_1);
        
        long elapsed_3 = instance.Elapsed(TimeUnit.MICROSECONDS);
        
        assertTrue(elapsed_3 == (elapsed_2/1000));        
    }

    /**
     * Test of ElapsedMilis method, of class Timer.
     */
    @Test
    public void testElapsedMilis() {
        System.out.println("ElapsedMilis");        
        
        instance.Start();
        for(int i=0; i<100000; ++i);
        instance.Stop();
        
        double elapsed = instance.Elapsed(TimeUnit.NANOSECONDS)/1000000.0;
        
        double elapsedMilis = instance.ElapsedMilis();
        
        assertEquals(elapsed, elapsedMilis, 0.001);
    }
}
