using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_UMBRALREPORTE
    /// </summary>
    public class CmUmbralreporteDTO : EntityBase
    {
        public int Cmurcodi { get; set; } 
        public decimal? Cmurminbarra { get; set; } 
        public decimal? Cmurmaxbarra { get; set; } 
        public decimal? Cmurminenergia { get; set; } 
        public decimal? Cmurmaxenergia { get; set; } 
        public decimal? Cmurminconges { get; set; } 
        public decimal? Cmurmaxconges { get; set; } 
        public decimal? Cmurdiferencia { get; set; } 
        public string Cmurestado { get; set; } 
        public DateTime? Cmurvigencia { get; set; } 
        public DateTime? Cmurexpira { get; set; } 
        public string Cmurusucreacion { get; set; } 
        public DateTime? Cmurfeccreacion { get; set; } 
        public string Cmurusumodificacion { get; set; } 
        public DateTime? Cmurfecmodificacion { get; set; }
        public string Vigencia { get; set; }
        public string Modificacion { get; set; }
        public string Expiracion { get; set; }
    }
}
