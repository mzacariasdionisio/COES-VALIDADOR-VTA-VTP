using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EN_ENSAYO
    /// </summary>
    public class EnEnsayoDTO : EntityBase
    {
        public int Ensayocodi { get; set; }
        public DateTime Ensayofecha { get; set; }
        public string Usercodi { get; set; }
        public int? Emprcodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Estadocodi { get; set; }
        public DateTime Ensayofechaevento { get; set; }
        public DateTime Lastdate { get; set; }
        public string Lastuser { get; set; }
        public string Ensayomodoper { get; set; }

        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Estadonombre { get; set; }
        public string Estadocolor { get; set; }
        public int NroRevisar { get; set; }
        public int NroObservar { get; set; }
        public int NroAprobados { get; set; }
        public string Unidadnomb { get; set; }
    }
}
