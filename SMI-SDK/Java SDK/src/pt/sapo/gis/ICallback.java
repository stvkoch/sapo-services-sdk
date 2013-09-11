package pt.sapo.gis;

import pt.sapo.gis.exception.EventCallException;

/**
 * @author GIS Team
 * Interface to events callback objects
 */
public interface ICallback <T> { 
    /**
     * Method to implement the calling from event producer
     */
    void call(T t) throws EventCallException;
}
