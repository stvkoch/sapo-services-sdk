/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
namespace pt.sapo.gis.trace.configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /**
     *
     * @author GIS Team
     */
    public class AppenderConfigProperties
    {

        protected IDictionary<String, String> properties = new Dictionary<String, String>();

        public AppenderConfigProperties(List<pt.sapo.gis.trace.configuration.Appender.Property> properties)
        {
            if (properties != null)
            {
                this.properties = properties.ToDictionary(p => p.Name, p => p.Content);
            }
        }
    }
}
