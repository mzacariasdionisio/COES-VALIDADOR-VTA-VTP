using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.DemandaPO.Models
{
    public class CargaDatosModel
    {
        public string Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public int idModulo { get; set; }
    }
}