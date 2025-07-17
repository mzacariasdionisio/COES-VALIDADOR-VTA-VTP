using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using System.ServiceModel.Web;
using COES.Servicios.Aplicacion.General.Helper;

namespace COES.WebService.Tramite.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ITramiteServicio
    {
       

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "notificaciontramitevirtual/{expediente}/{tipo}")]
        int EnviarNotificacionTramiteVirtual(string expediente, string tipo);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenercorreosnotificacion/{ruc}")]
        List<SiEmpresaCorreoDTO> ObtenerCorreosNotificacion(string ruc);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerlistacorreosportipo/{ruc}/{tipo}")]
        List<String> ObtenerListaCorreosPorTipo(string ruc,string tipo);
    }
}
