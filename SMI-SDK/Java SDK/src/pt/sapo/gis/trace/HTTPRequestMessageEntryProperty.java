package pt.sapo.gis.trace;

/**
* @author GIS Team 
 * Entry property to wrap an HTTP request message
 */
public class HTTPRequestMessageEntryProperty extends HTTPMessageEntryProperty{
    public static final String PATH_AND_QUERY_PROPERTY_NAME = "pathAndQuery";
    public static final String VERB_PROPERTY_NAME = "verb";
    public static final String SECURE_PROPERTY_NAME = "secure";
    
   public String getPathAndQuery() {
        return (String)this.get(PATH_AND_QUERY_PROPERTY_NAME);
    }

    public void setPathAndQuery(String pathAndQuery) {
        this.put(PATH_AND_QUERY_PROPERTY_NAME, pathAndQuery);
    }

    public boolean isSecure() {
        return ((Boolean)this.get(SECURE_PROPERTY_NAME)).booleanValue();
    }

    public void setSecure(boolean secure) {
        this.put(SECURE_PROPERTY_NAME, Boolean.valueOf(secure));
    }

    public String getVerb() {
        return (String)this.get(VERB_PROPERTY_NAME);
    }

    public void setVerb(String verb) {
        this.put(VERB_PROPERTY_NAME, verb);
    }
}