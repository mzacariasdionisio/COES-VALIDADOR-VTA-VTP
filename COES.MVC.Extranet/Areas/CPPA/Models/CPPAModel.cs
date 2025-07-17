using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.CPPA.Models
{
    public class CPPAModel
    {
        #region General
        //Acciones de permisos y validación
        public bool bNuevo { get; set; }
        public bool bEditar { get; set; }
        public bool bGrabar { get; set; }
        public bool bEliminar { get; set; }
        public string sError { get; set; }
        public string sMensaje { get; set; }
        public string sTipo { get; set; }
        public string sResultado { get; set; }
        public string sDetalle { get; set; }

        #endregion

        #region REQ020
        public List<GenericoDTO> ListaAnio { get; set; }
        public List<GenericoDTO> ListaReporte { get; set; }
        public List<GenericoDTO> ListaReporte2 { get; set; }
        public List<CpaRevisionDTO> ListRevision { get; set; }
        public string EstadoPublicacion { get; set; }
        #endregion
    }
}