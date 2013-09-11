using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace pt.sapo.gis.trace.configuration
{
    public class TraceConfigurationSectionHandler: IConfigurationSectionHandler
    {   
            #region IConfigurationSectionHandler Members

            public object Create(object parent, object configContext, System.Xml.XmlNode section)
            {
                var xnsm = new XmlNamespaceManager(section.OwnerDocument.NameTable);
                xnsm.AddNamespace("c", "http://services.sapo.pt/GIS/Trace/Configuration");
                return new XmlSerializer(typeof(trace.configuration.configuration)).Deserialize(new XmlNodeReader(section.SelectSingleNode("c:configuration", xnsm)));
            }

            #endregion        
    }
}
