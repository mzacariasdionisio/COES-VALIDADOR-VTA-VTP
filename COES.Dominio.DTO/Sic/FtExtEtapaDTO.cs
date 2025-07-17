using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ETAPA
    /// </summary>
    public partial class FtExtEtapaDTO : EntityBase
    {
        public int Ftetcodi { get; set; }
        public string Ftetnombre { get; set; }
    }

    public partial class FtExtEtapaDTO 
    {
        public bool EtapaDefault { get; set; }
    }
}
