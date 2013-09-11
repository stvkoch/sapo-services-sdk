
package pt.sapo.gis.trace.configuration;

import java.util.ArrayList;
import java.util.List;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for appenders complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="appenders">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="appender" type="{http://www.sapo.pt/GIS/Trace/configuration}appender" maxOccurs="unbounded" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "appenders", propOrder = {
    "list"
})
public class Appenders {

    @XmlElement(name = "appender")
    protected List<Appender> list;

    /**
     * Gets the value of the list property.
     * 
     * <p>
     * This accessor method returns a reference to the live list,
     * not a snapshot. Therefore any modification you make to the
     * returned list will be present inside the JAXB object.
     * This is why there is not a <CODE>set</CODE> method for the list property.
     * 
     * <p>
     * For example, to add a new item, do as follows:
     * <pre>
     *    getList().add(newItem);
     * </pre>
     * 
     * 
     * <p>
     * Objects of the following type(s) are allowed in the list
     * {@link Appender }
     * 
     * 
     */
    public List<Appender> getList() {
        if (list == null) {
            list = new ArrayList<Appender>();
        }
        return this.list;
    }

}
