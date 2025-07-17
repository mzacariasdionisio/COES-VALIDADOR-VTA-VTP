using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.MVC.Intranet.SeguridadServicio;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Models
{
    public class EtapaERAModel
    {
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public List<EtapaERADTO> ListaEtapas { get; set; }
        public EtapaERADTO Entidad { get; set; }
        public int IdEtapa { get; set; }
        public string sError { get; set; }
        public string sAccion { get; set; }
        public string IdEtapas { get; set; }
    }
}