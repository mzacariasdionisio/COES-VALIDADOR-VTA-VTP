using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_COSTOPORTDETALLE
    /// </summary>
    public class VcrCostoportdetalleDTO : EntityBase
    {
        public int Vcrcodcodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; } 
        public DateTime Vcrcodfecha { get; set; }
        public int Equicodi { get; set; }
        public int Vcrcodinterv { get; set; }
        public decimal Vcrcodpdo { get; set; }
        public decimal Vcrcodcmgcp { get; set; }
        public decimal Vcrcodcv { get; set; } 
        public decimal Vcrcodcostoportun { get; set; } 
        public string Vcrcodusucreacion { get; set; } 
        public DateTime Vcrcodfeccreacion { get; set; } 
        
    }
}
