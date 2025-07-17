using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_PERIODO
    /// </summary>
    public partial class PfPeriodoDTO : EntityBase
    {
        public int Pfpericodi { get; set; } 
        public string Pfperinombre { get; set; } 
        public int Pfperianio { get; set; } 
        public int Pfperimes { get; set; } 
        public int Pfperianiomes { get; set; } 
        public string Pfperiusucreacion { get; set; } 
        public DateTime? Pfperifeccreacion { get; set; } 
        public string Pfperiusumodificacion { get; set; } 
        public DateTime? Pfperifecmodificacion { get; set; } 
    }

    public partial class PfPeriodoDTO : EntityBase 
    {
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
