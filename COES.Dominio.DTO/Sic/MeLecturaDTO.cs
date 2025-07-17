using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_LECTURA
    /// </summary>
    public class MeLecturaDTO : EntityBase
    {
        public int? Lectnro { get; set; } 
        public string Lectnomb { get; set; } 
        public string Lectabrev { get; set; } 
        public int? Origlectcodi { get; set; } 
        public int Lectcodi { get; set; }
        public int? Lectperiodo { get; set; }
        public int? Areacode { get; set; }

    }
}
