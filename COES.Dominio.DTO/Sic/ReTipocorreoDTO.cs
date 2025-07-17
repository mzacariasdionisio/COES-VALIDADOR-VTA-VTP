using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_TIPOCORREO
    /// </summary>
    public class ReTipocorreoDTO : EntityBase
    {
        public int Retcorcodi { get; set; } 
        public string Retcornombre { get; set; } 
        public string Retcorusucreacion { get; set; } 
        public DateTime? Retcorfeccreacion { get; set; } 
        public string Retcorusumodificacion { get; set; } 
        public DateTime? Retcorfecmodificacion { get; set; } 
    }
}
