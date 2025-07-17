using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.RechazoCarga.Models
{
    public class Respuesta
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public object Datos { get; set; }

        public int CodigoPrograma { get; set; }

        public int CodigoCuadroPrograma { get; set; }
    }
}