using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RTU_CONFIGURACION
    /// </summary>
    public class RtuConfiguracionDTO : EntityBase
    {
        public int Rtuconcodi { get; set; }
        public int? Rtuconanio { get; set; }
        public int? Rtuconmes { get; set; }
        public string Rtuconusucreacion { get; set; }
        public DateTime? Rtuconfeccreacion { get; set; }
        public DateTime? Rtuconfecmodificacion { get; set; }
        public string Rtuconusumodificacion { get; set; }

        public string Pernomb { get; set; }
        public int Percodi { get; set; }
        public int Perorden { get; set; }
        public int Grupocodi { get; set; }
        public string Gruptipo { get; set; }
        public int Grupoorden { get; set; }
    }

}
