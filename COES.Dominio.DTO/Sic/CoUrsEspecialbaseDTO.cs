using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_URS_ESPECIALBASE
    /// </summary>
    public class CoUrsEspecialbaseDTO : EntityBase
    {
        public int Couebacodi { get; set; } 
        public string Couebausucreacion { get; set; } 
        public DateTime? Couebafeccreacion { get; set; } 
        public int? Grupocodi { get; set; } 
        public string Gruponomb { get; set; }
    }
}
