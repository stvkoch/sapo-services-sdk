package pt.sapo.gis.trace.appender.SMI;

import com.google.gson.Gson;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.net.InetAddress;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import org.apache.commons.codec.binary.Base64;
import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.protocol.HTTP;

import pt.sapo.gis.exception.EventCallException;
import pt.sapo.gis.trace.Entry;
import pt.sapo.gis.trace.ExceptionEntry;
import pt.sapo.gis.trace.HTTPMessageEntryProperty;
import pt.sapo.gis.trace.HTTPRequestMessageEntryProperty;
import pt.sapo.gis.trace.HTTPResponseMessageEntryProperty;
import pt.sapo.gis.trace.LinkedEntry;
import pt.sapo.gis.trace.MessageEntry;
import pt.sapo.gis.trace.MessageEntryProperty;
import pt.sapo.gis.trace.SummaryTraceProperty;
import pt.sapo.gis.trace.Trace;
import pt.sapo.gis.trace.appender.Appender;
import pt.sapo.sdb.trace.Client;
import pt.sapo.sdb.trace.Server;
import pt.sapo.sdb.trace.TraceContext;

/**
 *@author GIS Team
 * Appender to send traces to SAPO SDB trace service
 */
public class SDBTraceAppender extends Appender{

    private final SDBTraceAppenderConfigProperties properties;
            
    public SDBTraceAppender(pt.sapo.gis.trace.configuration.Appender config){
        super(config);
        properties = new SDBTraceAppenderConfigProperties(config.getProperties());          
    }
    
    /**
     * Entry events callbacks
     */ 
    @Override
    public void entryEventHandler(Entry e) {}

    /** 
     * Trace events callbacks
     */
    @Override
    public void traceEventHandler(Trace t) throws EventCallException {
        HttpPost request;
        HttpResponse response;
        
       if(t.getContextId() != null){
            try {
                pt.sapo.sdb.trace.Trace trace = toSDBTrace(t);
                              
                
                Gson gson = new Gson();
                String json = gson.toJson(trace);

                DefaultHttpClient  httpClient = new  DefaultHttpClient();

                URIBuilder builder = new URIBuilder();
                builder.setScheme(this.properties.getProtocol())
                         .setHost(this.properties.getHost())                         
                         .setPath(this.properties.getPath())                         
                         .setParameter("traceContext", t.getContextId());
                if(this.properties.getPort() != null)
                     builder.setPort(this.properties.getPort());

                URI uri;
               
                uri = builder.build();
                request = new HttpPost(uri);

                request.setHeader("Authorization", String.format("ESB Token=\"%s\"", this.properties.getToken()));
                request.setHeader(HTTP.CONTENT_TYPE, "application/json");

                StringEntity entity = new StringEntity( json);                
                request.setEntity(entity);

                response = httpClient.execute(request);
                
                System.out.println(" ============= SDBTraceAppender ==============");
                System.out.printf("Response: %s\n", response);
                
                byte[] bytes = Base64.decodeBase64(t.getContextId());
                String traceContextStr = new String(bytes, "UTF-8");
                TraceContext traceContext = gson.fromJson(traceContextStr, TraceContext.class);                
                System.out.printf("https://backoffice.services.bk.sapo.pt/Trace/%s\n", traceContext.LinkedTraceId);
            } catch (IOException ex) {
                throw new EventCallException("Error sending trace to SDB Trace service", ex);            
            } catch (URISyntaxException ex) {
                throw new EventCallException("Error creating URI for SDB Trace service", ex);
            }
        }       
    }
    
