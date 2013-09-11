using System;
using System.Collections.Generic;
namespace pt.sapo.sdb.smi
{
    public class Trace
    {
        public Authentication Authentication { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int TenantId { get; set; }
        public String TraceId { get; set; }
        public String Id { get; set; }
        public String ActivityId { get; set; }
        public String ParentId { get; set; }

        public Server Server { get; set; }
        public Client Client { get; set; }
        public Service Service { get; set; }
        public Summary Summary { get; set; }

        public ICollection<Entry> Entries { get; protected set; }
        public IList<Message> Messages { get; protected set; }

        public Trace() {
            Entries = new List<Entry>();
            Messages = new List<Message>();
        }
    }
}