using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_DESPACHO_RESUMEN
    /// </summary>
    public partial class MeDespachoResumenDTO : EntityBase
    {
        public int Dregencodi { get; set; }
        public DateTime Dregenfecha { get; set; }
        public int Dregentipo { get; set; }

        public decimal? Dregentotalsein { get; set; }
        public decimal? Dregentotalexp { get; set; }
        public decimal? Dregentotalimp { get; set; }
        public decimal? Dregentotalnorte { get; set; }
        public decimal? Dregentotalcentro { get; set; }
        public decimal? Dregentotalsur { get; set; }

        public DateTime Dregenmdhora { get; set; }
        public decimal? Dregenmdsein { get; set; }
        public decimal? Dregenmdexp { get; set; }
        public decimal? Dregenmdimp { get; set; }
        public decimal? Dregenmdhidro { get; set; }
        public decimal? Dregenmdtermo { get; set; }
        public decimal? Dregenmdeolico { get; set; }
        public decimal? Dregenmdsolar { get; set; }

        public DateTime Dregenhphora { get; set; }
        public decimal? Dregenhpsein { get; set; }
        public decimal? Dregenhpexp { get; set; }
        public decimal? Dregenhpimp { get; set; }

        public DateTime Dregenfhphora { get; set; }
        public decimal? Dregenfhpsein { get; set; }
        public decimal? Dregenfhpexp { get; set; }
        public decimal? Dregenfhpimp { get; set; }

        public DateTime Dregenmdnoiihora { get; set; }
        public decimal? Dregenmdnoiisein { get; set; }
    }

    public partial class MeDespachoResumenDTO
    {
        public int HMaxD48 { get; set; }
    }
}
