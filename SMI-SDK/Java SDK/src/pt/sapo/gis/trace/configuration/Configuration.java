
package pt.sapo.gis.trace.configuration;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for anonymous complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType>
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="appenders" type="{http://www.sapo.pt/GIS/Trace/configuration}appenders"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "appenders"
})
@XmlRootElement(name = "configuration")
public class Configuration {

    @XmlElement(required = true)
    protected Appenders appenders;

    /**
     * Gets the value of the appenders property.
     * 
     * @return
     *     possible object is
     *     {@link Appenders }
     *     
     */
    public Appenders getAppenders() {
        return appenders;
    }

    /**
     * Sets the value of the appenders property.
     * 
     * @param value
     *     allowed object is
     *     {@link Appenders }
     *     
     */
    public void setAppenders(Appenders value) {
        this.appenders = value;
    }

}
