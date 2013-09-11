namespace pt.sapo.gis.trace
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Net;
    using pt.sapo.gis.trace.configuration;
    using System.Diagnostics;
    using System.Collections;
    using System.Threading;

    /// <summary>
    /// Type that represents a trace
    /// </summary>
    public class Trace : ITrace
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Stopwatch stopwatch = Stopwatch.StartNew();

        public event GenericEventHandler<Entry> OnEntry;
        public event GenericEventHandler<Trace> OnTrace;

        public Guid Id { get; protected set; }
        public String ContextId { get; protected set; }
        public DateTime Start { get; protected set; }
        public DateTime End { get; protected set; }
        public List<Entry> Entries { get; protected set; }
        protected Stack<Entry> Context
        {
            get
            {
                if (context.IsValueCreated == false)
                    context.Value = new Stack<Entry>();
                return context.Value;
            }
        }
        protected ThreadLocal<Stack<Entry>> context = new ThreadLocal<Stack<Entry>>();

        public IPAddress ServerInfo { get; set; }
        public IPAddress ClientInfo { get; set; }
        public Summary Summary { get; set; }
        public IDictionary<String, Object> Properties { get; protected set; }

        /// <summary> 
        /// Creates a new trace     
        /// </summary>
        public Trace()
            : this(Guid.NewGuid(), null) { }

        /// <summary> 
        /// Creates a new trace with a given identifier and context identifier
        /// </summary>
        /// <param name="param name="id">Trace identifier</param>
        /// <param name="contextId">Trace context identifier</param>
        public Trace(Guid id, String contextId)
        {
            if (id == null)
            {
                id = Guid.NewGuid();
            }
            this.Id = id;
            this.ContextId = contextId;
            this.Start = DateTime.UtcNow;
            Entries = new List<Entry>();
            Properties = new Dictionary<String, Object>();
            stopwatch.Start();
        }

        /// <summary>
        /// Trace a new entry
        /// </summary>
        /// <param name="entry">entry the entry to add to the trace</param>
        public void TraceEntry(Entry entry)
        {
            entry.Parent = this;
            entry.Offset = stopwatch.ElapsedMilliseconds;
            AddEntry(entry);
        }

        private void AddEntry(Entry entry)
        {
            if (Context.Count == 0)
            {
                lock (Entries)
                {
                    Entries.Add(entry);
                }
            }
            else
            {
                Context.Peek().Entries.Add(entry);
            }
            try
            {
                if (OnEntry != null)
                {
                    lock (Entries)
                    {
                        OnEntry(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(null, ex);
            }
        }

        /// <summary>Start a new entry context. Every entry added after this will be added as sub entry of this one.</summary>
        public Entry BeginEntry()
        {
            return BeginEntry(new Entry());
        }

        /// <summary>Start a new entry context. Every entry added after this will be added as sub entry of this one.</summary>
        /// <param name="description">the new entry description</param>
        /// <param name="severity">the new entry severity type</param>

        public Entry BeginEntry(String description, severityType severity)
        {
            return BeginEntry(new Entry(description, severity));
        }

        /// <summary>Start a new entry context. Every entry added after this will be added as sub entry of this one.</summary>
        /// <param name="description">the new entry description</param>
        /// <param name="severity">the new entry severity type</param>
        /// <param name="properties">the new entry properties map</param>
        public Entry BeginEntry(String description, severityType severity, IDictionary<String, Object> properties)
        {
            return BeginEntry(new Entry(description, severity, properties));
        }

        ///
        /// <summary>Start a new entry context. Every entry added after this will be added as sub entry of this one.</summary>
        /// <param name="entry">the entry to start the new context     </param>
        public Entry BeginEntry(Entry entry)
        {
            entry.Offset = stopwatch.ElapsedMilliseconds;
            entry.Parent = this;
            Context.Push(entry);
            return entry;
        }

        /// <summary>Ends the current entry context, adding it to the trace entries list or to the parent entry if it was created inside a context</summary>
        public void EndEntry()
        {
            var current = Context.Pop();
            current.Duration = stopwatch.ElapsedMilliseconds - current.Offset;
            AddEntry(current);
        }

        /// <summary>Finishes the trace closing all the open context entries</summary>
        public void Finish()
        {
            End = DateTime.UtcNow;
            while (Context.Count > 0)
            {
                this.EndEntry();
            }
            try
            {
                if (OnTrace != null)
                    OnTrace(this);
            }
            catch (Exception ex)
            {
                log.Fatal(null, ex);
            }
        }

        /// <summary>Gets all severity types that exists on trace entries, including sub entries</summary>
        /// <returns>the severity types collection set</returns>
        public ISet<severityType> GetSeverityTypes()
        {
            var severities = new HashSet<severityType>();
            lock (Entries)
            {
                foreach (Entry e in this.Entries)
                {
                    GetEntrySeverityTypes(e, severities);
                }
            }
            return severities;
        }

        private void GetEntrySeverityTypes(Entry entry, ISet<severityType> severities)
        {
            severities.Add(entry.Severity);
            foreach (Entry e in entry.Entries)
            {
                GetEntrySeverityTypes(e, severities);
            }
        }
    }
}