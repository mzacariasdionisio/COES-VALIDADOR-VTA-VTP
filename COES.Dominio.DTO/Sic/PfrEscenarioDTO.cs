using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_ESCENARIO
    /// </summary>
    public partial class PfrEscenarioDTO : EntityBase
    {
        public int Pfresccodi { get; set; } 
        public int? Pfrrptcodi { get; set; } 
        public string Pfrescdescripcion { get; set; } 
        public DateTime Pfrescfecini { get; set; } 
        public DateTime Pfrescfecfin { get; set; } 
        public decimal? Pfrescfrf { get; set; } 
        public decimal? Pfrescfrfr { get; set; } 
        public decimal? Pfrescpfct { get; set; } 
    }

    public partial class PfrEscenarioDTO : EntityBase
    {
        public string FechaDesc { get; set; }
        public int Numero { get; set; }
        public List<PfrReporteTotalDTO> ListaPfrReporteTotal { get; set; }
    }
}
