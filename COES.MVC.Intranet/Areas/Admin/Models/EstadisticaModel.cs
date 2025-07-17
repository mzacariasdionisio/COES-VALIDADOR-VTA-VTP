using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class EstadisticaModel
    {
        public List<LogOptionDTO> ListaEstadistica { get; set; }
        public int IdOpcion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }
}