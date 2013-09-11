/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.exception;

/**
 *
 * @author GIS Team
 */
public class IntializatonException extends TraceSDKException {

    public IntializatonException() {
    }

    public IntializatonException(String message) {
        super(message);
    }

    public IntializatonException(Throwable cause) {
        super(cause);
    }

    public IntializatonException(String message, Throwable cause) {
        super(message, cause);
    }
    
}
