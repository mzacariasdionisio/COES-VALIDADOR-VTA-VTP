using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FW_COUNTER
    /// </summary>
    public class FwCounterDTO : EntityBase
    {
        public string Tablename { get; set; } 
        public int? Maxcount { get; set; } 
    }
}
