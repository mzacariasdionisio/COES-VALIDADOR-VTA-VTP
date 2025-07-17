using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Migraciones.Models
{
    public class HttpResponseModel
    {
        public string Detalle { get; set; }
        public string Mensaje { get; set; }
        public int NroRegistros { get; set; }
        public string Resultado { get; set; }
    }
}