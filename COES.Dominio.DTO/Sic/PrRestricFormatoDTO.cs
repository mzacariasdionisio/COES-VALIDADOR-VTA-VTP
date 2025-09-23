using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_RESTRIC_FORMATO
    /// </summary>
    public class PrRestricFormatoDTO : EntityBase
    {
        public int Resfmtcodi { get; set; } 
        public string Resfmtnombre { get; set; } 
        public string Resfmtusucreacion { get; set; } 
        public DateTime? Resfmtfeccreacion { get; set; } 
    }
}
