
namespace pt.sapo.gis.trace.appender.SMI
{
    using System;
    using System.Linq;
    using pt.sapo.sdb.smi;
    using System.Collections.Generic;
    using SapoBrokerClient;
    using System.IO;
    using pt.sapo.gis.exception;

    public class BrokerSMILoggerAppender : SMILoggerAppender
    {
        public new BrokerSMILoggerAppenderConfigProperties Properties { get; protected set; }

        private BrokerClient brokerClient;
        private String topic;

        public BrokerSMILoggerAppender(pt.sapo.gis.trace.configuration.appender config)
            : base(config)
        {
            Properties = new BrokerSMILoggerAppenderConfigProperties(base.Properties);
            base.Properties = Properties;
            try
            {
                this.brokerClient = new BrokerClient(new HostInfo(Properties.Host, Properties.Port));
            }
            catch (Exception ex)
            {
                throw new IntializationException("Error initialize sapo broker client", ex);
            }
            this.topic = Properties.Topic;
        }

        protected override void Send(ManagementReport report, String activityId, String agentId)
        {
            var reportJson = new Dictionary<String, Object>{
                {"ID", report.ID},
                {"SourceID", report.SourceID },
                {"DateTime", report.DateTime},
                {"State", new Dictionary<String, Object> {
                        {"Health", report.State.Health.ToString() },
                        {"Execution", report.State.Execution.ToString()},
                        {"Failures", (from f in report.State.Failures
                              select new Dictionary<String, Object> {
                                   {"FailureID", f.FailureID},
                                   {"SourceID", f.SourceID},
                                   {"Details", f.Details}
                              }).ToArray()}
                }},
                {"Metrics", report.Metrics != null ? (from m in report.Metrics
                              select new Dictionary<String, Object> {
                                   {"MetricID", m.MetricID},
                                   {"SourceID", m.SourceID},
                                   {"Reference", m.Reference},
                                   {"Value", m.Value},
                                   {"Details", m.Details},
                                   {"CategoryID", m.CategoryID},
                                   {"DateTime", m.DateTime }
                              }).ToArray() : null},
                {"ActivityID", activityId},
                {"AgentID", agentId},
                };
            
            var str = new StringWriter();
            var jsonSer = new Newtonsoft.Json.JsonSerializer();
            jsonSer.Serialize(str, reportJson);
            NetBrokerMessage brokerMessage = new NetBrokerMessage(str.ToString());
            this.brokerClient.Publish(brokerMessage, this.topic);
        }        

    }
}