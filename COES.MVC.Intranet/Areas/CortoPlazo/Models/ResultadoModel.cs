using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    /// <summary>
    /// Model para presentacion de resultados
    /// </summary>
    public class ResultadoModel
    {
        public string FechaConsulta { get; set; }
        public List<CmCostomarginalDTO> Listado { get; set; }
        public string PathResultado { get; set; }
        public string PathPrincipal { get; set; }
        public string FechaInicio { get; set; }
        public string FechaInicioAnterior { get; set; }
        public string FechaFin { get; set; }
        public List<CmGeneracionEmsDTO> ListadoGeneracionEms { get; set; } 
        public List<CmRestriccionDTO> ListaRestricciones { get; set; }
        public CmVersionprogramaDTO VersionPrograma { get; set; }
        public bool OpcionGrabar { get; set; }

        #region Mejoras CMgN
        public string FechaEjecucion { get; set; }
        public string UsuarioEjecucion { get; set; }
        public string TipoProceso { get; set; }
        #endregion

        #region CMgCP_PR07

        public string FechaVigenciaPR07 { get; set; }

        #endregion
    }

    public class ReprocesamientoModel
    {
        public List<ReprocesoItemModel> ListaDetalle { get; set; }
    }

    public class ReprocesoItemModel
    {
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Ems { get; set; }
        public List<CpTopologiaDTO> ListaEscenarios { get; set; }
        public int Id { get; set; }
    }
}