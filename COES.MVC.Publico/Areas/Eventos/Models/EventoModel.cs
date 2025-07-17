using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.Eventos.Models
{
    public class BusquedaEventoModel
    {
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }        
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }        
        public List<EventoDTO> ListaEventos { get; set; }
        public EventoDTO Entidad { get; set; }
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
    }
}