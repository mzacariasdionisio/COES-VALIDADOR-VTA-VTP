using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AF_ERACMF_ZONA
    /// </summary>
    public class AfEracmfZonaDTO : EntityBase
    {
        public DateTime? Aferacfeccreacion { get; set; }
        public string Aferacusucreacion { get; set; }
        public decimal? Aferacdertemp { get; set; }
        public decimal? Aferacderpend { get; set; }
        public decimal? Aferacderarrq { get; set; }
        public decimal? Aferacumbraltemp { get; set; }
        public decimal? Aferacumbralarrq { get; set; }
        public decimal? Aferacporcrechazo { get; set; }
        public int? Aferacnumetapa { get; set; }
        public string Aferaczona { get; set; }
        public DateTime Aferacfechaperiodo { get; set; }
        public int Aferaccodi { get; set; }
    }
}
