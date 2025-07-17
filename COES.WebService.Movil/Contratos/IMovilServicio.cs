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

namespace COES.WebService.Movil.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IMovilServicio
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerreportedemanda/{fechaConsulta}")]
        ChartDemanda ObtenerReporteDemanda(string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerreportegeneracion/{fechaConsulta}")]
        GraficoGeneracion ObtenerReporteGeneracion(string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerreportegeneraciontipogeneracion/{fechaConsulta}")]
        GraficoGeneracion ObtenerReporteGeneracionTipoGeneracionbyFecha(string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerreportegeneraciontipogeneracion/")]
        GraficoGeneracion ObtenerReporteGeneracionTipoGeneracion();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenereventosrelevantes/{fechaConsulta}")]
        List<EveEventoDTO> ObtenerEventosRelevantes(string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerfrecuencia/")]
        List<GraficoFrecuencia> ObtenerFrecuenciaSein();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerfiltromantenimiento/")]
        List<EveManttoTipoDTO> ObtenerFiltroMantenimientos();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenermantenimientos/{fechaConsulta}/{tipo}")]        
        List<EveManttoDTO> ObtenerMantenimientos(string fechaConsulta, string tipo);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenermantenimientos/{fechaConsulta}")]
        List<EveManttoDTO> ObtenerMantenimientosProgramados(string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerleyendacostomarginal/")]
        List<CmParametroDTO> ObtenerColoresCostosMarginal();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenermapacostomarginal/{corrida}")]
        List<CmCostomarginalDTO> ObtenerMapaCostosMarginal(string corrida);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenercorridascostomarginal/{fechaConsulta}")]
        List<CmCostomarginalDTO> ObtenerCorridasCostoMarginal(string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenercalendariocoes/")]
        List<WbCalendarioDTO> ObtenerCalendarioCOES();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenercalendariocoes/{fechaConsulta}")]
        List<WbCalendarioDTO> ObtenerCalendarioMesCOES(string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenertipocalendario/")]
        List<WbCaltipoventoDTO> ObtenerTipoCalendarioCOES();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerreportemaximademanda/{fechaConsulta}")]
        GraficoMaximaDemanda ObtenerReporteMaximaDemanda(string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenermaximademanda/{fechaConsulta}")]
        GraficoMaximaDemanda ObtenerMaximaDemanda(string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerdatosresumen/")]
        DatosResumenMovil ObtenerDatosResumen();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerlistadocodigoimei/")]
        List<WbRegistroimeiDTO> ObtenerListadoCodigoIMEI();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerayudaventana/")]
        List<WbAyudaappDTO> ObtenerAyudaVentanaAPP();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obteneragendacoes/{idEmpresa}")]
        List<WbContactoDTO> ObtenerAgendaCOES(string idEmpresa);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerempresas/")]
        List<EmpresaContactoDTO> ObtenerEmpresas();

        #region METODOS AGREGADOS

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "enviarnotificacion")]
        Response EnviarNotificacion();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "acercadecoes")]
        string AcercaDeCoes();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtnernotificaciones")]
        List<WbNotificacionDTO> ObtnerNotificaciones();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenerversion")]
        WbVersionappDTO ObtenerVersionAPP();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "validarcredenciales/{usuario}/{clave}")]
        int ValidarCredenciales(string usuario, string clave);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "validarcredencialesapple/{usuario}/{clave}")]
        bool ValidarCredencialesApple(string usuario, string clave);

        #endregion

        #region METODOS PARA NUEVO REQUERIMIENTO

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenertiposnotificacioneventos")]
        List<WbTipoNotificacionDTO> ObtenerTipoNotificacionEventos();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "obtenercmvstarifabarra/{fechaConsulta}")]
        List<ChartCmVsTarifaBarra> ObtenerCmVsTarifaBarra(string fechaConsulta);

        #endregion
    }
}
