using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_NIVEL_TENSION
    /// </summary>
    public class ReNivelTensionDTO : EntityBase
    {
        public int Rentcodi { get; set; } 
        public string Rentabrev { get; set; } 
        public string Rentnombre { get; set; } 
        public string Rentusucreacion { get; set; } 
        public DateTime? Rentfeccreacion { get; set; } 
        public string Rentusumodificacion { get; set; } 
        public DateTime? Rentfecmodificacion { get; set; } 
    }
}
