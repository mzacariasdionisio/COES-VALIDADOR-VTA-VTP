using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_FPMDETALLE
    /// </summary>
    public class CmFpmdetalleDTO : EntityBase
    {
        public int Cmfpmdcodi { get; set; } 
        public int? Cmfpmcodi { get; set; } 
        public int? Barrcodi { get; set; } 
        public decimal? Cmfpmdbase { get; set; } 
        public decimal? Cmfpmdmedia { get; set; } 
        public decimal? Cmfpmdpunta { get; set; } 
        public DateTime FechaDatos { get; set; }
    }
}
