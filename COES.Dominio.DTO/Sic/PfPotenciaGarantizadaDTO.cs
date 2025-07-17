using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_POTENCIA_GARANTIZADA
    /// </summary>
    [Serializable]
    public partial class PfPotenciaGarantizadaDTO : EntityBase
    {
        public int Pfpgarcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Equipadre { get; set; }
        public int? Emprcodi { get; set; }
        public decimal? Pfpgarpe { get; set; }
        public decimal? Pfpgarpg { get; set; }
        public int? Pfpericodi { get; set; }
        public int? Pfverscodi { get; set; }
        public int Equicodi { get; set; }
        public string Pfpgarunidadnomb { get; set; }
    }

    public partial class PfPotenciaGarantizadaDTO
    {
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Famnomb { get; set; }
    }
}
