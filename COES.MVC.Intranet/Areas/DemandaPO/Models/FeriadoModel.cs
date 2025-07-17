using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.DemandaPO.Models
{
    public class FeriadoModel
    {
        public string FechaIniRango { get; set; }
        public string FechaFinRango { get; set; }

        public string Fecha { get; set; }

        public List<DpoFeriadosDTO> ListaFeriados { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
    }
}