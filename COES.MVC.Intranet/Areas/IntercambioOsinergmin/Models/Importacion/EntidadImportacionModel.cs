using System.ComponentModel.DataAnnotations;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Importacion
{
    public class EntidadImportacionModel
    {
        [Display(Name = "Entidad")]
        public string Nombre { get; set; }
        [Display(Name = "No Registros COES")]
        public int NroRegistrosCoes { get; set; }
        [Display(Name = "No Registros Osinergmin")]
        public int NroRegistrosOsinergmin { get; set; }
        [Display(Name = "Selec.")]
        public bool EstaSeleccionado { get; set; }
        [Display(Name = "Cod. Ent")]
        public int CantidadErrores { get; set; }
        public string mensaje { get; set; }
        public int resultado { get; set; }
        public int empresasPendientes { get; set; }
        public List<EmpresasRemision> ListasEmpresas { get; set; }
        public List<IioControlImportacionDTO> ListaControlTabla04 { get; set; }
        public List<IioControlImportacionDTO> ListaControlTabla05 { get; set; }
        public List<string> lstMensajes { get; set; }

        public List<IioControlImportacionDTO> ListaControlTabla02 { get; set; }

    }
}