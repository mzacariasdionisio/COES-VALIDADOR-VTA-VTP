using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_SUBEVENTOS
    /// </summary>
    public class EveSubeventosDTO : EntityBase
    {
        public int Evencodi { get; set; } 
        public int Equicodi { get; set; } 
        public string Subevedescrip { get; set; } 
        public DateTime? Subevenfin { get; set; } 
        public DateTime Subevenini { get; set; }
        public string EquiAbrev { get; set; }
        public string EmprNomb { get; set; }
        public string AreaNomb { get; set; }
    }
}

