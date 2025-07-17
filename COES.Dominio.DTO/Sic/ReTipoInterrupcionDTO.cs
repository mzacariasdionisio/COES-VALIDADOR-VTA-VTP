using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_TIPO_INTERRUPCION
    /// </summary>
    public class ReTipoInterrupcionDTO : EntityBase
    {
        public int Retintcodi { get; set; } 
        public string Retintnombre { get; set; } 
        public string Retintestado { get; set; } 
        public string Retintusucreacion { get; set; } 
        public DateTime? Retintfeccreacion { get; set; } 
        public string Retintusumodificacion { get; set; } 
        public DateTime? Retintfecmodificacion { get; set; } 
        public string IndicadorEdicion { get; set; }
    }
}
