using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.DemandaPO.Models
{
    public class SeguimientoModel
    {
        public string FechaMes { get; set; }
        public string FechaDia { get; set; }
        public List<MePtomedicionDTO> ListaPuntos { get; set; }
    }
}