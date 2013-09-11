using System;
namespace pt.sapo.gis.exception
{
    /**
     *
     * @author GIS Team
     */
    public class EventCallException : TraceException
    {

        public EventCallException() { }

        public EventCallException(String message) : base(message) { }

        public EventCallException(String message, Exception cause) : base(message, cause) { }

    }
}
