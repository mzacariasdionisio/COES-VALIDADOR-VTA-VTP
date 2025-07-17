using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_FORMATO
    /// </summary>
    public partial class FtExtFormatoDTO : EntityBase
    {
        public int Ftfmtcodi { get; set; }
        public int Fteqcodi { get; set; } 
        public int Ftetcodi { get; set; } 
    }

    public partial class FtExtFormatoDTO 
    {
        public int? Famcodi { get; set; }
        public int? Catecodi { get; set; }
    }
}
