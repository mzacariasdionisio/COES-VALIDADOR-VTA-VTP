using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_REPORTE
    /// </summary>
    public class PfrReporteDTO : EntityBase
    {
        public int Pfrrptcodi { get; set; } 
        public int? Pfrcuacodi { get; set; } 
        public int? Pfrreccodi { get; set; } 
        public int? Pfrrptesfinal { get; set; } 
        public string Pfrrptusucreacion { get; set; } 
        public DateTime? Pfrrptfeccreacion { get; set; } 
        public decimal? Pfrrptcr { get; set; } 
        public decimal? Pfrrptca { get; set; } 
        public decimal? Pfrrptmr { get; set; } 
        public DateTime? Pfrrptfecmd { get; set; } 
        public decimal? Pfrrptmd { get; set; }
        public int Pfrrptrevisionvtp { get; set; }
        public List<PfrEscenarioDTO> ListaPfrEscenario { get; set; }
        public List<PfrReporteTotalDTO> ListaPfrReporteTotal { get; set; }
    }
}
