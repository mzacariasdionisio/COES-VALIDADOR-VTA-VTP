using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_FACTORES
    /// </summary>
    public partial class PfFactoresDTO : EntityBase
    {
        public int Pffactcodi { get; set; }
        public int? Pfpericodi { get; set; }
        public int Pfverscodi { get; set; }
        public int Emprcodi { get; set; }
        public int Famcodi { get; set; }
        public int Equipadre { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public int Pffacttipo { get; set; }
        public decimal? Pffactfi { get; set; }
        public decimal? Pffactfp { get; set; }
        public int Pffactincremental { get; set; }
        public string Pffactunidadnomb { get; set; }
    }

    public partial class PfFactoresDTO
    {
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Famnomb { get; set; }
        public string Formula { get; set; }
    }
}
