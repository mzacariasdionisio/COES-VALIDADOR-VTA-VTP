using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Models
{
    public class InformacionFrecuenciaModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public List<InformacionFrecuenciaDTO> ListaInformacionFrecuencia { get; set; }
        public List<LecturaDTO> ListaLectura { get; set; }
        public List<InformacionFrecuenciaDTO> ListaDesviacionFrecuencia { get; set; }
        public InformacionFrecuenciaDTO Entidad { get; set; }
        public string sError { get; set; }
        public string sAccion { get; set; }
    }
}