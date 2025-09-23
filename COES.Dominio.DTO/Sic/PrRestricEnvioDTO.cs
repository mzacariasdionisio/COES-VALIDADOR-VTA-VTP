using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_RESTRIC_ENVIO
    /// </summary>
    public class PrRestricEnvioDTO : EntityBase
    {
        public int Resenvcodi { get; set; } 
        public DateTime? Resenvfechaperiodo { get; set; } 
        public string Resenvusucreacion { get; set; } 
        public DateTime? Resenvfeccreacion { get; set; } 
        public string Resenvestado { get; set; } 
        public int Resfmtcodi { get; set; } 
    }
}
