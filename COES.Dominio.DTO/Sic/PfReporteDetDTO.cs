using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_REPORTE_DET
    /// </summary>
    public partial class PfReporteDetDTO : EntityBase
    {
        public int Pfdetcodi { get; set; } 
        public int Pftotcodi { get; set; } 
        public int Pfdettipo { get; set; } 
        public DateTime? Pfdetfechaini { get; set; } 
        public DateTime? Pfdetfechafin { get; set; } 
        public decimal? Pfdetenergia { get; set; } 
        public int Pfdetnumdiapoc { get; set; } 
    }

    public partial class PfReporteDetDTO : EntityBase 
    {
        public int Equipadre { get; set; }
        public int TotalDias { get; set; }
        public int TotalHP { get; set; }
        public string FechaDesc { get; set; }
        public int TotalMes { get; set; }
    }
}
