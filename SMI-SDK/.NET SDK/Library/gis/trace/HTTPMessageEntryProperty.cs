using System;
using System.Collections.Generic;
namespace pt.sapo.gis.trace
{

    /**
    * @author GIS Team 
     * Entry property to wrap an HTTP message
     */
    public class HTTPMessageEntryProperty : MessageEntryProperty
    {
        public static readonly String HEADERS_PROPERTY_NAME = "headers";
        public static readonly String BODY_PROPERTY_NAME = "body";

        public IDictionary<String, String> Headers
        {
            get { return (IDictionary<String, String>)this[HEADERS_PROPERTY_NAME]; }
            set { this[HEADERS_PROPERTY_NAME] = value; }
        }

        public String Body
        {
            get { return (String)this[BODY_PROPERTY_NAME]; }
            set { this.Add(BODY_PROPERTY_NAME, value); }
        }
    }
}