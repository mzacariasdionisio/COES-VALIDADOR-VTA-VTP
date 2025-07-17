using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_TOLERANCIA_PERIODO
    /// </summary>
    public class ReToleranciaPeriodoDTO : EntityBase
    {
        public int? Retolninf { get; set; } 
        public int? Retoldinf { get; set; } 
        public int? Retolnsup { get; set; } 
        public int? Retoldsup { get; set; } 
        public string Retolusucreacion { get; set; } 
        public DateTime? Retolfeccreacion { get; set; } 
        public string Retolusumodificacion { get; set; } 
        public DateTime? Retolfecmodificacion { get; set; } 
        public int Retolcodi { get; set; } 
        public int? Repercodi { get; set; } 
        public int? Rentcodi { get; set; } 
        public string Rentabrev { get; set; }
    }
}
