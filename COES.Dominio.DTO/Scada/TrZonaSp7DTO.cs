using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_ZONA_SP7
    /// </summary>
    public class TrZonaSp7DTO : EntityBase
    {
        public int Zonacodi { get; set; } 
        public string Zonanomb { get; set; } 
        public int? Emprcodi { get; set; } 
        public string Zonaabrev { get; set; } 
        public int? Zonasiid { get; set; } 
        public string Zonausucreacion { get; set; } 
        public DateTime? Zonafeccreacion { get; set; } 
        public string Zonausumodificacion { get; set; } 
        public DateTime? Zonafecmodificacion { get; set; } 
        public string Emprenomb { get; set; }
    }
}
