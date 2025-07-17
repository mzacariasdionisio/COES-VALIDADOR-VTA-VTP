using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_RESUMENGEN
    /// </summary>
    public class WbResumengenDTO : EntityBase
    {
        public int Resgencodi { get; set; } 
        public decimal? Resgenactual { get; set; } 
        public decimal? Resgenanterior { get; set; } 
        public decimal? Resgenvariacion { get; set; }
        public DateTime Resgenfecha { get; set; }
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