    private pt.sapo.sdb.trace.Trace toSDBTrace(Trace t){
        pt.sapo.sdb.trace.Trace trace = new pt.sapo.sdb.trace.Trace();        
        trace.start = t.getStart();
        trace.end = t.getEnd();
        
        InetAddress serverInfo = t.getServerInfo();
        if(serverInfo != null){
            trace.Server = new Server(serverInfo.getHostName(), serverInfo.getHostAddress());
        }
        
        InetAddress clientInfo = t.getClientInfo();
        if(clientInfo != null){
            trace.Client = new Client(clientInfo.getHostAddress());
        }
                
        SummaryTraceProperty summary = (SummaryTraceProperty)t.getProperty(SummaryTraceProperty.SUMMARY_TRACE_PROPERTY_NAME);
        if(summary != null){
            pt.sapo.sdb.trace.Summary sum = new pt.sapo.sdb.trace.Summary();
            sum.success = summary.sucess;
            sum.result = summary.result;
            trace.Summary = sum;
        }        
        
        pt.sapo.sdb.trace.Entry entry;
        for(Entry e : t.getEntries()){
            entry = toSDBEntry(e, trace.messages);
            trace.entries.add(entry);
        }
        return trace;
    }

    private pt.sapo.sdb.trace.Entry toSDBEntry(Entry e, List<pt.sapo.sdb.trace.Message> messages) {
        pt.sapo.sdb.trace.Entry entry = new pt.sapo.sdb.trace.Entry();
        entry.Description = e.getDescription();
        entry.Severity = pt.sapo.sdb.trace.SeverityType.valueOf(pt.sapo.sdb.trace.SeverityType.class, e.getSeverity().name());
        entry.Offset = e.getOffset();
        entry.Duration = e.getDuration();
        entry.Properties = new HashMap<String, Object>(e.getProperties());
        
        MessageEntryProperty m = (MessageEntryProperty) entry.Properties.remove(MessageEntry.MESSAGE_PROPERTY_NAME);
        if(m != null){
            pt.sapo.sdb.trace.Message message = toSDBMessage(m);
            messages.add(message);
            entry.MessageNumber = messages.indexOf(message);
        }
        
        Exception ex = (Exception) entry.Properties.remove(ExceptionEntry.EXCEPTION_PROPERTY_NAME);
        if(ex != null){
            pt.sapo.sdb.trace.Exception exception = new pt.sapo.sdb.trace.Exception();
            exception.Type = ex.getClass().toString();
            StringWriter sw = new StringWriter();
            PrintWriter writer = new PrintWriter(sw);
            ex.printStackTrace(writer);            
            exception.StackTrace = writer.toString();
            
            entry.Exception = exception;
            
        }
        
        String link = (String) entry.Properties.remove(LinkedEntry.LINKED_TRACE_ID_PROPERTY);
        if(link != null){
            entry.LinkedTraceId = link;
        }
        if(entry.Properties.isEmpty())
            entry.Properties = null;
        
        pt.sapo.sdb.trace.Entry innerEntry;
        for(Entry ie : e.getEntries()){
            innerEntry = toSDBEntry(ie, messages);
            entry.Entries.add(innerEntry);
        }
        return entry;
    }
    
    private pt.sapo.sdb.trace.Message toSDBMessage(MessageEntryProperty msg){
        pt.sapo.sdb.trace.Message message = new pt.sapo.sdb.trace.Message();
        
        HTTPMessageEntryProperty httpMsg = msg.as(HTTPMessageEntryProperty.class);
        if(httpMsg != null){
            // fill headers
            message.Headers = httpMsg.getHeaders();
            
            // fill body
            // TODO: validate string encoding.
            String body = Base64.encodeBase64String(httpMsg.getBody().getBytes());
            message.Body = body;        
        }
        
        // fill request
        HTTPRequestMessageEntryProperty httpReqMsg = msg.as(HTTPRequestMessageEntryProperty.class);
        if(httpReqMsg != null){
            pt.sapo.sdb.trace.Request request = new pt.sapo.sdb.trace.Request();
            request.Verb = "";
            request.PathAndQuery = "";
            request.Secure = true;
                    
            message.Request = request;
        }
        // fill response
        HTTPResponseMessageEntryProperty httpResMsg = msg.as(HTTPResponseMessageEntryProperty.class);
        if(httpResMsg != null){
            pt.sapo.sdb.trace.Response response = new pt.sapo.sdb.trace.Response();
            response.StatusCode = 0;
            response.ReasonPhrase = "";
                   
            message.Response = response;
        }
        return message;
    }

   
}
