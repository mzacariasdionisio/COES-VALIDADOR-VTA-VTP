using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_IMPUGNACION
    /// </summary>
    public class WbImpugnacionDTO : EntityBase
    {
        public string Impgnombre { get; set; } 
        public string Impgtitulo { get; set; } 
        public int? Impgnumeromes { get; set; } 
        public int Impgcodi { get; set; } 
        public string Impgregsgdoc { get; set; } 
        public string Impginpugnante { get; set; } 
        public string Impgdescinpugnad { get; set; } 
        public string Impgpetitorio { get; set; } 
        public DateTime? Impgfechrecep { get; set; } 
        public DateTime? Impgfechpubli { get; set; } 
        public DateTime? Impgplazincorp { get; set; } 
        public string Impgincorpresent { get; set; } 
        public string Impgdescdirecc { get; set; } 
        public DateTime? Impgfechdesc { get; set; } 
        public int? Impgdiastotaten { get; set; } 
        public string Impgusuariocreacion { get; set; } 
        public string Impgrutaarch { get; set; } 
        public int Timpgcodi { get; set; } 
        public DateTime? Impgfechacreacion { get; set; } 
        public string Impgusuarioupdate { get; set; }
        public DateTime? Impgfechaupdate { get; set; }
        public DateTime? Impgmesanio { get; set; }
        public string Impgextension { get; set; } 
    }
}
