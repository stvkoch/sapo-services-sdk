namespace pt.sapo.gis.trace
{
    using System;
    using pt.sapo.gis.trace.configuration;
    using System.Collections.Generic;

    /**
     * @author GIS Team
     * Class for entry with an exception
     */
    public class ExceptionEntry : Entry
    {
        public static readonly String EXCEPTION_PROPERTY = "exception";

        public Exception Exception
        {
            get { return ((ExceptionEntryProperty)Properties[EXCEPTION_PROPERTY]).Exception; }
            protected set { Properties.Add(EXCEPTION_PROPERTY, new ExceptionEntryProperty(value)); }
        }

        public ExceptionEntry() { }

        public ExceptionEntry(String description, severityType severity, Exception exception) : base(description, severity) {
            Exception = exception;
        }

        public ExceptionEntry(String description, severityType severity, IDictionary<String, Object> properties, Exception exception) : base(description, severity, properties) {
            Exception = exception;
        }
    }
}