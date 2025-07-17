using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.FichaTecnica.ViewModels
{
    public class DetalleCentralEolicaViewModel
    {
        public string IdCentral;
        public string IdFamilia;
        public string NombreCentral;
        public string NombreEmpresa;
        public string CódigoCentral;

        public string PotApaBrut { get; set; }
        public string PotInstNom { get; set; }
        public string NumAerog { get; set; }
        public string HeqPCAno { get; set; }
        public string HeqPCMes { get; set; }
        public string CurPotReac { get; set; }
        public string CurPotPCR { get; set; }
        public string SistCont { get; set; }
        public string ContTen { get; set; }
        public string ContFrec { get; set; }
        public string NivMedTen { get; set; }
        public string IntCortCirc { get; set; }
        public string DiaUnif { get; set; }
    }
}