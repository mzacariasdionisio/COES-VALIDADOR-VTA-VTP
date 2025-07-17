using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_DETALLEETAPA
    /// </summary>
    public class CpDetalleetapaDTO : EntityBase
    {
        public int? Etpini { get; set; } 
        public decimal? Etpdelta { get; set; } 
        public int? Etpfin { get; set; } 
        public int? Etpbloque { get; set; } 
        public int Topcodi { get; set; }
        public decimal? Etpacumulado { get; set; }
        public DateTime EtapaFechaInicio { get; set; }
        public string EtapaFechaInicioStr { get; set; }
    }
}
