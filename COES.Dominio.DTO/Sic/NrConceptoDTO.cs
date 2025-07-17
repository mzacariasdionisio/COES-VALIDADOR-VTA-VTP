using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla NR_CONCEPTO
    /// </summary>
    public class NrConceptoDTO : EntityBase
    {
        public int Nrcptcodi { get; set; } 
        public int? Nrsmodcodi { get; set; } 
        public string Nrcptabrev { get; set; } 
        public string Nrcptdescripcion { get; set; } 
        public int? Nrcptorden { get; set; } 
        public string Nrcpteliminado { get; set; } 
        public int? Nrcptpadre { get; set; } 
        public string Nrsmodnombre { get; set; }
    }
}
