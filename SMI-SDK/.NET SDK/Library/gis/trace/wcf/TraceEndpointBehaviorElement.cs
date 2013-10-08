using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;

namespace pt.sapo.gis.trace.wcf
{
    public class TraceEndpointBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(TraceEndpointBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new TraceEndpointBehavior();
        }
    }
}
