using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_PROGAUDI_INVOLUCRADO
    /// </summary>
    public class AudProgaudiInvolucradoDTO : EntityBase
    {
        public int Progaicodi { get; set; } 
        public int Progacodi { get; set; } 
        public int Tabcdcoditipoinvolucrado { get; set; } 
        public int Percodiinvolucrado { get; set; } 
        public string Progaiactivo { get; set; } 
        public string Progaihistorico { get; set; } 
        public string Progaiusuregistro { get; set; } 
        public DateTime? Progaifecregistro { get; set; } 
        public string Progaiusumodificacion { get; set; } 
        public DateTime? Progaifecmodificacion { get; set; }

        public string Responsable { get; set; }
        public string Peremail { get; set; }
        public int Percodi { get; set; }
        public int Areacodi { get; set; }
    }
}
