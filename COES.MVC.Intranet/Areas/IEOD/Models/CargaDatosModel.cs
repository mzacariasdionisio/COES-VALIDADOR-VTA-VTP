using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.IEOD.Models
{
    public class CargaDatosModel
    {
        public string Fecha { get; set; }
        public int IdReporteIeodPotencia { get; set; }
        public int IdReporteIdcosPotencia { get; set; }
        public int IdReporteIeodTension { get; set; }

    }
}