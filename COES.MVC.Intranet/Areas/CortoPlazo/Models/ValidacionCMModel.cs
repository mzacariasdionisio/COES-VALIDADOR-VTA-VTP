using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class ValidacionCMModel
    {
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public int? Version { get; set; }

        public bool TienePermiso { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public bool TienePermisoGuardar { get; set; }

        public bool UsarLayoutModulo { get; set; }
        public string Fecha { get; internal set; }
    }
}