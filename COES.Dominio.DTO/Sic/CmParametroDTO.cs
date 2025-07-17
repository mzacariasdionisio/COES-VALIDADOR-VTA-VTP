using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_PARAMETRO
    /// </summary>
    public class CmParametroDTO : EntityBase
    {
        public int Cmparcodi { get; set; } 
        public string Cmparnombre { get; set; } 
        public string Cmparvalor { get; set; } 
        public string Cmparlastuser { get; set; } 
        public DateTime? Cmparlastdate { get; set; }
        public decimal Cmparinferior { get; set; }
        public decimal Cmparsuperior { get; set; }

    }
}
