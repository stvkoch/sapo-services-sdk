package pt.sapo.gis.trace;

import java.util.Collection;

import pt.sapo.gis.trace.configuration.SeverityType;

public class TraceSeverityEventFilter extends SeverityEventFilter<Trace>{ 

    public TraceSeverityEventFilter(Collection<SeverityType> severities){
        super(severities);
        
    }
            
    @Override
    public Boolean test(Trace t) {
        return super.containsAny(t.getSeverityTypes());        
    }    
}
