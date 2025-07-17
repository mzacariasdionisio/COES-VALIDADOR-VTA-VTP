using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_GENFORZADA_MAESTRO
    /// </summary>
    public class PrGenforzadaMaestroDTO : EntityBase
    {
        public int Genformaecodi { get; set; } 
        public int? Relacioncodi { get; set; } 
        public string Indestado { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public string Genformaesimbolo { get; set; }
        public string Genfortipo { get; set; }
        public string Equinomb { get; set; }
        public string Nombarra { get; set; }
        public string Idgener { get; set; }
        public int? Subcausacodi { get; set; }
        public string Subcausadesc { get; set; }
    }
}
