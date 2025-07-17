using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_FLUJO_POTENCIA
    /// </summary>
    public class CmFlujoPotenciaDTO : EntityBase
    {
        public int Flupotcodi { get; set; } 
        public int? Cmgncorrelativo { get; set; } 
        public int Equicodi { get; set; }
        public int Famcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal? Flupotvalor { get; set; } 
        public int? Flupotoperativo { get; set; } 
        public DateTime? Flupotfecha { get; set; } 
        public string Flupotusucreacion { get; set; } 
        public DateTime? Flupotfechacreacion { get; set; }
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public decimal? Flupotvalor1 { get; set; }
        public decimal? Flupotvalor2 { get; set; }
        public int? Configcodi { get; set; }
        public string Nodobarra1 { get; set; }
        public string Nodobarra2 { get; set; }
    }
}
