using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Models
{
    public class IeodModel
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<ReporteCMGRealDTO> ListaReporte { get; set; }
    }
}