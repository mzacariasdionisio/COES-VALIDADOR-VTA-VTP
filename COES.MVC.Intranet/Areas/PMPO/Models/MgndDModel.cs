using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class MgndDModel
    {
        public List<PmoDatMgndDTO> ListMgnd { get; set; }
        public List<PrGrupoDTO> ListBarra { get; set; }

        public int PmMgndCodi { get; set; }
        public DateTime? PmMgndFecha { get; set; }
        public int? GrupoCodi { get; set; }
        public int? CodCentral { get; set; }
        public string NombCentral { get; set; }
        public int? CodBarra { get; set; }
        public string NombBarra { get; set; }
        public string PmMgndTipoPlanta { get; set; }
        public int? PmMgndNroUnidades { get; set; }
        public decimal? PmMgndPotInstalada { get; set; }
        public decimal? PmMgndFactorOpe { get; set; }
        public decimal? PmMgndProbFalla { get; set; }
        public decimal? PmMgndCorteOFalla { get; set; }

        public string PmMgndFechastr { get; set; }//20190317 - NET: Corrección
    }
}