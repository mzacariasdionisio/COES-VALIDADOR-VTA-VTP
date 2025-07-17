using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.InformesOsinergmin.Models
{
    public class CostoMarginalRealModel
    {
        public string mesInforme { get; set; }
        public string nombreArchivoExcel { get; set; }
        public List<PsuDesvcmgsncDTO> ListadoCMRDiario { get; set; }
        public PsuDesvcmgDTO CMRMensual { get; set; }
    }
}