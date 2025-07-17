using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_EVENTO_PERIODO
    /// </summary>
    public class ReEventoPeriodoDTO : EntityBase
    {
        public int Reevecodi { get; set; } 
        public int? Repercodi { get; set; } 
        public string Reevedescripcion { get; set; } 
        public DateTime? Reevefecha { get; set; } 
        public int? Reeveempr1 { get; set; } 
        public int? Reeveempr2 { get; set; } 
        public int? Reeveempr3 { get; set; } 
        public int? Reeveempr4 { get; set; } 
        public int? Reeveempr5 { get; set; } 
        public decimal? Reeveporc1 { get; set; } 
        public decimal? Reeveporc2 { get; set; } 
        public decimal? Reeveporc3 { get; set; } 
        public decimal? Reeveporc4 { get; set; } 
        public decimal? Reeveporc5 { get; set; } 
        public string Reevecomentario { get; set; } 
        public string Reeveestado { get; set; } 
        public string Reeveusucreacion { get; set; } 
        public DateTime? Reevefeccreacion { get; set; } 
        public string Reeveusumodificacion { get; set; } 
        public DateTime? Reevefecmodificacion { get; set; } 
        public string Responsablenomb1 { get; set; }
        public string Responsablenomb2 { get; set; }
        public string Responsablenomb3 { get; set; }
        public string Responsablenomb4 { get; set; }
        public string Responsablenomb5 { get; set; }
        public string Porcentaje1 { get; set; }
        public string Porcentaje2 { get; set; }
        public string Porcentaje3 { get; set; }
        public string Porcentaje4 { get; set; }
        public string Porcentaje5 { get; set; }
        public string FechaEvento { get; set; }
    }
}
