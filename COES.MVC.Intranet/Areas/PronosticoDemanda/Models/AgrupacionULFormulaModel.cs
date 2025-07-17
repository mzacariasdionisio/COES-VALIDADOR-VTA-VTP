using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class AgrupacionULFormulaModel
    {
        public string Fecha { get; set; }
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        //Para el popup
        public List<MePtomedicionDTO> ListaAgrupacionesUL { get; set; }
        public List<object> DtSeleccionados { get; set; }
        public List<object> DtDisponibles { get; set; }
    }
}