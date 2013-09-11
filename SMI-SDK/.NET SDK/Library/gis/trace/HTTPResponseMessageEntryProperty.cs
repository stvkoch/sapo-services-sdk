namespace pt.sapo.gis.trace
{
    using System;
    using System.Collections.Generic;

    /**
    * @author GIS Team 
     * Entry property to wrap an HTTP response message
     */
    public class HTTPResponseMessageEntryProperty : HTTPMessageEntryProperty
    {
        public static readonly String STATUS_CODE_PROPERTY_NAME = "statusCode";
        public static readonly String REASONPHRASE_PROPERTY_NAME = "reasonPhrase";

        
        public int StatusCode
        {
            get { return (int)this[STATUS_CODE_PROPERTY_NAME]; }
            set { this.Add(STATUS_CODE_PROPERTY_NAME, value); }
        }

        public String ReasonPhrase
        {
            get { return (String)this[STATUS_CODE_PROPERTY_NAME]; }
            set { this.Add(STATUS_CODE_PROPERTY_NAME, value); }
        }
    }
}