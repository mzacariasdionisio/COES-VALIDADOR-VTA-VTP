using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class ReprocesoModel
    {
        public string Fecha { get; set; }
        public List<CmCostomarginalDTO> ListaProceso { get; set; }
        public List<EqCongestionConfigDTO> ListaLinea { get; set; }
        public List<EveIeodcuadroDTO> ListaOperaciones { get; set; }
        public List<CmConfigbarraDTO> ListaBarra { get; set; }
        public string FechaVigenciaPR07 { get; set; }
    }
}