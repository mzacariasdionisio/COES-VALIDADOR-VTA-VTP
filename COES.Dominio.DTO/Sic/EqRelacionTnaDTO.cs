using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_RELACION_TNA
    /// </summary>
    public class EqRelacionTnaDTO : EntityBase
    {
        public int Reltnacodi { get; set; } 
        public int Relacioncodi { get; set; } 
        public string Reltnanombre { get; set; } 
        public string Reltnaestado { get; set; } 
        public string Reltnausucreacion { get; set; } 
        public DateTime? Reltnafeccreacion { get; set; } 
        public string Reltnausumodificacion { get; set; } 
        public DateTime? Reltnafecmodificacion { get; set; } 
    }
}
