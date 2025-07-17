using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.MVC.Intranet.SeguridadServicio;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Models
{
    public class ReporteSegundosFaltantesModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public List<ReporteSegundosFaltantesDTO> ListaReporte { get; set; }
        public List<EquipoGPSDTO> ListaEquipos { get; set; }
        public List<EtapaERADTO> ListaEtapas { get; set; }
        public List<ReporteFrecuenciaDescargaDTO> ListaFrecuencia { get; set; }
        public List<ReporteFrecuenciaDescargaDTO> ListaFrecuenciaMinuto { get; set; }
        public List<string> ListaFechas { get; set; }
        public ReporteSegundosFaltantesDTO Entidad { get; set; }
        public int IdEquipo { get; set; }
        public List<EmpresaDTO> ListaEmpresas { get; set; }
        public string sError { get; set; }
        public string sAccion { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string IndOficial { get; set; }
    }
}