
package pt.sapo.gis.trace.configuration;

import javax.xml.bind.annotation.XmlRegistry;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the pt.sapo.gis.trace.configuration package. 
 * <p>An ObjectFactory allows you to programatically 
 * construct new instances of the Java representation 
 * for XML content. The Java representation of XML 
 * content can consist of schema derived interfaces 
 * and classes representing the binding of schema 
 * type definitions, element declarations and model 
 * groups.  Factory methods for each of these are 
 * provided in this class.
 * 
 */
@XmlRegistry
public class ObjectFactory {


    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: pt.sapo.gis.trace.configuration
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link Configuration }
     * 
     */
    public Configuration createConfiguration() {
        return new Configuration();
    }

    /**
     * Create an instance of {@link Appender.Property }
     * 
     */
    public Appender.Property createAppenderProperty() {
        return new Appender.Property();
    }

    /**
     * Create an instance of {@link Filters }
     * 
     */
    public Filters createFilters() {
        return new Filters();
    }

    /**
     * Create an instance of {@link Appenders }
     * 
     */
    public Appenders createAppenders() {
        return new Appenders();
    }

    /**
     * Create an instance of {@link Appender }
     * 
     */
    public Appender createAppender() {
        return new Appender();
    }

    /**
     * Create an instance of {@link Events }
     * 
     */
    public Events createEvents() {
        return new Events();
    }

    /**
     * Create an instance of {@link Event }
     * 
     */
    public Event createEvent() {
        return new Event();
    }

}
