using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class ConfiguracionEstimadorModel
    {
        public string Fecha { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<MePtomedicionDTO> ListFormulas { get; set; }
        public List<MePerfilRuleDTO> ListFormulasEstimador { get; set; }
        public List<PrnRelacionTnaDTO> ListRegistros { get; set; }
        public List<PrnRelacionTnaDTO> ListRegistrosBarras { get; set; }
        public List<PrGrupoDTO> ListBarrasCP { get; set; }
        public List<PrnRelacionTnaDTO> ListRelacion { get; set; }
        public List<PrnFormularelDTO> ListFormulasTna { get; set; }
    }
}