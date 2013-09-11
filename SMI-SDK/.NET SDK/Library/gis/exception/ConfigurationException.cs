/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
using System;
namespace pt.sapo.gis.exception
{

    /**
     * Class for Configuration exceptions on trace application
     * @author GIS Tam
     */
    public class ConfigurationException : TraceException
    {

        public ConfigurationException() {}

        public ConfigurationException(String message) : base(message) {}

        public ConfigurationException(String message, Exception cause) : base(message, cause) {}
    }
}