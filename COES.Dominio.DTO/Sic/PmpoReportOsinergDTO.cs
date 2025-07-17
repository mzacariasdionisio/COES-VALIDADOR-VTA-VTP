using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMPO_REPORT_OSINERG
    /// </summary>
    public partial class PmpoReportOsinergDTO : EntityBase
    {
        public int Repcodi { get; set; }
        public string Repdescripcion { get; set; }
        public DateTime Repfecha { get; set; }
        public string Repmeselaboracion { get; set; }
        public string Repusucreacion { get; set; }
        public DateTime? Repfeccreacion { get; set; }
        public string Repusumodificacion { get; set; }
        public DateTime? Repfecmodificacion { get; set; }
        public string Repestado { get; set; }
    }

    public partial class PmpoReportOsinergDTO
    {
        public string RepfechaDesc { get; set; }
        public string RepfeccreacionDesc { get; set; }

    }
}
