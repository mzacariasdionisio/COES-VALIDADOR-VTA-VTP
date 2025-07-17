using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_RESULTADOS_GAMS
    /// </summary>
    public class PfrResultadosGamsDTO : EntityBase
    {
        public int Pfrrgcodi { get; set; }
        public int? Pfresccodi { get; set; }

        public int? Pfrrgecodi { get; set; }
        public int? Pfreqpcodi { get; set; }
        public int? Pfrcgtcodi { get; set; }

        public int? Pfrrgtipo { get; set; }
        public decimal? Pfrrgresultado { get; set; }
    }
}
