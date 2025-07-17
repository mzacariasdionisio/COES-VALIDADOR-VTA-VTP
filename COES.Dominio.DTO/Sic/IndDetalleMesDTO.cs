using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_DETALLE_MES
    /// </summary>
    public class IndDetalleMesDTO : EntityBase
    {
        public int Detmescodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public int? Equicodi { get; set; } 
        public decimal? Detmeship { get; set; } 
        public decimal? Detmeshif { get; set; } 
        public int? Detmesanno { get; set; } 
        public int? Detmesmes { get; set; } 
        public string Detmesusucreacion { get; set; } 
        public DateTime? Detmesfeccreacion { get; set; } 
        public string Detmesusumodificacion { get; set; } 
        public DateTime? Detmesfecmodificacion { get; set; } 
    }
}
