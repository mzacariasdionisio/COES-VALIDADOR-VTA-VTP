using DocumentFormat.OpenXml.Presentation;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Web.Script.Services;

namespace COES.Servicios.Distribuidos.Contratos
{
    /// <summary>
    /// Interface con los contratos de los servicios
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IHtrabajoServicio
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SubirArchivoFTPPronosticoRER/{fecha}/{hMediaHora}")]
        Task<int> SubirArchivoFTPPronosticoRER(string fecha, string hMediaHora);
    }
}