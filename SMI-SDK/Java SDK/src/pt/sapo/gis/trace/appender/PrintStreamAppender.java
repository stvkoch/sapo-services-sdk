/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace.appender;

import java.io.PrintStream;
import pt.sapo.gis.trace.Entry;
import pt.sapo.gis.trace.Trace;

/**
 *
 * @author GIS Team
 */
public abstract class PrintStreamAppender extends Appender{
    protected  final PrintStream  oStream;
    
    public PrintStreamAppender(pt.sapo.gis.trace.configuration.Appender config, PrintStream oStream){
        super(config);
        this.oStream = oStream;
    }
            
    @Override
    protected void entryEventHandler(Entry e) {
        synchronized(oStream){
            PrintEntry(e,0);
        }
    }

    @Override
    protected void traceEventHandler(Trace t) {
        synchronized(oStream){
            PrintTrace(t);
        }        
    }
    
    protected abstract void PrintTrace(Trace t);
    
    protected abstract void PrintEntry(Entry e, int deep);
}
