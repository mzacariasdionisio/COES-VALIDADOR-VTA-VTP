using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Models
{
    public class ExtraerFrecuenciaModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public List<ExtraerFrecuenciaDTO> ListaExtraerFrecuencia { get; set; }
        public List<EquipoGPSDTO> ListaEquipos { get; set; }
        public List<LecturaVirtualDTO> ListaMilisegundos { get; set; }
        public ExtraerFrecuenciaDTO Entidad { get; set; }
        public int IdCarga { get; set; }
        public string sError { get; set; }
        public string sAccion { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
    }
}