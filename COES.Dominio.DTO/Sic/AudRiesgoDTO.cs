using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_RIESGO
    /// </summary>
    public class AudRiesgoDTO : EntityBase
    {
        public string Riesactivo { get; set; } 
        public string Rieshistorico { get; set; } 
        public string Riesusucreacion { get; set; } 
        public DateTime? Riesfeccreacion { get; set; } 
        public string Riesusumodificacion { get; set; } 
        public DateTime? Riesfecmodificacion { get; set; } 
        public int Riescodi { get; set; } 
        public int? Proccodi { get; set; } 
        public int? Tabcdcodivaloracioninherente { get; set; } 
        public int? Tabcdcodivaloracionresidual { get; set; } 
        public string Riescodigo { get; set; }
        public string Riesdescripcion { get; set; }

        public string Valoracioninherente { get; set; }
        public int Areacodi { get; set; }
    }
}
