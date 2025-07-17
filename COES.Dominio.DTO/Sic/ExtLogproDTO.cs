using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EXT_LOGPRO
    /// </summary>
    public class ExtLogproDTO : EntityBase
    {
        public int? Mencodi { get; set; } 
        public string Logpdetmen { get; set; } 
        public DateTime? Logpfechor { get; set; } 
        public int? Logpsecuen { get; set; } 
        public int? Earcodi { get; set; } 
 
    }
}
