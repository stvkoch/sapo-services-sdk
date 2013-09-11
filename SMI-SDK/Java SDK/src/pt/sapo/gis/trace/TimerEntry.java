package pt.sapo.gis.trace;

import pt.sapo.gis.Timer;

/**
 * @author GIS Team
 * Class to associate an entry with a timer
 */
public class TimerEntry {    
    Timer timer;
    Entry entry;
    
    public TimerEntry(Timer timer, Entry entry){
        this.timer = timer;
        this.entry = entry;
    }
}