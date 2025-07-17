using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Models
{
    public class ConsultaEstimadorModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<MePtomedicionDTO> ListaUnidades { get; set; }
        public List<MePtomedicionDTO> ListaUnidadesEstimador { get; set; }
        public List<PrnAsociacionDTO> ListaAsociacion { get; set; }
        public List<PrnAsociacionDTO> ListaAsociacionDetalle { get; set; }
        public List<PrnVariableDTO> ListaVariables { get; set; }
        public int idModulo { get; set; }
    }
}