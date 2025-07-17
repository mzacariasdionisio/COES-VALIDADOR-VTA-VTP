using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_CONGESTION_GRUPO
    /// </summary>
    public class PrCongestionGrupoDTO : EntityBase
    {
        public int Congrpcodi { get; set; } 
        public int? Congescodi { get; set; } 
        public int? Grupocodi { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
