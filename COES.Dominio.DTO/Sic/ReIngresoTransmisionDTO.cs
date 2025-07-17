using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_INGRESO_TRANSMISION
    /// </summary>
    public class ReIngresoTransmisionDTO : EntityBase
    {
        public int Reingcodi { get; set; } 
        public int? Repercodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public decimal? Reingvalor { get; set; } 
        public string Reingmoneda { get; set; }
        public string Reingusucreacion { get; set; } 
        public DateTime? Reingfeccreacion { get; set; } 
        public string Reingusumodificacion { get; set; } 
        public DateTime? Reingfecmodificacion { get; set; } 
        public string Emprnomb { get; set; }
        public string Reingfuente { get; set; }
        public string Reingsustento { get; set; }
        public bool TieneArchivo { get; set; }
    }
}
