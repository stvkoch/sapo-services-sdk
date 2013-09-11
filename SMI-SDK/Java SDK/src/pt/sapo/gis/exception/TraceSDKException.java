/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.exception;

/**
 * @author GIS Team
 *
 * Base class for trace SDK aplication exceptions
 */
public abstract class TraceSDKException extends Exception {

    public TraceSDKException() {
        super();
    }

    public TraceSDKException(String message) {
        super(message);
    }

    public TraceSDKException(Throwable cause) {
        super(cause);
    }

    public TraceSDKException(String message, Throwable cause) {
        super(message, cause);
    }    
}
