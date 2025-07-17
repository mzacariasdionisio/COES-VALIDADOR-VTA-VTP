using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_PTOINTERRUP
    /// </summary>
    public class EvePtointerrupDTO : EntityBase
    {
        public int Ptointerrcodi { get; set; } 
        public int? Ptoentregacodi { get; set; } 
        public string Ptointerrupnomb { get; set; } 
        public int? Ptointerrupsectip { get; set; }
        public string Ptoentrenomb { get; set; }
        public int Clientecodi { get; set; }
        public int Equicodi { get; set; }
        public string Emprnomb { get; set; }
    }
}
