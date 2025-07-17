using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_RAEJECUTADA
    /// </summary>
    public class CoRaejecutadaDTO : EntityBase
    {
        public int Coraejcodi { get; set; } 
        public int? Copercodi { get; set; } 
        public int? Covercodi { get; set; } 
        public decimal? Coraejrasub { get; set; } 
        public decimal? Coraejrabaj { get; set; } 
        public DateTime? Coraejfecha { get; set; } 
        public DateTime? Coraejfecini { get; set; } 
        public DateTime? Coraejfecfin { get; set; } 
        public string Coraejusucreacion { get; set; } 
        public DateTime? Coraejfeccreacion { get; set; } 
        public int? Equicodi { get; set; } 
        public int? Grupocodi { get; set; } 
    }
}
