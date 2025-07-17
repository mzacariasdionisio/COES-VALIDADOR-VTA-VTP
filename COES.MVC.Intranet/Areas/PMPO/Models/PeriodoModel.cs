using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class PeriodoModel
    {
        public int? PmPeriCodi { get; set; }
        public string PmPeriNombre { get; set; }
        public int? PmPeriAniOMes { get; set; }
        public string PmPeriTipo { get; set; }
        public string PmPeriEstado { get; set; }
        public string PmPeriUsuCreacion { get; set; }
        public DateTime? PmPeriFecCreacion { get; set; }
        public string PmPeriUsuModificacion { get; set; }
        public DateTime? PmPeriFecModificacion { get; set; }
    }
}