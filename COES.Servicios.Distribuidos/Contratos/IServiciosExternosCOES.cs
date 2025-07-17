using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Distribuidos.Resultados;

namespace COES.Servicios.Distribuidos.Contratos
{
    /// <summary>
    /// Conjunto de servicios para consumo y envío de información para agentes
    /// </summary>
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IServiciosExternosCOES
    {
        /// <summary>
        /// Consulta de demanda ejecutada
        /// </summary>
        /// <param name="fecha">Fecha de consulta</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerdemandaejecutada/{fecha}")]
        ObtenerDemandaEjecutadaResult ObtenerDemandaEjecutada(string fecha);

        /// <summary>
        /// Consulta de Costo marginal programado en tiempo real
        /// </summary>
        /// <param name="fecha">Fecha de consulta</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenercostomarginal/{fecha}")]
        List<ObtenerCostoMarginalResult> ObtenerCostoMarginal(string fecha);
    }
}
