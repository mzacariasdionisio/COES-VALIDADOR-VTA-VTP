using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_LOGMIGRA
    /// </summary>
    public class SiLogmigraDTO : EntityBase
    {
        public int Migracodi { get; set; } 
        public int Logcodi { get; set; } 
        public string Logmigusucreacion { get; set; } 
        public DateTime? Logmigfeccreacion { get; set; }
        public int? Logmigtipo { get; set; }
        public int? Miqubacodi { get; set; }
    }
}
