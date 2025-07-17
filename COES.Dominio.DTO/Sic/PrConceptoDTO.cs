using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_CONCEPTO
    /// </summary>
    public partial class PrConceptoDTO : EntityBase
    {
        public int Catecodi { get; set; }
        public int Concepcodi { get; set; }
        public int Conceppadre { get; set; }
        public string Concepabrev { get; set; }
        public string Concepdesc { get; set; }
        public string Concepunid { get; set; }
        public string Conceptipo { get; set; }
        public int Conceporden { get; set; }
        public string Concepfichatec { get; set; }

        public string Concepactivo { get; set; }// preguntar
        public string Concepnombficha { get; set; }
        public string Concepdefinicion { get; set; }
        public int? Conceptipolong1 { get; set; }
        public int? Conceptipolong2 { get; set; }
        public string Concepusucreacion { get; set; }
        public DateTime? Concepfeccreacion { get; set; }
        public string Concepusumodificacion { get; set; }
        public DateTime? Concepfecmodificacion { get; set; }
        public string Concepocultocomentario { get; set; }

        public decimal? Concepliminf { get; set; }
        public decimal? Conceplimsup { get; set; }
        public int? Concepflagcolor { get; set; }

        #region MigracionSGOCOES-GrupoB
        public string Catenomb { get; set; }
        public int Conceppropeq { get; set; }
        public string ConfiguradoVisualizacionDesc { get; set; }
        #endregion
    }

    public partial class PrConceptoDTO
    {
        public string EstiloEstado { get; set; }
        public bool ExisteCambio { get; set; }
        public string ConcepfichatecDesc { get; set; }
        public string ConcepfeccreacionDesc { get; set; }
        public string ConcepfecmodificacionDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string StrConcepliminf { get; set; }
        public string StrConceplimsup { get; set; }
        public string StrConceptipolong1 { get; set; }
        public string StrConceptipolong2 { get; set; }

        public int NroItem { get; set; } // Indice para la importación de conceptos
        public string Observaciones { get; set; }
        public bool ChkMensaje { get; set; }
    }
}
