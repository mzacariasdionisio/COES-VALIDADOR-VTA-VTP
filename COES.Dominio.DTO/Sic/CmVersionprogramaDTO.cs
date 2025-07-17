using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_VERSIONPROGRAMA
    /// </summary>
    public class CmVersionprogramaDTO : EntityBase
    {
        public int Cmveprcodi { get; set; } 
        public int Cmgncorrelativo { get; set; } 
        public string Cmveprvalor { get; set; } 
        public string Cmveprtipoprograma { get; set; }
        public string Cmveprtipoestimador { get; set; }
        public string Cmveprtipocorrida { get; set; }
        public int? Topcodi { get; set; }
        public int? Cmveprversion { get; set; }
    }
}
