using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_CLASEPROG
    /// </summary>
    public class InClaseProgDTO : EntityBase
    {
        public int Claprocodi { get; set; }
        public string Clapronombre { get; set; }
        public string Claprousucreacion { get; set; }
        public DateTime? Claprofeccreacion { get; set; }
        public string Claprousumodificacion { get; set; }
        public DateTime? Claprofecmodificacion { get; set; } 
    }
}
