namespace pt.sapo.gis.trace
{
    using System;
    using System.Collections.Generic;

    /**
    * @author GIS Team 
     * Entry property to wrap an HTTP request message
     */
    public class HTTPRequestMessageEntryProperty : HTTPMessageEntryProperty
    {
        public static readonly String URL_PROPERTY_NAME = "url";
        public static readonly String VERB_PROPERTY_NAME = "verb";
        
        public String Verb
        {
            get { return (String)this[VERB_PROPERTY_NAME]; }
            set { this.Add(VERB_PROPERTY_NAME, value); }
        }

        public Uri Url
        {
            get { return (Uri)this[URL_PROPERTY_NAME]; }
            set { this.Add(URL_PROPERTY_NAME, value); }
        }
    }
}