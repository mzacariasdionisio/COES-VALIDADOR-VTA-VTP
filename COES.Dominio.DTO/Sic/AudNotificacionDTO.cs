using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_NOTIFICACION
    /// </summary>
    public class AudNotificacionDTO : EntityBase
    {
        public int Noticodi { get; set; } 
        public int? Progacodi { get; set; } 
        public int? Archcodiarchivoadjunto { get; set; } 
        public int Tabcdcoditiponotificacion { get; set; } 
        public string Notimensaje { get; set; } 
        public string Notiactivo { get; set; } 
        public string Notihistorico { get; set; } 
        public string Notiusuregistro { get; set; } 
        public DateTime? Notifecregistro { get; set; } 
        public string Notiusumodificacion { get; set; } 
        public DateTime? Notifecmodificacion { get; set; }

        public List<int> Para { get; set; }
        public List<int> Copia { get; set; }
    }
}
