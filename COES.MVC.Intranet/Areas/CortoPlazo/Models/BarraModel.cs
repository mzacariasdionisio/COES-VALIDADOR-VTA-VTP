using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class BarraModel
    {
        public List<CmConfigbarraDTO> Listado { get; set; }
        public CmConfigbarraDTO Entidad { get; set; }
        public string Nodobarra { get; set; }
        public string Nombrebarra { get; set; }
        public string IndPublicar { get; set; }
        public string Estado { get; set; }
        public int Codigo { get; set; }
        public string CoordenadaX { get; set; }
        public string CoordenadaY { get; set; }
        public string NombreNCP { get; set; }
        public string IndDefecto { get; set; }
        [AllowHtml]
        public string ListaCoordenada { get; set; }
        public string NombreTna { get; set; }

        #region Mejoras CMgN

        public int? Recurcodi { get; set; }
        public int? Canalcodi { get; set; }

        #endregion
    }
}