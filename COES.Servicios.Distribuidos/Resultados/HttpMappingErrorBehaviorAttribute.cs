using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;

namespace COES.Servicios.Distribuidos.Resultados
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpMappingErrorBehaviorAttribute
        : Attribute, IServiceBehavior
    {
        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
            // No op
        }

        public void ApplyDispatchBehavior(
                          ServiceDescription serviceDescription,
                          ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler = new MappingErrorHandler();
            foreach (ChannelDispatcherBase channelDispatcherBase
                  in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher =
                    channelDispatcherBase as ChannelDispatcher;
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }
        }

        public void Validate(ServiceDescription serviceDescription,
           ServiceHostBase serviceHostBase)
        {
            // No op
        }
    }
}