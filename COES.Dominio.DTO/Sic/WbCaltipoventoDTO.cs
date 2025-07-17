using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_CALTIPOVENTO
    /// </summary>
    public class WbCaltipoventoDTO : EntityBase
    {
        public int Tipcalcodi { get; set; } 
        public string Tipcaldesc { get; set; } 
        public string Tipcalcolor { get; set; } 
        public string Tipcalicono { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
