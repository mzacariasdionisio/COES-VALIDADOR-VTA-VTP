using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_VERSIONREP
    /// </summary>
    public class SpoVersionrepDTO : EntityBase
    {
        public int Verrcodi { get; set; } 
        public int? Repcodi { get; set; }
        public DateTime? Verrfechaperiodo { get; set; } 
        public string Verrusucreacion { get; set; }
        public int? Verrestado { get; set; }
        public DateTime? Verrfeccreacion { get; set; } 
        public string Verrusumodificacion { get; set; }
        public DateTime? Verrfecmodificacion { get; set; }
        public int? Verrnro { get; set; } 

    }
}
