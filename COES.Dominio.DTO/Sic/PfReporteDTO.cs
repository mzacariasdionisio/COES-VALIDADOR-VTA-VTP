using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PF_REPORTE
    /// </summary>
    public class PfReporteDTO : EntityBase
    {
        public int Pfrptcodi { get; set; } 
        public string Pfrptusucreacion { get; set; } 
        public DateTime Pfrptfeccreacion { get; set; } 
        public int? Pfrptesfinal { get; set; } 
        public int Pfrecacodi { get; set; } 
        public int? Pfcuacodi { get; set; }

        public List<PfEscenarioDTO> ListaPfEscenario { get; set; }
    }
}
