/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
using System;
namespace pt.sapo.gis.exception
{
    /**
     * @author GIS Team
     *
     * Base class for trace SDK aplication exceptions
     */
    public abstract class TraceException : Exception
    {

        public TraceException() { }

        public TraceException(String message) : base(message) { }

        public TraceException(String message, Exception cause) : base(message, cause) { }
    }
}