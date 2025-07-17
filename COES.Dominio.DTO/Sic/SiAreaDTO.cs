using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_AREA
    /// </summary>
    public class SiAreaDTO : EntityBase
    {
        public int? Emprcodi { get; set; } 
        public int Areacodi { get; set; } 
        public string Areanomb { get; set; } 
        public string Areaabrev { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
