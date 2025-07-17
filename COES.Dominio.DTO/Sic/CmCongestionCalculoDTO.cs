using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_CONGESTION_CALCULO
    /// </summary>
    public class CmCongestionCalculoDTO : EntityBase
    {
        public int Cmcongcodi { get; set; }
        public int? Configcodi { get; set; }
        public int? Grulincodi { get; set; }
        public int? Regsegcodi { get; set; }
        public DateTime? Cmconfecha { get; set; }
        public int? Cmcongperiodo { get; set; }
        public int? Cmgncorrelativo { get; set; }
        public decimal? Cmconglimite { get; set; }
        public decimal? Cmcongenvio { get; set; }
        public decimal? Cmcongrecepcion { get; set; }
        public decimal? Cmcongcongestion { get; set; }
        public decimal? Cmconggenlimite { get; set; }
        public decimal? Cmconggeneracion { get; set; }
        public string Cmcongusucreacion { get; set; }
        public DateTime? Cmcongfeccreacion { get; set; }
        public string Cmcongusumodificacion { get; set; }
        public DateTime? Cmcongfecmodificacion { get; set; }

        public string Famnomb { get; set; }
        public string Equinomb { get; set; }
        public int Tipo { get; set; }
        public DateTime Congesfecinicio { get; set; }
        public DateTime Congesfecfin { get; set; }
    }
}
