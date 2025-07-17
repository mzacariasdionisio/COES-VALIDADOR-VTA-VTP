using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_RSFDETALLE
    /// </summary>
    public class EveRsfdetalleDTO : EntityBase
    {
        public int? Grupocodi { get; set; } 
        public int? Rsfhorcodi { get; set; } 
        public int Rsfdetcodi { get; set; } 
        public decimal? Rsfdetvalman { get; set; } 
        public decimal? Rsfdetvalaut { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; }
        public string Ursnomb { get; set; }
        public string Gruponomb { get; set; }
        public string Emprnomb { get; set; }
        public DateTime? HorInicio { get; set; }
        public DateTime? HorFin { get; set; }
        public string Grupotipo { get; set; }
        public int? Equicodi { get; set; }
        public int Famcodi { get; set; }
        public int Equipadre { get; set; }
        public int vali { get; set; }

        public string Rsfdetindope { get; set; }
        public decimal? Rsfdetsub { get; set; }
        public decimal? Rsfdetbaj { get; set; }

        public decimal? Rsfdetdesp { get; set; }
        public decimal? Rsfdetload { get; set; }
        public decimal? Rsfdetmingen { get; set; }
        public decimal? Rsfdetmaxgen { get; set; }

        public decimal? RSFDETSUB { get; set; }
        public decimal? RSFDETBAJ { get; set; }

        public int Emprcodi { get; set; }
    }
}
