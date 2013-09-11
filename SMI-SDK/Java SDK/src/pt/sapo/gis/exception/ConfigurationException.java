/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.exception;

/**
 * Class for Configuration exceptions on trace application
 * @author GIS Tam
 */
public class ConfigurationException extends TraceSDKException{

    public ConfigurationException() {
    }

    public ConfigurationException(String message) {
        super(message);
    }

    public ConfigurationException(Throwable cause) {
        super(cause);
    }

    public ConfigurationException(String message, Throwable cause) {
        super(message, cause);
    }    
}
