package pt.sapo.gis.trace;

import java.util.Collection;
import pt.sapo.gis.trace.configuration.SeverityType;

/**
 * @author GIS Team
 * Filter entry events by severity type.
 */
public class EntrySeverityEventFilter extends SeverityEventFilter<Entry>{

    public EntrySeverityEventFilter(Collection<SeverityType> severities){
        super(severities);
        
    }
            
    @Override
    public Boolean test(Entry e) {
        return super.contains( e.severity);
    }
}
