using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.DemandaPO.Models
{
    public class RelacionPuntoBarraModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<DpoVersionRelacionDTO> ListaVersiones { get; set; }
        public List<DpoRelSplFormulaDTO> ListaBarrasSPL { get; set; }
        public List<DpoRelacionPtoBarraDTO> ListaBarrasGrilla { get; set; }
        public List<MePtomedicionDTO> ListaPuntosTna { get; set; }
    }
}