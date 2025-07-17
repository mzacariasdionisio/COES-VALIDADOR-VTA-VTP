using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_CALENDARIO
    /// </summary>
    public class WbCalendarioDTO : EntityBase
    {
        public string Calendicon { get; set; } 
        public string Calendestado { get; set; } 
        public string Calendusumodificacion { get; set; } 
        public DateTime? Calendfecmodificacion { get; set; } 
        public int Calendcodi { get; set; } 
        public string Calenddescripcion { get; set; } 
        public string Calendtitulo { get; set; } 
        public DateTime? Calendfecinicio { get; set; } 
        public DateTime? Calendfecfin { get; set; }
        public string Calendcolor { get; set; }
        public string Calendtipo { get; set; }
        public int? Tipcalcodi { get; set; }
    }
}
