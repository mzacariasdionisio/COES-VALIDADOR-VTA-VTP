using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_ESCENARIO
    /// </summary>
    public partial class PfEscenarioDTO : EntityBase
    {
        public int Pfescecodi { get; set; }
        public int Pfrptcodi { get; set; }
        public DateTime Pfescefecini { get; set; } 
        public DateTime Pfescefecfin { get; set; } 
        public string Pfescedescripcion { get; set; }
    }

    public partial class PfEscenarioDTO : EntityBase
    {
        public string FechaDesc { get; set; }
        public int Numero { get; set; }
        public int Pfescetipo { get; set; }
        public List<PfReporteTotalDTO> ListaPfReporteTotal { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPe { get; set; }
    }
}
