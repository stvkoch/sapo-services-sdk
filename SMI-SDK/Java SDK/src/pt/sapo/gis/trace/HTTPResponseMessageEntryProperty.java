package pt.sapo.gis.trace;

/**
* @author GIS Team 
 * Entry property to wrap an HTTP response message
 */
public class HTTPResponseMessageEntryProperty extends HTTPMessageEntryProperty{
    public static final String STATUS_CODE_PROPERTY_NAME = "statusCode";
    public static final String REASON_PHRASE_CODE_PROPERTY_NAME = "reasonPhrase";
   
    public int getStatusCode() {
        return ((Integer)this.get(STATUS_CODE_PROPERTY_NAME)).intValue();
    }

    public void setStatusCode(int statusCode) {
        this.put(STATUS_CODE_PROPERTY_NAME, Integer.valueOf(statusCode));
    }
    
    public String getReasonPhrase() {
        return (String)this.get(REASON_PHRASE_CODE_PROPERTY_NAME);
    }

    public void setReasonPhrase(String reasonPhrase) {
        this.put(REASON_PHRASE_CODE_PROPERTY_NAME, reasonPhrase);
    }
}