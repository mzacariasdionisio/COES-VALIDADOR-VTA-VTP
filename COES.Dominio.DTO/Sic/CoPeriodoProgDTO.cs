using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_PERIODO_PROG
    /// </summary>
    public partial class CoPeriodoProgDTO : EntityBase
    {
        public int Perprgcodi { get; set; } 
        public DateTime? Perprgvigencia { get; set; } 
        public decimal? Perprgvalor { get; set; } 
        public string Perprgestado { get; set; } 
        public string Perprgusucreacion { get; set; } 
        public DateTime? Perprgfeccreacion { get; set; } 
        public string Perprgusumodificacion { get; set; } 
        public DateTime? Perprgfecmodificacion { get; set; } 
    }

    public partial class CoPeriodoProgDTO
    {        
        public string PerprgfecmodificacionDesc { get; set; }
        public string PerprgestadoDesc { get; set; }
        public string PerprgvigenciaDesc { get; set; }
        

    }
}
