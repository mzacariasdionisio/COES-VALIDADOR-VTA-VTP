using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_POTENCIA_ADICIONAL
    /// </summary>
    public partial class PfPotenciaAdicionalDTO : EntityBase
    {
        public int Pfpadicodi { get; set; }
        public int Famcodi { get; set; }
        public int Equipadre { get; set; }
        public int Emprcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Equicodi { get; set; }
        public decimal? Pfpadipe { get; set; }
        public decimal? Pfpadifi { get; set; }
        public decimal? Pfpadipf { get; set; }
        public int? Pfpericodi { get; set; }
        public int? Pfverscodi { get; set; }
        public int Pfpadiincremental { get; set; }
        public string Pfpadiunidadnomb { get; set; }
    }

    public partial class PfPotenciaAdicionalDTO
    {
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Famnomb { get; set; }
        public string Formula { get; set; }
    }
}
