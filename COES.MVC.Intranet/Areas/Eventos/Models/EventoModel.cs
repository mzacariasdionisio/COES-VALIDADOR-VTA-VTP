using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class EventoModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }
        public List<TipoEventoDTO> ListaTipoEvento { get; set; }
        public List<EventoDTO> ListaEventos { get; set; }
        public List<SubCausaEventoDTO> ListaCausaEvento { get; set; }
        public List<MeEnvioDTO> ListaEnvios { get; set; }
        public EventoDTO Evento { get; set; }
        public int IdEvento { get; set; }
        public List<EveSubeventosDTO> ListaSubEventos { get; set; }
        public List<EveInterrupcionDTO> ListaInterrupciones { get; set; }

        public bool AccionEliminar { get; set; }
        public bool AccionVerInforme { get; set; }
        public bool AccionCopiar { get; set; }
        public string Evenini { get; set; }
        public string Afefechainterr { get; set; }
        public string Afecodi { get; set; }
        public int Evencodi { get; set; }
        public bool AccionEventoCtaf { get; set; }
    }

    public class BusquedaEventoModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }
        public List<TipoEventoDTO> ListaTipoEvento { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<EveCausaeventoDTO> ListaCausaEvento { get; set; }
        public int IdFamilia { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoEvento { get; set; }
        public int IdTipoEmpresa { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Version { get; set; }
        public string Turno { get; set; }
        public string Informe { get; set; }
        public int ParametroDefecto { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public string IndInterrupcion { get; set; }
        public string CampoOrden { get; set;}
        public string TipoOrden { get; set; }
        public bool AccionNuevo { get; set; }
        public int Indicador { get; set; }
        public string AreaOperativa { get; set; }
        public bool IndicadorGrabarAseg { get; set; }        
        public int IdOpcion { get; set; }
        public bool AccionEventoCtaf { get; set; }
    }
    
    public class PerturbacionModel
    {
        public string FechaEvento { get; set; }
        public string HoraEvento { get; set; }
        public string EquipoEvento { get; set; }
        public string EmpresaEvento { get; set; }
        public string AsuntoEvento { get; set; }
        public int CodigoEvento { get; set; }
        public string IndicadorExistencia { get; set; }
        public string IndicadorGrabado { get; set; }
        public List<SubCausaEventoDTO> ListaCausaEvento { get; set; }
        public List<InformePerturbacionDTO> ListaInforme { get; set; }
    }

    public class BusquedaEquipoModel
    {
        public List<EmpresaDTO> ListaEmpresa { get; set; }
        public List<FamiliaDTO> ListaFamilia { get; set; }
        public List<AreaDTO> ListaArea { get; set; }
        public List<EquipoDTO> ListaEquipo { get; set; }   
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

    }
}