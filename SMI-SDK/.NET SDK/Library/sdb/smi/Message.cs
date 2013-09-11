namespace pt.sapo.sdb.smi
{
    using System.Collections;
    using System;
    using pt.sapo.gis.trace;
    using System.Collections.Generic;

    public class Message
    {
        public String Body { get; set; }
        public IDictionary<String, String> Headers { get; set; }
        public IDictionary<String, String> Request { get; set; }
        public IDictionary<String, String> Response { get; set; }
    }
}