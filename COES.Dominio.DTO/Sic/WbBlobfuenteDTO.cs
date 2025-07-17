using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_BLOBFUENTE
    /// </summary>
    public class WbBlobfuenteDTO : EntityBase
    {
        public int Blofuecodi { get; set; }
        public string Blofuenomb { get; set; }
        public string Blofueestado { get; set; }
        public string Blofueusucreacion { get; set; }
        public DateTime? Blofuefeccreacion { get; set; }
    }
}
