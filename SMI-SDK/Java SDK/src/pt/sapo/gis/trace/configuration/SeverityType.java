
package pt.sapo.gis.trace.configuration;

import javax.xml.bind.annotation.XmlEnum;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for severityType.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * <p>
 * <pre>
 * &lt;simpleType name="severityType">
 *   &lt;restriction base="{http://www.w3.org/2001/XMLSchema}string">
 *     &lt;enumeration value="ERROR"/>
 *     &lt;enumeration value="FATAL"/>
 *     &lt;enumeration value="WARN"/>
 *     &lt;enumeration value="DEBUG"/>
 *     &lt;enumeration value="INFO"/>
 *   &lt;/restriction>
 * &lt;/simpleType>
 * </pre>
 * 
 */
@XmlType(name = "severityType")
@XmlEnum
public enum SeverityType {

    ERROR,
    FATAL,
    WARN,
    DEBUG,
    INFO;

    public String value() {
        return name();
    }

    public static SeverityType fromValue(String v) {
        return valueOf(v);
    }

}
