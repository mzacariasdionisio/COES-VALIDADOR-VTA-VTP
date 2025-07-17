using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Yupana.Models
{
    public class VariableModel
    {
        public string NombreVariable { get; set; }
        public string IdVariable { get; set; }
        public short IdRestriccion { get; set; }
        public short TipoParametro { get; set; }
        public double? Valor { get; set; }
    }
}