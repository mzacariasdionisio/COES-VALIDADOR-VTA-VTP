using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.MVC.Intranet.SeguridadServicio;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Models
{
    public class ReporteTransgresionesModel
    {
        // public bool bEditar { get; set; }
        //public bool bNuevo { get; set; }
        //public bool bEliminar { get; set; }
        //public bool bGrabar { get; set; }
        public List<ListaReporte> ListaReporte { get; set; }

    }

    public class ListaReporte
    {
        public string Id { get; set; }
        public string RGPSOSINERG { get; set; }
        public string GPS { get; set; }
    }
}