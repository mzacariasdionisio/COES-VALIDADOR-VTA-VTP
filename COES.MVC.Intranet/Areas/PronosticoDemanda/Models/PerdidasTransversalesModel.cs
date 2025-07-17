using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class PerdidasTransversalesModel
    {
        public string Fecha { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<PrGrupoDTO> ListBarraCPTransversales { get; set; }
        public List<MePerfilRuleDTO> DtFormulasDisponibles { get; set; }

        //Para el popup
        public List<PrnPrdTransversalDTO> DtSeleccionadas { get; set; }
        public List<PrGrupoDTO> DtDisponibles { get; set; }
    }
}