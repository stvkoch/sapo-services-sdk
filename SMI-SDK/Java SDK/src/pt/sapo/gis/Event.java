package pt.sapo.gis;

import java.util.ArrayList;
import java.util.Collection;


/**
 *
 * @author GIS Tem
 * Event class 
 */
public class Event<T> implements IEvent<T>, IEventProducer<T>{
    
    public class Listener<T>{
        ICallback<T> callback;
        IEventFilter<T> filter;

        public Listener(ICallback<T> callback, IEventFilter<T> filter) {
            this.callback = callback;
            this.filter = filter;
        }        
        
        @Override
        public boolean equals(Object listener){
            if ( this == listener ) return true;
            if ( !(listener instanceof Listener) ) return false;
            return listener.hashCode() == this.hashCode();
        }

        @Override
        public int hashCode() {
            int hash = 3;
            hash = 97 * hash + (this.callback != null ? this.callback.hashCode() : 0);
            hash = 97 * hash + (this.filter != null ? this.filter.hashCode() : 0);
            return hash;
        }
    }
    
    private Collection<Listener<T>> listeners = new ArrayList<Listener<T>>();

    public Collection<Listener<T>> getListeners() {
        return listeners;
    }
    
    @Override
    public void registerListener(ICallback<T> callback) {
        registerListener(callback, null);
    }
    
    @Override
    public void registerListener(ICallback<T> callback, IEventFilter<T> filter) {
        listeners.add(new Listener<T>(callback, filter));
    }
    
    @Override
    public boolean unregisterListener(ICallback<T> callback) {
        return unregisterListener(callback, null);
    }
    
    @Override
    public boolean unregisterListener(ICallback<T> callback, IEventFilter<T> filter) {
        return listeners.remove(new Listener<T>(callback, filter));
    }

    @Override
    public void notify(T t) throws Exception{
        for(Listener<T> listner : listeners){
            if(listner.filter == null || listner.filter.test(t))
                listner.callback.call(t);
        }
    }
}