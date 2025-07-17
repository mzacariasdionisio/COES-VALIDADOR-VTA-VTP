using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MMM_VERSION
    /// </summary>
    public class MmmVersionDTO : EntityBase
    {
        public int Vermmcodi { get; set; }
        public DateTime Vermmfechaperiodo { get; set; }
        public string Vermmusucreacion { get; set; }
        public string Vermmmotivo { get; set; }
        public string Vermmmsjgeneracion { get; set; }
        public decimal Vermmporcentaje { get; set; }
        public int Vermmnumero { get; set; }

        public int Vermmestado { get; set; }
        public DateTime Vermmfeccreacion { get; set; }
        public string Vermmusumodificacion { get; set; }
        public DateTime? Vermmfecmodificacion { get; set; }
        public int Vermmmotivoportal { get; set; }
        public DateTime? Vermmfechageneracion { get; set; }
        public DateTime? Vermmfechaaprobacion { get; set; }

        public string VermmestadoDesc { get; set; }
        public string vermmPortalDesc { get; set; }

        public string VermmfeccreacionDesc { get; set; }
        public string VermmfechaaprobacionDesc { get; set; }

    }
}
