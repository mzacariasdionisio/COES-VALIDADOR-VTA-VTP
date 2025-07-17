using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MAP_VERSION
    /// </summary>
    public class MapVersionDTO : EntityBase
    {
        public int Vermcodi { get; set; } 
        public DateTime? Vermfechaperiodo { get; set; } 
        public string Vermusucreacion { get; set; } 
        public int? Vermestado { get; set; } 
        public DateTime? Vermfeccreacion { get; set; } 
        public string Vermusumodificacion { get; set; } 
        public DateTime? Vermfecmodificacion { get; set; }
        public int Vermnumero { get; set; } 
    }
}
