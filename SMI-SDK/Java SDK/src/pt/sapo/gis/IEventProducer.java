package pt.sapo.gis;

/**
 * @author GIS Team
 * Event producer interface
 */
public interface IEventProducer<T> {
    void notify(T t)throws Exception;
}
