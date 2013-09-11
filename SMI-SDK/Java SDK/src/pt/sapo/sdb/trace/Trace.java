package pt.sapo.sdb.trace;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class Trace {    
    
    public Authentication authentication;
    public Date start;
    public Date end;
    public Integer tenantId;
    public String traceId ;
    public String id ;
    public String activityId ;
    public String parentId ;
    
    public Server Server = null;
    public Client Client = null;
    public Service Service = null;
    public Summary Summary = null;
                
    public List<Entry> entries = new ArrayList<Entry>();
    public ArrayList<Message> messages = new ArrayList<Message>();
}
