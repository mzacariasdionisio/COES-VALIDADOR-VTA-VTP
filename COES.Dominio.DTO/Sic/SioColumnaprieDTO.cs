using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SIO_COLUMNAPRIE
    /// </summary>
    public class SioColumnaprieDTO : EntityBase
    {
        public int Cpriecodi { get; set; } 
        public string Cprienombre { get; set; } 
        public string Cpriedescripcion { get; set; } 
        public int? Cprietipo { get; set; } 
        public int? Cprielong1 { get; set; } 
        public int? Cprielong2 { get; set; } 
        public int? Tpriecodi { get; set; } 
        public string Cprieusumodificacion { get; set; } 
        public DateTime? Cpriefecmodificacion { get; set; } 
        public string Cprieusucreacion { get; set; }
        public DateTime? Cpriefeccreacion { get; set; }

        public int opCrud { get; set; }
    }
}
