using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_COMITE
    /// </summary>
    public class WbComiteDTO : EntityBase
    {
        public int Comitecodi { get; set; } 
        public string Comitename { get; set; } 
        public string Comiteestado { get; set; } 
        public string Comiteusucreacion { get; set; } 
        public string Comiteusumodificacion { get; set; } 
        public DateTime? Comitefeccreacion { get; set; } 
        public DateTime? Comitefecmodificacion { get; set; } 
    }
}
