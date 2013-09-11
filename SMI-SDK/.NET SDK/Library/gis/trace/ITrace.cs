using System;
using pt.sapo.gis.trace.configuration;
using System.Collections.Generic;
namespace pt.sapo.gis.trace
{

    /**
    * @author GIS Team 
     * Trace interface
     */
    public interface ITrace
    {
        void TraceEntry(Entry entry);
        Entry BeginEntry();
        Entry BeginEntry(String description, severityType severity);
        Entry BeginEntry(String description, severityType severity, IDictionary<String, Object> properties);
        Entry BeginEntry(Entry entry);
        void EndEntry();
        void Finish();
        ISet<severityType> GetSeverityTypes();
    }
}
