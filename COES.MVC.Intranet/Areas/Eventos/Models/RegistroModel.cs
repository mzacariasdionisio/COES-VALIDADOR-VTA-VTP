using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    /// <summary>
    /// Clase model
    /// </summary>
    public class RegistroModel
    {
        public List<EveTipoeventoDTO> ListaTipoEvento { get; set; }
        public List<EveSubcausaeventoDTO> ListaSubCausaEvento { get; set; }
        public List<EveSubcausaeventoDTO> ListaTipoOperacion { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EveInterrupcionDTO> ListaInterrupciones { get; set; }
        public EveEventoDTO Entidad { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public int IdTipoEvento { get; set; }
        public int IdTipoOperacion { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public int IdSubCausaEvento { get; set; }
        public string TipoFalla { get; set; }
        public string Fases { get; set; }
        public string MensajeSMS { get; set; }
        public string Relevante { get; set; }
        public string CTAnalisis { get; set; }
        public string InformeFalla { get; set; }
        public string InformeFalla2 { get; set; }
        public string Descripcion { get; set; }
        public string Detalle { get; set; }
        public string Comentarios { get; set; }
        public string IndicadorInterrupcion { get; set; }
        public int IdEvento { get; set; }
        public string TipoRegistro { get; set; }
        public string TipoMalaCalidad { get; set; }
        public string DesconexionInterrupcion { get; set; }
        public string DesconexionDisminucion { get; set; }
        public string DesconexionManual { get; set; }
        public string DesconexionAutomatico { get; set; }
        public string IndAuditoria { get; set; }
        public int IdEmpresa { get; set; }
        public int IdEquipo { get; set; }
        public bool IndicadorGrabar { get; set; }
        public bool IndicadorInforme { get; set; }
        public bool IndicadorAdicional { get; set; }
        public bool IndicadorImportar { get; set; }
        public string ValDesconexionInterrupcion { get; set; }
        public string ValDesconexionDisminucion { get; set; }
        public string ValDesconexionManual { get; set; }
        public string ValDesconexionAutomatico { get; set; }
        public string RemitenteCorreo { get; set; }
        public string IndRemitenteCorreo { get; set; }
        public string ProvocaInterrupcion { get; set; }
        public decimal? MWInterrumpidos { get; set; }
        public decimal? TensionFalla { get; set; }
        public string AreaOperativa { get; set; }
        public string IdsEquipos { get; set; }
        public string DeconectaGeneracion { get; set; }
        public decimal? MWGeneracionDesconectada { get; set; }

        public string ArchivoAdicional { get; set; }

        //AseguramientoOperacion SEV
        public string EvenAsegOperacion { get; set; }
        public bool IndicadorGrabarAseg { get; set; }

        // CTAF
        public int DiaIpiAmpliacion { get; set; }
        public int DiaIfAmpliacion { get; set; }
        public string HorarioIpiAmpliacion { get; set; }
        public string HorarioIfAmpliacion { get; set; }
        public int EvenInfCodi { get; set; }
        public int EvenInfn2Codi { get; set; }

    }

    /// <summary>
    /// Clase modelo para las interrupciones
    /// </summary>
    public class InterrupcionModel
    {
        public List<EvePtointerrupDTO> ListaPuntos { get; set; }
        public EveInterrupcionDTO Entidad { get; set; }
        public List<EveInformeItemDTO> ListaInterrupcionInforme { get; set; }
        public decimal? InterrmwDe { get; set; }
        public decimal? InterrmwA { get; set; }
        public decimal? Interrminu { get; set; }
        public decimal? Interrmw { get; set; }
        public string Interrdesc { get; set; }
        public int Interrupcodi { get; set; }
        public int? Ptointerrcodi { get; set; }
        public int? Evencodi { get; set; }
        public string Interrnivel { get; set; }
        public string Interrracmf { get; set; }
        public int? Interrmfetapa { get; set; }
        public string Interrmanualr { get; set; }
        public int IdItemInforme { get; set; }
        public bool IndicadorGrabar { get; set; }


    }

    /// <summary>
    /// Modelo para manejar la audioria de eventos
    /// </summary>
    public class AuditoriaModel
    {
        public List<EveEventoLogDTO> ListaAuditoria { get; set; }
    }
}
