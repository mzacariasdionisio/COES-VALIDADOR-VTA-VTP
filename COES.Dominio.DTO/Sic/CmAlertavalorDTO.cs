using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_ALERTAVALOR
    /// </summary>
    public class CmAlertavalorDTO : EntityBase
    {
        public int Alevalcodi { get; set; } 
        public string Alevalindicador { get; set; } 
        public decimal Alevalmax { get; set; }
        public decimal Alevalmaxsinconge { get; set; }
        public decimal Alevalmaxconconge { get; set; }
        public decimal Alevalcisinconge { get; set; }
        public decimal Alevalciconconge { get; set; }
        public DateTime? Alevalfechaproceso { get; set; }
    }
}
