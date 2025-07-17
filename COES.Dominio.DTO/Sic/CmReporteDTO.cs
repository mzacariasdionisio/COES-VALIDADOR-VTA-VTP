using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_REPORTE
    /// </summary>
    public class CmReporteDTO : EntityBase
    {
        public int Cmrepcodi { get; set; } 
        public int? Cmpercodi { get; set; } 
        public int? Cmurcodi { get; set; } 
        public int? Cmrepversion { get; set; } 
        public DateTime? Cmrepfecha { get; set; } 
        public string Cmrepestado { get; set; } 
        public string Cmrepusucreacion { get; set; } 
        public DateTime? Cmrepfeccreacion { get; set; } 
        public string Cmrepusumodificacion { get; set; } 
        public DateTime? Cmrepfecmodificacion { get; set; } 
        public string FechaReporte { get; set; }
        public string FechaModificacion { get; set; }
    }
}
