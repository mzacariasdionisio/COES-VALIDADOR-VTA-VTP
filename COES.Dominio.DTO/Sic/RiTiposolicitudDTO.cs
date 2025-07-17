using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RI_TIPOSOLICITUD
    /// </summary>
    public class RiTiposolicitudDTO : EntityBase
    {
        public int Tisocodi { get; set; } 
        public string Tisonombre { get; set; } 
        public string Tisoestado { get; set; } 
        public string Tisousucreacion { get; set; } 
        public DateTime? Tisofeccreacion { get; set; } 
        public string Tisousumodificacion { get; set; } 
        public DateTime? Tisofecmodificacion { get; set; } 
    }
}
