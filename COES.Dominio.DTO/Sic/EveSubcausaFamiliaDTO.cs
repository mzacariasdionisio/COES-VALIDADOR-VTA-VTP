using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_SUBCAUSA_FAMILIA
    /// </summary>
    public class EveSubcausaFamiliaDTO : EntityBase
    {
        public int Scaufacodi { get; set; } 
        public int? Subcausacodi { get; set; } 
        public int Famcodi { get; set; } 
        public string Scaufaeliminado { get; set; } 
        public string Scaufausucreacion { get; set; } 
        public DateTime? Scaufafeccreacion { get; set; } 
        public string Scaufausumodificacion { get; set; } 
        public DateTime? Scaufafecmodificacion { get; set; } 
        public string Subcausadesc { get; set; }
        public string Famabrev { get; set; }
        public string Famnomb { get; set; }
    }
}
