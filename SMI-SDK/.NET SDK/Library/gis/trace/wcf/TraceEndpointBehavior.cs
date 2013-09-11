using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;

namespace pt.sapo.gis.trace.wcf
{
    public class TraceEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {            
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {            
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new TraceMessageInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
