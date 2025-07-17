using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_AGRUPACIONCONCEPTO
    /// </summary>
    public partial class PrAgrupacionConceptoDTO : EntityBase
    {
        public int Agrconcodi { get; set; }
        public int Agrupcodi { get; set; }
        public int? Concepcodi { get; set; }
        public int? Propcodi { get; set; }
        public int Conceporigen { get; set; }
        
        public DateTime? Agrconfecha { get; set; }
        public int? Agrconactivo { get; set; }
        public DateTime? Agrconfeccreacion { get; set; }
        public string Agrconusucreacion { get; set; }
        public string Agrconusumodificacion { get; set; }
        public DateTime? Agrconfecmodificacion { get; set; }
    }

    public partial class PrAgrupacionConceptoDTO
    {
        public string Concepabrev { get; set; }
        public string Concepdesc { get; set; }
        public string Concepnombficha { get; set; }
        public string Concepunid { get; set; }
        public string Conceptipo { get; set; }
        public string Catenomb { get; set; }
        public string Cateabrev { get; set; }

        public int? Catecodi { get; set; }
        public int? Famcodi { get; set; }

        public int Orden { get; set; }
    }
}
