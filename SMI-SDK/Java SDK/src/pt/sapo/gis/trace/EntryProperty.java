package pt.sapo.gis.trace;

import java.util.HashMap;

/**
 * @author GIS Team
 * Base class to represent an entry property.
 */
public abstract class EntryProperty extends HashMap {  
    
    public <T> T as(Class<T> clazz){
	if(clazz.isInstance(this)){
		return clazz.cast(this);
	}
	return null;
    }
}
