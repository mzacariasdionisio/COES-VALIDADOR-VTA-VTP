using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_RELACION_INDISPONIBILIDAD
    /// </summary>
    public class PfrRelacionIndisponibilidadDTO : EntityBase
    {
        public int Pfrrincodi { get; set; } 
        public int? Pfrrptcodi { get; set; } 
        public int? Irptcodi { get; set; }
        public int Pfrrintipo { get; set; }
    }
}
