package pt.sapo.gis.trace;

import java.io.File;
import java.lang.reflect.InvocationTargetException;
import java.util.ArrayList;
import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;
import pt.sapo.gis.Event;
import pt.sapo.gis.ICallback;
import pt.sapo.gis.IEvent;
import pt.sapo.gis.exception.ConfigurationException;
import pt.sapo.gis.exception.EventCallException;
import pt.sapo.gis.trace.appender.Appender;
import pt.sapo.gis.trace.configuration.Configuration;

/**
 * @author GIS Team
 * Helper Static class to manage the current trace
 */
public class TraceManager{
    private static final String DEFAUL_FILE_NAME = "trace.xml";                
    
    private static final Event<Entry> onEntry = new Event<Entry>();
    private static final Event<Trace> onTrace = new Event<Trace>();
    private static final ThreadLocal<Trace> localTrace = new InheritableThreadLocal<Trace>();
    private static final ArrayList<Appender> appenders = new ArrayList<Appender>() {};
    
    private static Configuration configuration;
    
    // trace events callbacks
    private static ICallback<Entry> entryCallback = new ICallback<Entry>() {
        @Override
        public void call(Entry e) throws EventCallException {
            try {
                onEntry.notify(e);
            } catch (Exception ex) {
                throw new EventCallException(ex);
            }
        }
    };
    
    private static ICallback<Trace> traceCallback = new ICallback<Trace>() {
        @Override
        public void call(Trace t) throws EventCallException {
            try {
                onTrace.notify(t);
            } catch (Exception ex) {
                throw new EventCallException(ex);
            }
        }        
    };
        
    static{
        try {
            LoadConfig();
        } catch (ConfigurationException ex) {
            Logger.getLogger(TraceManager.class.getName()).log(Level.SEVERE, null, ex);
            throw new ExceptionInInitializerError(ex);
        }
    }

    // Private constructor => static class
    private TraceManager() {
        throw new AssertionError();
       // TraceManager
    }
    
    private static void LoadConfig() throws ConfigurationException {
        try {
            String logConfigFileName = System.getProperty("trace.configFile", DEFAUL_FILE_NAME);
            
            // Load Configuration        
            File file = new File(logConfigFileName);        
            JAXBContext jaxbContext = JAXBContext.newInstance(Configuration.class);
            Unmarshaller jaxbUnmarshaller = jaxbContext.createUnmarshaller();
            configuration = (Configuration) jaxbUnmarshaller.unmarshal(file);
                    
            if(configuration.getAppenders() != null && configuration.getAppenders() != null){
                for(pt.sapo.gis.trace.configuration.Appender appenderConfig : configuration.getAppenders().getList()){
                    String type = appenderConfig.getType();                
                    Class[] args = new Class[1];
                    args[0] = pt.sapo.gis.trace.configuration.Appender.class;
                    Appender appender = (Appender)Class.forName(type).getConstructor(args).newInstance(appenderConfig);
                    appenders.add(appender);
                }
            }
        } catch (InstantiationException ex) {
            throw new ConfigurationException("An error occurred on appender object creation", ex.getCause());
        } catch (IllegalAccessException ex) {
            throw new ConfigurationException("An error occurred on appender object creation", ex.getCause());
        } catch (IllegalArgumentException ex) {
            throw new ConfigurationException("An error occurred on appender object creation", ex.getCause());
        } catch (InvocationTargetException ex) {
            throw new ConfigurationException("An error occurred on appender object creation", ex.getCause());
        } catch (NoSuchMethodException ex) {
            throw new ConfigurationException("An error occurred on appender object creation", ex.getCause());
        } catch (SecurityException ex) {
            throw new ConfigurationException("An error occurred on appender object creation", ex.getCause());
        } catch (ClassNotFoundException ex) {
            throw new ConfigurationException("An error occurred on appender object creation", ex.getCause());
        } catch (JAXBException ex) {
            throw new ConfigurationException("An error occurred on configuration object creation", ex.getCause());
        }
         
    }
    
    public static IEvent<Entry> getOnEntryEvent() { 
        return onEntry; 
    }
    
    public static IEvent<Trace> getOnTraceEvent() { 
        return onTrace; 
    }
    public static Trace BeginTrace(String requestId, String context){
        Trace trace = new Trace(requestId, context);
        trace.getOnEntryEvent().registerListener(entryCallback);
        trace.getOnTraceEvent().registerListener(traceCallback);
        localTrace.set(trace);
        return trace;
    }
    
    public static Trace BeginTrace(){
       return BeginTrace(UUID.randomUUID().toString(), null); 
    }
    
    public static Trace GetTrace(){
        return localTrace.get();
    }
    
    public static void EndTrace(){
        EndTrace(true, null);
    }
    
    public static void EndTrace(boolean sucess){        
        EndTrace(sucess, null);
    }
    
    public static void EndTrace(boolean sucess, String result){        
        Trace trace = localTrace.get();
        trace.setProperty(SummaryTraceProperty.SUMMARY_TRACE_PROPERTY_NAME, new SummaryTraceProperty(sucess, result));
        trace.finish();
        localTrace.remove();
    }    
}