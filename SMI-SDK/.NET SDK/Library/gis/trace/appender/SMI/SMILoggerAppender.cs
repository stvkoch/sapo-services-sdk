namespace pt.sapo.gis.trace.appender.SMI
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using pt.sapo.sdb;
    using log4net.Core;
    using pt.sapo.sdb.smi;
    using pt.sapo.gis.trace.configuration;


    public abstract class SMILoggerAppender : Appender
    {
        public static readonly String REPORT_ACTIVITY_ID_PROPERTY = "activityId";
        public static readonly String REPORT_AGENT_ID_PROPERTY = "agentId";
        public static readonly String REPORT_SOURCE_ID_PROPERTY = "sourceId";
        public static readonly String FAILURE_SOURCE_ID_PROPERTY = "sourceId";

        private delegate bool EntryFilterHandler(pt.sapo.gis.trace.Entry e);

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public new SMILoggerAppenderConfigProperties Properties { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="IntializatonException"></exception>
        public SMILoggerAppender(pt.sapo.gis.trace.configuration.appender config) : base(config) {
            this.Properties = new SMILoggerAppenderConfigProperties(base.Properties);
            base.Properties = this.Properties;
        }

        private String GetFailureCodeFromEntry(pt.sapo.gis.trace.Entry e)
        {
            return e.Type.ToString();            
        }

        public override void OnEntry(trace.Entry e)
        {
            var failures = new List<Failure>();
            FillFailure(e, failures, et => severities.ContainsKey(eventType.entry) == false || severities[eventType.entry].Contains(et.Severity));
            ManagementReport report = BuildReport(e.Parent, failures);
            Send(report, e.Parent.Properties.ContainsKey(REPORT_ACTIVITY_ID_PROPERTY) ? e.Parent.Properties[REPORT_ACTIVITY_ID_PROPERTY] as String : null, e.Parent.Properties.ContainsKey(REPORT_AGENT_ID_PROPERTY) ? e.Parent.Properties[REPORT_AGENT_ID_PROPERTY] as String : null);
        }

        public override void OnTrace(trace.Trace t)
        {
            var failures = new List<Failure>();
            foreach (trace.Entry e in t.Entries)
                FillFailures(e, failures, et => severities.ContainsKey(eventType.trace) == false || severities[eventType.trace].Contains(et.Severity));

            ManagementReport report = BuildReport(t, failures.ToArray());
            Send(report, t.Properties.ContainsKey(REPORT_ACTIVITY_ID_PROPERTY) ? t.Properties[REPORT_ACTIVITY_ID_PROPERTY] as String : null, t.Properties.ContainsKey(REPORT_AGENT_ID_PROPERTY) ? t.Properties[REPORT_AGENT_ID_PROPERTY] as String : null);
        }

        private ManagementReport BuildReport(trace.Trace trace, IEnumerable<Failure> failures)
        {
            ManagementReport report = new ManagementReport
            {
                ID = trace.Id.ToString(),
                State = new State()
                {
                    Execution = ExecutionState.Active,
                    Health = HealthState.OperationalWithFailures,
                    Failures = failures.ToArray()
                },
                DateTime = DateTime.UtcNow,
                SourceID = trace.Properties.ContainsKey(REPORT_SOURCE_ID_PROPERTY) ? trace.Properties[REPORT_SOURCE_ID_PROPERTY] as String : Properties.DefaultSourceId
            };

            return report;            
        }

        protected abstract void Send(ManagementReport report, String activityId, String agentId);

        private void FillFailure(pt.sapo.gis.trace.Entry entry, List<Failure> failures, EntryFilterHandler filter)
        {
            // only add entry if there is no filter or it pass the filter test
            if (filter(entry))
            {
                Failure failure = new Failure
                {
                    FailureID = GetFailureCodeFromEntry(entry),
                    SourceID = entry.Properties.ContainsKey(FAILURE_SOURCE_ID_PROPERTY) ? entry.Properties[FAILURE_SOURCE_ID_PROPERTY] as String : (TraceManager.Trace.Properties.ContainsKey(REPORT_SOURCE_ID_PROPERTY) ? TraceManager.Trace.Properties[REPORT_SOURCE_ID_PROPERTY] as String : Properties.DefaultSourceId),
                    Details = entry.Properties
                };
                failures.Add(failure);
            }
        }

        private void FillFailures(trace.Entry entry, List<Failure> failures, EntryFilterHandler filter)
        {
            FillFailure(entry, failures, filter);
            // fill child entries
            foreach (trace.Entry innerEntry in entry.Entries)
                FillFailures(innerEntry, failures, filter);
        }        
    }
}