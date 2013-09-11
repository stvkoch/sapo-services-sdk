package pt.sapo.gis;

/**
 * @author GIS Team
 * Event filter interface
 */
public interface IEventFilter<T> {
    
    Boolean test(T t);
}
