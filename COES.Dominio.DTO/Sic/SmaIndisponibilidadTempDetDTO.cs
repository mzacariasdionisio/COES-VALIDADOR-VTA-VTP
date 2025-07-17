using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_INDISPONIBILIDAD_TEMP_DET
    /// </summary>
    public partial class SmaIndisponibilidadTempDetDTO : EntityBase
    {
        public int Intdetcodi { get; set; } 
        public int? Intcabcodi { get; set; } 
        public int? Urscodi { get; set; } 
        public string Intdetindexiste { get; set; } 
        public string Intdettipo { get; set; } 
        public decimal? Intdetbanda { get; set; } 
        public string Intdetmotivo { get; set; } 
    }

    public partial class SmaIndisponibilidadTempDetDTO
    {
        public string Ursnomb { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Centralnomb { get; set; }
        public decimal? BandaUrsCalificada { get; set; }
        public string IntdetindexisteDesc { get; set; }
        public string IntdettipoDesc { get; set; }


    }
}
