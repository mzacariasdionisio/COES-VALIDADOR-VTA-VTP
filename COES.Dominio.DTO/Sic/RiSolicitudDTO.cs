using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RI_SOLICITUD
    /// </summary>
    public class RiSolicitudDTO : EntityBase
    {
        public int Solicodi { get; set; }
        public string Soliestado { get; set; }
        public string SoliestadoInterno { get; set; }
        public string Solienviado { get; set; }
        public DateTime? Solifecenviado { get; set; }
        public int? Emprcodi { get; set; }
        public DateTime? Solifecsolicitud { get; set; }
        public string Soliusucreacion { get; set; }
        public DateTime? Solifeccreacion { get; set; }
        public string Soliusumodificacion { get; set; }
        public DateTime? Solifecmodificacion { get; set; }
        public int? Tisocodi { get; set; }
        public DateTime? Solifecproceso { get; set; }
        public int? Soliususolicitud { get; set; }
        public int? Soliusuproceso { get; set; }
        public string Soliobservacion { get; set; }

        public string Solinotificado { get; set; }
        public DateTime? Solifecnotificado { get; set; }

        public string Tisonombre { get; set; }
        public string Emprrazsocial { get; set; }
        public string EmprnombComercial { get; set; }
        public string Emprsigla { get; set; }
        public int HorasTranscurridas { get; set; }
    }
}
