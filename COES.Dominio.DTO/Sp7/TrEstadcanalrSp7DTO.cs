using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sp7
{
    /// <summary>
    /// Clase que mapea la tabla TR_ESTADCANALR_SP7
    /// </summary>
    public class TrEstadcanalrSp7DTO : EntityBase
    {
        public int Estcnlcodi { get; set; } 
        public int? Vercodi { get; set; } 
        public int Canalcodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public int? Zonacodi { get; set; } 
        public DateTime Estcnlfecha { get; set; } 
        public decimal? Estcnltvalido { get; set; } 
        public decimal? Estcnltcong { get; set; } 
        public decimal? Estcnltindet { get; set; } 
        public decimal? Estcnltnnval { get; set; } 
        public int? Estcnlultcalidad { get; set; }
        public DateTime? Estcnlultcambio { get; set; }
        public DateTime? Estcnlultcambioe { get; set; } 
        public decimal? Estcnlultvalor { get; set; } 
        public decimal? Estcnltretraso { get; set; } 
        public int? Estcnlnumregistros { get; set; } 
        public int? Estcnlverantcodi { get; set; } 
        public int? Estcnlverdiaantcodi { get; set; } 
        public string Estcnlingreso { get; set; }         
        public string Estcnlusucreacion { get; set; } 
        public DateTime? Estcnlfeccreacion { get; set; } 
        public string Estcnlusumodificacion { get; set; } 
        public DateTime? Estcnlfecmodificacion { get; set; }
        public DateTime? Verfechaini { get; set; }

        public string Canaliccp { get; set; }
        public string Canalnomb { get; set; } 
        public string Canalunidad { get; set; }
        public string Emprenomb { get; set; }
        public string Zonanomb { get; set; } 
    }
}
