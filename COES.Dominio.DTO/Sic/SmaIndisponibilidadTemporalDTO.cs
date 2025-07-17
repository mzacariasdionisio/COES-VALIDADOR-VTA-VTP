using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_INDISPONIBILIDAD_TEMPORAL
    /// </summary>
    public partial class SmaIndisponibilidadTemporalDTO : EntityBase
    {
        public int Smaintcodi { get; set; } 
        public int? Urscodi { get; set; } 
        public DateTime? Smaintfecha { get; set; } 
        public string Smaintindexiste { get; set; } 
        public string Smainttipo { get; set; } 
        public decimal? Smaintbanda { get; set; } 
        public string Smaintmotivo { get; set; } 
        public string Smaintusucreacion { get; set; } 
        public DateTime? Smaintfeccreacion { get; set; } 
        public string Smaintusumodificacion { get; set; } 
        public DateTime? Smaintfecmodificacion { get; set; } 
    }

    public partial class SmaIndisponibilidadTemporalDTO 
    {
        public string Ursnomb { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Centralnomb { get; set; }
        public decimal? BandaUrsCalificada { get; set; }
        public string SmaintindexisteDesc { get; set; }
        public string SmainttipoDesc { get; set; }
        public string SmaintfeccreacionDesc { get; set; }
        public string SmaintfecmodificacionDesc { get; set; }
        

    }
}
