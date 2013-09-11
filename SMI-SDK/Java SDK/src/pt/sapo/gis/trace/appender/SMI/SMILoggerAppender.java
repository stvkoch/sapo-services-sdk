package pt.sapo.gis.trace.appender.SMI;

import java.util.Date;
import java.util.EnumMap;
import java.util.GregorianCalendar;
import java.util.TimeZone;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.xml.datatype.DatatypeConfigurationException;
import javax.xml.datatype.DatatypeFactory;
import javax.xml.datatype.XMLGregorianCalendar;
import org.tmforum.schemas.smi.ExecutionState;
import org.tmforum.schemas.smi.Failure;
import org.tmforum.schemas.smi.Failures;
import org.tmforum.schemas.smi.HealthState;
import org.tmforum.schemas.smi.ManagementReport;
import org.tmforum.schemas.smi.State;

import pt.sapo.gis.trace.Entry;
import pt.sapo.gis.trace.EntryType;

import pt.sapo.gis.trace.SeverityEventFilter;
import pt.sapo.gis.trace.Trace;
import pt.sapo.gis.trace.appender.Appender;
import pt.sapo.sdb.trace.FailureCode;

public abstract class SMILoggerAppender extends Appender{
    public static final String REPORT_ACTIVITY_ID_PROPERTY = "activityId";
    public static final String REPORT_SOURCE_ID_PROPERTY = "sourceId";
    public static final String FAILURE_SOURCE_ID_PROPERTY = "sourceId";

    private static final EnumMap<EntryType, FailureCode> codeMapping = new EnumMap<EntryType, FailureCode>(EntryType.class);
        
    protected SMILoggerAppenderConfigProperties properties;
    
    public SMILoggerAppender(pt.sapo.gis.trace.configuration.Appender config){
        super(config);
        properties = new SMILoggerAppenderConfigProperties(config.getProperties());
    }
    
    @Override
    protected void entryEventHandler(Entry e) {
        Failures failures = new Failures();
        FillFailure(e, failures, entrySeverityFilter);
        
        ManagementReport report = BuildReport(e.getParent(), failures);
        send(report);
    }

    @Override
    protected void traceEventHandler(Trace t) {        
        Failures failures = new Failures();
        for(Entry e : t.getEntries())
            FillFailures(e, failures, traceSeverityFilter);            
        
        ManagementReport report = BuildReport(t, failures);
        send(report);
    }
    
    private ManagementReport BuildReport(Trace trace, Failures failures){
        ManagementReport report = new ManagementReport();
        
        report.setSourceID(getReportSourceId(trace));
        report.setID(trace.getId());
        
        //report.setActivityID = (String)t.getProperty(REPORT_ACTIVITY_ID_PROPERTY)
        
        State state = new State();
        state.setExecution(ExecutionState.ACTIVE);
        state.setHealth(HealthState.OPERATIONAL_WITH_FAILURES);
        state.setFailures(failures);
        report.setState(state);
        
        GregorianCalendar cal = new GregorianCalendar();
        cal.setTime(new Date());
        cal.setTimeZone(TimeZone.getTimeZone("GMT"));
        XMLGregorianCalendar dateTime = null;
        try {
            dateTime = DatatypeFactory.newInstance().newXMLGregorianCalendar(cal);
        } catch (DatatypeConfigurationException ex) {
            Logger.getLogger(SMILoggerAppender.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        report.setDateTime(dateTime);
        //report.setMetrics(null);
        
        return report;
    }

    private String GetFailureIdFromEntry(Entry e){
        if(e.getType() != null){            
            return e.getType().toString();
        }

        return FailureCode.UNKNOWN.name();
    }
    
    private void FillFailure(Entry entry, Failures  failures, SeverityEventFilter filter) {
        // only add entry if there is no filter or it pass the filter test
        if(filter == null || filter.contains(entry.getSeverity())){
            Failure failure = new Failure();
            failure.setFailureID(GetFailureIdFromEntry(entry));
            
            failure.setSourceID(getFailureSourceId(entry));
            if(entry.getProperties() != null && !entry.getProperties().isEmpty())
                failure.setDetails(entry.getProperties());
            failures.getFailure().add(failure);
        }
    }
    
    private void FillFailures(Entry entry, Failures failures, SeverityEventFilter filter) {
        FillFailure(entry, failures, filter);
        // fill child entries
        for(Entry innerEntry : entry.getEntries())
            FillFailures(innerEntry, failures, filter);
    }

    private String getReportSourceId(Trace t){
        Object sourceIdProp = t.getProperty(REPORT_SOURCE_ID_PROPERTY);
        if(sourceIdProp != null)       
            return (String)sourceIdProp;
        return properties.getDefaultSourceID();
    }
    
    private String getFailureSourceId(Entry e){
        Object sourceIdProp = e.getProperty(FAILURE_SOURCE_ID_PROPERTY);
        if(sourceIdProp != null)       
            return (String)sourceIdProp;
        return getReportSourceId(e.getParent());
    }
    
    protected abstract void send(ManagementReport report);   
}