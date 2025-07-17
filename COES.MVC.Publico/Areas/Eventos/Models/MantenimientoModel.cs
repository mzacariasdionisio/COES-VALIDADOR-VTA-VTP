using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.Eventos.Models
{
    public class MantenimientoModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }
        public List<EveEvenclaseDTO> ListaTipoMantenimiento { get; set; }
        public List<EveManttoDTO> ListaManttos { get; set; }
        public List<ReporteManttoDTO> ListarReporteManttos { get; set; }
        public List<TipoEventoDTO> ListaTipoEvento { get; set; }
        public List<SubCausaEventoDTO> ListaCausaEvento { get; set; }
        public EventoDTO Evento { get; set; }
        public int IdMantto { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }   
        public string Resultado { get; set; }
    }

    public class BusquedaMantenimientoModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }
        public List<EveEvenclaseDTO> ListaTipoMantenimiento { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<EveCausaeventoDTO> ListaCausaEvento { get; set; }
        public List<EveTipoeventoDTO> ListaTipoEvento { get; set; }
        public int IdFamilia { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoMantenimiento { get; set; }
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
    }
}