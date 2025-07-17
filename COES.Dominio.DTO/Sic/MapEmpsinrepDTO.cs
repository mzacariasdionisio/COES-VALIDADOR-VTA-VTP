using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MAP_EMPSINREP
    /// </summary>
    public class MapEmpsinrepDTO : EntityBase
    {
        public int Empsrcodi { get; set; } 
        public DateTime? Empsrperiodo { get; set; } 
        public DateTime? Empsrfecha { get; set; } 
        public string Empsrusucreacion { get; set; } 
        public DateTime? Empsrfeccreacion { get; set; } 
        public string Empsrusumodificacion { get; set; } 
        public DateTime? Empsrfecmodificacion { get; set; } 
        public int? Emprcodi { get; set; }
        public int? Mediccodi { get; set; } 
    }
}
