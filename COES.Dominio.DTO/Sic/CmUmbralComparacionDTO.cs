using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_UMBRAL_COMPARACION
    /// </summary>
    public class CmUmbralComparacionDTO : EntityBase
    {
        public int Cmumcocodi { get; set; } 
        public decimal? Cmumcohopdesp { get; set; } 
        public decimal? Cmumcoemsdesp { get; set; } 
        public decimal? Cmuncodemanda { get; set; } 
        public string Cmumcousucreacion { get; set; } 
        public DateTime? Cmumcofeccreacion { get; set; } 
        public string Cmuncousumodificacion { get; set; } 
        public DateTime? Cmuncofecmodificacion { get; set; }
        public decimal? Cmumcoci { get; set; }
        public decimal? Cmumconumiter { get; set; }
        public decimal? Cmumcovarang { get; set; }

    }
}
