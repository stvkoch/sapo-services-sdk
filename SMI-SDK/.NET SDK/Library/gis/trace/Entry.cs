namespace pt.sapo.gis.trace
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using pt.sapo.gis.trace.configuration;    

    /**
     * @author GIS Team
     * Entry class that represents a trace entry
    */
    public class Entry
    {
        public Trace Parent { get; internal set; }
        public String Type { get; set; }
        public String Description { get; set; }
        public severityType Severity { get; set; }
        public double Offset { get; set; }
        public double Duration { get; set; }
        public List<Entry> Entries { get; protected set; }
        public IDictionary<String, Object> Properties { get; set; }

        /**
         * Create a new entry
        */
        public Entry() : this(null, severityType.INFO, new Dictionary<String, Object>()) {}

        /**
         * Create a new entry
         *  @param  description the entry description
         *  @param  severity the entry severity enum value
        */
        public Entry(String description, severityType severity) : this(description, severity, new Dictionary<String, Object>()) { }

        /**
         * Create a new entry
         * @param description the entry description
         * @param severity the entry severity enum value
         * @param properties the entry properties
        */
        public Entry(String description, severityType severity, IDictionary<String, Object> properties)
        {
            this.Description = description;
            this.Severity = severity;
            this.Properties = properties;
            this.Entries = new List<Entry>();
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;
            var entry = obj as Entry;
            return entry != null && Equals(entry);
        }

        public bool Equals(Entry e)
        {
            return
                Type == e.Type &&
                Description == e.Description &&
                Offset == e.Offset &&
                Duration == e.Duration &&
                Entries.Intersect(e.Entries).Count() == Entries.Count &&
                Properties.Intersect(e.Properties).Count() == Properties.Count;
        }
                
        public override int GetHashCode()
        {
            return base.GetHashCode() + Type.GetHashCode() + Description.GetHashCode() + Offset.GetHashCode() + Duration.GetHashCode() + Entries.GetHashCode() + Properties.GetHashCode();
        }
    }
}