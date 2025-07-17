using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_GRUPOXCNFBAR
    /// </summary>
    public class PrGrupoxcnfbarDTO : EntityBase
    {
        public int Grcnfbcodi { get; set; }
        public int Cnfbarcodi { get; set; }
        public int Grupocodi { get; set; }
        public int Grcnfbestado { get; set; }
        public string Grcnfbusucreacion { get; set; }
        public DateTime? Grcnfbfeccreacion { get; set; }
        public string Grcnfbusumodificacion { get; set; }
        public DateTime? Grcnfbfecmodificacion { get; set; }

        public string Cnfbarnombre { get; set; }
    }
}
