using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_REPORTE
    /// </summary>
    public class SpoReporteDTO : EntityBase
    {
        public int Repcodi { get; set; } 
        public int? Repdiaapertura { get; set; } 
        public int? Repdiaclausura { get; set; } 
    }
}
