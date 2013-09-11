using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAnt.Core;
using NAnt.Core.Attributes;
using pt.sapo.gis.trace.configuration;

namespace pt.sapo.gis.trace.nant
{
    // TODO: Not finished
    public class TraceBeginEntryTask : Task
    {
        [TaskAttribute("description", Required=false)]
        public String Description { get; set; }

        [TaskAttribute("severity", Required=false)]
        public severityType Severity { get; set; }

        public IDictionary<String, Object> Properties { get; set; }

        protected override void ExecuteTask()
        {
            TraceManager.Trace.BeginEntry(Description, Severity, Properties);
        }
    }
}
