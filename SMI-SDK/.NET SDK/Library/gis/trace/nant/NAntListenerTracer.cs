namespace pt.sapo.gis.trace.ant
{

    using NAnt.Core;
    using System;
    using System.Configuration;
    using pt.sapo.gis.trace.configuration;

    public class NAntListenerTracer : IBuildListener
    {

        private bool autoTrace = true;

        public void BuildStarted(Object sender, BuildEventArgs be)
        {
            autoTrace = be.Project.Properties.Contains("trace.ant.disableAutoTrace") && be.Project.Properties["trace.ant.disableAutoTrace"].Equals("true", StringComparison.InvariantCultureIgnoreCase);

            TraceManager.BeginTrace();
            if (autoTrace)
                TraceManager.Trace.BeginEntry(String.Format("Start build '{0}'.", be.Project.ProjectName), severityType.INFO);
        }

        public void BuildFinished(Object sender, BuildEventArgs be)
        {
            if (autoTrace)
                TraceManager.Trace.EndEntry();
            TraceManager.EndTrace();
        }

        public void TargetStarted(Object sender, BuildEventArgs be)
        {
            TraceManager.Trace.BeginEntry(String.Format("Start target '{0}'.", be.Target.Name), severityType.INFO);
        }

        public void TargetFinished(Object sender, BuildEventArgs be)
        {
            TraceManager.Trace.EndEntry();
        }

        public void TaskStarted(Object sender, BuildEventArgs be)
        {
            TraceManager.Trace.BeginEntry(String.Format("Start task '{0}'.", be.Target.Name), severityType.INFO);
        }

        public void TaskFinished(Object sender, BuildEventArgs be)
        {
            TraceManager.Trace.EndEntry();
        }

        public void MessageLogged(Object sender, BuildEventArgs be)
        {
            TraceManager.Trace.TraceEntry(new Entry(String.Format("", be.Message), be.MessageLevel != Level.Error ? severityType.INFO : severityType.ERROR));
        }
    }
}