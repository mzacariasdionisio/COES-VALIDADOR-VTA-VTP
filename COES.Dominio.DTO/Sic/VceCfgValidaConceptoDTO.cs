using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla VCE_CFG_VALIDA_CONCEPTO
    /// </summary>
    public class VceCfgValidaConceptoDTO : EntityBase
    {
        public int Crcvalcodi { get; set; }
        public string Crcvaldescripcion { get; set; }
        public int Concepcodi { get; set; }
        public string Crcvalcondicion { get; set; }
        public decimal Crcvalvalorref { get; set; }
        public string Crgexcusucreacion { get; set; }
        public DateTime Crgexcfeccreacion { get; set; }
        public string Crgexcusumodificacion { get; set; }
        public DateTime? Crgexcfecmodificacion { get; set; }

    }
}
