using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_FACTORPERDIDA
    /// </summary>
    public class CmFactorperdidaDTO : EntityBase
    {
        public int Cmfpmcodi { get; set; } 
        public int? Cmpercodi { get; set; } 
        public DateTime? Cmfpmfecha { get; set; } 
        public string Cmfpmestado { get; set; } 
        public string Cmfpmusucreacion { get; set; } 
        public DateTime? Cmfpmfeccreacion { get; set; } 
        public string Cmfpmusumodificacion { get; set; } 
        public DateTime? Cmfpmfecmodificacion { get; set; } 
        public string FechaModificacion { get; set; }
        public string FechaDatos { get; set; }
    }
}
