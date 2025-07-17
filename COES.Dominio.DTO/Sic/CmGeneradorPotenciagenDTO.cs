using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_GENERADOR_POTENCIAGEN
    /// </summary>
    public class CmGeneradorPotenciagenDTO : EntityBase
    {
        public int Genpotcodi { get; set; } 
        public int? Relacioncodi { get; set; } 
        public int? Grupocodi { get; set; } 
        public decimal? Genpotvalor { get; set; } 
        public string Genpotusucreacion { get; set; } 
        public DateTime? Genpotfeccreacion { get; set; } 
        public string Genpotusumodificacion { get; set; } 
        public DateTime? Genpotfecmodificacion { get; set; } 
        public string Gruponomb { get; set; }
    }
}
