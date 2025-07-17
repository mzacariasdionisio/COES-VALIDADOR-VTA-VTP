using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    public class PropiedadesModel
    {
        //termicas
        public PrGrupoDTO CargaDescargaModo { get; set; }
        public List<PrGrupoDTO> ListaCargaDescargaModo { get; set; }
        //hidro
        public EqEquipoDTO CargaDescargaEquipo { get; set; }
        public List<EqEquipoDTO> ListaCargaDescargaEquipo { get; set; }

        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public string CargaValor { get; set; }
        public string DescargaValor { get; set; }
        public int Accion { get; set; }
        public string[][] Datos { get; set; }
        public int Famcodi { get; set; }
    }

}
