using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_EVENTO_SUMINISTRADOR
    /// </summary>
    public class ReEventoSuministradorDTO : EntityBase
    {
        public int Reevsucodi { get; set; } 
        public int? Reevprcodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public string Reevsuindcarga { get; set; } 
        public decimal? Reevsuresarcimiento { get; set; } 
        public string Reevsuestado { get; set; }
        public string Reevsuusucreacion { get; set; } 
        public DateTime? Reevsufeccreacion { get; set; } 
    }
}
