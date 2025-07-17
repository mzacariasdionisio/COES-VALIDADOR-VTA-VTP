using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Models
{

    public class ReporteModel
    {
        public List<COES.Dominio.DTO.Sic.EmpresaDTO> ListaEmpresas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }
        public List<TipoEventoDTO> ListaTipoEvento { get; set; }
        public List<SrmRecomendacionDTO> ListaRecomendacion { get; set; }
        public List<SubCausaEventoDTO> ListaCausaEvento { get; set; }
        public EventoDTO Evento { get; set; }
        public int IdEvento { get; set; }
        public List<EveInterrupcionDTO> ListaInterrupciones { get; set; }

        public bool AccionEliminar { get; set; }
        public bool AccionVerInforme { get; set; }
        public bool AccionCopiar { get; set; }
    }

    public class BusquedaReporteModel
    {
        public List<COES.Dominio.DTO.Sic.EmpresaDTO> ListaEmpresas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }
        public List<TipoEventoDTO> ListaTipoEvento { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<EveCausaeventoDTO> ListaCausaEvento { get; set; }
        public List<fwUserDTO> ListaFwUser { get; set; }

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
        public string CampoOrden { get; set; }
        public string TipoOrden { get; set; }
        public bool AccionNuevo { get; set; }
        public int Indicador { get; set; }        
        public string EquiAbrev { get; set; }
        public string Recomendacion { get; set; }   
        public List<SrmEstadoDTO> ListaEstado { get; set; }
        public List<SrmCriticidadDTO> ListaCriticidad { get; set; }
        public int IdCriticidad { get; set; }
        public int IdEstado { get; set; }
        public int IdResponsable { get; set; }      

        //lista de recomendaciones
        public List<SrmRecomendacionDTO> listaRecomendacion { get; set; }

        public bool AccionConsultar { get; set; }
        
    }
    
}