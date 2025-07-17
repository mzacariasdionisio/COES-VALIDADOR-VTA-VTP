using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Models
{
    public class AdminModel
    {

    }

    /// <summary>
    /// Model para el CRUD de CO_PERIODO
    /// </summary>
    public class PeriodoModel
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int Codigo { get; set; }
        public CoPeriodoDTO Entidad { get; set; }
        public List<CoPeriodoDTO> Listado { get; set; }
        public List<int> ListaAnios { get; set; }
        public List<CoPeriodoDTO> ListaMeses { get; set; }
        public List<CoEnvioliquidacionDTO> ListadoEnvio { get; set; }
        public List<CoMedicion48DTO> ListaBandas { get; set; }
        public string DesPeriodo { get; set; }
    }

    public class VersionModel
    {
        public CoVersionDTO Entidad { get; set; }
        public List<CoVersionbaseDTO> ListaVersionBase { get; set; }
        public List<CoVersionDTO> Listado { get; set; }
        public List<CoUrsEspecialDTO> ListaUrsEspecial { get; set; }
        public List<CoUrsEspecialbaseDTO> ListaBaseUrsEspecial { get; set; }
        public List<CoUrsEspecialDTO> ListaUrs { get; set; }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Estado { get; set; }
        public int TipoVersion { get; set; }
        public int CodigoPeriodo { get; set; }
        public string ListaGrupo { get; set; }       
        public string DesPeriodo { get; set; }
    }

    #region RSF_PR22
    public class ConfiguracionUrsModel
    {
        public List<CoPeriodoDTO> ListaPeriodo { get; set; }
        public List<CoVersionDTO> ListaVersion { get; set; }
        public List<CoConfiguracionUrsDTO> ListadoConfiguracionURS { get; set; }
        public List<CoMedicion48DTO> ListadoURS { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string FechaActual { get; set; }
        public string UltimaVigencia { get; set; }
        public List<int> ListaAnios { get; set; }
        public int Anio { get; set; }
        public List<CoPeriodoDTO> ListaPeriodos { get; set; }
        public bool HayDiferencia { get; set; }
        public string Rango { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public List<CoConfiguracionGenDTO> ListaGenerador { get; set; }
        public int IdConfiguracionDet { get; set; }
    }
    #endregion
}