using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace COES.Servicios.Distribuidos.Contratos
{
    [Obsolete("El servicio está obsoleto, se debe utilizar el servicio distribuido COES.WebService.SCOSinac.")]
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ISCOSinacServicio
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ListaScoSinacGetEventosCV")]
        List<PrRepcvDTO> ListaScoSinacGetEventosCV(BuscarEventosRequest data);        

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ListaScoSinacGetPotenciaTNA")]
        List<MeMedicion48DTO> ListaScoSinacGetPotenciaTNA(PegarScadaRequest data);
        

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ListaScoSinacGetPotenciaScada")]
        List<MeMedicion48DTO> ListaScoSinacGetPotenciaScada(PegarScadaRequest data);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ListaScoSinacGetFlujosScada")]
        List<MeMedicion48DTO> ListaScoSinacGetFlujosScada(PegarScadaRequest data);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "ListaScoSinacLectura/{zeroh}/{gpscodi}/{fechaConsulta}")]
        List<FLecturaSp7DTO> ListaScoSinacLectura(string zeroh, string gpscodi, string fechaConsulta);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ListaScoSinacDespacho")]
        List<Medicion48> ListaScoSinacDespacho(Medicion48Request data);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ListaScoSinacDespachoRER")]
        List<Medicion48> ListaScoSinacDespachoRER(Medicion48Request data);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ListaScoEscenarioReprogramaYupana")]
        List<CpTopologiaDTO> ListaScoEscenarioReprogramaYupana(FechaRequest data);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "Eliminarmedicion48")]
        int Eliminarmedicion48();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "Insertarmedicion48")]
        int Insertarmedicion48(Insert dato);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GrabarReprogramaHidrologia")]
        int GrabarReprogramaHidrologia(HidrologiaResult data);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GrabarDespachoRER")]
        int GrabarDespachoRER(RERResult data);
    }
    [DataContract]
    public class Insert
    {
        [DataMember]
        public string fecha { get; set; }
        [DataMember]
        public string medicodi { get; set; }
        [DataMember]
        public string valor { get; set; }
    }

    [DataContract]
    public class FechaRequest
    {
        [DataMember]
        public string Fecha { get; set; }
    }

    [DataContract]
    public class Medicion48Request
    {
        [DataMember]
        public string Lectura { get; set; }
        [DataMember]
        public string Medida { get; set; }
        [DataMember]
        public string Fuente { get; set; }
        [DataMember]
        public string Puntos { get; set; }
        [DataMember]
        public string Fecha { get; set; }
        [DataMember]
        public string Escenario { get; set; }
    }

    [DataContract]
    public class HidrologiaResult
    {
        [DataMember]
        public string Fuente { get; set; }
        [DataMember]
        public string Fecha { get; set; }
        [DataMember]
        public int Horainicio { get; set; }
        [DataMember]
        public List<Medicion48HidroResult> Listam48hidro { get; set; }
    }

    [DataContract]
    public class RERResult
    {
        [DataMember]
        public string Fuente { get; set; }
        [DataMember]
        public string Fecha { get; set; }
        [DataMember]
        public int Horainicio { get; set; }
        [DataMember]
        public List<Medicion48HidroResult> Listam48rer { get; set; }
    }

    [DataContract]
    public class Medicion48HidroResult
    {
        [DataMember]
        public int Ptomedicodi { get; set; }
        [DataMember]
        public int Tipoinfocodi { get; set; }
        [DataMember]
        public List<decimal> ListaH { get; set; }
    }

    [DataContract]
    public class PegarScadaRequest
    {        
        [DataMember]
        public string Fecha { get; set; }        
        [DataMember]
        public List<int> LstIds { get; set; }
        [DataMember]
        public string NombreHoja { get; set; }

    }

    [DataContract]
    public class BuscarEventosRequest
    {
        [DataMember]
        public string FechaIni { get; set; }
        [DataMember]
        public string FechaFin { get; set; }
        

    }

    public class AddInParametroGeneral
    {
        public string AbrevGeneral { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public string Formula { get; set; }
    }
    public class AddInColModo
    {
        public string AbrevModo { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public string Asterisco { get; set; }
        public string Formula { get; set; }
    }

    public class CVDataRequest
    {
        public int Repcodi { get; set; }
        public List<AddInParametroGeneral> LstParamGenerales { get; set; }
        public List<AddInColModo> LstParamModo { get; set; }
        public string FlagIncluirMONoActivos { get; set; }
    }

}

