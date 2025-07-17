using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_PARAM_ESQUEMA
    /// </summary>
    public class RcaParamEsquemaDTO : EntityBase
    {
        public int Rcparecodi { get; set; }
        public int Emprcodi { get; set; }
        public int? Equicodi { get; set; }
        public decimal? Rcparehperacmf { get; set; }
        public decimal? Rcparehperacmt { get; set; }
        public decimal? Rcparehfperacmf { get; set; }
        public decimal? Rcparehfperacmt { get; set; }
        public string Rcpareestregistro { get; set; }
        public string Rcpareusucreacion { get; set; }
        public DateTime Rcparefeccreacion { get; set; }
        public string Rcpareusumodificacion { get; set; }
        public DateTime? Rcparefecmodificacion { get; set; }

        public int Rcpareanio { get; set; }

        public decimal? Rcparehperacmf2 { get; set; }
        public decimal? Rcparehfperacmf2 { get; set; }
        public int Rcparenroesquema { get; set; }

        public string Emprrazsocial { get; set; }
        public string Areanomb { get; set; }
        public string Equinomb { get; set; }

        public decimal? Rcparehpdemandaref { get; set; }
        public decimal? Rcparehfpdemandaref { get; set; }
    }
}
