using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_INDEMPRESAT_SP7
    /// </summary>
    public class TrIndempresatSp7DTO : EntityBase
    {
        public DateTime Fecha { get; set; } 
        public int Emprcodi { get; set; } 
        public int Media { get; set; } 
        public int Factor { get; set; } 
        public int Media2 { get; set; } 
        public int Factor2 { get; set; } 
        public int Findispon { get; set; } 
        public int Ciccpe { get; set; } 
        public int? Ciccpea { get; set; } 
        public int? Factorg { get; set; } 
        public DateTime? Lastdate { get; set; }
        public decimal disponibilidad { get; set; }
        public double zona { get; set; }
    }
}
