using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_PROGAUDI_HALLAZGOS
    /// </summary>
    public class AudProgaudiHallazgosDTO : EntityBase
    {
        public int Progahcodi { get; set; } 
        public int Progaecodi { get; set; } 
        public int? Archcodianalisiscausa { get; set; } 
        public int? Archcodievidencia { get; set; } 
        public int Tabcdcoditipohallazgo { get; set; } 
        public int? Progaicodiresponsable { get; set; } 
        public string Progahdescripcion { get; set; } 
        public string Progahplanaccion { get; set; } 
        public string Progahaccionmejora { get; set; } 
        public DateTime? Progahaccionmejoraplazo { get; set; } 
        public int? Tabcdestadocodi { get; set; } 
        public string Progahactivo { get; set; } 
        public string Progahhistorico { get; set; } 
        public string Progahusucreacion { get; set; } 
        public DateTime? Progahfeccreacion { get; set; } 
        public string Progahusumodificacion { get; set; } 
        public DateTime? Progahfecmodificacion { get; set; }

        public string Tipohallazgo { get; set; }
        public string Elemcodigo { get; set; }
        public string Elemdescripcion { get; set; }
        public int Progacodi { get; set; }

        public int Audicodi { get; set; }
        public int Tabcdcodiestado { get; set; }
        public string Estadohallazgo { get; set; }

        public int Usercode { get; set; }
    }
}
