using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_VERSIONIEOD
    /// </summary>
    public class CpuntosDTO : EntityBase
    {
        public int PointNumber { get; set; }
        public string PointName { get; set; }
        public string PointText { get; set; }
        public string PointType { get; set; }
        public string PointTypeName { get; set; }
        public string Active { get; set; }
        public int AorId { get; set; }
        public int CollectionRate { get; set; }
    }
}
