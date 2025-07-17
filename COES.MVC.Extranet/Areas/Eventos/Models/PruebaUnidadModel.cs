using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Eventos.Models
{
    public class PruebaUnidadModel : FormatoModel
    {
        public Boolean EnabledInicio { get; set; }
        public List<decimal> ListaInicio { get; set; }
        public Boolean IsExcelWeb { get; set; }
        public string FechaNext { get; set; }
    }
}