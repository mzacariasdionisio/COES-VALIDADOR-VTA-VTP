using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_INFORMEFALLA_N2
    /// </summary>
    public class EveInformefallaN2DTO : EntityBase
    {
        public int Eveninfn2codi { get; set; }
        public int? Evencodi { get; set; }
        public int? Evenanio { get; set; }
        public int? Evenn2corr { get; set; }
        public DateTime? Eveninfpin2fechemis { get; set; }
        public string Eveninfpin2emitido { get; set; }
        public string Eveninfpin2elab { get; set; }
        public string Eveninffn2emitido { get; set; }
        public string Eveninffn2elab { get; set; }
        public string Eveninfn2lastuser { get; set; }
        public DateTime? Eveninfn2lastdate { get; set; }
        public DateTime? Eveninffn2fechemis { get; set; }
        public string EvenipiEN2emitido { get; set; }
        public string EvenipiEN2elab { get; set; }
        public DateTime? EvenipiEN2fechem { get; set; }
        public string EvenifEN2emitido { get; set; }
        public string EvenifEN2elab { get; set; }
        public DateTime? EvenifEN2fechem { get; set; }

        public string Emprnomb { get; set; }
        public string Tareaabrev { get; set; }
        public string Areanomb { get; set; }
        public string Famabrev { get; set; }
        public string Equiabrev { get; set; }
        public decimal? Evenmwindisp { get; set; }
        public DateTime Evenini { get; set; }
        public string Obsprelimini { get; set; }
        public string Obsfinal { get; set; }
        public int? Eveninfplazodiasipi { get; set; }
        public int? Eveninfplazodiasif { get; set; }
        public int? Eveninfplazohoraipi { get; set; }
        public int? Eveninfplazohoraif { get; set; }
        public int? Eveninfplazominipi { get; set; }
        public int? Eveninfplazominif { get; set; }


    }
}
