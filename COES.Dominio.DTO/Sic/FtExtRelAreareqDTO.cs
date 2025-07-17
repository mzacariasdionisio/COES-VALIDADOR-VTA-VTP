using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_REL_AREAREQ
    /// </summary>
    public partial class FtExtRelAreareqDTO : EntityBase
    {
        public int Faremcodi { get; set; } 
        public int Fevrqcodi { get; set; } 
        public int Frracodi { get; set; } 
        public string Frraestado { get; set; } 
        public DateTime? Frrafeccreacion { get; set; } 
        public string Frrausucreacion { get; set; } 
        public DateTime? Frrafecmodificacion { get; set; } 
        public string Frrausumodificacion { get; set; }
        public string Frraflaghidro { get; set; }
        public string Frraflagtermo { get; set; }
        public string Frraflagsolar { get; set; }
        public string Frraflageolico { get; set; }
    }

    public partial class FtExtRelAreareqDTO
    {
        public string Strflaghidro { get; set; }
        public string Strflagtermo { get; set; }
        public string Strflagsolar { get; set; }
        public string Strflageolico { get; set; }

        public string Chkhidro { get; set; }
        public string Chktermo { get; set; }
        public string Chksolar { get; set; }
        public string Chkolico { get; set; }
        public string RequisitoItem { get; set; }

        public string Faremnombre { get; set; }
    }
}
