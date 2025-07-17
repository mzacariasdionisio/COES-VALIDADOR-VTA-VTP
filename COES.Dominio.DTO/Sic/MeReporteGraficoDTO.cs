using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_REPORTE_GRAFICO
    /// </summary>
    public class MeReporteGraficoDTO : EntityBase
    {
        public int Repgrcodi { get; set; } 
        public string Repgrname { get; set; } 
        public string Repgrtype { get; set; } 
        public int? Repgryaxis { get; set; } 
        public string Repgrcolor { get; set; } 
        public int Reporcodi { get; set; } 
        public int Ptomedicodi { get; set; } 
    }
}
