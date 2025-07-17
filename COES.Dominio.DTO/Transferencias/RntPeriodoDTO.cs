using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RNT_PERIODO
    /// </summary>
    [XmlRoot("RNT_PERIODO")]
    public class RntPeriodoDTO : EntityBase
    {
        public int PeriodoCodi { get; set; } 
        public string PerdEstado { get; set; } 
        public int? PerdAnio { get; set; } 
        public string PerdNombre { get; set; } 
        public string PerdUsuarioCreacion { get; set; } 
        public DateTime? PerdFechaCreacion { get; set; } 
        public string PerdUsuarioUpdate { get; set; } 
        public DateTime? PerdFechaUpdate { get; set; } 
        public string PerdSemestre { get; set; }
        public string PeriodoDesc { get; set; }
    }
}
