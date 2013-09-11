namespace pt.sapo.gis.trace
{
    using System;
    using System.Collections.Generic;
    using pt.sapo.gis.trace.configuration;

    /**
     * @author GIS Team
     * Class for entry with an message
     */
    public class MessageEntry : Entry
    {
        public static readonly String MESSAGE_PROPERTY_NAME = "message";

        public MessageEntryProperty getMessage()
        {
            return (MessageEntryProperty)Properties[MESSAGE_PROPERTY_NAME];
        }

        public void setMessage(MessageEntryProperty message)
        {
            Properties.Add(MESSAGE_PROPERTY_NAME, message);
        }

        public MessageEntry() { }

        public MessageEntry(String description, severityType severity) : base(description, severity) { }

        public MessageEntry(String description, severityType severity, IDictionary<String, Object> properties) : base(description, severity, properties) { }

    }
}