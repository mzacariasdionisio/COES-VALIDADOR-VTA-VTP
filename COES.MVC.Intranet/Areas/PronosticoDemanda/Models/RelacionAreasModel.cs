using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class RelacionAreasModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<EqAreaNivelDTO> ListaNiveles { get; set; }
        public List<EqAreaDTO> ListaAreas { get; set; }
        public List<EqAreaDTO> ListaSubestacionesSeleccionadas { get; set; }
        public List<EqAreaDTO> ListaSubestacionesDisponibles { get; set; }
        public List<PrGrupoDTO> ListaBarrasSeleccionadas { get; set; }
        public List<PrGrupoDTO> ListaBarrasDisponibles { get; set; }
        public object JDisponibles { get; set; }
        public object JSeleccionados { get; set; }
        public object BarrasDisponibles { get; set; }
        public object BarrasSeleccionados { get; set; }
        public string Nombre { get; set; }
        public EqAreaDTO Area { get; set; }

    }
}