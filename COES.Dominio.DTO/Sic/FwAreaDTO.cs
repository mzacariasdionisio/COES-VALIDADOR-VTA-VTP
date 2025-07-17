using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FW_AREA
    /// </summary>
    public class FwAreaDTO : EntityBase
    {
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; } 
        public string Areaabrev { get; set; } 
        public string Areaname { get; set; } 
        public int Areacode { get; set; } 
        public int? Compcode { get; set; } 
        public string Flagreclamos { get; set; } 
        public int? Areapadre { get; set; } 
        public int? Areaorder { get; set; } 
    }
}
