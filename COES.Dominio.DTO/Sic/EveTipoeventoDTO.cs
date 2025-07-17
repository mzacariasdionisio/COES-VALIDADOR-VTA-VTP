using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_TIPOEVENTO
    /// </summary>
    public class EveTipoeventoDTO : EntityBase
    {
        public int Tipoevencodi { get; set; } 
        public string Tipoevendesc { get; set; } 
        public int? Subcausacodi { get; set; } 
        public string Tipoevenabrev { get; set; } 
        public int? Cateevencodi { get; set; } 
    }
}

