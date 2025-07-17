using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.RegistroIntegrante.Models
{
    public class RepresentanteModel
    {
        public int Id { get; set; }
        public int Rptecodi { get; set; }
        public string Rptetipo { get; set; }
        public string Rptetiprepresentantelegal { get; set; }
        public string Rptebaja { get; set; }
        public string Rptetipdocidentidad { get; set; }
        public string Rptedocidentidad { get; set; }
        public string Rptedocidentidadadj { get; set; }
        public string Rptenombres { get; set; }
        public string Rpteapellidos { get; set; }
        public string Rptevigenciapoder { get; set; }
        public string Rptecargoempresa { get; set; }
        public string Rptetelefono { get; set; }
        public string Rptetelfmovil { get; set; }
        public string Rptecorreoelectronico { get; set; }
        public DateTime Rptefeccreacion { get; set; }
        public string Rpteusucreacion { get; set; }
        public int Emprcodi { get; set; }
        public string Rpteusumodificacion { get; set; }
        public DateTime Rptefecmodificacion { get; set; }
        public string Rptefechavigenciapoder { get; set; }

        public HttpPostedFileBase DNI { get; set; }
        public HttpPostedFileBase VigenciaPoder { get; set; }
        public string RpteObservacion { get; set; }
    }
}