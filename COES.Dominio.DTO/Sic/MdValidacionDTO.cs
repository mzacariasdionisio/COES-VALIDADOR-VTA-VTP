using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MD_VALIDACION
    /// </summary>
    public class MdValidacionDTO : EntityBase
    {
        public int Emprcodi { get; set; } 
        public DateTime Validames { get; set; } 
        public DateTime? Validafecha { get; set; } 
        public string Validaestado { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; }
        public int Tipoemprcodi { get; set; }
    }
}
