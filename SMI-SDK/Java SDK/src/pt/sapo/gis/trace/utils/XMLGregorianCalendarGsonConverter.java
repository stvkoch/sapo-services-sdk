/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace.utils;

import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;

import com.google.gson.JsonPrimitive;
import com.google.gson.JsonSerializationContext;
import com.google.gson.JsonSerializer;
import java.lang.reflect.Type;

import javax.xml.datatype.DatatypeFactory;
import javax.xml.datatype.XMLGregorianCalendar;

/**
 *
 * @author GIS Team
 */
public class XMLGregorianCalendarGsonConverter {

    public static class Serializer implements JsonSerializer {

        public Serializer() {
            super();
        }

        @Override
        public JsonElement serialize(Object t, Type type,
                JsonSerializationContext jsonSerializationContext) {
            XMLGregorianCalendar xgcal = (XMLGregorianCalendar) t;
            return new JsonPrimitive(xgcal.toXMLFormat());
        }
    }

    public static class Deserializer implements JsonDeserializer {

        @Override
        public Object deserialize(JsonElement jsonElement, Type type,
                JsonDeserializationContext jsonDeserializationContext) {
            try {
                return DatatypeFactory.newInstance().newXMLGregorianCalendar(jsonElement.getAsString());
            } catch (Exception ex) {
                return null;
            }
        }
    }
}
