using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class FormulasModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<EqAreaDTO> ListaAreaOperativa { get; set; }
        public List<PrnClasificacionDTO> ListaPtomedicion { get; set; }
        public List<PrnClasificacionDTO> ListaEmpresa { get; set; }

        public List<MePtomedicionDTO> ListaAgrupacion { get; set; }
        public List<PrnFormularelDTO> DtTodos { get; set; }
        public List<PrnFormularelDTO> DtSeleccionado { get; set; }
    }
}