using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Models
{
    public class AyudaMovilModel
    {
        public List<WbAyudaappDTO> Listado { get; set; }
        public int Codigo { get; set; }
        public string IdVentana { get; set; }
        public string NombreVentana { get; set; }
        public string Indicador { get; set; }
        [AllowHtml]
        public string TextoAyuda { get; set; }
        [AllowHtml]
        public string TextoAyudaEng { get; set; }
    }
}