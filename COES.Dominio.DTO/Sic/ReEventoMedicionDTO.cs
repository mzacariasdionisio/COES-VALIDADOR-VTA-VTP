using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_EVENTO_MEDICION
    /// </summary>
    public class ReEventoMedicionDTO : EntityBase
    {
        public int Reemedcodi { get; set; } 
        public int? Reevprcodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public DateTime? Reemedfechahora { get; set; } 
        public decimal? Reemedtensionrs { get; set; } 
        public decimal? Reemedtensionst { get; set; } 
        public decimal? Reemedtensiontr { get; set; } 
        public decimal? Reemedvarp { get; set; } 
        public decimal? Reemedvala { get; set; } 
        public decimal? Reemedvalap { get; set; } 
        public decimal? Reemedvalep { get; set; } 
        public decimal? Reemedvalaapep { get; set; } 
        public string Reemedusucreacion { get; set; } 
        public DateTime? Reemedfeccreacion { get; set; } 
        public string Reemedusumodificacion { get; set; } 
        public DateTime? Reemedfecmodificacion { get; set; } 
        public string Fecha { get; set; }
    }
}
