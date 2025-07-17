using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RI_DETALLE_REVISION
    /// </summary>
    public class RiDetalleRevisionDTO : EntityBase
    {
        public int Dervcodi { get; set; } 
        public string Dervcampo { get; set; } 
        public string Dervvalor { get; set; } 
        public string Dervobservacion { get; set; } 
        public string Dervadjunto { get; set; } 
        public string Dervvaloradjunto { get; set; }
        public string Dervestadorevision { get; set; }
        public string Dervestado { get; set; }
        public int? Revicodi { get; set; } 
        public string Dervusucreacion { get; set; } 
        public DateTime? Dervfeccreacion { get; set; } 
        public string Dervusumoficicacion { get; set; } 
        public DateTime? Dervfecmodificacion { get; set; } 
    }
}
