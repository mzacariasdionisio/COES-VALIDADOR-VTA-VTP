using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AUD_AUDITORIAPROCESO
    /// </summary>
    public class AudAuditoriaprocesoDTO : EntityBase
    {
        public string Audipsplanificado { get; set; } 
        public string Audipactivo { get; set; } 
        public string Audiphistorico { get; set; } 
        public string Audipusucreacion { get; set; } 
        public DateTime? Audipfeccreacion { get; set; } 
        public string Audipusumodificacion { get; set; } 
        public DateTime? Audipfecmodificacion { get; set; } 
        public int Audipcodi { get; set; } 
        public int Audicodi { get; set; } 
        public int? Audppcodi { get; set; }
        public int? Proccodi { get; set; }
        public string Elemdescripcion { get; set; }

        public string Procdescripcion { get; set; }
        public int Areacodi { get; set; }
    }
}
