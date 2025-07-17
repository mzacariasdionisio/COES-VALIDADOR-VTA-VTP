using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ABI_MEDIDORES_RESUMEN
    /// </summary>
    public partial class AbiMedidoresResumenDTO : EntityBase
    {
        public int Mregencodi { get; set; }
        public DateTime Mregenfecha { get; set; }

        public decimal? Mregentotalsein { get; set; }
        public decimal? Mregentotalexp { get; set; }
        public decimal? Mregentotalimp { get; set; }
        public decimal? Mregentotalnorte { get; set; }
        public decimal? Mregentotalcentro { get; set; }
        public decimal? Mregentotalsur { get; set; }

        public DateTime Mregenmdhora { get; set; }
        public decimal? Mregenmdsein { get; set; }
        public decimal? Mregenmdexp { get; set; }
        public decimal? Mregenmdimp { get; set; }
        public decimal? Mregenmdhidro { get; set; }
        public decimal? Mregenmdtermo { get; set; }
        public decimal? Mregenmdeolico { get; set; }
        public decimal? Mregenmdsolar { get; set; }

        public DateTime Mregenhphora { get; set; }
        public decimal? Mregenhpsein { get; set; }
        public decimal? Mregenhpexp { get; set; }
        public decimal? Mregenhpimp { get; set; }

        public DateTime Mregenfhphora { get; set; }
        public decimal? Mregenfhpsein { get; set; }
        public decimal? Mregenfhpexp { get; set; }
        public decimal? Mregenfhpimp { get; set; }

        public DateTime? Mregenmdnoiihora { get; set; }
        public decimal? Mregenmdnoiisein { get; set; }

    }

    public partial class AbiMedidoresResumenDTO
    {
    }
}
