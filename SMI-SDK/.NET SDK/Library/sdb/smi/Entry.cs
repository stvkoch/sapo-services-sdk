using System;
using System.Collections.Generic;
namespace pt.sapo.sdb.smi
{

    public class Entry
    {
        public String description;
        public SeverityType severity;
        public double offset;
        public double duration;
        private List<Entry> entries;
        public IDictionary<String, Object> properties;
        public int messageNumber;
        public Exception exception;
        public String linkedTraceId;

        public List<Entry> getEntries()
        {
            if (entries == null)
                entries = new List<Entry>();
            return entries;
        }
    }
}