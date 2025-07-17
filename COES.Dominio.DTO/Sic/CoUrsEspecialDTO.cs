using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_URS_ESPECIAL
    /// </summary>
    public class CoUrsEspecialDTO : EntityBase
    {
        public int Courescodi { get; set; } 
        public int? Copercodi { get; set; } 
        public int? Covercodi { get; set; } 
        public int? Grupocodi { get; set; } 
        public int? Couebacodi { get; set; } 
        public string Couresusucreacion { get; set; } 
        public DateTime? Couresfeccreacion { get; set; } 
        public string Gruponomb { get; set; }
        public string Centralnomb { get; set; }
    }
}
