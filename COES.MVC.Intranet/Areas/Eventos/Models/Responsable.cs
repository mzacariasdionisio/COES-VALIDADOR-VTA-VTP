using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class Responsable
    {
        public int CodigoResponsable { get; set; }
        public int CodigoDirector { get; set; }
        public string NombreCompleto { get; set; }
        public string Estado { get; set;}
        public string NombreArchivo { get; set; }
    }
}