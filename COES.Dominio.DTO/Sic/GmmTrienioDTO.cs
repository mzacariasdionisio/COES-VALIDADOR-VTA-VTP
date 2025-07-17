using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla GMM_TRIENIO
    /// </summary>
    public class GmmTrienioDTO : EntityBase
    {
        public int TRICODI { get; set; }       
        public int EMPGCODI { get; set; }
        public int TRINUMINC { get; set; }
        public int TRISECUENCIA { get; set; }
        public DateTime? TRIFECLIMITE { get; set; }
        public DateTime? TRIFECINICIO { get; set; }
    }
}
