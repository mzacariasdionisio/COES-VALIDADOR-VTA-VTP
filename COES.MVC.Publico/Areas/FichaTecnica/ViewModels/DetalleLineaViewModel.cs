using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.FichaTecnica.ViewModels
{
    public class DetalleLineaViewModel
    {
        public string NombreLinea { get; set; }
        public string NombreEmpresa { get; set; }

        public string Capacidadcontinua { get; set; }
        public string LongitudLinea { get; set; }
        public string NivelTension { get; set; }
        public string R0m { get; set; }
        public string G1 { get; set; }
        public string X1 { get; set; }
        public string X0 { get; set; }
        public string R1 { get; set; }
        public string R0 { get; set; }
        public string B1 { get; set; }
        public string B0 { get; set; }
        public string Xom { get; set; }
        public string EstacionInicio { get; set; }
        public string EstacionFin { get; set; }
    }
}