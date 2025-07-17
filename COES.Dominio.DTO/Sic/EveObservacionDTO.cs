using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_OBSERVACION
    /// </summary>
    public class EveObservacionDTO : EntityBase
    {
        public int Obscodi { get; set; } 
        public int? Subcausacodi { get; set; } 
        public DateTime? Obsfecha { get; set; } 
        public string Obsdescrip { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public int? Evenclasecodi { get; set; } 
    }
}
