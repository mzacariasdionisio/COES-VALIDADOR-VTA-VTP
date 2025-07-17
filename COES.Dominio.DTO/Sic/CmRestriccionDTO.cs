using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_RESTRICCION
    /// </summary>
    public class CmRestriccionDTO : EntityBase
    {
        public int Cmrestcodi { get; set; } 
        public int Cmgncorrelativo { get; set; } 
        public int Equicodi { get; set; } 
        public int Subcausacodi { get; set; } 
        public string Equiabrev { get; set; }
        public string Subcausaabrev { get; set; }
    }
}
