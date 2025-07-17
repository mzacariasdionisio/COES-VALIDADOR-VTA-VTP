using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class CgndModel
    {
        public List<PmoDatCgndDTO> ListCgndpe { get; set; }
        public List<PrGrupoDTO> ListBarra { get; set; }
        public int? PmCgndCodi { get; set; }
        public int? GrupoCodi { get; set; }
        
        public int? PmCgndGrupoCodiBarra { get; set; }
        public string PmCgndTipoPlanta { get; set; }
        public int? PmCgndNroUnidades { get; set; }
        public decimal? PmCgndPotInstalada { get; set; }
        public decimal? PmCgndFactorOpe { get; set; }
        public decimal? PmCgndProbFalla { get; set; }
        public decimal? PmCgndCorteOFalla { get; set; }

    }
}