using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_CAUSAEVENTO
    /// </summary>
    public partial class EveCausaeventoDTO : EntityBase
    {
        public int Causaevencodi { get; set; } 
        public string Causaevendesc { get; set; } 
        public string Causaevenabrev { get; set; } 
    }

    public partial class EveCausaeventoDTO 
    {
        public int Total { get; set; }
    }
}
