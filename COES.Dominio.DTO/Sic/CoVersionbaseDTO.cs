using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_VERSIONBASE
    /// </summary>
    public class CoVersionbaseDTO : EntityBase
    {
        public int Covebacodi { get; set; } 
        public string Covebadesc { get; set; } 
        public string Covebatipo { get; set; } 
        public int? Covebadiainicio { get; set; } 
        public int? Covebadiafin { get; set; } 
        public string Covebausucreacion { get; set; } 
        public DateTime? Covebafeccreacion { get; set; } 
    }
}
