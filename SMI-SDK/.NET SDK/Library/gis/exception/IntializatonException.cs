namespace pt.sapo.gis.exception
{
    using System;
    /**
     *
     * @author GIS Team
     */
    public class IntializatonException : TraceException
    {

        public IntializatonException() { }

        public IntializatonException(String message) : base(message) { }

        public IntializatonException(String message, Exception cause) : base(message, cause) { }

    }
}
