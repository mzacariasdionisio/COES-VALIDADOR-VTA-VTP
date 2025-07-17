using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.IND.Models
{
    public class IndModel
    {

        public enum TipoEstado
        {
            Error = 0,
            Ok = 1
        }

        public int Estado { get; set; }
        public string Mensaje { get; set; }
        public string ExceptionMessaje { get; set; }
    }
}