using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_BANDANCP
    /// </summary>
    public class CoBandancpDTO : EntityBase
    {
        public int Bandcodi { get; set; } 
        public decimal? Bandmin { get; set; } 
        public decimal? Bandmax { get; set; } 
        public int? Grupocodi { get; set; } 
        public DateTime? Bandfecha { get; set; } 
        public string Bandusumodificacion { get; set; } 
        public DateTime? Bandfecmodificacion { get; set; }

        public string Gruponomb { get; set; }
    }
}
