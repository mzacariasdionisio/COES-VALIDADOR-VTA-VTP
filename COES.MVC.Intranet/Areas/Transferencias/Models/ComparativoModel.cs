using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class ComparativoModel
    {
        public decimal valorInicial { get; set; }
        public decimal valorFinal { get; set; }
        public string fecha { get; set; }

        public decimal variacion { get; set; }
    }
}