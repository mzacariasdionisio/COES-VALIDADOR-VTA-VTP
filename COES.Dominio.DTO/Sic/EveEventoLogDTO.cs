using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_EVENTO_LOG
    /// </summary>
    public class EveEventoLogDTO : EntityBase
    {
        public int Evelogcodi { get; set; } 
        public int Evencodi { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Desoperacion { get; set; } 
    }
}
