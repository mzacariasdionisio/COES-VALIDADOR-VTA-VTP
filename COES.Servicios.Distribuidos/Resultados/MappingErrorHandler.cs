using log4net;
using System;
using System.Net;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace COES.Servicios.Distribuidos.Resultados
{
    public class MappingErrorHandler : IErrorHandler
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(MappingErrorHandler));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public bool HandleError(Exception error)
        {
            return error is ApplicationException;
        }

        public void ProvideFault(
            Exception error, MessageVersion version
          , ref System.ServiceModel.Channels.Message fault)
        {
            if (error == null) { return; }
            else
            {
                try
                {
                    log4net.Config.XmlConfigurator.Configure();
                    Log.Error(NameController, error);
                }
                catch (Exception ex)
                {
                }

                //if (error is NotFoundException)
                //{
                //    SetStatus(HttpStatusCode.Conflict);
                //}
                if (error is ApplicationException)
                {
                    SetStatus(HttpStatusCode.Created);
                }
                else
                {
                    throw error;
                }
            }
        }

        private void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current
             .OutgoingResponse.StatusCode = status;
            WebOperationContext.Current
             .OutgoingResponse.SuppressEntityBody = true;
        }
    }
}