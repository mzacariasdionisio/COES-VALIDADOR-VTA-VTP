using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_NOTIFICACIONDEST
    /// </summary>
    public class AudNotificaciondestDTO : EntityBase
    {
        public int Notdcodi { get; set; } 
        public int Noticodi { get; set; } 
        public int Percodidestinatario { get; set; } 
        public int Tabcdcoditipodestinatario { get; set; } 
        public string Notdactivo { get; set; } 
        public string Notdhistorico { get; set; } 
        public string Notdusucreacion { get; set; } 
        public DateTime? Notdfeccreacion { get; set; } 
        public string Notdusumodificacion { get; set; } 
        public DateTime? Notdfecmodificacion { get; set; } 
    }
}
