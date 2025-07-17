using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_HORAOPERACION_EQUIPO
    /// </summary>
    public class EveHoraoperacionEquipoDTO : EntityBase
    {
        public int Hopequcodi { get; set; } 
        public int? Hopcodi { get; set; } 
        public int? Configcodi { get; set; } 
        public int? Grulincodi { get; set; } 
        public int? Regsegcodi { get; set; } 
        public string Hopequusucreacion { get; set; } 
        public DateTime? Hopequfeccreacion { get; set; } 
        public string Hopequusumodificacion { get; set; } 
        public DateTime? Hopequfecmodificacion { get; set; } 
    }
}
