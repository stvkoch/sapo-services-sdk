package pt.sapo.gis.trace;

import java.io.PrintWriter;
import java.io.StringWriter;

/**
* @author GIS Team 
 * Entry property to wrap an exception 
 */
public class ExceptionEntryProperty extends EntryProperty {
    
    private Exception exception;

    public Exception getException() {
        return exception;
    }
    
    public ExceptionEntryProperty(Exception e) {
        this.exception = e;
        this.put("exceptionMessage", e.getMessage());
        StringWriter sw = new StringWriter();
        e.printStackTrace(new PrintWriter(sw));
        this.put("exceptionStackTrace", sw.toString());
    }    
}
