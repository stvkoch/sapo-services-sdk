package pt.sapo.sdb.trace;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 *
 * @author GIS Team
 */
public class Entry {
    public String Description;
    
    public SeverityType Severity;
    
    public double Offset;
    
    public double Duration;
    
    public List<Entry> Entries = new ArrayList<Entry>();;    
    
    public Map<String,Object> Properties;
    
    public Integer MessageNumber;   
    
    public Exception Exception;
    
    public String LinkedTraceId;    
}
