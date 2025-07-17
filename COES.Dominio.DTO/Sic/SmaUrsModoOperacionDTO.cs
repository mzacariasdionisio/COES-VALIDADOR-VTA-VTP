using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_URS_MODO_OPERACION
    /// </summary>
    public class SmaUrsModoOperacionDTO : EntityBase
    {
        public int Urscodi { get; set; } 
        public string Ursnomb { get; set; } 
        public string Urstipo { get; set; } 
        public int? Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public string Gruponombncp { get; set; }
        public int Grupocodincp { get; set; }
        public string Grupoabrev { get; set; } 
        public int? Emprcodi { get; set; } 
        public string Emprnomb { get; set; } 
        public string Grupotipo { get; set; }
        public decimal? CapacidadMax { get; set; }
        public string ManttoProgramado { get; set; }
        public string[,] Intervalo { get; set; }
        public string ListIntervalos { get; set; }
        public decimal? PMin { get; set; }
        public decimal? PMax { get; set; }
        public decimal? BandaCalificada { get; set; }
        public string Comentario { get; set; }
        public decimal? BandaAdjudicada { get; set; }
        public decimal? BandaDisponible { get; set; }
        public decimal? PrecMin { get; set; }
        public decimal? PrecMax { get; set; }

        //
        public int Grupopadre { get; set; }
        public string Central { get; set; }
        public int Catecodi { get; set; }
        public string FechaInico { get; set; }
        public string FechaFin { get; set; }
        public string Acta { get; set; }
        public decimal? BandaURS { get; set; }
        public string Estado { get; set; }
        public string UsuarioModif { get; set; }
        public string FechaModif { get; set; }

        public string PMinDesc { get; set; }
        public string PMaxDesc { get; set; }
        public string BandaCalifDesc { get; set; }
        public string MensajeBandaCalificada { get; set; }

        public string ModFechIni { get; set; }
        public string ModFechFin { get; set; }
        public string BandaAdjudDesc { get; set; }
        public string PrecMinDesc { get; set; }
        public string PrecMaxDesc { get; set; }
        public int FlagValidateFecha { get; set; }

        
        public SmaUrsModoOperacionDTO Copy()
        {
            return (SmaUrsModoOperacionDTO)this.MemberwiseClone();
        }
    }
}
