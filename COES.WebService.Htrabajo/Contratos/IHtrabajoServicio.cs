using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace COES.WebService.Htrabajo.Contratos
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
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EjecutarCargaFTPPronRER")]
        Task<int> EjecutarCargaFTPPronRER();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EjecutarCargaHtrabajoEnSicoes/{fecha}")]
        Task<int> EjecutarCargaHtrabajoEnSicoes(string fecha);
    }
}