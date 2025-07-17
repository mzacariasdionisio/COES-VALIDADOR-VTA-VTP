using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Demo.Models
{
    public class DemoModel
    {
        public string Nombre { get; set; }
        public List<SiPruebaDTO> ListaPrueba { get; set; }
    }
}