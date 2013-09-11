package pt.sapo.gis;

import java.util.concurrent.TimeUnit;

/**
 * @author GIS Team
 * Fine time measuring
 */
public class Timer {
    
    private boolean isRunning;  
    private long elapsedNanos;
    private long startTicks;

    
    /**
    * Creates a new timer
    */
    public Timer(){}
    
    /**
    * Returns true if counter as started,
    */
    public boolean IsRunning() {
        return isRunning;   
    }

    /**
    * Starts the counter.
    *
    * @return the timer instance
    */
    public Timer Start() {
        if(!isRunning){            
            isRunning = true;
            elapsedNanos = 0;
            startTicks = System.nanoTime();  
        }
        return this;
    }
    
    /**
    * Stops the counter. Future reads will return the fixed duration that had
    * elapsed up to this point.
    *
    * @return the timer instance    
    */
    public Timer Stop() {
        long stopTicks = System.nanoTime();  
        if(isRunning){            
            isRunning = false;
            elapsedNanos += stopTicks - startTicks;      
        }
        return this;
    }
    
    /**
     * Gets the elapsed nonoseconds since start in the given unit.
     * Convertion from nanoseconds truncate, so lose precision.
     */
    public long Elapsed(TimeUnit unit) {
        return unit.convert(ElapsedNanos(), TimeUnit.NANOSECONDS);
    }
    
    /**
     * Gets the elapsed nonoseconds since start in the given unit.     
     */
    public long ElapsedNanos() {
        return isRunning ? System.nanoTime() - startTicks : elapsedNanos;
    }
    
    /**
     * Gets the elapsed microseconds since start in the given unit,
     * with precision.
     */
    public double ElapsedMicros() {        
        return ElapsedNanos()/1000.0;
    }
    
    /**
     * Gets the elapsed milisecconds since start in the given unit,
     * with precision.
     */
    public double ElapsedMilis() {        
        return ElapsedMicros()/1000.0;
    }    
}