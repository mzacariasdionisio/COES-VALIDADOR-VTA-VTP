using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAGRUPO
    /// </summary>
    public class SiMigragrupoDTO : EntityBase
    {
        public int Miggrucodi { get; set; } 
        public int Migempcodi { get; set; } 
        public int Grupocodimigra { get; set; } 
        public int Grupocodibajanuevo { get; set; } 
        public string Miggruusucreacion { get; set; } 
        public DateTime? Miggrufeccreacion { get; set; } 
    }
}
