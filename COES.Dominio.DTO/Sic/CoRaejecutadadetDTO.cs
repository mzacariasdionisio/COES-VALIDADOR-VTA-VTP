using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_RAEJECUTADADET
    /// </summary>
    public class CoRaejecutadadetDTO : EntityBase
    {
        public int Coradecodi { get; set; } 
        public DateTime? Coradefecha { get; set; } 
        public int? Coradeindice { get; set; } 
        public int? Corademinutos { get; set; } 
        public decimal? Coraderasub { get; set; } 
        public decimal? Coraderabaj { get; set; } 
        public int? Grupocodi { get; set; } 
        public int? Copercodi { get; set; } 
        public int? Covercodi { get; set; } 
        public string Gruponomb { get; set; }
        public string Bloquehorario { get; set; }
    }
}
