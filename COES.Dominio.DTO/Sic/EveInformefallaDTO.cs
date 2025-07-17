using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_INFORMEFALLA
    /// </summary>
    public class EveInformefallaDTO : EntityBase
    {
        public int Eveninfcodi { get; set; }
        public int? Evencodi { get; set; }
        public int? Evenanio { get; set; }
        public int? Evencorr { get; set; }
        public DateTime? Eveninffechemis { get; set; }
        public string Eveninfelab { get; set; }
        public string Eveninfrevs { get; set; }
        public string Eveninflastuser { get; set; }
        public DateTime? Eveninflastdate { get; set; }
        public string Eveninfemitido { get; set; }
        public DateTime? Eveninfpfechemis { get; set; }
        public string Eveninfpelab { get; set; }
        public string Eveninfprevs { get; set; }
        public DateTime? Eveninfpifechemis { get; set; }
        public string Eveninfpielab { get; set; }
        public string Eveninfpirevs { get; set; }
        public string Eveninfpemitido { get; set; }
        public string Eveninfpiemitido { get; set; }
        public string Eveninfmem { get; set; }
        public string Eveninfpiemit { get; set; }
        public string Eveninfpemit { get; set; }
        public string Eveninfemit { get; set; }
        public int? Evencorrmem { get; set; }
        public DateTime? Eveninfmemfechemis { get; set; }
        public string Eveninfmemelab { get; set; }
        public string Eveninfmemrevs { get; set; }
        public string Eveninfmememit { get; set; }
        public string Eveninfmememitido { get; set; }
        public int? EvencorrSco { get; set; }
        public string Eveninfactuacion { get; set; }
        public string Eveninfactllamado { get; set; }
        public string Eveninfactelab { get; set; }
        public DateTime? Eveninfactfecha { get; set; }
        public string Evenasunto { get; set; }

        public string Corrmem { get; set; }
        public string Emprnomb { get; set; }
        public string Tareaabrev { get; set; }
        public string Areanomb { get; set; }
        public string Famabrev { get; set; }
        public string Equiabrev { get; set; }
        public decimal? Evenmwindisp { get; set; }
        public DateTime Evenini { get; set; }
        public string Extosinerg { get; set; }
        public string Obsprelimini { get; set; }
        public string Obsprelim { get; set; }
        public string Obsfinal { get; set; }
        public string Obsmem { get; set; }

        public string Correlativo { get; set; }
        public decimal Plazo { get; set; }
        #region CTAF
        public int? Eveninfplazodiasipi { get; set; }
        public int? Eveninfplazodiasif { get; set; }
        public int? Eveninfplazohoraipi { get; set; }
        public int? Eveninfplazohoraif { get; set; }
        public int? Eveninfplazominipi { get; set; }
        public int? Eveninfplazominif { get; set; }
        #endregion
    }
}
