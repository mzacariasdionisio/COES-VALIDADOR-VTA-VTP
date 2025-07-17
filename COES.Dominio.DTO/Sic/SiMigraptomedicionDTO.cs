using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAPTOMEDICION
    /// </summary>
    public class SiMigraPtomedicionDTO : EntityBase
    {
        public int Mgpmedcodi { get; set; } 
        public int Migempcodi { get; set; } 
        public int? Ptomedcodimigra { get; set; } 
        public int? Ptomedbajanuevo { get; set; } 
        public string Mgpmedusucreacion { get; set; } 
        public DateTime? Mgpmedfeccreacion { get; set; } 
    }
}
