namespace pt.sapo.gis.trace
{
    using System;
    using System.Linq;
    using System.Threading;
    using pt.sapo.gis.trace.configuration;
    using pt.sapo.gis.exception;
    using System.Configuration;
    using System.IO;
    using System.Xml.Serialization;
    using pt.sapo.gis.trace.appender;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /**
     * @author GIS Team
     * Helper Static class to manage the current trace
     */
    public static class TraceManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static event GenericEventHandler<Entry> OnEntry;
        public static event GenericEventHandler<Trace> OnTrace;

        private static readonly ThreadLocal<Trace> localTrace = new ThreadLocal<Trace>();
        private static ICollection<Appender> Appenders { get; set; }
        private static bool ConfigurationLoaded = false;

        static TraceManager()
        {
            Appenders = new List<Appender>();
            LoadConfig();
        }

        public static void LoadConfig()
        {
            lock (Appenders)
            {
                if (ConfigurationLoaded == false)
                {
                    ConfigurationLoaded = true;

                    try
                    {
                        // For tests purposes
                        OnEntry = null;
                        OnTrace = null;

                        var envConfigFile = Environment.GetEnvironmentVariable("TRACE_CONFIG_FILE", EnvironmentVariableTarget.Process);

                        var configuration = envConfigFile != null ?
                            (configuration.configuration)new XmlSerializer(typeof(trace.configuration.configuration)).Deserialize(new FileStream(envConfigFile, FileMode.Open)) :
                            (configuration.configuration)ConfigurationManager.GetSection("trace");

                        var waitedForAll = Task.WaitAll(
                            (from appenderConfig in configuration.appenders
                             select Task.Factory.StartNew(() =>
                             {
                                 try
                                 {
                                     Appender appender = (Appender)Type.GetType(appenderConfig.type)
                                         .GetConstructor(new Type[] { typeof(pt.sapo.gis.trace.configuration.appender) })
                                         .Invoke(new Object[] { appenderConfig });
                                     lock (Appenders)
                                     {
                                         Appenders.Add(appender);
                                     }
                                 }
                                 catch (Exception ex)
                                 {
                                     System.Diagnostics.Trace.TraceError(String.Format("An error occurred on loading trace configuration.{0}{1}{0}{2}", Environment.NewLine, ex.Message, ex.StackTrace));
                                 }
                             })
                             ).ToArray(),
                            TimeSpan.FromSeconds(1));
                        if (waitedForAll == false)
                        {
                            System.Diagnostics.Trace.TraceWarning("Service start without all Trace appenders initialized.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new pt.sapo.gis.exception.ConfigurationException("An error occurred on loading trace configuration.", ex);
                    }
                }
                
            }
        }

        public static Trace BeginTrace(Guid requestId, String context)
        {
            Trace trace = new Trace(requestId, context);
            trace.OnEntry += e =>
            {
                if (OnEntry != null)
                    OnEntry(e);
            };
            trace.OnTrace += t =>
            {
                if (OnTrace != null)
                    OnTrace(t);
            };
            Trace = trace;
            return trace;
        }

        public static Trace BeginTrace()
        {
            return BeginTrace(Guid.NewGuid(), null);
        }

        public static Trace BeginTrace(String context)
        {
            return BeginTrace(Guid.NewGuid(), context);
        }

        public static Trace Trace
        {
            get { return localTrace.Value; }
            set { localTrace.Value = value; }
        }

        public static void EndTrace()
        {
            if (Trace != null)
            {
                Trace.Finish();
            }
        }
    }
}