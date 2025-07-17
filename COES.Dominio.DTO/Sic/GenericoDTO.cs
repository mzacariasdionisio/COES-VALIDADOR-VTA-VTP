using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea una tabla generica
    /// </summary>
    public class GenericoDTO : EntityBase
    {
        public DateTime? ValorFecha1 { get; set; }
        public DateTime? ValorFecha2 { get; set; }  
        public int? Entero1 { get; set; }
        public int? Entero2 { get; set; }
        public int? Entero3 { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; } 
        public bool Selected1 { get; set; }
        public decimal? Decimal1 { get; set; }
        public decimal? Decimal2 { get; set; }
    }
}