using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_COSTO_INCREMENTAL
    /// </summary>
    public class CmCostoIncrementalDTO : EntityBase
    {
        public int Cmcicodi { get; set; } 
        public int? Equicodi { get; set; } 
        public int Grupocodi { get; set; } 
        public DateTime Cmcifecha { get; set; } 
        public int Cmciperiodo { get; set; } 
        public int Cmgncorrelativo { get; set; } 
        public decimal? Cmcitramo1 { get; set; } 
        public decimal? Cmcitramo2 { get; set; } 
        public string Cmciusucreacion { get; set; } 
        public DateTime? Cmcifeccreacion { get; set; } 
        public string Cmciusumodificacion { get; set; } 
        public DateTime? Cmcifecmodificacion { get; set; } 
        public int Grupopadre { get; set; }
        public string Equinomb { get; set; }
    }
}
