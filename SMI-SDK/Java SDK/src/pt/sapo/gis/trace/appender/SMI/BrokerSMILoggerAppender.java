package pt.sapo.gis.trace.appender.SMI;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import javax.xml.datatype.XMLGregorianCalendar;
import org.tmforum.schemas.smi.ManagementReport;
import pt.com.broker.client.BrokerClient;
import pt.com.broker.types.NetBrokerMessage;
import pt.sapo.gis.exception.IntializatonException;
import pt.sapo.gis.trace.utils.XMLGregorianCalendarGsonConverter;

public class BrokerSMILoggerAppender extends SMILoggerAppender {

    private BrokerClient brokerClient;
    
    public BrokerSMILoggerAppender(pt.sapo.gis.trace.configuration.Appender config) throws IntializatonException {
        super(config);
        properties = new BrokerSMILoggerAppenderConfigProperties(config.getProperties());
        try {
            this.brokerClient = new BrokerClient(((BrokerSMILoggerAppenderConfigProperties)properties).getHost(), ((BrokerSMILoggerAppenderConfigProperties)properties).getPort().intValue());
        } catch (Throwable ex) {
            throw new IntializatonException("Error initialize sapo broker client", ex);
        }
    }

    @Override
    public void send(ManagementReport report) {
       
        GsonBuilder builder = new GsonBuilder();
        builder.registerTypeAdapter(XMLGregorianCalendar.class, new XMLGregorianCalendarGsonConverter.Serializer());
        builder.registerTypeAdapter(XMLGregorianCalendar.class, new XMLGregorianCalendarGsonConverter.Deserializer());
        Gson gson = builder.create();
        
        String json = gson.toJson(report);

        NetBrokerMessage brokerMessage = new NetBrokerMessage(json);
        this.brokerClient.publishMessage(brokerMessage, ((BrokerSMILoggerAppenderConfigProperties)properties).getTopic());
    } 
}