using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PSU_DESVCMGSNC
    /// </summary>
    public class PsuDesvcmgsncDTO : EntityBase
    {
        public DateTime Desvfecha { get; set; }      
        public decimal? Cmgsnc { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
    }
}
