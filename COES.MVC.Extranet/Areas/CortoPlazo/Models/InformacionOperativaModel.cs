using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;

namespace COES.MVC.Extranet.Areas.CortoPlazo.Models
{
    /// <summary>
    /// Model para presentacion de resultados
    /// </summary>
    public class InformacionOperativaModel
    {
        public string FechaConsulta { get; set; }
        public List<CmCostomarginalDTO> Listado { get; set; }
        public string PathResultado { get; set; }
        public string PathPrincipal { get; set; }
        public string FechaInicio { get; set; }
        public string FechaInicioAnterior { get; set; }
        public string FechaFin { get; set; }
        public bool OpcionGrabar { get; set; }

        #region Mejoras CMgN
        public string FechaEjecucion { get; set; }
        public string UsuarioEjecucion { get; set; }
        public string TipoProceso { get; set; }
        #endregion

        #region CMgCP_PR07

        public string FechaVigenciaPR07 { get; set; }

        #endregion

        public string BaseDirectory { get; set; }
        public List<FileData> DocumentList { get; set; }
        public List<BreadCrumb> BreadList { get; set; }
        public string Origen { get; set; }
    }
}