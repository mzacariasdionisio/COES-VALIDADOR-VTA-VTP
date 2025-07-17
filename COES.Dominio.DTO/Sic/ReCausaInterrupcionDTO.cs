using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_CAUSA_INTERRUPCION
    /// </summary>
    public class ReCausaInterrupcionDTO : EntityBase
    {
        public int Recintcodi { get; set; } 
        public int? Retintcodi { get; set; } 
        public string Recintnombre { get; set; } 
        public string Recintestado { get; set; } 
        public string Recintusucreacion { get; set; } 
        public DateTime? Recintfeccreacion { get; set; } 
        public string Recintusumodificacion { get; set; } 
        public DateTime? Recintfecmodificacion { get; set; } 
        public string Retintnombre { get; set; }
        public decimal? Reindki { get; set; }
        public decimal? Reindni { get; set; }
        public string IndicadorEdicion { get; set; }
    }
}
