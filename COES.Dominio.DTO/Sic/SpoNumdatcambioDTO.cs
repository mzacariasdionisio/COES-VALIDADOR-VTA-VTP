using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_NUMDATCAMBIO
    /// </summary>
    public class SpoNumdatcambioDTO : EntityBase
    {
        public int Numdcbcodi { get; set; } 
        public int? Verncodi { get; set; } 
        public int? Sconcodi { get; set; } 
        public int? Clasicodi { get; set; } 
        public int? Tipoinfocodi { get; set; } 
        public decimal? Numdcbvalor { get; set; }
        public DateTime Numdcbfechainicio { get; set; }
        public DateTime? Numdcbfechafin { get; set; } 
    }
}
