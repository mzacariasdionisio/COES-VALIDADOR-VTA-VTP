using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class PlanificacionModel
    {
        public List<WbRegistroModplanDTO> ListaRegistro { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }
}