using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class VolumenModel
    {
        public string Fecha { get; set; }
        public List<CpRecursoDTO> ListaEmbalses { get; set; }
        public string[][] Data { get; set; }
    }
}