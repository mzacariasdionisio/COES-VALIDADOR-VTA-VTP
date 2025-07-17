using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.FichaTecnica.ViewModels
{
    public class DetalleCentralSolarViewModel
    {
        public string CodCentral { get; set; }
        public string NombreCentral { get; set; }
        public string NombreEmpresa { get; set; }
        public string IdCentral { get; set; }

        public string Sbrut { get; set; }
        public string Pinst { get; set; }
        public string NumMod { get; set; }
        public string TecnSeg { get; set; }
        public string AngIncl { get; set; }
        public string DistMod { get; set; }
        public string HEqPCAn { get; set; }
        public string HEqPCMes { get; set; }
        public string CurGen { get; set; }
        public string PenMax { get; set; }
        public string CurPot { get; set; }
        public string ConTen { get; set; }
        public string ConFrec { get; set; }
        public string NivMTen { get; set; }
        public string IntCC { get; set; }
        public string DiaUni { get; set; }
    }
}