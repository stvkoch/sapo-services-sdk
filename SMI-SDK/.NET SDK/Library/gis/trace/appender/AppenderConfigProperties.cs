/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
namespace pt.sapo.gis.trace.configuration
{
    using System.Collections.Generic;
    using System;
    using System.Linq;

    /**
     *
     * @author GIS Team
     */
    public class AppenderConfigProperties : Dictionary<String, String>
    {
        protected AppenderConfigProperties(IDictionary<String, String> properties) 
            : base(properties) { } 

        public AppenderConfigProperties(ICollection<appenderProperty> properties) 
            : this(properties != null ? properties.ToDictionary(p => p.name, p => String.Join(Environment.NewLine, p.Text)) : new Dictionary<String, String>()) {}
    }
}
