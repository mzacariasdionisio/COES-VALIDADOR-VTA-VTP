using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_VERSION_DET
    /// </summary>
    public partial class SiVersionDetDTO : EntityBase
    {
        public int Versdtcodi { get; set; }
        public int Mrepcodi { get; set; }
        public int Verscodi { get; set; }
        public decimal? Versdtnroreporte { get; set; }
        public byte[] Versdtdatos { get; set; }
    }

    public partial class SiVersionDetDTO 
    {
        public List<SiVersionDatDTO> ListaDat { get; set; }
    }
}
