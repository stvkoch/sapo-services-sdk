package pt.sapo.gis.trace.ant;

import org.apache.tools.ant.BuildEvent;
import org.apache.tools.ant.BuildListener;
import org.apache.tools.ant.Project;
import pt.sapo.gis.trace.Entry;

import pt.sapo.gis.trace.TraceManager;
import pt.sapo.gis.trace.configuration.SeverityType;

public class AntListenerTracer implements BuildListener {

    private boolean autoTrace = true;
    
    public AntListenerTracer() {
        if("true".equals(System.getProperty("trace.ant.disableAutoTrace")))
            autoTrace = false;
    }    
    
    @Override
    public void buildStarted(BuildEvent be) {
        TraceManager.BeginTrace();
        if(autoTrace)
            TraceManager.GetTrace().beginEntry(String.format("Start build '%'.", be.getProject().getName()), SeverityType.INFO);
    }

    @Override
    public void buildFinished(BuildEvent be) {
        if(autoTrace)
            TraceManager.GetTrace().endEntry();
        TraceManager.EndTrace();
    }

    @Override
    public void targetStarted(BuildEvent be) {
        TraceManager.GetTrace().beginEntry(String.format("Start target '%'.", be.getTarget().getName()), SeverityType.INFO);
    }

    @Override
    public void targetFinished(BuildEvent be) {
        TraceManager.GetTrace().endEntry();
    }

    @Override
    public void taskStarted(BuildEvent be) {
        TraceManager.GetTrace().beginEntry(String.format("Start task '%'.", be.getTarget().getName()), SeverityType.INFO);
    }

    @Override
    public void taskFinished(BuildEvent be) {
        TraceManager.GetTrace().endEntry();
    }

    @Override
    public void messageLogged(BuildEvent be) {
        TraceManager.GetTrace().trace(new Entry(String.format("", be.getMessage()), be.getPriority() != Project.MSG_ERR ? SeverityType.INFO : SeverityType.ERROR));
    }
    
}
