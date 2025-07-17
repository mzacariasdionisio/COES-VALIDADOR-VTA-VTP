using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EMS_GENERACION
    /// </summary>
    public class EmsGeneracionDTO : EntityBase
    {
        public int Emggencodi { get; set; } 
        public DateTime? Emsgenfecha { get; set; } 
        public int? Equicodi { get; set; } 
        public string Emsgenoperativo { get; set; } 
        public decimal? Emsgenvalor { get; set; } 
        public string Emsgenusucreacion { get; set; } 
        public DateTime? Emsgenfeccreacion { get; set; } 
        public int Grupocodi { get; set; }
        public int Indice { get; set; }
        public decimal? Emsgenpotmax { get; set; }
        public int Tgenercodi { get; set; }
    }
}
