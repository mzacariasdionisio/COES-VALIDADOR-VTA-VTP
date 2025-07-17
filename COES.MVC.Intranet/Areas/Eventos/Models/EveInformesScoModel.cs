using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Eventos.Helper;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class EveInformesScoModel
    {
        public int Eveninfcodi { get; set; }
        public string PortalWeb { get; set; }
        public string Eveinfcodigo { get; set; }
    }
}