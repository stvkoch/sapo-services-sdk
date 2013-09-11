package pt.sapo.gis;

/**
 * @author GIS Team
 * Event client interface.
 */
public interface IEvent<T> {
    
    void registerListener(ICallback<T> callback);
    
    void registerListener(ICallback<T> callback, IEventFilter<T> filter);
    
    boolean unregisterListener(ICallback<T> callback);
    
    boolean unregisterListener(ICallback<T> callback, IEventFilter<T> filter);
}
