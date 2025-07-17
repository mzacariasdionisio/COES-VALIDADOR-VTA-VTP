using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_FICTECITEM_NOTA
    /// </summary>
    public class FtFictecItemNotaDTO : EntityBase
    {
        public int Ftitntcodi { get; set; }
        public int Ftitcodi { get; set; }
        public int Ftnotacodi { get; set; }
        public DateTime? Ftitntfecha { get; set; }

        public int? Ftnotanum { get; set; }
    }
}
