using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Compensacion.Models
{
    public class ModosOperacionModel
    {
        public List<PeriodoDTO> ListTrnPeriodo { get; set; }
        public List<PrGrupoDTO> ListModosOperacion { get; set; }

        public List<VceCfgValidaConceptoDTO> ListValidaConcepto { get; set; }
        public List<PrGrupodatDTO> ListErroresConcepto { get; set; }

        public PrGrupoDTO ModoOperacion { get; set; }

        public List<PrGrupodatDTO> ListAsignacionBarras { get; set; }

        public List<PrBarraDTO> ListBarras { get; set; }
        
    }
}