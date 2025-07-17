using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_FCOSTOF
    /// </summary>
    public class CpFcostofDTO : EntityBase
    {
        public string Fcfembalses { get; set; } 
        public int? Fcfnumcortes { get; set; } 
        public int Topcodi { get; set; } 
    }
}
