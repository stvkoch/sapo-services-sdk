/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.exception;

/**
 *
 * @author GIS Team
 */
public class EventCallException extends TraceSDKException {

    public EventCallException() {
    }

    public EventCallException(String message) {
        super(message);
    }

    public EventCallException(Throwable cause) {
        super(cause);
    }

    public EventCallException(String message, Throwable cause) {
        super(message, cause);
    }
    
}
