using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.PMPO.Models
{
    public class GndseModel
    {
        public List<PrGrupoDTO> ListCentrales { get; set; }
        
        public int? periodo { get; set; }
        
    }
}