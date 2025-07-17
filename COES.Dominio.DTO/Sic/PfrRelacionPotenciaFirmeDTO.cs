using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_RELACION_POTENCIA_FIRME
    /// </summary>
    public class PfrRelacionPotenciaFirmeDTO : EntityBase
    {
        public int Pfrrpfcodi { get; set; } 
        public int? Pfrrptcodi { get; set; } 
        public int? Pfrptcodi { get; set; } 
    }
}
