using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_ARCHIVO
    /// </summary>
    public class MeArchivoDTO : EntityBase
    {
        public int Archcodi { get; set; } 
        public int? Formatcodi { get; set; } 
        public decimal? Archsize { get; set; } 
        public string Archnomborig { get; set; } 
        public string Archnombpatron { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
