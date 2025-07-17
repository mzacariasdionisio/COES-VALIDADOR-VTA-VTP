using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla TR_MUESTRARIS_SP7
    /// </summary>
    public class TrMuestrarisSp7DTO : EntityBase
    {
        public int Canalcodi { get; set; } 
        public DateTime Canalfecha { get; set; } 
        public int? Canalcalidad { get; set; } 
        public DateTime Canalfhora { get; set; } 
        public DateTime Canalfhora2 { get; set; } 
        public string Canalnomb { get; set; } 
        public string Canaliccp { get; set; } 
        public int? Emprcodi { get; set; } 
        public decimal? Canalvalor { get; set; } 
        public string Estado { get; set; } 
        public string Muerisusucreacion { get; set; } 
        public DateTime? Muerisfeccreacion { get; set; } 
        public string Muerisusumodificacion { get; set; } 
        public DateTime? Muerisfecmodificacion { get; set; }
        public int Delta { get; set; }
        public string canalnomb { get; set; }
        public string fecharep { get; set; }
        public string CALIDAD2 { get; set; }
        public double CANALVALOR { get; set; }
        public string estado { get; set; }
        public string CanalIccp { get; set; }
        public string HoraEmpresa { get; set; }
        public string HoraCoes { get; set; } 
    }
}
