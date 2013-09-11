package pt.sapo.gis.trace;

import java.util.Collection;
import pt.sapo.gis.IEventFilter;
import pt.sapo.gis.trace.configuration.SeverityType;


/**
 * @author GIS Team
 * Represents the class to use as a event filter for a severity collection values
*/
public abstract class SeverityEventFilter<T> implements IEventFilter<T>{
    Collection<SeverityType> severities;
            
    public SeverityEventFilter(Collection<SeverityType> severities){
        this.severities = severities;
    }
     
    /**
     * Test if severities collection contains a given severity value
     * @param severity the severity value to test
     * @return true if the collection contains the given value
    */
    public Boolean contains(SeverityType severity) {
        return this.severities.contains(severity);
    }
    
    /**
     * Test if severities collection contains any of the given severity values
     * @param severities the severity values to test
     * @return true if the collection contains any of the given values
    */
    public Boolean containsAny(Collection<SeverityType> severities) {
        for(SeverityType severity : severities)
            if(this.severities.contains(severity))
                return true;
        return false;
    }
     
    /**
     * Test if severities collection contains all of the given severity values
     * @param severities the severity values to test
     * @return true if the collection contains all of the given values
    */
    public Boolean containsAll(Collection<SeverityType> severities) {
        for(SeverityType severity : severities)
            if(!this.severities.contains(severity))
                return false;
        return true;
    }

}
