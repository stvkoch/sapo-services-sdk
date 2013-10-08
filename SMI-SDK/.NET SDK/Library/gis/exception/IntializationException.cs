namespace pt.sapo.gis.exception
{
    using System;
    /**
     *
     * @author GIS Team
     */
    public class IntializationException : TraceException
    {

        public IntializationException() { }

        public IntializationException(String message) : base(message) { }

        public IntializationException(String message, Exception cause) : base(message, cause) { }

    }
}
