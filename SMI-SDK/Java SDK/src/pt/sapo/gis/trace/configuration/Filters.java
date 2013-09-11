
package pt.sapo.gis.trace.configuration;

import java.util.ArrayList;
import java.util.List;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for filters complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="filters">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="severityFilter" type="{http://www.sapo.pt/GIS/Trace/configuration}severityType" maxOccurs="unbounded" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "filters", propOrder = {
    "severityFilters"
})
public class Filters {

    @XmlElement(name = "severityFilter")
    protected List<SeverityType> severityFilters;

    /**
     * Gets the value of the severityFilters property.
     * 
     * <p>
     * This accessor method returns a reference to the live list,
     * not a snapshot. Therefore any modification you make to the
     * returned list will be present inside the JAXB object.
     * This is why there is not a <CODE>set</CODE> method for the severityFilters property.
     * 
     * <p>
     * For example, to add a new item, do as follows:
     * <pre>
     *    getSeverityFilters().add(newItem);
     * </pre>
     * 
     * 
     * <p>
     * Objects of the following type(s) are allowed in the list
     * {@link SeverityType }
     * 
     * 
     */
    public List<SeverityType> getSeverityFilters() {
        if (severityFilters == null) {
            severityFilters = new ArrayList<SeverityType>();
        }
        return this.severityFilters;
    }

}
