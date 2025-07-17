using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.DemandaPO.Models
{
    public class RelacionBarraModel
    {
        public List<DpoVersionRelacionDTO> ListaVersiones { get; set; }
        public List<DpoRelSplFormulaDTO> ListaBarras { get; set; }
        public List<DpoBarraSplDTO> ListaBarrasSPL { get; set; }
        public List<PrGrupoDTO> ListaBarrasGrupo { get; set; }
        //Adicional
        public List<MePerfilRuleDTO> ListaFormulasVegetativa { get; set; }
        public List<MePerfilRuleDTO> ListaFormulasIndustrial { get; set; }
    }
}