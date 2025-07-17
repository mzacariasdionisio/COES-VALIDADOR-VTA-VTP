using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_PERIODO
    /// </summary>
    public class RePeriodoDTO : EntityBase
    {
        public int Repercodi { get; set; } 
        public int? Reperanio { get; set; } 
        public string Repertipo { get; set; } 
        public string Repernombre { get; set; } 
        public int? Reperpadre { get; set; } 
        public string Reperrevision { get; set; } 
        public DateTime? Reperfecinicio { get; set; } 
        public DateTime? Reperfecfin { get; set; } 
        public string Reperestado { get; set; } 
        public int? Reperorden { get; set; } 
        public decimal? Repertcambio { get; set; } 
        public decimal? Reperfactorcomp { get; set; } 
        public string Reperusucreacion { get; set; } 
        public DateTime? Reperfeccreacion { get; set; } 
        public string Reperusumodificacion { get; set; } 
        public DateTime? Reperfecmodificacion { get; set; } 

        public string Repernombrepadre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Data { get; set; }
    }
}
