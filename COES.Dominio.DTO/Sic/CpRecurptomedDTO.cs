using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_RECURPTOMED
    /// </summary>
    public class CpRecurptomedDTO : EntityBase
    {
        public int Recurcodi { get; set; } 
        public int Topcodi { get; set; } 
        public int Ptomedicodi { get; set; } 
        public int? Recptok { get; set; }

        public string Ptomedinomb { get; set; }
        public bool IsValid { get; set; }
        public string Famabrev { get; set; }
        public string Catnombre { get; set; }
        public string Equinomb { get; set; }
        public string Ptomedibarranomb { get; set; }
    }
}
