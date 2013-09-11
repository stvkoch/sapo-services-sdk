package pt.sapo.gis.trace.appender;

import pt.sapo.gis.trace.Entry;
import pt.sapo.gis.trace.Trace;

public class ConsoleAppender extends PrintStreamAppender {
    
    public ConsoleAppender(pt.sapo.gis.trace.configuration.Appender config){
        super(config, System.out);
    }              
    
    @Override
    protected void PrintEntry(Entry e, int deep){
        oStream.print("\n");
        for(int i=0; i<deep; ++i )  
            oStream.print("\t");
        oStream.printf(".: %s ;%.3f ms; %s", e.getDescription(), e.getDuration(), e.getSeverity());
        if(e.getProperties() != null)
            oStream.printf(e.getProperties().toString());
        for(Entry s : e.getEntries()){
            PrintEntry(s,deep+1);   
        }
    }

    @Override
    protected void PrintTrace(Trace t) {
        oStream.printf("\n--------------\n Trace from %s to %s \n entries:", t.getStart().toString(), t.getEnd().toString());
            for(Entry e : t.getEntries()){
                PrintEntry(e,0);
            }
            oStream.print("\n--------------\n");
    }

}
