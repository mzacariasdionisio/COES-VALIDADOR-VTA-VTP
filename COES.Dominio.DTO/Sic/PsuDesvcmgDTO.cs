using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PSU_DESVCMG
    /// </summary>
    public class PsuDesvcmgDTO : EntityBase
    {
        public DateTime Desvfecha { get; set; } 
        public decimal? Cmgrpunta { get; set; } 
        public decimal? Cmgrmedia { get; set; } 
        public decimal? Cmgrbase { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
    }
}
