using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sp7
{
    /// <summary>
    /// Clase que mapea la tabla TR_REPORTEVERSION_SP7
    /// </summary>
    public class TrReporteversionSp7DTO : EntityBase
    {
        public int Revcodi { get; set; } 
        public int? Vercodi { get; set; } 
        public int? Emprcodi { get; set; }
        public DateTime Revfecha { get; set; } 
        public decimal? Revsumaciccpsmed { get; set; } 
        public decimal? Revsumaciccpsest { get; set; }
        public decimal? Revsumaciccpsestnoalm { get; set; } 
        public decimal? Revsumaciccpsalm { get; set; } 
        public decimal? Revsumaciccpsmedc { get; set; } 
        public decimal? Revsumaciccpsestc { get; set; }
        public decimal? Revsumaciccpsestnoalmc { get; set; }
        public decimal? Revsumaciccpsalmc { get; set; } 
        public int? Revnummed { get; set; } 
        public int? Revnumest { get; set; }
        public int? Revnumestnoalm { get; set; } 
        public int? Revnumalm { get; set; } 
        public int? Revnummedc { get; set; } 
        public int? Revnumestc { get; set; }
        public int? Revnumestnoalmc { get; set; } 
        public int? Revnumalmc { get; set; } 
        public int? Revnummedcsindef { get; set; } 
        public int? Revnumestcsindef { get; set; } 
        public int? Revnumalmcsindef { get; set; } 
        public int? Revtfse { get; set; } 
        public int? Revtfss { get; set; } 
        public int? Revttotal { get; set; } 
        public decimal? Revfactdisp { get; set; } 
        public decimal? Revciccpe { get; set; } 
        public decimal? Revciccpemedest { get; set; } 
        public decimal? Revciccpecrit { get; set; } 
        public int? Revttng { get; set; } 
        public string Revusucreacion { get; set; } 
        public DateTime? Revfeccreacion { get; set; } 
        public string Revusumodificacion { get; set; } 
        public DateTime? Revfecmodificacion { get; set; }

        public DateTime? Verfechaini { get; set; }
        public DateTime? Verfechafin { get; set; }
        public string Emprenomb { get; set; }
        public int CanGeneral { get; set; }
        public int CanMedEst { get; set; }
        public int CanCritico { get; set; }
        public int Vernumero { get; set; }
        
    }
}
