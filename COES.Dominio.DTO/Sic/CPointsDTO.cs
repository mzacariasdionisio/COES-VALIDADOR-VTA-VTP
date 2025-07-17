using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla C_POINTS
    /// </summary>
    public class CPointsDTO : EntityBase
    {
        public int? PointNumber { get; set; } 
        public string PointName { get; set; } 
        public string PointType { get; set; }  
        public string Active { get; set; } 
        public int? AorId { get; set; } 
    }
}
