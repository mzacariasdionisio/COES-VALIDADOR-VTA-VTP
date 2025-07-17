using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_ESTADCANAL_SP7
    /// </summary>
    public class TrEstadcanalSp7DTO : EntityBase
    {
        public int Canalcodi { get; set; } 
        public DateTime Fecha { get; set; } 
        public decimal? Tvalido { get; set; } 
        public decimal? Tcong { get; set; } 
        public decimal? Tindet { get; set; } 
        public decimal? Tnnval { get; set; } 
        public int? Ultcalidad { get; set; } 
        public DateTime? Ultcambio { get; set; } 
        public DateTime? Ultcambioe { get; set; } 
        public decimal? Ultvalor { get; set; } 
        public decimal? Tretraso { get; set; } 
        public int? Emprcodi { get; set; } 
        public DateTime? Trstdlastdate { get; set; } 
        public int? Numregistros { get; set; } 
        public string Trstdingreso { get; set; }
        public string zona { get; set; }
        public decimal dispo { get; set; }
        public string nombcanal { get; set; }
        public string unidad { get; set; }
        public string iccp { get; set; }
    }
}
