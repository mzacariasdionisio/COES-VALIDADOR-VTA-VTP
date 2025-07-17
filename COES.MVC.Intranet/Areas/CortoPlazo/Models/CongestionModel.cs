using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Models
{
    /// <summary>
    /// Model para el manejo de datos de congestión en Líneas de Transmisión
    /// </summary>
    public class CongestionModel
    {

        public string FechaEjecutadoInicio { get; set; }
        public string FechaEjecutadoFin { get; set; }
        public List<PrCongestionDTO> ListaCongestionSimple { get; set; }
        public List<PrCongestionDTO> ListaCongestionConjunto { get; set; }
        public PrCongestionDTO Entidad { get; set; }

        public int Codigo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int FlagHoraTR { get; set; }
        public string HoraTR { get; set; }

        public int? CodigoLinea { get; set; }
        public int? CodigoGrupo { get; set; }
        public string Barra1 { get; set; }
        public string Barra2 { get; set; }
        public string IndTipo { get; set; }

        public List<EqCongestionConfigDTO> ListaLinea { get; set; }
        public List<EqGrupoLineaDTO> ListaGrupo { get; set; }
        public List<string> ListaBarra1 { get; set; }
        public List<string> ListaBarra2 { get; set; }

        public string[][] Datos { get; set; }

        
        public List<EqCongestionConfigDTO> ListLineaEquipo { get; set; }
        public List<EqCongestionConfigDTO> ListTrafo2dEquipo { get; set; }
        public List<EqCongestionConfigDTO> ListTrafo3dEquipo { get; set; }
        public List<EqGrupoLineaDTO> ListGrupoLinea { get; set; }

        public List<EqGrupoLineaDTO> ListaGrupoLineaMinimo { get; set; }
        public List<PrCongestionDTO> ListaCongestion { get; set; }

        public List<CongestionFuente> ListaFuente { get; set; }

        public int Registro { get; set; }

        public List<PrGrupoDTO> ListaGrupoDespacho { get; set; }

        #region Regiones_seguridad

        public List<CmRegionseguridadDTO> ListaRegionSeguridad { get; set; }

        #endregion
    }

    public class CongestionFuente
    {
        public int Id { get; set; }
        public string Texto { get; set; }
    }
}