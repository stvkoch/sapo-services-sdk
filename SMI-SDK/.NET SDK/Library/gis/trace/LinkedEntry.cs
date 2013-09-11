namespace pt.sapo.gis.trace
{
    using System;
    using System.Collections.Generic;
    using pt.sapo.gis.trace.configuration;

    /**
     * @author GIS Team
     * Class for entry with an linked trace Id
     */
    public class LinkedEntry : Entry
    {
        public static readonly String LINKED_TRACE_ID_PROPERTY = "linkedTraceId";

        public String LinkedTraceId
        {
            get
            {
                return (String)Properties[LINKED_TRACE_ID_PROPERTY];
            }
            set
            {
                Properties.Add(LINKED_TRACE_ID_PROPERTY, value);
            }
        }

        public LinkedEntry() { }

        public LinkedEntry(String description, severityType severity) : base(description, severity) { }

        public LinkedEntry(String description, severityType severity, IDictionary<String, Object> properties) : base(description, severity, properties) { }
    }
}