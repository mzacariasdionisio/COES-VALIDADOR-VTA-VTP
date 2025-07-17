using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_INDICADOR_PERIODO
    /// </summary>
    public class ReIndicadorPeriodoDTO : EntityBase
    {
        public int Reindcodi { get; set; } 
        public int? Repercodi { get; set; } 
        public int? Recintcodi { get; set; } 
        public decimal? Reindki { get; set; } 
        public decimal? Reindni { get; set; } 
        public string Reindusucreacion { get; set; } 
        public DateTime? Reindfeccreacion { get; set; } 
        public string Reindusumodificacion { get; set; } 
        public DateTime? Reindfecmodificacion { get; set; } 
    }
}
